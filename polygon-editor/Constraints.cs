using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public class EdgeLengthConstraint
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

    }
}
