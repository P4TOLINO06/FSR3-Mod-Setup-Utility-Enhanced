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
                "Optiscaler FSR 3.1/DLSS",
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
                "Black Myth: Wukong",

                "RTX:\r\n" +
                "1 - Select 'RTX DLSS FG Wukong' and install.\r\n" +
                "2 - In the game, select DLSS and Frame Generation.\r\n\r\n" +

                "AMD/GTX DLSS FG:\r\n" +
                "1 - Select Optiscaler FSR 3.1/DLSS and install it.\r\n" +
                "2 - In the game, press the 'Insert' key to open the menu, and in the menu, select the upscaler you want to use.\r\n\r\n" +

                "Graphic Preset:\r\n" +
                "1 - Install the mod and the ReShade application.\r\n" +
                "2 - In ReShade, select b1.exe, DirectX 10/11/12, click on 'Browse,' find the file Black Myth Wukong.ini (the path should look something like BlackMythWukong\\Black Myth Wukong.ini), select it, then click on 'Uncheck All' and 'Next.'\r\n" +
                "3 - In the game, press the 'Insert' key to open the menu and check the options you want.\r\n\r\n" +

                "Optimized Wukong:\r\n" +
                "1 - Faster Loading Times - By tweaking async-related settings: AsyncLoadingThread, the mod allows assets to load in the background, reducing loading times and potentially eliminating loading pauses during gameplay.\r\n\r\n" +

                "2 - Optimized CPU and GPU Utilization - By tweaking multi-core rendering (MultiCoreRendering) and multi-threaded shader compilation (MultiThreadedShaderCompile*), the mod allows the game to utilize the full potential of modern CPUs and GPUs. This can result in improved performance, higher frame rates, and more stable gameplay.\r\n\r\n" +

                "3 - Enhanced Streaming and Level Loading - By tweaking various streaming variables (r.Streaming., s.LevelStreamingComponents*), the mod improves the efficiency of streaming assets and level loading. This can lead to faster streaming and reduced stuttering when moving through different areas of the game world.\r\n\r\n" +

                "4 - Optimized Memory Management - By adjusting memory-related settings (MinBulkDataSizeForAsyncLoading & ForceGCAfterLevelStreamedOut*), the mod optimizes memory allocation and garbage collection. This can lead to more efficient memory usage, reduced memory-related stutters, and improved overall performance.\r\n"
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
                "FFXVI DLSS RTX:\r\n" +
                "1. Select \"FFXVI DLSS RTX\" and install it.\r\n" +
                "2. In the game, select Frame Generation.\r\n\r\n" +

                "FFXVI DLSS ALL GPU:\r\n" +
                "1. Select \"FFXVI DLSS ALL GPU\", install it, and choose an option in the window that will appear.\r\n" +
                "2. In the game, select Frame Generation.\r\n\r\n" +

                "Optiscaler FSR 3.1/DLSS:\r\n" +
                "1. Select Optiscaler FSR 3.1/DLSS and install it.\r\n" +
                "2. In the game, press the Insert key and choose the Upscaler you want to use.\r\n" +
                "3. Select Frame Generation.\r\n"
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
                "1- Select Uniscaler V3\r\n" +
                "2 - Check the Fake Nvidia GPU box (AMD/GTX). If you can't see DLSS in the game, also check the Nvapi Results box.\r\n" +
                "3 - Check the Nvngx.dll box and select Default, then check the Enable Signature Override box.\r\n" +
                "4 - In the game, select DLSS to enable Frame Generation.\r\n" +
                "5 - To fix the HUD glitch, switch between the upscalers (FSR, DLSS, etc.) until the HUD stops flickering.\r\n"
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
                "4 - Select 'Default' in Nvngx.dll.\r\n"
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
                "1 - Select Horizon Forbidden West FSR3 or Optiscaler FSR 3.1/DLSS and install\r\n" +
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
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - Activate Fake Nvidia GPU and Nvapi Results (AMD only).\r\n"
            },
            {
                "Layers of Fear",
                "1 - Select a mod of your preference (0.10.3 is recommended).\r\n" +
                "2 - Check the box Fake Nvidia GPU (AMD/GTX).\r\n" +
                "3 - If you don't notice Frame Generation, select Replace DLSS FG in 'Mod Operates'.\r\n" +
                "4 - In the game, select Frame Generation and DLSS or FSR.\r\n"
            },
            {
                "Lies of P",
                "1 - Select a version of the mod of your choice (version 0.10.4 is recommended).\r\n" +
                "2 - Activate Fake Nvidia Gpu and UE Compatibility Mode (AMD only).\r\n" +
                "3 - To fix the flickering of the HUD, first select DLSS Quality,\r\n" +
                "   then select FSR Quality (without disabling DLSS), then select DLSS again.\r\n"
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
                "1 - Select Uniscaler.\r\n" +
                "2 - Check the boxes for Fake Nvidia GPU (AMD/GTX) and Nvapi Results (GTX). If the DLSS option is not available for AMD GPU, check the Nvapi Results box.\r\n" +
                "3 - In Nvngx.dll, select Default and check the box Enable Signature Override.\r\n" +
                "4 - In the game, select DLSS.\r\n"
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
                "Red Dead Redemption 2",
                "● RDR2_Build_2\r\n" +
                "1 - Launch the game, go to settings, turn off Triple buffering + V-sync, unlock\r\n" +
                "advanced settings, and change API to DX12. Then restart the game, turn on DLSS (RTX) or FSR2 (non-RTX) (Required settings before playing the game).\r\n\n" +
                "• Attention, don't install another version of the Reshade app after using this mod.\r\n\n" +
                "If your game still does not open after turning off Afterburner and Rivatuner, try\r\n" +
                "setting 'Run this program as administrator' and 'Run this program in compatibility\r\n" +
                "mode for Windows 7' in the Compatibility tab of the Properties in the right\r\n" +
                "mouse menu.\r\n\n" +
                "2 - While playing, press the hotkey 'End' to go to the mod menu \r\n" +
                "(don't set anything in the lobby (main game menu before playing),\r\n" +
                "if you turn Frame Generation On or Off, it may cause an ERR_GFX_STATE error),\r\n" +
                "set DLSS (RTX) or FSR3 (non-RTX), and toggle Frame Generation Off and On again.\r\n" +
                "If you have a black screen, check Upscale Type in the menu mod again,\r\n" +
                "change from DLSS to FSR3.\r\n\n" +
                "3 - Check again with the toggle 'Enable UI Hud Fix' On or Off. If you see UI\r\n" +
                "flickering when turning 'Enable UI Hud Fix' Off, that means the mod works.\r\n\n" +
                "Other versions of the mod install normally, but may experience flickering on the HUD.\r\n"
            },
            {
                "Red Dead Redemption 2 MIX",
                "1 - Set the game to DX12 in advanced options.\r\n" +
                "2 - Turn off Triple Buffering and Vsync, and set the game to Full Screen.\r\n" +
                "3 - Select the RDR2 Mix mod and install it.\r\n" +
                "4 - You don’t need to use any upscaler, as Frame Generation is automatically activated.\r\n" +
                "However, if you want to use an upscaler, when installing, check the 'Addon Mods' box,\r\n" +
                "select 'Optiscaler,' and below in DX11 select 'FSR 2.1 DX11,' and in DX12 select 'FSR 2.1 DX12.'\r\n"
            },
            {
                "Red Dead Redemption Mix 2",
                "1 - Set the game to DX12.\r\n" +
                "2 - Turn off Triple Buffering, Vsync, and disable any upscaler;\r\n" +
                "leave it on TAA.\r\n" +
                "3 - It’s not necessary to activate any upscaler for the mod to work,\r\n" +
                "but if you want to use one, refer to the RDR2 Mix guide.\r\n"
            },
            {
                "Red Dead Redemption V2",
                "1 - Turn off Vsync, Triple Buffering, and set the game to DX12.\r\n" +
                "2 - Install the mod.\r\n" +
                "3 - In-game, press the 'END' key and select an upscaler.\r\n" +
                "If you want to use native resolution, check the Native Resolution box below the upscaler, and restart the game.\r\n" +
                "(FSR3 Upscaling or FSR 2 is recommended; you can also try others, but they may not work.)\r\n"
            },
            {
                "RDR2 Non Steam",
                "1 - Leave the game in DX12 and turn off Vsync/Triple Buffering.\r\n" +
                "2 - In-game, if you have an RTX card, enable DLSS. If you don’t have an RTX card, disable any upscaler and turn on TAA. Press the 'END' key, select FSR3 in Upscaler Type, and check the box 'Enable Frame Generation.'\r\n" +
                "3 - If a DLL error occurs, reinstall the mod and click 'YES' in the DLL file installation message box.\r\n"
            },
            {
                "The Callisto Protocol",
                "The Callisto Protocol Fsr3\r\n"+
                "1 - Select The Callisto Protocol Fsr3.\r\n" +
                "2 - Check the Fake Nvidia GPU box and install.\r\n\r\n" +

                "Uniscaler V3\r\n" +
                "1 - Select Uniscaler V3.\r\n" +
                "2 - Check the Nvngx box and select Default.\r\n" +
                "3 - Check the Enable Signature Over box.\r\n\r\n" +

                "HUD Correction:\r\n" +
                "Select FSR2 and start the campaign, play for a few seconds, and return to the menu. In the menu, select Temporal and return to the campaign.\r\n\r\n" +

                "Real Life:\r\n" +
                "Adds more detail to the world, making the wood effects stand out more, as well as the ground, lighting, walls, dirt marks, and skin.\r\n\r\n" +

                "TCP:\r\n" +
                "A ReShade config that implements duller colours, nearby sharpness, and distant depth of field blur to give a grittier and more cinematic style to emphasise the sci-fi horror atmosphere.\r\n\r\n" +

                "1 - Install the ReShade application.\r\n" +
                "2 - Select DirectX 10/11/12, click 'Browse,' and select the TCP.ini file that was installed in the destination folder chosen in the Utility.\r\n" +
                "3 - Click 'Uncheck All,' and then click 'Next.'\r\n" +
                "4 - Do the same for the Real Life mod."
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
                {"Optiscaler FSR 3.1/DLSS","Opti.png"},
                {"Add-on Mods","Addon.png" },
                {"Alone in the Dark","Alone.png"},
                {"A Plague Tale Requiem","Requiem.png"},
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
                {"Cyberpunk 2077","Cyber.png"},
                {"Dakar Desert Rally","Dakar.png"},
                {"Dead Space Remake","DeadSpace.png"},
                {"Dead Island 2","Dead2.png"},
                {"Death Stranding Director's Cut","Ds.png"},
                {"Deathloop","Df.png"},
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
                {"GTA V","GtaV.png"},
                {"Ghost of Tsushima","GhostT.png"},
                {"Ghostrunner 2","Ghost2.png"},
                {"Ghostwire: Tokyo","Ghostwire2.png"},
                {"Hellblade: Senua's Sacrifice","Hell.png"},
                {"Hellblade 2","Hell2.png"},
                {"High On Life","Hol.png"},
                {"Hitman 3","Hitman.png"},
                {"Hogwarts Legacy","Hog.png"},
                {"Horizon Forbidden West","HZDF.png"},
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
                {"Metro Exodus Enhanced","Metro.png"},
                {"Monster Hunter Rise","Mrise.png"},
                {"Nobody Wants To Die","Nobody.png"},
                {"Outpost Infinity Siege","Outpost.png"},
                {"Pacific Drive","Pacific.png"},
                {"Palworld","Palworld.png"},
                {"Ratchet and Clank","Ratchet.png"},
                {"Rise of The Tomb Raider","Rtb.png"},
                {"Ready or Not","Ready.png"},
                {"Red Dead Redemption 2","RDR2.png"},
                {"Red Dead Redemption 2 MIX","Rdr2Mix.png"},
                {"Red Dead Redemption Mix 2","Rdr2Mix2.png"},
                {"Red Dead Redemption V2","Rdr2V2.png"},
                {"RDR2 Non Steam","Rdr2NSteam.png"},
                {"The Callisto Protocol","Callisto2.png"}
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