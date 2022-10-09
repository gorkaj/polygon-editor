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
        private Point previousCursorPos;

        // flags
        private bool isPolyOpen;

        public Main()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            polygons = new List<Polygon>();

            isPolyOpen = false;
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private Vertex? SnapVertex(Point pt)
        {
            foreach (Polygon polygon in polygons)
            {
                foreach (Vertex v in polygon.Vertices)
                {
                    double dist = Distance(pt, v.Point);
                    if (dist <= 2 * Vertex.RADIUS)
                        return v;
                }
            }
            return null;
        }

        private (Vertex start, Vertex end)? SnapEdge(Point pt)
        {
            foreach (Polygon polygon in polygons)
            {
                for (int i=0; i<polygon.Vertices.Count; i++)
                {
                    double distAB = Distance(polygon.Vertices[i].Point, polygon.Vertices[(i + 1)%polygon.Vertices.Count].Point);
                    double distAC = Distance(polygon.Vertices[i].Point, pt);
                    double distBC = Distance(pt, polygon.Vertices[(i + 1) % polygon.Vertices.Count].Point);

                    if (Math.Abs(distAB - distAC - distBC) < Edge.EPSILON)
                        return (polygon.Vertices[i], polygon.Vertices[(i + 1) % polygon.Vertices.Count]);
                }
            }
            return null;
        }

        private Polygon? SnapPolygon(Point pt)
        {
            foreach (Polygon polygon in polygons)
            {
                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;
                foreach (Vertex vertex in polygon.Vertices)
                {
                    minX = Math.Min(minX, vertex.Point.X);
                    minY = Math.Min(minY, vertex.Point.Y);
                    maxX = Math.Max(maxX, vertex.Point.X);
                    maxY = Math.Max(maxY, vertex.Point.Y);

                    if (pt.X < maxX && pt.X > minX && pt.Y > minY && pt.Y < maxY)
                        return polygon;
                }
            }
            return null;
        }

        private void DeleteOpenPolygon()
        {
            polygons.RemoveAt(polygons.Count - 1);
            isPolyOpen = false;
        }

        private void MoveVertices()
        {
            int dx = cursorPos.X - previousCursorPos.X;
            int dy = cursorPos.Y - previousCursorPos.Y;
            foreach(Polygon polygon in polygons)
            {
                foreach(Vertex v in polygon.Vertices)
                {
                    if(v.Selected)
                    {
                        UpdateVertexCoordinates(v, dx, dy);
                    }
                }
            }
        }

        private void UpdateVertexCoordinates(Vertex v, int dx, int dy)
        {
            (int, int) oldPos = (v.Point.X, v.Point.Y);
            (int, int) newPos = (oldPos.Item1 += dx, oldPos.Item2 += dy);
            v.Point = new Point(newPos.Item1, newPos.Item2);
        }

        private Point MidPoint(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
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
                    polygon.Draw(g, radioButtonSystemAlgo.Checked);
                }

                if (polygons.Count > 0 && !polygons[^1].IsFinished && radioButtonAdding.Checked)
                {
                    if (radioButtonSystemAlgo.Checked)
                        Edge.DrawEdge(polygons[^1].Vertices[^1].Point, cursorPos, g, Polygon.POLY_PEN);
                    else
                        Edge.DrawEdgeBresenham(polygons[^1].Vertices[^1].Point, cursorPos, g, Polygon.POLY_PEN);
                }
            }
            
            Canvas.Image = drawArea;
            Canvas.Refresh();
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Left Mouse Button
            if (e.Button == MouseButtons.Left)
            {
                var w = SnapVertex(e.Location);

                // Creation mode
                if (radioButtonAdding.Checked)
                {
                    var v = new Vertex(e.Location, false);

                    if (polygons.Count > 0 && w != null && w != polygons[^1].Vertices[0])
                        return;

                    if (!isPolyOpen)
                    {
                        Polygon poly = new() { IsFinished = false };
                        poly.AddVertex(v);
                        this.polygons.Add(poly);
                        isPolyOpen = true;
                    }
                    else
                    {
                        if (w == polygons[^1].Vertices[0])
                        {
                            if(polygons[^1].Vertices.Count > 2)
                            {
                                polygons[^1].IsFinished = true;
                                isPolyOpen = false;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            polygons[^1].AddVertex(v);
                        }

                    }
                }

                // Editing mode
                else if (radioButtonEditing.Checked)
                {
                    var edge = w == null ? SnapEdge(e.Location) : null;
                    var poly = (w == null && edge == null) ? SnapPolygon(e.Location) : null;

                    if (w != null)
                    {
                        previousCursorPos = e.Location;
                        w.Selected = true;
                    }
                    if (edge != null)
                    {
                        previousCursorPos = e.Location;
                        edge.Value.start.Selected = true;
                        edge.Value.end.Selected = true;
                    }
                    if (poly != null)
                    {
                        previousCursorPos = e.Location;
                        foreach (Vertex v in poly.Vertices)
                            v.Selected = true;
                    }

                }
            }
            //Right Mouse Button
            if (e.Button == MouseButtons.Right)
            {
                if (radioButtonEditing.Checked)
                {
                    var w = SnapVertex(e.Location);
                    if (w != null)
                    {
                        foreach(var poly in polygons)
                        {
                            if(poly.Vertices.Contains(w))
                            {
                                poly.Vertices.Remove(w);
                                if(poly.Vertices.Count <= 2)
                                    polygons.Remove(poly);
                                break;
                            }
                        }
                    }
                }
            }


            RepaintCanvas();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;
            // Mock edge tracking
            if (radioButtonAdding.Checked && isPolyOpen == false) return;

            cursorPos = e.Location;
            MoveVertices();
            previousCursorPos = cursorPos;

            RepaintCanvas();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && radioButtonAdding.Checked && isPolyOpen)
            {
                DeleteOpenPolygon();
            }

            RepaintCanvas();
        }

        private void radioButtonEditing_CheckedChanged(object sender, EventArgs e)
        {
            if (isPolyOpen)
            {
                DeleteOpenPolygon();
            }
            RepaintCanvas();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Polygon polygon in polygons)
            {
                foreach (Vertex v in polygon.Vertices)
                {
                    v.Selected = false;
                }
            }
        }

        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(radioButtonEditing.Checked)
            {
                var edge = SnapEdge(e.Location);

                if (edge == null) return;
                var u = edge.Value.start;
                var v = edge.Value.end;
                
                foreach(var poly in polygons)
                {
                    for(int i = 0; i < poly.Vertices.Count; i++)
                    {
                        if(poly.Vertices[i] == u)
                        {
                            var vert = new Vertex(MidPoint(u.Point, v.Point), false);
                            poly.Vertices.Insert(i + 1, vert);
                            break;
                        }
                    }
                }
            }
        }
    }
}
