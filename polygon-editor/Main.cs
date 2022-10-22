﻿using System;
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

        private bool isPolyOpen;
        private List<EdgeLengthConstraint> edgeLengthConstraints;
        private List<ParallelConstraint> parallelConstraints;
        private bool extendParallel;
        private Vertex? movingVertex;

        public Main()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            polygons = new();
            edgeLengthConstraints = new();
            parallelConstraints = new();
            movingVertex = null;
            isPolyOpen = false;
            extendParallel = false;
            FillScene();
            RepaintCanvas();
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public void FillScene()
        {
            var v1 = new Vertex(new Point(200, 400), false);
            var v2 = new Vertex(new Point(250, 100), false);

            Polygon poly1 = new();
            poly1.AddVertex(new Vertex(new Point(100, 200), false));
            poly1.AddVertex(v1);
            poly1.AddVertex(v2);
            poly1.IsFinished = true;

            var c = new EdgeLengthConstraint(v1, v2, 210);
            edgeLengthConstraints.Add(c);
            c.ApplyConstraint(false);

            var v3 = new Vertex(new Point(500, 400), false);
            var v4 = new Vertex(new Point(700, 400), false);
            var v6 = new Vertex(new Point(400, 300), false);
            var v5 = new Vertex(new Point(750, 300), false);

            Polygon poly2 = new();
            poly2.AddVertex(v3);
            poly2.AddVertex(v4);
            poly2.AddVertex(v5);
            poly2.AddVertex(v6);
            poly2.IsFinished = true;

            var pc = new ParallelConstraint(new List<(Vertex, Vertex)>() { (v3, v4) }, 1);
            pc.Edges.Add((v5, v6));
            parallelConstraints.Add(pc);
            //pc.ApplyConstraint(true, false);

            polygons.Add(poly1);
            polygons.Add(poly2);
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

        private void MovePointOnEdge(Vertex start, Vertex end, double newLength)
        {
            double oldLength = Distance(start.Point, end.Point);
            var ratio = newLength / oldLength;

            var vector = (end.Point.X - start.Point.X, end.Point.Y - start.Point.Y);
            vector = ((int)(ratio * vector.Item1), (int)(ratio * vector.Item2));

            end.Point = new Point(start.Point.X + vector.Item1, start.Point.Y + vector.Item2);
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

        private double showEdgeLengthDialog(Vertex v1, Vertex v2)
        {
            var response = PopupDialog.ShowDialog("New edge length", "Set fixed length for the edge", 
                Math.Round(Distance(v1.Point, v2.Point), 1));

            if (response == null)
                return -1;
            response.Replace(",", ".");

            return double.Parse(response);
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

                foreach (var constraint in edgeLengthConstraints)
                    constraint.DrawSymbol(g);
                foreach (var constraint in parallelConstraints)
                    constraint.DrawSymbol(g);

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
                        movingVertex = w;
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

                // Constraints mode
                else if (radioButtonConstraints.Checked)
                {
                    var edge = SnapEdge(e.Location);
                    if (edge == null)
                        return;

                    double newLength = showEdgeLengthDialog(edge.Value.start, edge.Value.end);

                    if (newLength == -1)
                        return;

                    edgeLengthConstraints.RemoveAll
                        (constraint => constraint.ContainsBoth(new List<Vertex>() { edge.Value.start, edge.Value.end }));

                    var c = new EdgeLengthConstraint(edge.Value.start, edge.Value.end, newLength);
                    edgeLengthConstraints.Add(c);
                    edgeLengthConstraints.Add(c);
                    c.ApplyConstraint(movingVertex != null && movingVertex == c.End);
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

                        edgeLengthConstraints.RemoveAll(c => c.Start == w || c.End == w);
                        parallelConstraints.RemoveAll(c => c.ContainsVertex(w).Item1);
                    }
                }

                // Constraints mode
                else if (radioButtonConstraints.Checked)
                {
                    var edge = SnapEdge(e.Location);
                    if (edge == null)
                        return;
                    
                    foreach(var constraint in parallelConstraints)
                    {
                        if (constraint.Edges.Contains((edge.Value.start, edge.Value.end)))
                        {
                            extendParallel = true;
                            constraint.Extendable = true;
                            return;
                        }
                    }

                    if(!extendParallel)
                    {
                        var c = new ParallelConstraint(new List<(Vertex u, Vertex v)>()
                        { (edge.Value.start, edge.Value.end) }, parallelConstraints.Count + 1);
                        parallelConstraints.Add(c);
                        c.ApplyConstraint(true, false);
                        extendParallel = true;
                    }
                    else
                    {
                        foreach(var constraint in parallelConstraints)
                        {
                            if(constraint.Extendable)
                            {
                                constraint.Extendable = false;
                                constraint.Edges.Add((edge.Value.start, edge.Value.end));
                                constraint.ApplyConstraint(true, false);
                                extendParallel = false;
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
            // Mock edge tracking
            if (radioButtonAdding.Checked && isPolyOpen == false) return;

            bool found = false;
            foreach(var poly in polygons)
            {
                foreach(var v in poly.Vertices)
                {
                    if (v.Selected)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (!found && isPolyOpen == false)
                return;

            //foreach(var poly in polygons)
            //{
            //    if(poly.IsFullyConstrained(edgeLengthConstraints))
            //    {
            //        cursorPos = e.Location;
            //        MoveVertices();

            //        previousCursorPos = cursorPos;

            //        RepaintCanvas();
            //        return;
            //    }
            //}

            foreach(var constraint in edgeLengthConstraints)
            {
                constraint.ApplyConstraint(movingVertex != null && movingVertex == constraint.End);
            }

            foreach(var constraint in parallelConstraints)
            {
                constraint.ApplyConstraint(false, movingVertex != null && constraint.ContainsVertex(movingVertex).Item2 == 1);
            }
            

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
            movingVertex = null;
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

                            edgeLengthConstraints.RemoveAll(constraint => constraint.ContainsBoth(new List<Vertex>() { u, v }));
                            parallelConstraints.RemoveAll(constraint => constraint.Edges.Contains((u, v)));

                            break;
                        }
                    }
                }
            }
        }

        private void radioButtonSystemAlgo_CheckedChanged(object sender, EventArgs e)
        {
            RepaintCanvas();
        }

        private void clearConstraintsBtn_Click(object sender, EventArgs e)
        {
            edgeLengthConstraints.Clear();
            parallelConstraints.Clear();
            RepaintCanvas();
        }
    }
}
