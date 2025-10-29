using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Bezier
{
    public class BezierControlVertex : IVertex
    {
        public float X { get;  set; }
        public float Y { get;  set; }
        public int Neighbor { get; set; }

        public IEdge[] Edges { get; } = new IEdge[1];

        public bool Lock { get ; set ; }

        public bool CorrectPolygon()
        {
            return Edges[0].CorrectPolygon();
        }

        public BezierControlVertex(BezierEdge edge, float x, float y, int neighbor)
        {
            Edges[0] = edge;
            X = x;
            Y = y;
            Lock = false;
            Neighbor = neighbor;
        }
    }
}
