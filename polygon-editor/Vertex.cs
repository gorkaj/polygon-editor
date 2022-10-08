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

        public const int RADIUS = 6;

        public Vertex(Point point, bool selected)
        {
            this.point = point;
            this.selected = selected;
        }

        public Point Point { get => point; set => point = value; }
        public bool Selected { get => selected; set => selected = value; }

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.IndianRed), point.X - RADIUS, point.Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }
    }
}
