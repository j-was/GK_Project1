using System;
using System.Drawing;
using System.Windows.Forms;
using PolygonEditor.Model.Edges;

namespace gk_project_1
{
    public class EdgeControl : Control
    {
        public event Action<IEdge>? Selected;
        public event Action<IEdge, float, float>? Dragged; // dx, dy in model units

        private bool _dragging = false;
        private Point _dragStartScreen;
        private readonly IEdge _edge;
        private readonly int _size = 20;

        public EdgeControl(IEdge edge)
        {
            _edge = edge;
            Width = _size;
            Height = _size;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            using var b = new SolidBrush(Color.FromArgb(200, 160, 64, 255)); 
            g.FillRectangle(b, 0, 0, Width, Height);
            using var pen = new Pen(Color.Black, 1);
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            string sym = "";
            if (_edge.Constraint != null) sym = _edge.Constraint.Symbol;
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(sym, SystemFonts.DefaultFont, Brushes.White, new RectangleF(0, 0, Width, Height), sf);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Selected?.Invoke(_edge);
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                _dragStartScreen = Cursor.Position;
                Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging && Parent != null)
            {
                var canvas = Parent;
                var form = this.FindForm() as MainForm;
                if (form == null) return;
                var curScreen = Cursor.Position;
                var deltaScreen = new Point(curScreen.X - _dragStartScreen.X, curScreen.Y - _dragStartScreen.Y);
                _dragStartScreen = curScreen;
                var dxModel = deltaScreen.X / form.Mapper.PixelsPerUnit;
                var dyModel = deltaScreen.Y / form.Mapper.PixelsPerUnit;
                Dragged?.Invoke(_edge, dxModel, dyModel);
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