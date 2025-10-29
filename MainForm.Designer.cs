namespace gk_project_1
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            drawNewToolStripMenuItem = new ToolStripMenuItem();
            newCustomSceneToolStripMenuItem = new ToolStripMenuItem();
            predefinedSceneToolStripMenuItem = new ToolStripMenuItem();
            documentationToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            controlPanel = new Panel();
            algorithmBox = new GroupBox();
            libAlgorithmButton = new RadioButton();
            bresenhamButton = new RadioButton();
            tableLayoutPanel1 = new TableLayoutPanel();
            canvas = new PictureBox();
            menuStrip1.SuspendLayout();
            controlPanel.SuspendLayout();
            algorithmBox.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // drawNewToolStripMenuItem
            // 
            drawNewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newCustomSceneToolStripMenuItem, predefinedSceneToolStripMenuItem });
            drawNewToolStripMenuItem.Name = "drawNewToolStripMenuItem";
            drawNewToolStripMenuItem.Size = new Size(71, 20);
            drawNewToolStripMenuItem.Text = "Draw new";
            // 
            // newCustomSceneToolStripMenuItem
            // 
            newCustomSceneToolStripMenuItem.Name = "newCustomSceneToolStripMenuItem";
            newCustomSceneToolStripMenuItem.Size = new Size(174, 22);
            newCustomSceneToolStripMenuItem.Text = "New custom scene";
            // 
            // predefinedSceneToolStripMenuItem
            // 
            predefinedSceneToolStripMenuItem.Name = "predefinedSceneToolStripMenuItem";
            predefinedSceneToolStripMenuItem.Size = new Size(174, 22);
            predefinedSceneToolStripMenuItem.Text = "Predefined scene";
            // 
            // documentationToolStripMenuItem
            // 
            documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            documentationToolStripMenuItem.Size = new Size(102, 20);
            documentationToolStripMenuItem.Text = "Documentation";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { drawNewToolStripMenuItem, documentationToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(algorithmBox);
            controlPanel.Dock = DockStyle.Fill;
            controlPanel.Location = new Point(553, 3);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(244, 420);
            controlPanel.TabIndex = 0;
            // 
            // algorithmBox
            // 
            algorithmBox.Controls.Add(libAlgorithmButton);
            algorithmBox.Controls.Add(bresenhamButton);
            algorithmBox.Dock = DockStyle.Top;
            algorithmBox.Location = new Point(0, 0);
            algorithmBox.Name = "algorithmBox";
            algorithmBox.Size = new Size(244, 73);
            algorithmBox.TabIndex = 0;
            algorithmBox.TabStop = false;
            algorithmBox.Text = "Line drawing algorithm";
            // 
            // libAlgorithmButton
            // 
            libAlgorithmButton.AutoSize = true;
            libAlgorithmButton.Location = new Point(6, 47);
            libAlgorithmButton.Name = "libAlgorithmButton";
            libAlgorithmButton.Size = new Size(61, 19);
            libAlgorithmButton.TabIndex = 1;
            libAlgorithmButton.Text = "Library";
            libAlgorithmButton.UseVisualStyleBackColor = true;
            // 
            // bresenhamButton
            // 
            bresenhamButton.AutoSize = true;
            bresenhamButton.Checked = true;
            bresenhamButton.Location = new Point(6, 22);
            bresenhamButton.Name = "bresenhamButton";
            bresenhamButton.Size = new Size(84, 19);
            bresenhamButton.TabIndex = 0;
            bresenhamButton.TabStop = true;
            bresenhamButton.Text = "Bresenham";
            bresenhamButton.UseVisualStyleBackColor = true;
            bresenhamButton.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.Controls.Add(controlPanel, 1, 0);
            tableLayoutPanel1.Controls.Add(canvas, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 426);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // canvas
            // 
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(3, 3);
            canvas.MinimumSize = new Size(500, 500);
            canvas.Name = "canvas";
            canvas.Size = new Size(544, 500);
            canvas.SizeMode = PictureBoxSizeMode.AutoSize;
            canvas.TabIndex = 1;
            canvas.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Polygon editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            controlPanel.ResumeLayout(false);
            algorithmBox.ResumeLayout(false);
            algorithmBox.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStripMenuItem lineDrawingAlgorithmToolStripMenuItem;
        private ToolStripMenuItem bezierToolStripMenuItem;
        private ToolStripMenuItem libraryToolStripMenuItem;
        private ToolStripMenuItem drawNewToolStripMenuItem;
        private ToolStripMenuItem newCustomSceneToolStripMenuItem;
        private ToolStripMenuItem predefinedSceneToolStripMenuItem;
        private ToolStripMenuItem documentationToolStripMenuItem;
        private MenuStrip menuStrip1;
        private Panel controlPanel;
        private GroupBox algorithmBox;
        private RadioButton libAlgorithmButton;
        private RadioButton bresenhamButton;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox canvas;
    }
}
