using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace polygon_editor
{
    public class Polygon
    {
        public static int EDGE_WIDTH = 3;
        public static Pen POLY_PEN = new Pen(new SolidBrush(Color.Black), EDGE_WIDTH);

        private List<Vertex> vertices;
        private bool isFinished;

        public Polygon()
        {
            vertices = new List<Vertex>();
            isFinished = true;
        }

        public List<Vertex> Vertices { get => vertices; set => vertices = value; }
        public bool IsFinished { get => isFinished; set => isFinished = value; }

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
        }

        public void Draw(Graphics g, bool useSystemDrawing, bool useBresenhamDrawing, int lineWidth)
        {
            POLY_PEN.Width = lineWidth;
            for (int i=0; i < vertices.Count; i++)
            {
                vertices[i].Draw(g);
                if (i < vertices.Count -1 || isFinished)
                {
                    if (useSystemDrawing)
                        Edge.DrawEdge(vertices[i].Point, vertices[(i + 1) % vertices.Count].Point, g, POLY_PEN);
                    else if(useBresenhamDrawing)
                        Edge.DrawEdgeBresenham(vertices[i].Point, vertices[(i + 1) % vertices.Count].Point, g, POLY_PEN);
                    else
                        Edge.DrawEdgeWu(vertices[i].Point, vertices[(i + 1) % vertices.Count].Point, g, POLY_PEN);
                }
                    
            }
        }

        public (bool, List<EdgeLengthConstraint>) IsFullyConstrained(List<EdgeLengthConstraint> constraints)
        {
            List<EdgeLengthConstraint> con = new List<EdgeLengthConstraint>();

            for (int i = 0; i < vertices.Count; i++)
            {
                var u = vertices[i];
                var v = vertices[(i + 1) % vertices.Count];
                bool found = false;

                foreach (var constraint in constraints)
                {
                    if (constraint.ContainsBoth(new List<Vertex>() { u, v }))
                    {
                        found = true;
                        con.Add(constraint);
                        break;
                    }
                }

                if (!found)
                    return (false, con);
            }

            return (true, con);
        }

    }
}
