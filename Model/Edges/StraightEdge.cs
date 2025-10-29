using PolygonEditor.Model.Edges.Constraints;
using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor.Model.Edges
{
    public class StraightEdge : IEdge
    {
        public Vertex[] Ends { get; } = new Vertex[2];

        public bool IsSatisfied()
        {
            return Constraint.IsSatisfied() && Ends[0].Continuity.IsSatisfied() && Ends[1].Continuity.IsSatisfied();
        }
        public IConstraint Constraint { get; private set; }

        public bool SetConstraint(IConstraint constraint)
        {
            if (constraint is VerticalConstraint && (Ends[0].Edges[1].Constraint is VerticalConstraint || Ends[1].Edges[0].Constraint is VerticalConstraint))
            {
                return false;
            }
            else
            {
                Constraint = constraint;
                Constraint.SetToEdge(this);
                Constraint.CorrectEdgeMed();
                return ((IEdge)this).CorrectPolygon();
            }
        }

        public TangentVector GetTangentVector(int id)
        {
            return new TangentVector((Ends[1 - id].X - Ends[id].X), (Ends[1 - id].Y - Ends[id].Y));
        }

        public bool FitToTangentVector(int id)
        {
            TangentVector vector = Ends[id].Continuity.GetTangentVector(id);
            ((IVertex)Ends[1 - id]).Move(Ends[id].X + vector.dX, Ends[id].Y + vector.dY);
            if(id==0)
            {
                Constraint.CorrectEdgeCW();
            }
            else
            {
                Constraint.CorrectEdgeCCW();
            }
                return true;
        }

        public bool CorrectCW()
        {
            Ends[0].Continuity.CorrectEdge(0);
            //Ends[1].Continuity.CorrectEdge(1);
            Constraint.CorrectEdgeCW();
            return IsSatisfied();
        }
        public bool CorrectCCW()
        {
            Ends[1].Continuity.CorrectEdge(1);
            //Ends[0].Continuity.CorrectEdge(0);
            Constraint.CorrectEdgeCCW();
            return IsSatisfied();
        }

        public bool FitToLine(int id)
        {
            TangentVector vector = Ends[id].Continuity.GetTangentVector(id);
            if (Constraint is DiagonalConstraint && Math.Abs(vector.Line) != 1)
            {
                return false;
            }
            if (Constraint is VerticalConstraint && vector.dX != 0)
            {
                return false;
            }
            float cX = Ends[id].X;
            float cY = Ends[id].Y;
            float oX = Ends[1 - id].X;
            float oY = Ends[1 - id].Y;
            float sX;
            float dY;
            if (vector.dX != 0)
            {
                sX = (oX - cX) / vector.dX;
                dY = vector.dY * sX;
            }
            else
            {
                dY = 0;
            }

            ((IVertex)Ends[1 - id]).Move(oX, cY + dY);

            return true;
        }

        public void Lock()
        {
            Ends[0].Lock = true;
            Ends[1].Lock = true;
        }

        public void Unlock()
        {
            Ends[0].Lock = false;
            Ends[1].Lock = false;
        }

        public void MoveEdge(float dx, float dy)
        {
            ((IVertex)Ends[1]).Relocate(dx, dy);
        }

        public void Reload()
        {
            return;
        }

        public StraightEdge(Vertex Begin, Vertex End)
        {
            Ends[0] = Begin;
            Ends[1] = End;
            Begin.Edges[0] = this;
            End.Edges[1] = this;

            Constraint = new NoConstraint();
        }



    }
}
