using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace polygon_editor
{
    public interface IConstraint
    {
        public void DrawSymbol(Graphics g);
    }

    public class EdgeLengthConstraint : IConstraint
    {
        // Class descibes an edge length constraint; We assume that `end` vertex has to be adjusted

        private Vertex start;
        private Vertex end;
        private double length;

        public EdgeLengthConstraint(Vertex start, Vertex end, double length)
        {
            this.start = start;
            this.end = end;
            this.length = length;
        }

        public Vertex Start { get => start; set { start = value; } }
        public Vertex End { get => end; set { end = value; } }
        public double Length { get => length; set { length = value; } }

        public void ApplyConstraint(bool swapVertices)
        {
            double oldLength = Main.Distance(start.Point, end.Point);
            var ratio = length / oldLength;

            if(swapVertices)
            {
                var vector = (start.Point.X - end.Point.X, start.Point.Y - end.Point.Y);
                vector = ((int)(ratio * vector.Item1), (int)(ratio * vector.Item2));

                start.Point = new Point(end.Point.X + vector.Item1, end.Point.Y + vector.Item2);
            }
            else
            {
                var vector = (end.Point.X - start.Point.X, end.Point.Y - start.Point.Y);
                vector = ((int)(ratio * vector.Item1), (int)(ratio * vector.Item2));

                end.Point = new Point(start.Point.X + vector.Item1, start.Point.Y + vector.Item2);
            }

        }

        public bool ContainsBoth(List<Vertex> vertices)
        {
            return vertices.Contains(start) && vertices.Contains(end);
        }

        public void DrawSymbol(Graphics g)
        {
            int offset = 50;
            Point location = new((start.Point.X + end.Point.X) / 2 + offset, (start.Point.Y + end.Point.Y) / 2);

            Font f = new("Loboto", 14);
            StringFormat sf = new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(length.ToString(), f, new SolidBrush(Color.Black), location, sf);
        }
    }

    public class ParallelConstraint : IConstraint
    {
        private List<(Vertex u, Vertex v)> edges;
        private int id;
        private bool extendable;

        public ParallelConstraint(List<(Vertex u, Vertex v)> _edges, int _id)
        {
            edges = _edges;
            id = _id;
            extendable = true;
        }

        public List<(Vertex u, Vertex v)> Edges { get => edges; set => edges = value; }
        public bool Extendable { get => extendable; set => extendable = value; }

        public void DrawSymbol(Graphics g)
        {
            int radius = 12;

            foreach (var (u, v) in edges)
            {
                Point midpoint = new((u.Point.X + v.Point.X) / 2, (u.Point.Y + v.Point.Y) / 2);

                g.FillEllipse(new SolidBrush(Color.FromArgb(133, 219, 166)), midpoint.X - radius, midpoint.Y - radius, 2 * radius, 2 * radius);
                g.DrawEllipse(new Pen(Color.Black, 3), midpoint.X - radius, midpoint.Y - radius, 2 * radius, 2 * radius);

                Font f = new("Loboto", 12);
                StringFormat sf = new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(id.ToString(), f, new SolidBrush(Color.Black), midpoint, sf);
            }
        }

        public (bool, int) ContainsVertex(Vertex vert)
        {
            foreach(var (u, v) in edges)
            {
                if (u == vert)
                    return (true, 0);
                if (v == vert)
                    return (true, 1);
            }
            return (false, 0);
        }

        public void ApplyConstraint(bool adjustLength, bool swapVertices)
        {
            if (edges.Count < 2)
                return;

            var modelEdge = edges[0];
            double tan = (double)(modelEdge.v.Point.Y - modelEdge.u.Point.Y) / (modelEdge.v.Point.X - modelEdge.u.Point.X);

            for (int i = 1; i < edges.Count; ++i)
            {
                int a = /* swapVertices ? edges[i].v.Point.X : */ edges[i].u.Point.X;
                int b = /* swapVertices ? edges[i].v.Point.Y : */ edges[i].u.Point.Y;
                Vertex u = edges[i].u;
                Vertex v = edges[i].v;

                int newX, newY;

                if(adjustLength)
                {
                    double d = Main.Distance(u.Point, v.Point);
                    double sq = d * Math.Sqrt(tan * tan + 1);

                    newX = (int)((a * tan * tan + a - sq) / (tan * tan + 1));
                    newY = (int)((b * tan * tan + b - tan * sq) / (tan * tan + 1));
                }
                else
                {
                    newX = /*swapVertices ? u.Point.X :*/ v.Point.X;
                    newY = (int)(tan * (/*swapVertices ? u.Point.X :*/ v.Point.X - a)) + b;
                }

                if (newX > 1073740288 || newX < -1073740288 || newY > 1073740288 || newY < -1073740288)
                    return;

                if (swapVertices)
                    v.Point = new Point(newX, newY);
                else
                    v.Point = new Point(newX, newY);
            }
        }
    }
}
