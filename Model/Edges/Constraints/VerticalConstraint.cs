using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Constraints
{
    public class VerticalConstraint : IConstraint
    {
        private StraightEdge _edge;

        public string Symbol => "|";


        public void CorrectEdgeCCW()
        {
            if (IsSatisfied())
            {
                return;
            }
            ((IVertex)_edge.Ends[0]).Move(_edge.Ends[1].X, _edge.Ends[0].Y);
        }

        public void CorrectEdgeCW()
        {
            if (IsSatisfied())
            {
                return;
            }
            ((IVertex)_edge.Ends[1]).Move(_edge.Ends[0].X, _edge.Ends[1].Y);
        }

        public void CorrectEdgeMed()
        {
            if (IsSatisfied())
            {
                return;
            }
            float newx = (_edge.Ends[1].X + _edge.Ends[0].X) / 2;
            ((IVertex)_edge.Ends[0]).Move(newx, _edge.Ends[0].Y);
            ((IVertex)_edge.Ends[1]).Move(newx, _edge.Ends[1].Y);
        }

        public bool IsSatisfied()
        {
            return _edge.Ends[0].X == _edge.Ends[1].X;
        }

        public void SetToEdge(IEdge edge)
        {
            var e = edge as StraightEdge;
            if (e != null)
            {
                _edge = e;
            }
        }
    }
}
