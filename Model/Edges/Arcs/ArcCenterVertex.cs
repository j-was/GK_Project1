using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Arcs
{
    public class ArcCenterVertex : IVertex
    {
        private ArcEdge _edge;
        public float X { get; set; }
        public float Y { get; set; }

        public IEdge[] Edges { get; } = new IEdge[1];

        public bool Lock { get; set; }

        public bool CorrectPolygon()
        {
            return true;
        }

        public ArcCenterVertex(ArcEdge edge, float x = 0, float y = 0)
        {
            Edges[0] = edge;
            X = x;
            Y = y;
            Lock = false;
        }


    }
}
