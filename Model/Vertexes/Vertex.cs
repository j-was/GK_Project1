using PolygonEditor.Model.Edges;
using PolygonEditor.Model.Edges.Arcs;
using PolygonEditor.Model.Vertexes.Continuity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Vertexes
{
    public class Vertex : IVertex
    {
        public float X { get; set; }
        public float Y { get; set; }

        public IContinuity Continuity { get; set; }

        public IEdge[] Edges { get; } = new IEdge[2];
        public bool Lock { get; set; }

        public bool SetContinuity(IContinuity continuity)
        {
            if (continuity is G1Continuity)
            {
                if (Edges[0] is ArcEdge && Edges[0].Ends[1].Continuity is G1Continuity)
                {
                    return false;
                }
                if (Edges[1] is ArcEdge && Edges[1].Ends[0].Continuity is G1Continuity)
                {
                    return false;
                }
            }
            Continuity = continuity;
            Continuity.SetToVertex(this);
            CorrectPolygon();
            return true;
        }

        public bool CorrectPolygon()
        {
            Lock = true;
            bool cw = false;
            bool ccw = false;
            CorrectCW();
            CorrectCCW();
            for (int i = 0; i < 50; i++)
            {
                if (CorrectCCW())
                {
                    ccw = true;
                    break;
                }
                if (CorrectCW())
                {
                    cw = true;
                    break;
                }
                
            }
            Lock = false;
            return cw && ccw;
        }

        public bool CorrectCW()
        {
            var v = this;
            do
            {
                var e = v.Edges[0];
                if (e.IsSatisfied())
                {
                    return true;
                }
                else
                {
                    e.CorrectCW();
                    v = e.Ends[1];
                }
            } while (v != this);
            return false;
        }

        public bool CorrectCCW()
        {
            var v = this;
            do
            {
                var e = v.Edges[1];
                if (e.IsSatisfied())
                {
                    return true;
                }
                else
                {
                    e.CorrectCCW();
                    v = e.Ends[0];
                }
            } while (v != this);
            return false;
        }

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
            Lock = false;
            Continuity = new NoContinuity();
        }


        public (StraightEdge eNew, IEdge e1old, IEdge e2old) RemoveVertex()
        {
            var e1old = Edges[1];
            var e2old = Edges[0];
            StraightEdge eNew = new StraightEdge(Edges[1].Ends[0], Edges[0].Ends[1]);
            return (eNew, e1old, e2old);
        }
    }
}
