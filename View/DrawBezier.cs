using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.View
{
    public static class DrawBezier
    {
        public static Pen _pen = new Pen(Color.Black, 1);
        public static void Draw(Graphics g,Bitmap b, Point p0, Point p1, Point p2, Point p3)
        {
            int nPoints = 2*Math.Max(Math.Abs(p3.X - p0.X), Math.Abs(p3.Y - p0.Y));
            Point[] points = new Point[nPoints];

            PointF A = new PointF(
            -p0.X + 3 * p1.X - 3 * p2.X + p3.X,
            -p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y
        );
            PointF B = new PointF(
            3 * p0.X - 6 * p1.X + 3 * p2.X,
            3 * p0.Y - 6 * p1.Y + 3 * p2.Y
        );
            PointF C = new PointF(
            -3 * p0.X + 3 * p1.X,
            -3 * p0.Y + 3 * p1.Y
        );
            PointF D = p0;

            for (int i = 0; i < nPoints; i++)
            {
                float t = i / (float)(nPoints - 1);
                PointF point = new PointF(
                    ((A.X * t + B.X) * t + C.X) * t + D.X,
                    ((A.Y * t + B.Y) * t + C.Y) * t + D.Y
                    );
                Point pixPoint = new Point((int)Math.Round(point.X), (int)Math.Round(point.Y));
                points[i] = pixPoint;
                if (i > 0)
                {
                    if (Math.Abs(points[i].X - points[i - 1].X) > 1 || Math.Abs(points[i].Y - points[i - 1].Y) > 1)
                    {
                        DrawLine.Draw(g, b, points[i - 1], points[i]);
                    }
                    else
                    {
                        BitmapUtility.AddPixel(b, points[i - 1].X, points[i - 1].Y, _pen.Color);
                        BitmapUtility.AddPixel(b, points[i].X, points[i].Y, _pen.Color);
                    }
                }
            }

        }
    }
}
