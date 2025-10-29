using PolygonEditor.Model.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes.Continuity
{
    public class NoContinuity : IContinuity
    {
        private Vertex _vertex; 
        public void CorrectEdge(int id)
        {
            return;
        }

        public TangentVector GetTangentVector(int id)
        {
            return _vertex.Edges[(id + 1) % 2].GetTangentVector((id + 1) % 2).Reverse();
        }

        public bool IsSatisfied()
        {
            return true;
        }

        public void SetToVertex(Vertex vertex)
        {
            _vertex = vertex;
        }
    }
}
