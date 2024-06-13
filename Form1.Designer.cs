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
            sidebar = new FlowLayoutPanel();
            panel1 = new Panel();
            returnMainForm = new Label();
            menuButton = new PictureBox();
            panel2 = new Panel();
            buttonHome = new Button();
            panel3 = new Panel();
            buttonSettings = new Button();
            panel4 = new Panel();
            buttonGuide = new Button();
            panel5 = new Panel();
            buttonExit = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            sidebarTimer = new System.Windows.Forms.Timer(components);
            mainPanel = new Panel();
            Date = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            DateOwLabel = new Label();
            flowClock = new FlowLayoutPanel();
            ClockLabel = new Label();
            pictureBox1 = new PictureBox();
            clockTimer = new System.Windows.Forms.Timer(components);
            sidebar.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            mainPanel.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowClock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(30, 35, 45);
            sidebar.Controls.Add(panel1);
            sidebar.Controls.Add(panel2);
            sidebar.Controls.Add(panel3);
            sidebar.Controls.Add(panel4);
            sidebar.Controls.Add(panel5);
            sidebar.Controls.Add(flowLayoutPanel2);
            sidebar.Location = new Point(0, -6);
            sidebar.MaximumSize = new Size(188, 0);
            sidebar.MinimumSize = new Size(56, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(56, 426);
            sidebar.TabIndex = 0;
            sidebar.Paint += flowLayoutPanel1_Paint;
            // 
            // panel1
            // 
            panel1.Controls.Add(returnMainForm);
            panel1.Controls.Add(menuButton);
            panel1.Location = new Point(3, 3);
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
            returnMainForm.Click += label1_Click;
            // 
            // menuButton
            // 
            menuButton.Cursor = Cursors.Hand;
            menuButton.Image = (Image)resources.GetObject("menuButton.Image");
            menuButton.Location = new Point(-3, 14);
            menuButton.Name = "menuButton";
            menuButton.Size = new Size(55, 63);
            menuButton.SizeMode = PictureBoxSizeMode.StretchImage;
            menuButton.TabIndex = 0;
            menuButton.TabStop = false;
            menuButton.Click += menuButton_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(buttonHome);
            panel2.Location = new Point(3, 89);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 69);
            panel2.TabIndex = 2;
            // 
            // buttonHome
            // 
            buttonHome.FlatAppearance.BorderSize = 0;
            buttonHome.FlatStyle = FlatStyle.Flat;
            buttonHome.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonHome.ForeColor = Color.White;
            buttonHome.Image = (Image)resources.GetObject("buttonHome.Image");
            buttonHome.ImageAlign = ContentAlignment.MiddleLeft;
            buttonHome.Location = new Point(3, 3);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(149, 51);
            buttonHome.TabIndex = 0;
            buttonHome.Text = "   Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(buttonSettings);
            panel3.Location = new Point(3, 164);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 74);
            panel3.TabIndex = 3;
            // 
            // buttonSettings
            // 
            buttonSettings.FlatAppearance.BorderSize = 0;
            buttonSettings.FlatStyle = FlatStyle.Flat;
            buttonSettings.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonSettings.ForeColor = Color.White;
            buttonSettings.Image = (Image)resources.GetObject("buttonSettings.Image");
            buttonSettings.ImageAlign = ContentAlignment.MiddleLeft;
            buttonSettings.Location = new Point(3, 3);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(147, 52);
            buttonSettings.TabIndex = 0;
            buttonSettings.Text = "       Settings";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += button2_Click_1;
            // 
            // panel4
            // 
            panel4.Controls.Add(buttonGuide);
            panel4.Location = new Point(3, 244);
            panel4.Name = "panel4";
            panel4.Size = new Size(200, 61);
            panel4.TabIndex = 4;
            // 
            // buttonGuide
            // 
            buttonGuide.FlatAppearance.BorderSize = 0;
            buttonGuide.FlatStyle = FlatStyle.Flat;
            buttonGuide.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonGuide.ForeColor = Color.White;
            buttonGuide.Image = (Image)resources.GetObject("buttonGuide.Image");
            buttonGuide.ImageAlign = ContentAlignment.MiddleLeft;
            buttonGuide.Location = new Point(3, 3);
            buttonGuide.Name = "buttonGuide";
            buttonGuide.Size = new Size(149, 52);
            buttonGuide.TabIndex = 0;
            buttonGuide.Text = "     Guide";
            buttonGuide.UseVisualStyleBackColor = true;
            buttonGuide.Click += button3_Click;
            // 
            // panel5
            // 
            panel5.Controls.Add(buttonExit);
            panel5.Location = new Point(3, 311);
            panel5.Name = "panel5";
            panel5.Size = new Size(200, 60);
            panel5.TabIndex = 5;
            // 
            // buttonExit
            // 
            buttonExit.FlatAppearance.BorderSize = 0;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonExit.ForeColor = Color.White;
            buttonExit.Image = Properties.Resources.images_removebg_preview__2___3_;
            buttonExit.ImageAlign = ContentAlignment.MiddleLeft;
            buttonExit.Location = new Point(-3, 3);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(155, 49);
            buttonExit.TabIndex = 0;
            buttonExit.Text = "   Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += button4_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.BackColor = Color.FromArgb(30, 35, 45);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(3, 377);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(0, 0);
            flowLayoutPanel2.TabIndex = 8;
            // 
            // sidebarTimer
            // 
            sidebarTimer.Interval = 13;
            sidebarTimer.Tick += sidebarTimer_Tick;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.Black;
            mainPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mainPanel.Controls.Add(Date);
            mainPanel.Controls.Add(flowLayoutPanel1);
            mainPanel.Controls.Add(flowClock);
            mainPanel.Controls.Add(pictureBox1);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(800, 401);
            mainPanel.TabIndex = 1;
            mainPanel.Paint += mainPanel_Paint;
            // 
            // Date
            // 
            Date.Anchor = AnchorStyles.Top;
            Date.AutoSize = true;
            Date.BackColor = Color.Transparent;
            Date.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Date.ForeColor = SystemColors.ButtonShadow;
            Date.Location = new Point(560, 95);
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
            flowLayoutPanel1.Location = new Point(488, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(312, 65);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // DateOwLabel
            // 
            DateOwLabel.AutoSize = true;
            DateOwLabel.BackColor = Color.Transparent;
            DateOwLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DateOwLabel.ForeColor = Color.LightGray;
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
            flowClock.Location = new Point(560, 132);
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
            ClockLabel.ForeColor = Color.LightGray;
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
            pictureBox1.Size = new Size(800, 401);
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
            ClientSize = new Size(800, 401);
            Controls.Add(sidebar);
            Controls.Add(mainPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mainForm";
            Text = " FSR3 Mod Setup Utility Enhanced";
            Load += mainPanel_Load;
            Resize += Form1_Resize;
            sidebar.ResumeLayout(false);
            sidebar.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowClock.ResumeLayout(false);
            flowClock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel sidebar;
        private Button buttonSettings;
        private Button buttonGuide;
        private Button buttonExit;
        private PictureBox menuButton;
        private Label returnMainForm;
        private Panel panel1;
        private Panel panel2;
        private Button buttonHome;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private System.Windows.Forms.Timer sidebarTimer;
        private Panel mainPanel;
        private System.Windows.Forms.Timer clockTimer;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowClock;
        private Label ClockLabel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label DateOwLabel;
        private Label Date;
        private FlowLayoutPanel flowLayoutPanel2;
    }
}
