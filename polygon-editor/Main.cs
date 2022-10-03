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
        private const int RADIUS = 13;
        private Pen pen;

        public Main()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            pen = new Pen(Color.Black, 3);
            
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            using(Graphics g = Graphics.FromImage(drawArea))
            {
                g.FillEllipse(new SolidBrush(Color.IndianRed), e.X, e.Y, RADIUS, RADIUS);
            }

            Canvas.Image = drawArea;
            Canvas.Refresh();
        }
    }
}
