using System.Drawing;

namespace gk_project_1
{
    public class CoordinateMapper
    {
        public float PixelsPerUnit { get; private set; }

        public CoordinateMapper(float pixelsPerUnit)
        {
            PixelsPerUnit = Math.Max(1f, pixelsPerUnit);
        }

        public void SetPixelsPerUnit(float ppu) => PixelsPerUnit = Math.Max(1f, ppu);

        public Point ModelToScreen(PointF modelPt)
        {
            int sx = (int)Math.Round(modelPt.X * PixelsPerUnit);
            int sy = (int)Math.Round(modelPt.Y * PixelsPerUnit);
            return new Point(sx, sy);
        }

        public PointF ScreenToModel(PointF screenPt)
        {
            float mx = screenPt.X / PixelsPerUnit;
            float my = screenPt.Y / PixelsPerUnit;
            return new PointF(mx, my);
        }
    }
}