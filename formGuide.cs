using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
                "Initial Information",
                "1 - When selecting the game folder, look for the game's .exe file. Some games have the full name .exe or abbreviated, while others have the .exe file in the game folder but within subfolders with the ending Binaries\\Win64, and the .exe usually ends with Win64-Shipping, for example: TheCallistoProtocol-Win64-Shipping.\r\n" +
                "2 - It is recommended to read the guide before installing the mod. Some games do not have a guide because you only need to install the mod, while others, like Fallout 4 for example, have extra steps for installation.\r\n" +
                "If something is done incorrectly, the mod will not work.\r\n" +
                "3 - Some games may not work for certain users after installing the mod. It is recommended to check the Enable Signature Override if the mod does not work with the default files.\r\n"
            },
            {
                "Add-on Mods","OptiScaler\r\nIs drop-in DLSS2 to XeSS/FSR2 replacement for games. OptiScaler implements all necessary API methods of DLSS2 & NVAPI to act as a man in the middle. So from games perspective it’s using DLSS2 but actually using OptiScaler and calls are interpreted/redirected to XeSS & FSR2.\r\n\r\n"+
                "Tweak\r\nHelps 'improve' aliasing caused by FSR 3 mod, may also slightly reduce ghosting, doesn't work in all games.\r\n"
            },
            {
                "Optiscaler Method","Installation Method for Optiscaler\r\n" +
                "Method Default: Default installation method.\r\n"+
                "Method 1 (RTX/RX 6000-7000): Installation method for RTX and RX 6xxx/7xxx series GPUs.\r\n"+
                "Method 2 (GTX/Old AMD): Installation method for older GPUs such as GTX and RX 5000 and below.\r\n"+
                "Method 3 (If none of the others work): Modified installation method if none of the other options work.\r\n" +
                "Follow the guide only if you are installing the standard Optiscaler; mods that already include Optiscaler (such as FSR 3.1.2/DLSS) do not require it."
            },
            {
                "Optiscaler FSR 3.1.1/DLSS",
                "This mod may not work with all games, so it is recommended to perform tests.\r\n" +
                "Select Optiscaler FSR 3.1.1/DLSS and install it.\r\n" +
                "In the game, press the Insert key to open the menu. In the menu, select your preferred upscaler. You can also increase image sharpness by enabling the Sharpness option in the menu.\r\n" +
                "If the menu does not appear, during the mod installation, check the Optiscaler box and select the desired upscaler under Add-on Upscaler, then install."
            },
            {
                "FSR 3.1.2/DLSS FG (Only Optiscaler)",
                "1. Run the game in DX12 or the mod will not work.\n" +
                "2. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "3. Check the Enable Signature Over box.\n" +
                "4. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, check the Frame Gen and Hud Fix boxes. If necessary, check the \"Extended FG\" box.\n" +
                "6. Some games have additional steps; it is recommended to check the guide for the game you are installing the mod for."
            },
            {
                "Achilles Legends Untold",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n"+
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX only).\r\n"+
                "3 - If the mod doesn't work, check the Nvapi Results box and select Default in NVNGX.dll.\r\n"+
                "4 - In-game, select DLSS."
            },
            {
                "Alan Wake Remastered",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\r\n" +
                "1 - Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\r\n" +
                "2 - Check the Enable Signature Over box.\r\n" +
                "3 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, select FSR 3x and FSR 3.1.2 right below; if you have an Nvidia GPU, select DLSS.\r\n" +
                "5 - Check the Frame Gen and Hud Fix boxes; in Hud Fix, select 2.\r\n\r\n"
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

                "Optiscaler FSR 3.1.1/DLSS\r\n" +
                "1 - Select Optiscaler FSR 3.1.1/DLSS.\r\n"+
                "2 - In-game, press the Insert key to open the menu and select your preferred upscaler.\r\n"+
                "3 - If the menu does not appear, you can select the upscaler during installation. Choose Optiscaler and under Addon Upscaler, select your preferred upscaler.\r\n\r\n" +

                "Preset\r\n" +
                "1 - Install ReShade.\r\n" +
                "2 - Inside ReShade, select the game's .exe and click next.\r\n" +
                "3 - Select DX 10/11/12 and click next.\r\n" +
                "4 - Click 'Browse' and locate the file 'Realistic Reshade.ini' that was installed in the folder selected in the Utility and click next.\r\n" +
                "5 - In the game, press the 'Home' key to open the menu and select the options you prefer.\r\n" +
                "6 - Install the Preset first and then the FSR3 mod if you plan to use it.\r\n\r\n"
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
                "Default Mods\r\n"+
                "1 - Select a mod of your choice (0.10.3 is recommended).\r\n"+
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX) and Nvapi Results (GTX). (If the mod doesn't work for AMD, also check Nvapi Results).\r\n"+
                "3 - To fix hub flickering, enable DLSS and Frame Generation and play for a few seconds, then disable DLSS and leave only Frame Generation enabled.\r\n\r\n"+

                "FSR 3.1.2/DLSS FG (Only Optiscaler)\r\n"+
                "1 - Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\r\n" +
                "2 - In the game, press the 'Insert' key to open the menu.\r\n" +
                "3 - In the menu, select an Upscaler, check 'Frame Gen' and 'Hud Fix' boxes.\r\n\r\n"
            },
            {
                "A Quiet Place: The Road Ahead",
                "1 - Select an FSR 3.1.1/DLSS mod and install it.\r\n" +
                "2 - In the game, select DLSS.\r\n" +
                "3 - Press the 'Insert' key to open the menu.\r\n" +
                "4 - Select an upscaler of your choice.\r\n" +
                "5 - Check the Frame Gen and Hud Fix boxes.\r\n" +
                "6 - If you can't see DLSS, enable Hardware Acceleration in Graphics Settings on Windows.\r\n\r\n"
            },
            {
                "Assassin\'s Creed Mirage",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\r\n"+
                "1 - Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\r\n"+
                "2 - Check the Enable Signature Over box.\r\n" +
                "3 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, select FSR 3x and FSR 3.1.2 right below; if you have an Nvidia GPU, select DLSS to use DLSS 3.8.10.\r\n" +
                "5 - Check the Frame Gen and Hud Fix boxes; in Hud Fix, select 2.\r\n" +
                "6 - If you are using ReShade, select FSR 3.1.2/DLSS FG Custom and follow the steps above.\r\n\r\n" +

                "Preset\r\n" +
                "1. Install ReShade.\r\n" +
                "2. In ReShade, select the game’s .exe and click next.\r\n" +
                "3. Select DX 10/11/12 and click next.\r\n" +
                "4. Click \"Browse\" and locate the file ACMirage lighting and package.ini that was installed in the folder selected in the Utility and click Next.\r\n" +
                "5. In the game, press the Home key to open the menu and select the options you prefer.\r\n" +
                "6. Install the Preset first and then the FSR3 mod if you plan to use it."
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
                "Black Myth: Wukong",
                "RTX:\n" +
                "1. Select \"DLSS FG (ALL GPUs) Wukong\" and install.\n" +
                "2. In the game, select DLSS and Frame Generation.\n" +
                "3. This mod fixes issues related to using the Somersault Cloud.\n\n" +

                "AMD/GTX DLSS FG:\n" +
                "1. Select Optiscaler FSR 3.1.1/DLSS and install it.\n" +
                "2. In the game, press the \"Insert\" key to open the menu, and in the menu, select the upscaler you want to use.\n" +
                "3. If an error occurs with the HUD, set the game to \"Windowed Mode\", then after a few seconds switch back to \"Borderless Windowed\".\n\n" +

                "Graphic Preset:\n" +
                "1. Install the mod and the ReShade application.\n" +
                "2. In ReShade, select b1.exe, DirectX 10/11/12, click on \"Browser\", and find the file Black Myth Wukong.ini (the path should look something like BlackMythWukong\\Black Myth Wukong.ini) and select it, then click on \"Uncheck All\" and \"Next\".\n" +
                "3. In the game, press the \"Insert\" key to open the menu and check the options you want.\n\n" +

                "Optimized Wukong:\n" +
                "Faster Loading Times - By tweaking async-related settings: the mod allows assets to load in the background, reducing loading times and potentially eliminating loading pauses during gameplay.\n\n" +

                "Optimized CPU and GPU Utilization - By tweaking multi-core rendering: allows the game to utilize the full potential of modern CPUs and GPUs. This can result in improved performance, higher frame rates, and more stable gameplay.\n\n" +

                "Enhanced Streaming and Level Loading - By tweaking various streaming variables: the mod improves the efficiency of streaming assets and level loading. This can lead to faster streaming and reduced stuttering when moving through different areas of the game world.\n\n" +

                "Optimized Memory Management - By adjusting memory-related settings: the mod optimizes memory allocation and garbage collection. This can lead to more efficient memory usage, reduced memory-related stutters, and improved overall performance."
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
                "Crysis 3 Remastered",
                "1 - Select an FSR 3.1.1/DLSS mod.\r\n" +
                "2 - Check the 'GPU' box that will appear during installation (Only FSR 3.1.1/DLSS Custom).\r\n" +
                "3 - In-game, press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, select the upscaler you prefer.\r\n\r\n"
            },
            {
                "Cyberpunk 2077",
                 "RTX DLSS FG\n" +
                "1. Select RTX DLSS FG and install.\n" +
                "2. It is recommended to install the \"FG Ghost Fix\" along with the mod, select \"Others Mods 2077\" to install.\n" +
                "3. In the game, select DLSS and DLSS Frame Gen.\n\n" +

                "FSR 3.1.2/XESS FG\n" +
                "1. Select FSR 3.1.2/XESS FG and install.\n" +
                "2. In the game, select DLSS and DLSS Frame Gen.\n" +
                "3. It is recommended to install the \"FG Ghost Fix\" along with the mod. Select \"Others Mods 2077\" to install.\n\n" +

                "Uniscaler FSR 3.1:\n" +
                "1. Select Uniscaler FSR 3.1.\n" +
                "2. If you have an RTX GPU and want to use the real DLSS, select DLSS under \"Mod Operates\". If you don't have an RTX GPU and can't see DLSS in the game, Click on Nvngx.dll and select \"Default\". You can also use XESS instead of FSR 3.1 by selecting XESS under \"Mod Operates\".\n" +
                "3. Check the box \"Enable Signature Over\".\n" +
                "4. In the game, choose an upscaler and frame generation option.\n\n" +

                "All Uniscalers:\n"+
                "1. Select a mod of your choice (Uniscaler is recommended).\n" +
                "2. Select Default in Nvngx.dll.\n" +
                "3. Check the box Enable Signature Override.\n" +
                "4. In-game, turn off Vsync, select DLSS (do not select auto as the game will crash), and turn on Frame Generation.\n\n" +

                "ReShade:\n" +
                "1. Download and install ReShade.\n" +
                "2. Select Cyberpunk2077.exe, DirectX 10/11/12, Update ReShade and Effects, and choose the V2.0 Real Life Reshade.ini.\n" +
                "3. Select check all effects (you can also use \"Uncheck all\" and \"Check all\" to select everything at once).\n" +
                "4. Install the mod using the Utility.\n\n" +

                "1. After completing the steps above, open the game for the first time. If a \"Menu\" (Ultra+) appears, select a key to open this \"Menu.\"\n" +
                "2. Select DLSS, Frame Gen, and restart the game.\n" +
                "3. After reopening the game, press the \"Insert\" key to open the FSR 3.1 mod menu, \"Home\" to open the ReShade menu (select the options you prefer), and the key you selected to open the \"Menu\" (Ultra+)."
            },

            {
                "Dakar Desert Rally",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box Fake Nvidia GPU and Nvapi Results (AMD/GTX).\r\n" +
                "In-game, select DLSS and Frame Generation."
            },
            {
                "Dead Island 2",
                "Default Mod\n"+
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - If it doesn't work with the default files, enable Enable Signature Override. If it still doesn't work, check the box lfz.sl.dlss.\r\n" +
                "3 - It's not necessary to activate an upscaler for this game for the mod to work, so enable it if you want.\n\n" +

                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. In the game, select FSR 2 and press the \"Insert\" key to open the menu.\n" +
                "3. In the menu, select FSR 3x, check the Frame Gen and Hud fix boxes."
            },
            {
                "Dead Rising Remaster",
                "1 - Select Dinput8 DRR and install.\r\n" +
                "2 - Open the game until the 'Re Framework' menu appears, once it appears, close the game and return to the Utility.\r\n" +
                "3 - Select FSR 3.1 FG DRR and install.\r\n" +
                "4 - In the game, select DLSS and Frame Gen.\r\n" +
                "5 - The mod already includes DLSS 3.7.20.\r\n\r\n"
            },

            {
                "Death Stranding Director's Cut",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n"+
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, disable Depth of Field, Motion Blur, and TAA.\n" +
                "4. Select DLSS and press the Insert key to open the menu.\n" +
                "5. In the menu, enable Frame Gen, HUD Fix, and FG Extended.\n\n"+

                "Uniscaler V4\n" +
                "Before installing the mod, open the game and disable FidelityFx Cas.\n" +
                "1. Select Unicaler V4.\n" +
                "2. Check the box for \"Enable Signature Over\", check the \"Nvngx.dll\" box, and select \"Default\".\n" +
                "3. In the game, enable FidelityFx Cas if you want more FPS (the mod is activated automatically when installed, but FidelityFx Cas provides a slight FPS boost).\n" +
                "4. If you want even more FPS, check the \"Fake Nvidia GPU\" box and reinstall the mod (this option may not work for some GPUs, so test it).\n" +
                "This game does not support ReShade and the mod together, so you will need to uninstall ReShade if you use it for the mod to work."
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
                "Dragon Age: Veilguard",
                "1 - Select FSR 3.1.1/DLSS FG DG Veil and install it.\r\n" +
                "2 - In the game, press the 'Insert' key to open the menu.\r\n" +
                "3 - In the menu, select 'FSR 3x' under 'Upscalers,' then select 'FSR 3.1.2' right below.\r\n\r\n"
            },
            {
                "Dragons Dogma 2",
                "Dinput8 DD2 and FSR 3.1.2/DLSS FG Custom\r\n" +
                "1 - Select Dinput8 DD2 in Mod Select and install.\r\n" +
                "2 - Open the game after Dinput8 is installed. A 'REFramework' menu will appear. Click on it, go to Settings and Menu Key, click on Menu Key, and select the preferred key (the key is used to open and close the menu).\r\n" +
                "3 - Close the game, in Utility select FSR 3.1.2/DLSS FG Custom and install (it is recommended to select 'Yes' when the message to delete the shader file appears).\r\n" +
                "4 - In the game, select DLSS and Frame Gen.\r\n" +
                "5 - It is possible that the game will crash the first 2 or 3 times after the mod is installed, so just close the game and open it again."
            },
            {
                "Dying Light 2",
                "Default Mods\n"+
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Enable Fake Nvidia GPU (only for AMD and GTX).\r\n" +
                "3 - In the game, select any upscaler and activate Frame Generation.\r\n" +
                "4 - If you experience any flickering or ghosting, go to Video > Advanced Settings and decrease the Lod Range Multiplier.\r\n\r\n"+

                "FSR 3.1.2/DLSS FG Custom\r\n" +
                "1 - Select FSR 3.1.2/DLSS FG Custom and install it.\r\n" +
                "2 - Select Frame Generation within the game.\r\n" +
                "3 - Press the \"Insert\" key to open the menu.\r\n" +
                "4 - In the menu, select the upscaler you prefer.\r\n" +
                "5 - This mod cannot fix the HUD error, but you can reduce it slightly by decreasing the HUD size and enabling motion blur.\r\n\r\n"+

                "FSR 3.1.2\r\n"+
                "1 - Select the FSR 3.1.2 DL2 Custom and install.\r\n" +
                "2 - In the game, select FSR 2 and press the 'Insert' key to open the menu.\r\n" +
                "3 - In the menu, select FSR 3.x and select FSR 2.3.2 right below.\r\n" +
                "4 - Check the 'Hud Fix' box to remove glitches.\r\n\r\n"
            },
            {
                "Elden Ring",
                "Default Mod\r\n"+
                "1 - Select \"Disable AntiCheat\" in the Select Mod and choose \"Yes\" in the anticheat deactivation confirmation window. Select the folder where the game exe is located, otherwise, it will not be possible to deactivate the anticheat. (Steam Only)\r\n" +
                "2 - Select \"Elden Ring FSR3\" in Select Mod and install it.\r\n" +
                "3 - Inside the game, press the \"Home\" key to open the mod menu. In \"Upscale Type,\" select the Upscaler according to your GPU (DLSS RTX or FSR3 non-RTX), then check the box \"Enable Frame Generation\" below.\r\n" +
                "• To remove Full Screen borders, select \"Full Screen\" in the game before installing the mod. If there is screen overflow after mod installation, select full screen -> window -> full screen.\r\n" +
                "• Enable AntiAliasing and Motion Blur; this mod will skip the actual rendering of motion blur, so don't worry if you don't like motion blur. The game only needs it to render motion vectors.\r\n\r\n"+

                "FSR 3.1.2/DLSS FG Custom Elden\r\n"+
                "1 - Select FSR 3.1.2/DLSS FG Custom Elden and install.\r\n" +
                "2 - For Steam users, add the following command in 'Launch Options' before starting the game: cmd /c start eldenring.exe & rem %command%\r\n" +
                "3 - In the game, press the 'Home' key to open the menu.\r\n" +
                "4 - In the menu, select 'Frame Gen' and an 'Upscaler'."
            },
            {
                "Everspace 2",
                "1 - Select a mod of your preference (0.10.3 is recommended)\r\n" +
                "2 - Check Fake Nvidia Gpu and Nvapi Results.\r\n" +
                "3 - Inside the game, select FSR or DLSS"
            },
            {
                "Evil West",
                "1 - Select a mod of your preference. (recommended 0.10.3)\r\n" +
                "2 - Install, within the game, set post-processing to low and\r\n" +
                "activate FSR."
            },
            {
                "Fallout 4",
                "Usage of the Sym Link:\r\n" +
                "1 - In SymLink, click on add file and navigate to the root folder of the game. In the root folder, look\r\n" +
                "for Data\\F4SE\\Plugins, within this folder select Fallout4Upscaler.dll.\r\n" +
                "2 - In 'Destination Path' in the SymLink, paste the path of the 'mods' folder. Simply navigate to\r\n" +
                "the mods folder and copy the path from the address bar of the file explorer, or you can navigate to\r\n" +
                "the folder through the SymLink itself.\r\n" +
                "3 - Click on Create symlinks.\r\n" +
                "4 - Go back to the mods folder, go to View (w10) or Options (w11), and uncheck the box 'File\r\n" +
                "name extensions.\r\n" +
                "5 - Rename the file Fallout4Upscaler.dll in the mods folder to RDR2Upscaler.org.\r\n" +
                "6 - Run the game launcher located in the root folder of the game, in the launcher set 'depth of\r\n" +
                "field' to Low.\r\n" +
                "7 - Run the game using the file f4se_loader.exe, also located in the root folder of the game.\r\n" +
                "8 - In the game, press the 'END' key to open the mod menu, select DLSS for RTX and FSR3 for\r\n" +
                "non-RTX."
            },
            {
                "Fist Forged in Shadow Torch",
                "1 - Select a mod of your choice. (0.10.3 is recommended)\r\n" +
                "2 - Check the Fake Nvidia GPU box (AMD/GTX)"
            },
            {
                "Flintlock: The Siege of Dawn",
                "1 - Select the FSR 3.1/DLSS Optiscaler mod and install it.\r\n" +
                "2 - In the game, select DLSS, press the Insert key to open\r\n" +
                "the Optiscaler menu, in Upscalers select an upscaler of your\r\n" +
                "preference. If you cannot see the menu, after installing the\r\n" +
                "mod, select Optiscaler in the Utility and choose an upscaler\r\n" +
                "in Upscaler Optiscaler and install."
            },
            {
                "Fort Solis",
                "1 - Select a mod of your preference. (0.10.3 is recommended)\r\n" +
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX) and the\r\n" +
                "box for Nvapi Results (GTX). If DLSS is not available for AMD,\r\n" +
                "check the Nvapi Results box.\r\n" +
                "3 - In the game, select DLSS and Frame Generation."
            },
            {
                "Forza Horizon 5",
                "1 - Choose Horizon Forza 5 FSR3 and install it. In the\r\n" +
                "confirmation window, select 'Yes' for RTX or 'No' for non-RTX.\r\n" +
                "2 - For RTX, in-game, select DLSS and enable Frame\r\n" +
                "Generation.\r\n" +
                "3 - For other GPUs, select FSR and activate Frame\r\n" +
                "Generation. You can use DLSS, but you will experience\r\n" +
                "ghosting."
            },
            {
                "Final Fantasy XVI",
                "FSR 3.1.2/DLSS FG Custom:\n" +
                "1. Select FSR 3.1.2/DLSS FG Custom.\n" +
                "2. Check the \"GPU\" box that will appear during installation.\n" +
                "3. In-game, press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, select the upscaler you prefer.\n\n" +

                "Anti Stutter:\n" +
                "Prevents possible crashes during the game and optimizes CPU/GPU usage.\n\n" +

                "FFXVI FIX:\n" +
                "General:\n" +
                "Adjust gameplay FOV, camera distance, and camera horizontal position.\n" +
                "JXL screenshot quality option and fixes hitching while taking screenshots.\n" +
                "Allow the use of motion blur + frame generation.\n" +
                "Disable depth of field.\n" +
                "Enable background audio.\n" +
                "Lock cursor to game window.\n\n" +

                "Performance:\n" +
                "Disable 30FPS cap in cutscenes/photo mode or set your own framerate limit.\n" +
                "Allow frame generation in cutscenes.\n" +
                "Disable graphics debugger checks.\n\n" +

                "Ultrawide/narrower:\n" +
                "Remove pillarboxing/letterboxing.\n" +
                "Fixed HUD scaling with configurable HUD size.\n" +
                "Fixed FOV scaling at <16:9.\n\n" +

                "ReShade:\n" +
                "1. Download and install ReShade.\n" +
                "2. Select ffxvi.exe, DirectX 10/11/12, Update ReShade and Effects, and choose the FINAL FANTASY XVI.ini. (The .ini is in the selected folder in the Utility).\n" +
                "3. Select check all effects (you can also use \"Uncheck all\" and \"Check"
            },
            {
                "F1 2022",
                "1 - Choose a version of the mod you prefer (version 0.10.3 is\r\n" +
                "recommended).\r\n" +
                "2 - Select 'Default' in Nvngx and check the box 'Enable\r\n" +
                "Signature Override.\r\n" +
                "3 - Check the box 'Fake Nvidia GPU' (AMD Only).\r\n" +
                "4 - Within the game, under AntiAliasing, select DLSS or FSR.\r\n" +
                "• To fix the HUD flickering, select DLSS in AntiAliasing before\r\n" +
                "starting the game. While playing, switch to TAA+FSR or TAA\r\n" +
                "only."
            },
            {
                "F1 2023",
                "1 - Choose a version of the mod you prefer (version 0.10.3 is\r\n" +
                "recommended).\r\n" +
                "2 - Select 'Default' in Nvngx and check the box 'Enable\r\n" +
                "Signature Override.\r\n" +
                "3 - Check the box 'Fake Nvidia GPU' (AMD Only).\r\n" +
                "4 - Inside the game, under AntiAliasing, select DLSS or FSR."
            },
            {
                "GTA Trilogy",
                "FSR 3.1.2/DLSS Custom GTA\r\n" +
                "1 - Select FSR 3.1.2/DLSS Custom GTA and install.\r\n" +
                "2 - Check the 'Enable Signature Over' box.\r\n" +
                "3 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, select an upscaler, check 'Frame Gen' and 'Fix Hud', then select 2 in 'Fix Hud'.\r\n" +
                "5 - The installation method is the same for the three GTA versions.\r\n\r\n"
            },
            {
                "GTA V",
                "Single Player and Multiplayer\r\n" +
                "1 - Select Dinput 8 and install. (only single player)\r\n" +
                "2 - Open the game and disable MSAA and TXAA and select\r\n" +
                "borderless window. If the mod doesn't work, disable FXAA.\r\n" +
                "3 - Close the game and select GTA V FSR3 and install\r\n" +
                "4 - Turn on Vsync, Nvidia (Vertical Sync), or AMD Adrenalin\r\n" +
                "(Wait for Vertical Sync Update)\r\n" +
                "5 - Press 'Home' to open the menu. If the mod is disabled,\r\n" +
                "check 'Enable Frame Generation'."
            },
            {
                "Ghost of Tsushima",
                "1 - Select Ghost of Tsushima FG DLSS and install\r\n" +
                "2 - In the game, select DLSS Frame Generation\r\n" +
                "3 - If you encounter any issues related to DX12, select 'YES'\r\n" +
                "in the 'DX12' window that will appear during the installation.\r\n" +
                "First, test the mod without confirming this window.\r\n" +
                "4 - If you are experiencing any issues with crashes, select\r\n" +
                "'Yes' in the 'Crash Issues' window that will appear during\r\n" +
                "the mod installation.\r\n" +

                "FSR 3.1\r\n" +
                "1 - Select Uniscaler FSR 3.1\r\n" +
                "2 - For AMD/GTX users: Check the boxes: Fake Nvidia GPU, Nvapi\r\n" +
                "Results, and Disable Signature Over.\r\n" +
                "3 - Check the Nvngx box and select Default.\r\n" +
                "4 - In the game, select DLSS; do not change to FSR as the\r\n" +
                "game will crash."
            },
            {
                "Ghostrunner 2",
                "1 - Select a version of the mod of your choice (version 0.10.3\r\n" +
                "is recommended)\r\n" +
                "2 - To make the mod work, run it in DX12. To run it in DX12, right-click\r\n" +
                "the game exe and create a shortcut, then right-click the shortcut\r\n" +
                "again, go to 'Properties,' and at the end of 'Target' (outside the\r\n" +
                "quotes), add -dx12 or go to your Steam library, select the game, go to\r\n" +
                "Settings > Properties > Startup options, and enter -dx12.\r\n" +
                "3 - Activate Fake Nvidia Gpu (AMD only)\r\n" +
                "4 - Inside the game, set the frame limit to unlimited, activate DLSS first\r\n" +
                "(disable other upscalers before) and then activate frame generation\r\n" +
                "• To fix the flickering of the HUD, activate and deactivate frame\r\n" +
                "generation again (no need to apply settings)."
            },
            {
                "Ghostwire: Tokyo",
                "Default Mod\r\n"+
                "1- Select Uniscaler V3\r\n" +
                "2 - Check the Fake Nvidia GPU box (AMD/GTX). If you can't see DLSS in the game, also check the Nvapi Results box.\r\n" +
                "3 - Check the Nvngx.dll box and select Default, then check the Enable Signature Override box.\r\n" +
                "4 - In the game, select DLSS to enable Frame Generation.\r\n" +
                "5 - To fix the HUD glitch, switch between the upscalers (FSR, DLSS, etc.) until the HUD stops flickering.\r\n\r\n"+

                "FSR 3.1.2/DLSS FG Custom\r\n"+
                "1 - Select FSR 3.1.2/DLSS FG Custom and install.\r\n" +
                "2 - Check the 'Enable Signature' box.\r\n" +
                "3 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, select 'FSR 3x' and right below select 'FSR 3.1.1', check 'Frame Gen' and 'Hud Fix'.\r\n" +
                "5 - Disable all FPS Overlay. If your game is from Epic Games, select 'Auto Search' in 'Epic Games Overlay' and click 'Disable'.\r\n\r\n"
            },
            {
                "God Of War 4",
                "1 - Select \"Gow 4 FSR 3.1\".\n" +
                "2 - Click on Add-on, then Optiscaler, and select 'Method Default.'\n" +
                "3 - In 'Add-on Upscaler,' choose 'fsr3.1 DX11' and install.\n" +
                "4 - In the game, select DLSS (do not choose ultra quality, as it will not work).\n" +
                "5 - Press the 'Insert' key to open the menu and select the desired upscaler (XESS is recommended).\n" +
                "6 - If the menu does not appear, set the preferred upscaler in 'Add-on Upscaler' in the Utility and install again. (Select only the DX11 upscalers, such as fsr3.1 DX11, xess DX11, etc.)\n" +
                "7 - If you don't see DLSS in the game, check 'Enable Signature Override' under 'Additional Settings.'"
            },
            {
                "God of War Ragnarök",
                "DLSS FG ALL GPU:\n" +
                "1. Select Uniscaler FSR 3.1.\n" +
                "2. Select FSR3 in \"Mod Operates\" (if you can\'t see DLSS in the game, select FSR3 in \"Frame Gen Method\" as well).\n" +
                "3. Check the \"Enable Signature Over\" box.\n" +
                "4. If you still can\'t see DLSS in the game, check the Nvngx.dll box, select \"Default,\" and reinstall the mod.\n" +
                "5. The game may freeze for a few seconds when selecting DLSS FG.\n\n" +

                "Unlock VRAM:\n" +
                "Removes the error for GPUs with less than 6GB of VRAM.\n\n" +

                "Anti Stutter:\n" +
                "Prevents possible game stuttering and optimizes CPU/GPU usage.\n\n" +

                "ReShade:\n" +
                "1. Download and install ReShade.\n" +
                "2. Select GoWR.exe, DirectX 10/11/12, Update ReShade and Effects and choose the God of War Ragnarök.ini. (The .ini is in the selected folder in the Utility).\n" +
                "3. Select check all effects (you can also use \"Uncheck all\" and \"Check all\" to select everything at once)."
            },
            {
                "Gotham Knights",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the \"Enable Signature Over\" box.\n" +
                "3. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, check the \"Frame Gen\" and \"Hud Fix\" boxes.\n" +
                "5. If you want to update DLSS, select \"Others Mods GK\" and install it. Select the .exe folder (Mercury\\Binaries\\Win64); otherwise, it will not be possible to update DLSS."
            },
            {
                "Hellblade: Senua's Sacrifice",
                "1 - Select a version of the mod of your choice (version 0.10.3\r\n" +
                "is recommended).\r\n" +
                "2 - Select Fake Nvidia Gpu and UE Compatibility (AMD only),\r\n" +
                "select Fake Nvidia Gpu and Nvapi Results (GTX only)."
            },
            {
                "Hellblade 2",
                "Only RTX\r\n" +
                "1 - Select Hellblade 2 FSR3 and install it.\r\n" +
                "2 - In the game, select Frame Generation.\r\n" +
                "3 - This mod only works for RTX.\r\n\n" +
                "All GPUs\r\n" +
                "1 - Select Uniscaler V2 (you can also test with the other mods)\r\n" +
                "2 - Check the box for Fake Nvidia GPU (AMD) and check the\r\nbox for UE compatibility mode (AMD and Nvidia)\r\n" +
                "3 - In-game, select Frame Generation\r\n" +
                "• If you can’t see the DLSS option in the game, select\r\n'YES' in the 'DLSS Fix' window during installation.\r\n\n" +
                "• To remove the black bars, select the Engine.ini file folder in\r\n'Select Folder' (if the file is not found automatically), select\r\n'Remove Black Bars' in mod version, and install. (The path to\r\nthe engine.ini file is something like: C:\\Users\\YourName\\\r\nAppData\\Local\\Hellblade2\\Saved\\Config\\Windows or\r\nWinGDK)\r\n\n" +
                "• If the bars are not removed, select 'Remove Black Bars Alt',\r\nthe removal of the black bars will be automatically performed if\r\nthe Engine.ini file is found. If it is not found, you need to select\r\nthe path in 'Select Folder' and press 'Install'.\r\n\n" +
                "• To remove only the main effects, such as Lens Distortion,\r\nBlack Bars, and Chromatic Aberration, select Remove Post\r\nProcessing.\r\n\n" +
                "• To remove all effects, select Remove All Post Processing\r\n(includes film grain).\r\n\n" +
                "• To restore the Post Processing effects, simply select\r\n'Restore Post Processing', and the Engine.ini file will be replaced\r\nwith the default file.\r\n\n" +
                "• If the Frame Generation is not visible, remove the black bars."
            },
            {
                "High On Life",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Enable Fake Nvidia Gpu. (only AMD and GTX)\r\n"
            },
            {
                "Hogwarts Legacy",
                "1 - Select a version of the mod of your choice (versions from 0.9.0\r\n" +
                "onwards are recommended to fix UI flickering).\r\n" +
                "2 - Enable the 'Enable Signature Override' checkbox if the mod\r\ndoesn't work.\r\n" +
                "3 - Enable Fake Nvidia GPU (Only for AMD GPUs).\r\n" +
                "4 - Select 'Default' in Nvngx.dll.\r\n\n"+

                "FSR 3.1.2/DLSS FG Custom / FSR 3.1.1/DLSS Optiscaler" +
                "1 - Select the FSR 3.1/DLSS mod.\r\n" +
                "2 - Check the 'GPU' window that will appear.\r\n" +
                "3 - In-game, select DLSS before starting the campaign.\r\n" +
                "4 - During the campaign, press the 'Insert' key to open the menu.\r\n" +
                "5 - In the menu, select the desired upscaler, check the Frame Gen and Hud Fix boxes.\r\n\r\n"+

                "ReShade Installation"+
                "1 - Install ReShade.\r\n" +
                "2 - Inside ReShade, select the game's .exe and click next.\r\n" +
                "3 - Select DX 10/11/12 and click next.\r\n" +
                "4 - Click 'Browse' and locate the file 'Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt' that was installed in the folder selected in the Utility and click next.\r\n" +
                "5 - In the game, press the 'Home' key to open the menu and select the options you prefer.\r\n" +
                "6 - Install the Preset first and then the FSR3 mod if you plan to use it.\r\n\r\n"
            },
            {
                "Horizon Zero Dawn",
                "FSR 3.1.2/DLSS FG (Only Optiscaler), FSR 3.1/DLSS Optiscaler & FSR 3.1.2/DLSS FG Custom:\n" +
                "1. FSR 3.1.2/DLSS FG (Only Optiscaler), FSR 3.1/DLSS Optiscaler or FSR 3.1.2/DLSS FG Custom.\n" +
                "2. Check the \"Enable Signature Over\" box.\n" +
                "3. In the game, press the \"Insert\" key to open the mod menu.\n" +
                "4. In the menu, select the Upscaler of your choice.\n" +
                "5. (For FSR 3.1/DLSS Optiscaler), if you cannot open the menu, select the desired upscaler in \"Upscaler Optiscaler\" and reinstall the mod."
            },
            {
                "Horizon Zero Dawn Remastered",
                "FSR 3.1.2 HZD Rem\r\n" +
                "1 - Select FSR 3.1.2 HZD Rem and install.\r\n" +
                "2 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "3 - In the menu, select FSR 3.1.2.\r\n\r\n"
            },

            {
                "Hitman 3",
                "1 - Select a mod of your preference. (0.10.3 is recommended\r\n" +
                "but if it doesn’t work, try 0.10.2)\r\n" +
                "2 - Check the box for Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - In the game, select FSR and Frame Generation. If Frame\r\n" +
                "Generation is not available, you can check the Nvapi Results\r\n" +
                "box or download the file EnableDLSSFrameGenerationHitmanIII.reg\r\n" +
                "and run it. This will activate Frame Generation even if\r\n" +
                "it is not available.\r\n"
            },
            {
                "Horizon Forbidden West",
                "1 - Select Horizon Forbidden West FSR3 or Optiscaler FSR 3.1.1/DLSS and install\r\n" +
                "2 - Choose Xess or FSR on the initial setup screen, turn on Frame\r\n" +
                "Generation, and do not select DLSS, otherwise the game will crash\r\n" +
                "3 - In-game, select the Low quality preset, then adjust the settings as\r\n" +
                "desired, but do not modify options below Hair Quality\r\n" +
                "4 - Select Xess or FSR.\r\n"
            },
            {
                "Icarus",
                "1 - Select Icarus FSR3 in mod version.\r\n" +
                "2 - If the option selected is RTX, confirm the window that appears.\r\n" +
                "3 - In case you can't see Frame Generation in the game, select replace_dlss_fg in Mod Operates.\r\n" +
                "4 - Start the game in DX12, if the game exe is in the destination folder where the mod was\r\n" +
                "installed, a DX12 shortcut will be created on your Desktop. If the exe is not found, you\r\n" +
                "need to create a shortcut and in the properties, at the end of Target, add -dx12 outside the\r\n" +
                "quotes if there are any, don't forget to put a space between -dx12 and the path.\r\n" +
                "5 - Run the game through the executable.\r\n"
            },
            {
                "Indiana Jones and the Great Circle",
                "1. Install ReShade.\n" +
                "2. In the ReShade, select the game’s .exe and click next.\n" +
                "3. Select DX 10/11/12 and click next.\n" +
                "4. Click \"Browse\" and locate the file TheGreatCircle.ini that was installed in the folder selected in the Utility and click Next.\n" +
                "5. In the game, press the Home key to open the menu and select the options you prefer.\n"
            },
            {
                "Judgment\r\n",
                "1 - Select a mod of your preference. (0.10.3 is recommended)\r\n" +
                "2 - In the game, select FSR 2.1\r\n"
            },
            {
                "Jusant",
                "1 - Select a mod of your preference (0.10.3 is recommended)\r\n" +
                "2 - Check the box for Fake Nvidia GPU. If the mod doesn’t work, also check Nvapi Results (only for AMD and GTX) and select Default for Nvngx.dll\r\n" +
                "3 - In-game, select DLSS and check the Frame Generation box.\r\n"
            },
            {
                "Kena: Bridge of Spirits",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Open the game and select DX12 in the Settings.\n" +
                "2. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "3. Check the Enable Signature Over box.\n" +
                "4. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, check the Frame Gen and Hud Fix boxes.\n\n"+

                "Others Mods\r\n"+
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - Activate Fake Nvidia GPU and Nvapi Results (AMD only)."
            },
            {
                "Layers of Fear",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - If you don't notice Frame Generation, select Replace DLSS FG in 'Mod Operates'.\r\n" +
                "4 - In the game, select Frame Generation and DLSS or FSR.\r\n"
            },
            {
                "Lego Horizon Adventures",
                "1. Select the FSR 3.1.2/DLSS FG Custom and install.\r\n" +
                "2. In the game, select DLSS and Frame Gen.\r\n" +
                "3. If you want to use FSR 3.1.2, press the \"Insert\" key and select FSR 3x."
            },

            {
                "Lies of P",
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - Activate Fake Nvidia Gpu and UE Compatibility Mode (AMD only).\r\n" +
                "3 - To fix the flickering of the HUD, first select DLSS Quality, then select FSR Quality (without disabling DLSS), then select DLSS again.\r\n\r\n"+

                "FSR 3.1.2/DLSS FG (Only Optiscaler)\r\n"+
                "1 - Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install it.\r\n" +
                "2 - Check the 'Enable Signature Over' box.\r\n" +
                "3 - In the game, select DLSS.\r\n" +
                "4 - Press the 'Insert' key to open the menu.\r\n" +
                "5 - Select an Upscaler of your choice, check the 'Frame Gen' and 'Hud Fix' boxes.\r\n" +
                "6 - If you cannot see DLSS in the game, go back to the Utility and check the 'Disable Signature Over' box."
            },
            {
                "Loopmancer",
                "1 - Select a mod of your preference (0.10.3 is recommended)\r\n" +
                "2 - Check the box Fake Nvidia GPU. (AMD/GTX)\r\n" +
                "3 - In the game, select DLSS or FSR\r\n"
            },
            {
                "Lords of the Fallen",
                "1 - Run the game through the launch.bat file. During the mod\ninstallation, you will be asked if you want to create a shortcut\nfor the .bat file on the desktop. If you don't want to, run the\n.bat file directly from the folder where the mod was installed.\r\n" +
                "2 - In-game, enable Frame Generation and select FSR or DLSS.\r\n"
            },
            {
                "Manor Lords",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the Fake Nvidia GPU box. (Only for AMD/GTX).\r\n" +
                "3 - In-game, select DLSS.\r\n"
            },
            {
                "Martha Is Dead",
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - Select Default in Nvngx\r\n" +
                "3 - Execute Enable Signature Override\r\n" +
                "4 - In Mod Operates, select 'Replace DLSS FG'\r\n" +
                "5 - Activate Fake Nvidia Gpu (AMD only)\r\n" +
                "• To fix the flickering on the Hud, set the screen mode to fullscreen (windowed), select FSR 1.0, turn off motion blur and depth of field, check Motion Blur and Depth of Field again after saving the settings. If they don't turn off, turn them off again.\r\n"
            },
            {
                "Marvel's Avengers",
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and start the campaign.\n" +
                "4. Press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, check Frame gen and Hud Fix boxes."
            },
            {
                "Marvel's Guardians of the Galaxy",
                "1 - Select a version of the mod of your choice (it is recommended 0.10.3 onwards or Uniscaler)\r\n" +
                "2 - Select the folder where the game's exe is located (something like gotg.exe)\r\n" +
                "3 - Activate Fake Nvidia GPU (if you don't have Rtx 3xxx/4xxx series)\r\n" +
                "4 - Inside the game, select DLSS or FSR\r\n" +
                "• If you want to use Uniscaler with the DLSS upscaler, select DLSS in Mod Operates (the default option of Uniscaler uses the FSR upscaler)\r\n" +
                "• If the game is on Epic Games, it is necessary to disable the Overlay, simply go to 'Epic Games Overlay'.\r\n"
            },
            {
                "Metro Exodus Enhanced",
                "Uniscaler\r\n"+
                "1 - Select Uniscaler.\r\n" +
                "2 - Check the boxes for Fake Nvidia GPU (AMD/GTX) and Nvapi Results (GTX). If the DLSS option is not available for AMD GPU, check the Nvapi Results box.\r\n" +
                "3 - In Nvngx.dll, select Default and check the box Enable Signature Override.\r\n" +
                "4 - In the game, select DLSS.\r\n\r\n" +

                 "FSR 3.1.1/DLSS\r\n" +
                 "1 - Select an FSR 3.1.1/DLSS mod and install it.\r\n" +
                 "2 - Check the 'Enable Signature Over' box.\r\n" +
                 "3 - In the game, select DLSS.\r\n" +
                 "4 - Press the 'Insert' key to open the menu.\r\n" +
                 "5 - Select an Upscaler of your choice, check the 'Frame Gen' and 'Hud Fix' boxes.\r\n" +
                 "6 - If you cannot see DLSS in the game, go back to the Utility and check the 'Disable Signature Over' box.\r\n\r\n" +

                 "ReShade Installation\r\n" +
                 "1 - Install ReShade.\r\n" +
                 "2 - Inside ReShade, select the game's .exe and click next.\r\n" +
                 "3 - Select DX 10/11/12 and click next.\r\n" +
                 "4 - Click 'Browse' and locate the file 'DefinitiveEdition.ini' that was installed in the folder selected in the Utility and click next.\r\n" +
                 "5 - In the game, press the 'Home' key to open the menu and select the options you prefer.\r\n" +
                 "6 - Install the Preset first and then the FSR3 mod if you plan to use it.\r\n\r\n"
            },
            {
                "Microsoft Flight Simulator 24",
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, check Frame gen and Hud Fix boxes."
            },
            {
                "Monster Hunter Rise",
                "1 - Select a mod of your choice. (recommended 0.10.3)\r\n" +
                "2 - Check the box Fake Nvidia GPU.\r\n" +
                "3 - If you don't see any differences, check the box Nvapi Results.\r\n" +
                "4 - To fix flickering in the hud, activate DLSS and play for a few seconds, then return to the menu and deactivate DLSS.\r\n"
            },
            {
                "Nobody Wants To Die",
                "1 - Select Uniscaler FSR 3.1\r\n" +
                "2 - For AMD/GTX users: Check the boxes for Fake Nvidia GPU, Ue compatibility Mode, Nvapi Results and Disable Signature Over.\r\n"
            },
            {
                "Outpost Infinity Siege",
                "1 - Select a mod of your preference; (0.10.3 is recommended)\r\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - In the game, select DLSS and Frame Generation.\r\n" +
                "4 - If you have any issues, in Nvngx.dll, select Default.\r\n"
            },
            {
                "Pacific Drive",
                "1 - Select a mod of your preference, (0.10.3 is recommended)\r\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - If you have any issues, in Nvngx.dll, select Default.\r\n"
            },
            {
                "Palworld",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, check the Frame Gen, Hud Fix, and FG Extended boxes.\n" +
                "5. Not recommended to utilize the mod in online mode.\n\n" +

                "Others Mods\n" +
                "For standard mods (0.10+ and Uniscaler), simply enable the Fake Nvidia GPU (for AMD and GTX) and UE Compatibility mode (for AMD) and set the game to DX12. Throughout the guide, it will be explained how to do this.\r\n" +
                "1. Select Palworld Build03 and locate the game folder with the ending binaries/win64 and see if the executable with the ending Win64-shipping.exe is present.\r\n" +
                "2. Install, confirm the GPU selection window that will appear.\r\n" +
                "3. To run the game in DX12, simply confirm the window that appears after confirming the GPU selection. Make sure the mentioned exe is in the selected folder. Alternatively, you can ignore the window and do it manually, by creating a shortcut and adding '-dx12' after the quotes in the 'Target' field.\r\n" +
                "4. Run the game through the shortcut.\r\n" +
                "• Currently, the mod only works on Steam versions and alternative versions with Steam files.\r\n"
            },
            {
                "Ratchet and Clank",
                "1 - Select a mod of your preference (0.10.3 is recommended, but you can also try with Uniscaler).\r\n" +
                "2 - If you encounter any issues, select Replace DLSS FG in 'Mod Operates'.\r\n" +
                "3 - In the initial configuration screen, select Frame Generation and FSR.\r\n" +
                "4 - In the game, turn off Frame Generation and then turn it back on again.\r\n"
            },
            {
                "Ready or Not",
                "1 - Select a version of the mod of your choice (version 0.10.3 is recommended).\r\n" +
                "2 - Select the game folder that has the ending '\\ReadyOrNot\\Binaries\\Win64'.\r\n" +
                "3 - Enable Fake Nvidia GPU (Only for AMD GPUs).\r\n" +
                "4 - Set Anti-Aliasing to High or Epic + FSR2 Quality (DLSS won't work with UI flickering fix).\r\n" +
                "5 - UI flickering fix: Change Anti-Aliasing from Epic or High to Medium.\r\n" +
                "After launching the game again, you need to set Anti-Aliasing back to High or Epic to activate the mod before playing the character.\r\n"
            },
            {
                "Red Dead Redemption",
                "Frame Generation\r\n" +
                "1 - Select an FSR 3.1.1/DLSS mod (FSR 3.1.1/DLSS Custom is recommended) and install it.\r\n" +
                "2 - In the game, disable Vsync, triple buffering, and enable Nvidia Reflex and Frame Generation.\r\n" +
                "3 - During the installation, a window labeled 'Enable' will appear. Only check 'Yes' if the mod does not work.\r\n" +
                "4 - If you are using Frame Generation with ReShade, press the 'Home' key and uncheck the 'MXAO' and 'SSR' options. This will remove the game glitches.\r\n\r\n" +

                "Preset\r\n" +
                "1 - Install ReShade.\r\n" +
                "2 - In ReShade, select 'RDR.exe'.\r\n" +
                "3 - Select DirectX 10/11/12.\r\n" +
                "4 - Click 'Browse' and locate the file 'Red Dead Redemption.ini' that was installed in the destination folder selected in the Utility.\r\n" +
                "5 - Finish the installation and open the game.\r\n" +
                "6 - In the game, press the 'Home' key to open the menu and select the graphic options you prefer.\r\n" +
                "7 - It is recommended to use version 6.3.1 as the newer versions cause significant FPS loss.\r\n\r\n"
            },
            {
                "Red Dead Redemption 2",
                "FSR 3.1.2/DLSS FG Custom RDR2\r\n"+
                "1. Disable Vsync, Triple Buffering, select Borderless Windowed, and run in DirectX 12.\n" +
                "2. Select FSR 3.1.2/DLSS FG Custom RDR2 and install.\n" +
                "3. Check the Enable Signature Over box.\n" +
                "4. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, check the Frame Gen and HUD Fix boxes.\n" +
                "6. If you want to use FSR 3.1.2, select FSR 3x in \"Upscalers.\"\n" +
                "7. Close MSI Afterburner, or the game may crash.\n\n" +

                "Red Dead Redemption 2 MIX\r\n" +
                "1. Set the game to DX12 in advanced options.\n" +
                "2. Turn off Triple Buffering and Vsync, and set the game to Full Screen.\n" +
                "3. Select the RDR2 Mix mod and install it.\n" +
                "4. You don\'t need to use any upscaler, as Frame Generation is automatically activated.\n" +
                "However, if you want to use an upscaler, when installing, check the Addon Mods box, select Optiscaler, and below in DX11 select FSR 2.1 DX11, and in DX12 select FSR 2.1 DX12.\n\n" +

                "RDR2 FG Custom\n" +
                "1. Select an upscaler, disable Triple Buffering, Vsync, and set the game to DX12 before installing the mod.\n" +
                "2. Select RDR2 Custom FG and install.\n" +
                "3. In the game, press the \"END\" key to open the menu.\n" +
                "4. In the menu, select Frame Gen."
            },
            {
                "Resident Evil 4 Remake", 
                "1. Select the game root folder (RESIDENT EVIL 4 BIOHAZARD RE4).\n" +
                "2. Disable FSR in the game.\n" +
                "3. Select FSR 3.1.3/DLSS RE4 Remake and install.\n" +
                "4. In the game, close the \"ReFramework\" window and press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, select an upscaler. It is recommended to choose the upscaler before entering the campaign, as the cursor may get stuck, and you will need to navigate the menu options using the arrow keys.\n" +
                "6. Do not select the game's native FSR, or the game may crash.\n" +
                "7. The game HUD might experience slight flickering."
            },
            {
                "Returnal",
                "Deafault Mod\n" +
                "1 - Choose a version of the mod you prefer (version 0.10.3 is recommended).\r\n" +
                "2 - Enable the \"Enable Signature Override\" checkbox if the mod doesn't work.\r\n" +
                "3 - Select \"Default\" in Nvngx.dll.\n\n" +

                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and press \"Insert\" key to open the menu.\n" +
                "4. In the menu, check the Frame Gen and Hud Fix boxes."
            },
            {
                "Rise of The Tomb Raider",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler).\n" +
                "2. Check the \"Enable Signature Over\" box.\n" +
                "3. To update DLSS in the game, check the \"Nvngx.dll\" box and select DLSS 3.8.10.\n" +
                "4. Open the game, in the configuration menu that appears when the game starts, select DX12 and DLSS if available (if DLSS does not appear in this menu, select it within the game).\n" +
                "5. In the game, press the \"Insert\" key to open the menu.\n" +
                "6. In the menu, check the \"Frame Gen,\" \"Hud Fix,\" and \"FG Extended\" boxes.\n\n" +

                "Others Mods\n" +
                "1 - Select a version of the mod of your choice (it is recommended 0.10.3 onwards or Uniscaler).\n" +
                "2 - Select the folder where the game\'s exe is located (something like ROTTR.exe).\n" +
                "3 - Activate Fake Nvidia GPU (if you don\'t have Rtx 3xxx/4xxx series)."
            },
            {
                "Ripout",
                "1 - Select a mod of your preference (0.10.3 is recommended).\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\n" +
                "3 - In the game, select DLSS and Frame Generation.\n"
            },
            {
                "Saints Row",
                "Default Mods\n"+
                "1 - Select a mod of your preference (0.10.3 is recommended).\n" +
                "2 - Choose the path for the overlay, under Epic Games Overlay, and select \"Disable.\"\n" +
                "3 - Start the game in DX12.\n" +
                "4 - Inside the game, select FSR.\n\n"+

                 "FSR 3.1.2/DLSS Custom SR\n"+
                "1 - Select FSR 3.1.2/DLSS Custom SR and install.\r\n" +
                "2 - Check the 'Enable Signature Over' box.\r\n" +
                "3 - In the game, select FSR 2 and press the 'Insert' key to open the menu.\r\n" +
                "4 - In the menu, check the 'Frame Gen' and 'Hud Fix' boxes, increase 'Hud Fix' until the HUD is corrected."
            },
            {
                "Sackboy: A Big Adventure",
                "1 - Select a version of the mod of your choice (version 0.10.3 is recommended).\n" +
                "2 - Select the game folder that has the ending \"\\GingerBread\\Binaries\\Win64\".\n" +
                "3 - Enable Fake Nvidia GPU (Only for AMD GPUs).\n" +
                "4 - In \"Mod Operates\", select \"Replace DLSS FG\".\n" +
                "5 - Select 'Default' in Nvngx.dll.\n" +
                "6 - Enable the \"Enable Signature Override\" checkbox if the mod doesn’t work.\n"
            },
            {
                "Shadow of the Tomb Raider",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, Select DLSS and press the Insert key to open the menu.\n" +
                "4. In the menu, enable Frame Gen, HUD Fix, and FG Extended.\n\n" +

                "Uniscaler V3\n" +
                "1 - Select Uniscaler V3.\n" +
                "2 - In Mod Operates, select XESS, and in Frame Gen Method, select FSR3.\n" +
                "3 - If you don't have an RTX GPU, check the Nvngx.dll box and select Default.\n" +
                "4 - In the game, turn off Anti-Aliasing and set XESS to Quality.\n" +
                "5 - To fix the HUD error, go to settings after completing the step above, turn off XESS, and select SMAA in Anti-Aliasing.\n"
            },
            {
                "Shadow Warrior 3",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and press the insert key to open the menu.\n" +
                "4. In the menu, check the Frame Gen, Hud Fix, and FG Extended boxes.\n" +
                "Steam: Library > Game > Gear icon to the right of Achievements > Properties > Launch Options, add -dx12.\n" +
                "Others: Go to the game's .exe > Properties and add -dx12 after the .exe.\n\n" +

                "Others Mods\n" +
                "1 - Select a mod of your preference (0.10.3 is recommended).\n" +
                "2 - Inside the game, select FSR. (You can use it with DLSS but there might be flickering).\n" +
                "3 - Set Ambient Occlusion and Post Processing to Low.\n"
            },
            {
              "Silent Hill 2",
              "FSR 3.1.2/DLSS FG Custom / FSR 3.1/DLSS OptiScaler:\n" +
              "1. Select the FSR 3.1/DLSS mod.\n" +
              "2. Check the \"GPU\" window that will appear.\n" +
              "3. In-game, select DLSS before starting the campaign.\n" +
              "4. During the campaign, press the \"Insert\" key to open the menu.\n" +
              "5. In the menu, select the desired upscaler, check the Frame Gen and Hud Fix boxes.\n\n" +

              "FSR3 Native:\n" +
              "1. Select FSR3 FG Native SH2 and install it.\n" +
              "2. In-game, select FSR 3.0 before starting the campaign.\n\n" +

              "RX 500/5000 and GTX Native FSR3 FG:\n" +
              "1 - Select Native FSR3 FG and install it.\r\n" +
              "2 - Select 'Yes' in the 'GPU' window that will appear.\r\n" +
              "3 - This is required only for the Native FSR3 FG mod.\r\n\r\n"+

              "DLSS FG RTX\n"+
              "1 - Select DLSS FG RTX and install.\r\n" +
              "2 - In the game, select DLSS and press the 'Home' key to open the menu.\r\n" +
              "3 - Press '*' to enable FG or enable it through the menu.\r\n\r\n"+

              "ReShade Installation"+
              "1 - Install ReShade.\r\n" +
              "2 - Inside ReShade, select the game's .exe and click next.\r\n" +
              "3 - Select DX 10/11/12 and click next.\r\n" +
              "4 - Click 'Browse' and locate the file 'Silent hill dark.ini' that was installed in the folder selected in the Utility and click next.\r\n" +
              "5 - In the game, press the 'Home' key to open the menu and select the options you prefer.\r\n" +
              "6 - Install the Preset first and then the FSR3 mod if you plan to use it.\r\n\r\n" +

              "Ultra Plus\n"+
              "1 - Install the mod and open the game.\r\n" +
              "2 - In the game, select a graphic preset from the game itself: low/medium/high/epic.\r\n" +
              "3 - Select FSR3 to use the native Frame Gen. If you don't see a change in FPS, go back to the Utility and install 'FSR3 FG Native SH2.'\r\n" +
              "4 - To get the full performance from the mod, select Xess Ultra Quality. (This option is very demanding, and you may experience a significant FPS drop.)\r\n" +
              "5 - Lock the game's FPS to half of your monitor's Hz minus 2, for example (60Hz, lock at 28), or lock it to the average FPS you get in the game.\r\n" +
              "6 - This mod works with FSR 3.1.1/DLSS (Optiscaler or Custom); just install and follow the FSR 3.1.1/DLSS guide above.\r\n\r\n"+

              "Ultra Plus Optimized\n"+
              "It is recommended for RX 500/5000 and GTX users.\r\n" +
              "Includes some graphical optimizations.\r\n\r\n"+

              "Ultra Plus Complete\n"+
              "This is the standard version of the mod with all available modifications.\r\n\r\n"+

              "Some of the modifications\n"+
              "Reduces black spots, improves Ray Tracing quality, enhances anisotropic filtering, reduces hair pixelation caused by MSAA.\r\n\r\n"+

              "Others Mods Sh2:\n" +
              "Unlock FPS Cutscenes:\n" +
              "Removes the 30fps lock from cutscenes.\n\n" +

              "Post-Processing:\n" +
              "Scene Color Fringe\n" +
              "Motion blur\n" +
              "Distortion"
            },
            {
              "Smalland",
              "1 - Select a mod of your choice. (0.10.3 is recommended)\n" +
              "2 - Check the Fake Nvidia GPU box. (AMD/GTX)\n" +
              "3 - In the game, select DLSS\n"
            },
            {
                "Spider Man/Miles",
                "FSR 3.1.2/DLSS FG (Only Optiscaler):\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. When launching the game, select DLSS in the initial settings menu. If that’s not possible, select it in-game.\n" +
                "4. In the game, press the \"Insert\" key to open the menu.\n" +
                "5. In the menu, check the Frame Gen and Hud Fix boxes."
            },
            {
              "S.T.A.L.K.E.R. 2",
              "Preset\n" +
              "1. Install ReShade\n" +
              "2. Inside ReShade, select the game’s .exe and click next\n" +
              "3. Select DX 10/11/12 and click next\n" +
              "4. Click \"Browse\" and locate the file Stalker2 REAL LIFE.ini that was installed in the folder selected in the Utility and click Next\n" +
              "5. In the game, press the Home key to open the menu and select the options you prefer"
            },

            {
              "Star Wars Jedi: Survivor",
              "DLSS Jedi (if you have RTX, use this mod if you want to use native DLSS):\n" +
              "1. Check the box Fake Nvidia GPU (GTX and AMD), Nvapi Results (GTX and AMD), and UE Compatibility (AMD).\n" +
              "2. If you can't see DLSS in-game, select \"DLSS\" under \"Mod Operates\".\n" +
              "3. In-game, select DLSS 3 and Frame Gen.\n\n" +

              "ALL GPUs:\n" +
              "1. Select Uniscaler FSR 3.1.\n" +
              "2. Check the \"Enable Signature Over\" box.\n" +
              "3. If you cannot see DLSS in the game, check the Nvngx.dll box, select Default, and reinstall the mod.\n" +
              "4. If you have an RTX GPU and want to use native DLSS, select DLSS in \"Mod Operates.\" Other GPUs can use FSR 3.1/XESS.\n" +
              "5. In the game, select DLSS FG.\n\n" +

              "Others Mods:\n" +
              "Intro Skip: Removes the initial intro when opening the game.\n\n" +

              "Anti Stutter:\n" +
              "1. Faster Loading Times.\n" +
              "2. Enhanced Streaming and Level Loading.\n" +
              "3. Optimized CPU and GPU Utilization.\n\n" +

              "Fix RT:\n" +
              "Fixes any crashes you experience with ray tracing enabled and also fixes occlusion culling. When you turn the camera and see white flashing at the corners of the screen, this resolves it and any crashes.\n\n" +

              "Graphic Preset:\n" +
              "1. Install ReShade.\n" +
              "2. In ReShade, select Star Wars Jedi: Survivor.\n" +
              "3. Select DirectX 10/11/12.\n" +
              "4. Click \"Browse\" and locate the file STARWAR-ULTRA-REALISTA.ini that was installed in the destination folder selected in the Utility.\n" +
              "5. Finish the installation and open the game.\n" +
              "6. In the game, press the \"Home\" key to open the menu and select the graphic options you prefer."
            },
            {
              "Star Wars Outlaws",
              "RTX:\r\n" +
              "1 - Select Star Wars DLSS RTX and install.\r\n" +
              "2 - Inside the game, select DLSS and Frame Gen.\r\n\r\n" +

              "All GPUs:\r\n" +
              "1 - Select FSR 3.1/DLSS Optiscaler.\r\n" +
              "2 - Inside the game, select an upscaler of your choice.\r\n" +
              "3 - Press the Insert key to open the menu and select an upscaler of your choice.\r\n\r\n" +

              "Graphic Preset:\n" +
              "1 - Install the mod and the ReShade application.\n" +
              "2 - In ReShade, select b1.exe, DirectX 10/11/12,\n" +
              "   click on \"Browser,\" and find the file Outlaws2.ini (the path\n" +
              "   should look something like Star Wars Outlaws\\Outlaws2.ini)\n" +
              "   and select it, then click on \"Uncheck All\" and \"Next.\"\n" +
              "3 - In the game, press the \"Insert\" key to open\n" +
              "   the menu and check the options you want.\n"
            },
            {
                "Suicide Squad: Kill the Justice League",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select the .exe path. (Stones\\Binaries\\Win64).\n" +
                "2. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "3. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, check the Frame Gen, Hud Fix, and FG Extended boxes.\n" +
                "5. The game may crash the first time the mod is installed."
            },
            {
              "Test Drive Unlimited Solar Crown",
              "1. Select Uniscaler FSR 3.1.\n" +
              "2. Check the box \"Enable Signature Over.\"\n" +
              "3. In-game, select DLSS FG.\n" +
              "4. If you can't see DLSS in the game, check the \"Nvngx.dll\" box, select \"Default,\" and install again."
            },
            {
                "The Ascent",
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and press the insert key to open the menu.\n" +
                "4. In the menu, check the Frame Gen and Hud Fix boxes; if the HUD is not fully corrected, also check the FG Extended box."
            },
            {
              "The Callisto Protocol",

              "The Callisto Protocol Fsr3\r\n"+
              "1 - Select The Callisto Protocol Fsr3.\r\n" +
              "2 - Check the Fake Nvidia GPU box and install.\r\n\r\n" +

              "FSR 3.1.2/DLSS Custom Callisto\n" +
              "1 - Select FSR 3.1.2/DLSS Custom Callisto and install.\r\n" +
              "2 - Check the 'Enable Signature Over' box.\r\n" +
              "3 - In the game, select FSR 2 and press the 'Insert' key to open the menu.\r\n" +
              "4 - In the menu, check the 'Frame Gen' and 'Fix Hud' boxes, select 2 in 'Fix Hud'.\r\n" +
              "5 - Disable MSI Afterburner.\r\n\r\n" +

              "Optiscaler Callisto\r\n" +
              "1 - Select FSR 3.1.1/DLSS Callisto and install it.\r\n" +
              "2 - In the game, select FSR 2 (which will act as DLSS for the mod to work).\r\n" +
              "3 - Press the 'Insert' key to open the menu.\r\n" +
              "4 - Select the Upscaler of your choice.\r\n" +
              "5 - Check the 'Frame Gen' and 'Hud Fix + Extended' boxes.\r\n\r\n" +

              "0.10.4:\r\n" +
              "1 - Select 0.10.4 and install it.\r\n" +
              "2 - Inside the game, select FSR 2 and start the campaign.\r\n" +
              "3 - If Frame Gen doesn't work, check the \"Nvngx.dll box andselect Default, then check the Enable Signature Override box. For Epic Games users: if the mod doesn't work or some bugs appear, check the \"Disable Overlay\" box.\r\n\r\n"+

              "Uniscaler V3\r\n" +
              "1 - Select Uniscaler V3.\r\n" +
              "2 - Check the Nvngx box and select Default.\r\n" +
              "3 - Check the Enable Signature Over box.\r\n\r\n" +

              "HUD Correction (FSR 3.1.1/DLSS is not necessary.) \r\n" +
              "Select FSR2 and start the campaign, play for a few seconds, and return to the menu. In the menu, select Temporal and return to the campaign.\r\n\r\n" +

              "Real Life:\r\n" +
              "Adds more detail to the world, making the wood effects stand out more, as well as the ground, lighting, walls, dirt marks, and skin.\r\n\r\n" +

              "TCP:\r\n" +
              "A ReShade config that implements duller colours, nearby sharpness, and distant depth of field blur to give a grittier and more cinematic style to emphasise the sci-fi horror atmosphere.\r\n\r\n" +

              "1 - Install the ReShade application.\r\n" +
              "2 - Select DirectX 10/11/12, click 'Browse,' and select the TCP.ini file that was installed in the destination folder chosen in the Utility.\r\n" +
              "3 - Click 'Uncheck All,' and then click 'Next.'\r\n" +
              "4 - Do the same for the Real Life mod."
            },
            {
              "The Casting Of Frank Stone",
              "FSR 3.1.2/DLSS FG Custom:\n" +
              "1. Select 'FSR 3.1.2/DLSS FG Custom', install it, and check the GPU window that appears.\n" +
              "2. In the game, select DLSS and Frame Generation.\n" +
              "3. If you want to use FSR 3.1, press the 'Insert' key to open the menu and select FSR 3.1.\n\n" +

              "0.10.4:\n" +
              "1. Select 0.10.4 and install it.\n" +
              "2. In the game, select DLSS and Frame Generation.\n" +
              "3. If you can't see DLSS in the game, click on 'Nvngx.dll' and then on 'Default.'\n\n" +

              "Optiscaler FSR 3.1.1/DLSS:\n" +
              "1. Select 'Optiscaler FSR 3.1.1/DLSS' and install it.\n" +
              "2. In the game, select DLSS and Frame Generation.\n" +
              "3. If you want to use FSR 3.1, press the 'Insert' key to open the menu and select FSR 3.1."
            },
            {
               "The Witcher 3",
               "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
               "1. Install the mod in the \"x64_dx12\" folder.\n" +
               "2. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
               "3. Check the Enable Signature Over box.\n" +
               "4. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
               "5. In the menu, check the Frame Gen and FG Hud fix. If you want to use FSR 3.1.2, select FSR 3x in \"Upscales.\"\n" +
               "6. For RTX users, this mod is not compatible with RT; if you want to use RT, use the Uniscaler V4 mod.\n\n" +

               "Uniscaler V4\n" +
               "1. Select Uniscaler V4 and install.\n" +
               "2. For AMD/GTX users: Check the boxes \"Fake Nvidia GPU\" and \"Enable Signature Over.\"\n" +
               "3. In the game, select DLSS and Frame Gen boxes."
            },
            {
                "Uncharted Legacy Of Thieves",
                "FSR 3.1.2/DLSS FG (Only Optiscaler)\n" +
                "1. Select FSR 3.1.2/DLSS FG (Only Optiscaler) and install.\n" +
                "2. Check the Enable Signature Over box.\n" +
                "3. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
                "4. In the menu, check the Frame Gen and Hud Fix boxes.\n" +
                "5. Close Afterburner, or the game will crash.\n\n" +

                "Others Mods\n" +
                "1. Select a mod of your preference (0.10.3 is recommended).\n" +
                "2. Run the game using the u4-l.exe executable. The game may crash the first time, so just run it again.\n" +
                "3. Inside the game, select FSR."
            },
            {
              "Unknown 9: Awakening",
              "1 - Select Uniscaler V4 and install it.\r\n" +
              "2 - In the game, select an option under Upscaling Quality.\r\n" +
              "3 - Check the box 'Enable Signature Override.'\r\n" +
              "4 - You can also select XESS in 'Mod Operates'; it has better quality, but it's optional.\r\n" +
              "5 - This game does not yet have a fix for the HUD.\r\n\r\n"
            },
            {
              "Until Dawn",
              "FSR 3.1.2/DLSS FG Custom / Optiscaler:\n" +
              "1. Select FSR 3.1.2/DLSS FG Custom or Optiscaler and install it.\n" +
              "2. In the game, select DLSS and press the \"Insert\" key to open the menu.\n" +
              "3. In the menu, select Frame Generation and an upscaler of your choice.\n" +
              "4. To fix the HUD bug, select Hud Fix FG in the menu.\n" +
              "5. You can also use the game's Frame Generation after selecting an upscaler in the menu."
            },
            {
              "Warhammer: Space Marine 2",
              "FSR 3.1.3/DLSS FG Marine\n" +
              "1. Select FSR 3.1.3/DLSS FG Marine and install.\n" +
              "2. In the game, select DLSS, Frame Gen, and press the \"Insert\" key to open the menu.\n" +
              "3. In the menu, select FSR 3x to use the FSR 3.1.3.\n\n" +

              "FSR 3.1.2/DLSS FG Custom / Optiscaler:\n" +
              "1. Select FSR 3.1.2/DLSS FG Custom and install it.\n" +
              "2. In the game, press the \"Insert\" key to open the menu.\n" +
              "3. In the menu, select Frame Generation and an upscaler of your choice.\n" +
              "4. To fix the HUD bug, select Hud Fix FG in the menu.\n\n" +

              "Uniscaler FSR 3.1:\n" +
              "1 - Check the 'Fake NVIDIA GPU' box if you want to use DLSS.\n" +
              "2 - Check the 'Enable Signature Over' box.\n" +
              "3 - This mod does not have a HUD fix, as frame generation is activated along with the mod.\n\n" +

              "Graphic Preset:\n" +
              "1 - Install the mod and the ReShade application.\r\n" +
              "2 - In ReShade, select SpaceMarine2.exe (or select the game .exe), DirectX 10/11/12, click on 'Browse,' find the fileWarhammer 40000 Space Marine 2.ini (the path should look something like client_pc\\root\\bin\\pc), select it, then click on 'Uncheck All' and 'Next.'\r\n" +
              "3 - In the game, press the 'Insert' key to open the menu and check the options you want.\r\n" +
              "4 -For the mod to work, you need to select the folder where the Warhammer 40000 Space Marine 2.ini file is located; the path is similar to client_pc\\root\\bin\\pc."
            },
            {
                "Watch Dogs Legion",
                "1 - Select the FSR 3.1.1/DLSS Optiscaler FG Custom and install.\r\n" +
                "2 - In the game, select DLSS and press the 'Insert' key to open the menu.\r\n" +
                "3 - In the menu, select an Upscaler, check 'Frame Gen' and 'Fix HUD' boxes.\r\n" +
                "4 - To install DLSS 3.7.20, click the 'Nvngx.dll' button and select 'DLSS 3.7.20'.\r\n\r\n"
            },

        };

        #endregion

        #region Guide Links
        Dictionary<string, string> guideLinks = new Dictionary<string, string>
        {
            { "FSR 3.1.2/DLSS FG (Only Optiscaler)", "https://www.youtube.com/embed/NEd0S5V6pns?autoplay=1&fs=1&controls=1&modestbranding=1&rel=0&enablejsapi=1" },
            { "Resident Evil 4 Remake", "https://www.youtube.com/embed/B28ntG0ef4E?autoplay=1&fs=1&controls=1&modestbranding=1&rel=0&enablejsapi=1\r\n" }
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

            //Exit Button
            panel5.Top = panel1.Top - panel5.Height;
            panel5.Left = panel1.Right - panel5.Width + 3;

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
                {"Alan Wake Remastered", "AwRemaster2.png"},
                {"Alan Wake 2","AW2.png"},
                {"Assassin\'s Creed Mirage","AcMirage2.png"},
                {"Optiscaler Method","Opti.png"},
                {"Optiscaler FSR 3.1.1/DLSS","Opti.png"},
                {"FSR 3.1.2/DLSS FG (Only Optiscaler)","Opti.png"},
                {"Add-on Mods","Addon.png" },
                {"Alone in the Dark","Alone.png"},
                {"A Plague Tale Requiem","Requiem.png"},
                {"A Quiet Place: The Road Ahead","QuietPlace2.png"},
                {"Assassin's Creed Valhalla","AcVal.png"},
                {"Atomic Heart","Atomic.png"},
                {"Baldur's Gate 3","Baldurs.png"},
                {"Black Myth: Wukong","wukong.png" },
                {"Blacktail","Black.png"},
                {"Banishers Ghost of New Eden","Banishers.png"},
                {"Bright Memory: Infinite","Bmi.png"},
                {"Brothers a Tale of Two Sons","Brothers.png"},
                {"Chernobylite","Cherno.png"},
                {"Cod Black Ops Cold War","Cod.png"},
                {"Cod MW3","mw3.png"},
                {"Control","Control.png"},
                {"Crime Boss Rockay City","Rockay.png"},
                {"Crysis 3 Remastered","Crysis2.png"},
                {"Cyberpunk 2077","Cyber.png"},
                {"Dakar Desert Rally","Dakar.png"},
                {"Dead Space Remake","DeadSpace.png"},
                {"Dead Island 2","Dead2.png"},
                {"Dead Rising Remaster","Drr2.png"},
                {"Death Stranding Director's Cut","Ds.png"},
                {"Deathloop","Df.png"},
                {"Dragon Age: Veilguard","DgVeil2.png"},
                {"Dragons Dogma 2","Dg2.png"},
                {"Dying Light 2","Dl2.png"},
                {"Elden Ring","Elden.png"},
                {"Everspace 2","Es2.png"},
                {"Evil West","Ew.png"},
                {"Fist Forged in Shadow Torch","Fist.png"},
                {"Flintlock: The Siege of Dawn","Flint.png"},
                {"Fort Solis","Fort.png"},
                {"Forza Horizon 5","Forza.png"},
                {"Final Fantasy XVI","Ffxvi2.png"},
                {"F1 2022","F1.png"},
                {"F1 2023","F1_23.png"},
                {"GTA Trilogy","GtaTrilogy2.png"},
                {"GTA V","GtaV.png"},
                {"Ghost of Tsushima","GhostT.png"},
                {"Ghostrunner 2","Ghost2.png"},
                {"Ghostwire: Tokyo","Ghostwire2.png"},
                {"God Of War 4","Gow42.png"},
                {"God of War Ragnarök","GowRag.png"},
                {"Gotham Knights","Gk2.png"},
                {"Hellblade: Senua's Sacrifice","Hell.png"},
                {"Hellblade 2","Hell2.png"},
                {"High On Life","Hol.png"},
                {"Hitman 3","Hitman.png"},
                {"Hogwarts Legacy","Hog.png"},
                {"Horizon Forbidden West","HZDF.png"},
                {"Horizon Zero Dawn","Hzd2.png" },
                {"Horizon Zero Dawn Remastered","HzdRem2.png"},
                {"Icarus","Icarus.png"},
                {"Indiana Jones and the Great Circle","Indy2.png"},
                {"Judgment","Jud.png"},
                {"Jusant","Jusant.png"},
                {"Kena: Bridge of Spirits","KENA.png"},
                {"Layers of Fear","Layers.png"},
                {"Lego Horizon Adventures","LegoHzd2.png"},
                {"Lies of P","Lop.png"},
                {"Loopmancer","Loopmancer.png"},
                {"Lords of the Fallen","Lotf.png"},
                {"Manor Lords","Manor.png"},
                {"Martha Is Dead","Martha.png"},
                {"Marvel's Avengers","Avengers2.png"},
                {"Marvel's Guardians of the Galaxy","Got.png"},
                {"Microsoft Flight Simulator 24","FlightSimulator242.png"},
                {"Metro Exodus Enhanced","Metro.png"},
                {"Monster Hunter Rise","Mrise.png"},
                {"Nobody Wants To Die","Nobody.png"},
                {"Outpost Infinity Siege","Outpost.png"},
                {"Pacific Drive","Pacific.png"},
                {"Palworld","Palworld.png"},
                {"Ratchet and Clank","Ratchet.png"},
                {"Rise of The Tomb Raider","Rtb.png"},
                {"Ready or Not","Ready.png"},
                {"Red Dead Redemption","Rdr12.png"},
                {"Red Dead Redemption 2","RDR2.png"},
                {"Resident Evil 4 Remake","Re42.png"},
                {"Returnal","Returnal2.png"},
                {"Ripout","Ripout.png"},
                {"Saints Row","SaintsRow2.png"},
                {"Sackboy: A Big Adventure","Sackboy2.png"},
                {"Shadow of the Tomb Raider","ShadowTomb2.png"},
                {"Shadow Warrior 3","Shadow32.png"},
                {"Smalland","Smalland.png"},
                {"Spider Man/Miles","Spider2.png"},
                {"Silent Hill 2","Sh22.png"},
                {"S.T.A.L.K.E.R. 2","Stalker2.png"},
                {"Star Wars Jedi: Survivor","JediSurvivor2.png"},
                {"Star Wars Outlaws","Outlaws2.png" },
                {"Suicide Squad: Kill the Justice League","Sskjl2.png"},
                {"Test Drive Unlimited Solar Crown","TestSolar2.png"},
                {"The Ascent","Ascent2.png"},
                {"The Callisto Protocol","Callisto2.png"},
                {"The Casting Of Frank Stone","FrankStone2.png" },
                {"The Witcher 3","Tw32.png"},
                {"Uncharted Legacy Of Thieves","Uncharted2.png"},
                {"Unknown 9: Awakening","Unknown92.png" },
                {"Until Dawn","Until2.png" },
                {"Warhammer: Space Marine 2","SpaceMarine2.png"},
                {"Watch Dogs Legion","Legion2.png"}
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

            if (listBox1.SelectedItem != null && guideLinks.TryGetValue(listBox1.SelectedItem.ToString(), out string videoLink))
            {
                buttonVideo.Visible = true;
            }
            else
            {
                buttonVideo.Visible = false;
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

        private void buttonVideo_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem != null && guideLinks.TryGetValue(listBox1.SelectedItem.ToString(), out string videoLink))
            {

                buttonExit.Visible = true;

                webView21.Dock = DockStyle.Fill;
                string tutorialVideo = videoLink;
                webView21.Source = new Uri(tutorialVideo);
                webView21.Visible = true;
            }
            else
            {
                buttonExit.Visible = false;
                webView21.Visible = false;
            }
        }

        private async void buttonExit_Click(object sender, EventArgs e)
        {
            string pauseVideoGuide = "var video = document.querySelector('video'); if (video) video.pause();";
            await webView21.CoreWebView2.ExecuteScriptAsync(pauseVideoGuide);
            webView21.Visible = false;
            buttonExit.Visible = false;
        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show("The video could not be loaded. Please check your internet connection or try again later.", "Error loading video", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}