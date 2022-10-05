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
        private List<Vertex> vertices;

        public Main()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            vertices = new List<Vertex>();
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var v = new Vertex(e.Location, false);
            vertices.Add(v);

            using (Graphics g = Graphics.FromImage(drawArea))
            {
                v.paintVertex(g);
            }

            Canvas.Image = drawArea;
            Canvas.Refresh();
        }
    }
}
