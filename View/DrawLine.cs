using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.View
{
    public static class DrawLine
    {
        public static Pen _pen = new Pen(Color.Black, 1);
        public static bool BresenhamAlg { get; set; } = true;
        private static void Bresenham(Bitmap b, Point p0, Point p1)
        {
            int x0 = p0.X;
            int y0 = p0.Y;
            int x1 = p1.X;
            int y1 = p1.Y;
            int dx = x1 - x0;
            int dy = y1 - y0;

            int stepX = 1;
            int stepY = 1;

            if (dx < 0)
            {
                stepX = -1;
            }
            if (dy < 0)
            {
                stepY = -1;
            }
            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            if (adx >= ady)
            {
                int d = 2 * ady - adx;
                int incrE = 2 * ady;
                int incrNE = 2 * (ady - adx);
                int x = x0;
                int y = y0;
                BitmapUtility.AddPixel(b, x, y, _pen.Color);
                int ax = 0;

                while (ax < adx)
                {
                    if (d < 0)
                    {
                        d += incrE;
                    }
                    else
                    {
                        d += incrNE;
                        y += stepY;
                    }
                    x += stepX;
                    ax++;
                    BitmapUtility.AddPixel(b, x, y, _pen.Color);
                }
            }
            else 
            {
                int d = 2 * adx - ady;
                int incrE = 2 * adx;
                int incrNE = 2 * (adx - ady);
                int x = x0;
                int y = y0;
                BitmapUtility.AddPixel(b, x, y, _pen.Color);
                int ay = 0;

                while (ay < ady)
                {
                    if (d < 0)
                    {
                        d += incrE;
                    }
                    else
                    {
                        d += incrNE;
                        x += stepX;
                    }
                    y += stepY;
                    ay++;
                    BitmapUtility.AddPixel(b, x, y, _pen.Color);
                }
            }


        }

        private static void Library(Graphics g, Point p0, Point p1)
        {
            g.DrawLine(_pen, p0, p1);
        }

        public static void Draw(Graphics g, Bitmap bitmap, Point p0, Point p1)
        {
            if (BresenhamAlg)
            {
                Bresenham(bitmap, p0, p1);
            }
            else
            {
                Library(g, p0, p1);
            }
        }
    }
}
