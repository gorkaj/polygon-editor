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
        public static double EPSILON = 1.0;
        public static void DrawEdge(Point v1, Point v2, Graphics g, Pen p)
        {
            try
            {
                p.Color = Color.Black;
                g.DrawLine(p, v1, v2);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void DrawEdgeBresenham(Point v1, Point v2, Graphics g, Pen p)
        {
            p.Color = Color.Black;

            int x = v1.X;
            int y = v1.Y;
            int x2 = v2.X;
            int y2 = v2.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                g.FillRectangle(p.Brush, x, y, 1, 1);
                for(int j=1;j<p.Width;++j)
                {
                    g.FillRectangle(p.Brush, x + j * dy1, y + j * dx1, 1, 1);
                    g.FillRectangle(p.Brush, x + j * dy2, y + j * dx2, 1, 1);
                }
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private static int ipart(double x) { return (int)x; }

        private static int round(double x) { return ipart(x + 0.5); }

        private static double fpart(double x)
        {
            if (x < 0) return (1 - (x - Math.Floor(x)));
            return (x - Math.Floor(x));
        }

        private static double rfpart(double x)
        {
            return 1 - fpart(x);
        }

        private static void plot(Pen p, Graphics g, double x, double y, double c)
        {
            int alpha = (int)(c * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Color color = Color.FromArgb(alpha, Color.Black);
            p.Color = color;
            double limit = 1e4;
            if (x > limit || x < -limit || y > limit || y < -limit)
                return;
            g.FillRectangle(p.Brush, (int)x, (int)y, 3, 3);
        }

        public static void DrawEdgeWu(Point v1, Point v2, Graphics g, Pen p)
        {
            int x0 = v1.X;
            int y0 = v1.Y;
            int x1 = v2.X;
            int y1 = v2.Y;

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            int temp;
            if (steep)
            {
                temp = x0; x0 = y0; y0 = temp;
                temp = x1; x1 = y1; y1 = temp;
            }
            if (x0 > x1)
            {
                temp = x0; x0 = x1; x1 = temp;
                temp = y0; y0 = y1; y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = rfpart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = ipart(yEnd);

            if (steep)
            {
                plot(p, g, yPixel1, xPixel1, rfpart(yEnd) * xGap);
                plot(p, g, yPixel1 + p.Width, xPixel1, fpart(yEnd) * xGap);
            }
            else
            {
                plot(p, g, xPixel1, yPixel1, rfpart(yEnd) * xGap);
                plot(p, g, xPixel1, yPixel1 + 1, fpart(yEnd) * xGap);
            }
            double intery = yEnd + gradient;

            xEnd = round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = fpart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = ipart(yEnd);
            if (steep)
            {
                plot(p, g, yPixel2, xPixel2, rfpart(yEnd) * xGap);
                plot(p, g, yPixel2 + 1, xPixel2, fpart(yEnd) * xGap);
            }
            else
            {
                plot(p, g, xPixel2, yPixel2, rfpart(yEnd) * xGap);
                plot(p, g, xPixel2, yPixel2 + 1, fpart(yEnd) * xGap);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(p, g, ipart(intery), x, rfpart(intery));
                    for (int k = 1; k < p.Width; ++k)
                        plot(p, g, ipart(intery) + k, x, rfpart(intery));
                    plot(p, g, ipart(intery) + p.Width, x, fpart(intery));
                    intery += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(p, g, x, ipart(intery), rfpart(intery));
                    for (int k = 1; k < p.Width; ++k)
                        plot(p, g, x, ipart(intery) + k, rfpart(intery));
                    plot(p, g, x, ipart(intery) + p.Width, fpart(intery));
                    intery += gradient;
                }
            }
        }
    }

}
