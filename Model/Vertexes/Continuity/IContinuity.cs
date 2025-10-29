using PolygonEditor.Model.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes.Continuity
{
    public interface IContinuity
    {
        public TangentVector GetTangentVector(int id);
        public bool IsSatisfied();
        public void CorrectEdge(int id);

        public void SetToVertex(Vertex vertex);

    }
}
