using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Constraints
{
    public class LengthConstraint : IConstraint
    {
        private StraightEdge _edge;
        public int Length { get; private set; }
        public string Symbol => $"{Length}";

        public void SetToEdge(IEdge edge)
        {
            var e = edge as StraightEdge;
            if (e != null)
            {
                _edge = e;
                if(Length==0)
                {
                    float cX = _edge.Ends[0].X;
                    float cY = _edge.Ends[0].Y;
                    float oX = _edge.Ends[1].X;
                    float oY = _edge.Ends[1].Y;
                    double currentLength = Math.Sqrt((oX - cX) * (oX - cX) + (oY - cY) * (oY - cY));
                    SetLength(Convert.ToInt32( currentLength));
                }
            }

            
        }

        public LengthConstraint(int l)
        {
            Length = l;
        }

        public void SetLength(int l)
        {
            Length = l;
            CorrectEdgeMed();
        }

        public bool IsSatisfied()
        {
            double currentLength = Math.Sqrt(
         (_edge.Ends[0].X - _edge.Ends[1].X) * (_edge.Ends[0].X - _edge.Ends[1].X) +
         (_edge.Ends[0].Y - _edge.Ends[1].Y) * (_edge.Ends[0].Y - _edge.Ends[1].Y));
            return Math.Abs(currentLength - Length) < 0.001;
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
            float dx = oX - cX;
            float dy = oY - cY;
            double currentLength = Math.Sqrt((oX - cX) * (oX - cX) + (oY - cY) * (oY - cY));

            double scale = Length / currentLength;
            float newX = (float)(cX + (oX - cX) * scale);
            float newY = (float)(cY + (oY - cY) * scale);

            ((IVertex)_edge.Ends[1]).Move(newX, newY);
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
            float dx = oX - cX;
            float dy = oY - cY;
            double currentLength = Math.Sqrt((oX - cX) * (oX - cX) + (oY - cY) * (oY - cY));

            double scale = Length / currentLength;
            float newX = (float)(cX + (oX - cX) * scale);
            float newY = (float)(cY + (oY - cY) * scale);

            ((IVertex)_edge.Ends[0]).Move(newX, newY);

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

            double currentLength = Math.Sqrt((oX - cX) * (oX - cX) + (oY - cY) * (oY - cY));
            double scale = Length / currentLength;
            float newX = (float)((oX - cX) * scale);
            float newY = (float)((oY - cY) * scale);

            float dx = (newX - (oX - cX))/2;
            float dy = (newY - (oY - cY))/2;

            ((IVertex)_edge.Ends[0]).Move(cX-dx, cY-dy);
            ((IVertex)_edge.Ends[1]).Move(oX+dx, oY+dy);
        }
    }
}
