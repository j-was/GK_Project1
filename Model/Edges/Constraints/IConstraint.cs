using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Constraints
{
    public interface IConstraint
    {
        public void CorrectEdgeCW();
        public void CorrectEdgeCCW();
        public void CorrectEdgeMed();
        public bool IsSatisfied();
        string Symbol { get; }
        public void SetToEdge(IEdge edge);
    }
}
