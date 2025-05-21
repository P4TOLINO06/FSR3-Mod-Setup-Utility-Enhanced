namespace FSR3ModSetupUtilityEnhanced
{
    partial class formLibrary
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formLibrary));
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panelGames = new Panel();
            btnLibrarySettings = new Button();
            labelGamesCount = new Label();
            labelGames = new Label();
            panel1 = new Panel();
            panelLine = new Panel();
            panel3 = new Panel();
            panel5 = new Panel();
            panel4 = new Panel();
            panel = new Panel();
            panel13 = new Panel();
            checkBoxDlssOverlay = new CheckBox();
            labelDlssOverlayValue = new Label();
            labelDlssOveray = new Label();
            panel12 = new Panel();
            checkBoxPreset = new CheckBox();
            panel8 = new Panel();
            panel10 = new Panel();
            panel11 = new Panel();
            btnSettings = new Button();
            btnGuide = new Button();
            labelCheckGuide = new Label();
            panel9 = new Panel();
            label2 = new Label();
            labelFolderName = new Label();
            labelOnSig = new Label();
            labelModName = new Label();
            labelFolder = new Label();
            labelSignatureOver = new Label();
            labelMod = new Label();
            panel6 = new Panel();
            labelPreset = new Label();
            panel2 = new Panel();
            panelIconGame = new Panel();
            btnGameMenu = new Button();
            picGameIcon = new PictureBox();
            labelGameIcon = new Label();
            panel7 = new Panel();
            panelIcon = new FlowLayoutPanel();
            labelLibrary = new Label();
            toolTip1 = new ToolTip(components);
            panelGames.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel.SuspendLayout();
            panel6.SuspendLayout();
            panelIconGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGameIcon).BeginInit();
            SuspendLayout();
            // 
            // panelGames
            // 
            panelGames.Anchor = AnchorStyles.None;
            panelGames.BackColor = SystemColors.ControlDarkDark;
            panelGames.Controls.Add(btnLibrarySettings);
            panelGames.Controls.Add(labelGamesCount);
            panelGames.Controls.Add(labelGames);
            panelGames.Location = new Point(0, 44);
            panelGames.MaximumSize = new Size(232, 56);
            panelGames.Name = "panelGames";
            panelGames.Size = new Size(232, 56);
            panelGames.TabIndex = 1;
            // 
            // btnLibrarySettings
            // 
            btnLibrarySettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLibrarySettings.BackgroundImage = (Image)resources.GetObject("btnLibrarySettings.BackgroundImage");
            btnLibrarySettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnLibrarySettings.Cursor = Cursors.Hand;
            btnLibrarySettings.FlatAppearance.BorderSize = 0;
            btnLibrarySettings.FlatStyle = FlatStyle.Flat;
            btnLibrarySettings.Location = new Point(213, 6);
            btnLibrarySettings.Name = "btnLibrarySettings";
            btnLibrarySettings.Size = new Size(19, 44);
            btnLibrarySettings.TabIndex = 6;
            toolTip1.SetToolTip(btnLibrarySettings, "Library Settings");
            btnLibrarySettings.UseVisualStyleBackColor = true;
            btnLibrarySettings.Click += btnLibrarySettings_Click;
            // 
            // labelGamesCount
            // 
            labelGamesCount.Anchor = AnchorStyles.Top;
            labelGamesCount.AutoSize = true;
            labelGamesCount.BackColor = Color.Transparent;
            labelGamesCount.FlatStyle = FlatStyle.Flat;
            labelGamesCount.Font = new Font("Segoe UI Semibold", 14.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelGamesCount.ForeColor = Color.White;
            labelGamesCount.Location = new Point(12, 3);
            labelGamesCount.Name = "labelGamesCount";
            labelGamesCount.Size = new Size(80, 25);
            labelGamesCount.TabIndex = 1;
            labelGamesCount.Text = "Games: ";
            labelGamesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelGames
            // 
            labelGames.Anchor = AnchorStyles.None;
            labelGames.AutoSize = true;
            labelGames.BackColor = Color.Transparent;
            labelGames.FlatStyle = FlatStyle.Flat;
            labelGames.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelGames.ForeColor = Color.White;
            labelGames.Location = new Point(12, 28);
            labelGames.Name = "labelGames";
            labelGames.Size = new Size(70, 25);
            labelGames.TabIndex = 0;
            labelGames.Text = "Games";
            labelGames.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.BackColor = Color.DarkGray;
            panel1.Controls.Add(panelLine);
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1125, 450);
            panel1.TabIndex = 2;
            // 
            // panelLine
            // 
            panelLine.BackColor = Color.Black;
            panelLine.Location = new Point(0, 100);
            panelLine.Name = "panelLine";
            panelLine.Size = new Size(236, 2);
            panelLine.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.BackColor = Color.DimGray;
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(panel7);
            panel3.Controls.Add(panelGames);
            panel3.Controls.Add(panelIcon);
            panel3.Controls.Add(labelLibrary);
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1125, 450);
            panel3.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BackColor = Color.Black;
            panel5.Location = new Point(233, 44);
            panel5.Name = "panel5";
            panel5.Size = new Size(2, 58);
            panel5.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = SystemColors.WindowFrame;
            panel4.Controls.Add(panel);
            panel4.Controls.Add(panel6);
            panel4.Controls.Add(panel2);
            panel4.Controls.Add(panelIconGame);
            panel4.Location = new Point(235, 44);
            panel4.Name = "panel4";
            panel4.Size = new Size(884, 406);
            panel4.TabIndex = 0;
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = Color.DimGray;
            panel.Controls.Add(panel13);
            panel.Controls.Add(checkBoxDlssOverlay);
            panel.Controls.Add(labelDlssOverlayValue);
            panel.Controls.Add(labelDlssOveray);
            panel.Controls.Add(panel12);
            panel.Controls.Add(checkBoxPreset);
            panel.Controls.Add(panel8);
            panel.Controls.Add(panel10);
            panel.Controls.Add(panel11);
            panel.Controls.Add(btnSettings);
            panel.Controls.Add(btnGuide);
            panel.Controls.Add(labelCheckGuide);
            panel.Controls.Add(panel9);
            panel.Controls.Add(label2);
            panel.Controls.Add(labelFolderName);
            panel.Controls.Add(labelOnSig);
            panel.Controls.Add(labelModName);
            panel.Controls.Add(labelFolder);
            panel.Controls.Add(labelSignatureOver);
            panel.Controls.Add(labelMod);
            panel.Location = new Point(5, 120);
            panel.Name = "panel";
            panel.Size = new Size(875, 170);
            panel.TabIndex = 4;
            // 
            // panel13
            // 
            panel13.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel13.BackColor = Color.Gray;
            panel13.Location = new Point(1, 165);
            panel13.Name = "panel13";
            panel13.Size = new Size(875, 2);
            panel13.TabIndex = 9;
            // 
            // checkBoxDlssOverlay
            // 
            checkBoxDlssOverlay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBoxDlssOverlay.AutoSize = true;
            checkBoxDlssOverlay.Cursor = Cursors.Hand;
            checkBoxDlssOverlay.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBoxDlssOverlay.ForeColor = Color.White;
            checkBoxDlssOverlay.Location = new Point(765, 76);
            checkBoxDlssOverlay.Name = "checkBoxDlssOverlay";
            checkBoxDlssOverlay.Size = new Size(110, 23);
            checkBoxDlssOverlay.TabIndex = 21;
            checkBoxDlssOverlay.Text = "DLSS Overlay";
            toolTip1.SetToolTip(checkBoxDlssOverlay, "Recommended if you want to check whether DLSS 4 is enabled (disabled by default; when checked, it adds the Overlay to the Preset)");
            checkBoxDlssOverlay.UseVisualStyleBackColor = true;
            checkBoxDlssOverlay.CheckedChanged += checkBoxDlssOverlay_CheckedChanged;
            // 
            // labelDlssOverlayValue
            // 
            labelDlssOverlayValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelDlssOverlayValue.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDlssOverlayValue.ForeColor = Color.White;
            labelDlssOverlayValue.Location = new Point(208, 77);
            labelDlssOverlayValue.Name = "labelDlssOverlayValue";
            labelDlssOverlayValue.Size = new Size(520, 19);
            labelDlssOverlayValue.TabIndex = 20;
            labelDlssOverlayValue.Text = "Recommended if you want to check whether DLSS 4 is enabled (disabled by default)";
            // 
            // labelDlssOveray
            // 
            labelDlssOveray.AutoSize = true;
            labelDlssOveray.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDlssOveray.ForeColor = SystemColors.ButtonFace;
            labelDlssOveray.Location = new Point(3, 72);
            labelDlssOveray.Name = "labelDlssOveray";
            labelDlssOveray.Size = new Size(123, 25);
            labelDlssOveray.TabIndex = 19;
            labelDlssOveray.Text = "DLSS Overlay";
            // 
            // panel12
            // 
            panel12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel12.BackColor = Color.Black;
            panel12.Location = new Point(702, 0);
            panel12.Name = "panel12";
            panel12.Size = new Size(2, 33);
            panel12.TabIndex = 18;
            // 
            // checkBoxPreset
            // 
            checkBoxPreset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBoxPreset.AutoSize = true;
            checkBoxPreset.Cursor = Cursors.Hand;
            checkBoxPreset.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBoxPreset.ForeColor = Color.White;
            checkBoxPreset.Location = new Point(609, 9);
            checkBoxPreset.Name = "checkBoxPreset";
            checkBoxPreset.Size = new Size(95, 23);
            checkBoxPreset.TabIndex = 17;
            checkBoxPreset.Text = "Add Preset";
            toolTip1.SetToolTip(checkBoxPreset, "Add the recommended Preset to the Settings section");
            checkBoxPreset.UseVisualStyleBackColor = true;
            checkBoxPreset.CheckedChanged += checkBoxPreset_CheckedChanged;
            // 
            // panel8
            // 
            panel8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel8.BackColor = Color.Gray;
            panel8.Location = new Point(1, 132);
            panel8.Name = "panel8";
            panel8.Size = new Size(878, 2);
            panel8.TabIndex = 8;
            // 
            // panel10
            // 
            panel10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel10.BackColor = Color.Gray;
            panel10.Location = new Point(0, 66);
            panel10.Name = "panel10";
            panel10.Size = new Size(878, 2);
            panel10.TabIndex = 6;
            // 
            // panel11
            // 
            panel11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel11.BackColor = Color.Gray;
            panel11.Location = new Point(0, 99);
            panel11.Name = "panel11";
            panel11.Size = new Size(878, 2);
            panel11.TabIndex = 13;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(710, 10);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(27, 21);
            btnSettings.TabIndex = 16;
            toolTip1.SetToolTip(btnSettings, "Go to the Settings");
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnGuide
            // 
            btnGuide.BackgroundImage = (Image)resources.GetObject("btnGuide.BackgroundImage");
            btnGuide.BackgroundImageLayout = ImageLayout.Stretch;
            btnGuide.Cursor = Cursors.Hand;
            btnGuide.FlatAppearance.BorderSize = 0;
            btnGuide.FlatStyle = FlatStyle.Flat;
            btnGuide.ForeColor = Color.Transparent;
            btnGuide.Location = new Point(710, 142);
            btnGuide.Name = "btnGuide";
            btnGuide.Size = new Size(27, 21);
            btnGuide.TabIndex = 15;
            toolTip1.SetToolTip(btnGuide, "Go to the Guide");
            btnGuide.UseVisualStyleBackColor = true;
            btnGuide.Click += btnGuide_Click;
            // 
            // labelCheckGuide
            // 
            labelCheckGuide.AutoSize = true;
            labelCheckGuide.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelCheckGuide.ForeColor = Color.White;
            labelCheckGuide.Location = new Point(208, 143);
            labelCheckGuide.Name = "labelCheckGuide";
            labelCheckGuide.Size = new Size(110, 19);
            labelCheckGuide.TabIndex = 14;
            labelCheckGuide.Text = "labelCheckGuide";
            // 
            // panel9
            // 
            panel9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel9.BackColor = Color.Gray;
            panel9.Location = new Point(3, 33);
            panel9.Name = "panel9";
            panel9.Size = new Size(878, 2);
            panel9.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(3, 138);
            label2.Name = "label2";
            label2.Size = new Size(62, 25);
            label2.TabIndex = 12;
            label2.Text = "Guide";
            // 
            // labelFolderName
            // 
            labelFolderName.AutoSize = true;
            labelFolderName.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelFolderName.ForeColor = Color.White;
            labelFolderName.Location = new Point(208, 110);
            labelFolderName.Name = "labelFolderName";
            labelFolderName.Size = new Size(45, 19);
            labelFolderName.TabIndex = 11;
            labelFolderName.Text = "label3";
            // 
            // labelOnSig
            // 
            labelOnSig.AutoSize = true;
            labelOnSig.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelOnSig.ForeColor = Color.White;
            labelOnSig.Location = new Point(208, 44);
            labelOnSig.Name = "labelOnSig";
            labelOnSig.Size = new Size(45, 19);
            labelOnSig.TabIndex = 10;
            labelOnSig.Text = "label2";
            // 
            // labelModName
            // 
            labelModName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelModName.AutoSize = true;
            labelModName.Font = new Font("Segoe UI", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelModName.ForeColor = Color.White;
            labelModName.Location = new Point(208, 11);
            labelModName.Name = "labelModName";
            labelModName.Size = new Size(45, 19);
            labelModName.TabIndex = 9;
            labelModName.Text = "label1";
            // 
            // labelFolder
            // 
            labelFolder.AutoSize = true;
            labelFolder.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelFolder.ForeColor = SystemColors.ButtonFace;
            labelFolder.Location = new Point(3, 105);
            labelFolder.Name = "labelFolder";
            labelFolder.Size = new Size(160, 25);
            labelFolder.TabIndex = 7;
            labelFolder.Text = "Installation folder";
            // 
            // labelSignatureOver
            // 
            labelSignatureOver.AutoSize = true;
            labelSignatureOver.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelSignatureOver.ForeColor = SystemColors.ButtonFace;
            labelSignatureOver.Location = new Point(3, 39);
            labelSignatureOver.Name = "labelSignatureOver";
            labelSignatureOver.Size = new Size(155, 25);
            labelSignatureOver.TabIndex = 5;
            labelSignatureOver.Text = "Enable Signature";
            // 
            // labelMod
            // 
            labelMod.AutoSize = true;
            labelMod.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMod.ForeColor = SystemColors.ButtonFace;
            labelMod.Location = new Point(3, 6);
            labelMod.Name = "labelMod";
            labelMod.Size = new Size(51, 25);
            labelMod.TabIndex = 0;
            labelMod.Text = "Mod";
            // 
            // panel6
            // 
            panel6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel6.BackColor = SystemColors.ControlDarkDark;
            panel6.Controls.Add(labelPreset);
            panel6.Location = new Point(3, 58);
            panel6.Name = "panel6";
            panel6.Size = new Size(881, 56);
            panel6.TabIndex = 3;
            // 
            // labelPreset
            // 
            labelPreset.AutoSize = true;
            labelPreset.Font = new Font("Lucida Sans Unicode", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPreset.ForeColor = Color.LightGray;
            labelPreset.Location = new Point(178, 19);
            labelPreset.Name = "labelPreset";
            labelPreset.Size = new Size(278, 23);
            labelPreset.TabIndex = 0;
            labelPreset.Text = "Recommended mod settings";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.Black;
            panel2.Location = new Point(0, 56);
            panel2.Name = "panel2";
            panel2.Size = new Size(1125, 2);
            panel2.TabIndex = 2;
            // 
            // panelIconGame
            // 
            panelIconGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelIconGame.BackColor = SystemColors.WindowFrame;
            panelIconGame.Controls.Add(btnGameMenu);
            panelIconGame.Controls.Add(picGameIcon);
            panelIconGame.Controls.Add(labelGameIcon);
            panelIconGame.Location = new Point(0, 0);
            panelIconGame.Name = "panelIconGame";
            panelIconGame.Size = new Size(884, 56);
            panelIconGame.TabIndex = 1;
            // 
            // btnGameMenu
            // 
            btnGameMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGameMenu.BackgroundImage = (Image)resources.GetObject("btnGameMenu.BackgroundImage");
            btnGameMenu.BackgroundImageLayout = ImageLayout.Stretch;
            btnGameMenu.Cursor = Cursors.Hand;
            btnGameMenu.FlatAppearance.BorderSize = 0;
            btnGameMenu.FlatStyle = FlatStyle.Flat;
            btnGameMenu.Location = new Point(865, 9);
            btnGameMenu.Name = "btnGameMenu";
            btnGameMenu.Size = new Size(19, 44);
            btnGameMenu.TabIndex = 5;
            toolTip1.SetToolTip(btnGameMenu, "Game Menu");
            btnGameMenu.UseVisualStyleBackColor = true;
            btnGameMenu.Click += btnGameMenu_Click;
            // 
            // picGameIcon
            // 
            picGameIcon.BackColor = Color.Transparent;
            picGameIcon.Location = new Point(3, 3);
            picGameIcon.Name = "picGameIcon";
            picGameIcon.Size = new Size(51, 51);
            picGameIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picGameIcon.TabIndex = 1;
            picGameIcon.TabStop = false;
            // 
            // labelGameIcon
            // 
            labelGameIcon.AutoSize = true;
            labelGameIcon.Font = new Font("Lucida Sans Unicode", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGameIcon.ForeColor = Color.LightGray;
            labelGameIcon.Location = new Point(60, 14);
            labelGameIcon.Name = "labelGameIcon";
            labelGameIcon.Size = new Size(58, 20);
            labelGameIcon.TabIndex = 0;
            labelGameIcon.Text = "label1";
            // 
            // panel7
            // 
            panel7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel7.BackColor = Color.Black;
            panel7.Location = new Point(233, 100);
            panel7.Name = "panel7";
            panel7.Size = new Size(2, 350);
            panel7.TabIndex = 3;
            // 
            // panelIcon
            // 
            panelIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panelIcon.AutoScroll = true;
            panelIcon.BackColor = Color.DimGray;
            panelIcon.FlowDirection = FlowDirection.TopDown;
            panelIcon.Location = new Point(2, 103);
            panelIcon.Name = "panelIcon";
            panelIcon.Padding = new Padding(50, 0, 0, 0);
            panelIcon.Size = new Size(230, 347);
            panelIcon.TabIndex = 2;
            panelIcon.WrapContents = false;
            // 
            // labelLibrary
            // 
            labelLibrary.BackColor = Color.Transparent;
            labelLibrary.FlatStyle = FlatStyle.Flat;
            labelLibrary.Font = new Font("Segoe UI", 21.8571434F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelLibrary.ForeColor = Color.White;
            labelLibrary.Location = new Point(454, 0);
            labelLibrary.Name = "labelLibrary";
            labelLibrary.Size = new Size(107, 41);
            labelLibrary.TabIndex = 0;
            labelLibrary.Text = "Library";
            labelLibrary.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // formLibrary
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1125, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formLibrary";
            Text = "formLibrary";
            Load += formLibrary_Load;
            Resize += formLibrary_Resize;
            panelGames.ResumeLayout(false);
            panelGames.PerformLayout();
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panelIconGame.ResumeLayout(false);
            panelIconGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picGameIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panelGames;
        private Label labelGames;
        private Panel panel1;
        private Label labelGamesCount;
        private ToolTip toolTip1;
        private Panel panel3;
        private Label labelLibrary;
        private Panel panel4;
        private Panel panel;
        private Panel panel12;
        private CheckBox checkBoxPreset;
        private Button btnSettings;
        private Button btnGuide;
        private Panel panel10;
        private Panel panel11;
        private Label labelCheckGuide;
        private Label label2;
        private Panel panel8;
        private Label labelFolderName;
        private Label labelOnSig;
        private Label labelModName;
        private Label labelFolder;
        private Label labelSignatureOver;
        private Panel panel9;
        private Label labelMod;
        private Panel panel7;
        private Panel panel6;
        private Label labelPreset;
        private Panel panel2;
        private Panel panelIconGame;
        private Panel panel5;
        private Button btnGameMenu;
        private PictureBox picGameIcon;
        private Label labelGameIcon;
        private Panel panelLine;
        private FlowLayoutPanel panelIcon;
        private Label labelDlssOveray;
        private Label labelDlssOverlayValue;
        private CheckBox checkBoxDlssOverlay;
        private Panel panel13;
        private Button btnLibrarySettings;
    }
}