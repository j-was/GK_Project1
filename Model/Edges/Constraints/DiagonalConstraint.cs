using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Constraints
{
    public class DiagonalConstraint : IConstraint
    {
        private StraightEdge _edge;
        public string Symbol => "╱";

        public void SetToEdge(IEdge edge)
        {
            var e = edge as StraightEdge;
            if (e != null)
            {
                _edge = e;
            }
        }

        public bool IsSatisfied()
        {
            return Math.Abs(_edge.Ends[0].X - _edge.Ends[1].X) == Math.Abs(_edge.Ends[0].Y - _edge.Ends[1].Y);

        }

        public void CorrectEdgeCW()
        {
            if (IsSatisfied())
            {
                return;
            }
            float cX = _edge.Ends[0].X;
            float cY = _edge.Ends[0].Y;
            float oX = _edge.Ends[1].X;
            float oY = _edge.Ends[1].Y;
            float dA = (oX - cX + oY - cY) / 2;
            ((IVertex)_edge.Ends[1]).Move(cX + dA, cY + dA);
        }

        public void CorrectEdgeCCW()
        {
            if (IsSatisfied())
            {
                return;
            }
            float cX = _edge.Ends[1].X;
            float cY = _edge.Ends[1].Y;
            float oX = _edge.Ends[0].X;
            float oY = _edge.Ends[0].Y;
            float dA = (oX - cX + oY - cY) / 2;
            ((IVertex)_edge.Ends[0]).Move(cX + dA, cY + dA);
        }

        public void CorrectEdgeMed()
        {
            if (IsSatisfied())
            {
                return;
            }
            float cX = _edge.Ends[0].X;
            float cY = _edge.Ends[0].Y;
            float oX = _edge.Ends[1].X;
            float oY = _edge.Ends[1].Y;
            float dA = (oX - cX + oY - cY) / 4;
            ((IVertex)_edge.Ends[0]).Move(cX + dA, cY + dA);
            ((IVertex)_edge.Ends[1]).Move(oX - dA, oY - dA);
        }
    }
}
