using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formHome : Form
    {
        private Image panelBackgroundG;
        public formHome()
        {
            InitializeComponent();
            panelBG.Paint += new PaintEventHandler(panelBG_Paint);
     
     
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
    }
}
