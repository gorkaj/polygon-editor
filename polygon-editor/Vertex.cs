using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public class Vertex
    {
        private Point point;
        private bool selected;

        private const int RADIUS = 13;

        public Vertex(Point point, bool selected)
        {
            this.point = point;
            this.selected = selected;
        }

        public Point Point { get; set; }
        public bool Selected { get; set; }

        public void paintVertex(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.IndianRed), point.X, point.Y, RADIUS, RADIUS);
        }
    }
}
