namespace FSR3ModSetupUtilityEnhanced
{
    partial class formEditorToml
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            richToml = new RichTextBox();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            labelToml = new Label();
            panel2 = new Panel();
            panel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.DimGray;
            panel1.Controls.Add(richToml);
            panel1.Controls.Add(menuStrip1);
            panel1.Location = new Point(236, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(359, 452);
            panel1.TabIndex = 1;
            // 
            // richToml
            // 
            richToml.BackColor = Color.DimGray;
            richToml.BorderStyle = BorderStyle.None;
            richToml.Dock = DockStyle.Fill;
            richToml.ForeColor = Color.White;
            richToml.Location = new Point(0, 24);
            richToml.Margin = new Padding(0);
            richToml.Name = "richToml";
            richToml.Size = new Size(359, 428);
            richToml.TabIndex = 0;
            richToml.Text = "";
            richToml.TextChanged += richTextBox1_TextChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.DarkGray;
            menuStrip1.BackgroundImageLayout = ImageLayout.None;
            menuStrip1.GripMargin = new Padding(0);
            menuStrip1.ImageScalingSize = new Size(0, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            menuStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(0);
            menuStrip1.RightToLeft = RightToLeft.No;
            menuStrip1.Size = new Size(359, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItem1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4 });
            toolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuItem1.ImageTransparentColor = Color.Transparent;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Padding = new Padding(0);
            toolStripMenuItem1.Size = new Size(42, 24);
            toolStripMenuItem1.Text = "Menu";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.BackColor = Color.DarkGray;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(180, 22);
            toolStripMenuItem2.Text = "Save";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.BackColor = Color.DarkGray;
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(180, 22);
            toolStripMenuItem3.Text = "Reload";
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.BackColor = Color.DarkGray;
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(180, 22);
            toolStripMenuItem4.Text = "Exit";
            toolStripMenuItem4.Click += toolStripMenuItem4_Click;
            // 
            // labelToml
            // 
            labelToml.AutoSize = true;
            labelToml.BackColor = Color.Transparent;
            labelToml.Font = new Font("Segoe UI Semibold", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelToml.ForeColor = Color.DimGray;
            labelToml.Location = new Point(0, 23);
            labelToml.Name = "labelToml";
            labelToml.Size = new Size(213, 50);
            labelToml.TabIndex = 2;
            labelToml.Text = "Toml Editor";
            // 
            // panel2
            // 
            panel2.BackColor = Color.DarkGray;
            panel2.Controls.Add(labelToml);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(218, 100);
            panel2.TabIndex = 3;
            // 
            // formEditorToml
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "formEditorToml";
            Text = "formEditorToml";
            FormClosed += formEditorToml_FormClosed;
            Load += formEditorToml_Load;
            Resize += formEditorToml_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private RichTextBox richToml;
        private MenuStrip menuStrip1;
        private Label labelToml;
        private Panel panel2;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
    }
}