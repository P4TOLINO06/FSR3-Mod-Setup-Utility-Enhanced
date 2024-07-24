using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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
                    string backgroundPicture = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Wallpaper", imageName);

                    panelBackgroundG = Image.FromFile(backgroundPicture);
                    panelBG.Invalidate();

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
                    {"A Plague Tale Requiem","APTL.png"},
                    {"Achilles Legends Untold","Achilles.png"},
                    {"Alan Wake 2","AW2.png"},
                    {"Alone in the Dark","Alone.png"},
                    {"Assassin's Creed Mirage","Ac.png"},
                    {"Assassin's Creed Valhalla","Valhalla.png"},
                    {"Atomic Heart","Atomic.png"},
                    {"Baldur's Gate 3","BG3.png"},
                    {"Banishers: Ghosts of New Eden","Banishers.png" },
                    {"Blacktail","Black.png" },
                    {"Bright Memory: Infinite","Bmi.png"},
                    {"Brothers: A Tale of Two Sons Remake","Brothers.png" },
                    {"Chernobylite","Cherno.png"},
                    {"Cod Black Ops Cold War","Cod.png"},
                    {"Control","Control.png"},
                    {"Crime Boss: Rockay City","Rockay.png"},
                    {"Cyberpunk 2077","Cyber.png"},
                    {"Dakar Desert Rally","Dakar.png"},
                    {"Dead Island 2","Dead2.png"},
                    {"Dead Space (2023)","DeadSpace.png"},
                    {"Death Stranding Director's Cut","Ds.png"},
                    {"Deathloop","Df.png"},
                    {"Dragons Dogma 2","Dg2.png"},
                    {"Dying Light 2","Dl2.png"},
                    {"Elden Ring","Elden.png"},
                    {"Everspace 2","Es2.png"},
                    {"Evil West","Ew.png"},
                    {"F1 2022","F1.png"},
                    {"F1 2023","F1_23.png"},
                    {"FIST: Forged In Shadow Torch","Fist.png"},
                    {"Fort Solis","Fort.png"},
                    {"Forza Horizon 5","Forza.png"},
                    {"Ghost of Tsushima","GhostT.png"},
                    {"Ghostrunner 2","Ghost2.png"},
                    {"GTA V","GtaV.png"},
                    {"Hellblade 2","Hell2.png" },
                    {"Hellblade: Senua's Sacrifice","Hell.png"},
                    {"High On Life","Hol.png"},
                    {"Hitman 3","Hitman.png"},
                    {"Hogwarts Legacy","Hog.png"},
                    {"Horizon Forbidden West","HZDF.png"},
                    {"Horizon Zero Dawn","Hzd.png"},
                    {"Icarus","Icarus.png"},
                    {"Judgment","Jud.png"},
                    {"Jusant","Jusant.png"},
                    {"Kena: Bridge of Spirits","KENA.png"},
                    {"Layers of Fear","Layers.png"},
                    {"Lies of P","Lop.png"},
                    {"Loopmancer","Loopmancer.png"},
                    {"Lords of the Fallen","Lotf.png"},
                    {"Manor Lords","Manor.png"},
                    {"Martha Is Dead","Martha.png"},
                    {"Marvel's Guardians of the Galaxy","Got.png"},
                    {"Marvel's Spider-Man Miles Morales","Miles.png"},
                    {"Marvel's Spider-Man Remastered","Spider.png"},
                    {"Metro Exodus Enhanced Edition","Metro.png"},
                    {"Monster Hunter Rise","Mrise.png"},
                    {"MOTO GP 24","Moto.png"},
                    {"Nightingale","Night.png"},
                    {"Outpost: Infinity Siege","Outpost.png"},
                    {"Pacific Drive","Pacific.png"},
                    {"Palworld","Palworld.png"},
                    {"Ratchet & Clank - Rift Apart","Ratchet.png"},
                    {"Ready or Not","Ready.png"},
                    {"Red Dead Redemption 2","RDR2.png"},
                    {"Remnant II","Remnant2.png"},
                    {"Returnal","Returnal.png"},
                    {"Ripout","Ripout.png"},
                    {"Rise of The Tomb Raider","Rtb.png"},
                    {"RoboCop: Rogue City","RoboCop.png"},
                    {"Sackboy: A Big Adventure","Sackboy.png"},
                    {"Saints Row","SaintsRow.png"},
                    {"Satisfactory","SatsF.png"},
                    {"Shadow of the Tomb Raider","ShadowTomb.png"},
                    {"Shadow Warrior 3","Shadow3.png"},
                    {"Smalland","Smalland.png"},
                    {"STAR WARS Jedi: Survivor","JedSurvivor.png"},
                    {"STARFIELD","Starfield.png"},
                    {"Steelrising","Steelrising.png"},
                    {"TEKKEN 8","Tekken.png"},
                    {"The Callisto Protocol","Callisto.png"},
                    {"The Chant","Chant.png"},
                    {"The Invincible","Invicible.png"},
                    {"The Last of Us Part I","Tlou.png"},
                    {"The Medium","Medium.png"},
                    {"The Outer Worlds: Spacer's Choice Edition","Outer.png"},
                    { "The Thaumaturge","Thaumaturge.png"},
                    {"The Witcher 3","Witcher.png"},
                    {"Uncharted Legacy of Thieves Collection","Uncharted.png"},
                    {"Wanted: Dead","Wanted.png"},
            };
            #endregion

            foreach (var gameName in gamesToAdd)
            {
                path_images.Add(gameName.Key, gameName.Value);
            }

            object? gameGet = listGames.SelectedItem;

            if (gameGet != null)
            {
                string? selectedGame = gameGet.ToString();

                if (path_images.ContainsKey(selectedGame))
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

            #region Games List
            List<string> rdr2B2List = new List<string> { "RDR2 Build_2", "RDR2 Build_4", "RDR2 Mix", "RDR2 Mix 2", "Red Dead Redemption V2", "RDR2 Non Steam FSR3", "RDR2 FSR 3.1 FG" };
            List<string> EldenList = new List<string> { "Elden Ring FSR3","Elden Ring FSR3 V2", "Elden Ring FSR3 V3", "Disable Anti Cheat"};
            List<string> Aw2List = new List<string> { "Alan Wake 2 FG RTX", "Alan Wake 2 Uniscaler Custom", "Optiscaler FSR 3.1/DLSS" };
            List<string> AcValhallaList = new List<string> { "Ac Valhalla Dlss (Only RTX)", "AC Valhalla FSR3 All GPU" };
            List<string> bdg3List = new List<string> { "Baldur's Gate 3 FSR3", "Baldur's Gate 3 FSR3 V2", "Baldur's Gate 3 FSR3 V3" };
            List<string> dd2List = new List<string> { "Dinput8", "Uniscaler_DD2", "Uniscaler V2", "Uniscaler V3", "Uniscaler + Xess + Dlss DD2"};
            List<string> callistoList = new List<string> { "The Callisto Protocol FSR3" };
            List<string> gtavList = new List<string> { "Dinput8", "GTA V FSR3", "GTA V FiveM", "GTA V Online", "GTA V Epic", "GTA V Epic V2" };
            List<string> cyberList = new List<string> { "RTX DLSS FG CB2077", "Optiscaler FSR 3.1/DLSS" };
            List<string> gotList = new List<string> { "Ghost of Tsushima FG DLSS" };
            List<string> pwList = new List<string> { "Palworld FG Build03" };
            List<string> jediList = new List<string> { "DLSS Jedi" };
            List<string> tekkenList = new List<string> { "Unlock FPS Tekken 8" };
            List<string> icarusiList = new List<string> { "RTX DLSS FG ICR", "FSR3 FG ICR All GPU" };
            List<string> lotfList = new List<string> { "Lords of The Fallen DLSS RTX", "Lords of The Fallen FSR3 ALL GPU" };
            List<string> forzaList = new List<string> { "RTX DLSS FG FZ5", "FSR3 FG FZ5 All GPU", "Optiscaler FSR 3.1/DLSS" };
            var modsDefaultList = new List<string> { "0.7.4", "0.7.5", "0.7.6", "0.8.0", "0.9.0",
                                 "0.10.0", "0.10.1", "0.10.1h1", "0.10.2h1", "0.10.3","0.10.4", "Uniscaler", "Uniscaler V2", "Uniscaler V3","Uniscaler FSR 3.1","Uniscaler + Xess + Dlss", "Optiscaler FSR 3.1/DLSS"};
            #endregion;

            #region List To Remove
            List<List<string>> listToRemove = new List<List<string>> 
            {
                rdr2B2List,
                EldenList,
                Aw2List,
                AcValhallaList,
                bdg3List,
                dd2List,
                callistoList,
                gtavList,
                cyberList,
                forzaList,
                gotList,
                lotfList,
                pwList,
                jediList,
                icarusiList,
                tekkenList
            };
            #endregion

            if (listGames.SelectedItem.ToString() == "Red Dead Redemption 2")
            {
                formSettings.Instance.AddItemlistMods(rdr2B2List);
            }
            else if(listGames.SelectedItem.ToString() == "Elden Ring")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(EldenList);
            }
            else if(listGames.SelectedItem.ToString() == "Alan Wake 2")
            {
                formSettings.Instance.AddItemlistMods(Aw2List);
            }
            else if (listGames.SelectedItem.ToString() == "Assassin's Creed Valhalla")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(AcValhallaList);
            }
            else if(listGames.SelectedItem.ToString() == "Baldur's Gate 3")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(bdg3List);
            }
            else if(listGames.SelectedItem.ToString() == "Dragons Dogma 2")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(dd2List);
            }
            else if (listGames.SelectedItem.ToString() == "The Callisto Protocol")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(callistoList);
            }
            else if (listGames.SelectedItem.ToString() == "GTA V")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(gtavList);
            }
            else if (listGames.SelectedItem.ToString() == "Cyberpunk 2077")
            {
                formSettings.Instance.AddItemlistMods(cyberList);
            }
            else if (listGames.SelectedItem.ToString() == "Forza Horizon 5")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(forzaList);
            }
            else if (listGames.SelectedItem.ToString() == "Ghost of Tsushima")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(gotList);
            }
            else if (listGames.SelectedItem.ToString() == "Lords of the Fallen")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(lotfList);
            }
            else if (listGames.SelectedItem.ToString() == "Palworld")
            {
                formSettings.Instance.AddItemlistMods(pwList);
            }
            else if (listGames.SelectedItem.ToString() == "STAR WARS Jedi: Survivor")
            {
                formSettings.Instance.AddItemlistMods(jediList);
            }
            else if (listGames.SelectedItem.ToString() == "Icarus")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(icarusiList);
            }
            else if (listGames.SelectedItem.ToString() == "TEKKEN 8")
            {
                formSettings.Instance.ClearListMods();
                formSettings.Instance.AddItemlistMods(tekkenList);
            }
            else
            {
                foreach (var lists in listToRemove) 
                {
                    formSettings.Instance.RemoveItemlistMods(lists);
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

        private void formHome_Resize(object sender, EventArgs e)
        {

        }

        private void listGames_Enter(object sender, EventArgs e)
        {

        }

        private void mainSelectGame_Enter(object sender, EventArgs e)
        {
        }

        private void listGames_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void mainSelectGame_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listFSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFsr = listFSR.SelectedItem.ToString();
            formSettings.fsrSelected = selectedFsr;

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
