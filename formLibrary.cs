using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formLibrary : Form
    {
        private List<string> gamesLibrary;
        private string exePath = "";
        private Panel panelGameMenu;
        private Panel panelLibrarySettings;
        private Panel panelScanPaths;
        private bool varBtnGameMenu = false;
        private bool varBtnLibrarySettings = false;
        private Button? selectedButton = null;
        public string gpuName { get; set; }
        private Button btnRestoreGame;
        private int gamesCount = 0;
        private int updatedGamesCount = 0;
        private Stack<Button> buttonsRemoved = new Stack<Button>();
        private List<string> pathScanLocation;
        private string rootExeDir = AppDomain.CurrentDomain.BaseDirectory;
        private string gameName;
        private string jsonScanPath;
        public formLibrary()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panel1, new object[] { true });

            jsonScanPath = Path.Combine(rootExeDir, "user_scan_paths.json");
        }
        private void formLibrary_Load(object sender, EventArgs e)
        {
            btnGameMenu.Left = panelIconGame.Width - btnGameMenu.Width - 10;
        }
        public void SetGamesLibrary(List<string> gamesLibrary)
        {
            this.gamesLibrary = gamesLibrary;

            string games = string.Join(", ", gamesLibrary);

            if (File.Exists(jsonScanPath))
            {
                try
                {
                    var userJson = File.ReadAllText(jsonScanPath);
                    var userScanPaths = JsonSerializer.Deserialize<List<string>>(userJson) ?? [];

                    ((mainForm)this.ParentForm).ScanGames(possiblePaths: userScanPaths);
                }
                catch (Exception ex) when (ex is JsonException || ex is IOException)
                {
                    try { File.Delete(jsonScanPath); } catch { }

                    MessageBox.Show("It was not possible to load the scan paths because the configuration file was corrupted or unreadable. The file has been removed. Please re-add the paths.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            gamesIcons();
        }
        public async Task gamesIcons()
        {
            #region Icons
            string rootPathicons = "Images\\Icon";

            Dictionary<string, string> gameIcons = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"A Plague Tale Requiem","Requiem.ico"},
                {"Elden Ring","Elden.ico"},
                {"Elden Ring Nightreign","Elden Ring Nightreign.ico"},
                {"Red Dead Redemption 2","Rdr2.ico"},
                {"Grand Theft Auto V","GTAV.ico"},
                {"Watch Dogs Legion","Legion.ico"},
                {"Lies of P","Lop.ico"},
                {"Bright Memory","BM.ico"},
                {"Fobia – St. Dinfna Hotel","Fobia.ico"},
                {"Alan Wake Remastered" , "AlanWakeR.ico"},
                {"Horizon Zero Dawn", "Hzd.ico"},
                {"Bright Memory Infinite", "BMI.ico"},
                {"A Quiet Place: The Road Ahead", "Aqp.ico"},
                {"Assassin\'s Creed Mirage", "Mirage.ico"},
                {"Assetto Corsa Evo", "ACEvo.ico"},
                {"Assassin\'s Creed Shadows", "AcShadows.ico"},
                {"Assassin's Creed Valhalla", "Valhalla.ico"},
                {"Atomic Heart", "Atomic.ico"},
                {"AVOWED", "Avowed.ico"},
                {"Back 4 Blood", "B4B.ico"},
                {"Baldur's Gate 3", "Bdg3.ico"},
                {"Banishers Ghost of New Eden","Banishers.ico"},
                {"Black Myth: Wukong", "Wukong.ico"},
                {"Blacktail", "Blacktail.ico"},
                {"Brothers a Tale of Two Sons", "Brothers.ico" },
                {"Chernobylite", "Cherno.ico"},
                {"Choo-Choo Charles", "CCC.ico"},
                {"Chorus", "Chorus.ico"},
                {"Clair Obscur: Expedition 33", "Coe33.ico"},
                {"Control", "Control.ico"},
                {"Crime Boss Rockay City", "CBR.ico"},
                {"Crysis 3 Remastered", "Crysis.ico"},
                {"Cyberpunk 2077", "CB2077.ico"},
                {"Dakar Desert Rally", "Dakar.ico"},
                {"Dead Island 2", "Di2.ico"},
                {"Dead Rising Remaster", "Drdr.ico"},
                {"Death Stranding","DS.ico"},
                {"Dead Space (2023)","DeadSpace.ico"},
                {"Deathloop","DeathLoop.ico"},
                {"Deliver Us Mars","DUM.ico"},
                {"Deliver Us The Moon","DUTM.ico"},
                {"Dragon Age: Veilguard","DAV.ico"},
                {"Dragons Dogma 2","DD2.ico"},
                {"Dying Light 2","DL2.ico"},
                {"Dynasty Warriors: Origins","DWO.ico"},
                {"The Elder Scrolls IV: Oblivion Remastered", "Elder4.ico"},
                {"Empire of the Ants","EOTA.ico"},
                {"Everspace 2","ES2.ico"},
                {"Evil West","EW.ico"},
                {"Eternal Strands","ES.ico"},
                {"Fist","fist.ico" }, //Fist Forged in Shadow Torch
				{"Flintlock: The Siege of Dawn","Flintlock.ico"},
                {"Fort Solis","FortSolis.ico"},
                {"Forza Horizon 5","Forza5.ico"},
                {"Final Fantasy VII Rebirth","FF7R.ico"},
                {"Final Fantasy XVI","FFXVI.ico"},
                {"Frostpunk 2","FP2.ico"},
                {"F1 22","F122.ico"},
                {"F1 23","F1 23.ico"},
                {"GTA San Andreas", "GTASA.ico"},
                {"GTA Vice City","GTAVC.ico"},
                {"GTA III", "GTAIII.ico"},
                {"Ghost of Tsushima", "Tsushima.ico"},
                {"Ghostrunner 2","Ghostrunner2.ico"},
                {"Ghostwire: Tokyo","GhostTokyo.ico"},
                {"God Of War","Gow4.ico"},
                {"God Of War Ragnarök","GowRag.ico"},
                {"Gotham Knights","GK.ico"},
                {"GreedFall II: The Dying World","GF2.ico"},
                {"Hellblade: Senua's Sacrifice","Hellblade.ico"},
                {"Senua's Saga Hellblade II","HB2.ico"},
                {"High On Life","HOL.ico"},
                {"Hogwarts Legacy","HL.ico"},
                {"Horizon Zero Dawn Remastered","HZDR.ico"},
                {"Hitman 3","HM3.ico"},
                {"Horizon Forbidden West","HZDFW.ico"},
                {"Hot Wheels Unleashed","Hotwheels.ico"},
                {"Icarus","Icarus.ico"},
                {"Indiana Jones and the Great Circle","Indi.ico"},
                {"Judgment","Jud.ico"},
                {"Jusant","Jusant.ico"},
                {"Kingdom Come: Deliverance 2","KCD2.ico"},
                {"Kena: Bridge of Spirits","Kena.ico"},
                {"Layers of Fear","LOF.ico"},
                {"Lego Horizon Adventures","LegoHzd.ico"},
                {"Like a Dragon: Pirates","Yakuza.ico"},
                {"Loopmancer","Loopmancer.ico"},
                {"Lords of the Fallen","LOTF.ico"},
                {"Manor Lords","ML.ico"},
                {"Lost Records Bloom And Rage","LRBR.ico"},
                {"Martha Is Dead","Martha.ico"},
                {"Marvel's Avengers","Avengers.ico"},
                {"Marvel's Guardians of the Galaxy","GuardiansGalaxy.ico"},
                {"Marvel's Midnight Suns","MidnightSuns.ico"},
                {"Metro Exodus","Exodus.ico"},
                {"Monster Hunter Rise","MHRise.ico"},
                {"Monster Hunter Wilds","MHW.ico"},
                {"Mortal Shell","MS.ico"},
                {"Ninja Gaiden 2 Black","NG2B.ico"},
                {"Nobody Wants To Die","NWTD.ico"},
                {"Orcs Must Die! Deathtrap","Orcs.ico"},
                {"Outpost","OIS.ico"},
                {"Pacific Drive","PD.ico"},
                {"Palworld","Palworld.ico"},
                {"Path of Exile 2","POE.ico"},
                {"Ratchet & Clank Rift Apart","Ratchet.ico"},
                {"Ready or Not","RON.ico"},
                {"Red Dead Redemption","Rdr.ico"},
                {"Remnant 2","Remn2.ico"},
                {"Resident Evil 4","RE4.ico"},
                {"Returnal","Returnal.ico"},
                {"Rise of The Tomb Raider","RiseTomb.ico"},
                {"Ripout","Ripout.ico"},
                {"Saints Row","Saints Row.ico"},
                {"Sackboy: A Big Adventure","Sackboy.ico"},
                {"Satisfactory","Satisfactory.ico"},
                {"Scorn","Scorn.ico"},
                {"Sengoku Dynasty","Sengoku.ico"},
                {"SOTTR","ShadowTomb.ico"},
                {"Shadow Warrior 3","SW3.ico"},
                {"Silent Hill 2","SH2.ico"},
                {"Sifu","Sifu.ico"},
                {"Six Days in Fallujah","SixDays.ico"},
                {"Smalland","Smalland.ico"},
                {"SuicideSquadKTJL","SS.ico"},
                {"Marvel's Spider-Man Remastered","Spider.ico"},
                {"Marvel's Spider-Man Miles Morales","Miles.ico"},
                {"Stalker 2","Stalker2.ico"},
                {"Star Wars Outlaws","Outlaws.ico"},
                {"Jedi Survivor","JediSurvivor.ico"},
                {"Steelrising","Steelrising.ico"},
                {"Soulslinger Envoy of Death","SoulSlinger.ico"},
                {"Soulstice","Soulstice.ico"},
                {"South of Midnight","Som.ico"},
                {"Tainted Grail Fall of Avalon","Tainted.ico"},
                {"Test Drive Unlimited Solar Crown","TestDrive.ico"},
                {"The Alters","The Alters.ico"},
                {"The Ascent", "TheAscent.ico"},
                {"The Callisto Protocol","Callisto.ico"},
                {"The Casting Of Frank Stone","Frankstone.ico"},
                {"The First Berserker: Khazan","Khazan.ico"},
                {"The Last of Us Part I","Tlou1.ico"},
                {"The Last of Us Part II","Tlou2.ico"},
                {"The Outlast Trials","Tot.ico"},
                {"The Talos Principle 2","TTP2.ico"},
                {"The Witcher 3","TW3.ico"},
                {"Thymesia","Thymesia.ico"},
                {"Uncharted Legacy Of Thieves","Uncharted.ico"},
                {"Unknown 9: Awakening","Unknown.ico"},
                {"Until Dawn","UD.ico"},
                {"Wanted Dead","WD.ico"},
                {"Space Marine 2","SpaceMarine2.ico"},
                {"Way Of The Hunter","WOTH.ico"},
                {"Wayfinder","Wayfinder.ico"}
            };

            List<string> validGames = new List<string> // Similar games that require exact comparison
            {
                "God Of War",
                "God Of War Ragnarok",
                "Red Dead Redemption",
                "Red Dead Redemption 2",
                "The Last of Us Part I",
                "The Last of Us Part II",
            };

            List<string> exactGames = new List<string>// Games that have the same root folder name (e.g., Lords Of The Fallen and Lords of the Fallen (<- LOTF 2023))
            {
                "Lords of the Fallen"
            };

            Dictionary<string, string> gameAbbr = new Dictionary<string, string> //Games that have the folder abbreviated as Shadow of the Tomb Raider (SOTTR) will have the full name displayed in btnGames.Text
			{
                {"SOTTR", "Shadow of the Tomb Raider" },
                {"Space Marine 2", "Warhammer 40.000: Space Marine 2"},
                {"SuicideSquadKTJL", "Suicide Squad: Kill the Justice League"},
                {"Fist", "Fist Forged in Shadow Torch" },
                {"Jedi Survivor", "Star Wars: Jedi Survivor"}
            };

            #endregion

            panelIcon.Controls.Clear();

            gamesCount = 0;

            int y = 10;
            Button defIcon = null;

            HashSet<string> addedGames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string firstExePath = gamesLibrary.FirstOrDefault(g => g.StartsWith("Exe Path: "))?.Replace("Exe Path: ", "").Trim()?.ToLowerInvariant() ?? "";

            foreach (var game in gamesLibrary)
            {
                string matchedGame;

                if (!game.StartsWith("Game: "))
                    continue;

                gameName = game.Substring(6).Trim();
                string normalizedGame = gameName.Replace(" ", "").ToLowerInvariant();

                if (!addedGames.Add(normalizedGame))
                    continue;

                bool isValidGame = validGames.Contains(normalizedGame);

                if (isValidGame)
                {
                    if (string.IsNullOrEmpty(firstExePath) || !firstExePath.Contains(normalizedGame))
                        continue;
                }

                if (exactGames.Contains(gameName) && gameIcons.ContainsKey(gameName))
                {
                    matchedGame = gameName;
                }
                else
                {
                    matchedGame = gameIcons.Keys
                        .OrderByDescending(k => k.Length)
                        .FirstOrDefault(key =>
                            normalizedGame.Contains(key.Replace(" ", "").ToLowerInvariant()));
                }

                if (matchedGame == null)
                    continue;

                gamesCount++;
                Button btnGames = new Button
                {
                    Text = gameAbbr.ContainsKey(matchedGame) ? gameAbbr[matchedGame] : matchedGame,
                    ForeColor = Color.White,
                    Size = new Size(175, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = SystemColors.ControlDarkDark,
                    Cursor = Cursors.Hand,
                    Location = new Point(56, y),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                btnGames.Click += (s, e) => selectedButton = (Button)s;

                if (gamesCount == 1) btnGames.PerformClick();

                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(btnGames, matchedGame);
                btnGames.Tag = gameName;

                #region IgnoreInd
                int maxLength = 10;
                string[] gamesToIgnoreInd = { "Elden Ring", "Fobia – St. Dinfna Hotel", "A Plague Tale Requiem", "God Of War", "Loopmancer", "Red Dead Redemption", "Remnant 2", "The Ascent", "Uncharted Legacy Of Thieves", "Until Dawn", "Wayfinder" };
                string[] otGamesToIgnoreInd = { "Fobia – St. Dinfna Hotel", "A Plague Tale Requiem", "Horizon Zero Dawn", "Alan Wake Remastered", "Bright Memory Infinite", "Back 4 Blood", "Baldur's Gate 3", "Black Myth: Wukong", "Brothers a Tale of Two Sons", "Chernobylite",
                "Chorus", "Dead Island 2", "Dead Rising Remaster", "Deliver Us The Moon", "Dragon Age: Veilguard", "Dragons Dogma 2", "Dying Light 2", "Dynasty Warriors: Origins", "Elden Ring Nightreign","Empire of the Ants", "Everspace 2", "Evil West", "Eternal Strands", "Forza Horizon 5",
                "Frostpunk 2", "Ghost of Tsushima", "Ghostwire: Tokyo", "Hellblade: Senua's Sacrifice", "Senua's Saga Hellblade II", "High On Life", "Horizon Forbidden West", "Hogwarts Legacy", "Hitman 3", "Hot Wheels Unleashed", "Indiana Jones and the Great Circle", "Kena: Bridge of Spirits",
                "Lego Horizon Adventures", "Like a Dragon: Pirates", "Lords of the Fallen", "Manor Lords", "Lost Records Bloom And Rage", "Martha Is Dead", "Marvel's Avengers", "Marvel's Guardians of the Galaxy", "Marvel's Midnight Suns", "Metro Exodus", "Monster Hunter Rise", "Monster Hunter Wilds",
                "Mortal Shell", "Ninja Gaiden 2 Black", "Orcs Must Die! Deathtrap", "Pacific Drive", "Path of Exile 2", "Ready or Not", "Resident Evil 4", "Rise of The Tomb Raider","SOTTR", "Six Days in Fallujah", "SuicideSquadKTJL", "Test Drive Unlimited Solar Crown","The Casting Of Frank Stone", "The Talos Principle 2",
                "The Witcher 3", "Thymesia", "Way Of The Hunter", "The Alters","The Last of Us Part I", "Banishers Ghost of New Eden", "The Outlast Trials", "South of Midnight", "Tainted Grail Fall of Avalon", "The Elder Scrolls IV: Oblivion Remastered"};
                string[] otGamesToIgnoreInd2 = { "Flintlock: The Siege of Dawn", "Final Fantasy VII Rebirth", "F1 22", "F1 23", "Silent Hill 2" };
                string[] otGamesToIgnoreInd3 = { "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Jedi Survivor", "Star Wars Outlaws", "Steelrising", "Soulslinger Envoy of Death", "The Callisto Protocol", "The First Berserker: Khazan", "The Last of Us Part II", "Stalker 2", "Death Stranding", "Fist", "Clair Obscur: Expedition 33", "Assassin\'s Creed Shadows", "Satisfactory" };
                #endregion

                if (gamesToIgnoreInd.Contains(matchedGame))
                {
                    btnGames.Text = "    " + (gameAbbr.ContainsKey(matchedGame) ? gameAbbr[matchedGame] : matchedGame);
                }

                if (matchedGame.Length > maxLength || (gameAbbr.ContainsKey(matchedGame) && gameAbbr[matchedGame].Length > maxLength))
                {
                    string ButtonTxt = gameAbbr.ContainsKey(matchedGame) ? gameAbbr[matchedGame] : matchedGame;

                    if (otGamesToIgnoreInd.Contains(matchedGame))
                    {
                        btnGames.Text = "      " + ButtonTxt.Substring(0, maxLength) + "...";
                    }
                    else if (otGamesToIgnoreInd2.Contains(matchedGame))
                    {
                        btnGames.Text = "   " + ButtonTxt.Substring(0, maxLength) + "...";
                    }
                    else if (otGamesToIgnoreInd3.Contains(matchedGame))
                    {
                        btnGames.Text = "     " + ButtonTxt.Substring(0, maxLength) + "...";
                    }
                    else
                    {
                        btnGames.Text = "        " + ButtonTxt.Substring(0, maxLength) + "...";
                        btnGames.UseCompatibleTextRendering = false;
                        btnGames.UseMnemonic = false;
                    }
                }

                if (gameIcons.ContainsKey(matchedGame))
                {
                    string icoPath = Path.Combine(rootPathicons, gameIcons[matchedGame]);
                    if (File.Exists(icoPath))
                    {
                        Image iconImage = Image.FromFile(icoPath);
                        btnGames.Image = new Bitmap(iconImage, new Size(30, 30));
                    }
                }

                if (defIcon == null)
                {
                    defIcon = btnGames;
                    string iconImage = Path.Combine(rootPathicons, gameIcons[matchedGame]);
                    UpdateGamePanel(matchedGame, iconImage, gameAbbr);
                }

                btnGames.Click += (s, e) =>
                {
                    if (checkBoxPreset.Checked)
                    {
                        checkBoxPreset.Checked = false;
                    }
                    string iconPath = Path.Combine(rootPathicons, gameIcons[matchedGame]);
                    UpdateGamePanel(matchedGame, iconPath, gameAbbr);
                };

                updatedGamesCount = gamesCount;
                labelGamesCount.Text = ($"{updatedGamesCount}/{gamesCount}").ToString();
                panelIcon.Controls.Add(btnGames);
                y += 50;


                //DLSS Overlay visible only for RTX users
                if (gpuName != null && !gpuName.Contains("rtx", StringComparison.OrdinalIgnoreCase))
                {
                    checkBoxDlssOverlay.Visible = labelDlssOveray.Visible = labelDlssOverlayValue.Visible = panel11.Visible = false;

                    int controlsMoveDown = 33;
                    labelFolder.Location = new Point(labelFolder.Left, labelSignatureOver.Top + controlsMoveDown);
                    labelFolderName.Location = new Point(labelFolderName.Left, labelOnSig.Top + controlsMoveDown);
                    panel8.Location = new Point(panel8.Left, panel10.Top + controlsMoveDown);
                    btnGuide.Location = new Point(btnGameMenu.Right - 35, btnGuide.Top);
                    label2.Location = new Point(label2.Left, labelFolder.Top + controlsMoveDown);
                    labelCheckGuide.Location = new Point(labelCheckGuide.Left, labelFolder.Top + 38);
                    panel13.Location = new Point(panel13.Left, panel8.Top + controlsMoveDown);
                }
            }
        }

        public void UpdateGamePanel(string matchedGame, string iconPath, Dictionary<string, string> gameAbbr)
        {
            foreach (var game in gamesLibrary)
            {
                if (game.StartsWith("Game: "))
                {
                    string gameName = game.Replace("Game: ", "").Trim();
                    string normalizedGame = gameName.ToLower().Replace(" ", "");

                    if (normalizedGame == matchedGame.ToLower().Replace(" ", ""))
                    {
                        int index = gamesLibrary.IndexOf(game);
                        if (index + 1 < gamesLibrary.Count && gamesLibrary[index + 1].StartsWith("Exe Path: "))
                        {
                            exePath = gamesLibrary[index + 1].Replace("Exe Path: ", "").Trim();
                        }
                        break;
                    }
                }
            }

            string folderPath = Path.GetDirectoryName(exePath);
            var defSettings = ("Yes", folderPath, "It is recommended to check before installation, as it contains important information.");
            var defVideoGuide = ("It is recommended to check before installation, as it contains important information. A video guide is available.");
            var defOptiscaler = ("FSR 3.1.4/DLSS FG (Only Optiscaler)", "Yes", folderPath, "It is recommended to check before installation.");

            #region game Info
            var gameInfo = new Dictionary<string, (string, string, string, string)>
            {
                {"Elden Ring", ("FSR 3.1.4/DLSS FG Custom Elden", defSettings.Item1, defSettings.Item2, defVideoGuide)},
                {"Elden Ring Nightreign", ("FSR 3.1.4/DLSS Nightreign RTX", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"A Plague Tale Requiem", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Red Dead Redemption 2", ("FSR 3.1.4/DLSS FG (Only Optiscaler RDR2) or Red Dead Redemption 2 MIX", defSettings.Item1, defSettings.Item2, defVideoGuide) },
                {"Grand Theft Auto V", ("FSR 3.1.4/DLSS FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defVideoGuide) },
                {"Assassin's Creed Shadows", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Bright Memory Infinite", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Assassin's Creed Valhalla", ("AC Valhalla DLSS (Only RTX) or AC Valhalla FSR3 ALL GPUs ", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Atomic Heart", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"AVOWED", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Back 4 Blood", ("FSR 3.1.4/DLSS FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Baldur's Gate 3", ("Baldur's Gate 3 FSR3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Black Myth: Wukong", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Cyberpunk 2077", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Dead Rising Remaster", ("FSR 3.1 FG DRR", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Deathloop", ("0.10.3 or 0.10.4", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Dragon Age: Veilguard", ("FSR 3.1.1/DLSS FG DG Veil", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"The Elder Scrolls IV: Oblivion Remastered", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Dragons Dogma 2", ("FSR 3.1.2/DLSS FG Custom", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Everspace 2", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Eternal Strands", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Forza Horizon 5", ("Forza 5 FSR3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Frostpunk 2", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Ghost of Tsushima", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Ghostrunner 2", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"God Of War", ("FSR 3.1.4/DLSS Gow4",defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"God Of War Ragnarök", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Senua's Saga Hellblade II", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Hogwarts Legacy", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Horizon Forbidden West", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Icarus", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or Icarus FSR3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Indiana Jones and the Great Circle", ("Indy FG (Only RTX)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Like a Dragon: Pirates", ("DLSSG Yakuza", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Loopmancer", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Lords of the Fallen", ("DLSS FG LOTF2 (RTX)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Manor Lords", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Lost Records Bloom And Rage", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Martha Is Dead", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.4", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Monster Hunter Rise", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or 0.10.3", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Monster Hunter Wilds", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Palworld", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or Palworld Build03", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Resident Evil 4", ("FSR 3.1.4/DLSS RE4", defSettings.Item1, defSettings.Item2, defSettings.Item3)},
                {"Marvel's Spider-Man Remastered", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Marvel's Spider-Man Miles Morales", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Star Wars: Jedi Survivor", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Stalker 2", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Satisfactory", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"The Alters", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"The First Berserker: Khazan", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"The Last of Us Part I", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"The Last of Us Part II", ("FSR 3.1.4/DLSSG FG (Only Optiscaler)", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
                {"Unknown 9 Awakening", ("FSR 3.1.4/DLSS FG (Only Optiscaler) or Uniscaler V4", defSettings.Item1, defSettings.Item2, defSettings.Item3) },
            };
            #endregion

            var gamesWithVideoGuide = new List<string> { "Resident Evil 4", "Sifu", "Steelrising", "Elden Ring", "Mortal Shell", "Control", "Fist Forged in Shadow Torch", "Senua's Saga Hellblade II", "Scorn", "Saints Row", "Way Of The Hunter", "Horizon Zero Dawn", "Horizon Zero Dawn Remastered", "Alone in the Dark", "God Of War", "The Last of Us Part I", "The Last of Us Part II", "The First Berserker: Khazan", "Spider/Miles", "Palworld", "Black Myth: Wukong", "Lies of P", "Red Dead Redemption 2", "Grand Theft Auto V", "Dead Island 2", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales" };

            var gamesWithDefOptiscaler = new List<string> { "Bright Memory", "Alan Wake Remastered", "Watch Dogs Legion", "Fobia – St. Dinfna Hotel", "Lies of P", "Horizon Zero Dawn", "A Quiet Place: The Road Ahead", "Assassin\'s Creed Mirage", "Assetto Corsa Evo" ,"Banishers Ghost of New Eden", "Blacktail", "Brothers a Tale of Two Sons", "Chernobylite", "Choo-Choo Charles",
            "Chorus", "Clair Obscur: Expedition 33", "Control", "Crime Boss Rockay City", "Crysis 3 Remastered", "Dakar Desert Rally", "Dead Island 2", "Death Stranding", "Dead Space (2023)", "Deliver Us Mars", "Deliver Us The Moon", "Dying Light 2", "Dynasty Warriors: Origins", "Empire of the Ants", "Evil West", "Fist Forged in Shadow Torch", "Flintlock: The Siege of Dawn", "Fort Solis",
            "Final Fantasy VII Rebirth", "Final Fantasy XVI", "F1 22", "F1 23", "GTA San Andreas", "GTA Vice City", "GTA III", "Grand Theft Auto V", "Ghostwire: Tokyo", "Gotham Knights", "GreedFall II: The Dying World", "Hellblade: Senua's Sacrifice", "High On Life", "Horizon Zero Dawn Remastered", "Hitman 3", "Horizon Forbidden West", "Hot Wheels Unleashed",
            "Judgment", "Jusant", "Kingdom Come: Deliverance 2", "Kena: Bridge of Spirits", "Layers of Fear", "Lego Horizon Adventures", "Marvel's Avengers", "Marvel's Guardians of the Galaxy", "Marvel's Midnight Suns", "Metro Exodus", "Mortal Shell", "Ninja Gaiden 2 Black", "Nobody Wants To Die", "Orcs Must Die! Deathtrap", "Outpost",
            "Pacific Drive", "Path of Exile 2", "Ratchet & Clank Rift Apart", "Ready or Not", "Red Dead Redemption", "Remnant 2", "Returnal", "Rise of The Tomb Raider", "Ripout", "Saints Row", "Sackboy: A Big Adventure", "Scorn", "Sengoku Dynasty", "Shadow of the Tomb Raider", "Shadow Warrior 3", "Silent Hill 2", "Sifu", "Six Days in Fallujah", "Smalland",
            "Star Wars Outlaws", "Steelrising", "Soulslinger Envoy of Death", "Soulstice", "South of Midnight", "Suicide Squad: Kill the Justice League", "Tainted Grail Fall of Avalon", "Test Drive Unlimited Solar Crown", "The Ascent", "The Callisto Protocol", "The Casting Of Frank Stone", "The Outlast Trials","The Talos Principle 2", "The Witcher 3", "Thymesia", "Uncharted Legacy Of Thieves", "Until Dawn", "Wanted Dead", "Warhammer 40.000: Space Marine 2", "Way Of The Hunter", "Wayfinder"};

            foreach (var gameOpti in gamesWithVideoGuide)
            {
                foreach (var key in gameInfo.Keys.Where(k => k.Contains(gameOpti)).ToList())
                {
                    if (gameInfo[key].Item1 == defOptiscaler.Item1)
                    {
                        gameInfo[key] = (defOptiscaler.Item1, defOptiscaler.Item2, defOptiscaler.Item3, defVideoGuide);
                    }
                }
            }

            foreach (var gameOpti in gamesWithDefOptiscaler)
            {
                if (gameInfo.ContainsKey(gameOpti))
                {
                    if (gameInfo[gameOpti].Item4 == defVideoGuide)
                        continue;

                    if (gamesWithVideoGuide.Contains(gameOpti) && gameInfo[gameOpti].Item1 == defOptiscaler.Item1)
                    {
                        gameInfo[gameOpti] = (defOptiscaler.Item1, defOptiscaler.Item2, defOptiscaler.Item3, defVideoGuide);
                    }
                }
                else
                {
                    if (gamesWithVideoGuide.Contains(gameOpti))
                    {
                        gameInfo[gameOpti] = (defOptiscaler.Item1, defOptiscaler.Item2, defOptiscaler.Item3, defVideoGuide);
                    }
                    else
                    {
                        gameInfo[gameOpti] = defOptiscaler;
                    }
                }
            }

            string displayName = gameAbbr.ContainsKey(matchedGame) ? gameAbbr[matchedGame] : matchedGame;

            labelGameIcon.Text = displayName;
            picGameIcon.Image = Image.FromFile(iconPath);

            if (gameInfo.TryGetValue(displayName, out var info))
            {
                labelModName.Text = info.Item1;
                labelOnSig.Text = info.Item2;
                labelFolderName.Text = info.Item3;
                labelCheckGuide.Text = info.Item4;
            }

        }
        private void formLibrary_Resize(object sender, EventArgs e)
        {
            #region Location
            panelGames.Location = new Point(panelIcon.Location.X, panelIcon.Location.Y - panelGames.Height - 2);

            labelGamesCount.Location = new Point(
                (panelGames.Width - labelGamesCount.Width) / 2 - 32,
                (panelGames.Height - labelGamesCount.Height) / 2 - 15
            );

            labelGames.Location = new Point(
                labelGamesCount.Left,
                labelGamesCount.Top + 25
            );

            labelLibrary.Location = new Point(
                panel3.Right / 2
            );

            labelPreset.Location = new Point(
                 panel3.Left + (panel3.Width - labelPreset.Width) / 2 - 180,
                labelPreset.Top
            );

            btnSettings.Location = new Point(
                (btnGameMenu.Right - 35),
                (btnSettings.Top)
            );

            panel12.Location = new Point(
                (btnSettings.Right - 35),
                (panel12.Top)
            );

            checkBoxPreset.Location = new Point(
                (panel12.Right - 98),
                (checkBoxPreset.Top)
            );

            btnGuide.Location = new Point(
                (btnGameMenu.Right - 35),
                (btnGuide.Top)
            );

            checkBoxDlssOverlay.Location = new Point(
                (panel12.Right - 98),
                (checkBoxDlssOverlay.Top)
            );

            btnLibrarySettings.Location = new Point(
                panelGames.Width - btnLibrarySettings.Width,
                btnGameMenu.Top
            );

            if (panelLibrarySettings != null)
            {
                panelLibrarySettings.Location = new Point(
                    btnLibrarySettings.Right - 20,
                    btnLibrarySettings.Top + 80
                    );
            }
            #endregion
        }
        private void btnGameMenu_Click(object sender, EventArgs e)
        {
            if (panelGameMenu == null)
            {
                panelGameMenu = new Panel()
                {
                    ForeColor = Color.White,
                    Size = new Size(175, 105),
                    BackColor = Color.Gray,
                    Location = new Point(btnGameMenu.Left + 80, 90),
                    Anchor = AnchorStyles.Right | AnchorStyles.Top,
                    Visible = false,
                };

                this.Controls.Add(panelGameMenu);

                panelGameMenu.BringToFront();
            }

            if (!varBtnGameMenu)
            {
                Button btnPlayGame = new Button()
                {
                    Text = "Play",
                    ForeColor = Color.White,
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                btnPlayGame.Click += BtnPlayGame_Click;


                Button btnGameFolder = new Button()
                {
                    Text = "Open File Location",
                    ForeColor = Color.White,
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 35),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                btnGameFolder.Click += BtnGameFolder_Click;

                Button btnRemoveGame = new Button()
                {
                    Text = "Remove Game",
                    ForeColor = Color.White,
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 70),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                btnRemoveGame.Click += BtnRemoveGame_Click;

                btnRestoreGame = new Button()
                {
                    Text = "Restore Game",
                    ForeColor = Color.White,
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 105),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                    Visible = false
                };
                btnRestoreGame.Click += BtnRestoreGame_Click;

                panelGameMenu.Controls.Add(btnPlayGame);
                panelGameMenu.Controls.Add(btnGameFolder);
                panelGameMenu.Controls.Add(btnRemoveGame);
                panelGameMenu.Controls.Add(btnRestoreGame);
                panelGameMenu.Refresh();
                varBtnGameMenu = true;
            }

            panelGameMenu.Visible = !panelGameMenu.Visible;
        }

        private void btnLibrarySettings_Click(object sender, EventArgs e)
        {
            if (!File.Exists(jsonScanPath))
            {
                string defaultJson = @"[]";

                try
                {
                    File.WriteAllText(jsonScanPath, defaultJson);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create the game scan file, please restart the Utility.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string jsonContent = File.ReadAllText(jsonScanPath).Trim();

            if (panelLibrarySettings == null)
            {
                panelLibrarySettings = new Panel()
                {
                    ForeColor = Color.White,
                    Size = new Size(175, 70),
                    BackColor = Color.Gray,
                    Location = new Point(btnLibrarySettings.Left, 90),
                    Anchor = AnchorStyles.Right | AnchorStyles.Top,
                    Visible = false,
                };

                this.Controls.Add(panelLibrarySettings);

                panelLibrarySettings.BringToFront();
            }

            if (!varBtnLibrarySettings)
            {
                Button btnRefresh = new Button()
                {
                    Text = "Refresh",
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                toolTip1.SetToolTip(btnRefresh, "Rescan the default game installation paths");
                btnRefresh.Click += BtnRefresh_Click;

                Button btnScanLocation = new Button()
                {
                    Text = "Add Scan Location",
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 35),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                toolTip1.SetToolTip(btnScanLocation, "Searches for compatible games in the selected path; select the path where the game's folder is located, not the game folder itself");
                btnScanLocation.Click += BtnScanLocation_Click;

                Button btnLocations = new Button()
                {
                    Text = "View Scan Locations",
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 70),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                toolTip1.SetToolTip(btnLocations, "Displays the manually added scan paths");
                btnLocations.Click += BtnLocations_Click;

                Button btnClearScan = new Button()
                {
                    Text = "Clear Scan Locations ",
                    ForeColor = Color.White,
                    Size = new Size(175, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    Location = new Point(0, 105),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Lucida Sans Unicode", 9),
                };
                toolTip1.SetToolTip(btnClearScan, "Removes all manually added paths.");
                btnClearScan.Click += BtnClearScan_Click;

                if (jsonContent != "[]")
                {
                    panelLibrarySettings.Height += 70;
                }

                panelLibrarySettings.Controls.Add(btnRefresh);
                panelLibrarySettings.Controls.Add(btnScanLocation);
                panelLibrarySettings.Controls.Add(btnLocations);
                panelLibrarySettings.Controls.Add(btnClearScan);
                panelLibrarySettings.Refresh();

                varBtnLibrarySettings = true;
            }

            panelLibrarySettings.Visible = !panelLibrarySettings.Visible;

            if (panelScanPaths != null && panelScanPaths.Visible && !panelLibrarySettings.Visible)
            {
                panelScanPaths.Visible = false;
            }
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            // Rescan the default game installation paths
            gamesLibrary.Clear();

            var main = (mainForm)this.ParentForm;
            main.ScanGames();

            var jsonPath = Path.Combine(rootExeDir, "user_scan_paths.json");
            List<string> pathsJson = [];

            if (File.Exists(jsonPath))
            {
                try
                {
                    pathsJson = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(jsonPath)) ?? [];
                }
                catch (JsonException)
                {
                    try { File.Delete(jsonPath); } catch { }

                    if (panelLibrarySettings.Height == 105)
                    {
                        panelLibrarySettings.Height -= 35;
                    }

                    MessageBox.Show("It was not possible to load/save the paths because the configuration file was corrupted. The file has been removed; please try adding/updating the paths again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (checkBoxPreset.Checked)
            {
                checkBoxPreset.Checked = false;
            }

            main.ScanGames(possiblePaths: pathsJson);

            await gamesIcons();
            panelIcon.Refresh();
        }
        private void BtnScanLocation_Click(object? sender, EventArgs e)
        {
            using (var scanLocationDialog = new FolderBrowserDialog())
            {
                if (scanLocationDialog.ShowDialog() == DialogResult.OK)
                {
                    string userScanLocation = scanLocationDialog.SelectedPath;

                    if (File.Exists(jsonScanPath))
                    {
                        string varScanJson = File.ReadAllText(jsonScanPath);

                        pathScanLocation = JsonSerializer.Deserialize<List<string>>(varScanJson) ?? new List<string>();
                    }
                    else
                    {
                        pathScanLocation = new List<string>();
                    }

                    if (!pathScanLocation.Contains(userScanLocation))
                    {
                        pathScanLocation.Add(userScanLocation);
                    }

                    File.WriteAllText(jsonScanPath, JsonSerializer.Serialize(pathScanLocation, new JsonSerializerOptions { WriteIndented = true })); // Saves the selected paths in 'scanLocationDialog' to a JSON file. The saved paths are loaded in the 'SetGamesLibrary' method when this form is opened

                    if (File.Exists(jsonScanPath) && !string.IsNullOrWhiteSpace(File.ReadAllText(jsonScanPath)) && panelLibrarySettings.Height < 105)
                    {
                        panelLibrarySettings.Height += 70;
                    }

                    if (panelScanPaths != null && panelScanPaths.Visible)
                    {
                        panelScanPaths.Refresh();
                        panelScanPaths.Visible = false;
                    }

                    ((mainForm)this.ParentForm).ScanGames(gameNames: null, possiblePaths: pathScanLocation);

                    gamesIcons();
                    panelIcon.Refresh();
                }
            }
        }
        private void BtnLocations_Click(object? sender, EventArgs e)
        {
            if (!File.Exists(jsonScanPath)) return;

            List<string> jsonPaths;
            try
            {
                string jsonContent = File.ReadAllText(jsonScanPath).Trim();
                if (jsonContent == "[]") return;
                jsonPaths = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();
            }
            catch
            {
                return;
            }

            if (panelScanPaths == null)
            {
                panelScanPaths = new Panel
                {
                    ForeColor = Color.White,
                    Size = new Size(175, 105),
                    BackColor = Color.Gray,
                    Location = new Point(panelLibrarySettings.Right, 90),
                    Anchor = AnchorStyles.Right | AnchorStyles.Top,
                    AutoScroll = true,
                    Visible = false
                };

                this.Controls.Add(panelScanPaths);
                panelScanPaths.BringToFront();
            }

            void RefreshScanPathPanel()
            {
                panelScanPaths.Controls.Clear();
                int y = 0;
                bool cntJsonPaths = jsonPaths.Count >= 4;

                foreach (var path in jsonPaths)
                {
                    var labelPaths = new System.Windows.Forms.Label
                    {
                        Text = path,
                        Location = new Point(0, y),
                        Size = new Size(cntJsonPaths ? panelScanPaths.Width - 45 : panelScanPaths.Width - 25, 25),
                        TextAlign = ContentAlignment.MiddleLeft,
                        BackColor = Color.DarkGray,
                        ForeColor = Color.Black
                    };
                    toolTip1.SetToolTip(labelPaths, path);

                    var btnRemove = new Button
                    {
                        Text = "X",
                        Size = new Size(25, 25),
                        Location = new Point(labelPaths.Width, labelPaths.Top),
                        BackColor = Color.DarkGray,
                        ForeColor = Color.Black,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnRemove.FlatAppearance.BorderSize = 0;

                    string pathToRemove = path;
                    btnRemove.Click += (s, ev) =>
                    {
                        if (jsonPaths.Remove(pathToRemove))
                        {
                            File.WriteAllText(jsonScanPath, JsonSerializer.Serialize(jsonPaths, new JsonSerializerOptions { WriteIndented = true }));
                            RefreshScanPathPanel();
                        }

                        if (jsonPaths.Count == 0)
                        {
                            panelScanPaths.Visible = false;
                            panelLibrarySettings.Height -= 70;
                        }
                    };

                    panelScanPaths.Controls.Add(labelPaths);
                    panelScanPaths.Controls.Add(btnRemove);
                    y += labelPaths.Height + 5;
                }

                panelScanPaths.AutoScrollPosition = new Point(0, 0);
                panelScanPaths.Refresh();
            }

            RefreshScanPathPanel();
            panelScanPaths.Visible = !panelScanPaths.Visible;
        }

        private void BtnClearScan_Click(object sender, EventArgs e)
        {
            string scanPathJson = Path.Combine(rootExeDir, "user_scan_paths.json");

            if (File.Exists(scanPathJson))
            {
                File.Delete(scanPathJson);

                panelLibrarySettings.Height -= 70;

                if (panelScanPaths.Visible)
                {
                    panelScanPaths.Visible = false;
                }

                MessageBox.Show("Paths removed successfully", "Scan");
            }
        }
        private void BtnPlayGame_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                {
                    Process.Start(exePath);
                }
            }
            catch
            {
                MessageBox.Show("Error executing, please try manually.", "Error", MessageBoxButtons.OK);
            }
        }
        private void BtnGameFolder_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(exePath));
            }
            else
            {
                MessageBox.Show("Path not found, please open the path manually", "Path not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnRemoveGame_Click(object? sender, EventArgs e)
        {

            if (selectedButton != null)
            {
                int index = panelIcon.Controls.GetChildIndex(selectedButton, false);

                if (!buttonsRemoved.Contains(selectedButton))
                {
                    buttonsRemoved.Push(selectedButton);
                }

                selectedButton.Visible = false;
                selectedButton = null;

                Button nextBtn = null;

                for (int i = index; i < panelIcon.Controls.Count; i++)
                {
                    if (panelIcon.Controls[i] is Button btn && btn.Visible)
                    {
                        nextBtn = btn;
                        break;
                    }
                }

                // Try the previous button if the next one is not found
                if (nextBtn == null)
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        if (panelIcon.Controls[i] is Button btn && btn.Visible)
                        {
                            nextBtn = btn;
                            break;
                        }
                    }
                }

                updatedGamesCount--;
                labelGamesCount.Text = ($"{updatedGamesCount}/{gamesCount}").ToString();

                int totalHeight = 0;

                foreach (Control ctrl in panelIcon.Controls)
                {
                    if (ctrl is Button btn && btn.Visible)
                    {
                        totalHeight += btn.Height + 5;
                    }
                }

                buttonsRemoved.Push(selectedButton);

                panelIcon.AutoScrollMinSize = new Size(0, totalHeight);
                panelIcon.PerformLayout();
                panelIcon.Invalidate();
                panelIcon.Update();

                if (nextBtn != null)
                {
                    nextBtn.PerformClick();
                }
                else
                {
                    labelModName.Visible = false;
                    labelOnSig.Visible = false;
                    labelFolderName.Visible = false;
                    labelCheckGuide.Visible = false;
                    labelGameIcon.Visible = false;
                    picGameIcon.Visible = false;
                }

                if (btnRestoreGame != null && !btnRestoreGame.Visible)
                {
                    panelGameMenu.Height += 35;
                    btnRestoreGame.Visible = true;
                }
            }
        }
        private void BtnRestoreGame_Click(object? sender, EventArgs e)
        {

            while (buttonsRemoved.Count > 0)
            {
                Button btnRestored = buttonsRemoved.Pop();

                if (btnRestored != null)
                {
                    btnRestored.Visible = true;
                    btnRestored.PerformClick();

                    updatedGamesCount++;
                    labelGamesCount.Text = ($"{updatedGamesCount}/{gamesCount}").ToString();

                    int totalHeight = 0;
                    foreach (Control ctrl in panelIcon.Controls)
                    {
                        if (ctrl is Button btn && btn.Visible)
                            totalHeight += btn.Height + 5;
                    }
                    panelIcon.AutoScrollMinSize = new Size(0, totalHeight);
                    panelIcon.PerformLayout();
                    panelIcon.Invalidate();
                    panelIcon.Update();

                    labelModName.Visible = true;
                    labelOnSig.Visible = true;
                    labelFolderName.Visible = true;
                    labelCheckGuide.Visible = true;
                    labelGameIcon.Visible = true;
                    picGameIcon.Visible = true;

                    // Hide the "Restore" button if no game has been "Removed"
                    if (buttonsRemoved.Count == 0)
                    {
                        btnRestoreGame.Visible = false;
                        panelGameMenu.Height -= 35;
                    }

                    break;
                }
            }
        }
        private void btnGuide_Click(object sender, EventArgs e)
        {
            string gameName = labelGameIcon.Text;
            var tempGuide = new formGuide();
            bool gameExists = tempGuide.ShowGameGuide(gameName);
            tempGuide.Dispose();

            if (gameExists)
            {
                ((mainForm)this.ParentForm).loadForm(typeof(formGuide), selectedGame: gameName);
            }
            else
            {
                MessageBox.Show("This game does not have a guide. Please refer to the Optiscaler FSR3.1.3/DLSS (Only Optiscaler) guide and perform the standard installation.", "Guide");
            }
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            ((mainForm)this.ParentForm).loadForm(typeof(formSettings));
        }

        private async void checkBoxPreset_CheckedChanged(object sender, EventArgs e)
        {
            var princForm = (mainForm)this.ParentForm;
            using (var tempHome = new formHome())
            {
                if (checkBoxPreset.Checked)
                {
                    var gamesModsConfig = tempHome.ReturnGamesMods();
                    bool gameFound = gamesModsConfig.Keys.Any(key => key.Contains(labelGameIcon.Text));

                    if (gameFound)
                    {
                        await tempHome.SelectGameWp(labelGameIcon.Text);
                    }
                    else
                    {
                        if (MessageBox.Show("This game does not have a specific preset. Please install the mod manually. Refer to the guide if you need help.\r\n\r\nWould you like to keep the default settings in the \"Mods Settings\" section? (The installation folder path for the selected game will be added, along with the default mod (FSR 3.1.4/DLSS — note that the mod may not work; it is recommended to check the guide and manually select the mod mentioned there), and \"Enable Signature Override\" will be enabled.)", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            await tempHome.SelectGameWp(labelGameIcon.Text);
                        }
                        else
                        {
                            checkBoxPreset.Checked = false;
                        }
                    }
                }
                else
                {
                    tempHome.SelectGameWp(cleanSettingsCtrl: true);
                    checkBoxDlssOverlay.Checked = false;
                }
            }

            FormsControl();

            if (!checkBoxPreset.Checked && princForm.HomeSettings != null)
            {
                princForm.HomeSettings.cleanCtrl(true);
            }
        }
        private void checkBoxDlssOverlay_CheckedChanged(object sender, EventArgs e)
        {
            FormsControl();
        }
        private void FormsControl()
        {
            var princForm = (mainForm)this.ParentForm;

            princForm.valueForSettings = labelFolderName.Text;
            princForm.valueSigForSettings = checkBoxPreset.Checked;
            princForm.flagValueTextBox1Settings = checkBoxPreset.Checked;
            princForm.valueDlssOverlayForSettings = checkBoxPreset.Checked && checkBoxDlssOverlay.Checked;

            if (princForm.HomeSettings != null)
            {
                princForm.HomeSettings.TextBox1Text = labelFolderName.Text;

                bool enableSig = checkBoxPreset.Checked ? checkBoxDlssOverlay.Checked : false;
                princForm.HomeSettings.CheckEnableSig(null, enableSig);

                if (labelOnSig.Text == "Yes")
                {
                    princForm.HomeSettings.CheckEnableSig(checkBoxPreset.Checked, null);
                }

                princForm.HomeSettings.Refresh();
            }

            if (!string.IsNullOrWhiteSpace(labelModName.Text))
            {
                string[] modsInfo = labelModName.Text.Split(new[] { " or " }, StringSplitOptions.None);
                string selectedModPreset = modsInfo.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

                if (!string.IsNullOrWhiteSpace(selectedModPreset) && princForm.flagValueTextBox1Settings)
                {
                    princForm.valueListModsForSettings = selectedModPreset;
                  
                    if (princForm.HomeSettings != null)
                    {
                        princForm.HomeSettings.listModsValue = selectedModPreset;
                        princForm.HomeSettings.Refresh();
                    }
                }
            }
        }
    }
}
