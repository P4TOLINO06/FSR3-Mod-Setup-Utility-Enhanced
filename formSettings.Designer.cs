namespace FSR3ModSetupUtilityEnhanced
{
    partial class formSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formSettings));
            panel1 = new Panel();
            label4 = new Label();
            label3 = new Label();
            flowLayoutPanel3 = new FlowLayoutPanel();
            AddOptionsSelect = new CheckedListBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            buttonInstall = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            label2 = new Label();
            listMods = new ComboBox();
            searchFolder = new FolderBrowserDialog();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel1.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(flowLayoutPanel3);
            panel1.Controls.Add(flowLayoutPanel2);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 450);
            panel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.BackColor = Color.Gray;
            label4.FlatStyle = FlatStyle.Flat;
            label4.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(406, 0);
            label4.Name = "label4";
            label4.Size = new Size(278, 52);
            label4.TabIndex = 7;
            label4.Text = " Additional Settings";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.BackColor = Color.Gray;
            label3.FlatStyle = FlatStyle.Flat;
            label3.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(53, 0);
            label3.Name = "label3";
            label3.Size = new Size(337, 52);
            label3.TabIndex = 6;
            label3.Text = "         Initial settings";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.BackColor = Color.Transparent;
            flowLayoutPanel3.BackgroundImageLayout = ImageLayout.None;
            flowLayoutPanel3.Controls.Add(AddOptionsSelect);
            flowLayoutPanel3.Location = new Point(406, 55);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(278, 77);
            flowLayoutPanel3.TabIndex = 8;
            // 
            // AddOptionsSelect
            // 
            AddOptionsSelect.BackColor = Color.DimGray;
            AddOptionsSelect.BorderStyle = BorderStyle.None;
            AddOptionsSelect.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AddOptionsSelect.ForeColor = Color.White;
            AddOptionsSelect.FormattingEnabled = true;
            AddOptionsSelect.Items.AddRange(new object[] { "Fake Nvidia Gpu", "Nvapi Results", "Ue Compatibility Mode" });
            AddOptionsSelect.Location = new Point(3, 3);
            AddOptionsSelect.Name = "AddOptionsSelect";
            AddOptionsSelect.Size = new Size(179, 66);
            AddOptionsSelect.TabIndex = 6;
            AddOptionsSelect.SelectedIndexChanged += AddOptionsSelect_SelectedIndexChanged;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Bottom;
            flowLayoutPanel2.Controls.Add(buttonInstall);
            flowLayoutPanel2.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel2.Location = new Point(367, 370);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(100, 36);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // buttonInstall
            // 
            buttonInstall.AutoSize = true;
            buttonInstall.BackColor = Color.Transparent;
            buttonInstall.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonInstall.ForeColor = Color.Black;
            buttonInstall.Image = (Image)resources.GetObject("buttonInstall.Image");
            buttonInstall.ImageAlign = ContentAlignment.MiddleLeft;
            buttonInstall.Location = new Point(3, 4);
            buttonInstall.Name = "buttonInstall";
            buttonInstall.Size = new Size(89, 29);
            buttonInstall.TabIndex = 4;
            buttonInstall.Text = "      Install";
            buttonInstall.UseVisualStyleBackColor = false;
            buttonInstall.Click += button2_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(textBox1);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(listMods);
            flowLayoutPanel1.Location = new Point(53, 58);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(337, 74);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Gray;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.ImageAlign = ContentAlignment.BottomLeft;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(128, 27);
            label1.TabIndex = 2;
            label1.Text = "Select Folder";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(137, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(158, 23);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(301, 3);
            button1.Name = "button1";
            button1.Size = new Size(27, 23);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.BackColor = Color.Gray;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(3, 29);
            label2.Name = "label2";
            label2.Size = new Size(128, 27);
            label2.TabIndex = 2;
            label2.Text = "Select Mod";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // listMods
            // 
            listMods.FormattingEnabled = true;
            listMods.Items.AddRange(new object[] { "0.7.4", "0.7.5", "0.7.6", "0.8.0", "0.9.0", "0.10.0", "0.10.1", "0.10.1h1", "0.10.2h1", "0.10.3", "0.10.4", "Uniscaler", "Uniscaler V2", "Uniscaler + Xess + Dlss" });
            listMods.Location = new Point(137, 32);
            listMods.Name = "listMods";
            listMods.Size = new Size(156, 23);
            listMods.TabIndex = 3;
            listMods.SelectedIndexChanged += listMods_SelectedIndexChanged;
            // 
            // formSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            Name = "formSettings";
            Text = "formSettings";
            Load += formSettings_Load;
            panel1.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
        private FolderBrowserDialog searchFolder;
        private ComboBox listMods;
        private Label label2;
        private Button buttonInstall;
        private FlowLayoutPanel flowLayoutPanel2;
        private CheckedListBox AddOptionsSelect;
        private Label label3;
        private Label label4;
        private FlowLayoutPanel flowLayoutPanel3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}