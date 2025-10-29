using System;
using System.Drawing;
using System.Windows.Forms;
using PolygonEditor.Model.Edges.Bezier;

namespace gk_project_1
{
    public class BezierPointControl : Control
    {
        public event Action<BezierControlVertex>? Selected;
        public event Action<BezierControlVertex, float, float>? Dragged;

        private bool _dragging = false;
        private readonly BezierControlVertex _control;
        private readonly int _size = 12;

        public BezierPointControl(BezierControlVertex control)
        {
            _control = control;
            Width = _size;
            Height = _size;
            Cursor = Cursors.SizeAll;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var b = new SolidBrush(Color.LightBlue);
            g.FillEllipse(b, 1, 1, Width - 2, Height - 2);
            if (Focused)
            {
                using var pen = new Pen(Color.Blue, 2);
                g.DrawEllipse(pen, 1, 1, Width - 2, Height - 2);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Selected?.Invoke(_control);
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
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
                Dragged?.Invoke(_control, modelPt.X, modelPt.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
            Capture = false;
        }
    }
}