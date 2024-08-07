using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formGuide : Form
    {
        private Image panelBG;
        public formGuide()
        {
            InitializeComponent();
            listBox1.Visible = false;
            panelImage.Paint += new PaintEventHandler(panelImage_Paint);
        }

        #region Guides
        private Dictionary<string, string> guideTexts = new Dictionary<string, string>
        {
            {
                "Initial Information", "1 - When selecting the game folder, look for the game's .exe file. Some games have the full name .exe or abbreviated, while others have the .exe file in the game folder but within subfolders with the ending Binaries\\Win64, and the .exe usually ends with Win64-Shipping, for example: TheCallistoProtocol-Win64-Shipping.\r\n" +
                      "2 - It is recommended to read the guide before installing the mod. Some games do not have a guide because you only need to install the mod, while others, like Fallout 4 for example, have extra steps for installation.\r\n" +
                      "If something is done incorrectly, the mod will not work.\r\n" +
                      "3 - Some games may not work for certain users after installing the mod. It is recommended to select Default in NVGX and enable Signature Override if the mod does not work with the default files.\r\n" +
                      "4 - Games that don't have numbers in 'Fsr' you don't need to check any option, just install the mod."
            },
            {
                "Add-on Mods","OptiScaler\r\nIs drop-in DLSS2 to XeSS/FSR2 replacement for games. OptiScaler implements all necessary API methods of DLSS2 & NVAPI to act as a man in the middle. So from games perspective it’s using DLSS2 but actually using OptiScaler and calls are interpreted/redirected to XeSS & FSR2.\r\n\r\n"+
                "Tweak\r\nHelps 'improve' aliasing caused by FSR 3 mod, may also slightly reduce ghosting, doesn't work in all games.\r\n"
            },
            {
                "Optiscaler Method","Installation Method for Optiscaler\r\n" +
                "Method Default: Default installation method. (Recommended for testing).\r\n"+
                "Method 1 (RTX/RX 6000-7000): Installation method for RTX and RX 6xxx/7xxx series GPUs.\r\n"+
                "Method 2 (GTX/Old AMD): Installation method for older GPUs such as GTX and RX 5000 and below.\r\n"+
                "Method 3 (If none of the others work): Modified installation method if none of the other options work.\r\n"
            },
            {
                "Achilles Legends Untold",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n"+
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX only).\r\n"+
                "3 - If the mod doesn't work, check the Nvapi Results box and select Default in NVNGX.dll.\r\n"+
                "4 - In-game, select DLSS."
            },
            {
                "Alan Wake 2",
                "RTX\r\n 1 - Select Alan Wake 2 FG RTX and install it.\r\n"+
                "2 - In the game, select DLSS and enable Frame Generation.\r\n"+
                "3 - It is also possible to use other versions of the mod.\r\n\r\n"+
                "AMD/GTX\r\n 1 - Select Alan Wake 2 Uniscaler Custom and install it.\r\n"+
                "2 - In the game, select DLSS and enable Frame Generation if it is not enabled by default.\r\n"+
                "3 - Do not switch to FSR as the game will crash.\r\n"+
                "4 - It is also possible to use other versions of the mod, except Alan Wake 2 FG RTX.\r\n\r\n"+
                "Optiscaler FSR 3.1/DLSS\r\n" +
                "1 - Select Optiscaler FSR 3.1/DLSS.\r\n"+
                "2 - In-game, press the Insert key to open the menu and select your preferred upscaler.\r\n"+
                "3 - If the menu does not appear, you can select the upscaler during installation. Choose Optiscaler and under Addon Upscaler, select your preferred upscaler."
            },
            {
                "Alone in the Dark",
                "1 - Select a version of the mod of your choice (version 0.10.3 is recommended).\r\n"+
                "2 - Enable the 'Enable Signature Override' checkbox.\r\n"+
                "3 - Enable Fake Nvidia GPU, if you want to use DLSS (Only for AMD GPUs).\r\n"+
                "4 - Set FSR in the game settings.\r\n"+
                "5 - If the mod doesn't work, select 'Default' in Nvngx.dll."
            }
        };

        #endregion

        private void formGuide_Load(object sender, EventArgs e)
        {
            panel1.Left = (panel3.Width - panel1.Width) / 2;
            panel1.Top = (panel3.Height - panel1.Height) / 2;

            panel4.Left = (panel1.Width - panel4.Width) / 2;
            panel4.Top = (panel1.Height - panel4.Height) / 2;

            panelImage.Left = panel4.Left + (panel4.Width - panelImage.Width) / 2;
            panelImage.Top = panel4.Top - panelImage.Height - 5;

            panel2.Left = panel1.Left + (panel1.Width - panel2.Width) / 2;
            panel2.Top = panel1.Top - panel2.Height - 5;

            richTextBox1.Enter += (sender, e) => //Remove the insertion cursor
            {
                richTextBox1.Parent.Focus();
            };
        }

        private void searchImage(string imageName, string guideName)
        {
            object? selectGuide = listBox1.Text;
            if (selectGuide != null)
            {
                if (selectGuide.Equals(guideName))
                {
                    string bgPicture = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Wallpaper", imageName);

                    panelBG = Image.FromFile(bgPicture);
                    panelImage.Invalidate();

                }
            }
        }

        private void panelImage_Paint(object sender, PaintEventArgs e)
        {
            if (panelBG != null)
            {
                Graphics g = e.Graphics;
                Rectangle destRect = new Rectangle(0, 0, panelImage.Width, panelImage.Height);
                g.DrawImage(panelBG, destRect);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, string> pathImages = new Dictionary<string, string>();

            #region Background Images formGuide
            Dictionary<string, string> gamesToAdd = new Dictionary<string, string>
            {
                {"Initial Information","Sett.png"},
                {"Achilles Legends Untold","Aut.png"},
                {"Alan Wake 2","AW2.png"},
                {"Optiscaler Method","Opti.png"},
                {"Add-on Mods","Addon.png" },
                {"Alone in the Dark","Alone.png"},
            };
            #endregion

            foreach (var guideName in gamesToAdd)
            {
                pathImages.Add(guideName.Key, guideName.Value);
            }

            object? guideGet = listBox1.Text;

            if (guideGet != null)
            {
                string? selectedGuide = guideGet.ToString();

                if (pathImages.ContainsKey(selectedGuide))
                {
                    string imagePath = pathImages[selectedGuide];

                    searchImage(imagePath, selectedGuide);
                }
            }

            if (listBox1.SelectedItem != null)
            {
                string optGuide = listBox1.SelectedItem.ToString();
                if (guideTexts.TryGetValue(optGuide, out string guideText))
                {
                    richTextBox1.Text = guideText;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Visible)
            {
                listBox1.Visible = false;
            }
            else
            {
                listBox1.Visible = true;
            }
        }
    }
}