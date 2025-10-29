using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.View
{
    public static class DrawArc
    {
        public static Pen _pen = new Pen(Color.Black, 1);
        public static void Draw(Graphics g, Point p0, Point p1, Point mid, int dir)
        {
            float radius = Distance(p0,mid);

            RectangleF rect = new RectangleF(
            mid.X - radius,
            mid.Y - radius,
            radius * 2,
            radius * 2
        );
            if(rect.Width==0 || rect.Height==0 )
            {
                return;
            }

            float startAngle = CalculateAngle(mid, p0);
            float endAngle = CalculateAngle(mid, p1);

            float sweepAngle = CalculateSweepAngle(startAngle, endAngle, dir);

            try
            {
                g.DrawArc(_pen, rect, startAngle, sweepAngle);
            }
            catch(Exception ex)
            {
                throw new Exception($"arc drawing error: {ex.Message}");
            }
        }

        private static float Distance(PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private static float CalculateAngle(PointF center, PointF point)
        {
            float dx = point.X - center.X;
            float dy = point.Y - center.Y;
            float angleRad = (float)Math.Atan2(dy, dx);

            float angleDeg = angleRad * (180f / (float)Math.PI);

            if (angleDeg < 0) angleDeg += 360;

            return angleDeg;
        }

        private static float CalculateSweepAngle(float startAngle, float endAngle, int dir)
        {
            bool clockwise = dir == 1;
            if (clockwise)
            {
                float sweep = startAngle - endAngle;
                if (sweep < 0) sweep += 360;
                return -sweep;
            }
            else
            {
                float sweep = endAngle - startAngle;
                if (sweep < 0) sweep += 360;
                return sweep;
            }
        }
    }
}
