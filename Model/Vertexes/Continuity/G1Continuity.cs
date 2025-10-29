using PolygonEditor.Model.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes.Continuity
{
    public class G1Continuity : IContinuity
    {
        private Vertex _vertex;
        public void CorrectEdge(int id)
        {
            if (IsSatisfied())
            {
                return;
            }
            _vertex.Edges[id].FitToLine(id);
            //if (!_vertex.Edges[id].FitToLine(id))
            //{
            //    _vertex.Edges[(id + 1) % 2].FitToLine((id + 1) % 2);
            //}

        }


        public TangentVector GetTangentVector(int id)
        {
            return _vertex.Edges[(id + 1) % 2].GetTangentVector((id + 1) % 2).Reverse();
        }

        public bool IsSatisfied()
        {
            var t1 = _vertex.Edges[0].GetTangentVector(0);
            var t2 = _vertex.Edges[1].GetTangentVector(1);

            return Math.Abs(t1.Line - t2.Line) < 0.001 && t1.Direction == -t2.Direction;
        }

        public void SetToVertex(Vertex vertex)
        {
            _vertex = vertex;
        }
    }
}
