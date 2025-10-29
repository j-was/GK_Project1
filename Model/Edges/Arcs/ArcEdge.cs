using PolygonEditor.Model.Edges.Constraints;
using PolygonEditor.Model.Vertexes;
using PolygonEditor.Model.Vertexes.Continuity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Arcs
{
    public class ArcEdge : IEdge
    {
        public Vertex[] Ends { get; } = new Vertex[2];
        public ArcCenterVertex Center { get; }
        public ArcCenterVertex Mid { get; }
        public int Dir { get; private set; }
        public IConstraint Constraint { get; }
        public bool FitToLine(int id)
        {
            TangentVector vector = Ends[id].Continuity.GetTangentVector(id);
            float x1 = Ends[id].X;
            float y1 = Ends[id].Y;
            float rx;
            float ry;
            float Mx = Mid.X;
            float My = Mid.Y;
            float vx = vector.dX;
            float vy = vector.dY;

            if (Math.Abs(vector.dX * (Ends[1-id].Y - y1) - vector.dY * (Ends[1-id].X - x1)) < 0.001)
            {
                return false;
            }

            float mx = Mx - x1;
            float my = My - y1;
            float dot = mx * vx + my * vy;
            float det = mx * vy - my * vx;

            if (Math.Abs(det) < 0.001f) return false;

            rx = my * dot / det;
            ry = -mx * dot / det;

            Center.X = Mx + rx;
            Center.Y = My + ry;

            float rxA = x1 - Center.X; 
            float ryA = y1 - Center.Y;

            float cross = rxA * vy - ryA * vx;

            Dir = cross > 0 ? 1 : -1;

            return true;

        }

        public bool FitToTangentVector(int id)
        {
            return true;
        }

        public TangentVector GetTangentVector(int id)
        {
            var ret = new TangentVector(0, 0);
            float dx = Ends[0].X - Center.X;
            float dy = Ends[0].Y - Center.Y;
            if (Dir > 0)
            {
                ret.dX = dy;
                ret.dY = -dx;
            }
            else
            {
                ret.dX = -dy;
                ret.dY = dx;
            }
            return ret;
        }

        public bool IsSatisfied()
        {
            return Ends[0].Continuity.IsSatisfied() && Ends[1].Continuity.IsSatisfied();
        }

        public bool SetConstraint(IConstraint constraint)
        {
            return true;
        }

        public void SetMid(int id = 0)
        {
            var v1 = Ends[0];
            var v2 = Ends[1];
            Mid.X = (v2.X + v1.X) / 2;
            Mid.Y = (v2.Y + v1.Y) / 2;
            var vector = Ends[id].Continuity.GetTangentVector(id);
            float cX = Ends[id].X;
            float cY = Ends[id].Y;
            float dX = Mid.X - cX;
            float dY = Mid.Y - cY;
            int cs = Math.Sign(vector.dX * dY - vector.dY * dX);
            if (cs >= 0)
            {
                Dir = -1;
            }
            else
            {
                Dir = 1;
            }
        }

        public void Reload()
        {
            SetMid();
            Center.X = Mid.X;
            Center.Y = Mid.Y;
            CorrectCW();
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

        public void MoveEdge(float dx, float dy)
        {
            ((IVertex)Ends[1]).Relocate(dx, dy);
            ((IVertex)Center).Relocate(dx, dy);
        }

        public ArcEdge(Vertex v1, Vertex v2)
        {
            Ends[0] = v1;
            Ends[1] = v2;
            v1.Edges[0] = this;
            v2.Edges[1] = this;
            Center = new ArcCenterVertex(this);
            Mid = new ArcCenterVertex(this);
            SetMid();
            Center.X = Mid.X;
            Center.Y = Mid.Y;
            Constraint = new ArcConstraint();
        }
    }
}
