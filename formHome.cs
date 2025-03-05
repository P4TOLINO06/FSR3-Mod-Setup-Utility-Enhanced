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
                            using (Image originalImage = Image.FromFile(backgroundPicture))
                            {
                                panelBackgroundG = new Bitmap(originalImage);
                                panelBG.Invalidate();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }

        private void buttonSelectGame_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> path_images = new Dictionary<string, string>();

            #region Background Images formHome
            Dictionary<string, string> gamesToAdd = new Dictionary<string, string>
            {
                    {"Select FSR Version","Ds2.png" },
                    {"A Plague Tale Requiem","Requiem.png"},
                    {"A Quiet Place: The Road Ahead","QuietPlace.png"},
                    {"Achilles Legends Untold","Achilles.png"},
                    {"Alan Wake Remastered","AwRemaster.png"},
                    {"Alan Wake 2","AW2.png"},
                    {"Alone in the Dark","Alone.png"},
                    {"Assassin's Creed Mirage","Ac.png"},
                    {"Assassin's Creed Valhalla","Valhalla.png"},
                    {"Assetto Corsa Evo","ACE.png"},
                    {"Atomic Heart","Atomic.png"},
                    {"AVOWED","AWD.png"},
                    {"Back 4 Blood","B4B.png"},
                    {"Baldur's Gate 3","BG3.png"},
                    {"Banishers: Ghosts of New Eden","Banishers.png" },
                    {"Black Myth: Wukong","wukong.png"},
                    {"Blacktail","Black.png"},
                    {"Bright Memory","BM.png"},
                    {"Bright Memory: Infinite","Bmi.png"},
                    {"Brothers: A Tale of Two Sons Remake","Brothers.png" },
                    {"Chernobylite","Cherno.png"},
                    {"Choo-Choo Charles","CCC.png"},
                    {"Chorus","Chorus.png"},
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
                    {"Deliver Us Mars","DUM.png"},
                    {"Deliver Us The Moon","DUTM.png"},
                    {"Dragon Age: Veilguard","DgVeil.png"},
                    {"Dragons Dogma 2","Dg2.png"},
                    {"Dying Light 2","Dl2.png"},
                    {"Dynasty Warriors: Origins","DWO.png"},
                    {"Elden Ring","Elden.png"},
                    {"Empire of the Ants","Eota.png"},
                    {"Everspace 2","Es2.png"},
                    {"Evil West","Ew.png"},
                    {"Eternal Strands","Ets.png"},
                    {"Final Fantasy VII Rebirth","FFVIIRBT.png"},
                    {"Final Fantasy XVI","Ffxvi.png"},
                    {"Five Nights at Freddy’s: Security Breach","FNAF.png"},
                    {"F1 2022","F1.png"},
                    {"F1 2023","F1_23.png"},
                    {"FIST: Forged In Shadow Torch","Fist.png"},
                    {"Flintlock: The Siege Of Dawn","Flint.png"},
                    {"Fobia – St. Dinfna Hotel","Fobia.png"},
                    {"Fort Solis","Fort.png"},
                    {"Frostpunk 2","FP2.png"},
                    {"Forza Horizon 5","Forza.png"},
                    {"Ghost of Tsushima","GhostT.png"},
                    {"Ghostrunner 2","Ghost2.png"},
                    {"Ghostwire: Tokyo","Ghostwire.png"},
                    {"God Of War 4","Gow4.png"},
                    {"God of War Ragnarök","GowRag2.png"},
                    {"Gotham Knights","Gk.png"},
                    {"GreedFall II: The Dying World","GF2.png"},
                    {"GTA Trilogy","GtaTrilogy.png"},
                    {"GTA V","GtaV.png"},
                    {"Hellblade 2","Hell2.png" },
                    {"Hellblade: Senua's Sacrifice","Hell.png"},
                    {"High On Life","Hol.png"},
                    {"Hitman 3","Hitman.png"},
                    {"Hogwarts Legacy","Hog.png"},
                    {"Horizon Forbidden West","HZDF.png"},
                    {"Horizon Zero Dawn\\Remastered","Hzd.png"},
                    {"Hot Wheels Unleashed","Hwu.png"},
                    {"Icarus","Icarus.png"},
                    {"Indiana Jones and the Great Circle","Indy.png"},
                    {"Judgment","Jud.png"},
                    {"Jusant","Jusant.png"},
                    {"Kingdom Come: Deliverance II","KCD2.png"},
                    {"Kena: Bridge of Spirits","KENA.png"},
                    {"Layers of Fear","Layers.png"},
                    {"Lego Horizon Adventures","LegoHzd.png"},
                    {"Lies of P","Lop.png"},
                    {"Like a Dragon: Pirate Yakuza in Hawaii","LDPYH.png"},
                    {"Loopmancer","Loopmancer.png"},
                    {"Lords of the Fallen","Lotf.png"},
                    {"Lost Records Bloom And Rage","LRBR.png"},
                    {"Manor Lords","Manor.png"},
                    {"Martha Is Dead","Martha.png"},
                    {"Marvel's Avengers","Avengers.png"},
                    {"Marvel's Guardians of the Galaxy","Got.png"},
                    {"Marvel's Spider-Man Miles Morales","Miles.png"},
                    {"Marvel's Spider-Man Remastered","Spider.png"},
                    {"Marvel's Spider-Man 2","SpiderMan2.png"},
                    {"Marvel\'s Midnight Suns","Mmds.png"},
                    {"Metro Exodus Enhanced Edition","Metro.png"},
                    {"Monster Hunter Rise","Mrise.png"},
                    {"Microsoft Flight Simulator 24","FlightSimulator24.png"},
                    {"Monster Hunter Wilds","MHW.png"},
                    {"MOTO GP 24","Moto.png"},
                    {"Mortal Shell","MShell.png"},
                    {"Nightingale","Night.png"},
                    {"Ninja Gaiden 2 Black","NinjaG.png"},
                    {"Nobody Wants To Die","Nobody.png"},
                    {"Orcs Must Die! Deathtrap","Omdd.png"},
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
                    {"Scorn","Scorn.png"},
                    {"Sengoku Dynasty","SG.png"},
                    {"Shadow of the Tomb Raider","ShadowTomb.png"},
                    {"Shadow Warrior 3","Shadow3.png"},
                    {"Sifu","Sifu.png"},
                    {"Silent Hill 2","Sh2.png"},
                    {"Six Days in Fallujah","6Days.png"},
                    {"Smalland","Smalland.png"},
                    {"Soulslinger Envoy of Death","SL.png"},
                    {"Stalker 2","Stalker.png"},
                    {"STAR WARS Jedi: Survivor","JedSurvivor.png"},
                    {"Star Wars Outlaws","Outlaws.png"},
                    {"STARFIELD","Starfield.png"},
                    {"Steelrising","Steelrising.png"},
                    {"Soulstice","STC.png"},
                    {"Suicide Squad: Kill the Justice League","Sskjl.png"},
                    {"TEKKEN 8","Tekken.png"},
                    {"Test Drive Unlimited Solar Crown","TestSolar.png"},
                    {"The Ascent","Ascent.png"},
                    {"The Callisto Protocol","Callisto.png"},
                    {"The Chant","Chant.png"},
                    {"The Casting Of Frank Stone","FrankStone.png"},
                    {"The First Berserker: Khazan","TFBK.png"},
                    {"The Invincible","Invicible.png"},
                    {"The Last Of Us Part I","TLOU.png"},
                    {"The Medium","Medium.png"},
                    {"The Outer Worlds: Spacer's Choice Edition","Outer.png"},
                    {"The Talos Principle 2","Ttp.png"},
                    {"The Thaumaturge","Thaumaturge.png"},
                    {"The Witcher 3","Witcher.png"},
                    {"Thymesia","Thymesia.png"},
                    {"Uncharted Legacy of Thieves Collection","Uncharted.png"},
                    {"Unknown 9: Awakening","Unknown9.png"},
                    {"Until Dawn","Until.png"},
                    {"Wanted: Dead","Wanted.png"},
                    {"Warhammer: Space Marine 2","SpaceMarine.png"},
                    {"Watch Dogs Legion","Legion.png"},
                    {"Way Of The Hunter","Woth.png"},
                    {"Wayfinder","Wayfinder.png"}
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

            var modsDefaultList = new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler)", "FSR 3.1.3/DLSSG FG (Only Optiscaler)", "FSR 3.1.1/DLSS FG Custom", "Optiscaler FSR 3.1.1/DLSS","0.7.4", "0.7.5", "0.7.6", "0.8.0", "0.9.0",
                                 "0.10.0", "0.10.1", "0.10.1h1", "0.10.2h1", "0.10.3","0.10.4", "Uniscaler", "Uniscaler V2", "Uniscaler V3","Uniscaler V4","Uniscaler FSR 3.1","Uniscaler + Xess + Dlss"};

            List<string> fsr31DlssMods = new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler)", "FSR 3.1.3/DLSSG FG (Only Optiscaler)", "FSR 3.1.2/DLSS FG Custom", "FSR 3.1.1/DLSS Optiscaler" };

            var gamesModsConfig = new Dictionary<string, List<string>>
            {
                { "Red Dead Redemption 2", new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler RDR2)", "RDR2 Mix", "RDR2 FG Custom", "FSR 3.1.1/DLSS FG Custom", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Elden Ring", new List<string> { "Elden Ring FSR3", "Elden Ring FSR3 V2", "FSR 3.1.3/DLSS FG Custom Elden", "Disable Anti Cheat", "Unlock FPS Elden" } },
                { "Alan Wake 2", new List<string> { "Others Mods AW2", "Alan Wake 2 FG RTX", "Alan Wake 2 Uniscaler Custom"} },
                { "Assassin's Creed Valhalla", new List<string> { "Ac Valhalla Dlss (Only RTX)", "AC Valhalla FSR3 All GPU" } },
                { "Baldur's Gate 3", new List<string> { "Baldur's Gate 3 FSR3", "Baldur's Gate 3 FSR3 V2", "Baldur's Gate 3 FSR3 V3" } },
                { "Dragons Dogma 2", new List<string> { "Dinput8 DD2", "FSR 3.1.1/DLSS FG Custom" } },
                { "The Callisto Protocol", new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler)", "The Callisto Protocol FSR3", "FSR 3.1.3/DLSS Custom Callisto", "0.10.4", "Uniscaler V4" } },
                { "GTA V", new List<string> {"Others Mods GTA V", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "GTA V FiveM", "GTA V Online", "GTA V Epic", "GTA V Epic V2" } },
                { "Cyberpunk 2077", new List<string> { "Others Mods 2077", "RTX DLSS FG CB2077", "FSR 3.1.3/XESS FG 2077"} },
                { "Ghost of Tsushima", new List<string> { "Ghost of Tsushima FG DLSS", "Optiscaler FSR 3.1.1/DLSS", "Uniscaler FSR 3.1" } },
                { "Lords of the Fallen", new List<string> { "Lords of The Fallen DLSS RTX", "Lords of The Fallen FSR3 ALL GPU" } },
                { "Palworld", new List<string> { "Others Mods PW", "Palworld FG Build03" } },
                { "STAR WARS Jedi: Survivor",  new List <string> { "Others Mods Jedi" }.Concat(fsr31DlssMods).ToList() },
                { "Icarus", new List<string> { "RTX DLSS FG ICR", "FSR3 FG ICR All GPU" } },
                { "TEKKEN 8", new List<string> { "Unlock FPS Tekken 8" } },
                { "Cod MW3", new List<string> { "COD MW3 FSR3" } },
                { "God Of War 4", new List<string> { "Others Mods Gow4", "FSR 3.1.3/DLSS Gow4"} },
                { "God of War Ragnarök", new List<string> { "Others Mods Gow Rag", "Uniscaler FSR 3.1" }.Concat(fsr31DlssMods).ToList() },
                { "Warhammer: Space Marine 2", new List<string> { "Others Mods Space Marine", "FSR 3.1.3/DLSS FG Marine", "Uniscaler FSR 3.1" }.Concat(fsr31DlssMods).ToList() },
                { "Black Myth: Wukong", new List<string> { "Others Mods Wukong", "DLSS FG (ALL GPUs) Wukong" }.Concat(fsr31DlssMods).ToList() },
                { "Final Fantasy XVI", new List<string> { "FFXVI DLSS RTX", "Others Mods FFXVI" } },
                { "Forza Horizon 5", new List<string>  { "RTX DLSS FG FZ5", "FSR3 FG FZ5 All GPU", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Star Wars Outlaws", new List<string> { "Outlaws DLSS RTX" }.Concat(fsr31DlssMods).ToList() },
                { "The Casting Of Frank Stone", new List<string> { "0.10.4", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Silent Hill 2", new List<string> { "Others Mods Sh2", "FSR 3.1.1/DLSS FG RTX Custom", "DLSS FG RTX", "Ultra Plus Complete", "Ultra Plus Optimized", "FSR3 FG Native SH2", "FSR3 FG Native SH2 + Optimization"}.Concat(fsr31DlssMods).ToList()  },
                { "Until Dawn", new List<string> { "Others Mods UD" }.Concat(fsr31DlssMods).ToList() },
                { "Hogwarts Legacy", new List<string> { "Others Mods HL" }.Concat(fsr31DlssMods).ToList() },
                { "A Quiet Place: The Road Ahead", new List<string> { "FSR 3.1.1/DLSS Quiet Place" }.Concat(fsr31DlssMods).ToList() },
                { "Metro Exodus Enhanced Edition", new List<string> { "Others Mods Metro" }.Concat(fsr31DlssMods).ToList() },
                { "Red Dead Redemption", new List<string> { "Others Mods RDR" } },
                { "Horizon Zero Dawn\\Remastered", new List<string> {"Others Mods HZD"}.Concat(fsr31DlssMods).ToList() },
                { "Dragon Age: Veilguard", new List<string> { "FSR 3.1.3/DLSS DG Veil", "Others Mods DG Veil" } },
                { "Dead Rising Remaster", new List<string> { "Dinput8 DRR", "FSR 3.1 FG DRR" } },
                { "GTA Trilogy", new List<string> { "Others GTA Trilogy" }.Concat(fsr31DlssMods).ToList() },
                { "Assassin's Creed Mirage", new List<string> { "Others Mods Mirage" } },
                { "Alan Wake Remastered", new List<string> { "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Lego Horizon Adventures", new List<string> { "Others Mods Lego HZD" } },
                { "Stalker 2", new List<string> { "Others Mods Stalker 2", "DLSS FG (Only Nvidia)" } },
                { "Returnal", new List<string> { "Others Mods Returnal" } },
                { "The Last Of Us Part I", new List<string> { "Others Mods Tlou" } },
                { "Marvel\'s Spider-Man Miles Morales", new List<string> { "Others Mods Spider" } },
                { "Marvel\'s Spider-Man Remastered", new List<string> { "Others Mods Spider" } },
                { "Marvel\'s Spider-Man 2", new List<string> { "Others Mods Spider" } },
                { "Microsoft Flight Simulator 24", new List<string> { "FSR 3.1.1/DLSS FG Custom", "FSR 3.1.3/DLSS FG (Only Optiscaler)", "Optiscaler FSR 3.1.1/DLSS" } },
                { "Shadow of the Tomb Raider", new List<string> { "Others Mods Shadow Tomb" } },
                { "Gotham Knights", new List<string> { "Others Mods GK" } },
                { "Indiana Jones and the Great Circle", new List<string> { "Others Mods Indy", "Indy FG (Only RTX)", "FSR 3.1.3/DLSS FG (Only Optiscaler Indy" } },
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
                { "Hellblade 2", new List<string> {"Others Mods HB2","HB2 FG (Only RTX)"} },
                { "Six Days in Fallujah", new List<string> {"Others Mods 6Days", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Way Of The Hunter", new List<string> { "Others Mods WOTH", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Alone in the Dark", new List<string> {"Others Mods AITD"} },
                { "Evil West", new List<string> {"Others Mods EW"} },
                { "The First Berserker: Khazan", new List<string> { "Others Mods TFBK"} },
                { "Assetto Corsa Evo", new List<string> { "Others Mods ACE"} },
                { "Soulstice", new List<string> { "Others Mods STC"} },
                { "Watch Dogs Legion", new List<string> { "Others Mods Legion"} },
                { "Back 4 Blood", new List<string> {"Others Mods B4B"} },
                { "Final Fantasy VII Rebirth", new List<string> { "Others Mods FF7RBT" } },
                { "Lies of P", new List<string> { "Others Mods LOP" }.Concat(fsr31DlssMods).ToList() },
                { "Kingdom Come: Deliverance II", new List<string> {"Others Mods KCD2"} },
                { "Atomic Heart", new List<string> { "Others Mods ATH"} },
                { "Monster Hunter Wilds", new List<string> {"Others Mods MHW", "DLSSG Wilds (Only RTX)" }.Concat(fsr31DlssMods).ToList() },
                { "Like a Dragon: Pirate Yakuza in Hawaii", new List<string> { "Others Mods LDPYH", "DLSSG Yakuza" } },
                { "A Plague Tale Requiem", new List<string> {"Others Mods Requiem"} },
                { "Fobia – St. Dinfna Hotel", new List<string> {"Others Mods Fobia", "FSR 3.1.3/DLSS FG (Only Optiscaler)" } },
                { "Frostpunk 2", new List<string> {"Others Mods FP2" } },
                { "Bright Memory",  new List<string> {"Others Mods BM" } },
                { "Choo-Choo Charles",  new List<string> {"Others Mods CCC" } },
                { "Lost Records Bloom And Rage",  new List<string> {"Others Mods LRBR" } },
                { "GreedFall II: The Dying World", new List<string> { "Others Mods Greed 2" } },
                { "Five Nights at Freddy’s: Security Breach", new List<string> {"Others Mods FNAF"} },
                { "Pacific Drive", new List<string> {"Others Mods PD"} },
                { "Chernobylite", new List<string> { "Others Mods CBL" } },
                { "Chorus", new List<string> { "Others Mods Chorus"} },
                { "Deliver Us Mars", new List<string> { "Others Mods DUM"}.Concat(fsr31DlssMods).ToList() },
                { "Deliver Us The Moon", new List<string> { "Others Mods DUTM"}.Concat(fsr31DlssMods).ToList() },
                { "Dying Light 2", new List<string> { "Others Mods DL2"} },
                { "Kena: Bridge of Spirits", new List<string> { "Others Mods Kena" } },
                { "The Witcher 3", new List<string> { "Others Mods TW3"} }

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
