namespace FSR3ModSetupUtilityEnhanced
{
    partial class formGuide
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
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            buttonVideo = new Button();
            listBox1 = new ListBox();
            button1 = new Button();
            panel4 = new Panel();
            richTextBox1 = new RichTextBox();
            panelImage = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            panel5 = new Panel();
            buttonExit = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Gray;
            panel1.Controls.Add(webView21);
            panel1.Controls.Add(buttonVideo);
            panel1.Controls.Add(listBox1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panelImage);
            panel1.Location = new Point(123, 114);
            panel1.Margin = new Padding(5, 6, 5, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(1905, 1100);
            panel1.TabIndex = 1;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(1414, 568);
            webView21.Name = "webView21";
            webView21.Size = new Size(131, 40);
            webView21.TabIndex = 6;
            webView21.ZoomFactor = 1D;
            webView21.NavigationCompleted += WebView21_NavigationCompleted;
            // 
            // buttonVideo
            // 
            buttonVideo.BackColor = Color.Gray;
            buttonVideo.BackgroundImageLayout = ImageLayout.None;
            buttonVideo.Cursor = Cursors.Hand;
            buttonVideo.FlatAppearance.BorderColor = Color.Silver;
            buttonVideo.FlatStyle = FlatStyle.Flat;
            buttonVideo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonVideo.ForeColor = Color.Silver;
            buttonVideo.Location = new Point(1602, 0);
            buttonVideo.Margin = new Padding(5, 6, 5, 6);
            buttonVideo.Name = "buttonVideo";
            buttonVideo.Size = new Size(303, 72);
            buttonVideo.TabIndex = 5;
            buttonVideo.Text = "Video Guide";
            buttonVideo.UseVisualStyleBackColor = false;
            buttonVideo.Visible = false;
            buttonVideo.Click += buttonVideo_Click;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Silver;
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 30;
            listBox1.Items.AddRange(new object[] { "Initial Information", "Add-on Mods", "Optiscaler Method", "Optiscaler FSR 3.1.1/DLSS", "Optiscaler FSR 3.1.3/DLSS (Only Optiscaler)", "Achilles Legends Untold", "Alan Wake Remastered", "Alan Wake 2", "Alone in the Dark", "A Plague Tale Requiem", "A Quiet Place: The Road Ahead", "Assassin's Creed Mirage", "Assassin's Creed Valhalla", "Assetto Corsa Evo", "Atomic Heart", "Back 4 Blood", "Baldur's Gate 3", "Black Myth: Wukong", "Blacktail", "Banishers Ghost of New Eden", "Bright Memory: Infinite", "Brothers a Tale of Two Sons", "Chernobylite", "Cod Black Ops Cold War", "Cod MW3", "Control", "Crime Boss Rockay City", "Crysis 3 Remastered", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Space Remake", "Dead Island 2", "Dead Rising Remaster", "Death Stranding Director's Cut", "Deathloop", "Dragon Age: Veilguard", "Dragons Dogma 2", "Dying Light 2", "Dynasty Warriors: Origins", "Elden Ring", "Empire of the Ants", "Everspace 2", "Evil West", "Fist Forged in Shadow Torch", "Flintlock: The Siege of Dawn", "Fort Solis", "Forza Horizon 5", "Final Fantasy VII Rebirth", "Final Fantasy XVI", "F1 2022", "F1 2023", "Ghost of Tsushima", "Ghostrunner 2", "Ghostwire: Tokyo", "God Of War 4", "God of War Ragnarök", "Gotham Knights", "GTA Trilogy", "GTA V", "Hellblade: Senua's Sacrifice", "Hellblade 2", "High On Life", "Hitman 3", "Hogwarts Legacy", "Horizon Forbidden West", "Horizon Zero Dawn\\Remastered", "Icarus", "Indiana Jones and the Great Circle", "Judgment", "Jusant", "Kena: Bridge of Spirits", "Layers of Fear", "Lies of P", "Lego Horizon Adventures", "Loopmancer", "Lords of the Fallen", "Manor Lords", "Martha Is Dead", "Marvel's Avengers", "Marvel's Guardians of the Galaxy", "Marvel's Midnight Suns", "Metro Exodus Enhanced", "Microsoft Flight Simulator 24", "Monster Hunter Rise", "Mortal Shell", "Nobody Wants To Die", "Outpost Infinity Siege", "Pacific Drive", "Palworld", "Path of Exile II", "Ratchet and Clank", "Rise of The Tomb Raider", "Ready or Not", "Red Dead Redemption", "Red Dead Redemption 2", "Remnant II", "Resident Evil 4 Remake", "Returnal", "Ripout", "Saints Row", "Sackboy: A Big Adventure", "Scorn", "Sengoku Dynasty", "Shadow of the Tomb Raider", "Shadow Warrior 3", "Sifu", "Silent Hill 2", "Six Days in Fallujah", "Smalland", "Spider Man/Miles", "S.T.A.L.K.E.R. 2", "Star Wars Jedi: Survivor", "Star Wars Outlaws", "Steelrising", "Soulslinger Envoy of Death", "Soulstice", "Suicide Squad: Kill the Justice League", "Test Drive Unlimited Solar Crown", "The Ascent", "The Callisto Protocol", "The Casting Of Frank Stone", "The First Berserker: Khazan", "The Last Of Us", "The Witcher 3", "Thymesia", "Uncharted Legacy Of Thieves", "Unknown 9: Awakening", "Until Dawn", "Warhammer: Space Marine 2", "Watch Dogs Legion", "Way Of The Hunter", "Wayfinder" });
            listBox1.Location = new Point(5, 84);
            listBox1.Margin = new Padding(5, 6, 5, 6);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(302, 182);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackColor = Color.Gray;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.Silver;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Silver;
            button1.Location = new Point(5, 6);
            button1.Margin = new Padding(5, 6, 5, 6);
            button1.Name = "button1";
            button1.Size = new Size(303, 72);
            button1.TabIndex = 2;
            button1.Text = "Games";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.None;
            panel4.BackColor = Color.DimGray;
            panel4.Controls.Add(richTextBox1);
            panel4.Location = new Point(375, 428);
            panel4.Margin = new Padding(5, 6, 5, 6);
            panel4.Name = "panel4";
            panel4.Size = new Size(967, 406);
            panel4.TabIndex = 4;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.DimGray;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Font = new Font("Segoe UI Variable Text", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            richTextBox1.ForeColor = Color.Gainsboro;
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Margin = new Padding(5, 6, 5, 6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(967, 406);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // panelImage
            // 
            panelImage.BackColor = Color.Gray;
            panelImage.BackgroundImageLayout = ImageLayout.Stretch;
            panelImage.Location = new Point(494, 74);
            panelImage.Margin = new Padding(5, 6, 5, 6);
            panelImage.Name = "panelImage";
            panelImage.Size = new Size(725, 342);
            panelImage.TabIndex = 3;
            panelImage.Paint += panelImage_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Silver;
            label1.Location = new Point(79, 4);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(179, 68);
            label1.TabIndex = 2;
            label1.Text = "GUIDE";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.None;
            panel2.Controls.Add(label1);
            panel2.Location = new Point(785, 6);
            panel2.Margin = new Padding(5, 6, 5, 6);
            panel2.Name = "panel2";
            panel2.Size = new Size(343, 96);
            panel2.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel1);
            panel3.Controls.Add(panel2);
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(5, 6, 5, 6);
            panel3.Name = "panel3";
            panel3.Size = new Size(2157, 972);
            panel3.TabIndex = 4;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.None;
            panel5.Controls.Add(buttonExit);
            panel5.Location = new Point(1725, 18);
            panel5.Name = "panel5";
            panel5.Size = new Size(308, 87);
            panel5.TabIndex = 4;
            // 
            // buttonExit
            // 
            buttonExit.BackColor = Color.Gray;
            buttonExit.BackgroundImageLayout = ImageLayout.None;
            buttonExit.Cursor = Cursors.Hand;
            buttonExit.FlatAppearance.BorderColor = Color.Silver;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonExit.ForeColor = Color.Silver;
            buttonExit.Location = new Point(0, 6);
            buttonExit.Margin = new Padding(5, 6, 5, 6);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(303, 72);
            buttonExit.TabIndex = 6;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = false;
            buttonExit.Visible = false;
            buttonExit.Click += buttonExit_Click;
            // 
            // formGuide
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(2157, 972);
            Controls.Add(panel3);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(5, 6, 5, 6);
            Name = "formGuide";
            Text = "formGuide";
            Load += formGuide_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private ListBox listBox1;
        private Button button1;
        private Panel panel2;
        private Panel panel4;
        private Panel panelImage;
        private Panel panel3;
        private RichTextBox richTextBox1;
        private Button buttonVideo;
        private Panel panel5;
        private Button buttonExit;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}