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
            listBox1 = new ListBox();
            button1 = new Button();
            panel4 = new Panel();
            richTextBox1 = new RichTextBox();
            panelImage = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Gray;
            panel1.Controls.Add(listBox1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panelImage);
            panel1.Location = new Point(72, 57);
            panel1.Name = "panel1";
            panel1.Size = new Size(1111, 550);
            panel1.TabIndex = 1;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Silver;
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Items.AddRange(new object[] { "Initial Information", "Add-on Mods", "Optiscaler Method", "Optiscaler FSR 3.1/DLSS", "Achilles Legends Untold", "Alan Wake 2", "Alone in the Dark", "A Plague Tale Requiem", "A Quiet Place: The Road Ahead", "Assassin's Creed Valhalla", "Atomic Heart", "Baldur's Gate 3", "Black Myth: Wukong", "Blacktail", "Banishers Ghost of New Eden", "Bright Memory: Infinite", "Brothers a Tale of Two Sons", "Chernobylite", "Cod Black Ops Cold War", "Cod MW3", "Control", "Crime Boss Rockay City", "Crysis 3 Remastered", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Space Remake", "Dead Island 2", "Death Stranding Director's Cut", "Deathloop", "Dragon Age: Veilguard", "Dragons Dogma 2", "Dying Light 2", "Elden Ring", "Everspace 2", "Evil West", "Fist Forged in Shadow Torch", "Flintlock: The Siege of Dawn", "Fort Solis", "Forza Horizon 5", "Final Fantasy XVI", "F1 2022", "F1 2023", "GTA V", "Ghost of Tsushima", "Ghostrunner 2", "Ghostwire: Tokyo", "God Of War 4", "God of War Ragnarök", "Hellblade: Senua's Sacrifice", "Hellblade 2", "High On Life", "Hitman 3", "Hogwarts Legacy", "Horizon Forbidden West", "Horizon Zero Dawn", "Horizon Zero Dawn Remastered", "Icarus", "Judgment", "Jusant", "Kena: Bridge of Spirits", "Layers of Fear", "Lies of P", "Loopmancer", "Lords of the Fallen", "Manor Lords", "Martha Is Dead", "Marvel's Guardians of the Galaxy", "Metro Exodus Enhanced", "Monster Hunter Rise", "Nobody Wants To Die", "Outpost Infinity Siege", "Pacific Drive", "Palworld", "Ratchet and Clank", "Rise of The Tomb Raider", "Ready or Not", "Red Dead Redemption", "Red Dead Redemption 2", "Returnal", "Ripout", "Saints Row", "Sackboy: A Big Adventure", "Shadow of the Tomb Raider", "Shadow Warrior 3", "Silent Hill 2", "Smalland", "Star Wars Jedi: Survivor", "Star Wars Outlaws", "Test Drive Unlimited Solar Crown", "The Callisto Protocol", "The Casting Of Frank Stone", "Unknown 9: Awakening", "Until Dawn", "Warhammer: Space Marine 2", "Watch Dogs Legion" });
            listBox1.Location = new Point(3, 42);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(177, 92);
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
            panel4.Anchor = AnchorStyles.None;
            panel4.BackColor = Color.DimGray;
            panel4.Controls.Add(richTextBox1);
            panel4.Location = new Point(219, 214);
            panel4.Name = "panel4";
            panel4.Size = new Size(564, 203);
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
            richTextBox1.Size = new Size(564, 203);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // panelImage
            // 
            panelImage.BackColor = Color.Gray;
            panelImage.BackgroundImageLayout = ImageLayout.Stretch;
            panelImage.Location = new Point(288, 37);
            panelImage.Name = "panelImage";
            panelImage.Size = new Size(423, 171);
            panelImage.TabIndex = 3;
            panelImage.Paint += panelImage_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Silver;
            label1.Location = new Point(46, 2);
            label1.Name = "label1";
            label1.Size = new Size(101, 40);
            label1.TabIndex = 2;
            label1.Text = "GUIDE";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.None;
            panel2.Controls.Add(label1);
            panel2.Location = new Point(458, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 48);
            panel2.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(panel1);
            panel3.Controls.Add(panel2);
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1258, 486);
            panel3.TabIndex = 4;
            // 
            // formGuide
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1258, 486);
            Controls.Add(panel3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formGuide";
            Text = "formGuide";
            Load += formGuide_Load;
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
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
    }
}