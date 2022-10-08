using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace polygon_editor
{
    public partial class Main : Form
    {
        private Bitmap drawArea;
        private List<Polygon> polygons;
        private Point cursorPos;

        // flags
        private bool isPolyOpen;

        public Main()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            polygons = new List<Polygon>();

            isPolyOpen = false;
        }
        private Vertex SnapVertex(Point pt)
        {
            foreach (Polygon polygon in polygons)
            {
                foreach (Vertex v in polygon.Vertices)
                {
                    double dist = Math.Sqrt((pt.X - v.Point.X) * (pt.X - v.Point.X) + (pt.Y - v.Point.Y) * (pt.Y - v.Point.Y));
                    if (dist <= Vertex.RADIUS)
                        return v;
                }
            }
            return null;
        }

        private void RepaintCanvas()
        {
            Size newSize = tableLayout.GetControlFromPosition(0, 0).Size;
            drawArea.Dispose();
            drawArea = new Bitmap(newSize.Width, newSize.Height);

            using (Graphics g = Graphics.FromImage(drawArea))
            {
                foreach(Polygon polygon in polygons)
                {
                    polygon.Draw(g);
                }

                if (!polygons[^1].IsFinished)
                    Edge.DrawEdge(polygons[^1].Vertices[^1].Point, cursorPos, g, Polygon.POLY_PEN);
            }
            
            Canvas.Image = drawArea;
            Canvas.Refresh();
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var v = new Vertex(e.Location, false);
            var w = SnapVertex(v.Point);

            if (!isPolyOpen)
            {
                Polygon poly = new();
                poly.IsFinished = false;
                poly.AddVertex(v);
                this.polygons.Add(poly);
                isPolyOpen = true;
            }
            else
            {
                if (w == polygons[^1].Vertices[0])
                {
                    polygons[^1].IsFinished = true;
                    isPolyOpen = false;
                }
                else
                {
                    polygons[^1].AddVertex(v);
                }

            }

            RepaintCanvas();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(isPolyOpen == false) return;

            cursorPos = e.Location;
            RepaintCanvas();
        }
    }
}
