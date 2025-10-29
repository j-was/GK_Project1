using PolygonEditor.Model.Edges;
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
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor.Model
{
    public class PolygonModel
    {
        public List<Vertex> _vertices = new List<Vertex>();
        public List<BezierControlVertex> _bezierControls = new List<BezierControlVertex>();
        public List<IEdge> _edges = new List<IEdge>();

        public void MovePoint(IVertex point, float newX, float newY)
        {
            point.Move(newX, newY);
            point.CorrectPolygon();
        }

        public void MovePolygon(float dx, float dy)
        {
            foreach(var edge in _edges)
            {
                edge.MoveEdge(dx, dy);
            }
        }

        public Vertex? First=null;
        public Vertex? Last = null;

        public (StraightEdge eNew, IEdge e1old, IEdge e2old)? RemoveVertex(Vertex vertex)
        {
            if(_vertices.Count<4)
            {
                return null;
            }
            var e = vertex.RemoveVertex();
            _edges.Add(e.eNew);
            _edges.Remove(e.e1old);
            _edges.Remove(e.e2old);
            _vertices.Remove(vertex);
            ((IEdge)e.eNew).CorrectPolygon();
            return e;

        }

        public BezierEdge SetBezierEdge(IEdge edge)
        {
            var be = edge.SetBezierEdge();
            ((IEdge)be).CorrectPolygon();
            _edges.Remove(edge);
            _edges.Add(be);
            return be;
        }

        public StraightEdge SetStraightEdge(IEdge edge)
        {
            var e = edge.SetStraightEdge();
            _edges.Remove(edge);
            ((IEdge)e).CorrectPolygon();
            _edges.Add(e);
            return e;
        }

        public ArcEdge SetArcEdge(IEdge edge)
        {
            var be = edge.SetArcEdge();
            _edges.Remove(edge);
            ((IEdge)be).CorrectPolygon();
            _edges.Add(be);
            return be;
        }

        public (Vertex v, StraightEdge e1, StraightEdge e2) SplitEdge(IEdge edge)
        {
            var r = edge.SplitEdge();
            _vertices.Add(r.v);
            _edges.Add(r.e1);
            _edges.Add(r.e2);
            _edges.Remove(edge);
            r.v.CorrectPolygon();
            return r;
        }


        public (Vertex v, StraightEdge e) AddNewVertex(float x, float y)
        {
            var v = new Vertex(x, y);
            StraightEdge e = new StraightEdge(Last, v);
            Last.Edges[0] = e;
            Last = v;
            _vertices.Add(v);
            _edges.Add(e);
            return (v, e);
        }

        public Vertex AddFirstVertex(float x, float y)
        {
            var v = new Vertex(x, y);
            First = v;
            Last = v;
            _vertices.Add(v);
            return v;
        }

        public StraightEdge? ClosePolygon()
        {
            if(_vertices.Count<3)
            {
                return null;
            }

            StraightEdge e = new StraightEdge(Last, First);
            
            _edges.Add(e);
            
            return e;
        }
    }
}
