using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges
{
    public class TangentVector
    {
        public float dX { get; set; }
        public float dY { get; set; }
        public double Length { get => Math.Sqrt(dY * dY + dX * dX); }
        public float Line { get => dX == 0 ? float.MaxValue:  dY / dX; }
        public int Direction { get => Math.Sign(dX); }
        public TangentVector(float dx, float dy)
        {
            dX = dx;
            dY = dy;
        }
        public TangentVector Reverse()
        {
            return new TangentVector(-dX, -dY);
        }
    }
}
