using System;
using System.Drawing;
using System.Windows.Forms;
using PolygonEditor.Model.Vertexes;

namespace gk_project_1
{
    // small red circle representing a vertex; supports click selection and dragging.
    public class PointControl : Control
    {
        public event Action<Vertex>? Selected;
        public event Action<Vertex, float, float>? Dragged;

        private bool _dragging = false;
        private Point _dragStartScreen;
        private PointF _dragStartModel;
        private readonly Vertex _vertex;
        private readonly int _size = 12;
        private CoordinateMapper? _mapper => Parent?.Parent is MainForm form ? form.Mapper : null;

        public PointControl(Vertex vertex)
        {
            _vertex = vertex;
            Width = _size;
            Height = _size;
            this.Cursor = Cursors.Hand;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var b = new SolidBrush(Color.Red);
            g.FillEllipse(b, 1, 1, Width - 2, Height - 2);
            if (Focused)
            {
                using var pen = new Pen(Color.Yellow, 2);
                g.DrawEllipse(pen, 1, 1, Width - 2, Height - 2);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Selected?.Invoke(_vertex);
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                _dragStartScreen = Cursor.Position;
                _dragStartModel = new PointF(_vertex.X, _vertex.Y);
                Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging && Parent != null)
            {
                var canvas = Parent;
                var mp = canvas.PointToClient(Cursor.Position);
                var form = this.FindForm() as MainForm;
                if (form == null) return;
                var modelPt = form.Mapper.ScreenToModel(new PointF(mp.X, mp.Y));
                Dragged?.Invoke(_vertex, modelPt.X, modelPt.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_dragging)
            {
                _dragging = false;
                Capture = false;
            }
        }
    }
}