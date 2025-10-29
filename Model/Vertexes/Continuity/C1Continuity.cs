using PolygonEditor.Model.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes.Continuity
{
    public class C1Continuity : IContinuity
    {
        private Vertex _vertex;
        public void CorrectEdge(int id)
        {
            if (IsSatisfied())
            {
                return;
            }
            //_vertex.Edges[id].FitToTangentVector(id);
            if (!_vertex.Edges[id].FitToTangentVector(id))
            {
                _vertex.Edges[1 - id].FitToTangentVector(1 - id);
            }
        }
        public TangentVector GetTangentVector(int id)
        {
            return _vertex.Edges[1 - id].GetTangentVector(1 - id).Reverse();
        }

        public bool IsSatisfied()
        {
            var t1 = _vertex.Edges[0].GetTangentVector(0);
            var t2 = _vertex.Edges[1].GetTangentVector(1);

            return Math.Abs(t1.dX + t2.dX) < 0.001 && Math.Abs(t1.dY + t2.dY) < 0.001;
        }

        public void SetToVertex(Vertex vertex)
        {
            _vertex = vertex;
        }
    }
}