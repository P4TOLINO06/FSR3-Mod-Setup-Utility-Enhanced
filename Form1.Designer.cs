namespace FSR3ModSetupUtilityEnhanced
{
    partial class mainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            mainPanel = new Panel();
            sidebar = new Panel();
            labelVersion = new Label();
            panel4 = new Panel();
            buttonGuide = new Button();
            panel5 = new Panel();
            buttonExit = new Button();
            panel2 = new Panel();
            buttonHome = new Button();
            panel1 = new Panel();
            returnMainForm = new Label();
            menuButton = new PictureBox();
            panel3 = new Panel();
            buttonMods = new Button();
            buttonLibrary = new Button();
            buttonSettings = new Button();
            Date = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            DateOwLabel = new Label();
            flowClock = new FlowLayoutPanel();
            ClockLabel = new Label();
            pictureBox1 = new PictureBox();
            clockTimer = new System.Windows.Forms.Timer(components);
            toolTip1 = new ToolTip(components);
            mainPanel.SuspendLayout();
            sidebar.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).BeginInit();
            panel3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowClock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.Black;
            mainPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mainPanel.Controls.Add(sidebar);
            mainPanel.Controls.Add(Date);
            mainPanel.Controls.Add(flowLayoutPanel1);
            mainPanel.Controls.Add(flowClock);
            mainPanel.Controls.Add(pictureBox1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(919, 511);
            mainPanel.TabIndex = 1;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(30, 34, 46);
            sidebar.Controls.Add(labelVersion);
            sidebar.Controls.Add(panel4);
            sidebar.Controls.Add(panel5);
            sidebar.Controls.Add(panel2);
            sidebar.Controls.Add(panel1);
            sidebar.Controls.Add(panel3);
            sidebar.Location = new Point(0, 0);
            sidebar.MaximumSize = new Size(188, 0);
            sidebar.MinimumSize = new Size(56, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(56, 511);
            sidebar.TabIndex = 8;
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.BackColor = Color.Transparent;
            labelVersion.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelVersion.ForeColor = Color.White;
            labelVersion.Location = new Point(3, 492);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(37, 19);
            labelVersion.TabIndex = 9;
            labelVersion.Text = "label";
            // 
            // panel4
            // 
            panel4.Controls.Add(buttonGuide);
            panel4.Location = new Point(3, 212);
            panel4.Name = "panel4";
            panel4.Size = new Size(200, 55);
            panel4.TabIndex = 4;
            // 
            // buttonGuide
            // 
            buttonGuide.Cursor = Cursors.Hand;
            buttonGuide.FlatAppearance.BorderSize = 0;
            buttonGuide.FlatStyle = FlatStyle.Flat;
            buttonGuide.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonGuide.ForeColor = Color.White;
            buttonGuide.Image = (Image)resources.GetObject("buttonGuide.Image");
            buttonGuide.ImageAlign = ContentAlignment.MiddleLeft;
            buttonGuide.Location = new Point(0, 3);
            buttonGuide.Name = "buttonGuide";
            buttonGuide.Size = new Size(149, 55);
            buttonGuide.TabIndex = 0;
            buttonGuide.Text = "     Guide";
            toolTip1.SetToolTip(buttonGuide, "Guide");
            buttonGuide.UseVisualStyleBackColor = true;
            buttonGuide.Click += button3_Click;
            // 
            // panel5
            // 
            panel5.Controls.Add(buttonExit);
            panel5.Font = new Font("Segoe UI", 13F);
            panel5.Location = new Point(0, 273);
            panel5.Name = "panel5";
            panel5.Size = new Size(200, 61);
            panel5.TabIndex = 5;
            // 
            // buttonExit
            // 
            buttonExit.Cursor = Cursors.Hand;
            buttonExit.FlatAppearance.BorderSize = 0;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonExit.ForeColor = Color.White;
            buttonExit.Image = Properties.Resources.images_removebg_preview__2___3_;
            buttonExit.ImageAlign = ContentAlignment.MiddleLeft;
            buttonExit.Location = new Point(-2, 3);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(155, 55);
            buttonExit.TabIndex = 0;
            buttonExit.Text = "   Exit";
            toolTip1.SetToolTip(buttonExit, "Exit");
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += button4_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(buttonHome);
            panel2.Location = new Point(0, 86);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 61);
            panel2.TabIndex = 2;
            // 
            // buttonHome
            // 
            buttonHome.Cursor = Cursors.Hand;
            buttonHome.FlatAppearance.BorderSize = 0;
            buttonHome.FlatStyle = FlatStyle.Flat;
            buttonHome.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonHome.ForeColor = Color.White;
            buttonHome.Image = (Image)resources.GetObject("buttonHome.Image");
            buttonHome.ImageAlign = ContentAlignment.MiddleLeft;
            buttonHome.Location = new Point(3, 3);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(149, 55);
            buttonHome.TabIndex = 0;
            buttonHome.Text = "   Home";
            toolTip1.SetToolTip(buttonHome, "Home");
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(returnMainForm);
            panel1.Controls.Add(menuButton);
            panel1.Location = new Point(0, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 80);
            panel1.TabIndex = 1;
            // 
            // returnMainForm
            // 
            returnMainForm.AutoSize = true;
            returnMainForm.Cursor = Cursors.Hand;
            returnMainForm.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            returnMainForm.ForeColor = Color.White;
            returnMainForm.Location = new Point(88, 36);
            returnMainForm.Name = "returnMainForm";
            returnMainForm.Size = new Size(64, 25);
            returnMainForm.TabIndex = 1;
            returnMainForm.Text = "Menu";
            toolTip1.SetToolTip(returnMainForm, "Main Menu");
            returnMainForm.Click += label1_Click;
            // 
            // menuButton
            // 
            menuButton.Cursor = Cursors.Hand;
            menuButton.Image = (Image)resources.GetObject("menuButton.Image");
            menuButton.Location = new Point(-2, 14);
            menuButton.Name = "menuButton";
            menuButton.Size = new Size(55, 63);
            menuButton.SizeMode = PictureBoxSizeMode.StretchImage;
            menuButton.TabIndex = 0;
            menuButton.TabStop = false;
            toolTip1.SetToolTip(menuButton, "Open Menu");
            menuButton.Click += menuButton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(buttonMods);
            panel3.Controls.Add(buttonLibrary);
            panel3.Controls.Add(buttonSettings);
            panel3.Location = new Point(3, 148);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 60);
            panel3.TabIndex = 3;
            // 
            // buttonMods
            // 
            buttonMods.Cursor = Cursors.Hand;
            buttonMods.FlatAppearance.BorderSize = 0;
            buttonMods.FlatStyle = FlatStyle.Flat;
            buttonMods.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonMods.ForeColor = Color.White;
            buttonMods.Image = (Image)resources.GetObject("buttonMods.Image");
            buttonMods.ImageAlign = ContentAlignment.MiddleLeft;
            buttonMods.Location = new Point(20, 59);
            buttonMods.Name = "buttonMods";
            buttonMods.Size = new Size(147, 35);
            buttonMods.TabIndex = 2;
            buttonMods.Text = "       Mods Settings";
            toolTip1.SetToolTip(buttonMods, "Mods Settings");
            buttonMods.UseVisualStyleBackColor = true;
            buttonMods.Click += buttonMods_Click;
            // 
            // buttonLibrary
            // 
            buttonLibrary.Cursor = Cursors.Hand;
            buttonLibrary.FlatAppearance.BorderSize = 0;
            buttonLibrary.FlatStyle = FlatStyle.Flat;
            buttonLibrary.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonLibrary.ForeColor = Color.White;
            buttonLibrary.Image = (Image)resources.GetObject("buttonLibrary.Image");
            buttonLibrary.ImageAlign = ContentAlignment.MiddleLeft;
            buttonLibrary.Location = new Point(20, 100);
            buttonLibrary.Name = "buttonLibrary";
            buttonLibrary.Size = new Size(147, 35);
            buttonLibrary.TabIndex = 1;
            buttonLibrary.Text = "    Your Library";
            toolTip1.SetToolTip(buttonLibrary, "Your Library");
            buttonLibrary.UseVisualStyleBackColor = true;
            buttonLibrary.Click += buttonLibrary_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.Cursor = Cursors.Hand;
            buttonSettings.FlatAppearance.BorderSize = 0;
            buttonSettings.FlatStyle = FlatStyle.Flat;
            buttonSettings.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonSettings.ForeColor = Color.White;
            buttonSettings.Image = (Image)resources.GetObject("buttonSettings.Image");
            buttonSettings.ImageAlign = ContentAlignment.MiddleLeft;
            buttonSettings.Location = new Point(0, 3);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(147, 55);
            buttonSettings.TabIndex = 0;
            buttonSettings.Text = "       Settings";
            toolTip1.SetToolTip(buttonSettings, "Settings");
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += button2_Click_1;
            // 
            // Date
            // 
            Date.Anchor = AnchorStyles.Top;
            Date.AutoSize = true;
            Date.BackColor = Color.Transparent;
            Date.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Date.ForeColor = Color.Gainsboro;
            Date.Location = new Point(619, 95);
            Date.Name = "Date";
            Date.Size = new Size(42, 20);
            Date.TabIndex = 7;
            Date.Text = "Date";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(DateOwLabel);
            flowLayoutPanel1.Location = new Point(547, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(312, 65);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // DateOwLabel
            // 
            DateOwLabel.AutoSize = true;
            DateOwLabel.BackColor = Color.Transparent;
            DateOwLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DateOwLabel.ForeColor = Color.Gainsboro;
            DateOwLabel.Location = new Point(3, 0);
            DateOwLabel.Name = "DateOwLabel";
            DateOwLabel.Size = new Size(306, 65);
            DateOwLabel.TabIndex = 5;
            DateOwLabel.Text = "DateofWeek";
            // 
            // flowClock
            // 
            flowClock.Anchor = AnchorStyles.Top;
            flowClock.AutoSize = true;
            flowClock.BackColor = Color.Transparent;
            flowClock.Controls.Add(ClockLabel);
            flowClock.ForeColor = Color.Gray;
            flowClock.Location = new Point(619, 132);
            flowClock.Name = "flowClock";
            flowClock.Size = new Size(179, 50);
            flowClock.TabIndex = 4;
            // 
            // ClockLabel
            // 
            ClockLabel.AutoSize = true;
            ClockLabel.BackColor = Color.Transparent;
            ClockLabel.FlatStyle = FlatStyle.Flat;
            ClockLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ClockLabel.ForeColor = Color.Gainsboro;
            ClockLabel.Location = new Point(3, 0);
            ClockLabel.Name = "ClockLabel";
            ClockLabel.Size = new Size(168, 50);
            ClockLabel.TabIndex = 1;
            ClockLabel.Text = "00:00:00";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(919, 511);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // clockTimer
            // 
            clockTimer.Tick += clockTimer_Tick;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(919, 511);
            Controls.Add(mainPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " FSR3 Mod Setup Utility Enhanced";
            WindowState = FormWindowState.Maximized;
            Load += mainPanel_Load;
            Shown += mainForm_Shown;
            Resize += Form1_Resize;
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            sidebar.ResumeLayout(false);
            sidebar.PerformLayout();
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).EndInit();
            panel3.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowClock.ResumeLayout(false);
            flowClock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel mainPanel;
        private System.Windows.Forms.Timer clockTimer;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowClock;
        private Label ClockLabel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label DateOwLabel;
        private Label Date;
        private ToolTip toolTip1;
        private Panel sidebar;
        private Panel panel2;
        private Button buttonHome;
        private Panel panel1;
        private Label returnMainForm;
        private PictureBox menuButton;
        private Panel panel3;
        private Button buttonMods;
        private Button buttonLibrary;
        private Button buttonSettings;
        private Panel panel4;
        private Button buttonGuide;
        private Panel panel5;
        private Button buttonExit;
        private Label labelVersion;
    }
}
