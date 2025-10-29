using PolygonEditor.Model.Edges.Constraints;
using PolygonEditor.Model.Vertexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Bezier
{
    public class BezierEdge : IEdge
    {
        public Vertex[] Ends { get; } = new Vertex[2];
        public BezierControlVertex[] ControlPoints = new BezierControlVertex[2];

        public bool IsSatisfied()
        {
            return Ends[0].Continuity.IsSatisfied() && Ends[1].Continuity.IsSatisfied();
        }

        public IConstraint Constraint { get; }

        public bool CorrectCW()
        {
            Ends[0].Continuity.CorrectEdge(0);
            Ends[1].Continuity.CorrectEdge(1);
            return IsSatisfied();
        }
        public bool CorrectCCW()
        {
            Ends[1].Continuity.CorrectEdge(1);
            Ends[0].Continuity.CorrectEdge(0);
            return IsSatisfied();
        }

        public void Reload()
        {
            return;
        }

        public BezierEdge(Vertex v0, Vertex v1)
        {
            Ends[0] = v0;
            Ends[1] = v1;
            v0.Edges[0] = this;
            v1.Edges[1] = this;
            ControlPoints[0] = new BezierControlVertex(this, v0.X + v0.Continuity.GetTangentVector(0).dX, v0.Y + v0.Continuity.GetTangentVector(0).dY, 0);
            ControlPoints[1] = new BezierControlVertex(this, v1.X + v1.Continuity.GetTangentVector(1).dX, v1.Y + v1.Continuity.GetTangentVector(1).dY, 1);
            Constraint = new NoConstraint();
        }
        public bool SetConstraint(IConstraint constraint)
        {
            return false;
        }

        public TangentVector GetTangentVector(int id)
        {
            return new TangentVector(ControlPoints[id].X - Ends[id].X, ControlPoints[id].Y - Ends[id].Y);
        }

        public bool FitToTangentVector(int id)
        {
            TangentVector vector = Ends[id].Continuity.GetTangentVector(id);
            ((IVertex)ControlPoints[id]).Move(Ends[id].X + vector.dX, Ends[id].Y + vector.dY);
            return true;
        }

        public bool FitToLine(int id)
        {
            TangentVector vector = Ends[id].Continuity.GetTangentVector(id);
            float vX = Ends[id].X;
            float vY = Ends[id].Y;
            float cX = ControlPoints[id].X;
            float cY = ControlPoints[id].Y;

            float sX;
            float dY;
            if(vector.dX!=0)
            {
                sX = (cX - vX) / vector.dX;
                dY = vector.dY * sX;
            }
            else
            {
                dY = 0;
            }

            ((IVertex)ControlPoints[id]).Move(cX, vY + dY);
            return true;
        }

        public void Lock()
        {
            Ends[0].Lock = true;
            Ends[1].Lock = true;
            Ends[0].Lock = true;
            Ends[1].Lock = true;
        }

        public void Unlock()
        {
            Ends[0].Lock = false;
            Ends[1].Lock = false;
            Ends[0].Lock = false;
            Ends[1].Lock = false;
        }

        public void MoveEdge(float dx, float dy)
        {
            ((IVertex)ControlPoints[0]).Relocate(dx, dy);
            ((IVertex)ControlPoints[1]).Relocate(dx, dy);
            ((IVertex)Ends[1]).Relocate(dx, dy);
        }
    }
}
