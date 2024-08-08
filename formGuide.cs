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
                "Optiscaler FSR 3.1/DLSS\r\n",
                "This mod may not work with all games, so it is recommended to perform tests.\r\n" +
                "Select Optiscaler FSR 3.1/DLSS and install it.\r\n" +
                "In the game, press the Insert key to open the menu. In the menu, select your preferred upscaler. You can also increase image sharpness by enabling the Sharpness option in the menu.\r\n" +
                "If the menu does not appear, during the mod installation, check the Optiscaler box and select the desired upscaler under Add-on Upscaler, then install."
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
            },
            {
                "A Plague Tale Requiem",
                "1 - Select a mod of your choice (0.10.3 is recommended).\r\n"+
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX) and Nvapi Results (GTX). (If the mod doesn't work for AMD, also check Nvapi Results).\r\n"+
                "3 - To fix hub flickering, enable DLSS and Frame Generation and play for a few seconds, then disable DLSS and leave only Frame Generation enabled."
            },
            {
                "Assassin's Creed Valhalla",
                "1 - Press the \"End\" key to open the Frame Gen menu or the\r\n" +
                "\"Home\" key to open the main menu.\r\n" +
                "2 - Select AC Valhalla DLSS3\r\n" +
                "3 - In the game, enable Motion Blur and disable FSR"
            },
            {
                "Atomic Heart",
                "1 - Select a mod of your choice (0.10.3 is recommended).\r\n" +
                "2 - In the game, select FSR."
            },
            {
                "Baldur's Gate 3",
                "1 - Start the game in DX11 and select Borderless.\r\n" +
                "2 - Choose DLSS or DLAA.\r\n" +
                "3 - Press the END key to enter the mod menu, check the Frame Generation box to activate the mod; you can also adjust the Upscaler. (To activate Frame Generation, simply press the * key; you can also change the key in the mod menu.)"
            },
            {
                "Blacktail",
                "1 - Select a mod of your choice (0.10.3 is recommended).\r\n" +
                "2 - Check the Fake Nvidia GPU box."
            },
            {
                "Bright Memory: Infinite",
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - To make the mod work, run it in DX12. To run it in DX12, right-click the game's exe and create a shortcut, then right-click the shortcut again, go to \"Properties,\" and at the end of \"Target\" (outside the quotes), add -dx12 or go to your Steam library, select the game, go to Settings > Properties > Startup options, and enter -dx12."
            },
            {
                "Brothers a Tale of Two Sons",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\r\n" +
                "In-game, select DLSS or FSR."
            },
            {
                "Chernobylite",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box 'Fake Nvidia GPU' (only for AMD/GTX).\r\n" +
                "3 - To fix flickering in the hub, select DLSS, play for a few seconds, then select FSR3 and repeat the process, finally select DLSS."

            },
            {
                "Cod Black Ops Cold War",
                "1 - Select a mod of your choice. (recommended 0.10.3)\r\n" +
                "2 - Check the box Fake Nvidia GPU.\r\n" +
                "3 - If you don't see any differences, check the box for Nvapi Results."
            },
            {
                "Cod MW3",
                "1 - Select the game path: CallofDuty\\Content\\sp23\r\n" +
                "2 - Select the COD MW3 FSR3 mod and install it\r\n" +
                "3 - In the game, select DLSS Frame Generation"
            },
            {
                "Control", 
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the Fake Nvidia GPU box. (AMD/GTX).\r\n" +
                "3 - Check the Enable Signature Override box.\r\n" +
                "4 - Before installing, configure the game as you wish, do not change the settings or turn off DLSS after the mod is installed, as the game will crash."
            },
            {
                "Crime Boss Rockay City",
                "1 - Select a mod of your choice (0.10.4 is recommended).\r\n" +
                "2 - Check the Fake Nvidia GPU box for AMD/GTX users.\r\n" +
                "If you can't see DLSS in the game, check the Nvapi Results and UE Compatibility Mode boxes.\r\n" +
                "3 - In the game, turn off Anti-Aliasing and select DLSS as the upscaler."
            },
            {
                "Cyberpunk 2077",
                "1 - Select a mod of your choice (Uniscaler is recommended).\r\n" +
                "2 - Select Default in Nvngx.dll.\r\n" +
                "3 - Check the box Enable Signature Override.\r\n" +
                "4 - In-game, turn off Vsync, select DLSS (do not select auto as the game will crash), and turn on Frame Generation."
            },
            {
                "Dakar Desert Rally",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box Fake Nvidia GPU and Nvapi Results (AMD/GTX).\r\n" +
                "In-game, select DLSS and Frame Generation."
            },
            {
                "Dead Island 2",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - If it doesn't work with the default files, enable Enable Signature Override. If it still doesn't work, check the box lfz.sl.dlss.\r\n" +
                "3 - It's not necessary to activate an upscaler for this game for the mod to work, so enable it if you want."
            },
            {
                "Death Stranding Director's Cut",
                "1 - Select a mod of your preference (0.10.3 or Uniscaler is recommended).\r\n" +
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - Inside the game, select DLSS or FSR.\r\n" +
                "4 - If you encounter problems related to DX12, select D3D12 in Dxgi.dll.\r\n" +
                "5 - The mod only works on the Director's Cut version."
            },
            {
                "Dead Space Remake",
                "1 - Select a version of the mod of your choice (versions from 0.9.0 onwards are recommended to fix UI flickering).\r\n" +
                "2 - Enable the 'Enable Signature Override' checkbox if the mod doesn't work.\r\n" +
                "3 - Enable Fake Nvidia GPU (Only for AMD GPUs).\r\n" +
                "4 - If the mod doesn't work, select 'Default' in Nvngx.dll."
            },
            {
                "Deathloop",
                "1 - Select a version of the mod of your choice (version 0.10.3 is recommended).\r\n" +
                "2 - Activate Fake Nvidia GPU and Nvapi Results (Only for AMD and GTX)."
            },
            {
                "Dragons Dogma 2",
                "1 - Select Deput8 in Mod Select and install.\r\n" +
                "2 - Open the game after Deput8 is installed, a \"REFramework\" menu will appear. Click on it, go to Settings and Menu Key, click on Menu Key, and select the preferred key (the key is used to open and close the menu).\r\n" +
                "3 - Close the game, in Utility select Uniscaler_DD2 in Mod Version and install (it is recommended to select \"Yes\" when the message to delete the shader file appears).\r\n" +
                "4 - Inside the game, select FSR3 to enable the mod.\r\n" +
                "• It is recommended to turn off any type of upscaler before opening the game with the mod.\r\n" +
                "• To fix the HUD, select Dynamic Resolution and turn off FSR3 (after turning it on for the first time), this will slightly decrease the FPS."
            },
            {
                "Dying Light 2",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Enable Fake Nvidia GPU (only for AMD and GTX).\r\n" +
                "3 - In the game, select any upscaler and activate Frame Generation.\r\n" +
                "4 - If you experience any flickering or ghosting, go to Video > Advanced Settings and decrease the Lod Range Multiplier.\r\n\r\n"+
                "DL2 DLSS FG\r\n" +
                "1 - Select DL2 DLSS FG and install it.\r\n" +
                "2 - Select Frame Generation within the game.\r\n" +
                "3 - This mod cannot fix the HUD error, but you can reduce it slightly by decreasing the HUD size and enabling motion blur."
            },
            {
                "Elden Ring",
                "1 - Select \"Disable AntiCheat\" in the Select Mod and choose \"Yes\" in the anticheat deactivation confirmation window. Select the folder where the game exe is located, otherwise, it will not be possible to deactivate the anticheat. (Steam Only)\r\n" +
                "2 - Select \"Elden Ring FSR3\" in Select Mod and install it.\r\n" +
                "3 - Inside the game, press the \"Home\" key to open the mod menu. In \"Upscale Type,\" select the Upscaler according to your GPU (DLSS RTX or FSR3 non-RTX), then check the box \"Enable Frame Generation\" below.\r\n" +
                "• To remove Full Screen borders, select \"Full Screen\" in the game before installing the mod. If there is screen overflow after mod installation, select full screen -> window -> full screen.\r\n" +
                "• Enable AntiAliasing and Motion Blur; this mod will skip the actual rendering of motion blur, so don't worry if you don't like motion blur. The game only needs it to render motion vectors."
            },
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
                {"Optiscaler FSR 3.1/DLSS","Opti.png"},
                {"Add-on Mods","Addon.png" },
                {"Alone in the Dark","Alone.png"},
                {"A Plague Tale Requiem","Requiem.png"},
                {"Assassin's Creed Valhalla","AcVal.png"},
                {"Atomic Heart","Atomic.png"},
                {"Baldur's Gate 3","Baldurs.png"},
                {"Blacktail","Black.png"},
                {"Banishers Ghost of New Eden","Banishers.png"},
                {"Bright Memory: Infinite","Bmi.png"},
                {"Brothers a Tale of Two Sons","Brothers.png"},
                {"Chernobylite","Cherno.png"},
                {"Cod Black Ops Cold War","Cod.png"},
                {"Cod MW3","mw3.png"},
                {"Control","Control.png"},
                {"Crime Boss Rockay City","Rockay.png"},
                {"Cyberpunk 2077","Cyber.png"},
                {"Dakar Desert Rally","Dakar.png"},
                {"Dead Space Remake","DeadSpace.png"},
                {"Dead Island 2","Dead2.png"},
                {"Death Stranding Director's Cut","Ds.png"},
                {"Deathloop","Df.png"},
                {"Dragons Dogma 2","Dg2.png"},
                {"Dying Light 2","Dl2.png"},
                {"Elden Ring","Elden.png"}
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