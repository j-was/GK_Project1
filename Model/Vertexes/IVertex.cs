using PolygonEditor.Model.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes
{
    public interface IVertex
    {
        public IEdge[] Edges { get; }
        public float X { get; set; }
        public float Y { get; set; }
        public void Move(float newX, float newY)
        {
            if (!Lock)
            {
                X = newX;
                Y = newY;
                foreach (var e in Edges)
                {
                    e.Reload();
                }
            }
        }

        public void Relocate(float dx, float dy)
        {
            X += dx;
            Y += dy;
        }

        public bool CorrectPolygon();
        public bool Lock { get; set; }
    }
}
