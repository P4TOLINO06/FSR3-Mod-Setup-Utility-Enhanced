using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formHome : Form
    {
        private Image panelBackgroundG;
        private formSettings settingsForm;

        public formHome()
        {
            InitializeComponent();
            panelBG.Paint += new PaintEventHandler(panelBG_Paint);  
            settingsForm = new formSettings();
        }

        private void formHome_Load(object sender, EventArgs e)
        {

        }

        private void searchImage(string imageName, string gameName)
        {
            object? selectGame = listGames.SelectedItem;
            if (selectGame != null)
            {
                if (selectGame.Equals(gameName))
                {
                    string backgroundPicture = Path.Combine(
                        Path.GetDirectoryName(Application.ExecutablePath)!,
                        "Images\\Wallpaper",
                        imageName
                    );

                    if (File.Exists(backgroundPicture))
                    {
                        try
                        {
                            Image resizedImage = ResizeImageForScreen(backgroundPicture);

                            panelBackgroundG = resizedImage;
                            panelBG.Invalidate();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
        private Image ResizeImageForScreen(string imagePath)
        {
            using (Image originalImage = Image.FromFile(imagePath))
            {
                var screenWidth = Screen.PrimaryScreen.Bounds.Width;
                var screenHeight = Screen.PrimaryScreen.Bounds.Height;

                Bitmap resizedImage = new Bitmap(screenWidth, screenHeight);

                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    graphics.DrawImage(originalImage, 0, 0, screenWidth, screenHeight);
                }

                return resizedImage;
            }
        }

        private void buttonSelectGame_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> path_images = new Dictionary<string, string>();

            #region Background Images formHome
            Dictionary<string, string> gamesToAdd = new Dictionary<string, string>
            {
                    {"Select FSR Version","Ds2.png" },
                    {"A Plague Tale Requiem","APTL.png"},
                    {"A Quiet Place: The Road Ahead","QuietPlace.png"},
                    {"Achilles Legends Untold","Achilles.png"},
                    {"Alan Wake Remastered","AwRemaster.png"},
                    {"Alan Wake 2","AW2.png"},
                    {"Alone in the Dark","Alone.png"},
                    {"Assassin's Creed Mirage","Ac.png"},
                    {"Assassin's Creed Valhalla","Valhalla.png"},
                    {"Atomic Heart","Atomic.png"},
                    {"Baldur's Gate 3","BG3.png"},
                    {"Banishers: Ghosts of New Eden","Banishers.png" },
                    {"Black Myth: Wukong","wukong.png"},
                    {"Blacktail","Black.png" },
                    {"Bright Memory: Infinite","Bmi.png"},
                    {"Brothers: A Tale of Two Sons Remake","Brothers.png" },
                    {"Chernobylite","Cherno.png"},
                    {"Cod Black Ops Cold War","Cod.png"},
                    {"Cod MW3","mw3.png"},
                    {"Control","Control.png"},
                    {"Crime Boss: Rockay City","Rockay.png"},
                    {"Crysis 3 Remastered", "Crysis.png"},
                    {"Cyberpunk 2077","Cyber.png"},
                    {"Dakar Desert Rally","Dakar.png"},
                    {"Dead Island 2","Dead2.png"},
                    {"Dead Rising Remaster","Drr.png"},
                    {"Dead Space (2023)","DeadSpace.png"},
                    {"Death Stranding Director's Cut","Ds.png"},
                    {"Deathloop","Df.png"},
                    {"Dragon Age: Veilguard","DgVeil.png"},
                    {"Dragons Dogma 2","Dg2.png"},
                    {"Dying Light 2","Dl2.png"},
                    {"Elden Ring","Elden.png"},
                    {"Everspace 2","Es2.png"},
                    {"Evil West","Ew.png"},
                    {"Final Fantasy XVI","Ffxvi.png"},
                    {"F1 2022","F1.png"},
                    {"F1 2023","F1_23.png"},
                    {"FIST: Forged In Shadow Torch","Fist.png"},
                    {"Flintlock: The Siege Of Dawn","Flint.png"},
                    {"Fort Solis","Fort.png"},
                    {"Forza Horizon 5","Forza.png"},
                    {"Ghost of Tsushima","GhostT.png"},
                    {"Ghostrunner 2","Ghost2.png"},
                    {"Ghostwire: Tokyo","Ghostwire.png"},
                    {"God Of War 4","Gow4.png"},
                    {"God of War Ragnarök","GowRag2.png"},
                    {"Gotham Knights","Gk.png"},
                    {"GTA Trilogy","GtaTrilogy.png"},
                    {"GTA V","GtaV.png"},
                    {"Hellblade 2","Hell2.png" },
                    {"Hellblade: Senua's Sacrifice","Hell.png"},
                    {"High On Life","Hol.png"},
                    {"Hitman 3","Hitman.png"},
                    {"Hogwarts Legacy","Hog.png"},
                    {"Horizon Forbidden West","HZDF.png"},
                    {"Horizon Zero Dawn","Hzd.png"},
                    {"Horizon Zero Dawn Remastered","HzdRem.png"},
                    {"Icarus","Icarus.png"},
                    {"Indiana Jones and the Great Circle","Indy.png"},
                    {"Judgment","Jud.png"},
                    {"Jusant","Jusant.png"},
                    {"Kena: Bridge of Spirits","KENA.png"},
                    {"Layers of Fear","Layers.png"},
                    {"Lego Horizon Adventures","LegoHzd.png"},
                    {"Lies of P","Lop.png"},
                    {"Loopmancer","Loopmancer.png"},
                    {"Lords of the Fallen","Lotf.png"},
                    {"Manor Lords","Manor.png"},
                    {"Martha Is Dead","Martha.png"},
                    {"Marvel's Avengers","Avengers.png"},
                    {"Marvel's Guardians of the Galaxy","Got.png"},
                    {"Marvel's Spider-Man Miles Morales","Miles.png"},
                    {"Marvel's Spider-Man Remastered","Spider.png"},
                    {"Marvel\'s Midnight Suns","Mmds.png"},
                    {"Metro Exodus Enhanced Edition","Metro.png"},
                    {"Monster Hunter Rise","Mrise.png"},
                    {"Microsoft Flight Simulator 24","FlightSimulator24.png"},
                    {"MOTO GP 24","Moto.png"},
                    {"Mortal Shell","MShell.png"},
                    {"Nightingale","Night.png"},
                    {"Nobody Wants To Die","Nobody.png"},
                    {"Outpost: Infinity Siege","Outpost.png"},
                    {"Pacific Drive","Pacific.png"},
                    {"Palworld","Palworld.png"},
                    {"Path of Exile II","POE2.png"},
                    {"Ratchet & Clank - Rift Apart","Ratchet.png"},
                    {"Ready or Not","Ready.png"},
                    {"Red Dead Redemption","Rdr1.png"},
                    {"Red Dead Redemption 2","RDR2.png"},
                    {"Remnant II","Remnant2.png"},
                    {"Resident Evil 4 Remake","Re4.png"},
                    {"Returnal","Returnal.png"},
                    {"Ripout","Ripout.png"},
                    {"Rise of The Tomb Raider","Rtb.png"},
                    {"RoboCop: Rogue City","RoboCop.png"},
                    {"Sackboy: A Big Adventure","Sackboy.png"},
                    {"Saints Row","SaintsRow.png"},
                    {"Satisfactory","SatsF.png"},
                    {"Shadow of the Tomb Raider","ShadowTomb.png"},
                    {"Shadow Warrior 3","Shadow3.png"},
                    {"Sifu","Sifu.png"},
                    {"Silent Hill 2","Sh2.png"},
                    {"Smalland","Smalland.png"},
                    {"Stalker 2","Stalker.png"},
                    {"STAR WARS Jedi: Survivor","JedSurvivor.png"},
                    {"Star Wars Outlaws","Outlaws.png"},
                    {"STARFIELD","Starfield.png"},
                    {"Steelrising","Steelrising.png"},
                    {"Suicide Squad: Kill the Justice League","Sskjl.png"},
                    {"TEKKEN 8","Tekken.png"},
                    {"Test Drive Unlimited Solar Crown","TestSolar.png"},
                    {"The Ascent","Ascent.png"},
                    {"The Callisto Protocol","Callisto.png"},
                    {"The Chant","Chant.png"},
                    {"The Casting Of Frank Stone","FrankStone.png"},
                    {"The Invincible","Invicible.png"},
                    {"The Last of Us Part I","Tlou.png"},
                    {"The Medium","Medium.png"},
                    {"The Outer Worlds: Spacer's Choice Edition","Outer.png"},
                    {"The Thaumaturge","Thaumaturge.png"},
                    {"The Witcher 3","Witcher.png"},
                    {"Uncharted Legacy of Thieves Collection","Uncharted.png"},
                    {"Unknown 9: Awakening","Unknown9.png"},
                    {"Until Dawn","Until.png"},
                    {"Wanted: Dead","Wanted.png"},
                    {"Warhammer: Space Marine 2","SpaceMarine.png"},
                    {"Watch Dogs Legion","Legion.png"}
            };
            #endregion

            foreach(var gameName in gamesToAdd)
            {
                path_images.Add(gameName.Key, gameName.Value);
            }

            object? gameGet = listGames.SelectedItem;

            if (gameGet != null)
            {
                string? selectedGame = gameGet.ToString();

                if (!string.IsNullOrEmpty(selectedGame) && path_images.ContainsKey(selectedGame))
                {
                    string imagePath = path_images[selectedGame];

                    searchImage(imagePath, selectedGame);
                }
            }

            string gamesSelected = null;
            formSettings.gameSelected = gamesSelected;

            if (listGames.SelectedItem != null)
            {
                gamesSelected = listGames.SelectedItem.ToString();
            }

            if (gamesSelected != null)
            {
                formSettings.gameSelected = gamesSelected;
            }

            if (gamesSelected == "Select FSR Version")
            {
                panelSelectFSR.Visible = true;
                listFSR.Visible = true;
            }
            else
            {
                panelSelectFSR.Visible = false;
                listFSR.Visible = false;
            }

            #region Game List

            var modsDefaultList = new List<string> { "FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS","0.7.4", "0.7.5", "0.7.6", "0.8.0", "0.9.0",
                                 "0.10.0", "0.10.1", "0.10.1h1", "0.10.2h1", "0.10.3","0.10.4", "Uniscaler", "Uniscaler V2", "Uniscaler V3","Uniscaler V4","Uniscaler FSR 3.1","Uniscaler + Xess + Dlss"};

            var gamesModsConfig = new Dictionary<string, List<string>>
            {
                { "Red Dead Redemption 2", new List<string> { "FSR 3.1.3/DLSS FG Custom RDR2", "RDR2 Mix", "RDR2 FG Custom", "FSR 3.1.1/DLSS FG Custom", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Elden Ring", new List<string> { "Elden Ring FSR3", "Elden Ring FSR3 V2", "FSR 3.1.3/DLSS FG Custom Elden", "Disable Anti Cheat", "Unlock FPS Elden" } },
                { "Alan Wake 2", new List<string> { "Alan Wake 2 FG RTX", "Alan Wake 2 Uniscaler Custom", "Optiscaler FSR 3.1.1/DLSS", "Uniscaler FSR 3.1", "Others Mods AW2" } },
                { "Assassin's Creed Valhalla", new List<string> { "Ac Valhalla Dlss (Only RTX)", "AC Valhalla FSR3 All GPU" } },
                { "Baldur's Gate 3", new List<string> { "Baldur's Gate 3 FSR3", "Baldur's Gate 3 FSR3 V2", "Baldur's Gate 3 FSR3 V3" } },
                { "Dragons Dogma 2", new List<string> { "Dinput8 DD2", "FSR 3.1.1/DLSS FG Custom" } },
                { "The Callisto Protocol", new List<string> { "The Callisto Protocol FSR3", "FSR 3.1.1/DLSS Callisto", "FSR 3.1.3/DLSS Custom Callisto", "0.10.4", "Uniscaler V3" } },
                { "GTA V", new List<string> { "Dinput8", "GTA V FSR3", "GTA V FiveM", "GTA V Online", "GTA V Epic", "GTA V Epic V2" } },
                { "Cyberpunk 2077", new List<string> { "Others Mods 2077", "RTX DLSS FG CB2077", "FSR 3.1.3/XESS FG 2077", "Optiscaler FSR 3.1.1/DLSS", "Uniscaler FSR 3.1" } },
                { "Ghost of Tsushima", new List<string> { "Ghost of Tsushima FG DLSS", "Optiscaler FSR 3.1.1/DLSS", "Uniscaler FSR 3.1" } },
                { "Lords of the Fallen", new List<string> { "Lords of The Fallen DLSS RTX", "Lords of The Fallen FSR3 ALL GPU" } },
                { "Palworld", new List<string> { "Palworld FG Build03" } },
                { "STAR WARS Jedi: Survivor", new List<string> { "DLSS Jedi" } },
                { "Icarus", new List<string> { "RTX DLSS FG ICR", "FSR3 FG ICR All GPU" } },
                { "TEKKEN 8", new List<string> { "Unlock FPS Tekken 8" } },
                { "Flintlock: The Siege Of Dawn", new List<string> { "Optiscaler FSR 3.1.1/DLSS" } },
                { "Cod MW3", new List<string> { "COD MW3 FSR3" } },
                { "God Of War 4", new List<string> { "Gow 4 FSR 3.1", "FSR 3.1.1/DLSS FG Custom" } },
                { "God of War Ragnarök", new List<string> { "Others Mods Gow Rag", "FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS", "Uniscaler FSR 3.1" } },
                { "Warhammer: Space Marine 2", new List<string> { "Others Mods Space Marine", "FSR 3.1.3/DLSS FG Marine", "FSR 3.1.1/DLSS FG Custom", "Uniscaler FSR 3.1", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Black Myth: Wukong", new List<string> { "Others Mods Wukong", "DLSS FG (ALL GPUs) Wukong", "FSR 3.1 Custom Wukong" } },
                { "Final Fantasy XVI", new List<string> { "FFXVI DLSS RTX", "Others Mods FFXVI" } },
                { "Forza Horizon 5", new List<string>  { "RTX DLSS FG FZ5", "FSR3 FG FZ5 All GPU", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Star Wars Outlaws", new List<string> { "Outlaws DLSS RTX", "FSR 3.1.1/DLSS FG Custom" } },
                { "The Casting Of Frank Stone", new List<string> { "0.10.4", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Silent Hill 2", new List<string> { "FSR 3.1.1/DLSS FG Custom", "Optiscaler FSR 3.1.1/DLSS", "FSR 3.1.1/DLSS FG RTX Custom", "DLSS FG RTX", "Ultra Plus Complete", "Ultra Plus Optimized", "FSR3 FG Native SH2", "FSR3 FG Native SH2 + Optimization", "Others Mods Sh2" } },
                { "Until Dawn", new List<string> { "Others Mods UD" } },
                { "Hogwarts Legacy", new List<string> { "Others Mods HL" } },
                { "A Quiet Place: The Road Ahead", new List<string> { "FSR 3.1.1/DLSS Quiet Place", "FSR 3.1.1/DLSS FG Custom", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Metro Exodus Enhanced Edition", new List<string> { "Others Mods Metro" } },
                { "Red Dead Redemption", new List<string> { "Others Mods RDR" } },
                { "Horizon Zero Dawn Remastered", new List<string> { "FSR 3.1.3 HZD Rem", "Others Mods HZD Rem" } },
                { "Dragon Age: Veilguard", new List<string> { "FSR 3.1.3/DLSS DG Veil", "Others Mods DG Veil" } },
                { "Dying Light 2", new List<string> { "FSR 3.1.3 Custom DL2" } },
                { "Dead Rising Remaster", new List<string> { "Dinput8 DRR", "FSR 3.1 FG DRR" } },
                { "Saints Row", new List<string> { "FSR 3.1.3/DLSS Custom SR" } },
                { "GTA Trilogy", new List<string> { "FSR 3.1.3/DLSS Custom GTA" } },
                { "Assassin's Creed Mirage", new List<string> { "Others Mods Mirage" } },
                { "Alan Wake Remastered", new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Lego Horizon Adventures", new List<string> { "Others Mods Lego HZD" } },
                { "Stalker 2", new List<string> { "Others Mods Stalker 2", "DLSS FG (Only Nvidia)" } },
                { "Returnal", new List<string> { "Others Mods Returnal" } },
                { "The Last of Us Part I", new List<string> { "Others Mods Tlou" } },
                { "Marvel\\'s Spider-Man Miles Morales", new List<string> { "Others Mods Spider" } },
                { "Marvel\\'s Spider-Man Remastered", new List<string> { "Others Mods Spider" } },
                { "Microsoft Flight Simulator 24", new List<string> { "FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Shadow of the Tomb Raider", new List<string> { "Others Mods Shadow Tomb" } },
                { "Gotham Knights", new List<string> { "Others Mods GK" } },
                { "Indiana Jones and the Great Circle", new List<string> { "Others Mods Indy", "Indy FG (Only RTX)" } },
                { "Suicide Squad: Kill the Justice League", new List<string> { "Others Mods SSKJL", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Resident Evil 4 Remake", new List<string> { "FSR 3.1.3/DLSS RE4" } },
                { "Sifu", new List<string> { "Others Mods Sifu", "FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS"}},
                { "Path of Exile II", new List<string> { "Others Mods POEII", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Steelrising", new List<string> { "Others Mods Steel"}},
                { "Mortal Shell", new List<string> {"Others Mods MShell", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Hitman 3", new List<string> { "Others Mods Hitman 3", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Control", new List<string> { "Others Mods Control"} },
                { "Remnant II", new List<string> { "Others Mods Remnant II", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "FIST: Forged In Shadow Torch", new List<string> { "Others Mods Fist"} },
                { "Ghostrunner 2", new List<string> { "Others Mods GR2" } },
                { "Marvel\'s Midnight Suns", new List<string> {"FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS"} },
                { "Hellblade 2", new List<string> {"Others Mods HB2","HB2 FG (Only RTX)"} }
            };
            #endregion

            if (gamesModsConfig.ContainsKey(gamesSelected))
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(gamesModsConfig[gamesSelected], modsDefaultList);
            }
            else
            {
                foreach (var lists in modsDefaultList)
                {
                    formSettings.Instance.RemoveItemlistMods(modsDefaultList);
                }
                formSettings.Instance.AddItemlistMods(modsDefaultList);
            }

        }

        private void panelBG_Paint(object sender, PaintEventArgs e)
        {
            if (panelBackgroundG != null)
            {
                Graphics g = e.Graphics;
                g.DrawImage(panelBackgroundG, new Rectangle(0, 0, panelBG.Width, panelBG.Height));
            }
        }

        private void listFSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFsr = listFSR.SelectedItem.ToString();
            formSettings.fsrSelected = selectedFsr;

        }
    }
}
