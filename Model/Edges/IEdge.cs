using PolygonEditor.Model.Edges.Arcs;
using PolygonEditor.Model.Edges.Bezier;
using PolygonEditor.Model.Edges.Constraints;
using PolygonEditor.Model.Vertexes;
using PolygonEditor.Model.Vertexes.Continuity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges
{
    public interface IEdge
    {
        public Vertex[] Ends { get; }
        public bool IsSatisfied();
        public IConstraint Constraint { get; }
        public bool SetConstraint(IConstraint constraint);
        public TangentVector GetTangentVector(int id);
        public bool FitToTangentVector(int id);
        public bool FitToLine(int id);

        public void Lock();
        public void Unlock();

        public void MoveEdge(float dx, float dy);
        public bool CorrectCW();
        public bool CorrectCCW();

        public void Reload();
        public bool CorrectPolygon()
        {
            bool ret = false;
            Lock();
            for (int i = 0; i < 50; i++)
            {
                var cw = Ends[1].CorrectCW();
                var ccw = Ends[0].CorrectCCW();
                if (cw && ccw)
                {
                    ret = true;
                    break;
                }
            }
            Unlock();
            return ret;
        }


        public (Vertex v, StraightEdge e1, StraightEdge e2) SplitEdge()
        {
            float midX = (Ends[1].X + Ends[0].X) / 2;
            float midY = (Ends[1].Y + Ends[0].Y) / 2;
            Vertex mid = new Vertex(midX, midY);
            StraightEdge e1 = new StraightEdge(Ends[0], mid);
            StraightEdge e2 = new StraightEdge(mid, Ends[1]);
            return (mid, e1, e2);
        }

        public bool SetVerticalEdge()
        {
            return SetConstraint(new VerticalConstraint());
        }

        public bool SetDiagonalEdge()
        {
            return SetConstraint(new DiagonalConstraint());
        }

        public bool SetFixedLengthEdge(int length = 0)
        {
            if (length == 0)
            {
                length = (int)Math.Sqrt((Ends[0].X - Ends[1].X) * (Ends[0].X - Ends[1].X) + (Ends[0].Y - Ends[1].Y) * (Ends[0].Y - Ends[1].Y));
            }
            return SetConstraint(new LengthConstraint(length));
        }

        public BezierEdge SetBezierEdge()
        {
            var v1 = Ends[0];
            var v2 = Ends[1];
            if (v1.Continuity is NoContinuity)
            {
                v1.Continuity = new C1Continuity();
                v1.Continuity.SetToVertex(v1);
            }
            if (v2.Continuity is NoContinuity)
            {
                v2.Continuity = new C1Continuity();
                v2.Continuity.SetToVertex(v2);
            }
            var be = new BezierEdge(v1, v2);
            return be;
        }

        public StraightEdge SetStraightEdge()
        {
            StraightEdge e = new StraightEdge(Ends[0], Ends[1]);
            return e;
        }

        public ArcEdge SetArcEdge()
        {

            var v1 = Ends[0];
            var v2 = Ends[1];
            if (v1.Continuity is NoContinuity || v1.Continuity is C1Continuity)
            {
                v1.Continuity = new G0Continuity();
                v1.Continuity.SetToVertex(v1);
            }
            if (v2.Continuity is NoContinuity || v2.Continuity is C1Continuity)
            {
                v2.Continuity = new G0Continuity();
                v2.Continuity.SetToVertex(v2);
            }
            if (v1.Continuity is G1Continuity && v2.Continuity is G1Continuity)
            {
                v2.Continuity = new G0Continuity();
                v2.Continuity.SetToVertex(v2);
            }
            var be = new ArcEdge(v1, v2);
            return be;
        }

    }
}
