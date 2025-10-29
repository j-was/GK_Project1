using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PolygonEditor.Model;
using PolygonEditor.Model.Vertexes;
using PolygonEditor.Model.Edges;
using PolygonEditor.Model.Edges.Bezier;
using PolygonEditor.Model.Edges.Arcs;
using PolygonEditor.Model.Edges.Constraints;
using PolygonEditor.Model.Vertexes.Continuity;
using PolygonEditor.View;
using System.Drawing.Drawing2D;

namespace gk_project_1
{
    public partial class MainForm : Form
    {
        private Pen _pen = new Pen(Color.Black, 2);

        private PolygonModel _model = new PolygonModel();

        private readonly Dictionary<Vertex, Control> _vertexControls = new();
        private readonly Dictionary<BezierControlVertex, Control> _bezierControlControls = new();
        private readonly Dictionary<IEdge, Control> _edgeControls = new();


        private object _selectedObject = null;

        public CoordinateMapper Mapper;

        private Bitmap _bitmap;

        private Panel _infoPanel;
        private ListBox _logBox;
        private float DefaultPixelsPerUnit = 4;

        private bool _createMode;

        public MainForm()
        {
            InitializeComponent();

            Mapper = new CoordinateMapper(DefaultPixelsPerUnit);

            this.MinimumSize = new Size(800, 800);
            canvas.BackColor = Color.White;
            EnsureBitmap();

            _infoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 400,
                AutoScroll = false,

            };
            controlPanel.Controls.Add(_infoPanel);
            controlPanel.Controls.SetChildIndex(_infoPanel, 0);

            _logBox = new ListBox
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };
            this.Controls.Add(_logBox);
            _logBox.BringToFront();

            newCustomSceneToolStripMenuItem.Click += (s, e) => MenuNewCustomScene();
            predefinedSceneToolStripMenuItem.Click += (s, e) => MenuPredefinedScene();
            documentationToolStripMenuItem.Click += (s, e) => OpenHelp();

            bresenhamButton.CheckedChanged += (s, e) => { DrawLine.BresenhamAlg = bresenhamButton.Checked; RedrawAll(); };
            libAlgorithmButton.CheckedChanged += (s, e) => { DrawLine.BresenhamAlg = bresenhamButton.Checked; RedrawAll(); };

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.Resize += Canvas_Resize;

            _createMode = true;

            ShowDrawInstructions();

            this.MinimumSize = new Size(800, 800);

            CreatePredefinedScene();
        }

        private void EnsureBitmap()
        {
            int w = canvas.Width;
            int h = canvas.Height;
            _bitmap?.Dispose();
            _bitmap = new Bitmap(w, h);
            canvas.Image = _bitmap;
            RedrawAll();
        }

        private void Canvas_Resize(object? sender, EventArgs e)
        {
            if (canvas.Width <= 0 || canvas.Height <= 0) return;
            int w = canvas.Width;
            int h = canvas.Height;
            canvas.Size = new Size(w, h);
            EnsureBitmap();
            PositionAllControls();
        }

        private void OpenHelp()
        {
            using var hf = new HelpForm();
            hf.ShowDialog(this);
        }

        private void MenuNewCustomScene()
        {
            ClearScene();
            ShowDrawInstructions();
            Log("Scene cleared");
        }

        private void MenuPredefinedScene()
        {
            ClearScene();
            CreatePredefinedScene();
            Log("Predefined scene loaded");
        }

        private void ClearScene()
        {
            _model = new PolygonModel();

            foreach (var c in _vertexControls.Values.ToList())
            {
                canvas.Controls.Remove(c);
                c.Dispose();
            }
            _vertexControls.Clear();

            foreach (var c in _bezierControlControls.Values.ToList())
            {
                canvas.Controls.Remove(c);
                c.Dispose();
            }
            _bezierControlControls.Clear();

            foreach (var c in _edgeControls.Values.ToList())
            {
                canvas.Controls.Remove(c);
                c.Dispose();
            }
            _edgeControls.Clear();

            _selectedObject = null;
            RedrawAll();
            _createMode = true;
        }

        private void CreatePredefinedScene()
        {
            _createMode = false;
            _vertexControls.Clear();
            _bezierControlControls.Clear();
            _edgeControls.Clear();
            var vb = _model.AddFirstVertex(80, 50);
            var v0 = _model.AddNewVertex(106,67);
            var v1 = _model.AddNewVertex(95, 98);
            var v2 = _model.AddNewVertex(49, 108);
            var v3 = _model.AddNewVertex(18, 77);
            var v4 = _model.AddNewVertex(19, 51);
            var v5 = _model.ClosePolygon();

            var newE1 = _model.SetBezierEdge(v5);
            ReplaceEdge(v5, newE1);
            CreateOrUpdateBezierControl(newE1.ControlPoints[0]);
            CreateOrUpdateBezierControl(newE1.ControlPoints[1]);
            var newE2 = _model.SetArcEdge(v2.e);
            ReplaceEdge(v2.e, newE2);
            v1.e.SetConstraint(new LengthConstraint(34));
            v3.e.SetConstraint(new DiagonalConstraint());
            v4.e.SetConstraint(new VerticalConstraint());
            v2.v.SetContinuity(new G1Continuity());


            foreach (var v in _model._vertices) CreateOrUpdateVertexControl(v);
            foreach (var e in _model._edges) CreateOrUpdateEdgeControl(e);

            RedrawAll();
        }

        private void CreateOrUpdateVertexControl(Vertex v)
        {
            if (_vertexControls.ContainsKey(v)) return;
            var pc = new PointControl(v);
            pc.Selected += (iv) => { OnSelectObject(iv); };
            pc.Dragged += (iv, mx, my) => { OnVertexDragged(iv, mx, my); };
            pc.Parent = canvas;
            canvas.Controls.Add(pc);
            _vertexControls[v] = pc;
            PositionControlForVertex(v, pc);
        }

        private void CreateOrUpdateBezierControl(BezierControlVertex bcv)
        {
            if (_bezierControlControls.ContainsKey(bcv)) return;
            var pc = new BezierPointControl(bcv);
            pc.Selected += (b) => { OnSelectObject(b); };
            pc.Dragged += (b, mx, my) => { OnBezierControlDragged(b, mx, my); };
            pc.Parent = canvas;
            canvas.Controls.Add(pc);
            _bezierControlControls[bcv] = pc;
            PositionControlForBezier(bcv, pc);
        }

        private void CreateOrUpdateEdgeControl(IEdge e)
        {
            if (_edgeControls.ContainsKey(e)) return;
            var ec = new EdgeControl(e);
            ec.Selected += (edge) => { OnSelectObject(edge); };
            ec.Dragged += (edge, dx, dy) => { OnEdgeDragged(edge, dx, dy); };
            ec.Parent = canvas;
            canvas.Controls.Add(ec);
            _edgeControls[e] = ec;
            PositionControlForEdge(e, ec);
        }

        private void PositionAllControls()
        {
            foreach (var kv in _vertexControls)
                PositionControlForVertex(kv.Key, kv.Value);
            foreach (var kv in _bezierControlControls)
                PositionControlForBezier(kv.Key, kv.Value);
            foreach (var kv in _edgeControls)
                PositionControlForEdge(kv.Key, kv.Value);
        }

        private void PositionControlForVertex(Vertex v, Control c)
        {
            var pt = Mapper.ModelToScreen(new PointF(v.X, v.Y));
            c.Left = (int)Math.Round(pt.X - c.Width / 2f);
            c.Top = (int)Math.Round(pt.Y - c.Height / 2f);
        }

        private void PositionControlForBezier(BezierControlVertex bcv, Control c)
        {
            var pt = Mapper.ModelToScreen(new PointF(bcv.X, bcv.Y));
            c.Left = (int)Math.Round(pt.X - c.Width / 2f);
            c.Top = (int)Math.Round(pt.Y - c.Height / 2f);
        }

        private void PositionControlForEdge(IEdge e, Control c)
        {
            var a = e.Ends[0];
            var b = e.Ends[1];
            PointF ma = new PointF(a.X, a.Y);
            PointF mb = new PointF(b.X, b.Y);
            var mid = new PointF((ma.X + mb.X) / 2f, (ma.Y + mb.Y) / 2f);

            float dx = mb.X - ma.X;
            float dy = mb.Y - ma.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            var nx = 0f;
            var ny = 0f;
            if (len > 1e-6f)
            {
                nx = -dy / len;
                ny = dx / len;
            }
            float offsetModel = 10f / Mapper.PixelsPerUnit;
            var pModel = new PointF(mid.X + nx * offsetModel, mid.Y + ny * offsetModel);
            var screen = Mapper.ModelToScreen(pModel);
            c.Left = (int)Math.Round(screen.X - c.Width / 2f);
            c.Top = (int)Math.Round(screen.Y - c.Height / 2f);
        }

        private void OnSelectObject(object obj)
        {
            if (_createMode)
            {
                if (_model.First == obj)
                {
                    var eclose = _model.ClosePolygon();
                    _createMode = false;
                    if (eclose != null)
                    {
                        CreateOrUpdateEdgeControl(eclose);
                    }

                    Log($"Polygon closed");
                }
            }
            _selectedObject = obj;
            UpdateInfoPanel();
        }

        private void UpdateInfoPanel()
        {
            _infoPanel.Controls.Clear();
            if (_selectedObject == null)
            {
                ShowDrawInstructionsInPanel(_infoPanel);
                return;
            }
            if (_selectedObject is Vertex v)
            {
                ShowVertexPanel(v);
            }
            else if (_selectedObject is BezierControlVertex bcv)
            {
                ShowBezierControlPanel(bcv);
            }
            else if (_selectedObject is IEdge e)
            {
                ShowEdgePanel(e);
            }
            else
            {
                ShowDrawInstructionsInPanel(_infoPanel);
            }
        }

        private void ShowDrawInstructions()
        {
            _infoPanel.Controls.Clear();
            ShowDrawInstructionsInPanel(_infoPanel);
        }

        private void ShowDrawInstructionsInPanel(Panel pnl)
        {
            pnl.Controls.Clear();
            var lbl = new Label
            {
                Text = "Instructions:\n- Left-click on canvas to add vertices (first click adds first vertex).\n- Left-click on first vertex to close polygon and start editing\n- Drag a vertex to move it.\n- Use side panel to change constraints, continuity and edge types.",
                Dock = DockStyle.Top,
                Height = 140
            };
            pnl.Controls.Add(lbl);
        }

        private void ShowVertexPanel(Vertex v)
        {
            var y = 8;
            void addLabel(string text)
            {
                var lbl = new Label { Text = text, Left = 8, Top = y, Width = _infoPanel.Width - 16, AutoSize = false, Height = 20 };
                _infoPanel.Controls.Add(lbl);
                y += 26;
            }

            addLabel($"Point ({v.X:0.##}, {v.Y:0.##})");

            var contLabel = new Label { Text = "Continuity", Left = 8, Top = y, Height = 20 };
            _infoPanel.Controls.Add(contLabel);
            y += 15;

            var rbNo = new RadioButton { Text = "No", Left = 12, Top = y, Checked = v.Continuity is NoContinuity };
            rbNo.CheckedChanged += (s, e) => { if (rbNo.Checked) v.SetContinuity(new NoContinuity()); RedrawAll(); };
            _infoPanel.Controls.Add(rbNo);
            y += 20;
            var rbG0 = new RadioButton { Text = "G0", Left = 12, Top = y, Checked = v.Continuity is G0Continuity };
            rbG0.CheckedChanged += (s, e) => { if (rbG0.Checked) v.SetContinuity(new G0Continuity()); RedrawAll(); };
            _infoPanel.Controls.Add(rbG0);
            y += 20;
            var rbG1 = new RadioButton { Text = "G1", Left = 12, Top = y, Checked = v.Continuity is G1Continuity };
            rbG1.CheckedChanged += (s, e) => { if (rbG1.Checked) v.SetContinuity(new G1Continuity()); RedrawAll(); };
            _infoPanel.Controls.Add(rbG1);
            y += 20;
            var rbC1 = new RadioButton { Text = "C1", Left = 12, Top = y, Checked = v.Continuity is C1Continuity };
            rbC1.CheckedChanged += (s, e) => { if (rbC1.Checked) v.SetContinuity(new C1Continuity()); RedrawAll(); };
            _infoPanel.Controls.Add(rbC1);
            y += 34;

            var btnRemove = new Button { Text = "Remove Vertex", Left = 8, Top = y, Width = 140 };
            btnRemove.Click += (s, e) =>
            {
                if (_model._vertices.Contains(v))
                {
                    var ret = _model.RemoveVertex(v);
                    if (ret != null)
                    {
                        if (_vertexControls.TryGetValue(v, out var ctrl))
                        {
                            canvas.Controls.Remove(ctrl);
                            ctrl.Dispose();
                            _vertexControls.Remove(v);
                        }

                        CreateOrUpdateEdgeControl(ret?.eNew);

                        if (_edgeControls.TryGetValue(ret?.e1old, out var o1ctrl))
                        {
                            canvas.Controls.Remove(o1ctrl);
                            o1ctrl.Dispose();
                            _edgeControls.Remove(ret?.e1old);
                        }
                        if (_edgeControls.TryGetValue(ret?.e2old, out var o2ctrl))
                        {
                            canvas.Controls.Remove(o2ctrl);
                            o2ctrl.Dispose();
                            _edgeControls.Remove(ret?.e2old);
                        }

                        _selectedObject = null;
                        UpdateInfoPanel();
                        RedrawAll();
                        Log($"Vertex ({v.X:0.##}, {v.Y:0.##}) removed");
                    }

                    Log($"Vertex ({v.X:0.##}, {v.Y:0.##}) not removed - you cannot destroy polygon");
                }
            };
            _infoPanel.Controls.Add(btnRemove);
        }

        private void ShowBezierControlPanel(BezierControlVertex bcv)
        {
            var y = 8;
            var lbl = new Label { Text = $"Bezier control point ({bcv.X:0.##}, {bcv.Y:0.##})", Left = 8, Top = y, Width = _infoPanel.Width - 16 };
            _infoPanel.Controls.Add(lbl);
            y += 26;
        }

        private void ShowEdgePanel(IEdge e)
        {
            var y = 8;
            var a = e.Ends[0];
            var b = e.Ends[1];

            string title = e is BezierEdge ? "Bezier Edge" : e is ArcEdge ? "Arc Edge" : "Straight Edge";
            var lblTitle = new Label { Text = title, Left = 8, Top = y, Width = _infoPanel.Width - 16 };
            _infoPanel.Controls.Add(lblTitle);
            y += 26;

            var lblBeg = new Label { Text = $"Begins at: ({a.X:0.##}, {a.Y:0.##})", Left = 8, Top = y, Width = _infoPanel.Width - 16 };
            _infoPanel.Controls.Add(lblBeg);
            y += 22;
            var lblEnd = new Label { Text = $"Ends at: ({b.X:0.##}, {b.Y:0.##})", Left = 8, Top = y, Width = _infoPanel.Width - 16 };
            _infoPanel.Controls.Add(lblEnd);
            y += 26;

            if (e is StraightEdge)
            {
                var lblConstraints = new Label { Text = "Constraints", Left = 8, Top = y, Width = _infoPanel.Width - 16 };
                _infoPanel.Controls.Add(lblConstraints);
                y += 24;

                var rbNo = new RadioButton { Text = "No", Left = 12, Top = y, Checked = !(e.Constraint is DiagonalConstraint || e.Constraint is VerticalConstraint || e.Constraint is LengthConstraint) };
                rbNo.CheckedChanged += (s, ev) => { if (rbNo.Checked) { e.SetConstraint(new NoConstraint()); UpdateInfoPanel(); RedrawAll(); } };
                _infoPanel.Controls.Add(rbNo);
                y += 24;
                var rbDiag = new RadioButton { Text = "Diagonal", Left = 12, Top = y, Checked = e.Constraint is DiagonalConstraint };
                rbDiag.CheckedChanged += (s, ev) => { if (rbDiag.Checked) { e.SetConstraint(new DiagonalConstraint()); UpdateInfoPanel(); RedrawAll(); } };
                _infoPanel.Controls.Add(rbDiag);
                y += 24;
                var rbVert = new RadioButton { Text = "Vertical", Left = 12, Top = y, Checked = e.Constraint is VerticalConstraint };
                rbVert.CheckedChanged += (s, ev) => { if (rbVert.Checked) { e.SetConstraint(new VerticalConstraint()); UpdateInfoPanel(); RedrawAll(); } };
                _infoPanel.Controls.Add(rbVert);

                y += 24;
                var rbLen = new RadioButton { Text = "Fixed length", Left = 12, Top = y, Checked = e.Constraint is LengthConstraint };
                _infoPanel.Controls.Add(rbLen);

                var tbLen = new TextBox { Left = 120, Top = y - 2, Width = 40,};

                if (e.Constraint is LengthConstraint lc)
                {
                    tbLen.Text = lc.Length.ToString();
                    tbLen.Invalidate();
                }
                _infoPanel.Controls.Add(tbLen);
                y += 24;
                var btnApplyLen = new Button { Text = "Apply", Left = 8, Top = y - 4, Width = 60 };
                btnApplyLen.Click += (s, ev) =>
                {
                    if (int.TryParse(tbLen.Text, out int L))
                    {
                        if (L > 0)
                        {
                            e.SetConstraint(new LengthConstraint(L));
                            RedrawAll();
                        }
                    }
                    else
                    {
                        e.SetConstraint(new LengthConstraint(0));
                        RedrawAll();
                    }
                    UpdateInfoPanel();
                };
                _infoPanel.Controls.Add(btnApplyLen);
                rbLen.CheckedChanged += (s, ev) => { btnApplyLen.PerformClick(); };
                y += 36;

                var btnSplit = new Button { Text = "Split edge", Left = 8, Top = y, Width = 100 };
                btnSplit.Click += (s, ev) =>
                {
                    try
                    {
                        var res = _model.SplitEdge(e);
                        if (res.v != null)
                        {
                            CreateOrUpdateVertexControl(res.v);
                            CreateOrUpdateEdgeControl(res.e1);
                            CreateOrUpdateEdgeControl(res.e2);

                            if (_edgeControls.TryGetValue(e, out var ctrl))
                            {
                                canvas.Controls.Remove(ctrl);
                                ctrl.Dispose();
                                _edgeControls.Remove(e);
                            }
                        }
                        _selectedObject = null;
                        UpdateInfoPanel();
                        RedrawAll();
                        Log("Edge split");
                    }
                    catch (Exception ex)
                    {
                        Log("Split edge failed: " + ex.Message);
                    }
                };
                _infoPanel.Controls.Add(btnSplit);
                y += 36;
            }
            var changeLabel = new Label { Text = "Change type", Left = 8, Top = y, Width = 120 };
            _infoPanel.Controls.Add(changeLabel);
            y += 24;
            var rbStraight = new RadioButton { Text = "Straight", Left = 12, Top = y, Checked = e is StraightEdge };
            rbStraight.CheckedChanged += (s, ev) => { if (rbStraight.Checked) { var newE = _model.SetStraightEdge(e); ReplaceEdge(e, newE); } };
            _infoPanel.Controls.Add(rbStraight);
            y += 24;
            var rbBezier = new RadioButton { Text = "Bezier", Left = 12, Top = y, Checked = e is BezierEdge };
            rbBezier.CheckedChanged += (s, ev) => { if (rbBezier.Checked) { var newE = _model.SetBezierEdge(e); ReplaceEdge(e, newE); } };
            _infoPanel.Controls.Add(rbBezier);
            y += 24;
            var rbArc = new RadioButton { Text = "Arc", Left = 12, Top = y, Checked = e is ArcEdge };
            rbArc.CheckedChanged += (s, ev) => { if (rbArc.Checked) { var newE = _model.SetArcEdge(e); ReplaceEdge(e, newE); } };
            _infoPanel.Controls.Add(rbArc);
        }

        private void ReplaceEdge(IEdge oldE, IEdge newE)
        {
            if (_edgeControls.TryGetValue(oldE, out var ctrl))
            {
                canvas.Controls.Remove(ctrl);
                ctrl.Dispose();
                _edgeControls.Remove(oldE);
            }
            CreateOrUpdateEdgeControl(newE);
            if (newE is BezierEdge be)
            {
                CreateOrUpdateBezierControl(be.ControlPoints[0]);
                CreateOrUpdateBezierControl(be.ControlPoints[1]);
            }
            if (oldE is BezierEdge be1)
            {
                if (_bezierControlControls.TryGetValue(be1.ControlPoints[0], out var ctrl1))
                {
                    canvas.Controls.Remove(ctrl1);
                    ctrl1.Dispose();
                    _bezierControlControls.Remove(be1.ControlPoints[0]);
                }
                if (_bezierControlControls.TryGetValue(be1.ControlPoints[1], out var ctrl2))
                {
                    canvas.Controls.Remove(ctrl2);
                    ctrl2.Dispose();
                    _bezierControlControls.Remove(be1.ControlPoints[1]);
                }
            }
            _selectedObject = null;
            UpdateInfoPanel();
            RedrawAll();
            Log("Edge type changed");
        }

        private void OnVertexDragged(Vertex v, float modelX, float modelY)
        {
            if (!_createMode)
            {
                try
                {
                    _model.MovePoint(v, modelX, modelY);
                    PositionControlForVertex(v, _vertexControls[v]);
                }
                catch (Exception ex)
                {
                    Log($"Vertex drag error: {ex.Message}");
                }
                RedrawAll();
            }
        }

        private void OnBezierControlDragged(BezierControlVertex bcv, float modelX, float modelY)
        {
            if (!_createMode)
            {
                _model.MovePoint(bcv, modelX, modelY);
                PositionControlForBezier(bcv, _bezierControlControls[bcv]);
                RedrawAll();
            }
        }

        private void OnEdgeDragged(IEdge e, float dxModel, float dyModel)
        {
            if (!_createMode)
            {
                _model.MovePolygon(dxModel, dyModel);
                PositionAllControls();
                RedrawAll();
            }
        }

        private bool _isMouseDown = false;
        private Point _mouseDownPoint;

        private void Canvas_MouseDown(object? sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _mouseDownPoint = e.Location;

            if (_createMode && e.Button == MouseButtons.Left)
            {
                var clickedCtrl = canvas.GetChildAtPoint(e.Location);

                if (clickedCtrl == null)
                {
                    var m = Mapper.ScreenToModel(new PointF(e.X, e.Y));
                    if (_model._vertices.Count == 0)
                    {
                        var v = _model.AddFirstVertex(m.X, m.Y);
                        CreateOrUpdateVertexControl(v);
                        Log($"Added first vertex ({m.X:0.##},{m.Y:0.##})");
                    }
                    else
                    {
                        var v = _model.AddNewVertex(m.X, m.Y);
                        CreateOrUpdateVertexControl(v.v);
                        CreateOrUpdateEdgeControl(v.e);
                        Log($"Added vertex ({m.X:0.##},{m.Y:0.##})");
                    }

                }

                RedrawAll();
            }
        }

        private void Canvas_MouseMove(object? sender, MouseEventArgs e)
        {

        }

        private void Canvas_MouseUp(object? sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

        private void OnSelectionChangedAndRefresh()
        {
            UpdateInfoPanel();
            RedrawAll();
        }

        private void RedrawAll()
        {
            if (_bitmap == null) EnsureBitmap();
            canvas.Invalidate();
            using (var g = Graphics.FromImage(_bitmap))
            {
                g.Clear(Color.White);

                foreach (var ec in _edgeControls)
                {
                    ec.Value.Invalidate();
                    var e = ec.Key;
                    var p0 = Mapper.ModelToScreen(new PointF(e.Ends[0].X, e.Ends[0].Y));
                    var p1 = Mapper.ModelToScreen(new PointF(e.Ends[1].X, e.Ends[1].Y));

                    using var pen = _pen;
                    if (e is BezierEdge bez)
                    {
                        var c1 = bez.ControlPoints[0];
                        var c2 = bez.ControlPoints[1];

                        var cp1 = Mapper.ModelToScreen(new PointF(c1.X, c1.Y));
                        var cp2 = Mapper.ModelToScreen(new PointF(c2.X, c2.Y));

                        DrawBezier.Draw(g, _bitmap, p0, cp1, cp2, p1);

                        using (Pen dashedPen = new Pen(Color.Gray, 1))
                        {
                            dashedPen.DashStyle = DashStyle.Dash;
                            g.DrawLine(dashedPen, p0, cp1);
                            g.DrawLine(dashedPen, p1, cp2);
                            g.DrawLine(dashedPen, cp1, cp2);
                        }
                    }
                    else if (e is ArcEdge arc)
                    {
                        try
                        {
                            var midp = Mapper.ModelToScreen(new PointF(arc.Center.X, arc.Center.Y));
                            using var penA = new Pen(Color.Black, 1);
                            DrawArc.Draw(g, p0, p1, midp, arc.Dir);
                        }
                        catch(Exception ex)
                        {
                            Log(ex.Message);
                        }
                    }
                    else
                    {
                        DrawLine.Draw(g, _bitmap, p0, p1);
                    }

                }

                foreach (var vc in _vertexControls)
                {
                    vc.Value.Invalidate();
                    var v = vc.Key;
                    var p = Mapper.ModelToScreen(new PointF(v.X, v.Y));
                    g.FillEllipse(Brushes.Red, p.X - 4, p.Y - 4, 8, 8);
                }

                foreach (var vb in _bezierControlControls)
                {
                    vb.Value.Invalidate();
                    var v = vb.Key;
                    var p = Mapper.ModelToScreen(new PointF(v.X, v.Y));
                    g.FillEllipse(Brushes.Blue, p.X - 4, p.Y - 4, 8, 8);

                    using (Pen dashedPen = new Pen(Color.Gray, 1))
                    {
                        var neigh = Mapper.ModelToScreen(new PointF(v.Edges[0].Ends[v.Neighbor].X, v.Edges[0].Ends[v.Neighbor].Y));
                        dashedPen.DashStyle = DashStyle.Dash;
                        g.DrawLine(dashedPen, neigh, p);
                    }
                }
            }
            canvas.Invalidate();
            PositionAllControls();
        }




        private void Log(string msg)
        {
            _logBox.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} {msg}");
            while (_logBox.Items.Count > 3) _logBox.Items.RemoveAt(3);
        }

        private void radioButton1_CheckedChanged(object? sender, EventArgs e)
        {
            DrawLine.BresenhamAlg = bresenhamButton.Checked;
            RedrawAll();
        }
    }
}