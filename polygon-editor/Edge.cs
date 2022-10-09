using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public class Edge
    {
        public static double EPSILON = 5.0;
        public static void DrawEdge(Point v1, Point v2, Graphics g, Pen p)
        {
            g.DrawLine(p, v1, v2);
        }

    }
}
