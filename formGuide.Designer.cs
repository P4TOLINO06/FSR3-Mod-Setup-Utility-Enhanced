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
            panel2 = new Panel();
            label1 = new Label();
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
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Gray;
            panel1.Controls.Add(webView21);
            panel1.Controls.Add(buttonVideo);
            panel1.Controls.Add(listBox1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panelImage);
            panel1.Location = new Point(82, 72);
            panel1.Name = "panel1";
            panel1.Size = new Size(1120, 574);
            panel1.TabIndex = 1;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(979, 229);
            webView21.Margin = new Padding(2);
            webView21.Name = "webView21";
            webView21.Size = new Size(76, 20);
            webView21.TabIndex = 6;
            webView21.ZoomFactor = 1D;
            webView21.NavigationCompleted += WebView21_NavigationCompleted;
            // 
            // buttonVideo
            // 
            buttonVideo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonVideo.AutoSize = true;
            buttonVideo.BackColor = Color.Gray;
            buttonVideo.BackgroundImageLayout = ImageLayout.None;
            buttonVideo.Cursor = Cursors.Hand;
            buttonVideo.FlatAppearance.BorderColor = Color.Silver;
            buttonVideo.FlatStyle = FlatStyle.Flat;
            buttonVideo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonVideo.ForeColor = Color.Silver;
            buttonVideo.Location = new Point(943, 3);
            buttonVideo.Name = "buttonVideo";
            buttonVideo.Size = new Size(177, 36);
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
            listBox1.ItemHeight = 15;
            listBox1.Items.AddRange(new object[] { "Initial Information", "Add-on Mods", "Optiscaler Method", "Optiscaler FSR 3.1.1/DLSS", "Optiscaler FSR 3.1.3/DLSS (Only Optiscaler)", "Achilles Legends Untold", "Alan Wake Remastered", "Alan Wake 2", "Alone in the Dark", "A Plague Tale Requiem", "A Quiet Place: The Road Ahead", "Assassin's Creed Mirage", "Assassin's Creed Shadows", "Assassin's Creed Valhalla", "Assetto Corsa Evo", "Atomic Heart", "AVOWED", "Back 4 Blood", "Baldur's Gate 3", "Black Myth: Wukong", "Blacktail", "Banishers Ghost of New Eden", "Bright Memory", "Bright Memory Infinite", "Brothers: A Tale of Two Sons", "Chernobylite", "Chernobylite 2: Exclusion Zone", "Cities: Skylines 2", "Choo-Choo Charles", "Chorus", "Clair Obscur: Expedition 33", "Cod Black Ops Cold War", "Cod MW3", "Control", "Crime Boss Rockay City", "Crysis Remastered", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Space (2023)", "Dead Island 2", "Dead Rising Remaster", "Death Stranding", "Deathloop", "Deliver Us Mars", "Deliver Us The Moon", "Dragon Age: Veilguard", "Dragons Dogma 2", "Dying Light 2", "Dynasty Warriors: Origins", "Elden Ring", "Empire of the Ants", "Everspace 2", "Evil West", "Eternal Strands", "Fist Forged in Shadow Torch", "Flintlock: The Siege of Dawn", "Fobia – St. Dinfna Hotel", "Fort Solis", "Forza Horizon 5", "Final Fantasy VII Rebirth", "Final Fantasy XVI", "Five Nights at Freddy’s: Security Breach", "Frostpunk 2", "F1 22", "F1 23", "Ghost of Tsushima", "Ghostrunner 2", "Ghostwire: Tokyo", "God Of War", "God Of War Ragnarök", "Gotham Knights", "GreedFall II: The Dying World", "GTA Trilogy", "Grand Theft Auto V", "Hellblade: Senua's Sacrifice", "High On Life", "Hitman 3", "Hogwarts Legacy", "Horizon Forbidden West", "Horizon Zero Dawn\\Remastered", "Hot Wheels Unleashed", "Icarus", "Indiana Jones and the Great Circle", "Judgment", "Jusant", "Kingdom Come: Deliverance 2", "Kena: Bridge of Spirits", "Layers of Fear", "Lies of P", "Like a Dragon: Pirates", "Lego Horizon Adventures", "Loopmancer", "Lords of the Fallen", "Lost Records Bloom And Rage", "Manor Lords", "Martha Is Dead", "Marvel's Avengers", "Marvel's Guardians of the Galaxy", "Marvel's Midnight Suns", "Metro Exodus", "Microsoft Flight Simulator 24", "Monster Hunter Rise", "Monster Hunter Wilds", "Mortal Shell", "Ninja Gaiden 2 Black", "Nobody Wants To Die", "Orcs Must Die! Deathtrap", "Outpost Infinity Siege", "Pacific Drive", "Palworld", "Path of Exile II", "Ratchet & Clank Rift Apart", "Rise of The Tomb Raider", "Ready or Not", "Red Dead Redemption", "Red Dead Redemption 2", "Remnant II", "Resident Evil 4", "Returnal", "Ripout", "Saints Row", "Sackboy: A Big Adventure", "Scorn", "Sengoku Dynasty", "Senua's Saga Hellblade II", "Shadow of the Tomb Raider", "Shadow Warrior 3", "Sifu", "Silent Hill 2", "Six Days in Fallujah", "Smalland", "Spider/Miles", "Stalker 2", "Star Wars: Jedi Survivor", "Star Wars Outlaws", "Steelrising", "Soulslinger Envoy of Death", "Soulstice", "South of Midnight", "Suicide Squad: Kill the Justice League", "Test Drive Unlimited Solar Crown", "The Ascent", "The Callisto Protocol", "The Casting Of Frank Stone", "The Elder Scrolls IV: Oblivion Remastered", "The First Berserker: Khazan", "The Last of Us Part I", "The Last of Us Part II", "The Outlast Trials", "The Talos Principle 2", "The Witcher 3", "Thymesia", "Uncharted Legacy Of Thieves", "Unknown 9: Awakening", "Until Dawn", "Warhammer 40.000: Space Marine 2", "Wanted Dead", "Watch Dogs Legion", "Way Of The Hunter", "Wayfinder", "WILD HEARTS" });
            listBox1.Location = new Point(3, 37);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(179, 92);
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
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(177, 36);
            button1.TabIndex = 2;
            button1.Text = "Games";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = Color.DimGray;
            panel4.Controls.Add(richTextBox1);
            panel4.Location = new Point(237, 243);
            panel4.MinimumSize = new Size(300, 150);
            panel4.Name = "panel4";
            panel4.Size = new Size(564, 200);
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
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(564, 200);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // panelImage
            // 
            panelImage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelImage.BackColor = Color.Gray;
            panelImage.BackgroundImageLayout = ImageLayout.Stretch;
            panelImage.Location = new Point(293, 37);
            panelImage.MinimumSize = new Size(223, 100);
            panelImage.Name = "panelImage";
            panelImage.Size = new Size(423, 200);
            panelImage.TabIndex = 3;
            panelImage.Paint += panelImage_Paint;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Location = new Point(458, 18);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 48);
            panel2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Silver;
            label1.Location = new Point(46, 5);
            label1.Name = "label1";
            label1.Size = new Size(101, 40);
            label1.TabIndex = 2;
            label1.Text = "GUIDE";
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel1);
            panel3.Controls.Add(panel2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1284, 658);
            panel3.TabIndex = 4;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel5.Controls.Add(buttonExit);
            panel5.Location = new Point(1018, 27);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(184, 36);
            panel5.TabIndex = 4;
            // 
            // buttonExit
            // 
            buttonExit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            buttonExit.BackColor = Color.Gray;
            buttonExit.BackgroundImageLayout = ImageLayout.None;
            buttonExit.Cursor = Cursors.Hand;
            buttonExit.FlatAppearance.BorderColor = Color.Silver;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonExit.ForeColor = Color.Silver;
            buttonExit.Location = new Point(3, 0);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(178, 36);
            buttonExit.TabIndex = 6;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = false;
            buttonExit.Visible = false;
            buttonExit.Click += buttonExit_Click;
            // 
            // formGuide
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.DimGray;
            ClientSize = new Size(1284, 658);
            Controls.Add(panel3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formGuide";
            Text = "formGuide";
            Load += formGuide_Load;
            Resize += formGuide_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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