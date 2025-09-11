using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class mainForm : Form
    {
        bool sidebar_Expand;
        private bool showSettingsSubMenu = false;
        private formHome homeForm;
        public formHome FormHomeInstance { get; private set; }
        private formSettings homeSettings;
        public formSettings HomeSettings => homeSettings;
        private formLibrary homeLibrary;
        private formGuide homeGuide;
        private formEditorToml homeEditor;
        public string GpuName;
        private static mainForm instance;
        private bool versionAvailable;
        private Button btnNewVersion;

        public string valueForSettings;
        public List<string> pendingModsForSettings { get; set; }

        public string valueListModsForSettings;
        public bool? valueSigForSettings;
        public bool valueDlssOverlayForSettings;
        public bool flagValueTextBox1Settings;
        public string nullHomeEditor { get; set; }
        public List<string> gamesLibrary = new List<string>();

        public mainForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pictureBox1, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, sidebar, new object[] { true });

            ScanGames(GetGameNames(), StorageHelper.GetGameDirectories().ToList());

        }

        public void ScanGames(List<string> gameNames = null, List<string> possiblePaths = null)
        {
            if (gameNames == null)
            {
                gameNames = GetGameNames();
            }

            if (possiblePaths == null)
            {
                possiblePaths = StorageHelper.GetGameDirectories().ToList();
            }

            HashSet<string> exactMatchGames = new HashSet<string>
            {
                "God Of War",
                "God Of War Ragnarök",
                "Red Dead Redemption",
                "Red Dead Redemption 2",
                "The Last of Us Part I",
                "The Last of Us Part II",
            };

            HashSet<string> eqMatchGames = new HashSet<string> // Games that have the same root folder name (e.g., Lords Of The Fallen and Lords of the Fallen (<- LOTF 2023))
            {
                "Lords of the Fallen"
            };

            foreach (var basePath in possiblePaths)
            {
                foreach (var dir in Directory.GetDirectories(basePath))
                {
                    string folderName = Path.GetFileName(dir); 
                    string normalizedFolder = StringHelper.NormalizeString(folderName);

                    List<string> matchedGames = gameNames
                        .Where(gameName =>
                        {
                            if (exactMatchGames.Contains(gameName))
                                return StringHelper.NormalizeString(gameName) == normalizedFolder;

                            if (eqMatchGames.Contains(gameName))
                            {
                                return gameName == folderName;
                            }

                            return gameName.Split(' ').All(word =>
                                normalizedFolder.Contains(StringHelper.NormalizeString(word)));
                        })
                        .ToList();

                    if (matchedGames.Count > 0)
                    {
                        long size = FileHelper.GetDirectorySize(dir);
                        long folderSizeGB = 3L * 1024 * 1024 * 1024 / 2;
                        if (size > folderSizeGB)
                        {
                            string exePath = FileHelper.FindGameExe(dir);
                            if (!string.IsNullOrEmpty(exePath))
                            {
                                foreach (var gameName in matchedGames)
                                {
                                    gamesLibrary.Add($"Game: {gameName}");
                                    gamesLibrary.Add($"Exe Path: {exePath}");
                                    gamesLibrary.Add("");
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void CheckGamesInDirectories(StreamWriter writer)
        {
            List<string> gameNames = GetGameNames();
            string[] possiblePaths = StorageHelper.GetGameDirectories();

            List<string> addedGames = new List<string>(); // Avoids duplicate games

            // Games that require exact comparison
            List<string> exactMatchGames = new List<string> { "GodOfWar", "God Of War Ragnarök" };

            foreach (var basePath in possiblePaths)
            {
                foreach (var dir in Directory.GetDirectories(basePath))
                {
                    string folderName = Path.GetFileName(dir);
                    string normalizedFolder = StringHelper.NormalizeString(folderName);

                    foreach (var gameName in gameNames)
                    {
                        string normalizedGameName = StringHelper.NormalizeString(gameName);

                        bool isMatch;
                        if (exactMatchGames.Contains(gameName))
                        {
                            isMatch = normalizedFolder == normalizedGameName;
                        }
                        else
                        {
                            isMatch = gameName.Split(' ').All(word => normalizedFolder.Contains(StringHelper.NormalizeString(word)));
                        }

                        if (isMatch && !addedGames.Contains(gameName))
                        {
                            long size = FileHelper.GetDirectorySize(dir);
                            if (size > 500 * 1024 * 1024)
                            {
                                string exePath = FileHelper.FindGameExe(dir);
                                if (!string.IsNullOrEmpty(exePath))
                                {
                                    writer.WriteLine($"Game: {folderName}");
                                    writer.WriteLine($"Exe Path: {exePath}");
                                    writer.WriteLine();

                                    addedGames.Add(gameName);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static class StringHelper
        {
            public static string NormalizeString(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return string.Empty;

                string nrmString = input.Normalize(NormalizationForm.FormD);
                nrmString = new string(nrmString
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray());

                nrmString = Regex.Replace(nrmString, @"[^a-zA-Z0-9]", "");

                return nrmString.ToLowerInvariant();
            }
        }

        public static List<string> GetGameNames()
        {
            #region Root folder name
            return new List<string>
            {
                "Elden Ring", "Elden Ring Nightreign","Red Dead Redemption 2", "Grand Theft Auto V",
                "Lies of P", "Watch Dogs Legion", "Bright Memory", "Fobia – St. Dinfna Hotel",
                "A Plague Tale Requiem", "Alan Wake Remastered", "Horizon Zero Dawn",
                "Bright Memory Infinite", "A Quiet Place: The Road Ahead", "Assassin\'s Creed Mirage",
                "Assetto Corsa Evo","Assassin's Creed Shadows", "Assassin's Creed Valhalla", "Atomic Heart",
                "AVOWED", "Back 4 Blood", "Baldur's Gate 3", "Banishers Ghost of New Eden","Black Myth: Wukong",
                "Blacktail", "Brothers a Tale of Two Sons", "Chernobylite",
                "Choo-Choo Charles", "Chorus", "Clair Obscur: Expedition 33", "Control", "Crime Boss Rockay City",
                "Crysis 3 Remastered", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Island 2",
                "Dead Rising Remaster", "Death Stranding", "Dead Space (2023)", "Deathloop",
                "Deliver Us Mars", "Deliver Us The Moon", "Dragon Age: Veilguard", "Dragons Dogma 2",
                "Dying Light 2", "Dynasty Warriors: Origins","The Elder Scrolls IV: Oblivion Remastered" ,"Empire of the Ants", "Everspace 2",
                "Evil West", "Eternal Strands", "Fist", "Flintlock: The Siege of Dawn", "Fort Solis",
                "Forza Horizon 5", "Final Fantasy VII Rebirth", "Final Fantasy XVI", "Frostpunk 2",
                "F1 22", "F1 23", "GTA San Andreas", "GTA Vice City", "GTA III", "Ghost of Tsushima",
                "Ghostrunner 2", "Ghostwire: Tokyo", "God Of War", "God Of War Ragnarök", "Gotham Knights",
                "GreedFall II: The Dying World", "Hellblade: Senua's Sacrifice", "Senua's Saga Hellblade II",
                "High On Life", "Hogwarts Legacy", "Horizon Zero Dawn Remastered", "Hitman 3", "Horizon Forbidden West",
                "Hot Wheels Unleashed","Icarus", "Indiana Jones and the Great Circle", "Judgment", "Jusant", "Kingdom Come: Deliverance 2",
                "Kena: Bridge of Spirits", "Layers of Fear", "Lego Horizon Adventures", "Like a Dragon: Pirates", "Loopmancer", "Lords of the Fallen",
                "Manor Lords", "Lost Records Bloom And Rage", "Martha Is Dead", "Marvel's Avengers", "Marvel's Guardians of the Galaxy","Marvel's Midnight Suns",
                "Metro Exodus", "Monster Hunter Rise", "Monster Hunter Wilds", "Mortal Shell", "Ninja Gaiden 2 Black","Nobody Wants To Die","Orcs Must Die! Deathtrap",
                "Outpost", "Pacific Drive","Palworld", "Path of Exile 2", "Ratchet & Clank Rift Apart", "Ready or Not", "Red Dead Redemption", "Remnant 2", "Resident Evil 4",
                "Returnal", "Rise of The Tomb Raider", "Ripout", "Saints Row", "Sackboy: A Big Adventure", "Satisfactory","Scorn", "Sengoku Dynasty", "SOTTR", "Shadow Warrior 3", "Silent Hill 2",
                "Sifu", "Six Days in Fallujah", "Smalland", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Stalker 2", "Jedi Survivor", "Star Wars Outlaws",
                "Steelrising", "Soulslinger Envoy of Death", "Soulstice", "South of Midnight", "SuicideSquadKTJL", "Tainted Grail Fall of Avalon","Test Drive Unlimited Solar Crown", "The Alters","The Ascent", "The Callisto Protocol", "The Casting Of Frank Stone", "The First Berserker: Khazan",
                "The Last of Us Part I", "The Last of Us Part II", "The Outlast Trials", "The Talos Principle 2", "The Witcher 3", "Thymesia", "Uncharted Legacy Of Thieves", "Unknown 9: Awakening", "Until Dawn", "Wanted Dead", "Space Marine 2", "Way Of The Hunter",
                "Wayfinder", "Wuchang: Fallen Feathers"
            };
            #endregion
        }
        public static class StorageHelper 
        {
            public static List<string> GetStorageDrives()
            {
                return DriveInfo.GetDrives()
                    .Where(drive => drive.IsReady && drive.DriveType != DriveType.CDRom)
                    .Select(drive => drive.RootDirectory.FullName)
                    .ToList();
            }
            public static string[] GetGameDirectories()
            {
                // Default installation path to search for games
                return GetStorageDrives()
                    .SelectMany(drive => new[]
                    {
                Path.Combine(drive, "Program Files (x86)", "Steam", "steamapps", "common"),
                Path.Combine(drive, "SteamLibrary", "steamapps", "common"),
                Path.Combine(drive, "Program Files", "Epic Games"),
                Path.Combine(drive, "Epic Games")
                    })
                    .Where(Directory.Exists)
                    .ToArray();
            }
        }
        public static class FileHelper
        {
            public static long GetDirectorySize(string path)
            {
                return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                    .Sum(file => new FileInfo(file).Length);
            }
            public static string FindGameExe(string gamePath)
            {
                // "Default" path of the .exe (Binaries\Win64)
                var binariesPaths = Directory.GetDirectories(gamePath, "Binaries", SearchOption.AllDirectories)
                    .SelectMany(binariesDir => Directory.GetDirectories(binariesDir, "Win64", SearchOption.AllDirectories))
                    .ToList();

                foreach (var binariesPath in binariesPaths)
                {
                    var shippingExe = Directory.GetFiles(binariesPath, "*Win64-Shipping.exe", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (!string.IsNullOrEmpty(shippingExe))
                        return shippingExe;
                }

                // If the .exe is not found in the "default" path, it searches for any .exe in the folder that is not a launcher (PlayStation games usually don't have Binaries\Win64).
                var delLaunchers = new[] { "launcher", "setup", "installer", "ubisoftconnectinstaller" };

                var exeFiles = Directory.GetFiles(gamePath, "*.exe", SearchOption.AllDirectories)
                    .Where(f => !delLaunchers.Any(k => f.ToLower().Contains(k)))
                    .ToList();

                if (exeFiles.Any())
                {
                    return exeFiles.OrderByDescending(f => new FileInfo(f).Length).FirstOrDefault();
                }

                return null;
            }
        }
        private async Task ToggleSidebarAsync()
        {
            int step = 15;

            if (sidebar_Expand)
            {
                while (sidebar.Width > sidebar.MinimumSize.Width)
                {
                    sidebar.Width -= step;
                    await Task.Delay(10);
                }
                sidebar.Width = sidebar.MinimumSize.Width;
                sidebar_Expand = false;
            }
            else
            {
                while (sidebar.Width < sidebar.MaximumSize.Width)
                {
                    sidebar.Width += step;
                    await Task.Delay(10);
                }
                sidebar.Width = sidebar.MaximumSize.Width;
                sidebar_Expand = true;
            }
        }
        private async void menuButton_Click(object sender, EventArgs e)
        {
            await ToggleSidebarAsync();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public async void loadForm(Type formType, string pathT = null, string selectedGame = null)
        {
            Form formToShow = null;

            if (formType == typeof(formHome))
            {
                if (homeForm == null)
                {
                    homeForm = new formHome(this);
                    homeForm.TopLevel = false;
                    homeForm.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeForm);
                    this.mainPanel.Tag = homeForm;
                    homeForm.Show();
                }
                else
                {
                    homeForm.BringToFront();
                }
                formToShow = homeForm;
            }
            else if (formType == typeof(formSettings))
            {
                if (string.IsNullOrEmpty(GpuName))
                {
                    GpuName = await formSettings.GetActiveGpu();
                }

                if (homeSettings == null)
                {
                    homeSettings = new formSettings(this);

                    homeSettings.gpuNameSettings = GpuName;

                    homeSettings.PresetValueFromLibrary = this.valueForSettings; // Passes the path of the selected game's .exe from formLibrary to textBox1 (Select Folder) in formSettings when it is opened for the first time.                  
                    homeSettings.EnableDlssOverlayChecked = this.valueDlssOverlayForSettings; // Passes the bool from CheckBoxDlssOverlay in formLibrary to Enable/Disable DLSS Overlay in formSettings when it is opened for the first time
                    homeSettings.FlagTextBox1 = this.flagValueTextBox1Settings; // Keep the value of valueForSettings in TextBox1 if the checkBoxPreset (formLibrary) is checked

                    homeSettings.TopLevel = false;
                    homeSettings.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeSettings);
                    this.mainPanel.Tag = homeSettings;

                    if (pendingModsForSettings != null && pendingModsForSettings.Any())
                    {
                        homeSettings.ClearListMods();
                        homeSettings.AddItemlistMods(pendingModsForSettings, formHome.modsDefaultList);
                    }

                    if (valueSigForSettings.HasValue)
                    {
                        homeSettings.EnableSigChecked = this.valueSigForSettings.Value; // Passes the bool from CheckBoxPreset in formLibrary to Enable Sig Over/Disable Sig Over in formSettings when it is opened for the first time
                    }

                    if (flagValueTextBox1Settings)
                    {
                        if (!string.IsNullOrEmpty(valueForSettings))
                        {
                            homeSettings.TextBox1Text = valueForSettings;
                        }

                        if (!string.IsNullOrEmpty(valueListModsForSettings))
                        {
                            homeSettings.listModsValue = valueListModsForSettings; // Passes the preset mod of the selected game in formLibrary to listMods (Select Mod) in formSettings when it is opened for the first time
                        }
                    }

                    homeSettings.Show();
                }
                else
                {
                    homeSettings.BringToFront();

                    if (!string.IsNullOrEmpty(valueForSettings) && flagValueTextBox1Settings)
                    {
                        homeSettings.TextBox1Text = valueForSettings;
                    }
                }

                formToShow = homeSettings;
            }
            else if (formType == typeof(formLibrary))
            {
                if (string.IsNullOrEmpty(GpuName))
                {
                    MessageBox.Show("Detecting GPU, please wait...", "Library");
                    GpuName = await formSettings.GetActiveGpu();
                }

                if (homeLibrary == null)
                {
                    homeLibrary = new formLibrary(this);
                    homeLibrary.gpuName = GpuName;
                    homeLibrary.TopLevel = false;
                    homeLibrary.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeLibrary);
                    this.mainPanel.Tag = homeLibrary;
                    homeLibrary.SetGamesLibrary(gamesLibrary);
                    homeLibrary.Show();
                }
                else
                {
                    homeLibrary.BringToFront();
                }
                formToShow = homeLibrary;
            }
            else if (formType == typeof(formGuide))
            {
                if (homeGuide == null)
                {
                    homeGuide = new formGuide(this);
                    homeGuide.TopLevel = false;
                    homeGuide.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeGuide);
                    this.mainPanel.Tag = homeGuide;
                    homeGuide.Show();
                }
                else
                {
                    homeGuide.BringToFront();
                }
                formToShow = homeGuide;

                if (!string.IsNullOrEmpty(selectedGame))
                {
                    homeGuide.ShowGameGuide(selectedGame);
                }
            }

            else if (formType == typeof(formEditorToml))
            {
                if (homeEditor == null)
                {
                    homeEditor = new formEditorToml();
                    homeEditor.TopLevel = false;
                    homeEditor.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeEditor);
                    this.mainPanel.Tag = homeEditor;
                    homeEditor.Show();
                }
                else
                {
                    homeEditor.BringToFront();
                }

                if (pathT != null)
                {
                    homeEditor.SetPathT(pathT);
                }

                formToShow = homeEditor;
                if (nullHomeEditor == null)
                {
                    homeEditor = null;
                }
            }
            else
            {
                if (this.mainPanel.Controls.Count > 0 && this.mainPanel.Controls[0].GetType() == formType)
                    return;

                if (this.mainPanel.Controls.Count > 0)
                    this.mainPanel.Controls.RemoveAt(0);

                Form f = (Form)Activator.CreateInstance(formType);
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                this.mainPanel.Controls.Add(f);
                this.mainPanel.Tag = f;
                f.Show();
                formToShow = f;
            }

            if (formToShow != null)
            {
                formToShow.BringToFront();
            }

            sidebar.BringToFront();
        }
        public bool GuideExists(string gameName)
        {
            if (homeGuide == null || homeGuide.IsDisposed)
            {
                homeGuide = new formGuide(this);
            }

            return homeGuide.ShowGameGuide(gameName);
        }

        public async Task<bool> CheckVersion()
        {
            System.Windows.Forms.Timer swingTimer;
            Version currentVersion = new Version("5.3");
            labelVersion.Text = $"v{currentVersion.ToString()}";
            labelVersion.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);

            string gitRepo = "https://api.github.com/repos/P4TOLINO06/FSR3-Mod-Setup-Utility-Enhanced/releases/latest";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                    var Resp = await client.GetStringAsync(gitRepo);

                    Image bellImg = new Bitmap(Image.FromFile("Images\\Bell.ico"), new Size(24, 24));

                    using (JsonDocument doc = JsonDocument.Parse(Resp))
                    {
                        string tagName = doc.RootElement.GetProperty("tag_name").GetString();

                        if (tagName.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                        {
                            tagName = tagName.Substring(1);
                        }

                        Version newVersion = new Version(tagName);

                        if (newVersion.CompareTo(currentVersion) > 0)
                        {
                            labelVersion.Location = new Point(labelVersion.Left, labelVersion.Top - 30);

                            btnNewVersion = new Button()
                            {
                                Location = new Point(labelVersion.Left - 5, labelVersion.Top + 25),
                                Size = new Size(56,30),
                                BackColor = Color.Transparent,
                                ForeColor = Color.White,
                                FlatStyle = FlatStyle.Flat,
                                Cursor = Cursors.Hand,
                                Image = bellImg,
                                ImageAlign = ContentAlignment.MiddleCenter
                            };
                            btnNewVersion.FlatAppearance.BorderSize = 0;
                            toolTip1.SetToolTip(btnNewVersion, $"New version available ({newVersion}), click to go to the download page");
                            sidebar.Controls.Add(btnNewVersion);

                            // Bell animation
                            swingTimer = new System.Windows.Forms.Timer();
                            swingTimer.Interval = 100;
                            int swingAngle = -15;

                            swingTimer.Tick += (s, e) =>
                            {
                                swingAngle = (swingAngle == -15) ? 15 : -15;
                                btnNewVersion.Image = RotateImage(bellImg, swingAngle);
                            };
                            swingTimer.Start();

                            // Open the latest release of the Utility repository
                            btnNewVersion.Click += (sender, e) => {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = "https://github.com/P4TOLINO06/FSR3-Mod-Setup-Utility-Enhanced/releases/latest",
                                    UseShellExecute = true
                                });

                                if (swingTimer.Enabled)
                                {
                                    btnNewVersion.Image = bellImg;
                                    btnNewVersion.ImageAlign = ContentAlignment.MiddleCenter;
                                }
                                swingTimer.Stop();
                            };                     

                            // Detect opening and stop bell animation
                            sidebar.SizeChanged += (s, e) =>
                            {
                                if (sidebar.Width == 188)
                                {
                                    btnNewVersion.Size = new Size(188,30);
                                    btnNewVersion.Text = $"New version available ({newVersion})";
                                    btnNewVersion.Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold);
                                    btnNewVersion.TextAlign = ContentAlignment.BottomRight;
                                    btnNewVersion.ImageAlign = ContentAlignment.MiddleLeft;
                                    swingTimer.Stop();
                                }
                                else
                                {
                                    btnNewVersion.Text = "";
                                    btnNewVersion.Size = new Size(56, 30);
                                    btnNewVersion.Image = bellImg;
                                    btnNewVersion.ImageAlign = ContentAlignment.MiddleCenter;
                                }
                            };

                            bool newVersionAvailable = true;

                            return newVersionAvailable;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Image RotateImage(Image img, float angle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TranslateTransform(img.Width / 2f, img.Height / 2f);
                g.RotateTransform(angle);
                g.TranslateTransform(-img.Width / 2f, -img.Height / 2f);
                g.DrawImage(img, new Point(0, 0));
            }
            return bmp;
        }

        private void Form1_Resize(object sender, EventArgs e)

        {
            sidebar.Height = this.ClientSize.Height - sidebar.Top;
            mainPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - mainPanel.Top);
            pictureBox1.Size = this.ClientSize;

            if (versionAvailable)
            {
                labelVersion.Location = new Point(sidebar.Left, sidebar.Bottom - labelVersion.Height - 37);  
                if (btnNewVersion.Enabled)
                {
                    btnNewVersion.Location = new Point(labelVersion.Left - 5, labelVersion.Top + 25);
                }
            }
            else
            {
                labelVersion.Location = new Point(sidebar.Left, sidebar.Bottom - labelVersion.Height - 10);
            }
        }
        private async void mainForm_Shown(object sender, EventArgs e)
        {
            GpuName = await formSettings.GetActiveGpu();
        }

        private async void mainPanel_Load(object sender, EventArgs e)
        {

            versionAvailable = await CheckVersion();
            clockTimer.Start();

            var imagePaths = new Dictionary<string, List<string>>()
            {
                { "morning", new List<string> {
                    "Images\\Wallpaper\\rdr2_morning.png",
                    "Images\\Wallpaper\\rdr2_morning2.png",
                    "Images\\Wallpaper\\rdr2_morning3.png",
                    "Images\\Wallpaper\\rdr2_morning4.png",
                    "Images\\Wallpaper\\rdr2_morning5.png"
                }},
                { "afternoon", new List<string> {
                    "Images\\Wallpaper\\rdr2_afternoon.png",
                    "Images\\Wallpaper\\rdr2_afternoon2.png",
                    "Images\\Wallpaper\\rdr2_afternoon3.png",
                    "Images\\Wallpaper\\rdr2_afternoon4.png",
                    "Images\\Wallpaper\\rdr2_afternoon5.png",
                }},
                { "night", new List<string> {
                    "Images\\Wallpaper\\rdr2_night.png",
                    "Images\\Wallpaper\\rdr2_night2.png",
                    "Images\\Wallpaper\\rdr2_night3.png",
                    "Images\\Wallpaper\\rdr2_night4.png",
                    "Images\\Wallpaper\\rdr2_night5.png",
                }}
            };

            string hourNow = DateTime.Now.ToString("HH:mm:ss");

            string period = hourNow switch
            {
                _ when (string.Compare(hourNow, "18:00:00") >= 0 && string.Compare(hourNow, "23:59:59") <= 0) ||
                             (string.Compare(hourNow, "00:00:00") >= 0 && string.Compare(hourNow, "05:00:00") <= 0) => "night",
                _ when string.Compare(hourNow, "12:00:00") >= 0 && string.Compare(hourNow, "17:59:59") <= 0 => "afternoon",
                _ => "morning"
            };

            if (imagePaths.ContainsKey(period))
            {
                var initialImages = imagePaths[period];
                var randomImage = initialImages[new Random().Next(initialImages.Count)];
                string fullPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, randomImage);

                if (File.Exists(fullPath))
                {

                    Image img = await Task.Run(() => Image.FromFile(fullPath));
                    pictureBox1.Image = img;
                }

            }
            mainPanel.BackgroundImage = pictureBox1.Image;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            loadForm(typeof(formHome));
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (showSettingsSubMenu)
            {
                panel3.Height = 55;

                panel4.Location = new Point(panel4.Left, panel3.Bottom);
                panel5.Location = new Point(panel5.Left, panel4.Bottom);
            }
            else
            {
                panel3.Height = 150;

                panel4.Location = new Point(panel4.Left, panel3.Bottom);
                panel5.Location = new Point(panel5.Left, panel4.Bottom);
            }

            showSettingsSubMenu = !showSettingsSubMenu;
        }

        private void buttonMods_Click(object sender, EventArgs e)
        {
            loadForm(typeof(formSettings));
        }

        private void buttonLibrary_Click(object sender, EventArgs e)
        {
            loadForm(typeof(formLibrary));
        }

        private void button3_Click(object sender, EventArgs e)
        {

            loadForm(typeof(formGuide));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this)
                {
                    form.Hide();
                    if (form == homeForm)
                    {
                        homeForm = null;
                    }
                    else if (form == homeSettings)
                    {
                        homeSettings = null;
                    }
                    else if (form == homeGuide)
                    {
                        homeGuide = null;
                    }
                    else if (form == homeLibrary)
                    {
                        homeLibrary = null;
                    }
                }
            }
        }
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            DateOwLabel.Text = DateTime.Now.DayOfWeek.ToString();
            Date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}

