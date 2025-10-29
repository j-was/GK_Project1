using System;
using System.Windows.Forms;

namespace gk_project_1
{
    public class HelpForm : Form
    {
        public HelpForm()
        {
            Text = "Help - Instructions";
            Width = 500;
            Height = 500;
            StartPosition = FormStartPosition.CenterParent;
            var tb = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                WordWrap = true,
                Font = new System.Drawing.Font("Consolas", 10)
            };
            tb.Text = @"Instructions:
1. Creating new scene:
    - Go to Draw New -> New Custom Scene
    - This will clear the canvas
    - Click on the canvas to place the first vertex
    - Continue adding vertices by clicking on the canvas — each new vertex will be automatically connected to the previous one with an edge
    - To close the polygon and enter editing mode, click on the first vertex

2. Editing scene:
Vertices, edges, and control points are managed via their respective icons and menus in the right-hand side panel.

Vertices (red):
- Click a vertex to view options in the right-hand panel
    -> Change continuity using radio buttons
    -> Remove the vertex with the button

Edge Controls (purple square):
- Each edge cotnrol is displayed by the middle of a straight line between ends
- Click an edge control to access options
    -> Change edge type
    -> Set constraints
    -> Split the edge

Dragging Behavior:
- Drag a vertex -> moves it; connected edges and adjacent vertices adjust automatically
- Drag an edge control -> moves the entire polygon while preserving its shape
- Drag Bézier control points (blue) -> adjusts only that control point; the rest of that edge remains utouched and other edges polygon adapt accordingly

3. Changing straight lines drawing algorithm:
- Use the radio button in the top-right panel
- This will update the line rendering algorithm used throughout the entire application

4. Loading a predefined scene:
- Go to Draw New → Predefined Scene
- This will clear the canvas
- The single available predefined scene will be loaded automatically, no further selection required

5. Limitations:
- Hierarchy (in some situations): current user move > constraints > continuity
- Moving another control usually fixes any bugs (if any happen)
- Application is tested, but in some strange cases bugs might happen
- Lab requirements are met, but in cases of behaviour that was not required and not directly prohibited, user may be able to set things but they may not be fully implemented (ex. C1 with Arc)
";
            Controls.Add(tb);
        }

        private void InitializeComponent()
        {

        }
    }
}