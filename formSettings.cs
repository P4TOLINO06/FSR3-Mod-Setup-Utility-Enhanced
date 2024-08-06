using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.Devices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;
using System.Drawing.Drawing2D;
using Tomlyn;
using Tomlyn.Model;
using System.Windows.Forms.Design;
using System.Security.Cryptography;
using Button = System.Windows.Forms.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Text.Json;
using System.Linq.Expressions;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formSettings : Form
    {
        public static string gameSelected { get; set; }
        public static string fsrSelected { get; set; }
        public formEditorToml EditorForm { get; set; }
        string path_fsr = "";

        private List<string> pendingItems = new List<string>();
        private static formSettings instance;
        public string selectMod;
        bool varLfz = false;
        private formEditorToml formEditor;
        private mainForm mainFormInstance;
        public System.Windows.Forms.TextBox fpsLimitTextBox;
        public System.Windows.Forms.Label labelFpsLimit;
        Form screenMethod = new Form(); //Optiscaler installation method screen

        public formSettings()
        {
            InitializeComponent();
            this.mainFormInstance = mainFormInstance;

            AddOptionsSelect.ItemCheck += new ItemCheckEventHandler(AddOptionsSelect_ItemCheck);
            listMods.SelectedIndexChanged += listMods_SelectedIndexChanged;
            this.Resize += new EventHandler(formSettings_Resize);
            TextBoxFpsLimit();
            SubMenuClose();
        }

        public static formSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new formSettings();
                }
                return instance;
            }
        }

        public void AddItemlistMods(List<string> items)
        {
            List<string> itensDelete = new List<string> { "Elden Ring FSR3", "Elden Ring FSR3 V2", "Elden Ring FSR3 V3", "Disable Anti Cheat", "Unlock FPS Elden" };

            List<string> gamesIgnore = new List<string> { "Cyberpunk 2077", "Red Dead Redemption 2", "Dying Light 2" }; //Ignore the removal of the default mods (0.7.6 etc.) for the games on the list

            if (itensDelete.Any(item => listMods.Items.Contains(item)))
            {
                foreach (string itemDelete in itensDelete)
                {
                    listMods.Items.Remove(itemDelete);
                }
            }
            if (listMods == null && !listMods.Items.Contains(items))
            {
                pendingItems.AddRange(items.Where(i => !pendingItems.Contains(i)));
            }
            else if (listMods != null && !listMods.Items.Contains(items) && !gamesIgnore.Contains(gameSelected))
            {
                listMods.Items.Clear();
                foreach (var item in items)
                {
                    listMods.Items.Add(item);
                }
            }
            else if (listMods != null && !listMods.Items.Contains(items) && gamesIgnore.Contains(gameSelected))
            {
                foreach (var item in items)
                {
                    listMods.Items.Add(item);
                }
            }
        }
        public void RemoveItemlistMods(List<string> items)
        {
            if (listMods == null)
            {
                pendingItems.RemoveAll(i => items.Contains(i));
            }
            else
            {
                foreach (var item in items)
                {
                    listMods.Items.Remove(item);
                }
            }
            listMods.Text = "";
        }

        public void ClearListMods()
        {
            listMods.Items.Clear();
        }

        #region Files Folder FSR 2.2
        Dictionary<string, string[]> origins_2_2_folder = new Dictionary<string, string[]>
         {
             { "0.7.4", new string[] { "mods\\FSR2FSR3_0.7.4\\FSR2FSR3_220" } },
             { "0.7.5", new string[] { "mods\\FSR2FSR3_0.7.5_hotfix\\FSR2FSR3_220" } },
             { "0.7.6", new string[] { "mods\\FSR2FSR3_0.7.6\\FSR2FSR3_220" } },
             { "0.8.0", new string[] { "mods\\FSR2FSR3_0.8.0\\FSR2FSR3_220" } },
             { "0.9.0", new string[] { "mods\\FSR2FSR3_0.9.0\\Generic FSR\\FSR2FSR3_220" } },
             { "0.10.0", new string[] { "mods\\FSR2FSR3_0.10.0\\Generic FSR\\FSR2FSR3_220" } },
             { "0.10.1", new string[] { "mods\\FSR2FSR3_0.10.1\\Generic FSR\\FSR2FSR3_220"} },
             { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_220" } },
             { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_220" } },
             { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_220" } },
             { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_220\\FSR2FSR3_220" } },
             { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod" } },
             { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod" } },
             { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } },
             { "Uniscaler V3", new string[]{  "mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
             { "Uniscaler FSR 3.1",new string[]{ "mods\\FSR2FSR3_Uniscaler_FSR3\\Uniscaler_FSR31"}}
         };
        #endregion

        #region Folder FSR 2.1
        Dictionary<string, string[]> origins_2_1_folder = new Dictionary<string, string[]>
        {
            { "0.7.4", new string[] { "mods\\FSR2FSR3_0.7.4\\FSR2FSR3_212" } },
            { "0.7.5", new string[] { "mods\\FSR2FSR3_0.7.5_hotfix\\FSR2FSR3_212" } },
            { "0.7.6", new string[] { "mods\\FSR2FSR3_0.7.6\\FSR2FSR3_212" } },
            { "0.8.0", new string[] { "mods\\FSR2FSR3_0.8.0\\FSR2FSR3_212" } },
            { "0.9.0", new string[] { "mods\\FSR2FSR3_0.9.0\\Generic FSR\\FSR2FSR3_210"} },
            { "0.10.0", new string[] { "mods\\FSR2FSR3_0.10.0\\Generic FSR\\FSR2FSR3_210"} },
            { "0.10.1", new string[] { "mods\\FSR2FSR3_0.10.1\\Generic FSR\\FSR2FSR3_210"} },
            { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_210" } },
            { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_210"} },
            { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_210" } },
            { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_210\\FSR2FSR3_210"} },
            { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod"} },
            { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod"} },
            { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } },
            { "Uniscaler V3", new string[]{"mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
            { "Uniscaler FSR 3.1",new string[]{ "mods\\FSR2FSR3_Uniscaler_FSR3\\Uniscaler_FSR31"}}
        };
        #endregion

        #region Folder FSR 2.0
        Dictionary<string, string[]> origins_2_0_folder = new Dictionary<string, string[]>
        {
            { "0.7.4", new string[] { "mods\\FSR2FSR3_0.7.4\\FSR2FSR3_201" } },
            { "0.7.5", new string[] { "mods\\FSR2FSR3_0.7.5_hotfix\\FSR2FSR3_201" } },
            { "0.7.6", new string[] { "mods\\FSR2FSR3_0.7.6\\FSR2FSR3_201" } },
            { "0.8.0", new string[] { "mods\\FSR2FSR3_0.8.0\\FSR2FSR3_201" } },
            { "0.9.0", new string[] { "mods\\FSR2FSR3_0.9.0\\Generic FSR\\FSR2FSR3_200"} },
            { "0.10.0", new string[] { "mods\\FSR2FSR3_0.10.0\\Generic FSR\\FSR2FSR3_200"} },
            { "0.10.1", new string[] { "mods\\FSR2FSR3_0.10.1\\Generic FSR\\FSR2FSR3_200"} },
            { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_200" } },
            { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_200"} },
            { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_200"} },
            { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_200\\FSR2FSR3_200"} },
            { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod"} },
            { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod"} },
            { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod"} },
            { "Uniscaler V3", new string[]{"mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
            { "Uniscaler FSR 3.1",new string[]{ "mods\\FSR2FSR3_Uniscaler_FSR3\\Uniscaler_FSR31"}}
        };
        #endregion

        #region Folder FSR SDK
        Dictionary<string, string[]> origins_sdk_folder = new Dictionary<string, string[]>
         {
             { "0.7.4", new string[] { "mods\\FSR2FSR3_0.7.4\\FSR2FSR3_SDK" } },
             { "0.7.5", new string[] { "mods\\FSR2FSR3_0.7.5_hotfix\\FSR2FSR3_SDK" } },
             { "0.7.6", new string[] { "mods\\FSR2FSR3_0.7.6\\FSR2FSR3_SDK" } },
             { "0.8.0", new string[] { "mods\\FSR2FSR3_0.8.0\\FSR2FSR3_SDK" } },
             { "0.9.0", new string[] { "mods\\FSR2FSR3_0.9.0\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.0", new string[] { "mods\\FSR2FSR3_0.10.0\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.1", new string[] { "mods\\FSR2FSR3_0.10.1\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_SDK" } },
             { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_SDK\\FSR2FSR3_SDK" } },
             { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod" } },
             { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod" } },
             { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } },
             { "Uniscaler V3", new string[]{"mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
             { "Uniscaler FSR 3.1",new string[]{ "mods\\FSR2FSR3_Uniscaler_FSR3\\Uniscaler_FSR31"}}
         };
        #endregion

        #region Fake Nvidia Gpu Toml File Path
        static Dictionary<string, string> folderFakeGpu = new Dictionary<string, string>()
        {
            {"0.7.4", @"\mods\Temp\FSR2FSR3_0.7.4\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.7.5", @"\mods\Temp\FSR2FSR3_0.7.5_hotfix\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.7.6", @"\mods\Temp\FSR2FSR3_0.7.6\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.8.0", @"\mods\Temp\FSR2FSR3_0.8.0\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.9.0", @"\mods\Temp\FSR2FSR3_0.9.0\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.0", @"\mods\Temp\FSR2FSR3_0.10.0\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.1", @"\mods\Temp\FSR2FSR3_0.10.1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.1h1", @"\mods\Temp\FSR2FSR3_0.10.1h1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.2h1", @"\mods\Temp\FSR2FSR3_0.10.2h1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.3", @"\mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.4", @"\mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"Uniscaler",@"\mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler + Xess + Dlss",@"\mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V2",@"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"},
            {"The Callisto Protocol FSR3",@"\mods\Temp\FSR3_Callisto\enable_fake_gpu\\fsr2fsr3.config.toml"}
        };
        #endregion

        #region Edit Fake Gpu List
        List<string> edit_fake_gpu_list = new List<string>
        {
            "0.10.2h1",
            "0.10.3",
            "0.10.4",
            "Uniscaler",
            "Uniscaler + Xess + Dlss",
            "Uniscaler V2",
            "Uniscaler V3",
            "Uniscaler FSR 3.1"
        };
        #endregion

        #region Edit Old Fake Gpu
        List<string> edit_old_fake_gpu = new List<string>
        {
            "0.7.4",
            "0.7.5",
            "0.7.6",
            "0.8.0",
            "0.9.0",
            "0.10.0",
            "0.10.1",
            "0.10.1h1"
        };
        #endregion

        #region Ue Compatibility Path
        Dictionary<string, string> folder_ue = new Dictionary<string, string>
            {
                { "0.9.0", @"\mods\Temp\FSR2FSR3_0.9.0\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.0", @"\mods\Temp\FSR2FSR3_0.10.0\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.1", @"\mods\Temp\FSR2FSR3_0.10.1\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.1h1", @"\mods\Temp\FSR2FSR3_0.10.1h1\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.2h1", @"\mods\Temp\FSR2FSR3_0.10.2h1\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.3", @"\mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.4", @"\mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "Uniscaler", @"\mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"\mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V3", @"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
            };
        #endregion

        #region Nvapi Results
        Dictionary<string, string> folder_nvapi = new Dictionary<string, string>
            {
                { "0.10.2h1", @"\mods\Temp\FSR2FSR3_0.10.2h1\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.3", @"\mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.4", @"\mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "Uniscaler", @"\mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"\mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
            };
        #endregion

        #region Folder Uniscaler
        Dictionary<string, string> folder_uniscaler = new Dictionary<string, string>
            {
                { "Uniscaler", @"mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V3",@"mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
            };
        #endregion

        #region Folder Uniscaler V2
        Dictionary<string, string> folder_uniscaler_v2 = new Dictionary<string, string>
        {
            {"Uniscaler V2",@"mods\\Temp\\Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml"},
            {"Uniscaler V3",@"mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Uniscaler V3
        Dictionary<string, string> folder_uniscalerV3 = new Dictionary<string, string>
        {
            {"Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Uniscaler FSR 3.1
        Dictionary<string, string> uniscaler_fsr31 = new Dictionary<string, string>
            {
                { "Uniscaler FSR 3.1", @"\mods\\Temp\\Uniscaler_FSR31\\enable_fake_gpu\\uniscaler.config.toml" },
            };
        #endregion

        #region Folder Elden Ring
        Dictionary<string, string[]> folderEldenRing = new Dictionary<string, string[]>
        {
            {"Disable Anti Cheat", new string[] {@"mods\Elden_Ring_FSR3\ToggleAntiCheat" } },
            {"Elden Ring FSR3", new string[] {@"mods\Elden_Ring_FSR3\EldenRing_FSR3" } },
            {"Elden Ring FSR3 V2", new string[] {@"mods\Elden_Ring_FSR3\EldenRing_FSR3 v2" } },
            {"Elden Ring FSR3 V3", new string[]{@"mods\Elden_Ring_FSR3\EldenRing_FSR3 v3"}},
            {"Unlock FPS Elden", new string[]{@"mods\\Elden_Ring_FSR3\\Unlock_Fps"}}
        };
        #endregion

        #region Del Elden Files
        List<string> del_elden = new List<string>
        {
            "_steam_appid.txt", "_winhttp.dll", "anti_cheat_toggler_config.ini", "anti_cheat_toggler_mod_list.txt",
            "start_game_in_offline_mode.exe", "toggle_anti_cheat.exe", "ReShade.ini", "EldenRingUpscalerPreset.ini",
            "dxgi.dll", "d3dcompiler_47.dll"
        };

        List<string> del_elden_custom = new List<string>
        {
            "ERSS2.dll", "dxgi.dll"
        };

        #endregion

        #region Folder Alan Wake 2
        Dictionary<string, string[]> folderAw2 = new Dictionary<string, string[]>
        {
            {"Alan Wake 2 FG RTX",new string [] {"mods\\FSR3_AW2\\RTX",
                "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlss.dll"}},

            {"Alan Wake 2 Uniscaler Custom",new string []{"mods\\FSR3_AW2\\AMD"}}
        };

        #endregion;

        #region Clean Aw2 Files
        List<string> del_aw2 = new List<string>
        {
            "Uniscaler.asi","winmm.dll","winmm.ini","dlssg_to_fsr3_amd_is_better.dll","DisableNvidiaSignatureChecks.reg","RestoreNvidiaSignatureChecks.reg"
        };
        #endregion

        #region Folder Ac Valhalla
        Dictionary<string, string[]> folderValhalla = new Dictionary<string, string[]>
        {
            {"AC Valhalla FSR3 All GPU",new string []{"mods\\Ac_Valhalla_DLSS2"}},
            {"Ac Valhalla Dlss (Only RTX)", new string []{ "mods\\acValhallaDlss" } }
        };
        #endregion

        #region Clean Ac Valhalla Files
        List<string> del_valhalla = new List<string>
        {
            "ReShade.ini","dxgi.dll","ACVUpscalerPreset.ini"
        };
        #endregion

        #region Folder Bdg3
        Dictionary<string, string[]> folderBdg3 = new Dictionary<string, string[]>
        {
            { "Baldur's Gate 3 FSR3", new string[] { "mods\\FSR3_BDG" } },
            { "Baldur's Gate 3 FSR3 V2", new string[] { "mods\\FSR3_BDG", "mods\\FSR3_BDG_2" } },
            { "Baldur's Gate 3 FSR3 V3", new string[] { "mods\\FSR3_BDG", "mods\\FSR3_BDG_2" } }
        };
        #endregion

        #region Folder DD2
        Dictionary<string, string[]> folderDd2 = new Dictionary<string, string[]>
        {
            { "Dinput8", new string[] { "mods\\FSR3_DD2\\dinput" } },
            { "Uniscaler_DD2", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod" } },
            { "Uniscaler + Xess + Dlss DD2", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod" } },
            { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } },
            { "Uniscaler V3", new string[]{  "mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
        };
        #endregion

        #region Clean DD2 FSR3
        List<string> del_dd2Fsr3 = new List<string>
        {
            "dinput8.dll","Uniscaler.asi","winmm.dll","winmm.ini","uniscaler.config.toml"
        };

        List<string> del_dd2_all_gpu = new List<string>
        {
        "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "DELETE_OPENVR_API_DLL_IF_YOU_WANT_TO_USE_OPENXR",
        "dinput8.dll", "DisableNvidiaSignatureChecks.reg", "DisableSignatureOverride.reg", "dlss-enabler-upscaler.dll",
        "dlss-enabler.dll", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "EnableSignatureOverride.reg",
        "libxess.dll", "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "openvr_api.dll",
        "openxr_loader.dll", "reframework_revision.txt", "RestoreNvidiaSignatureChecks.reg", "unins000.dat", "_nvngx.dll"
        };

        List<string> del_dd2_nv = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "DELETE_OPENVR_API_DLL_IF_YOU_WANT_TO_USE_OPENXR",
            "dinput8.dll", "DisableSignatureOverride.reg", "dlss-enabler-upscaler.dll", "dlss-enabler.dll",
            "dlss-enabler.log", "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll",
            "EnableSignatureOverride.reg", "libxess.dll", "nvngx-wrapper.dll", "nvngx.dll",
            "nvngx.ini", "openvr_api.dll", "openxr_loader.dll", "reframework_revision.txt",
            "unins000.dat", "unins000.exe"
        };

        #endregion

        #region Backup DD2
        List<string> bkup_dd2_all = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dinput8.dll", "dxgi.dll",
            "libxess.dll", "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.dll",
            "nvngx.ini", "openvr_api.dll", "openxr_loader.dll", "_nvngx.dll"
        };

        List<string> bkup_dd2_nv = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dinput8.dll", "dxgi.dll",
            "libxess.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "openvr_api.dll", "openxr_loader.dll",
            "unins000.dat"
        };
        #endregion

        #region Folder GTA V
        Dictionary<string, string[]> folderGtaV = new Dictionary<string, string[]>
        {
            { "Dinput8", new string[] { "mods\\FSR3_GTAV\\dinput8_gtav" } },
            { "GTA V FSR3", new string[] { "mods\\FSR3_GTAV\\GtaV_B02_FSR3" } },
            { "GTA V Online", new string []{"mods\\FSR3_GTAV\\GtaV_B02_FSR3"} },
            { "GTA V FiveM", new string []{"mods\\FSR3_GTAV\\GtaV_B02_FSR3"} },
            { "GYA V Epic", new string[] { "mods\\FSR3_GTAV\\GtaV_B02_FSR3" } },
            { "GTA V Epic V2", new string[] { "mods\\FSR3_GTAV\\Gtav_Epic" } },
        };
        #endregion 

        #region Clean RDR2 Files
        List<string> del_rdr2_custom_files = new List<string>
        {
            "ReShade.ini", "RDR2UpscalerPreset.ini", "d3dcompiler_47.dll", "d3d12.dll", "dinput8.dll",
            "ScriptHookRDR2.dll", "NVNGX_Loader.asi", "d3dcompiler_47.dll", "nvngx.dll", "winmm.ini",
            "winmm.dll", "fsr2fsr3.config.toml", "FSR2FSR3.asi", "fsr2fsr3.log"
        };

        #endregion

        #region Folder RDR2
        Dictionary<string, string[]> origins_rdr2_folder = new Dictionary<string, string[]>
        {
            {"0.9.0", new string[] {"mods\\FSR2FSR3_0.9.0\\Red Dead Redemption 2",
                                    "mods\\FSR2FSR3_0.9.0\\FSR2FSR3_COMMON"}},

            {"0.10.0", new string[] {"mods\\FSR2FSR3_0.10.0\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.0\\Red Dead Redemption 2"}},

            {"0.10.1", new string[] {"mods\\FSR2FSR3_0.10.1\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.1\\Red Dead Redemption 2"}},

            {"0.10.1h1", new string[] {"mods\\FSR2FSR3_0.10.1h1\\FSR2FSR3_COMMON",
                                       "mods\\FSR2FSR3_0.10.1h1\\Red Dead Redemption 2"}},

            {"0.10.2h1", new string[] {"mods\\FSR2FSR3_0.10.2h1\\FSR2FSR3_COMMON",
                                       "mods\\FSR2FSR3_0.10.2h1\\Red Dead Redemption 2"}},

            {"0.10.3", new string[] {"mods\\FSR2FSR3_0.10.3\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.3\\Red Dead Redemption 2"}},

            {"0.10.4", new string[] {"mods\\FSR2FSR3_0.10.4\\Red Dead Redemption 2\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.4\\Red Dead Redemption 2\\RDR2_FSR"}},

            {"Uniscaler", new string[] {"mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod"}},
            {"Uniscaler + Xess + Dlss", new string[] {"mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod"}},
            {"Uniscaler V2", new string[] {"mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod"}},
            {"Uniscaler V3", new string[]{"mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
            {"Uniscaler FSR 3.1",new string[]{"mods\\FSR2FSR3_Uniscaler_FSR3\\Uniscaler_FSR31"}}
        };

        Dictionary<string, string[]> rdr2_folder = new Dictionary<string, string[]>
        {
            { "RDR2 Build_2", new string[] { "mods\\Red_Dead_Redemption_2_Build02" } },
            { "RDR2 Build_4", new string[] { "mods\\RDR2Upscaler-FSR3Build04" } },
            { "RDR2 Mix", new string[] { "mods\\RDR2_FSR3_mix" } },
            { "RDR2 Mix 2", new string[] { "mods\\RDR2_FSR3_mix" } },
            { "Red Dead Redemption V2", new string[] { "mods\\RDR2_FSR3_V2" } },
            { "RDR2 Non Steam FSR3", new string[] { "mods\\FSR3_RDR2_Non_Steam\\RDR2_FSR3" } },
            { "RDR2 FSR 3.1 FG",new string[]{"mods\\RDR2_FSR3_1" } }
        };
        #endregion

        #region Clean Cyberpunk Files
        List<string> del_cb2077_fsr3 = new List<string>
        {
            "nvngx.dll","RestoreNvidiaSignatureChecks.reg","dlssg_to_fsr3_amd_is_better.dll","DisableNvidiaSignatureChecks.reg"
        };
        #endregion

        #region Folder Cyberpunk 2077
        Dictionary<string, string[]> folderCb2077 = new Dictionary<string, string[]>
        {
            { "RTX DLSS FG CB2077", new string[] {"mods\\FSR3_CYBER2077\\dlssg-to-fsr3-0.90_universal"}},
        };
        #endregion

        #region Folder Forza
        Dictionary<string, string[]> folderForza = new Dictionary<string, string[]>
        {
            { "RTX DLSS FG FZ5", new string[] {"mods\\FSR3_FH\\RTX"}},
            { "FSR3 FG FZ5 All GPU" , new string [] {"mods\\FSR3_FH\\Ot_Gpu"}},
        };
        #endregion

        #region Clean Forza Files
        List<string> del_fz5_files = new List<string>
        {
            "winmm.ini","winmm.dll","nvapi64.asi","dlssg_to_fsr3_amd_is_better.dll","dlssg_to_fsr3.asi","dinput8.dll"
        };
        #endregion

        #region Folder Got
        Dictionary<string, string[]> folderGot = new Dictionary<string, string[]>
        {
            { "Ghost of Tsushima FG DLSS", new string[] {"mods\\FSR3_GOT\\DLSS FG"}},
        };
        #endregion

        #region Clean Got Files
        List<string> del_got_files = new List<string>
        {
            "version.dll", "RestoreNvidiaSignatureChecks.reg", "dxgi.dll", "dlssg_to_fsr3_amd_is_better.dll", "DisableNvidiaSignatureChecks.reg", "d3d12core.dll", "d3d12.dll", "no-filmgrain.reg"

        };
        #endregion

        #region Folder Lords of The Fallen 
        Dictionary<string, string[]> folderLotf = new Dictionary<string, string[]>
        {
            { "Lords of The Fallen DLSS RTX", new string[] {"mods\\FSR3_LOTF\\RTX\\LOTF_DLLS_3_RTX"}},
            { "Lords of The Fallen FSR3 ALL GPU", new string[] {"mods\\FSR3_LOTF\\AMD_GTX"}}
        };
        #endregion

        #region Clean Lotf Files
        List<string> del_lotf_files = new List<string>
        {
            "winmm.ini","winmm.dll","Uniscaler.asi","launch.bat","DisableEasyAntiCheat.bat"

        };
        #endregion

        #region Folder Jedi 
        Dictionary<string, string[]> folderJedi = new Dictionary<string, string[]>
        {
            { "DLSS Jedi", new string[] {"mods\\FSR2FSR3_Miles\\Uni_Custom_miles"}},
        };
        #endregion

        #region Folder Palworld
        Dictionary<string, string[]> folderPw = new Dictionary<string, string[]>
        {
            { "Palworld FG Build03", new string[] {"mods\\FSR3_PW"}},
        };
        #endregion

        #region Clean Palworld Files
        List<string> del_pw_files = new List<string>
        {
            "ReShade.ini", "PalworldUpscalerPreset.ini", "d3dcompiler_47.dll", "d3d12.dll",
            "nvngx.dll"
        };
        #endregion

        #region Folder Icarus
        Dictionary<string, string[]> folderIcr = new Dictionary<string, string[]>
        {
            { "RTX DLSS FG ICR", new string[] {"mods\\FSR3_ICR\\ICARUS_DLSS_3_FOR_RTX"}},
            { "FSR3 FG ICR All GPU", new string[] {"mods\\FSR3_ICR\\ICARUS_FSR_3_FOR_AMD_GTX"}},
        };
        #endregion

        #region Clean Icarus Files
        List<string> del_icr_files = new List<string>
        {
            "winmm.ini","winmm.dll","fsr2fsr3.config.toml","FSR2FSR3.asi"

        };
        #endregion

        #region Folder Tekken
        Dictionary<string, string[]> folderTekken = new Dictionary<string, string[]>
        {
            { "Unlock FPS Tekken 8", new string[] {"mods\\Unlock_fps_Tekken"}},
        };
        #endregion

        #region Clean Dlss Global Files
        List<string> del_dlss_global_rtx = new List<string>
        {
            "dlss-enabler-upscaler.dll", "dlss-enabler.log", "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better.dll",
            "libxess.dll", "nvngx-wrapper.dll", "nvngx.ini", "unins000.dat",
            "version.dll", "dlss_rtx.txt"

        };

        List<string> del_dlss_global_amd = new List<string>
        {
            "DisableNvidiaSignatureChecks.reg", "dlss-enabler-upscaler.dll", "dlss-enabler.log", "dlss-finder.exe",
            "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "libxess.dll",
            "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.ini", "RestoreNvidiaSignatureChecks.reg",
            "unins000.dat", "unins000.exe", "winmm.dll", "_nvngx.dll", "dlss_amd.txt"

        };
        #endregion

        #region Clean RTX DLSS Files
        List<string> del_rtx_dlss = new List<string>
        {
            "nvngx.dll","RestoreNvidiaSignatureChecks.reg","dlssg_to_fsr3_amd_is_better.dll","DisableNvidiaSignatureChecks.reg"
        };
        #endregion

        #region Folder Disable Console
        Dictionary<string, string> folder_disable_console = new Dictionary<string, string>
            {
                { "0.10.3", @"mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.4", @"mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "Uniscaler", @"mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
            };
        #endregion

        #region Default Mods Files
        List<string> modCleanList = new List<string> {
            "fsr2fsr3.config.toml", "winmm.ini", "winmm.dll", "lfz.sl.dlss.dll", "FSR2FSR3.asi",
            "EnableSignatureOverride.reg", "DisableSignatureOverride.reg", "dxgi.dll",
            "fsr2fsr3.log", "nvngx.ini", "fsr2fsr3.log", "Uniscaler.asi",
            "uniscaler.config.toml", "uniscaler.log"
        };

        #endregion

        #region Clean Optiscaler Files
        List<string> del_optiscaler = new List<string>
        {
            "nvngx.ini", "nvngx.dll", "libxess.dll", "EnableSignatureOverride.reg", "DisableSignatureOverride.reg", "amd_fidelityfx_vk.dll", "amd_fidelityfx_dx12.dll"
        };
        #endregion

        #region Clean Optiscaler Custom Files
        List<string> del_optiscaler_custom = new List<string>
        {
        "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "DisableNvidiaSignatureChecks.reg", "DisableSignatureOverride.reg", "dlss-enabler-upscaler.dll", "dlss-enabler.log", "dlss-finder.exe", "dlssg_to_fsr3.ini", "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better.dll",
        "dxgi.dll", "EnableSignatureOverride.reg", "libxess.dll", "licenses", "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "RestoreNvidiaSignatureChecks.reg", "unins000.dat", "unins000.exe", "version.dll", "_nvngx.dll"
        };
        #endregion

        #region Clean Uniscaler Default
        List<string> del_uni_files = new List<string>
        {
            "Uniscaler.asi","winmm.dll","winmm.ini","uniscaler.config.toml"
        };
        #endregion

        #region Clean Ini File Folder

        Dictionary<string, string> folder_clean_ini = new Dictionary<string, string>
        {
            { "0.7.4", "mods\\FSR2FSR3_0.7.4\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.7.5", "mods\\FSR2FSR3_0.7.5_hotfix\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.7.6", "mods\\FSR2FSR3_0.7.6\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.8.0", "mods\\FSR2FSR3_0.8.0\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.9.0", "mods\\FSR2FSR3_0.9.0\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.0", "mods\\FSR2FSR3_0.10.0\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.1", "mods\\FSR2FSR3_0.10.1\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.1h1", "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.2h1", "mods\\FSR2FSR3_0.10.2h1\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.3", "mods\\FSR2FSR3_0.10.3\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.4", "mods\\FSR2FSR3_0.10.4\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "Uniscaler", "mods\\FSR2FSR3_Uniscaler\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler + Xess + Dlss", "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\enable_fake_gpu\\uniscaler.config.toml" },
            { "The Callisto Protocol FSR3", "mods\\FSR3_Callisto\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "Uniscaler V2", "mods\\FSR2FSR3_Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler V3", "mods\\FSR2FSR3_Uniscaler_V3\\enable_fake_gpu\\uniscaler.config.toml"},
            { "Uniscaler FSR 3.1","mods\\FSR2FSR3_Uniscaler_FSR3\\enable_fake_gpu\\uniscaler.config.toml"},
            { "Uni Custom Miles", "mods\\FSR2FSR3_Miles\\uni_miles_toml" }
        };

        #endregion

        #region Folder Mod Operates
        static Dictionary<string, string> folder_mod_operates = new Dictionary<string, string>()
        {
            {"0.9.0", @"\mods\Temp\FSR2FSR3_0.9.0\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.0", @"\mods\Temp\FSR2FSR3_0.10.0\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.1", @"\mods\Temp\FSR2FSR3_0.10.1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.1h1", @"\mods\Temp\FSR2FSR3_0.10.1h1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.2h1", @"\mods\Temp\FSR2FSR3_0.10.2h1\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.3", @"\mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"0.10.4", @"\mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"Uniscaler",@"\mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler + Xess + Dlss",@"\mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V2",@"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"},
            {"The Callisto Protocol FSR3",@"\mods\FSR3_Callisto\enable_fake_gpu\fsr2fsr3.config.toml"},
            {"Uni Custom Miles", @"mods\Temp\FSR3_Miles\enable_fake_gpu\uniscaler.config.toml"},
            {"Dlss Jedi", @"mods\Temp\FSR3_Miles\enable_fake_gpu\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Optiscaler
        static Dictionary<string, string> folder_optiscaler = new Dictionary<string, string>()
        {
            { "fsr22","@mods\\Temp\\OptiScaler\\nvngx.ini" }
        };
        #endregion

        //Ini/Toml Editor
        public void ConfigToml(string key, string value, Dictionary<string, string> DictionaryPath, string? section = null)
        {
            selectMod = listMods.SelectedItem as string;
            string pathToml = DictionaryPath[selectMod];
            if (DictionaryPath.ContainsKey(selectMod))
            {
                IniEditor iniEditor = new IniEditor(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + pathToml));

                iniEditor.Write(section, key, " " + value);
            }
        }
        public void ConfigIni(string key, string value, string path, string? section = null)
        {
            string pathIni = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, path);
            IniEditor iniEditor = new IniEditor(pathIni);

            iniEditor.Write(section, key, value);
        }

        public void ConfigIni2(string key, string value, string path, string? section = null)
        {
            string pathIni = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, path);
            IniEditor2.ConfigIni2(key, value, pathIni, section);
            Debug.WriteLine(pathIni);
        }


        public void ReplaceIni()
        {
            if (folder_clean_ini.ContainsKey(selectMod) && folderFakeGpu.ContainsKey(selectMod))
            {
                string path_clean_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder_clean_ini[selectMod]);
                string modified_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + folderFakeGpu[selectMod]);
                File.Copy(path_clean_ini, modified_ini, true);
            }
        }

        public static void ConfigJson(string pathJson, Dictionary<string, bool> valuesJson, string iniMessage = null)
        {
            bool varConfigJson = false;

            try
            {
                while (!varConfigJson)
                {
                    if (File.Exists(pathJson))
                    {
                        varConfigJson = true;
                    }
                    else
                    {
                        DialogResult varFolderIni = MessageBox.Show("Path not found, the path to the renderer.ini file is something like this: C:\\Users\\YourName\\AppData\\Local\\Remedy\\AlanWake2. Would you like to select the path manually?","Path Not Found",MessageBoxButtons.YesNo);

                        if (varFolderIni == DialogResult.Yes)
                        {
                            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                            {
                                if (folderDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string folderIni = folderDialog.SelectedPath;

                                    if (File.Exists(Path.Combine(folderIni, "renderer.ini")))
                                    {
                                        varConfigJson = true;
                                        pathJson = Path.Combine(folderIni, "renderer.ini");
                                    }
                                    else
                                    {
                                        MessageBox.Show("renderer.ini was not found in the folder. Please select the folder containing the renderer.ini file.","File Not Found",MessageBoxButtons.OK);
                                    }
                                }
                                else
                                {
                                    varFolderIni = MessageBox.Show("No path was selected. Would you like to try selecting the path again?","Empty Path",MessageBoxButtons.YesNo);

                                    if (varFolderIni == DialogResult.No)
                                    {
                                        MessageBox.Show("Post-processing effects were not removed", "Cancelled", MessageBoxButtons.OK);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (varConfigJson)
                {
                    string pathIni = Path.Combine(pathJson, "..","..");
                    string pathBackupIni = Path.GetFullPath(pathIni);
                    if (File.Exists(pathJson))
                    {
                        File.Copy(pathJson, pathBackupIni + "\\renderer.ini");
                    }

                    string json = File.ReadAllText(pathJson);
                    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    foreach (var entry in valuesJson)
                    {
                        if (data.ContainsKey(entry.Key))
                        {
                            data[entry.Key] = entry.Value;
                        }
                    }

                    json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(pathJson, json);

                    MessageBox.Show(iniMessage, "Success", MessageBoxButtons.OK);
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);
                MessageBox.Show("An error occurred in the Utility. Try closing and reopening it","Error",MessageBoxButtons.OK);
            }
        }

        public async Task Backup(Dictionary<string, string[]>pathModFiles = null)
        {
            try
            {

                string backupFolder = Path.Combine(selectFolder, "Backup Files");

                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                if (origins_2_2_folder.ContainsKey(selectMod) && !uniscaler_list.Contains(selectMod))
                {
                    if (File.Exists(selectFolder + "\\winmm.dll") || File.Exists(selectFolder + "\\winmm.ini"))
                    {
                        File.Copy(selectFolder + "\\winmm.dll", selectFolder + "\\Backup Files",true);
                        File.Copy(selectFolder + "\\winmm.ini", selectFolder + "\\Backup Files",true);
                    }
                }
  
                if (pathModFiles.ContainsKey(selectMod))
                {
                    string[] pathBkMod = pathModFiles[selectMod];
           
                    var originFiles = Directory.GetFiles(selectFolder);

                    foreach (var dir in pathBkMod)
                    {
                        if (Directory.Exists(dir))
                        {
                            var dcFiles = Directory.GetFiles(dir);

                            var compFiles = dcFiles.Select(Path.GetFileName).Intersect(originFiles.Select(Path.GetFileName));

                            if (!compFiles.Any())
                            {
                                MessageBox.Show("No identical files were found for backup. You can proceed with the mod installation", "No identical files",MessageBoxButtons.OK);
                                Directory.Delete(selectFolder + "\\Backup Files");
                                return;
                            }

                            foreach (var fileName in compFiles)
                            {
                                string scPath = Path.Combine(selectFolder, fileName);
                                string destPath = Path.Combine(backupFolder, fileName);

                                File.Copy(scPath, destPath, overwrite: true);
                            }
                        }
                    }
                    MessageBox.Show("Backup completed successfully.", "Sucess", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Backup could not be completed");
                    return;
                }
            }
            catch (Exception e) { }
        }

        public void LoadEditorTomlForm()
        {
            if (formEditor == null)
            {
                formEditor = new formEditorToml();
                formEditor.TopLevel = false;
                formEditor.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(formEditor);
                this.panel1.Tag = formEditor;
                formEditor.Show();
            }
            else
            {
                formEditor.BringToFront();
            }
        }

        public void DeselectAddOpt()
        {
            AddOptionsSelect.SetItemCheckState(0, CheckState.Unchecked);
        }

        public void TextBoxFpsLimit()
        {
            fpsLimitTextBox = new System.Windows.Forms.TextBox();
            fpsLimitTextBox.Location = new System.Drawing.Point(475, 100);
            fpsLimitTextBox.Size = new System.Drawing.Size(25, 25);
            panel1.Controls.Add(fpsLimitTextBox);
            fpsLimitTextBox.BringToFront();
            fpsLimitTextBox.Visible = false;

            labelFpsLimit = new System.Windows.Forms.Label();
            labelFpsLimit.Location = new System.Drawing.Point(406, 100);
            labelFpsLimit.Size = new System.Drawing.Size(125, 20);
            labelFpsLimit.Text = "Fps Limit";
            labelFpsLimit.Font = new Font("Segoe UI Semibold", 11.25f, FontStyle.Bold);
            labelFpsLimit.ForeColor = Color.White; ;
            panel1.Controls.Add(labelFpsLimit);
            labelFpsLimit.BringToFront();
            labelFpsLimit.Visible = false;

            fpsLimitTextBox.KeyPress += textBoxFps_KeyPress;
            fpsLimitTextBox.TextChanged += textBoxFps_TextChanged;
        }
        private void textBoxFps_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (fpsLimitTextBox.Text.Length >= 3 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }
        private void textBoxFps_TextChanged(object sender, EventArgs e)
        {
            fpsLimit();
        }
        public void fpsLimit()
        {
            if (fpsLimitTextBox.Text.ToString() != null)
            {
                ConfigToml("original_frame_rate_limit", fpsLimitTextBox.Text.ToString(), uniscaler_path, "general");
            }
        }
        private void AddOptionsSelect_ItemCheck(object? sender, ItemCheckEventArgs e)
        {

            int index = e.Index;

            bool CheckedOption = e.NewValue == CheckState.Checked;

            string itemText = AddOptionsSelect.Items[index].ToString();

            string AddOptSelect = CheckedOption.ToString().ToLower();

            //Toml file configuration based on the checked box (AddOptionsSelect - CheckedListBox)

            selectMod = listMods.SelectedItem as string;

            void ShowErrorMessage(string message = null)
            {
                if (message != null)
                {
                    DialogResult = MessageBox.Show(message, "Error", MessageBoxButtons.OK);
                }
                e.NewValue = CheckState.Unchecked;
            }

            void ConfigureMod(string configKey, string modVersionMessage, Dictionary<string, string> folder, string section)
            {
                if (selectMod != null)
                {
                    if (folder.ContainsKey(selectMod))
                    {
                        string pathToml = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder[selectMod]);
                        ConfigToml(configKey, AddOptSelect, folder, section);
                    }
                    else
                    {
                        ShowErrorMessage(modVersionMessage);
                    }
                }
                else
                {
                    ShowErrorMessage(modVersionMessage);
                }
            }

            if (itemText == "Toml Editor")
            {
                if (selectMod != null)
                {
                    selectMod = listMods.SelectedItem as string;
                    if (selectMod != null && folderFakeGpu.ContainsKey(selectMod))
                    {
                        string path1 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), folderFakeGpu[selectMod]);

                        ShowErrorMessage();//Uncheck the Toml Editor in AddOptionsSelect option.

                        ((mainForm)this.ParentForm).loadForm(typeof(formEditorToml), selectMod);
                    }
                }
                else
                {
                    ShowErrorMessage("Select a mod version to proceed. (excluding specific versions, for exemple: Elden Ring FSR3");
                    return;
                }
            }

            if (itemText == "Backup")
            {

                #region Backup dictionaries
                var dcGames = new Dictionary<string, Dictionary<string, string[]>>
                {
                    { "folderAw2", folderAw2 },
                    { "folderBdg3", folderBdg3 },
                    { "folderDd2", folderDd2 },
                    { "folderEldenRing", folderEldenRing },
                    { "origins_2_2_folder",origins_2_2_folder },
                    { "folderGtaV",folderGtaV },
                    { "folderGot",folderGot },
                    { "folderForza",folderForza },
                    { "rdr2_folder",rdr2_folder },
                    { "folderCb2077",folderCb2077 },
                    { "folderPw",folderPw },
                    { "folderJedi",folderJedi },
                    { "folderTekken",folderTekken },
                    { "folderIcr",folderIcr },
                    { "folderValhalla",folderValhalla }
                };
                #endregion

                foreach (var pathDc in dcGames)
                {
                    if (pathDc.Value.ContainsKey(selectMod))
                    {
                        Backup(pathDc.Value);
                        break; 
                    }
                }

            }

            if (itemText == "Fake Nvidia Gpu" && selectMod != null)
            {
                string pathToml_f_gpu = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folderFakeGpu[selectMod]);

                if (edit_fake_gpu_list.Contains(selectMod))
                {
                    ConfigToml("fake_nvidia_gpu", AddOptSelect, folderFakeGpu, "compatibility");
                }
                else if (edit_old_fake_gpu.Contains(selectMod))
                {
                    string[] iniLines = File.ReadAllLines(pathToml_f_gpu);

                    if (iniLines.Length > 0)
                    {
                        iniLines[0] = "fake_nvidia_gpu = " + AddOptSelect;

                        File.WriteAllLines(pathToml_f_gpu, iniLines);
                    }
                }
            }

            if (itemText == "Ue Compatibility Mode")
            {
                ConfigureMod("amd_unreal_engine_dlss_workaround", "Select a mod version starting from 0.9.0", folder_ue, "compatibility");
            }
            if (itemText == "Nvapi Results")
            {
                ConfigureMod("fake_nvapi_results", "Select a mod version starting from 0.10.2h1.", folder_nvapi, "compatibility");
            }
            if (itemText == "MacOS Crossover Support")
            {
                ConfigureMod("macos_crossover_support", "Select a mod version starting from 0.9.0.", folder_ue, "compatibility");
            }
            if (itemText == "Auto Exposure")
            {
                ConfigureMod("force_auto_exposure", "Select a mod version starting from Uniscaler.", folder_uniscaler, "general");
            }
            if (itemText == "Debug View")
            {
                ConfigureMod("enable_debug_view", "Select a mod version starting from 0.9.0.", folder_ue, "debug");
            }
            if (itemText == "Debug Tier Lines")
            {
                ConfigureMod("enable_debug_tear_lines", "Select a mod version starting from 0.9.0.", folder_ue, "debug");
            }
            if (itemText == "Off Frame Gen")
            {
                ConfigureMod("disable_frame_generation", "Select a mod version starting from Uniscaler V2 to use this option.", folder_uniscaler_v2, "general");
            }
            if (itemText == "Disable Console")
            {
                ConfigureMod("disable_console", "Select a mod version starting from 0.10.3.", folder_disable_console, "logging");
            }
            if (itemText == "Ignore Ingame Fg")
            {
                ConfigureMod("ignore_ingame_frame_generation_toggle", "Select Uniscaler V3 to proceed", folder_uniscalerV3, "general");
            }
            if (itemText == "Ignore Fg Resources")
            {
                ConfigureMod("ignore_ingame_frame_generation_resources", "Select a mod version starting from Uniscaler V3 to proceed", folder_uniscalerV3, "general");
            }
            if (itemText == "Disable Overlay")
            {
                ConfigureMod("disable_overlay_blockers", "Select a mod version starting from Uniscaler V2 to proceed", folder_uniscaler_v2, "general");
            }

            if (itemText == "Fps Limit")
            {
                if (selectMod != null && uniscaler_path.ContainsKey(selectMod))
                {
                    if (fpsLimitTextBox != null)
                    {
                        if (e.NewValue == CheckState.Checked)
                        {
                            fpsLimitTextBox.Visible = true;
                            labelFpsLimit.Visible = true;
                            AddOptionsSelect.Size = new Size(275, 44);
                        }
                        else if (e.NewValue == CheckState.Unchecked)
                        {
                            fpsLimitTextBox.Visible = false;
                            labelFpsLimit.Visible = false;
                            AddOptionsSelect.Size = new Size(275, 66);
                        }
                    }
                }
                else
                {
                    ShowErrorMessage("Select a Uniscaler option to proceed.");
                    return;
                }
            }
            if (itemText == "Install lfz.sl.dlss")
            {
                if (CheckedOption is true)
                {
                    varLfz = true;
                }
                else
                {
                    varLfz = false;
                }
            }
            if (itemText == "Enable Signature Over" && CheckedOption is true)
            {

                string path_en_over = @"mods\\Temp\\enable signature override\\EnableSignatureOverride.reg";

                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "regedit.exe";
                    process.StartInfo.Arguments = "/s \"" + path_en_over + "\"";
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                }
            }
            if (itemText == "Disable Signature Over" && CheckedOption is true)
            {
                string path_dis_over = @"mods\Temp\disable signature override\DisableSignatureOverride.reg";

                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "regedit.exe";
                    process.StartInfo.Arguments = "/s \"" + path_dis_over + "\"";
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void CopyToml()
        {
            string pathToml = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folderFakeGpu[selectMod]);

            string destFolder = Path.Combine(selectFolder, Path.GetFileName(pathToml));

            File.Copy(pathToml, destFolder, true);
        }

        private void AddOptionsSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void formSettings_Load(object sender, EventArgs e)
        {
            foreach (var item in pendingItems)
            {
                listMods.Items.Add(item);
            }
            pendingItems.Clear();

            buttonAddOn.Top = buttonNvngx.Top + 30;
            panelAddOn2.Top = panelNvngx.Top + 35;
            buttonAddUps.Top = buttonNvngx.Top + 30;
            panelAddOnUps.Top = panelNvngx.Top + 32;
            buttonFgMethod.Top = buttonAddOn.Top + 30;
            panelFgMethod.Top = panelAddOn.Top + 34;
        }

        public string selectFolder;
        public void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog searchFolderPath = new FolderBrowserDialog();
            searchFolderPath.RootFolder = Environment.SpecialFolder.Desktop;
            searchFolderPath.Description = "Select the folder where the game's exe is located (usually the exe ends with Win64-Shipping)";
            searchFolderPath.ShowNewFolderButton = false;

            if (searchFolderPath.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = searchFolderPath.SelectedPath;
                selectFolder = searchFolderPath.SelectedPath;
            }
        }

        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMods.SelectedItem != null)
            {
                selectMod = listMods.SelectedItem.ToString();
                SetTextModOp();
            }
            if (!folder_mod_operates.ContainsKey(selectMod))
            {
                panelModOp.Visible = false;
                panelRes.Visible = false;
            }
            if (!uniscaler_path.ContainsKey(selectMod))
            {
                panelResPreset.Visible = false;
            }

            if (selectMod == "Uniscaler + Xess + Dlss")
            {
                string[] removeOptNvngx = { "Xess 1.3", "Dlss 3.7.0", "Dlss 3.7.0 FG" };

                foreach (string nvngxOpt in removeOptNvngx)
                {
                    optionsNvngx.Items.Remove(nvngxOpt);
                }
            }
            else
            {
                string[] addOptNvngx = { "Xess 1.3", "Dlss 3.7.0", "Dlss 3.7.0 FG" };

                foreach (string addNvngx in addOptNvngx)
                {
                    if (!optionsNvngx.Items.Contains(addNvngx))
                    {
                        optionsNvngx.Items.Add(addNvngx);
                    }
                }
            }

        }

        List<string> fsr_2_2_opt = new List<string> {"A Plague Tale Requiem", "Achilles Legends Untold", "Alan Wake 2", "Assassin's Creed Mirage", "Atomic Heart", "Banishers: Ghosts of New Eden", "Blacktail", "Bright Memory: Infinite", "COD Black Ops Cold War", "Control", "Cyberpunk 2077", "Dakar Desert Rally", "Dead Island 2", "Death Stranding Director's Cut", "Dying Light 2",
            "Everspace 2", "Evil West", "F1 2022", "F1 2023", "FIST: Forged In Shadow Torch", "Fort Solis", "Hellblade 2", "Hogwarts Legacy", "Kena: Bridge of Spirits", "Lies of P", "Loopmancer", "Manor Lords", "Metro Exodus Enhanced Edition", "Monster Hunter Rise","Nobody Wants To Die", "Outpost: Infinity Siege", "Palworld", "Ready or Not", "Remnant II", "RoboCop: Rogue City",
            "Sackboy: A Big Adventure", "Satisfactory", "Shadow Warrior 3", "Smalland", "STAR WARS Jedi: Survivor", "Starfield", "Steelrising", "TEKKEN 8", "The Chant", "The Invincible", "The Medium", "Wanted: Dead"};

        List<string> fsr_2_1_opt = new List<string> { "Chernobylite", "Dead Space (2023)", "Hellblade: Senua's Sacrifice", "Hitman 3", "Horizon Zero Dawn", "Judgment", "Martha Is Dead", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Returnal", "Ripout", "Saints Row", "Uncharted Legacy of Thieves Collection" };

        List<string> fsr_2_0_opt = new List<string> { "Alone in the Dark", "Brothers: A Tale of Two Sons Remake", "Crime Boss: Rockay City", "Deathloop", "Dying Light 2", "Ghostrunner 2", "High On Life", "Jusant", "Layers of Fear", "Marvel's Guardians of the Galaxy", "Nightingale", "Rise of The Tomb Raider", "Shadow of the Tomb Raider", "The Outer Worlds: Spacer's Choice Edition", "The Witcher 3" };

        List<string> fsr_sdk_opt = new List<string> { "MOTO GP 24", "Pacific Drive", "Ratchet & Clank - Rift Apart" };

        List<string> fsr_sct_2_2 = new List<string> { "2.2" };

        List<string> fsr_sct_2_1 = new List<string> { "2.1" };

        List<string> fsr_sct_2_0 = new List<string> { "2.0" };

        List<string> fsr_sct_sdk = new List<string> { "SDK" };

        public List<string> fsr_sct_rdr2 = new List<string> { "RDR2", "Red Dead Redemption 2" };

        public static async Task CopyModsAsync(string sourcePath, string destPath)
        {
            foreach (var file in Directory.EnumerateFiles(sourcePath))
            {
                string destFile = Path.Combine(destPath, Path.GetFileName(file));
                using (var sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                using (var destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await sourceStream.CopyToAsync(destStream);
                }
            }

            foreach (var dir in Directory.EnumerateDirectories(sourcePath))
            {
                string destDir = Path.Combine(destPath, Path.GetFileName(dir));
                Directory.CreateDirectory(destDir);
                await CopyModsAsync(dir, destDir);
            }
        }

        public async Task CopyFSR(Dictionary<string, string[]> DictionaryFSR)
        {
            string path_dest = selectFolder;
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string selectedVersion = listMods.SelectedItem as string;
            string[] uniscalerVersion = { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3", "Uniscaler FSR 3.1" };

            if (selectedVersion != null)
            {
                if (DictionaryFSR.TryGetValue(selectedVersion, out string[] paths))
                {
                    foreach (string relativePath in paths)
                    {
                        string path_final = Path.GetFullPath(Path.Combine(exeDirectory, relativePath));
                        string path_fsr_initial = (path_final + "\\..\\..") + "\\FSR2FSR3_COMMON";
                        string path_fsr_common = Path.GetFullPath(path_fsr_initial);
                        if (Directory.Exists(path_final))
                        {
                            await CopyModsAsync(path_final, path_dest);

                            if (uniscalerVersion.All(uniscalerVersion => !selectedVersion.Contains(uniscalerVersion) && !rdr2_folder.ContainsKey(selectMod) && !folderEldenRing.ContainsKey(selectMod)))
                            {
                                await CopyModsAsync(path_fsr_common, path_dest);
                            }
                        }
                    }
                }
            }
        }

        public async Task CopyFolder(string pathFolder)
        {
            foreach (string files_fsr in Directory.GetFiles(pathFolder))
            {
                string fileName = Path.GetFileName(files_fsr);
                File.Copy(files_fsr, selectFolder + "\\" + fileName, true);
            }
            foreach (var subPath in Directory.GetDirectories(pathFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = subPath.Substring(pathFolder.Length + 1);
                string fullPath = Path.Combine(selectFolder, relativePath);

                Directory.CreateDirectory(fullPath);

                foreach (string filePath in Directory.GetFiles(subPath))
                {
                    string relativeFilePath = filePath.Substring(subPath.Length + 1);
                    string destFilePath = Path.Combine(fullPath, relativeFilePath);
                    File.Copy(filePath, destFilePath, true);
                }
            }
        }

        public void runReg(string pathReg)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "regedit.exe";
                process.StartInfo.Arguments = "/s \"" + pathReg + "\"";
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task AutoShortCut(string pathExe, string nameShortCut, string dx12, string nameMessageBox = null)
        {
            AutoShortcut.AShortcut(pathExe, nameShortCut, dx12, nameMessageBox);
        }

        public void fsr2_2()
        {
            string path_final;

            CopyFSR(origins_2_2_folder);
        }

        public void fsr2_1()
        {
            CopyFSR(origins_2_1_folder);
        }

        public void fsr_2_0()
        {
            CopyFSR(origins_2_0_folder);
        }

        public void fsr_sdk()
        {
            CopyFSR(origins_sdk_folder);
        }

        public void optiscaler_custom()
        {
            #region Backup Files
            try
            {
                string backupFolderOpts = Path.Combine(selectFolder, "Backup Optiscaler");

                if (!Directory.Exists(backupFolderOpts))
                {
                    Directory.CreateDirectory(backupFolderOpts);
                }

                foreach (var filesOpts in Directory.GetFiles(selectFolder))
                {
                    string filesOptsName = Path.GetFileName(filesOpts);

                    if (del_optiscaler_custom.Contains(filesOptsName))
                    {
                        string destBackupFolder = Path.Combine(backupFolderOpts, filesOptsName);
                        File.Copy(filesOpts, destBackupFolder, true);
                    }
                    else
                    {
                        Directory.Delete(selectFolder + "Backup Optiscaler");
                    }
                }

                File.Copy("mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", selectFolder + "\\nvngx.ini", true);
                runReg("mods\\Optiscaler FSR 3.1 Custom\\EnableSignatureOverride.reg");
                runReg("mods\\Optiscaler FSR 3.1 Custom\\DisableNvidiaSignatureChecks.reg");
                File.Copy("mods\\Optiscaler FSR 3.1 Custom\\nvngx.ini", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", true);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            #endregion

            CopyFolder("mods\\Optiscaler FSR 3.1 Custom");
        }

        public void dlssGlobal()
        {
            string pathRtx = "mods\\DLSS_Global\\RTX";
            string pathAmd = "mods\\DLSS_Global\\AMD";

            DialogResult gpuDlss = MessageBox.Show("Do you have a GPU starting from GTX 1660? (For other GPUs, select \"No\" (including AMD)).", "Dlss GPU", MessageBoxButtons.YesNo);

            if (gpuDlss == DialogResult.Yes)
            {
                CopyFolder(pathRtx);
            }
            else
            {
                CopyFolder(pathAmd);
            }

            runReg("mods\\FSR3_LOTF\\RTX\\LOTF_DLLS_3_RTX\\DisableNvidiaSignatureChecks.reg");
        }

        public void rdr2Fsr3()
        {
            CopyFSR(origins_rdr2_folder);
        }

        public void acValhallaDlss()
        {
            CopyFSR(folderValhalla);
        }

        public void jediFsr3()
        {
            if (selectMod == "DLSS Jedi")
            {
                CopyFSR(folderJedi);
            }
        }

        public async Task pwFSR3()
        {
            #region FSR3 Palworld
            if (selectMod == "Palworld FG Build03")
            {
                CopyFSR(folderPw);
            }

            AutoShortCut(selectFolder + "\\Palworld-Win64-Shipping.exe", "Palworld", "-dx12", "Do you want to create a DX12 shortcut? If you prefer to create it manually, click \"NO\" . This is necessary for the mod to work correctly");

            if (MessageBox.Show("Do you have an Nvidia RTX GPU?", "Select GPU", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ConfigIni2("mUpscaleType", "0", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", "Settings");
            }
            else
            {
                ConfigIni2("mUpscaleType", "3", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", "Settings");
            }
            await Task.Delay((2000));
            {
                File.Copy("mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", selectFolder + "\\mods\\PalworldUpscaler.ini", true);
                File.Copy("mods\\FSR3_PW\\mods\\PalworldUpscaler.ini", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", true);
            }
            #endregion
        }

        public async Task bdg3Fsr3()
        {
            CopyFSR(folderBdg3);

            #region Copy ini file for mods folder Baldur's Gate 3 FSR3 V3 
            if (selectMod == "Baldur's Gate 3 FSR3 V3")
            {
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string pathBdgIni = "mods\\FSR3_BDG_3\\BG3Upscaler.ini";
                string fullPath = Path.Combine(exeDirectory, pathBdgIni);
                try
                {
                    await Task.Delay((2000));
                    {
                        File.Copy(fullPath, selectFolder + "\\mods\\BG3Upscaler.ini", true);
                    }
                }
                catch { }
            }
            #endregion
        }

        public async Task dd2Fsr3()
        {
            #region CopyWinmm and delete shader.cache2
            void CopyWinmm()
            {
                if (selectMod != "FSR 3.1/DLSS DD2 NVIDIA" || selectMod != "FSR 3.1/DLSS DD2 ALL GPU")
                {
                    if (Directory.Exists(selectFolder + "\\_storage_"))
                    {
                        string pathWinmm = "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod\\winmm.dll";
                        File.Copy(pathWinmm, selectFolder + "\\_storage_\\winmm.dll", true);
                    }
                }

                if (File.Exists(selectFolder + "\\shader.cache2"))
                {
                    DialogResult varCache = MessageBox.Show("Do you want to delete the shader.cache2 file ? Not deleting this file may result in bugs and game crashes.", "Shader Cache", MessageBoxButtons.YesNo);
                    if (varCache == DialogResult.Yes)
                    {
                        File.Delete(selectFolder + "\\shader.cache2");
                    }
                }
            }
            #endregion

            if (selectMod != "Dinput8" && File.Exists(selectFolder + "\\dinput8.dll"))
            {
                CopyFSR(folderDd2);
                CopyWinmm();
            }
            else if (selectMod == "Dinput8")
            {
                CopyFSR(folderDd2);
            }
            if (selectMod != "FSR 3.1/DLSS DD2 NVIDIA" && selectMod != "FSR 3.1/DLSS DD2 ALL GPU")
            {
                if (!File.Exists(selectFolder + "\\dinput8.dll"))
                {
                    MessageBox.Show("Install \"Dinput8\" before installing the main mod", "Dinput8", MessageBoxButtons.OK);
                    return;
                }
            }

            #region FSR 3.1/DLSS DD2 NVIDIA and FSR 3.1/DLSS DD2 ALL GPU
            if (selectMod == "FSR 3.1/DLSS DD2 ALL GPU")
            {
                if (!Directory.Exists(selectFolder + "\\BackupDD2"))
                {
                    Directory.CreateDirectory(selectFolder + "\\BackupDD2");
                }

                foreach(string dd2Bkup in Directory.GetFiles(selectFolder))
                {
                    string dd2FileName = Path.GetFileName(dd2Bkup);

                    if (bkup_dd2_all.Contains(dd2FileName))
                    {
                        string fullPathDd2 = Path.Combine(selectFolder,"BackupDD2\\" + dd2FileName);

                        File.Copy(dd2Bkup, fullPathDd2, true); 
                    }
                }

                DialogResult fsr31_dd2 = MessageBox.Show("Would you like to use FSR 3.1? The game might have some graphical bugs", "FSR 3.1", MessageBoxButtons.YesNo);

                if (fsr31_dd2 == DialogResult.Yes)
                {
                    ConfigIni2("Dx12Upscaler","fsr31", "mods\\Temp\\Optiscaler_DD2\\nvngx.ini", "Upscalers");
                }
                else
                {
                    ConfigIni2("Dx12Upscaler", "xess", "mods\\Temp\\Optiscaler_DD2\\nvngx.ini", "Upscalers");
                }

                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\Optiscaler_DD2");
                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\Re_Framework");
                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\DD2_DLSS");

                File.Copy("mods\\Temp\\Optiscaler_DD2\\nvngx.ini", selectFolder + "\\nvngx.ini",true);
                File.Copy("mods\\FSR2FSR3_DD2_FSR31\\Optiscaler_DD2\\nvngx.ini", "mods\\Temp\\Optiscaler_DD2\\nvngx.ini",true);

                runReg("mods\\Temp\\enable signature override\\EnableSignatureOverride.reg");
                runReg("mods\\FSR2FSR3_DD2_FSR31\\DD2_DLSS\\DisableNvidiaSignatureChecks.reg");
            }

            if (selectMod == "FSR 3.1/DLSS DD2 NVIDIA")
            {

                if (!Directory.Exists(selectFolder + "\\BackupDD2"))
                {
                    Directory.CreateDirectory(selectFolder + "\\BackupDD2");
                }

                foreach (string dd2Bkup in Directory.GetFiles(selectFolder))
                {
                    string dd2FileName = Path.GetFileName(dd2Bkup);

                    if (bkup_dd2_nv.Contains(dd2FileName))
                    {
                        string fullPathDd2 = Path.Combine(selectFolder, "BackupDD2\\" + dd2FileName);

                        File.Copy(dd2Bkup, fullPathDd2, true);
                    }
                }

                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\Optiscaler_DD2");
                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\Re_Framework");
                CopyFolder("mods\\FSR2FSR3_DD2_FSR31\\DD2_NVIDIA");

                runReg("mods\\Temp\\enable signature override\\EnableSignatureOverride.reg");
                runReg("mods\\FSR2FSR3_DD2_FSR31\\DD2_DLSS\\DisableNvidiaSignatureChecks.reg");
            }
            #endregion
        }

        public void callistoFsr3()
        {
            string pathCallisto = "mods\\FSR3_Callisto\\FSR_Callisto";

            foreach (string filesCallisto in Directory.GetFiles(pathCallisto))
            {
                string fileName = Path.GetFileName(filesCallisto);

                File.Copy(filesCallisto, selectFolder + "\\" + fileName, true);
            }
        }

        public async Task fsrRdr2Build02()
        {

            if (selectMod == "RDR2 Non Steam FSR3")
            {
                string path_dll = "mods\\FSR3_RDR2_Non_Steam\\RDR2_DLL";
                string[] dll_files = Directory.GetFiles(path_dll);
                DialogResult var_rdr2_non_steam = MessageBox.Show("Do you want to copy the DLL files? Some users may receive a DLL error when running the game with the mod. (Only select \'Yes\' if you have received the error)", "DLL", MessageBoxButtons.YesNo);

                if (var_rdr2_non_steam == DialogResult.Yes)
                {
                    foreach (string dll_file in dll_files)
                    {
                        string dll_name = Path.GetFileName(dll_file);
                        string full_path_dll = Path.Combine(selectFolder, dll_name);
                        File.Copy(dll_file, full_path_dll, true);
                    }
                }
            }

            CopyFSR(rdr2_folder);

            try
            {
                await Task.Delay((2000));
                {
                    string path_ini = "mods\\Temp\\RDR2_FSR3\\rdr2_mix2_ini\\RDR2Upscaler.ini";

                    if (selectMod == "RDR2 Mix 2")
                    {
                        Directory.CreateDirectory(selectFolder + "\\mods");

                        File.Copy(path_ini, selectFolder + "\\mods\\RDR2Upscaler.ini", true);
                    }
                    if (selectMod == "RDR2 FSR 3.1 FG")
                    {
                        CopyFolder("mods\\Optiscaler FSR 3.1 Custom");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void eldenFsr3()
        {
            CopyFSR(folderEldenRing);

            if (selectMod == "Umlock FPS Elden")
            {
                CopyFolder("mods\\Elden_Ring_FSR3\\Unlock_Fps");
            }
        }

        public async Task motogpFsr3()
        {
            await Task.Delay((2000));
            {
                if (Directory.Exists(selectFolder + "\\uniscaler"))
                {
                    Directory.Delete(selectFolder + "\\uniscaler", true);
                }
            }
        }

        public void chernobyliteFsr3()
        {
            string pathExeCherno = selectFolder + "\\ChernobylGame-Win64-Shipping.exe";
            AutoShortCut(pathExeCherno, "Chernobylite", "-dx12", "Do you want to create a DX12 shortcut? If you prefer to create it manually, click \"NO\". This is necessary for the mod to work correctly.");
        }

        public void controlFsr3()
        {
            #region Files nvngx required for the mod to work in the game Control
            string pathNvngx = "mods\\FSR3_Control";

            foreach (string nvngxItem in Directory.GetFiles(pathNvngx))
            {
                string nvngxName = Path.GetFileName(nvngxItem);
                string fullpathNvngx = Path.Combine(selectFolder, nvngxName);

                File.Copy(pathNvngx + "\\" + nvngxName, fullpathNvngx, true);
            }
            #endregion
        }

        public void codFsr3()
        {
            MessageBox.Show("Do not use the mod in multiplayer, otherwise you may be banned. We are not responsible for any bans", "Ban", MessageBoxButtons.OK);

            dlssGlobal();
        }

        public void dl2Fsr3()
        {
            dlssGlobal();
        }
        public void tekkenFsr3()
        {
            CopyFSR(folderTekken);
        }

        public void aw2Fsr3()
        {
            string pathIniAw2 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Remedy","AlanWake2","renderer.ini"
            );

            if (folderAw2.ContainsKey(selectMod))
            {
                CopyFSR(folderAw2);

                //Disable Nvidia Signature Checks
                if (selectMod == "Alan Wake 2 FG RTX")
                {
                    string path_aw2_over = @"mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";

                    runReg(path_aw2_over);
                }
            }

            DialogResult varAw2 = MessageBox.Show("Do you want to fix possible ghosting issues caused by the FSR3 mod?", "Fix Ghosting Aw2", MessageBoxButtons.YesNo);

            if (varAw2 == DialogResult.Yes)
            {
               Dictionary<string, bool> valueRemovePos = new Dictionary<string, bool>
               {
                   { "m_bLensDistortion",false},
                   { "m_bFilmGrain",false},
                   { "m_bVignette",false}
               };

                ConfigJson(pathIniAw2,valueRemovePos, "Post-processing effects successfully removed");
            }
                
        }

        public void icrFsr3()
        {
            string pathExeIcr = selectFolder + "\\Icarus-Win64-Shipping.exe";
            if (selectMod == ("RTX DLSS FG ICR"))
            {
                runReg(@"mods\\FSR3_ICR\\ICARUS_DLSS_3_FOR_RTX\\DisableNvidiaSignatureChecks.reg");
            }
            CopyFSR(folderIcr);
            AutoShortCut(pathExeIcr, "Icarus-Win64-Shipping.exe", "-dx12", "Do you want to create a DX12 shortcut? If you prefer to create it manually, click \"NO\". This is necessary for the mod to work correctly.");
        }

        public async Task gtavFsr3()
        {
            if (selectMod != "GTA V FSR3")
            {
                CopyFSR(folderGtaV);
            }
            else if (selectMod == "Dinput8")
            {
                CopyFSR(folderGtaV);
            }
            else if (selectMod == "GTA V FSR3" && !File.Exists(selectFolder + "\\dinput8.dll"))
            {
                MessageBox.Show("Install \"Dinput8\" before installing the main mod", "Dinput8", MessageBoxButtons.OK);
                return;
            }
            else if (selectMod == "GTA V FSR3" && File.Exists(selectFolder + "\\Dinput8.dll"))
            {
                CopyFSR(folderGtaV);
            }

            if (selectMod == "GTA V FiveM")
            {
                DialogResult fivemUi = MessageBox.Show("Do you want to fix the delay in the FiveM user interface?", "UI Fix", MessageBoxButtons.YesNo);
                await Task.Delay((2000));
                if (DialogResult.Yes == fivemUi && File.Exists(selectFolder + "\\dxgi.asi"))
                {
                    File.Move(selectFolder + "\\dxgi.asi", selectFolder + "\\dxgi.dll ", true);
                }
            }

            if (selectMod == "GTA V Online")
            {
                DialogResult ban = MessageBox.Show("We are not responsible if you get banned. Do you want to proceed with the installation of the mod?", "Ban", MessageBoxButtons.YesNo);

                await Task.Delay((2000));//Delay to rename files, to avoid the possibility of renaming before the files are copied
                if (DialogResult.Yes == ban && File.Exists(selectFolder + "\\dxgi.asi"))
                {
                    File.Move(selectFolder + "\\dxgi.asi", selectFolder + "\\d3d12.dll", true);
                    File.Move(selectFolder + "\\GTAVUpscaler.asi", selectFolder + "\\GTAVUpscaler.dll", true);
                    File.Move(selectFolder + "\\GTAVUpscaler.org.asi", selectFolder + "\\GTAVUpscaler.org.dll", true);

                    File.Copy(selectFolder + "\\d3d12.dll", selectFolder + "\\mods\\d3d12.dll", true);
                    File.Copy(selectFolder + "\\GTAVUpscaler.dll", selectFolder + "\\mods\\GTAVUpscaler.dll", true);
                    File.Copy(selectFolder + "\\GTAVUpscaler.org.dll", selectFolder + "\\mods\\GTAVUpscaler.org.dll", true);
                }
            }
        }
        public async Task cyberFsr3()
        {
            CopyFSR(folderCb2077);

            string path_cb2077_over = @"mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";
            runReg(path_cb2077_over);

            DialogResult varMods = MessageBox.Show("Do you want to install Nova LUT 2-1 and HD Reworked for Cyberpunk 2077?", "Install Mods", MessageBoxButtons.YesNo);

            if (DialogResult.Yes == varMods)
            {
                string pathNovaLut = "mods\\FSR3_CYBER2077\\mods\\Nova_LUT_2-1";
                string pathHdReworked = "mods\\FSR3_CYBER2077\\mods\\Cyberpunk 2077 HD Reworked";

                CopyFolder(pathNovaLut);
                CopyFolder(pathHdReworked);
            }
        }
        public void forzaFsr3()
        {
            CopyFSR(folderForza);

            if (selectMod == "RTX DLSS FG FZ5")
            {
                string pathRegFz5 = @"mods\\FSR3_FH\RTX\\DisableNvidiaSignatureChecks.reg";
                runReg(pathRegFz5);
            }
        }
        public async Task gotFsr3()
        {
            #region Copy files using only the path as a parameter
            async Task CopyFiles(string pathFolder)
            {
                foreach (string filePath in Directory.GetFiles(pathFolder))
                {
                    string fileName = Path.GetFileName(filePath);
                    string destinationPath = Path.Combine(selectFolder, fileName);

                    try
                    {
                        File.Copy(filePath, destinationPath, true);
                    }
                    catch (IOException e)
                    {
                    }
                }
            }
            #endregion

            string regGot = "mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";
            CopyFSR(folderGot);
            runReg(regGot);

            #region Additional mods
            DialogResult dx12_got = MessageBox.Show("Do you want to install the DX12 files? These files fix issues related to DX12. (Only confirm if you have encountered a DX12 related error)", "DX12", MessageBoxButtons.YesNo);
            if (dx12_got == DialogResult.Yes)
            {
                string pathDx12Got = "mods\\FSR3_GOT\\Fix_DX12";
                CopyFiles(pathDx12Got);
            }
            try
            {
                if (MessageBox.Show("Would you like to remove the post-processing effects? (Film grain, Mouse Smoothing, etc.)", "Remove Post Processing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string postProcessingFolder = @"mods\\FSR3_GOT\\Remove_Post_Processing";
                    string pathVarPostProcessing = @"mods\\FSR3_GOT\\Remove_Post_Processing\\no-filmgrain.reg";

                    foreach (string file in Directory.GetFiles(postProcessingFolder, "*.reg"))
                    {
                        runReg(file);
                    }

                    File.Copy(pathVarPostProcessing, selectFolder + "\\no-filmgrain.reg", true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "It was not possible to remove the post-processing effects.");
                Debug.WriteLine(ex);
            }

        }
        #endregion

        public async Task mediumFsr3()
        {
            string pathMediumExe = Path.Combine(selectFolder, "Medium-Win64-Shipping.exe");
            string nameMediumShortCut = "The Medium";
            string messageMedium = "Do you want to create a DX12 shortcut? If you prefer to create it manually, click \"NO\" . This is necessary for the mod to work correctly.";

            AutoShortCut(pathMediumExe, nameMediumShortCut, "-dx12", messageMedium);
        }

        public async Task lotfFsr3()
        {

            CopyFSR(folderLotf);
            runReg(@"mods\\FSR3_LOTF\\RTX\\LOTF_DLLS_3_RTX\\DisableNvidiaSignatureChecks.reg");

            string pathLotfExe = Path.Combine(selectFolder, "launch.bat");
            string nameLotfShortCut = "launch";
            string messageLotf = "Do you want to create a shortcut for the .bat file? To make the mod work, you need to run the game through the .bat file. (If you can't run the .bat file, run the .bat file that is inside the folder where the mod was installed)";

            AutoShortCut(pathLotfExe, nameLotfShortCut, "", messageLotf);
        }

        public void CleanupMod(List<string> ListClean, Dictionary<string, string[]> DictionaryPath)
        {
            string[] DelFiles = Directory.GetFiles(selectFolder);
            try
            {
                if (DictionaryPath.ContainsKey(selectMod))
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    string[] folderModsReshade = { "Red Dead Redemption 2", "Dragons Dogma 2 ", "Palworld" };
                    if (folderModsReshade.Contains(gameSelected)) //Check to delete the 'mods'/'reshade' folder, some FSR3 mods have a 'mods'/'reshade' folder by default
                    {
                        if (Directory.Exists(selectFolder + "\\mods"))
                        {
                            Directory.Delete(selectFolder + "\\mods", true);
                        }
                        if (Directory.Exists(selectFolder + "\\reshade-shaders"))
                        {
                            Directory.Delete(selectFolder + "\\reshade-shaders", true);
                        }
                    }
                    if (Directory.Exists(selectFolder + "\\uniscaler"))
                    {
                        Directory.Delete(selectFolder + "\\uniscaler", true);
                    }

                    MessageBox.Show("Mod Successfully Removed", "Success", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please close the game or any other folders related to the game.", "Error", MessageBoxButtons.OK);
            }
        }

        public async Task InstallMethod()
        {
            string varMethod = null;

            screenMethod.TopLevel = false;
            panel1.Controls.Add(screenMethod);
            screenMethod.Location = new System.Drawing.Point(550, 150);
            screenMethod.MaximizeBox = false;
            screenMethod.MinimizeBox = false;
            screenMethod.MinimumSize = screenMethod.MaximumSize = new Size(350, 200);
            screenMethod.Text = "Optiscaler Install";
            screenMethod.FormClosing += new FormClosingEventHandler(screenMethod_FormClosing);
            screenMethod.BringToFront();
            screenMethod.Show();
            Icon screenMethodIcon = new Icon(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Method.ico"));
            screenMethod.Icon = screenMethodIcon;

            Button buttonMethod0 = new Button();
            buttonMethod0.Text = "Default (For test)";
            buttonMethod0.Location = new System.Drawing.Point(50, 10);
            buttonMethod0.Size = new Size(220, 30); ;
            buttonMethod0.Click += buttonMethod0_Click;

            Button buttonMethod1 = new Button();
            buttonMethod1.Text = "Method 1 (RTX/AMD 6000-7000)"; //Use the Default method.
            buttonMethod1.Location = new System.Drawing.Point(50, 50);
            buttonMethod1.Size = new Size(220, 30);
            buttonMethod1.Click += buttonMethod1_Click;

            Button buttonMethod2 = new Button();
            buttonMethod2.Text = "Method 2 (GTX/Old AMD)";
            buttonMethod2.Location = new System.Drawing.Point(50, 90);
            buttonMethod2.Size = new Size(220, 30);
            buttonMethod2.Click += buttonMethod2_Click;

            Button buttonMethod3 = new Button();
            buttonMethod3.Text = "Method 3 (If none of the others work)";
            buttonMethod3.Location = new System.Drawing.Point(50, 130);
            buttonMethod3.Size = new Size(220, 30);
            buttonMethod3.Click += buttonMethod3_Click;

            screenMethod.Controls.Add(buttonMethod0);
            screenMethod.Controls.Add(buttonMethod1);
            screenMethod.Controls.Add(buttonMethod2);
            screenMethod.Controls.Add(buttonMethod3);

            screenMethod.Owner = this;

            screenMethod.ShowDialog();
        }

        private void screenMethod_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                screenMethod.Visible = false;
            }
        }

        public async Task BackupOptiscaler()
        {
            string[] filesBackup = ["libxess.dll", "amd_fidelityfx_vk.dll", "nvngx.dll", "amd_fidelityfx_dx12.dll"];

            foreach (string backupFiles in filesBackup)
            {
                File.Copy(selectFolder + "\\" + backupFiles, selectFolder + "\\Backup DLL\\" + backupFiles);
            }
            runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");
        }

        private void buttonMethod0_Click(object sender, EventArgs e)
        {
            screenMethod.Visible = false;

            MessageBox.Show("Default Method successfully selected, the window has been closed automatically.", "Method Selected", MessageBoxButtons.OK);
        }

        private void buttonMethod1_Click(object sender, EventArgs e)
        {
            screenMethod.Visible = false;

            MessageBox.Show("Method 1 (RTX/RX 6000-7000) successfully selected, the window has been closed automatically.", "Method Selected", MessageBoxButtons.OK);
        }

        private void buttonMethod2_Click(object sender, EventArgs e)
        {
            File.Copy("mods\\Addons_mods\\Optiscaler dxgi\\dxgi.dll", selectFolder + "\\dxgi.dll", true);

            BackupOptiscaler();

            screenMethod.Visible = false;

            MessageBox.Show("Method 2 (GTX/Old AMD) successfully selected, the window has been closed automatically.", "Method Selected", MessageBoxButtons.OK);
        }

        private void buttonMethod3_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(selectFolder + "\\Backup DLL");
            string pathFolderBackup = selectFolder + "\\Backup DLL";
            string ranameNvngxDlss = "\\nvngx.dll";
            string renameNvngx = "\\dxgi.dll";

            try
            {
                if (File.Exists(selectFolder + "\\nvngx.dll"))
                {
                    if (File.Exists(selectFolder + "\\dxgi.dll"))
                    {
                        File.Copy(selectFolder + "\\dxgi.dll", pathFolderBackup + "\\dxgi.dll", true);
                        File.Delete(selectFolder + "\\dxgi.dll");
                    }


                    File.Copy(selectFolder + "\\nvngx.dll", pathFolderBackup + "\\nvngx.dll", true);
                    File.Move(selectFolder + "\\nvngx.dll", selectFolder + renameNvngx);

                    if (File.Exists(selectFolder + "\\nvngx_dlss.dll"))
                    {
                        File.Copy(selectFolder + "\\nvngx_dlss.dll", pathFolderBackup + "\\nvngx_dlss.dll", true);
                        File.Move(selectFolder + "\\nvngx_dlss.dll", selectFolder + ranameNvngxDlss);
                    }

                    BackupOptiscaler();
                }
                MessageBox.Show("Method 3 (If none of the others work) successfully selected, the window has been closed automatically.", "Method Selected", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying the files using method 3. Please try again or select the default method", "Error", MessageBoxButtons.OK);
                Debug.WriteLine(ex.Message);
            }
            screenMethod.Visible = false;
        }

        private void optionsAddOn_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (optionsAddOn.Items[e.Index].ToString() == "Optiscaler")
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (selectMod != "Optiscaler FSR 3.1/DLSS")
                    {
                        InstallMethod();
                    }

                }
                else
                {
                    screenMethod.Visible = false;
                }

            }
        }

        #region CleanupMod2
        public void CleanupMod2(List<string> ListClean, Dictionary<string, string> DictionaryPath, string message = null)
        {
            string[] DelFiles;
            if (selectFolder != null)
            {
                DelFiles = Directory.GetFiles(selectFolder);
            }
            else
            {
                MessageBox.Show("Select the folder where the mod is located before proceeding.", "Error", MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (DictionaryPath.ContainsKey(selectMod))
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    if (Directory.Exists(selectFolder + "\\uniscaler"))
                    {
                        Directory.Delete(selectFolder + "\\uniscaler", true);
                    }
                    if (message != null)
                    {
                        MessageBox.Show(message, "Success", MessageBoxButtons.OK);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Please close the game or any other folders related to the game.", "Error", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region Cleanup Mod 3
        public void CleanupMod3(List<string> ListClean, string modName)
        {
            string[] DelFiles = Directory.GetFiles(selectFolder);
            try
            {
                if (selectMod == modName)
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    if (Directory.Exists(selectFolder + "\\mods"))
                    {
                        Directory.Delete(selectFolder + "\\mods", true);
                    }
                    if (Directory.Exists(selectFolder + "\\reshade-shaders"))
                    {
                        Directory.Delete(selectFolder + "\\reshade-shaders", true);
                    }
                    MessageBox.Show("Mod Successfully Removed", "Success", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please close the game or any other folders related to the game.", "Error", MessageBoxButtons.OK);
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            selectMod = listMods.SelectedItem as string;
            if (selectFolder != null && selectMod != null && gameSelected != null)
            {
                if (gameSelected == "Select FSR Version" && fsrSelected == null)
                {
                    MessageBox.Show("Please fill out the first 4 options, Select Game, Select Folder, Select Mod and FSR Version (FSR Version is located to the right of Select Game).", "Error", MessageBoxButtons.OK);
                    return;
                }

                if (fsr_2_2_opt.Contains(gameSelected) || fsr_sct_2_2.Contains(fsrSelected))
                {
                    fsr2_2();
                }

                else if (fsr_2_1_opt.Contains(gameSelected) || fsr_sct_2_1.Contains(fsrSelected))
                {
                    fsr2_1();
                }

                else if (fsr_2_0_opt.Contains(gameSelected) || fsr_sct_2_0.Contains(fsrSelected))
                {
                    fsr_2_0();
                }

                else if (fsr_sdk_opt.Contains(gameSelected) || fsr_sct_sdk.Contains(fsrSelected))
                {
                    fsr_sdk();
                }

                else if ((fsr_sct_rdr2.Contains(gameSelected) && origins_rdr2_folder.ContainsKey(selectMod)) || (fsr_sct_rdr2.Contains(fsrSelected) && origins_rdr2_folder.ContainsKey(selectMod)))
                {
                    rdr2Fsr3();
                }

                else if ((fsr_sct_rdr2.Contains(gameSelected) && rdr2_folder.ContainsKey(selectMod) || fsr_sct_rdr2.Contains(fsrSelected) && rdr2_folder.ContainsKey(selectMod)))
                {
                    fsrRdr2Build02();
                }
                if (folderEldenRing.ContainsKey(selectMod) || selectMod == "Unlock FPS Elden")
                {
                    eldenFsr3();
                }
                if (gameSelected == "Alan Wake 2")
                {
                   aw2Fsr3();
                }
                if (selectMod == "Optiscaler FSR 3.1/DLSS")
                {
                    optiscaler_custom();
                }
                if (selectMod == "Ac Valhalla Dlss (Only RTX)")
                {
                    acValhallaDlss();
                }
                if (selectMod == "The Callisto Protocol FSR3")
                {
                    callistoFsr3();
                }
                if (folderBdg3.ContainsKey(selectMod))
                {
                    bdg3Fsr3();
                }
                if (gameSelected == "Cyberpunk 2077")
                {
                    cyberFsr3();
                }
                if (gameSelected == "Control")
                {
                    controlFsr3();
                }
                if (gameSelected == "Forza Horizon 5")
                {
                    forzaFsr3();
                }
                if (gameSelected == "Ghost of Tsushima")
                {
                    gotFsr3();
                }
                if (gameSelected == "The Medium")
                {
                    mediumFsr3();
                }
                if (gameSelected == "Lords of the Fallen")
                {
                    lotfFsr3();
                }
                if (selectMod == "DLSS Jedi")
                {
                    jediFsr3();
                }
                if (selectMod == "Palworld FG Build03")
                {
                    pwFSR3();
                }
                if (selectMod == "Unlock FPS Tekken 8")
                {
                    tekkenFsr3();
                    MessageBox.Show("Run TekkenOverlay.exe for the mod to work, the .exe is in the folder where the mod was installed", "Run Overlay", MessageBoxButtons.OK);
                }
                if (gameSelected == "Chernobylite")
                {
                    chernobyliteFsr3();
                }
                if (gameSelected == "Icarus")
                {
                    icrFsr3();
                }
                if (gameSelected == "MOTO GP 24")
                {
                    motogpFsr3();
                }
                if (gameSelected == "Cod MW3")
                {
                    codFsr3();
                }
                if (selectMod == "DL2 DLSS FG")
                {
                    dl2Fsr3();
                }

                if (gameSelected == "Dragons Dogma 2")
                {
                    dd2Fsr3();
                    if (selectMod != "FSR 3.1/DLSS DD2 NVIDIA" && selectMod != "FSR 3.1/DLSS DD2 ALL GPU")
                    {
                        if (!File.Exists(selectFolder + "\\dinput8.dll"))
                        {
                            return;
                        }
                    }
                }

                if (folderGtaV.ContainsKey(selectMod))
                {
                    gtavFsr3();
                    if (selectMod == "GTA V FSR3" && !File.Exists(selectFolder + "\\dinput8.dll"))
                    {
                        return;
                    }
                }

                selectMod = listMods.SelectedItem as string;

                if (selectMod != null)
                {
                    if (folderFakeGpu.ContainsKey(selectMod))
                    {
                        CopyToml();
                    }
                }
                if (varLfz is true)
                {
                    string path_lfz = "mods\\Temp\\global _lfz\\lfz.sl.dlss.dll";
                    File.Copy(path_lfz, selectFolder + "\\lfz.sl.dlss.dll", true);
                }

                if (gameSelected == "Select FSR Version" && fsrSelected != null)
                {
                    MessageBox.Show("Successful installation", "Successful", MessageBoxButtons.OK);
                }
                else if (gameSelected != "Select FSR Version")
                {
                    MessageBox.Show("Successful installation", "Successful", MessageBoxButtons.OK);
                }

                if (selectMod != null)
                {
                    foreach (string optNvngx in optionsNvngx.CheckedItems)
                    {
                        string pathNvngx;
                        if (optNvngx.Contains("Default"))
                        {
                            if (File.Exists(selectFolder + "\\nvngx.dll"))
                            {
                                try
                                {
                                    string newNameNvngx = selectFolder + "\\nvngx.txt";
                                    string oldNameNvngx = selectFolder + "\\nvngx.dll";
                                    File.Move(oldNameNvngx, newNameNvngx);
                                }
                                catch { }
                            }
                            if (selectMod == "Uniscaler FSR 3.1")
                            {
                                string pathNvngxUni = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_uni_fsr3\\nvngx.dll";
                                File.Copy(pathNvngxUni, selectFolder + "\\nvngx.dll", true);
                            }
                            else if (selectMod == "Uniscaler V3")
                            {
                                string pathNvngxV3 = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_uni_fsr3\\nvngx.dll";
                                File.Copy(pathNvngxV3, selectFolder + "\\nvngx.dll", true);
                            }
                            else
                            {
                                pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx.dll";
                                File.Copy(pathNvngx, selectFolder + "\\nvngx.dll", true);
                            }
                        }
                        if (optNvngx.Contains("NVNGX Version 1"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx.ini";
                            File.Copy(pathNvngx, selectFolder + "\\nvngx.ini", true);
                        }
                        if (optNvngx.Contains("Xess 1.3"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\libxess.dll";
                            File.Copy(pathNvngx, selectFolder + "\\libxess.dll", true);
                        }
                        if (optNvngx.Contains("Dlss 3.7.0"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlss.dll";
                            File.Copy(pathNvngx, selectFolder + "\\nvngx_dlss.dll", true);
                        }
                        if (optNvngx.Contains("Dlss 3.7.0 FG"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlssg.dll";
                            File.Copy(pathNvngx, selectFolder + "\\nvngx_dlssg.dll", true);
                        }
                    }
                    foreach (string optDxgi in optionsDxgi.CheckedItems)
                    {
                        string pathDxgi;
                        if (optDxgi == "Dxgi.dll")
                        {
                            pathDxgi = "mods\\Temp\\dxgi_global\\dxgi.dll";
                            File.Copy(pathDxgi, selectFolder + "\\dxgi.dll", true);
                        }
                        if (optDxgi == "D3D12.dll")
                        {
                            pathDxgi = "mods\\Temp\\dxgi_global\\d3d12.dll";
                            File.Copy(pathDxgi, selectFolder + "\\d3d12.dll", true);
                        }
                    }
                    foreach (string optAddOn in optionsAddOn.CheckedItems)
                    {
                        string pathAddOn;
                        if (optAddOn == "Optiscaler" && selectMod != "Optiscaler FSR 3.1/DLSS")
                        {

                            pathAddOn = "mods\\Addons_mods\\OptiScaler";
                            string[] fileOptiscaler = Directory.GetFiles(pathAddOn);

                            foreach (string optFile in fileOptiscaler)
                            {
                                string nameOptiscaler = Path.GetFileName(optFile);
                                string fullPath = Path.Combine(selectFolder, nameOptiscaler);
                                string pathIni = "mods\\Temp\\OptiScaler\\nvngx.ini";
                                File.Copy(optFile, fullPath, true);
                                File.Copy(pathIni, selectFolder + "\\nvngx.ini", true);
                            }
                            string oldIni = "mods\\Temp\\OptiScaler\\nvngx.ini";
                            string newIni = "mods\\Addons_mods\\OptiScaler\\nvngx.ini";
                            File.Copy(newIni, oldIni, true);
                        }
                        if (optAddOn == "Tweak")
                        {
                            pathAddOn = "mods\\Addons_mods\\tweak";
                            string[] filesTweak = Directory.GetFiles(pathAddOn);
                            foreach (string fileTweak in filesTweak)
                            {
                                string fileName = Path.GetFileName(fileTweak);
                                string fullPath = Path.Combine(selectFolder, fileName);
                                File.Copy(fileTweak, fullPath, true);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a version of the mod before proceeding.", "Successful", MessageBoxButtons.OK);
                    return;
                }
                ReplaceIni();
            }
            else
            {
                MessageBox.Show("Please fill out the first 3 options, Select Game, Select Folder, and Mod Options.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        public void CleanDlssGlobal(string modName)
        {
            if (File.Exists(selectFolder + "\\dlss_rtx.txt"))
            {
                CleanupMod3(del_dlss_global_rtx, modName);
            }
            else if (File.Exists(selectFolder + "\\dlss_amd.txt"))
            {
                CleanupMod3(del_dlss_global_amd, modName);
            }

            runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");
        }
        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (selectMod != null)
            {
                try
                {
                    if (File.Exists(selectFolder + "\\nvngx.txt"))
                    {
                        DialogResult var_nvngx = MessageBox.Show("A backup of the file nvngx.dll has been found. Do you want to restore the backup?", "Nvngx.dll", MessageBoxButtons.YesNo);

                        if (var_nvngx == DialogResult.Yes)
                        {
                            string oldNvngx = selectFolder + "\\nvngx.txt";
                            string newNvngx = selectFolder + "\\nvngx.dll";
                            File.Delete(newNvngx);
                            File.Move(oldNvngx, newNvngx);
                        }
                    }
                }
                catch { }

                if (folderFakeGpu.ContainsKey(selectMod))
                {
                    if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
                    {
                        CleanupMod2(modCleanList, folderFakeGpu);
                    }
                    else
                    {
                        CleanupMod2(modCleanList, folderFakeGpu, "Mod Successfully Removed");
                    }
                }
                
                if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
                {
                    #region  Clean Optiscaler
                    string pathBackupFolder = selectFolder + "\\Backup DLL";

                    if (File.Exists(selectFolder + "\\amd_fidelityfx_vk.dll"))
                    {
                        foreach (var fileBackup in Directory.GetFiles(pathBackupFolder))
                        {
                            string fileName = Path.GetFileName(fileBackup);
                            File.Copy(fileBackup, Path.Combine(selectFolder, fileName), true);
                        }
                        Directory.Delete(pathBackupFolder, true);
                    }
                    runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");
                    #endregion

                    CleanupMod2(del_optiscaler, folderFakeGpu, "Mod Successfully Removed");
                }
                if (selectMod == "Optiscaler FSR 3.1/DLSS")
                {
                    CleanupMod3(del_optiscaler_custom, "Optiscaler FSR 3.1/DLSS");
                    runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");

                    #region Restore Files
                    string backupOptiscalerFolder = selectFolder + "\\Backup Optiscaler";
                    if (Directory.Exists(backupOptiscalerFolder))
                    {
                        foreach (string filesBackup in Directory.GetFiles(backupOptiscalerFolder))
                        {
                            string fileBackupName = Path.GetFileName(filesBackup);
                            string restoreFilesPath = Path.Combine(selectFolder, fileBackupName);
                            File.Copy(filesBackup, restoreFilesPath, true);
                        }

                        Directory.Delete(backupOptiscalerFolder, true);
                    }
                    #endregion
                }
                if (gameSelected == "Cyberpunk 2077")
                {
                    #region Remove Files Cyberpunk
                    if (selectMod == "RTX DLSS FG CB2077")
                    {
                        CleanupMod(del_cb2077_fsr3, folderCb2077);
                    }
                    //Remove the folder with HD Rework and Nova Lut
                    if (Directory.Exists(selectFolder + "\\archive"))
                    {
                        Directory.Delete(selectFolder + "\\archive", true);
                    }
                    #endregion
                }
                else if (rdr2_folder.ContainsKey(selectMod))
                {
                    CleanupMod(del_rdr2_custom_files, rdr2_folder);
                }
                else if (gameSelected == "Alan Wake 2")
                {
                    if (folderAw2.ContainsKey(selectMod))
                    {
                    CleanupMod(del_aw2, folderAw2);
                    #region RestoreNvidiaSignatureChecks
                    if (selectMod == "Alan Wake 2 FG RTX")
                    {
                        string path_aw2_en = @"mods\\FSR3_GOT\\DLSS FG\\RestoreNvidiaSignatureChecks.reg";
                        runReg(path_aw2_en);
                    }
                    #endregion
                    }

                    #region Restore Pos-Processing
                    string modifiedIniAw2 = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Remedy", "AlanWake2", "renderer.ini"
                    );

                    string pathReplaceInAw2 = Path.Combine(modifiedIniAw2, "..", "..");

                    if (File.Exists(Path.GetFullPath(pathReplaceInAw2 + "\\renderer.ini")))
                    {
                        DialogResult restoreini = MessageBox.Show("Do you want to restore post-processing effects?", "Restore", MessageBoxButtons.YesNo);

                        if (restoreini == DialogResult.Yes)
                        {
                            File.Copy(pathReplaceInAw2 + "\\renderer.ini", modifiedIniAw2, true);

                            MessageBox.Show("Post-processing effects successfully restored", "Sucess", MessageBoxButtons.OK);
                        }
                    }
                    #endregion
                }
                else if (selectMod == "Ac Valhalla Dlss (Only RTX)")
                {
                    CleanupMod3(del_valhalla, "Ac Valhalla Dlss (Only RTX)");
                }
                else if (gameSelected == "Dragons Dogma 2")
                {
                    if (folderDd2.ContainsKey(selectMod))
                    {
                        CleanupMod(del_dd2Fsr3, folderDd2);
                    }
                    #region FSR 3.1/DLSS DD2 NVIDIA and FSR 3.1/DLSS DD2 ALL GPU
                    else if (selectMod == "FSR 3.1/DLSS DD2 ALL GPU")
                    {
                        CleanupMod3(del_dd2_all_gpu, "FSR 3.1/DLSS DD2 ALL GPU");
                        Directory.Delete(selectFolder + "\\reframework",true);

                        if (Directory.Exists(selectFolder + "\\BackupDD2"))
                        {
                            DialogResult backupDd2 = MessageBox.Show("A backup of the original game files was found. Do you want to restore them after deleting the mod? (This is highly recommended)", "Backup DD2", MessageBoxButtons.YesNo);

                            if(backupDd2 == DialogResult.Yes)
                            {
                                foreach(string originFilesDD2 in Directory.GetFiles(selectFolder + "\\BackupDD2"))
                                {
                                    string originFileNameDD2 = Path.GetFileName(originFilesDD2);

                                    if (bkup_dd2_all.Contains(originFileNameDD2))
                                    {
                                       
                                        string destFileDD2 = Path.Combine(selectFolder, originFileNameDD2);

                                        File.Copy(originFilesDD2, destFileDD2, true);                                     
                                    }
                                }
                                Directory.Delete(selectFolder + "\\BackupDD2",true);
                                MessageBox.Show("Files restored successfully", "Sucess", MessageBoxButtons.OK);
                            }
                        }
                    }
                    else if (selectMod == "FSR 3.1/DLSS DD2 NVIDIA")
                    {
                        CleanupMod3(del_dd2_nv, "FSR 3.1/DLSS DD2 NVIDIA");
                        Directory.Delete(selectFolder + "\\reframework",true);

                        if (Directory.Exists(selectFolder + "\\BackupDD2"))
                        {
                            DialogResult backupDd2 = MessageBox.Show("A backup of the original game files was found. Do you want to restore them after deleting the mod? (This is highly recommended)", "Backup DD2", MessageBoxButtons.YesNo);

                            if (backupDd2 == DialogResult.Yes)
                            {
                                foreach (string originFilesDD2 in Directory.GetFiles(selectFolder + "\\BackupDD2"))
                                {
                                    string originFileNameDD2 = Path.GetFileName(originFilesDD2);

                                    if (bkup_dd2_nv.Contains(originFileNameDD2))
                                    {

                                        string destFileDD2 = Path.Combine(selectFolder, originFileNameDD2);

                                        File.Copy(originFilesDD2, destFileDD2, true);
                                    }
                                }
                                Directory.Delete(selectFolder + "\\BackupDD2",true);
                                MessageBox.Show("Files restored successfully", "Sucess", MessageBoxButtons.OK);
                            }
                        }
                    }
                    #endregion
                }
                else if (folderPw.ContainsKey(selectMod))
                {
                    CleanupMod(del_pw_files, folderPw);
                }
                else if (folderJedi.ContainsKey(selectMod))
                {
                    CleanupMod(del_uni_files, folderJedi);
                }
                else if (folderGot.ContainsKey(selectMod))
                {
                    #region Addition Mods
                    if (File.Exists(selectFolder + "\\version.dll"))
                    {
                        File.Delete(selectFolder + "\\version.dll");
                    }
                    if (File.Exists(selectFolder + "\\d3d12core.dll"))
                    {
                        File.Delete(selectFolder + "\\d3d12core.dll");
                        File.Delete(selectFolder + "\\d3d12.dll");
                    }

                    if (MessageBox.Show("Would you like to restore the post-processing effects? (Film grain, Mouse Smoothing, etc.)", "Restore Post Processing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (File.Exists(selectFolder + "\\no-filmgrain.reg"))
                        {
                            File.Delete(selectFolder + "\\no-filmgrain.reg");

                            try
                            {
                                string postProcessingFolder = @"mods\\FSR3_GOT\\Remove_Post_Processing\\restore"; ;

                                foreach (string file in Directory.GetFiles(postProcessingFolder, "*.reg"))
                                {
                                    runReg(file);
                                }
                            }
                            catch { }
                        }
                    }
                    CleanupMod(del_got_files, folderGot);
                    #endregion
                }
                else if (folderEldenRing.ContainsKey(selectMod) || selectMod == "Unlock FPS Elden")
                {
                    #region Del Mods Elden Ring
                    if (File.Exists(selectFolder + "\\UnlockFps.txt"))
                    {
                        DialogResult delUlcFps = MessageBox.Show("Do you want to remove the Unlock FPS mod?", "Unlock FPS Elden", MessageBoxButtons.YesNo);

                        if (delUlcFps == DialogResult.Yes)
                        {
                            File.Delete(selectFolder + "\\UnlockFps.txt");
                            File.Delete(selectFolder + "\\mods\\UnlockTheFps.dll");
                            Directory.Delete(selectFolder + "\\mods\\UnlockTheFps",true);
                            MessageBox.Show("Mod Successfully Removed", "Success", MessageBoxButtons.OK);
                        }
                    }
                    else if (selectMod == "Elden Ring FSR3 V3")
                    {
                        CleanupMod(del_elden_custom, folderEldenRing);
                        Directory.Delete(selectFolder + "\\ERSS2",true);
                    }
                    else
                    {
                        CleanupMod(del_elden, folderEldenRing);
                        Directory.Delete(selectFolder + "\\reshade-shaders", true);
                    }
                    #endregion
                }
                else if (folderIcr.ContainsKey(selectMod))
                {
                    if (selectMod == "RTX DLSS FG ICR")
                    {
                        CleanupMod(del_rtx_dlss, folderIcr);
                    }
                    else
                    {
                        CleanupMod(del_icr_files, folderIcr);
                    }
                }
                else if (folderForza.ContainsKey(selectMod))
                {
                    if (selectMod == "RTX DLSS FG FZ5")
                    {
                        CleanupMod(del_rtx_dlss, folderForza);
                    }
                    else
                    {
                        CleanupMod(del_fz5_files, folderForza);
                    }
                }
                else if (selectMod == "COD MW3 FSR3")
                {
                    CleanDlssGlobal("COD MW3 FSR3");
                }
                else if (selectMod == "DL2 DLSS FG")
                {
                    CleanDlssGlobal("DL2 DLSS FG");
                }

                else if (folderLotf.ContainsKey(selectMod))
                {
                    #region Clean Mods Lotf
                    if (selectMod == "Lords of The Fallen DLSS RTX")
                    {
                        CleanupMod(del_rtx_dlss, folderLotf);
                        if (File.Exists(selectFolder + "\\launch.bat"))
                        {
                            File.Delete(selectFolder + "\\launch.bat");
                            File.Delete(selectFolder + "\\DisableEasyAntiCheat.bat");
                            File.Delete(selectFolder + "\\version.dll");
                        }
                    }
                    else
                    {
                        if (Directory.Exists(selectFolder + "\\uniscaler"))
                        {
                            Directory.Delete(selectFolder + "\\uniscaler", true);
                        }
                        CleanupMod(del_lotf_files, folderLotf);
                    }
                    #endregion
                }

                if (Directory.Exists(selectFolder + "\\Backup Files"))
                {
                    DialogResult restoreOriginFiles = MessageBox.Show("A backup folder with the original game files was found. Do you want to restore these files? (This is highly recommended)", "Restore Files", MessageBoxButtons.YesNo);

                    if (restoreOriginFiles == DialogResult.Yes)
                    {
                        foreach (string filesRestore in Directory.GetFiles(selectFolder + "\\Backup Files"))
                        {
                            string nameFileRestore = Path.GetFileName(filesRestore);

                            string destFilesRestore = Path.Combine(selectFolder, nameFileRestore);

                            File.Copy(filesRestore, destFilesRestore, true);
                        }
                    }

                    Directory.Delete(selectFolder + "\\Backup Files");

                    MessageBox.Show("The files have been successfully restored", "Sucess", MessageBoxButtons.OK);
                }

                if (selectMod == null && selectFolder == null)
                {
                    MessageBox.Show("Please fill out the first 3 options, Select Game, Select Folder, and Mod Options.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Select a version of the mod before proceeding.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        //Config Resolution/Mod Operates
        #region Unlock Mod Operates
        List<string> unlock_mod_operates_list = new List<string> { "0.10.0", "0.10.1", "0.10.1h1", "0.10.2h1", "0.10.3", "0.10.4" };
        List<string> uniscaler_list = new List<string> { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3", "Uniscaler FSR 3.1", "Uni Custom Miles", "Dlss Jedi" };
        #endregion

        #region UniResolutionCustom
        static Dictionary<string, Dictionary<string, string>> uni_res_custom = new Dictionary<string, Dictionary<string, string>>
        {
            { "1080p", new Dictionary<string, string>
                {
                    { "balanced", "0.666667" },
                    { "quality", "0.886" }
                }
            },
            { "1440p", new Dictionary<string, string>
                {
                    { "performance", "0.50" },
                    { "balanced", "0.666667" },
                    { "quality", "0.75" }
                }
            },
            { "2160p", new Dictionary<string, string>
                {
                    { "ultra_performance", "0.33" },
                    { "performance", "0.44" },
                    { "balanced", "0.50" },
                    { "quality", "0.666667" }
                }
            }
        };

        #endregion

        #region Uniscaler Path
        Dictionary<string, string> uniscaler_path = new Dictionary<string, string>
            {
                { "Uniscaler", @"\mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"\mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V3", @"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"mods\\Temp\\Uniscaler_FSR31\\enable_fake_gpu\\uniscaler.config.toml"}
            };
        #endregion

        public void SubMenuClose()
        {
            panelModOp.Visible = false;
            panelRes.Visible = false;
            panelResPreset.Visible = false;
            panelNvngx.Visible = false;
            panelDxgi.Visible = false;
            panelAddOn2.Visible = false;
            panelAddOnUps.Visible = false;
            panelFgMethod.Visible = false;
        }

        public void HideSubMenu()
        {
            if (panelModOp.Visible == true)
            {
                panelModOp.Visible = false;
            }
            if (panelRes.Visible == true)
            {
                panelRes.Visible = false;
            }
            if (panelResPreset.Visible == true)
            {
                panelResPreset.Visible = false;
            }
            if (panelNvngx.Visible == true)
            {
                panelNvngx.Visible = false;
            }
            if (panelDxgi.Visible == true)
            {
                panelDxgi.Visible = false;
            }
            if (panelAddOn2.Visible == true)
            {
                panelAddOn2.Visible = false;
            }
            if (panelAddOnUps.Visible == true)
            {
                panelAddOnUps.Visible = false;
            }
            if (panelFgMethod.Visible == true)
            {
                panelFgMethod.Visible = false;
            }
        }

        public void ShowSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        public void WriteUniCustomRes(string selectedResolution)
        {
            if (uniscaler_path.ContainsKey(selectMod))
            {
                if (uni_res_custom.ContainsKey(selectedResolution))
                {
                    var resolutionSettings = uni_res_custom[selectedResolution];

                    foreach (var sett in resolutionSettings)
                    {
                        string key = sett.Key;
                        string value = sett.Value;

                        string tomlKey;
                        switch (key)
                        {
                            case "balanced":
                                tomlKey = "balanced";
                                break;
                            case "quality":
                                tomlKey = "quality";
                                break;
                            case "performance":
                                tomlKey = "performance";
                                break;
                            case "ultra_performance":
                                tomlKey = "ultra_performance";
                                break;
                            case "ultra_quality":
                                tomlKey = "ultra_quality";
                                break;
                            default:
                                continue;
                        }
                        ConfigToml(tomlKey, value, uniscaler_path, "resolution_override");
                    }
                }
            }
        }

        public void SetTextModOp()
        {
            if (unlock_mod_operates_list.Contains(selectMod))
            {
                modOpt1.Text = "Default";
                modOpt2.Text = "Enable Upscaling Only";
                modOpt3.Text = "Use Game Upscaling";
                modOpt4.Text = "Replace dlss fg";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt4.Visible = true;
                modOpt5.Visible = false;
            }
            else if (selectMod == "0.9.0")
            {
                modOpt1.Text = "Enable Upscaling Only";
                modOpt2.Visible = false;
                modOpt3.Visible = false;
                modOpt4.Visible = false;
                modOpt5.Visible = false;
            }
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler FSR 3.1")
            {
                modOpt1.Text = "FSR3";
                modOpt2.Text = "DLSS";
                modOpt3.Text = "XESS";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt4.Visible = false;
                modOpt5.Visible = false;
            }
            else if (selectMod == "Uniscaler V3")
            {
                modOpt1.Text = "None";
                modOpt2.Text = "FSR3";
                modOpt3.Text = "DLSS";
                modOpt4.Text = "XESS";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt5.Visible = false;
            }
            else if (selectMod == "Uniscaler FSR 3.1")
            {
                modOpt1.Text = "None";
                modOpt2.Text = "FSR3";
                modOpt3.Text = "DLSS";
                modOpt4.Text = "XESS";
                modOpt5.Text = "FSR 3.1";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt4.Visible = true;
                modOpt5.Visible = true;
            }
            this.Invalidate();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            selectMod = listMods.SelectedItem as string;
            if (selectMod != null && folder_mod_operates.ContainsKey(selectMod))
            {
                SetTextModOp();
                ShowSubMenu(panelModOp);
            }
            else
            {
                MessageBox.Show("Select a mod starting from 0.9.0 to use this option", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        //The sequence of options for the Uniscaler V3 mod replaces each other within the ConfigToml method in the last else if. button3_Click: fsr3 = "none" | button4_Click: dlss = "fsr3" | button5_Click: xess = "dlss" | button6_Click: "xess"
        bool var_modop = false;

        private void button3_Click(object sender, EventArgs e)
        {
            selectMod = listMods.SelectedItem as string;
            if (var_modop == false)
            {
                var_modop = true;
            }
            else
            {
                var_modop = false;
            }

            if (selectMod == "0.9.0")
            {
                ConfigToml("enable_upscaling_only", var_modop.ToString().ToLower(), folder_mod_operates, "general");
            }

            else if (unlock_mod_operates_list.Contains(selectMod))
            {
                ConfigToml("mode", "\"default\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"fsr3\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"none\"", folder_mod_operates, "general");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (unlock_mod_operates_list.Contains(selectMod))
            {
                ConfigToml("mode", "\"enable_upscaling_only\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"dlss\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"fsr3\"", folder_mod_operates, "general");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (unlock_mod_operates_list.Contains(selectMod))
            {
                ConfigToml("mode", "\"use_game_upscaling\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"xess\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"dlss\"", folder_mod_operates, "general");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (unlock_mod_operates_list.Contains(selectMod))
            {
                ConfigToml("mode", "\"replace_dlss_fg\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"xess\"", folder_mod_operates, "general");
            }
        }

        private void modOpt5_Click(object sender, EventArgs e)
        {
            if (selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"fsr3_1\"", folder_mod_operates, "general");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void formSettings_Resize(object sender, EventArgs e)
        {
            if (label5 != null)
            {
                int newLabel5Left = label4.Left + label4.Width + 16;

                if (newLabel5Left + label5.Width <= this.ClientSize.Width)
                {
                    label5.Top = label4.Top;
                    label5.Left = newLabel5Left;
                    mainPanelUpsRes.Top = label5.Top + 58;
                    mainPanelUpsRes.Left = newLabel5Left;
                    label6.Top = label3.Top + label3.Height + 70;
                    label6.Left = label3.Left;
                    panelAddOn.Top = label3.Top + label3.Height + 130;
                    panelAddOn.Left = label3.Left;
                    panel1.Location = new Point(10, 10);
                    panel1.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 20);
                }
                else
                {
                    label5.Top = label3.Top + label3.Height + 70;
                    label5.Left = label3.Left;
                    mainPanelUpsRes.Top = label3.Top + label3.Height + 130;
                    mainPanelUpsRes.Left = label3.Left;
                    label6.Top = label5.Top + label5.Height - 52;
                    label6.Left = label5.Left + 511;
                    panelAddOn.Top = label3.Top + label3.Height + 130;
                    panelAddOn.Left = label5.Left + 511;
                }
                int newLabel6Left = label5.Left + label5.Width + 16;

                if (newLabel6Left + label6.Width <= this.ClientSize.Width)
                {
                    label6.Top = label5.Top;
                    label6.Left = newLabel6Left;
                    panelAddOn.Top = label6.Top + 58;
                    panelAddOn.Left = newLabel6Left;
                }
                panelAddOn.SendToBack();
                mainPanelUpsRes.SendToBack();
                label3.SendToBack();
                label4.SendToBack();
                label5.SendToBack();
                flowLayoutPanel3.SendToBack();
                buttonInstall.BringToFront();
                buttonDel.BringToFront();
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod))
            {
                ShowSubMenu(panelRes);
            }
            else
            {
                MessageBox.Show("Select a mod starting from 0.9.0 to use this option", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueUltraQ != null)
            {
                decimal valueConvUltraQ = valueUltraQ.Value / 100;
                ConfigToml("ultra_quality", valueConvUltraQ.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }
        private void buttonQ_Click(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueQ != null)
            {
                decimal valueConvQ = valueQ.Value / 100;
                ConfigToml("quality", valueConvQ.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }
        private void buttonBalanced_Click(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueBalanced != null)
            {
                decimal valueConvBalanced = valueBalanced.Value / 100;
                ConfigToml("balanced", valueConvBalanced.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonPerf_Click(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valuePerf != null)
            {
                decimal valueConvPerf = valuePerf.Value / 100;
                ConfigToml("performance", valueConvPerf.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonUltraP_Click(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueUltraP != null)
            {
                decimal valueConvUltraP = valueUltraP.Value / 100;
                ConfigToml("ultra_performance", valueConvUltraP.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonNative_Click(object sender, EventArgs e)
        {
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueNative != null)
            {
                decimal valueConvNat = valueNative.Value / 100;
                ConfigToml("native", valueConvNat.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            decimal valueConvSharpOver = valueSharpOver.Value;

            if (valueSharpOver.Value == -1.0m)
            {
                valueConvSharpOver = -1.0m;
            }
            else
            {
                valueConvSharpOver = valueSharpOver.Value / 10;
            }
            if (selectMod != null && folder_ue.ContainsKey(selectMod) && valueSharpOver != null)
            {
                ConfigToml("sharpness_override", valueConvSharpOver.ToString(CultureInfo.InvariantCulture), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonResPreset_Click(object sender, EventArgs e)
        {
            if (selectMod != null && uniscaler_path.ContainsKey(selectMod))
            {
                ShowSubMenu(panelResPreset);
            }
            else
            {
                MessageBox.Show("Select a mod starting from Uniscaler to use this option", "Error", MessageBoxButtons.OK);
                return;
            }
        }
        private void button1080_Click(object sender, EventArgs e)
        {
            WriteUniCustomRes("1080p");
        }
        private void button1440_Click(object sender, EventArgs e)
        {
            WriteUniCustomRes("1440p");
        }

        private void button2160_Click(object sender, EventArgs e)
        {
            WriteUniCustomRes("2160p");
        }

        private void optionsAddOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!optionsAddOn.CheckedItems.Contains("Optiscaler") && panelAddOnUps.Visible == true)
            {
                panelAddOnUps.Visible = false;
            }
            if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                InstallMethod();
            }
        }

        private void buttonNvngx_Click(object sender, EventArgs e)
        {
            if (panelNvngx.Visible == true)
            {
                buttonAddOn.Top = panelNvngx.Top + 3;
                panelAddOn2.Top = buttonAddOn.Top + 30;

                buttonFgMethod.Top = panelAddOn2.Top;
                panelFgMethod.Top = buttonAddOn.Top + 30;
            }
            else
            {
                buttonAddOn.Top = panelNvngx.Top + 72;
                panelAddOn2.Top = buttonAddOn.Top + 28;

                buttonFgMethod.Top = panelAddOn2.Top + 3;
                panelFgMethod.Top = buttonAddOn.Top;
            }

            if (panelAddOn2.Visible == true)
            {
                buttonFgMethod.Top = panelAddOn2.Top + 48;
                panelFgMethod.Top = buttonAddOn.Top + 101;
            }
            else
            {
                panelFgMethod.Top = buttonAddOn.Top + 58;
            }
            ShowSubMenu(panelNvngx);
        }
        private void buttonFgMethod_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelFgMethod);
        }

        private void ShowSelectedNvngx(object sender, EventArgs e)
        {
            string[] optNvngx = { "Xess 1.3", "Dlss 3.7.0", "Dlss  3.7.0 FG" };
            foreach (string opt in optNvngx)
            {

            }

        }
        private void optionsNvngx_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = e.Index;

            bool CheckedNvngx = e.NewValue == CheckState.Checked;

            string ItemNvngx = optionsNvngx.Items[index].ToString();

            string SelectedNvngx = CheckedNvngx.ToString().ToLower();

        }
        private void buttonDxgi_Click_1(object sender, EventArgs e)
        {
            if (panelDxgi.Visible == true)
            {
                buttonAddUps.Top = panelDxgi.Top;
                panelAddOnUps.Top = buttonAddUps.Top + 28;
            }
            else
            {
                buttonAddUps.Top = panelDxgi.Top + 49;
                panelAddOnUps.Top = buttonAddUps.Top + 28;
            }
            ShowSubMenu(panelDxgi);
        }
        private void buttonAddOn_Click(object sender, EventArgs e)
        {
            if (panelAddOn2.Visible == true && panelNvngx.Visible == true)
            {
                buttonFgMethod.Top = panelAddOn2.Top + 3;
                panelFgMethod.Top = buttonAddOn.Top + 57;
            }
            else if (panelAddOn2.Visible == true)
            {
                buttonFgMethod.Top = panelAddOn2.Top;
                panelFgMethod.Top = buttonAddOn.Top + 57;
            }
            else
            {
                buttonFgMethod.Top = panelAddOn2.Top + 45;
                panelFgMethod.Top = buttonAddOn.Top + 104;
            }
            ShowSubMenu(panelAddOn2);
        }
        private void buttonAddUps_Click(object sender, EventArgs e)
        {
            if (optionsAddOn.CheckedItems.Contains("Optiscaler") || selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ShowSubMenu(panelAddOnUps);
            }
            else
            {
                MessageBox.Show("Check the \"Optiscaler\" box in \"Add-On Mods\" to select this option.", "Optiscaler", MessageBoxButtons.OK);
                return;
            }
        }

        private void buttonAddUps1_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "fsr22", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonAddUps2_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "fsr22_12", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "fsr22_12", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonAddUps3_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "fsr21_12", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "fsr21_12", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonAddUps4_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "xess", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "xess", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "fsr31_12 ", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "fsr31_12 ", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonDlssDX11_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx11Upscaler", "dlss", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx11Upscaler", "dlss ", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonAddUps5_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx12Upscaler", "fsr22", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx12Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx12Upscaler", "fsr31", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx12Upscaler", "fsr31", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }
        private void buttonAddUps6_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx12Upscaler", "fsr21", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx12Upscaler", "fsr21", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonAddUps7_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx12Upscaler", "xess", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx12Upscaler", "xess", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonDlssDx12_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("Dx12Upscaler", "dlss", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("Dx12Upscaler", "dlss", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonFsr21Vulkan_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("VulkanUpscaler", "fsr21", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("VulkanUpscaler", "fsr21", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonFsr22Vulkan_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("VulkanUpscaler", "fsr22", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("VulkanUpscaler", "fsr22", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonFsr31Vulkan_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("VulkanUpscaler", "fsr31", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("VulkanUpscaler", "fsr31", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonDlssVulkan_Click(object sender, EventArgs e)
        {
            if (selectMod == "Optiscaler FSR 3.1/DLSS")
            {
                ConfigIni("VulkanUpscaler", "dlss", "mods\\Temp\\Optiscaler FG 3.1\\nvngx.ini", "Upscalers");
            }
            else if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
            {
                ConfigIni("VulkanUpscaler", "dlss", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
            }
        }

        private void buttonFg3_Click(object sender, EventArgs e)
        {
            if (selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("frame_generator", "fsr3", uniscaler_fsr31, "general");
            }
            else
            {
                MessageBox.Show("Select Uniscaler FSR 3.1 to proceed", "Uniscaler FSR 3.1", MessageBoxButtons.OK);
                return;
            }
        }

        private void buttonFg31_Click(object sender, EventArgs e)
        {
            if (selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("frame_generator", "fsr3_1", uniscaler_fsr31, "general");
            }
            else
            {
                MessageBox.Show("Select Uniscaler FSR 3.1 to proceed", "Uniscaler FSR 3.1", MessageBoxButtons.OK);
                return;
            }
        }
    }
}
