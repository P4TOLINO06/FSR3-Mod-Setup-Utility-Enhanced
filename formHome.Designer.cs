namespace FSR3ModSetupUtilityEnhanced
{
    partial class formHome
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
            listGames = new ComboBox();
            listFSR = new ComboBox();
            labelSelectGame = new Label();
            panelBG = new Panel();
            panelSelectFSR = new Panel();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panelBG.SuspendLayout();
            panelSelectFSR.SuspendLayout();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listGames
            // 
            listGames.BackColor = Color.Silver;
            listGames.CausesValidation = false;
            listGames.Cursor = Cursors.Hand;
            listGames.FlatStyle = FlatStyle.Flat;
            listGames.FormattingEnabled = true;
            listGames.IntegralHeight = false;
            listGames.Items.AddRange(new object[] { "Select FSR Version", "A Plague Tale Requiem", "A Quiet Place: The Road Ahead", "Achilles Legends Untold", "Alan Wake Remastered", "Alan Wake 2", "Alone in the Dark", "Assassin's Creed Mirage", "Assassin's Creed Valhalla", "Atomic Heart", "Banishers: Ghosts of New Eden", "Black Myth: Wukong", "Blacktail", "Baldur's Gate 3", "Bright Memory: Infinite", "Brothers: A Tale of Two Sons Remake", "Chernobylite", "Cod Black Ops Cold War", "Cod MW3", "Control", "Crime Boss: Rockay City", "Crysis 3 Remastered", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Island 2", "Dead Rising Remaster", "Dead Space (2023)", "Death Stranding Director's Cut", "Deathloop", "Dragon Age: Veilguard", "Dragons Dogma 2", "Dying Light 2", "Elden Ring", "Everspace 2", "Evil West", "Final Fantasy XVI", "F1 2022", "F1 2023", "FIST: Forged In Shadow Torch", "Flintlock: The Siege Of Dawn", "Fort Solis", "Forza Horizon 5", "Ghostrunner 2", "Ghostwire: Tokyo", "Ghost of Tsushima", "God Of War 4", "God of War Ragnarök", "Gotham Knights", "GTA Trilogy", "GTA V", "Hellblade: Senua's Sacrifice", "Hellblade 2", "High On Life", "Hitman 3", "Horizon Forbidden West", "Horizon Zero Dawn\\Remastered", "Hogwarts Legacy", "Icarus", "Indiana Jones and the Great Circle", "Judgment", "Jusant", "Kena: Bridge of Spirits", "Layers of Fear", "Lies of P", "Lego Horizon Adventures", "Lords of the Fallen", "Loopmancer", "Manor Lords", "Martha Is Dead", "Marvel's Avengers", "Marvel's Guardians of the Galaxy", "Marvel's Spider-Man Miles Morales", "Marvel's Spider-Man Remastered", "Marvel's Midnight Suns", "Metro Exodus Enhanced Edition", "Microsoft Flight Simulator 24", "MOTO GP 24", "Monster Hunter Rise", "Mortal Shell", "Nightingale", "Nobody Wants To Die", "Outpost: Infinity Siege", "Pacific Drive", "Palworld", "Path of Exile II", "Ratchet & Clank - Rift Apart", "Ready or Not", "Red Dead Redemption", "Red Dead Redemption 2", "Remnant II", "Resident Evil 4 Remake", "Returnal", "Ripout", "RoboCop: Rogue City", "Saints Row", "Sackboy: A Big Adventure", "Satisfactory", "Scorn", "Sengoku Dynasty", "Shadow Warrior 3", "Shadow of the Tomb Raider", "Sifu", "Silent Hill 2", "Six Days in Fallujah", "Smalland", "Stalker 2", "STAR WARS Jedi: Survivor", "Star Wars Outlaws", "Steelrising", "STARFIELD", "Suicide Squad: Kill the Justice League", "TEKKEN 8", "Test Drive Unlimited Solar Crown", "The Ascent", "The Callisto Protocol", "The Chant", "The Casting Of Frank Stone", "The Invincible", "The Last of Us Part I", "The Medium", "The Outer Worlds: Spacer's Choice Edition", "The Thaumaturge", "The Witcher 3", "Uncharted Legacy of Thieves Collection", "Unknown 9: Awakening", "Until Dawn", "Wanted: Dead", "Warhammer: Space Marine 2", "Watch Dogs Legion", "Way Of The Hunter", "Wayfinder" });
            listGames.Location = new Point(5, 6);
            listGames.Margin = new Padding(5, 6, 5, 6);
            listGames.Name = "listGames";
            listGames.Size = new Size(357, 38);
            listGames.TabIndex = 1;
            listGames.SelectedIndexChanged += buttonSelectGame_Click;
            // 
            // listFSR
            // 
            listFSR.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            listFSR.BackColor = Color.Silver;
            listFSR.Cursor = Cursors.Hand;
            listFSR.FlatStyle = FlatStyle.Flat;
            listFSR.FormattingEnabled = true;
            listFSR.Items.AddRange(new object[] { "SDK", "2.0", "2.1", "2.2", "RDR2" });
            listFSR.Location = new Point(2, 6);
            listFSR.Margin = new Padding(5, 6, 5, 6);
            listFSR.Name = "listFSR";
            listFSR.Size = new Size(86, 38);
            listFSR.TabIndex = 3;
            listFSR.Visible = false;
            listFSR.SelectedIndexChanged += listFSR_SelectedIndexChanged;
            // 
            // labelSelectGame
            // 
            labelSelectGame.BackColor = Color.Black;
            labelSelectGame.FlatStyle = FlatStyle.Flat;
            labelSelectGame.Font = new Font("Lucida Sans Unicode", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSelectGame.ForeColor = Color.Gainsboro;
            labelSelectGame.Location = new Point(5, 0);
            labelSelectGame.Margin = new Padding(5, 0, 5, 0);
            labelSelectGame.Name = "labelSelectGame";
            labelSelectGame.Size = new Size(360, 96);
            labelSelectGame.TabIndex = 2;
            labelSelectGame.Text = "SELECT GAME";
            labelSelectGame.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelBG
            // 
            panelBG.AutoSize = true;
            panelBG.BackColor = Color.Black;
            panelBG.BackgroundImageLayout = ImageLayout.Stretch;
            panelBG.Controls.Add(panelSelectFSR);
            panelBG.Controls.Add(panel1);
            panelBG.Controls.Add(flowLayoutPanel1);
            panelBG.Dock = DockStyle.Fill;
            panelBG.Location = new Point(0, 0);
            panelBG.Margin = new Padding(5, 6, 5, 6);
            panelBG.Name = "panelBG";
            panelBG.Size = new Size(1630, 900);
            panelBG.TabIndex = 2;
            // 
            // panelSelectFSR
            // 
            panelSelectFSR.Anchor = AnchorStyles.Top;
            panelSelectFSR.AutoSize = true;
            panelSelectFSR.BackColor = Color.Transparent;
            panelSelectFSR.Controls.Add(listFSR);
            panelSelectFSR.Location = new Point(1042, 104);
            panelSelectFSR.Margin = new Padding(5, 6, 5, 6);
            panelSelectFSR.Name = "panelSelectFSR";
            panelSelectFSR.Size = new Size(94, 58);
            panelSelectFSR.TabIndex = 6;
            panelSelectFSR.Visible = false;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top;
            panel1.AutoSize = true;
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(listGames);
            panel1.Location = new Point(662, 104);
            panel1.Margin = new Padding(5, 6, 5, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(370, 58);
            panel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = Color.Black;
            flowLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            flowLayoutPanel1.Controls.Add(labelSelectGame);
            flowLayoutPanel1.Location = new Point(662, 6);
            flowLayoutPanel1.Margin = new Padding(5, 6, 5, 6);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(370, 98);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // formHome
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1630, 900);
            Controls.Add(panelBG);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(5, 6, 5, 6);
            Name = "formHome";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "formHome";
            Load += formHome_Load;
            panelBG.ResumeLayout(false);
            panelBG.PerformLayout();
            panelSelectFSR.ResumeLayout(false);
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox listGames;
        private PictureBox pictureBox1;
        private Panel panelBG;
        private Label labelSelectGame;
        private ComboBox listFSR;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private Panel panelSelectFSR;
    }
}