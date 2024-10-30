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
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Reflection.Metadata;

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

        public void AddItemlistMods(List<string> items,List<string>defaultMods = null)
        {
            List<string> itensDelete = new List<string> { "Elden Ring FSR3", "Elden Ring FSR3 V2", "Elden Ring FSR3 V3", "Disable Anti Cheat", "Unlock FPS Elden"};

            List<string> gamesIgnore = new List<string> { "Cyberpunk 2077", "Red Dead Redemption 2", "Dying Light 2", "Black Myth: Wukong", "Final Fantasy XVI","Star Wars Outlaws", "Horizon Zero Dawn", "Until Dawn", "Hogwarts Legacy", "Metro Exodus Enhanced Edition", "Lies of P" }; //List of games that have custom mods (e.g., Outlaws DLSS RTX) and also have default mods (0.7.6, etc.)

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
                listMods.Items.Clear();
                foreach (var item in items)
                {
                    listMods.Items.Add(item);
                }

                if (defaultMods != null)
                {
                    foreach (var defMods in defaultMods)
                    {
                        listMods.Items.Add(defMods);
                    }
                }
            }
            if (listMods.Text != "")
            {
                listMods.Text = "";
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
             { "Uniscaler V4", new string[]{  "mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod"}},
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
            { "Uniscaler V3", new string[]{  "mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
            { "Uniscaler V4", new string[]{  "mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod"}},
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
            { "Uniscaler V3", new string[]{  "mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
            { "Uniscaler V4", new string[]{  "mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod"}},
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
             { "Uniscaler V3", new string[]{  "mods\\FSR2FSR3_Uniscaler_V3\\Uni_V3\\Uni_Mod"}},
             { "Uniscaler V4", new string[]{  "mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod"}},
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
            {"Uniscaler V4",@"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"},
            {"The Callisto Protocol FSR3",@"\mods\Temp\FSR3_Callisto\enable_fake_gpu\\fsr2fsr3.config.toml"},
            {"FSR 3.1 Custom Wukong",@"\mods\Temp\Wukong_FSR31\enable_fake_gpu\\uniscaler.config.toml" }
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
            "Uniscaler V4",
            "Uniscaler FSR 3.1",
            "FSR 3.1 Custom Wukong"
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
                { "Uniscaler V4", @"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
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
                { "Uniscaler V4",@"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
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
                { "Uniscaler V4",@"mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
                { "Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
            };
        #endregion

        #region Folder Uniscaler V2
        Dictionary<string, string> folder_uniscaler_v2 = new Dictionary<string, string>
        {
            {"Uniscaler V2",@"mods\\Temp\\Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml"},
            {"Uniscaler V3",@"mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V4",@"mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Uniscaler V3
        Dictionary<string, string> folder_uniscalerV3 = new Dictionary<string, string>
        {
            {"Uniscaler V3",@"\mods\Temp\Uniscaler_V3\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler V4",@"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
            {"Uniscaler FSR 3.1",@"\mods\Temp\Uniscaler_FSR31\enable_fake_gpu\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Uniscaler FSR 3.1
        Dictionary<string, string> uniscaler_fsr31 = new Dictionary<string, string>
            {
                { "Uniscaler FSR 3.1", @"\mods\\Temp\\Uniscaler_FSR31\\enable_fake_gpu\\uniscaler.config.toml" },
                { "Uniscaler V4",@"\mods\\Temp\\Uniscaler_V4\\enable_fake_gpu\\uniscaler.config.toml"},
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
            {"Uniscaler V4", new string[]{"mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod"}},
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

        #region Clean Callisto Optiscaler Custom
        List<string> del_callisto_custom = new List<string>
        {
             "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dlsstweaks.ini", "DLSSTweaksConfig.exe", "dxgi.dll",
             "FSRBridge.asi", "libxess.dll", "nvngx.dll", "nvngx.ini","winmm.dll", "winmm.ini"
        };
        #endregion;

        #region Folder Tekken
        Dictionary<string, string[]> folderTekken = new Dictionary<string, string[]>
        {
            { "Unlock FPS Tekken 8", new string[] {"mods\\Unlock_fps_Tekken"}},
        };
        #endregion

        #region Folder Nvngx
           Dictionary<string, string> folderNvngx = new Dictionary<string, string>
            {
                { "NVNGX Version 1", "mods\\Temp\\nvngx_global\\nvngx\\nvngx.ini" },
                { "Xess 1.3", "mods\\Temp\\nvngx_global\\nvngx\\libxess.dll" },
                { "Dlss 3.7.0", "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlss.dll" },
                { "Dlss 3.7.0 FG", "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlssg.dll" },
                { "Dlss 3.7.2", "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll"},
                { "Dlssg 3.7.2 FG", "mods\\Temp\\nvngx_global\\nvngx\\Dlssg_3_7_1\\nvngx_dlssg.dll" },
                { "Dlssd 3.7.2", "mods\\Temp\\nvngx_global\\nvngx\\Dlssd_3_7_1\\nvngx_dlssd.dll" }
            };
        #endregion

        #region Clean Dlss Global Files
        List<string> del_dlss_global_rtx = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dlss-enabler-upscaler.dll", "dlss-enabler.log", "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better-3.0.dll",
            "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "fakenvapi.log", "libxess.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "unins000.dat", "winmm.dll","dlss-enabler.dll","dlss_rtx.txt"

        };

        List<string> del_dlss_global_amd = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dlss-enabler-upscaler.dll", "dlss-enabler.dll", "dlss-enabler.log", "dlssg_to_fsr3.ini", "dlssg_to_fsr3.log",
            "dlssg_to_fsr3_amd_is_better-3.0.dll", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "fakenvapi.ini", "fakenvapi.log", "libxess.dll", "nvapi64.dll", "nvngx-wrapper.dll","dlss_amd.txt",
            "nvngx.dll", "nvngx.ini", "nvngx_dlss.dll", "nvngx_dlssg.dll", "unins000.dat"

        };
        #endregion

        #region Clean DLSS To FSR
        List<string> del_dlss_to_fsr = new List<string>
        {
            "dlssg_to_fsr3_amd_is_better.dll","version.dll"
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
                { "Uniscaler V4",@"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
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
        "dxgi.dll", "EnableSignatureOverride.reg", "libxess.dll", "licenses", "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "RestoreNvidiaSignatureChecks.reg", "unins000.dat", "unins000.exe", "version.dll", "_nvngx.dll","dlss-enabler.dll",
            "dlssg_to_fsr3_amd_is_better-3.0.dll","fakenvapi.ini","fakenvapi.log","nvapi64.dll"
        };
        #endregion

        #region Clean Uniscaler Default
        List<string> del_uni_files = new List<string>
        {
            "Uniscaler.asi","winmm.dll","winmm.ini","uniscaler.config.toml"
        };
        #endregion

        #region Clean Toml File Folder

        Dictionary<string, string> folder_clean_toml = new Dictionary<string, string>
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
            { "FSR 3.1 Custom Wukong","mods\\FSR3_WUKONG\\WukongFSR31\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler V2", "mods\\FSR2FSR3_Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler V3", "mods\\FSR2FSR3_Uniscaler_V3\\enable_fake_gpu\\uniscaler.config.toml"},
            { "Uniscaler V4", "mods\\FSR2FSR3_Uniscaler_V4\\enable_fake_gpu\\uniscaler.config.toml"},
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
            {"Uniscaler V4",@"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
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

        public void ConfigIni3(Dictionary<string, string> keyValuePairs, string path, string? section = null)
        {
            string pathIni3 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, path);
            IniEditor3 iniEditor3 = new IniEditor3(pathIni3);

            iniEditor3.ConfigIni3(keyValuePairs, section);
        }


        public void ReplaceIni()
        {
            if (folder_clean_toml.ContainsKey(selectMod) && folderFakeGpu.ContainsKey(selectMod))
            {
                string path_clean_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder_clean_toml[selectMod]);
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
            panelAddOn2.Top = panelNvngx.Top + 33;
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

        List<string> fsr_2_2_opt = new List<string> {"A Plague Tale Requiem", "Achilles Legends Untold", "Alan Wake 2", "Assassin's Creed Mirage", "Atomic Heart", "Banishers: Ghosts of New Eden","Black Myth: Wukong","Blacktail", "Bright Memory: Infinite", "COD Black Ops Cold War", "Control", "Crysis 3 Remastered","Cyberpunk 2077", "Dakar Desert Rally", "Dead Island 2", "Death Stranding Director's Cut", "Dying Light 2",
            "Everspace 2", "Evil West", "F1 2022", "F1 2023","Final Fantasy XVI","FIST: Forged In Shadow Torch", "Fort Solis", "Hellblade 2","Ghostwire: Tokyo","God of War Ragnarök", "Hogwarts Legacy", "Kena: Bridge of Spirits", "Lies of P", "Loopmancer", "Manor Lords", "Metro Exodus Enhanced Edition", "Monster Hunter Rise","Nobody Wants To Die", "Outpost: Infinity Siege", "Palworld", "Ready or Not", "Remnant II", "RoboCop: Rogue City",
            "Sackboy: A Big Adventure", "Satisfactory", "Shadow Warrior 3", "Silent Hill 2", "Smalland", "STAR WARS Jedi: Survivor","Star Wars Outlaws", "Starfield", "Steelrising", "TEKKEN 8","Test Drive Unlimited Solar Crown", "The Chant","The Casting Of Frank Stone", "The Invincible", "The Medium","Until Dawn", "Unknown 9: Awakening", "Wanted: Dead","Warhammer: Space Marine 2"};

        List<string> fsr_2_1_opt = new List<string> { "Chernobylite", "Dead Space (2023)", "Hellblade: Senua's Sacrifice", "Hitman 3", "Horizon Zero Dawn", "Judgment", "Martha Is Dead", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Returnal", "Ripout", "Saints Row", "The Callisto Protocol", "Uncharted Legacy of Thieves Collection" };

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
            string[] uniscalerVersion = { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3","Uniscaler V4", "Uniscaler FSR 3.1" };

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
            Task.Run(async () =>
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
            });
        }

        #region CopyFolder2
        private void CopyFolder2(string sourceDirectory, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            foreach (string file in Directory.GetFiles(sourceDirectory))
            {
                string destFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }
        }
        #endregion

        #region CopyFolder3
        private void CopyFolder3(string sourceDirectory, string destinationDirectory)
        {
            if (!Path.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (string file in Directory.GetFiles(sourceDirectory))
            {
                string destFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }


            foreach (string subDirectory in Directory.GetDirectories(sourceDirectory))
            {
                string destSubDirectory = Path.Combine(destinationDirectory, Path.GetFileName(subDirectory));
                CopyFolder3(subDirectory, destSubDirectory);
            }
        }
        #endregion

        #region CopyFilesCustom
        private void CopyFilesCustom(string sourceFile, string destinationFile, string pathHelp)
        {
            if (Path.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            else
            {
                MessageBox.Show($"Please select the .exe path in \"Select Folder\". The path should look something like this: {pathHelp}", "Not Found", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region CopyFilesCustom2
        private void CopyFilesCustom2(string sourceDir, string destinationDir, string pathHelp = null)
        {
            if (Directory.Exists(sourceDir))
            {
                string[] files = Directory.GetFiles(sourceDir);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    string destFile = Path.Combine(destinationDir, fileName);

                    File.Copy(file, destFile, true);
                }
            }
            else
            {
                if (pathHelp != null)
                {
                    MessageBox.Show(pathHelp, "Not Found", MessageBoxButtons.OK);
                }
            }
        }

        #endregion

        #region CopyFilesCustom3
        private void CopyFilesCustom3(string sourceFile, string destDir, string messageNotFound, string regPath = null)
        {
            if (Path.Exists(sourceFile))
            {
                File.Copy(sourceFile, destDir, true);

                if (regPath != null)
                {
                    runReg(regPath);
                }
            }
            else
            {
                MessageBox.Show(messageNotFound, "Not Found", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region CopyFilesCustom4
        private void CopyFilesCustom4(string sourceFile, string destDir, string regPath = null)
        {
            if (Path.Exists(sourceFile))
            {
                File.Copy(sourceFile, destDir, true);

                if (regPath != null)
                {
                    runReg(regPath);
                }
            }
        }
        #endregion

        #region BackupDxgi
        public void BackupDxgi(string renameFile, string pathDxgi,string fileName)
        {
            if (File.Exists(pathDxgi))
            {
                string backupFolderDxgi = Path.Combine(selectFolder, "BackupDxgi");

                if (!Path.Exists(backupFolderDxgi))
                {
                    Directory.CreateDirectory(backupFolderDxgi);
                }

                File.Copy(pathDxgi, backupFolderDxgi + "\\" + fileName , true );

                File.Move(pathDxgi,selectFolder + "\\" + renameFile,true);
            }
        }
        #endregion

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

        private async Task optiscaler_custom()
        {
            #region Backup Files
            try
            {
                Task.Run(async () =>
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
                });
            }

            catch (Exception ex)
            {
            }
            #endregion

            CopyFolder("mods\\Optiscaler FSR 3.1 Custom");
        }

        private async Task optiscalerFsrDlss()
        {
            string pathOptiscaler = "mods\\Addons_mods\\OptiScaler";
            string pathOptiscalerDlss = "mods\\Addons_mods\\Optiscaler DLSS";

            if (File.Exists(Path.Combine(selectFolder, "nvngx_dlss.dll")))
            {
                await CopyFolder(pathOptiscaler);

                await Task.Delay((500));

                File.Move(Path.Combine(selectFolder, "nvngx.dll"), Path.Combine(selectFolder, "dxgi.dll"), true);
                File.Move(Path.Combine(selectFolder, "nvngx_dlss.dll"), Path.Combine(selectFolder, "nvngx.dll"), true);
            }
            else
            {
                await CopyFolder(pathOptiscalerDlss);
            }
        }

        private void dlssGlobal()
        {
            string pathRtx = "mods\\DLSS_Global\\RTX";
            string pathVarRtx = "mods\\DLSS_Global\\VarTxt\\RTX\\dlss_rtx.txt";
            string pathAmd = "mods\\DLSS_Global\\AMD";
            string pathVarAmd = "mods\\DLSS_Global\\VarTxt\\AMD\\dlss_amd.txt";

            string backupFolderDlss = Path.Combine(selectFolder, "Backup Dlss");

            DialogResult gpuDlss = MessageBox.Show("Do you have a RTX GPU?.", "Dlss GPU", MessageBoxButtons.YesNo);

            string pathGpu = gpuDlss == DialogResult.Yes ? pathRtx : pathAmd;

            if (!Directory.Exists(backupFolderDlss))
            {
                Directory.CreateDirectory(backupFolderDlss);
            }

            var originFiles = Directory.GetFiles(selectFolder);
            var dcFiles = Directory.GetFiles(pathGpu);

            var dlssFileNames = originFiles.Select(Path.GetFileName).ToHashSet();
            var dcFileNames = dcFiles.Select(Path.GetFileName).ToHashSet();

            var commonFiles = dcFileNames.Intersect(dlssFileNames).ToList();

            if (commonFiles.Any())
            {
                foreach (var fileName in commonFiles)
                {
                    string filesBackup = Path.Combine(pathGpu, fileName);
                    string pathUser = Path.Combine(backupFolderDlss, fileName);

                    File.Copy(filesBackup, pathUser, overwrite: true);
                }
            }

            CopyFolder(pathGpu);

            string pathVarToCopy = gpuDlss == DialogResult.Yes ? pathVarRtx : pathVarAmd;
            File.Copy(pathVarToCopy, Path.Combine(selectFolder, gpuDlss == DialogResult.Yes ? "dlss_rtx.txt" : "dlss_amd.txt"), true);

            runReg("mods\\FSR3_LOTF\\RTX\\LOTF_DLLS_3_RTX\\DisableNvidiaSignatureChecks.reg");
        }

        private void dlss_to_fsr()
        {
            string path_dlss_fsr = "mods\\DLSS_TO_FSR";

            CopyFolder(path_dlss_fsr);

            runReg("mods\\Temp\\disable signature override\\DisableSignatureOverride.reg");
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
            string jediPreset = "mods\\FSR3_Jedi\\Mods\\Jedi Preset\\STARWAR-ULTRA-REALISTA.ini";
            string jediFixRt = "mods\\FSR3_Jedi\\Mods\\Jedi Fix RT\\pakchunk99-Mods_CustomMod_P.pak";
            string jediAntiStutter = "mods\\FSR3_Jedi\\Mods\\Jedi Anti Stutter\\SWJS - FAI\\SWJSFAI.pak";
            string jediIntroSkip = "mods\\FSR3_Jedi\\Mods\\Jedi Intro Skip\\Default_Startup.mp4";
            string originFolderJedi = Path.GetFullPath(Path.Combine(selectFolder,"..\\..\\..\\SwGame"));

            if (selectMod == "DLSS Jedi")
            {
                CopyFSR(folderJedi);
            }

            if (MessageBox.Show("Do you want to install Graphics Preset?","Graphic Preset",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Copy(jediPreset, selectFolder + "\\STARWAR-ULTRA-REALISTA.ini",true);
            }
            if (Path.Exists(originFolderJedi + "\\Content\\Paks"))
            {
                if (MessageBox.Show("Do you want to install fix Ray Tracing?", "Fix RT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(jediFixRt, originFolderJedi + "\\Content\\Paks\\pakchunk99-Mods_CustomMod_P.pak",true);
                }

                if (MessageBox.Show("Do you want to install Anti Stutter?", "Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(jediAntiStutter, originFolderJedi + "\\Content\\Paks\\SWJSFAI.pak",true);
                }

                if (Path.Exists(originFolderJedi + "\\Content\\Movies"))
                {
                    if (MessageBox.Show("Do you want to skip the game\'s initial intro?", "intro SKip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(jediIntroSkip, originFolderJedi + "\\Content\\Movies\\Default_Startup.mp4",true);
                    }
                }
            }
            else
            {
                MessageBox.Show("If you want to install the other mods (Anti Stutter, Fix Rt, and Intro Skip), select the path to the game\\'s .exe file. The path should look like: Jedi Survivor\\SwGame\\Binaries\\Win64", "Path Not Found");
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

        private DialogResult ShowMessage(string message, string titleMessage)
        {
            return MessageBox.Show(message,titleMessage , MessageBoxButtons.YesNo);
        }

        public void wukongFsr3()
        {
            string wukong_file_optimized = @"mods\FSR3_WUKONG\BMWK\BMWK - SPF\pakchunk99-Mods_CustomMod_P.pak";
            string wukongGraphicPreset = @"mods\FSR3_WUKONG\Graphic Preset\Black Myth Wukong.ini";
            string wukongUe4Map = @"mods\FSR3_WUKONG\Map\WukongUE4SS";
            string wukongMap = @"mods\FSR3_WUKONG\Map\LogicMods";
            string wukongHdr = @"mods\FSR3_WUKONG\HDR\Force_HDR_Mode_P.pak";
            string wukongFsrCustom = @"mods\FSR3_WUKONG\WukongFSR31\FSR31_Wukong";
            string wukongCacheEnviroment = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string wukongCache = Path.Combine(wukongCacheEnviroment, "AppData");
            bool finalMessage = false;

            string fullPathWukong = Path.GetFullPath(Path.Combine(selectFolder, @"..\..\..\"));

            if (selectMod == "RTX DLSS FG Wukong")
            {
                dlss_to_fsr();
            }
            if (selectMod == "FSR 3.1 Custom Wukong")
            {
                CopyFolder(wukongFsrCustom);
            }
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
            {
                if (Path.Exists(wukongCache + "\\Local\\b1\\Saved\\D3DDriverByteCodeBlob_V4098_D5686_S372641794_R220.ushaderprecache"))
                {
                    if (MessageBox.Show("Do you want to clear the game cache? (it may prevent possible texture errors caused by the mod)", "Cache",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete(wukongCache + "\\Local\\b1\\Saved\\D3DDriverByteCodeBlob_V4098_D5686_S372641794_R220.ushaderprecache");
                    }
                }
            }

            #region Others Mods
            if (Path.Exists(fullPathWukong + "\\b1\\Binaries\\Win64"))
            {
                string modsPath = Path.Combine(fullPathWukong, "b1", "Content", "Paks", "~mods");
                if (!Path.Exists(modsPath)) Directory.CreateDirectory(modsPath);

                string message, title;

                if (MessageBox.Show(message = "Do you want to install the optimization mod? (Faster Loading Times, Optimized CPU and GPU Utilization, etc. To check the other optimizations, see the guide).", title = "Optimized Wukong", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string PathOptimized = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\Content\\Paks"));
                    if (Path.Exists(PathOptimized))
                    {
                        if (!Path.Exists(PathOptimized + "\\~mods")) Directory.CreateDirectory(PathOptimized + "\\~mods");
                        File.Copy(wukong_file_optimized, PathOptimized + "\\~mods\\pakchunk99-Mods_CustomMod_P.pak", true);
                    }
                    else
                    {
                        MessageBox.Show("Path \"b1\\Content\\Paks\" not found, please select the .exe path in \"Select Folder\". The path should look something like this: BlackMythWukong\\b1\\Binaries\\Win64", "Not Found", MessageBoxButtons.OK);
                        Debug.WriteLine(PathOptimized);
                    }
                    finalMessage = true;
                }

                if (MessageBox.Show(message = "Do you want to apply the Graphics Preset? (ReShade must be installed for the preset to work, check the guide for more information)", title = "Graphic Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    CopyFilesCustom(Path.Combine(wukongGraphicPreset), Path.Combine(Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..")), "Black Myth Wukong.ini"), "BlackMythWukong\\b1\\Binaries\\Win64");

                if (MessageBox.Show(message = "Do you want to enable Anti-Stutter - High CPU Priority? (prevents possible stuttering in the game)", title = "High CPU Priority", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    runReg("mods\\FSR3_WUKONG\\HIGH CPU Priority\\Install Black Myth Wukong High Priority Processes.reg");
                    File.Copy("mods\\FSR3_WUKONG\\HIGH CPU Priority\\Anti-Stutter - Utility.txt", selectFolder + "\\Anti-Stutter - Utility.txt", true);
                }

                if (MessageBox.Show(message = "Would you like to install the mini map?", title = "Mini Map", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(wukongUe4Map);
                    CopyFolder2(wukongMap, Path.Combine(fullPathWukong, "b1", "Content", "Paks", "LogicMods"));
                    finalMessage = true;
                }

                if (MessageBox.Show(message = "Would you like to install the HDR correction?", title = "HDR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(wukongHdr, modsPath + "\\Force_HDR_Mode_P.pak", true);
                    finalMessage = true;
                }

                if (finalMessage)
                    MessageBox.Show("Preset applied successfully. To complete the installation, go to the game's page in your Steam library, click the gear icon 'Manage' to the right of 'Achievements', select 'Properties', and in 'Launch Options', enter -fileopenlog.", "Success", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Path not found, please select the path: BlackMythWukong\\b1\\Binaries\\Win64 if you want to install additional mods (Mini Map, Anti Stuttering, etc.).", "Not Found", MessageBoxButtons.OK);
            }
            #endregion
        }

        public void gow4Fsr3()
        {
            if (selectMod == "Gow 4 FSR 3.1")
            {
                string var_gow4 = "mods\\FSR3_GOW4\\optiscaler.txt";

                File.Copy(var_gow4, selectFolder + "\\optiscaler.txt",true);
            }

            MessageBox.Show("Check the God of War 4 guide on Guide to complete the installation. (If you do not follow the steps in the guide, the mod will not work).", "Guider", MessageBoxButtons.OK);
        }

        public void gowRagFsr3()
        {
            #region Others Mods Gow Rag
            string gowRagAntiStutter = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Anti-Stutter GoW Ragnarok\\Install GoWR High CPU Priority.reg";
            string gowRagAntiStutterVar = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Anti-Stutter GoW Ragnarok\\Anti_Stutter.txt";
            string gowRagPreset = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\ReShade\\God of War Ragnarök.ini";
            string gowRagIntroSkip = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Intro Skip";
            string gowRagIntroPath = Path.Combine(selectFolder, "exec\\cinematics");
            string gowRag3050_2060 = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Unlock Vram\\GTX 1060 3050 6GB\\dxgi.dll";
            string gowRagVram6gb = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Unlock Vram\\6GB VRAM\\dxgi.dll";
            string gowRegVramVar = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Unlock Vram\\Vram.txt";

            if (selectMod == "Others Mods Gow Rag")
            {
                if (Path.Exists(gowRagIntroPath))
                {
                    if (MessageBox.Show("Do you want to install the Anti Stutter?","Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        runReg(gowRagAntiStutter);

                        File.Copy(gowRagAntiStutterVar, selectFolder + "\\Anti_Stutter.txt",true);
                    }

                    if (MessageBox.Show("Do you want to install the Graphics Preset?","Preset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(gowRagPreset, selectFolder + "\\God of War Ragnarök.ini",true);
                    }

                    if (MessageBox.Show("Do you want to install the Intro Skip?","intro Skip",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder3(gowRagIntroSkip, selectFolder);
                    }

                    if (MessageBox.Show("Do you want to install the VRAM Unlocker?","VRAM Fix",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("Do you have a 3050 or 1060 GPU?. If the game doesn\'t work, select the opposite option (if you selected \'yes\' the first time, select \'no\' so a different file will be installed)","GPU",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Copy(gowRag3050_2060, selectFolder + "\\dxgi.dll",true);
                        }
                        else
                        {
                            File.Copy(gowRagVram6gb, selectFolder + "\\dxgi.dll", true);
                        }

                        File.Copy(gowRegVramVar, selectFolder + "\\Vram.txt",true);
                    }
                }
                else
                {
                    MessageBox.Show("If you want to install the other mods (Anti Stutterr, Graphic Preset, etc.), select the path to the .exe, something like: God of War Ragnarök\\GoWR.exe", "Path Not Found");
                }
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
            string pathTcp = "mods\\FSR3_Callisto\\Reshade\\TCP Cinematic\\TCP.ini";
            string pathRealLife = "mods\\FSR3_Callisto\\Reshade\\The Real Life\\The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini";
            string optiscalerCallisto = "mods\\FSR3_Callisto\\Optiscaler";

            if (selectMod == "FSR 3.1.1/DLSS Callisto")
            {
                CopyFolder(optiscalerCallisto);
                runReg("mods\\Temp\\enable signature override\\EnableSignatureOverride.reg");
            }

            if (selectMod == "The Callisto Protocol FSR3")
            {

                foreach (string filesCallisto in Directory.GetFiles(pathCallisto))
                {
                    string fileName = Path.GetFileName(filesCallisto);

                    File.Copy(filesCallisto, selectFolder + "\\" + fileName, true);
                }
            }

            DialogResult callistoTcp = MessageBox.Show("Do you want to install the TCP mod? (It is necessary to install ReShade for this mod to work, check the guide in FSR GUIDE for more information about the mod.)", "TCP", MessageBoxButtons.YesNo);

            if (DialogResult.Yes == callistoTcp)
            {
                File.Copy(pathTcp, selectFolder + "\\TCP.ini",true);
            }

            DialogResult callistoRealLife = MessageBox.Show("Do you want to install the Real Life mod? (It is necessary to install ReShade for the mod to work, check the guide in FSR GUIDE for more information about the mod and how to install it.)", "Real Life", MessageBoxButtons.YesNo);

            if (DialogResult.Yes == callistoRealLife)
            {
                File.Copy(pathRealLife, selectFolder + "\\The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini",true);
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

        public void lopFsr3()
        {
            if (selectMod == "FSR 3.1.1/DLSS LOP")
            {
                optiscalerFsrDlss();
            }
        }

        public void quietPlaceFsr3()
        {
            string optiscalerQuietPlace = "mods\\Addons_mods\\OptiScaler";

            if (selectMod == "FSR 3.1.1/DLSS Quiet Place")
            {
                CopyFolder(optiscalerQuietPlace);
                runReg("mods\\Temp\\enable signature override\\EnableSignatureOverride.reg");
            }
        }

        public void metroFsr3()
        {
            string presetMetro = "mods\\FSR3_Metro\\Preset\\DefinitiveEdition.ini";

            if (selectMod == "Others Mods Metro")
            {
                if (MessageBox.Show("Do you want to install the Graphics Preset?", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(presetMetro, Path.Combine(selectFolder, "DefinitiveEdition.ini"), true);
                }
            }

        }

        public void untilFsr3()
        {
            string pathDocumentsUd = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pathEngineUd = Path.Combine(pathDocumentsUd, "My Games\\Bates\\Saved\\Config\\Windows\\Engine.ini");
            string antiStutterUd = "mods\\FSR3_UD\\Anti Stutter\\Install UD High CPU Priority.reg";
            string varAntiStutterUd = "mods\\FSR3_SH2\\Anti_Stutter\\AntiStutter.txt";
            string varPostProcessingUd = "mods\\FSR3_SH2\\Var\\PostProcessing.txt";

            if (selectMod == "Others Mods UD")
            {
               // Anti Strtter
               if (MessageBox.Show("Do you want to install the Anti Stutter?","Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom3(varAntiStutterUd, Path.Combine(selectFolder, "AntiStutter.txt"), "Path not found. Please try again", antiStutterUd);
                }

               // Post Processing
               if (Path.Exists(pathEngineUd))
               {
                    if (MessageBox.Show("Do you want to disable Depth of Field?","Depth of Field",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ConfigIni("r.DepthOfFieldQuality", "0", pathEngineUd, "SystemSettings");
                        File.Copy(varPostProcessingUd, Path.Combine(selectFolder, "PostProcessing.txt"), true);

                        MessageBox.Show("Depth of Field Successfully removed", "Depth of Field", MessageBoxButtons.OK);
                    }
               }
                else
                {
                    MessageBox.Show("Path not found. The path to the Engine.ini file is something like this: Documents\\My Games\\Bates\\Saved\\Config\\Windows.", "Not Found", MessageBoxButtons.OK);
                }
            }
        }

        public void sh2Fsr3()
        {
            string rootPathSh2 = Path.GetFullPath(Path.Combine(selectFolder, @"..\..\.."));
            string modsPathSh2 = @"mods\FSR3_SH2";
            string rtxFgSh2 = "mods\\FSR3_SH2\\RTX_FG";
            string dx12DllSh2 = "mods\\FSR3_SH2\\DX12DLL\\D3D12.dll";
            string pathUltraPlusOptimized = "mods\\FSR3_SH2\\Ultra Plus\\Optimized";
            string pathUltraPlusComplete = "mods\\FSR3_SH2\\Ultra Plus\\Normal";
            string pathEngineUltraPlus = "mods\\FSR3_SH2\\Ultra Plus\\Engine.ini";
            string pathFsr3FgOptimized = "mods\\FSR3_SH2\\FSR3 Native Optimized\\Engine.ini";
            string rayReconstructionDllSh2 = Path.Combine(modsPathSh2, @"RayReconstruction\nvngx_dlssd.dll");
            string rayReconstructionIniSh2 = Path.Combine(modsPathSh2, @"RayReconstruction\Engine.ini");
            string presetSh2 = "mods\\FSR3_SH2\\Preset\\Silent hill dark.ini";
            string introSkipSh2 = Path.Combine(modsPathSh2, @"Intro_Skip\LoadingScreen.bk2");
            string fsr31CustomRtxSh2 = @"mods\DLSS_Global\RTX_Custom";
            string antiStutterSh2 = Path.Combine(modsPathSh2, @"Anti_Stutter\Install Silent Hill 2 Remake High Priority Processes.reg");
            string varAntiStutterSh2 = Path.Combine(modsPathSh2, @"Anti_Stutter\AntiStutter.txt");
            string dlssPathSh2 = Path.Combine(rootPathSh2, @"SHProto\Plugins\DLSS\Binaries\ThirdParty\Win64");
            string unlockCutsceneFpsSh2 = Path.Combine(modsPathSh2, @"Unlock Cutscene Fps");
            string varNativeFsr3Sh2 = Path.Combine(modsPathSh2, @"Var\NativeFSR3.txt");
            string varPostProcessingSh2 = Path.Combine(modsPathSh2, @"Var\PostProcessing.txt");
            string varFsr3FgOpt = "mods\\FSR3_SH2\\FSR3 Native Optimized\\NativeFSR3Opt.txt";
            string appDataSh2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pathFolderEngineSh2 = Path.Combine(appDataSh2, @"SilentHill2\Saved\Config\Windows");
            string pathEngineIniSh2 = Path.Combine(appDataSh2, @"SilentHill2\Saved\Config\Windows\Engine.ini");
            string messagePathExeSh2 = "SHProto\\Binaries\\Win64";
            var postProcessingSh2 = new Dictionary<string, string>
            {
                {"r.SceneColorFringe.Max", "0"},
                {"r.SceneColorFringeQuality", "0"},
                {"r.motionblurquality", "0"},
                {"r.Distortion", "0"},
                {"r.DisableDistortion", "1"}
            };

            if (selectMod == "FSR 3.1.1/DLSS FG RTX Custom")
            {
                CopyFolder(fsr31CustomRtxSh2);
            }

            // DLSS FG RTX
            if (selectMod == "DLSS FG RTX")
            {
                CopyFolder(rtxFgSh2);
            }
            

            // Ultra Plus
            if (selectMod.Contains("Ultra Plus Complete") || selectMod.Contains("Ultra Plus Optimized"))
            {
                if (Path.Exists(pathEngineIniSh2))
                {
                    string sourcePathSh2;
                    if (selectMod == "Ultra Plus Optimized")
                    {
                        sourcePathSh2 = pathUltraPlusOptimized;
                    }
                    else
                    {
                        sourcePathSh2 = pathUltraPlusComplete;
                    }

                    CopyFolder3(sourcePathSh2, rootPathSh2);
                    File.Copy(pathEngineUltraPlus, Path.Combine(pathFolderEngineSh2, "Engine.ini"), true);
                    MessageBox.Show("Check the Silent Hill 2 guide to see how to activate Ultra Plus", "Guide");
                }
            }

            if (selectMod == "Others Mods Sh2")
            {
                // Anti Stutter
                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    runReg(antiStutterSh2);
                    File.Copy(varAntiStutterSh2, selectFolder + "\\AntiStutter.txt", true);
                }

                // Intro Skip
                if (Path.Exists(Path.Combine(rootPathSh2, "SHProto.exe")))
                {
                    if (MessageBox.Show("Do you want to install the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFilesCustom(introSkipSh2, Path.Combine(rootPathSh2, @"SHProto\Content\Movies\LoadingScreen.bk2"), messagePathExeSh2);
                    }
                }

                // Graphics Preset
                if (MessageBox.Show("Do you want to install the Graphics Preset?`It is recommended to view the guide before proceeding with the installation","Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(presetSh2, Path.Combine(selectFolder, "Silent hill dark.ini"), true);
                }

                // Unlock Cutscene FPS
                if (MessageBox.Show("Do you want to install the Unlock Cutscene Fps?", "Unlock Fps", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom2(unlockCutsceneFpsSh2, selectFolder, "Engine.ini file not found, please check the path C:\\Users\\YourName\\AppData\\Local\\SilentHill2\\Saved\\Config\\Windows and see if the file exists. If it doesn\'t, open the game for a few seconds and try reinstalling the mod.");
                }

                if (Path.Exists(pathEngineIniSh2))
                {
                    // Ray Reconstruction
                    if (Path.Exists(dlssPathSh2))
                    {
                        if (MessageBox.Show("Do you want to install the Ray Reconstruction?", "Reconstruction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CopyFilesCustom(rayReconstructionDllSh2, Path.Combine(dlssPathSh2, "nvngx_dlssd.dll"), messagePathExeSh2);
                            CopyFilesCustom(rayReconstructionIniSh2, Path.Combine(pathFolderEngineSh2, "Engine.ini"), messagePathExeSh2);
                        }
                    }

                    // Post Processing Effects
                    if (MessageBox.Show("Do you want to remove Post Processing Effects, such as Motion Blur, Distortion, etc.? Check Silent 2 guide in the Guide to see all the effects", "Post Processing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ConfigIni3(postProcessingSh2, pathEngineIniSh2, "SystemSettings");
                        File.Copy(varPostProcessingSh2, pathFolderEngineSh2 + "\\PostProcessing.txt", true);
                    }
                }
                else
                {
                    MessageBox.Show("Engine.ini not found. To install the other mods, check if the file is located in C:\\Users\\YourName\\AppData\\Local\\SilentHill2\\Saved\\Config\\Windows. If it's not there, open the game for a few seconds and reinstall.", "Not Found", MessageBoxButtons.OK);
                }
            }

            if (selectMod == "FSR 3.1.1/DLSS FG Custom" || selectMod == "Optiscaler FSR 3.1.1/DLSS")
            {
                if (Path.Exists(Path.Combine(pathFolderEngineSh2, "NativeFSR3.txt")))
                {
                    ConfigIni2("r.FidelityFX.FI.Enabled", "0", pathEngineIniSh2, "SystemSettings");

                    MessageBox.Show("Native FSR3 FG was removed, it is necessary for the mod to work", "FSR 3.1/DLSS");
                }
            }

            if (selectMod.Contains("FSR3 FG Native SH2") || selectMod.Contains("FSR3 FG Native SH2 + Optimization"))
            {
                if (Path.Exists(pathEngineIniSh2))
                {
                    if (MessageBox.Show("Do you have an RX 500/5000 or GTX GPU?", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(dx12DllSh2, Path.Combine(selectFolder, "DXD12.dll"), true);
                    }

                    if (selectMod == "FSR3 FG Native SH2")
                    {
                        ConfigIni("r.FidelityFX.FI.Enabled", "1", pathEngineIniSh2, "SystemSettings");
                        File.Copy(varNativeFsr3Sh2, Path.Combine(pathFolderEngineSh2, "NativeFSR3.txt"), true);
                    }
                    else
                    {
                        File.Copy(pathFsr3FgOptimized, Path.Combine(pathFolderEngineSh2, "Engine.ini"), true);
                        File.Copy(varFsr3FgOpt, Path.Combine(pathFolderEngineSh2, "NativeFSR3Opt.txt"), true);
                    }
                }
                else
                {
                    MessageBox.Show("Engine.ini file not found, please check the path C:\\Users\\YourName\\AppData\\Local\\SilentHill2\\Saved\\Config\\Windows and see if the file exists. If it doesn\'t, open the game for a few seconds and try reinstalling the mod.", "Not Found", MessageBoxButtons.OK);
                }
            }
        }

        public void hogLegacyFsr3()
        {
            string hlPreset = "mods\\FSR3_HL\\Preset\\Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt";
            string hlAntiStutter = "mods\\FSR3_HL\\Anti Stutter\\Install Hogwarts Legacy CPU Priority.reg";
            string hlVarAntiStutter = "mods\\FSR3_SH2\\Anti_Stutter\\AntiStutter.txt";
            string hlD13D12Dll = "d3d12.dll";
            string hlD3D12DllPath = Path.Combine(selectFolder, hlD13D12Dll);
            string hlDxgiDllPath = Path.Combine(selectFolder, "dxgi.dll");

            if (selectMod == "Others Mods HL")
            {
                // Graphics Preset
                if (MessageBox.Show("Do you want to install the Graphics Preset? See the guide to learn how to complete the installation.", "Preset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(hlPreset, Path.Combine(selectFolder,"Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt"), true);

                    if (File.Exists(hlDxgiDllPath))
                    {
                        File.Copy(hlDxgiDllPath, Path.Combine(selectFolder, "dxgi.txt"), true);

                        File.Move(hlDxgiDllPath, hlD3D12DllPath);
                    }
                }

                // Anti Stutter
                if (MessageBox.Show("Do you want to install the Anti Stutter?","Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom3(hlVarAntiStutter, Path.Combine(selectFolder, "AntiStutter.txt"), "File Not Found", hlAntiStutter);
                }
            }
        }

        public async void whmMarineFsr3()
        {
            string antiStutterMarine = "mods\\FSR3_Outlaws\\Anti_Stutter\\Install Star Wars Outlaws CPU Priority.reg";
            string txtStutterMarine = "mods\\FSR3_SpaceMarine\\Anti_Stutter\\Marine_Anti_Stutter.txt";
            string presetMarine = "mods\\FSR3_SpaceMarine\\Preset\\Warhammer 40000 Space Marine 2.ini";
            string pathDxgiMarine = Path.Combine(selectFolder, "dxgi.dll");
            string pathRenameDxgiMarine = Path.Combine(selectFolder, "d3d12.dll");
            string pathFsr31MarineRtx = "mods\\FSR3_SpaceMarine\\FSR 3.1\\RTX";
            string pathFsr31MarineAmd = "mods\\FSR3_SpaceMarine\\FSR 3.1\\AMD";
            string backupMarine = Path.Combine(selectFolder,"Backup DXGI");

            if (selectMod == "FSR 3.1 Space Marine")
            {
                if (Path.Exists(pathDxgiMarine))
                {             
                    if (!Path.Exists(backupMarine))
                    {
                        Directory.CreateDirectory(backupMarine);
                    }

                    File.Copy(pathDxgiMarine, Path.Combine(backupMarine,"dxgi.dll"), true);

                    File.Move(pathDxgiMarine, pathRenameDxgiMarine,true);

                }

                if (MessageBox.Show("Do you have an RTX GPU?", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(pathFsr31MarineRtx);
                }
                else
                {
                    CopyFolder(pathFsr31MarineAmd);
                }

                if (MessageBox.Show("Do you want to install the Anti Stutter?","Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    runReg(antiStutterMarine);
                    File.Copy(txtStutterMarine, Path.Combine(selectFolder, "Marine_Anti_Stutter.txt"),true);
                }

                if (MessageBox.Show("Do you have to install the Graphic Preset?, select the path similar to: client_pc\\root\\bin\\pc for the mod to work. (It is necessary to install ReShade for the preset to work. " +
                    "See the guide in the Guide to learn how to install it.)", "Graphic Preset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(presetMarine, Path.Combine(selectFolder, "Warhammer 40000 Space Marine 2.ini"), true);
                }
            }
        }

        public void ffxviFsr3()
        {

            string ffxviAntiStutter = "mods\\FSR3_FFVXI\\Anti Stutter\\Final Fantasy XVI High Priority Processes-7-2-1726663253\\Install Final Fantasy XVI High Priority Processes.reg";
            string ffsxiVarAntiStutter = "mods\\FSR3_FFVXI\\Anti Stutter\\Anti_Stutter.txt";
            string ffxviFix = "mods\\FSR3_FFVXI\\FFXVIFix";
            string ffxviPreset = "mods\\FSR3_FFVXI\\ReShade";

            if (selectMod == "FFXVI DLSS RTX")
            {
                dlss_to_fsr();
            }

            if (!File.Exists(Path.Combine(selectFolder, "d3d12.dll")))
            {
                BackupDxgi("d3d12.dll", selectFolder + "\\dxgi.dll","dxgi.dll");
            }


            if (selectMod == "Others Mods FFXVI")
                {
                if (Path.Exists(Path.Combine(selectFolder, "ffxvi.exe")))
                {
                    if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        runReg(ffxviAntiStutter);
                        File.Copy(ffsxiVarAntiStutter, selectFolder + "\\Anti_Stutter.txt",true);
                    }

                    if (MessageBox.Show("Do you want to install the fixes mod? (It unlocks FPS in cutscenes, adds ultrawide support, etc. See all fixes in the Guide)","FFXVI FIX",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder(ffxviFix);
                    }

                    if (MessageBox.Show("Do you want to install the Graphics Preset?", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder(ffxviPreset);

                        MessageBox.Show("Check the FINAL FANTASY XVI guide to complete the installation. (If you do not follow the steps in the guide, the mod will not work).", "Guide", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("If you want to install the other mods (Anti Stutterr, Graphic Preset, etc.), select the path to the .exe, something like: FINAL FANTASY XVI\\ffxvi.exe", "Path Not Found");
                }
            }
        }

        public void outlawsFsr3()
        {
            string presetOutlaws = "mods\\FSR3_Outlaws\\Preset\\Outlaws2.ini";
            string varStutterOutlaws ="mods\\FSR3_Outlaws\\Anti_Stutter\\Anti_Sttuter.txt";

            if (selectMod == "Outlaws DLSS RTX")
            {
                dlss_to_fsr();
            }

            if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                runReg("mods\\FSR3_Outlaws\\Anti_Stutter\\Install Star Wars Outlaws CPU Priority.reg");
                File.Copy(varStutterOutlaws, selectFolder + "\\Anti_Sttuter.txt", true); //File used to remove the Anti-Stutter in 'Cleanup Mod'
            }

            if (MessageBox.Show("Do you want to install the Graphics Preset", "Graphics Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Copy(presetOutlaws, selectFolder + "\\Outlaws2.ini", true);

                MessageBox.Show("To apply the graphics preset, see the Star Wars Outlaws guide in the Guide.", "Guide", MessageBoxButtons.OK);
            }
        }

        public void tekkenFsr3()
        {
            CopyFSR(folderTekken);
        }

        public void aw2Fsr3()
        {
            string appDataAw2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pathDxgiAw2 = Path.Combine(selectFolder, "dxgi.dll");
            string pathFolderIniAw2 = Path.Combine(appDataAw2, "Remedy\\AlanWake2");
            string pathIniAw2 = Path.Combine(pathFolderIniAw2, "renderer.ini");
            string pathBackupIniAw2 = Path.GetFullPath(Path.Combine(pathFolderIniAw2, ".."));
            string presetAw2 = "mods\\FSR3_AW2\\Preset\\Realistic Reshade.ini";
            string rtNormalAw2 = "mods\\FSR3_AW2\\RT\\Normal\\renderer.ini";
            string rtUltraAw2 = "mods\\FSR3_AW2\\RT\\Ultra\\renderer.ini";
            string varRtAw2 = "mods\\FSR3_AW2\\RT\\Var\\VarRT.txt";
            string antiStutterAw2 = "mods\\FSR3_AW2\\Anti Stutter\\Install Alan Wake 2 CPU Priority.reg";
            string varAntiStutterAw2 = "mods\\FSR3_SH2\\Anti_Stutter\\AntiStutter.txt";
            string varPostProcessingAw2 = "mods\\FSR3_AW2\\Var Post Processing\\VarPost.txt";

            Dictionary<string, bool> valueRemovePosAw2 = new Dictionary<string, bool>
               {
                   { "m_bLensDistortion",false},
                   { "m_bFilmGrain",false},
                   { "m_bVignette",false}
               };

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

            if (selectMod == "Others Mods AW2")
            {
                // Anti stutter
                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom4(varAntiStutterAw2, Path.Combine(selectFolder, "AntiStutter.txt"), antiStutterAw2);
                }

                // Preset
                if (MessageBox.Show("Do you want to install the Realistic preset? ReShade is required for the mod to work. See the guide for installation instructions.\nif you are going to use the FSR3 FG mod, it is recommended to install the preset first.", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom4(presetAw2, Path.Combine(selectFolder, "Realistic Reshade.ini"));

                    if (File.Exists(pathDxgiAw2)) // Rename the dxgi file so that the FG mods work
                    {
                        File.Copy(pathDxgiAw2, Path.Combine(selectFolder, "dxgi.txt"), true);
                        File.Move(pathDxgiAw2, Path.Combine(selectFolder, "D3D12.dll"), true);
                    }
                }

                // Control RT
                if (Path.Exists(pathIniAw2))
                {

                    if (MessageBox.Show("Do you want to unlock Ray Tracing from the game Control in Alan Wake 2? If you want to install Ultra Ray Tracing, just select \"No\" and then select \"Yes\" in the next window (This is the Standard version)", "RT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFilesCustom4(pathIniAw2, Path.Combine(pathBackupIniAw2, "renderer.ini"));
                        CopyFilesCustom4(varRtAw2, Path.Combine(selectFolder, "VarRT.txt"));
                        CopyFilesCustom4(rtNormalAw2, Path.Combine(pathFolderIniAw2, "renderer.ini"));
                    }

                    if (MessageBox.Show("Do you want to install Ultra Ray Tracing from the game Control in Alan Wake 2?", "RT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (!Path.Exists(Path.Combine(pathBackupIniAw2, "renderer.ini")))
                        {
                            CopyFilesCustom4(pathIniAw2, Path.Combine(pathBackupIniAw2, "renderer.ini"));
                        }

                        CopyFilesCustom4(varRtAw2, Path.Combine(selectFolder, "VarRT.txt"));
                        CopyFilesCustom4(rtUltraAw2, Path.Combine(pathFolderIniAw2, "renderer.ini"));
                    }
                }
                else
                {
                    MessageBox.Show("If you want to install Ray Tracing from the game Control for Alan Wake 2, please check if the path C:\\Users\\USER_NAME\\AppData\\Local\\Remedy\\AlanWake2 exists and try again.", "Not Found", MessageBoxButtons.OK);
                }

                if (MessageBox.Show("Do you want to fix possible ghosting issues caused by the FSR3 mod?", "Fix Ghosting Aw2", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!Path.Exists(Path.Combine(pathBackupIniAw2, "renderer.ini")))
                    {
                        CopyFilesCustom4(pathIniAw2, Path.Combine(pathBackupIniAw2, "renderer.ini"));
                    }

                    ConfigJson(pathIniAw2, valueRemovePosAw2, "Post-processing effects successfully removed");
                }
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
            #region Path Mods
            List<string> pathModsCb2077 = new List<string>
            {
                @"mods\\FSR3_CYBER2077\\mods\\CET",
                @"mods\\FSR3_CYBER2077\\mods\\Cyberpunk UltraPlus",
                @"mods\\FSR3_CYBER2077\\mods\\Nova_LUT_2-1",
                @"mods\\FSR3_CYBER2077\\mods\\Cyberpunk 2077 HD Reworked"
            };

            string path_cb2077_over = @"mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";
            string pathNovaLut = "mods\\FSR3_CYBER2077\\mods\\Nova_LUT_2-1";
            string pathHdReworked = "mods\\FSR3_CYBER2077\\mods\\Cyberpunk 2077 HD Reworked";
            string originPathCb2077 = Path.GetFullPath(Path.Combine(selectFolder, "..\\.."));
            #endregion

            if (selectMod == "RTX DLSS FG CB2077")
            {
                CopyFSR(folderCb2077);
                runReg(path_cb2077_over);
            }

            if (Path.Exists(originPathCb2077 + "\\bin"))
            {

                if (MessageBox.Show("Do you want to install Nova LUT 2-1 and HD Reworked for Cyberpunk 2077?", "Install Mods", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    foreach (string modsPathCb2077 in pathModsCb2077)
                    {
                        CopyFolder3(modsPathCb2077, originPathCb2077);
                    };
                }

                if ( MessageBox.Show("Do you want to install Reshade Real Life 2.0? (It is necessary to install Reshade for this mod to work. Please refer to the Guide for installation instructions.)", "ReShade", MessageBoxButtons.YesNo)== DialogResult.Yes)
                {
                    CopyFolder3("mods\\FSR3_CYBER2077\\mods\\V2.0 Real Life Reshade", originPathCb2077);
                }
            }
            else
            {
                MessageBox.Show("If you want to install the other mods (Nova Lut, Real Life and Ultra Realistic Textures), select the path to the .exe, it should be something like: Cyberpunk 2077/bin/x64", "Others Mods", MessageBoxButtons.OK);
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

        public void hzdFsr3()
        {

            if (selectMod == "Optiscaler Custom HZD")
            {
                optiscalerFsrDlss();
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
            buttonMethod0.Text = "Default";
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
                    if (selectMod != "Optiscaler FSR 3.1.1/DLSS")
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

                if (selectMod == "FSR 3.1.1/DLSS FG Custom")
                {
                    dlssGlobal();
                }
                if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
                {
                    optiscaler_custom();
                }
                if (folderEldenRing.ContainsKey(selectMod) || selectMod == "Unlock FPS Elden")
                {
                    eldenFsr3();
                }
                if (gameSelected == "Alan Wake 2")
                {
                   aw2Fsr3();
                }
                if (selectMod == "Ac Valhalla Dlss (Only RTX)")
                {
                    acValhallaDlss();
                }
                if (gameSelected == "The Callisto Protocol")
                {
                    callistoFsr3();
                }
                if (gameSelected == "Lies of P")
                {
                    lopFsr3();
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
                if (gameSelected == "Horizon Zero Dawn")
                {
                    hzdFsr3();
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
                if (gameSelected == "STAR WARS Jedi: Survivor")
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
                if (gameSelected == "Final Fantasy XVI")
                {
                    ffxviFsr3();
                }
                if (gameSelected == "Warhammer: Space Marine 2")
                {
                    whmMarineFsr3();
                }
                if (gameSelected == "Black Myth: Wukong")
                {
                    wukongFsr3();
                }
                if (gameSelected == "Silent Hill 2")
                {
                    sh2Fsr3();
                }
                if (gameSelected == "Until Dawn")
                {
                    untilFsr3();
                }
                if (gameSelected == "Metro Exodus Enhanced Edition")
                {
                    metroFsr3();
                }
                if (gameSelected == "A Quiet Place: The Road Ahead")
                {
                    quietPlaceFsr3();
                }
                if (gameSelected == "Hogwarts Legacy")
                {
                    hogLegacyFsr3();
                }
                if (gameSelected == "Star Wars Outlaws")
                {
                    outlawsFsr3();                 
                }
                if (gameSelected == "God Of War 4")
                {
                    gow4Fsr3();
                }
                if (gameSelected == "God of War Ragnarök")
                {
                    gowRagFsr3();
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
                            if (selectMod == "Uniscaler FSR 3.1" || selectMod == "Uniscaler V4")
                            {
                                string pathNvngxUni = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_uni_fsr3\\nvngx.dll";
                                File.Copy(pathNvngxUni, selectFolder + "\\nvngx.dll", true);
                            }
                            else if (selectMod == "Uniscaler V3")
                            {
                                string pathNvngxV3 = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_uni_fsr3\\nvngx.dll";
                                File.Copy(pathNvngxV3, selectFolder + "\\nvngx.dll", true);
                            }
                            else if (selectMod == "0.10.4")
                            {
                                string pathNvngx0_10_4 = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_0_10_4\\nvngx.dll";
                                File.Copy(pathNvngx0_10_4, selectFolder + "\\nvngx.dll", true);
                            }
                            else
                            {
                                pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx.dll";
                                File.Copy(pathNvngx, selectFolder + "\\nvngx.dll", true);
                            }
                        }
                        foreach (var fileDll in folderNvngx)
                        {
                            if (optNvngx.Contains(fileDll.Key))
                            {
                                string fileName = Path.GetFileName(fileDll.Value);
                                File.Copy(fileDll.Value, Path.Combine(selectFolder, fileName), true);
                            }
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
                        if (optAddOn == "Optiscaler" && selectMod != "Optiscaler FSR 3.1.1/DLSS")
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

        public async Task RestoreBackup(string nameFolderBackup)
        {
            try
            {
                Task.Run(async () =>
                {
                    if (Directory.Exists(selectFolder + "\\" + nameFolderBackup))
                    {
                        DialogResult restoreOriginFiles = MessageBox.Show("A backup folder with the original game files was found. Do you want to restore these files? (This is highly recommended)", "Restore Files", MessageBoxButtons.YesNo);

                        if (restoreOriginFiles == DialogResult.Yes)
                        {
                            foreach (string filesRestore in Directory.GetFiles(selectFolder + "\\" + nameFolderBackup))
                            {
                                string nameFileRestore = Path.GetFileName(filesRestore);

                                string destFilesRestore = Path.Combine(selectFolder, nameFileRestore);

                                File.Copy(filesRestore, destFilesRestore, true);
                            }
                            MessageBox.Show("The files have been successfully restored", "Sucess", MessageBoxButtons.OK);
                        }
                    }
                    Directory.Delete(selectFolder + "\\" + nameFolderBackup, true);
                });
            }
            catch { }
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

            RestoreBackup("Backup Dlss");
        }

        #region Cleanup Others Mods
        public bool CleanupOthersMods(string modName, string fileName,string destPath,string regPath = null)
        {
            string filePath = Path.Combine(destPath, fileName);
            if (File.Exists(filePath))
            {
                if (MessageBox.Show($"Do you want to remove the {modName} mod?", $"Remove {modName}", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(filePath);

                    if (regPath != null)
                    {
                        runReg(regPath);
                    }

                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Cleanup Others Mods 2
        public bool CleanupOthersMods2(string modName, string[] fileNames, string destPath, string messageOtMods, string regPath = null)
        {
            bool filesRemoved = false;
            List<string> filesToRemove = new List<string>();

            foreach (string fileName in fileNames)
            {
                string filePath = Path.Combine(destPath, fileName);
                if (File.Exists(filePath))
                {
                    filesToRemove.Add(filePath);
                }
            }

            if (filesToRemove.Count > 0)
            {
                if (MessageBox.Show(messageOtMods, $"Remove {modName}", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (string filePath in filesToRemove)
                    {
                        File.Delete(filePath);
                        filesRemoved = true;

                        if (regPath != null)
                        {
                            runReg(regPath);
                        }
                    }
                }
            }

            return filesRemoved;
        }
        #endregion

        #region Cleanup Optiscaler FSR DLSS
        public void CleanupOptiscalerFsrDlss(List<string> modList, string modName, bool removeDxgi = false, string searchFolderName = null)
        {
            try
            {
                if (selectMod == modName && MessageBox.Show($"Do you want to remove the {modName}?","Cleanup",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string nvngxPath = Path.Combine(selectFolder, "nvngx.dll");
                    string nvngxDlssPath = Path.Combine(selectFolder, "nvngx_dlss.dll");

                    if (File.Exists(nvngxPath))
                    {
                        File.Move(nvngxPath, nvngxDlssPath, true); // Reverts the original nvngx_dlss.dll file
                    }

                    if (removeDxgi)
                    {
                        string dxgiPath = Path.Combine(selectFolder, "dxgi.dll");
                        if (File.Exists(dxgiPath))
                        {
                            File.Delete(dxgiPath);
                        }
                    }

                    foreach (var item in Directory.GetFiles(selectFolder))
                    {
                        string fileName = Path.GetFileName(item);
                        if (modList.Contains(fileName))
                        {
                            File.Delete(item);
                        }
                    }

                    if (!string.IsNullOrEmpty(searchFolderName))
                    {
                        string modsPath = Path.Combine(selectFolder, searchFolderName);
                        if (Directory.Exists(modsPath))
                        {
                            Directory.Delete(modsPath, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing Callisto mods files, please try again or do it manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }
        #endregion

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
                        if (Path.Exists(pathBackupFolder))
                        {
                            foreach (var fileBackup in Directory.GetFiles(pathBackupFolder))
                            {
                                string fileName = Path.GetFileName(fileBackup);
                                File.Copy(fileBackup, Path.Combine(selectFolder, fileName), true);
                            }
                            Directory.Delete(pathBackupFolder, true);
                        }
                    }
                    runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");
                    #endregion

                    CleanupMod2(del_optiscaler, folderFakeGpu, "Mod Successfully Removed");
                }

                if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
                {
                    CleanupMod3(del_optiscaler_custom, "Optiscaler FSR 3.1.1/DLSS");
                    runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");

                    RestoreBackup("Backup Optiscaler");
                }

                if (selectMod == "FSR 3.1.1/DLSS FG Custom")
                {
                    CleanDlssGlobal("FSR 3.1.1/DLSS FG Custom");
                }

                if (gameSelected == "Cyberpunk 2077")
                {
                    #region Remove Files Cyberpunk
                    string rootPathCb2077 = Path.GetFullPath(Path.Combine(selectFolder, "..\\.."));
                    string[] modsNameCb2077 = ["HD Reworked Project.archive", "#####-NovaLUT-2.archive", "version.dll", "global.ini"]; 

                    if (selectMod == "RTX DLSS FG CB2077")
                    {
                        CleanupMod(del_cb2077_fsr3, folderCb2077);
                    }
                    //Remove the folder with HD Rework and Nova Lut
                    if (File.Exists(rootPathCb2077 + "\\archive\\pc\\mod\\HD Reworked Project.archive"))
                    {
                       if (MessageBox.Show("Would you like to remove the HD Reworked,Nova Lut 2-1 mods and Cyberpunk UltraPlus?", "Mods",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach (string modsRemoveCb2077 in modsNameCb2077)
                            {
                                File.Delete(rootPathCb2077 + "\\archive\\pc\\mod\\" + modsRemoveCb2077);
                                File.Delete(rootPathCb2077 + "\\bin\\x64\\" + modsRemoveCb2077);

                                if (Directory.Exists(rootPathCb2077 + "\\bin\\x64\\plugins"))
                                {
                                    Directory.Delete(rootPathCb2077 + "\\bin\\x64\\plugins", true);
                                }
                            }
                        }
                    }

                    //Remove Real Life Reshade
                    if (File.Exists(rootPathCb2077 + "\\bin\\x64\\V2.0 Real Life Reshade.ini"))
                    {
                        if (MessageBox.Show("Would you like to remove the V2.0 Real Life Reshade?","Mods",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(rootPathCb2077 + "\\bin\\x64\\V2.0 Real Life Reshade.ini");
                            Directory.Delete(rootPathCb2077 + "\\bin\\x64\\reshade-shaders",true);
                        }
                    }
                    #endregion
                }

                if (gameSelected == "The Callisto Protocol")
                {
                    #region Remove others mods

                    CleanupOthersMods("TCP", "TCP.ini",selectFolder);
                    CleanupOthersMods("Real Life", "The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini",selectFolder);
                   

                    if (selectMod == "FSR 3.1.1/DLSS Callisto")
                    {
                        CleanupMod3(del_callisto_custom, "FSR 3.1.1/DLSS Callisto");
                    }
                    #endregion
                }

                if (gameSelected  == "Lies of P")
                {
                    CleanupOptiscalerFsrDlss(del_optiscaler, "FSR 3.1.1/DLSS LOP", true);
                }

                if (gameSelected == "Metro Exodus Enhanced Edition")
                {
                    #region Remove Others Mods Metro

                    CleanupOthersMods("Graphics Preset", "DefinitiveEdition.ini", selectFolder);

                    #endregion
                }

                if (gameSelected == "God Of War 4")
                {
                    #region Del Files Optiscaler Gow 4
                    if (File.Exists(selectFolder + "\\amd_fidelityfx_vk.dll"))
                    {
                        CleanupMod3(del_optiscaler, "Gow 4 FSR 3.1");

                        runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");
                    }
                    if (File.Exists(selectFolder + "\\optiscaler.txt"))
                    {
                        File.Delete(selectFolder + "\\optiscaler.txt");
                    }
                    #endregion
                }

                if (gameSelected == "God of War Ragnarök")
                {
                    #region Remove others mods Gow Rag
                    try
                    {
                        string gowRagDisableAntiStutter = "mods\\FSR3_GOW_RAG\\God of War Ragnarök\\Anti-Stutter GoW Ragnarok\\Uninstall GoWR High CPU Priority.reg";
                        string[] gowRagIntroFiles = { "pss_studios.bk2", "pss_studios_30.bk2", "pss_studios_4k_30.bk2" };

                        if (Path.Exists(Path.Combine(selectFolder, "exec")))
                        {
                            if (File.Exists(Path.Combine(selectFolder, "Anti_Stutter.txt")))
                            {
                                CleanupOthersMods("Anti Stutter", "Anti_Stutter.txt", selectFolder, gowRagDisableAntiStutter);
                            }

                            if (File.Exists(Path.Combine(selectFolder, "Vram.txt")))
                            {
                                if (CleanupOthersMods("VRAM", "dxgi.dll", selectFolder))
                                {
                                     File.Delete(Path.Combine(selectFolder, "Vram.txt"));
                                }
                            }

                            if (File.Exists(Path.Combine(selectFolder, "exec\\cinematics\\pss_studios.bk2")))
                            {
                                if (MessageBox.Show("Do you want to remove the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    foreach (string introFile in gowRagIntroFiles)
                                    {
                                        string filesIntro = Path.Combine(selectFolder, "exec\\cinematics\\" + introFile);
                                        if (File.Exists(filesIntro))
                                        {
                                            File.Delete(filesIntro);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Could not remove the mod. Please close all folders related to the game and try again", "Error");
                    }
                    #endregion
                }

                if (gameSelected == "Star Wars Outlaws")
                {
                    if (selectMod == "Outlaws DLSS RTX")
                    {
                        CleanupMod3(del_dlss_to_fsr, "Outlaws DLSS RTX");
                    }

                    #region Others Mods

                    if (Path.Exists(selectFolder + "\\Anti_Sttuter.txt"))
                    {
                        if (MessageBox.Show("Do you want to remove the Anti Stutter? ", "Remove Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            runReg("mods\\FSR3_Outlaws\\Anti_Stutter\\Uninstall Star Wars Outlaws CPU Priority.reg");
                            File.Delete(selectFolder + "\\Anti_Sttuter.txt");
                        }
                    }

                    #endregion
                }

                if (gameSelected == "Warhammer: Space Marine 2")
                {
                    
                    #region Del Mods Warhammer: Space Marine 2
                    if (selectMod == "FSR 3.1 Space Marine")
                    {
                        if (MessageBox.Show("Do you have an RTX GPU?","GPU",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CleanupMod3(del_dlss_global_rtx, "FSR 3.1 Space Marine");
                        }
                        else
                        {
                            CleanupMod3(del_dlss_global_amd, "FSR 3.1 Space Marine");
                        }  
                        
                        if (Path.Exists(selectFolder + "\\Backup DXGI\\dxgi.dll"))
                        {
                            File.Copy(selectFolder + "\\Backup DXGI\\dxgi.dll", selectFolder + "\\dxgi.dll", true);

                           if (Path.Exists(selectFolder + "\\d3d12.dll"))
                            {
                                File.Delete(selectFolder + "\\d3d12.dll");
                            }

                            Directory.Delete(selectFolder + "\\Backup DXGI", true);
                        }
                    }
                    if (Path.Exists(selectFolder + "\\Marine_Anti_Stutter.txt"))
                    {
                        if (MessageBox.Show("Do you want to remove the Anti Stutter?","Anti Stutter",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            runReg("mods\\FSR3_Outlaws\\Anti_Stutter\\Uninstall Star Wars Outlaws CPU Priority.reg");
                        }
                    }
                    #endregion
                }

                if (gameSelected == "Horizon Zero Dawn")
                {
                    #region Del Others Mods Hzd

                        CleanupOptiscalerFsrDlss(del_optiscaler, "Optiscaler Custom HZD", false);

                    #endregion
                }

                if (gameSelected == "STAR WARS Jedi: Survivor")
                {
                    #region Cleanup others mods Jedi Survivor
                    string rootPathJedi = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..\\SwGame"));

                    if (selectMod == "DLSS Jedi")
                    {
                        CleanupMod(del_uni_files, folderJedi);
                    }

                    CleanupOthersMods("Fix RT", "pakchunk99-Mods_CustomMod_P.pak", rootPathJedi + "\\Content\\Paks");
                    CleanupOthersMods("Anti Stutter", "SWJSFAI.pak", rootPathJedi + "\\Content\\Paks");
                    CleanupOthersMods("Intro Skip", "Default_Startup.mp4", rootPathJedi + "\\Content\\Movies");
                    #endregion
                }

                if (gameSelected == "Final Fantasy XVI")
                {
                    #region Cleanup Mods FFXVI
                    if (selectMod == "FFXVI DLSS RTX")
                    {
                        CleanupMod3(del_dlss_to_fsr, "FFXVI DLSS RTX");
                    }
                    #endregion

                    #region Cleanup Others Mods FFXVI

                    string ffxviDisableAntiStutter = "mods\\FSR3_FFVXI\\Anti Stutter\\Final Fantasy XVI High Priority\\Uninstall Final Fantasy XVI High Priority Processes.reg";
                    string[] ffxviFixList = { "UltimateASILoader_LICENSE.md", "FFXVIFix.ini", "FFXVIFix.asi", "EXTRACT_TO_GAME_FOLDER", "dinput8.dll" };

                    CleanupOthersMods("Anti_Stutter.txt", "Anti_Stutter.txt", selectFolder, ffxviDisableAntiStutter);

                    if (File.Exists(Path.Combine(selectFolder, "UltimateASILoader_LICENSE.md")))
                    {
                        if (MessageBox.Show("Do you want to remove the FFXVI FIX?","Remove FFXVI FIx",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach(string ffxviFixFiles in Directory.GetFiles(selectFolder))
                            {
                                string ffxviFixName = Path.GetFileName(ffxviFixFiles);

                                if (ffxviFixList.Contains(ffxviFixName))
                                {
                                    string removeFfxviFixFiles = Path.Combine(selectFolder, ffxviFixName);

                                    File.Delete(removeFfxviFixFiles);
                                }
                            }
                        }
                    }

                    if (File.Exists(Path.Combine(selectFolder,"BackupDxgi\\dxgi.dll")))
                    {
                        File.Copy(Path.Combine(selectFolder, "BackupDxgi\\dxgi.dll"), selectFolder + "\\dxgi.dll",true);

                        Directory.Delete(Path.Combine(selectFolder, "BackupDxgi"),true);

                        if (File.Exists(Path.Combine(selectFolder, "d3d12.dll")))
                        {
                            File.Delete(Path.Combine(selectFolder, "d3d12.dll"));
                        }
                    }

                    #endregion
                }

                if (gameSelected == "Silent Hill 2")
                {
                    #region Cleanup Others Mods Sh2
                    try
                    {
                        string pathSh2 = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));
                        string pathAppDataSh2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string folderEngineIniSh2 = Path.Combine(pathAppDataSh2, "SilentHill2\\Saved\\Config\\Windows");
                        string engineIniSh2 = Path.Combine(folderEngineIniSh2, "Engine.ini");
                        string defaultEngineIniSh2 = "mods\\FSR3_SH2\\Engine_ini\\Default\\Engine.ini";
                        string removeAntiStutterSh2 = "mods\\FSR3_SH2\\Anti_Stutter\\Uninstall Silent Hill 2 Remake High Priority Processes.reg";
                        string pathMoviesSh2 = Path.Combine(pathSh2, "SHProto\\Content\\Movies");
                        string pathDlssDllSh2 = Path.Combine(pathSh2, "SHProto\\Plugins\\DLSS\\Binaries\\ThirdParty\\Win64");
                        string pathContentSh2 = Path.Combine(pathSh2, "SHProto\\Content\\Paks");
                        string[] removeUnlockFpsSh2 = { "SilentHill2RemakeFPSRose.asi", "dsound.dll" };
                        string[] restorePostProcessingSh2 = { "Engine.ini", "PostProcessing.txt" };
                        string[] rtxCustomFilesSh2 = {"amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dlss-enabler.dll", "dxgi.dll", "libxess.dll", "nvngx.ini" };
                        string[] delDlssSh2 = { "dxgi.dll", "ReShade.ini", "SH2UpscalerPreset.ini" };

                        if (selectMod == "DLSS FG RTX")
                        {
                            if (CleanupOthersMods2("DLSS FG RTX", delDlssSh2, selectFolder, "Do you want to remove the DLSS FG RTX?"))
                            {
                                if (Path.Exists(Path.Combine(selectFolder,"mods")) && Path.Exists(Path.Combine(selectFolder, "reshade-shaders")))
                                {
                                    Directory.Delete(Path.Combine(selectFolder, "mods"),true);
                                    Directory.Delete(Path.Combine(selectFolder, "reshade-shaders"),true);
                                }
                            }
                        }

                        if (selectMod.Contains("Ultra Plus Complete") || selectMod.Contains("Ultra Plus Optimized"))
                        {
                            CleanupOthersMods("Ultra Plus Complete", "~UltraPlus_v0.8.0_P.pak", pathContentSh2);
                            CleanupOthersMods("Ultra Plus Optimized","~UltraPlus_v1.0.4_P.pak", pathContentSh2);
                            
                            if (Path.Exists(folderEngineIniSh2))
                            {
                                File.Copy(defaultEngineIniSh2, Path.Combine(folderEngineIniSh2, "Engine.ini"),true);
                            }
                        }

                        if (CleanupOthersMods("Ray Reconstruction", "nvngx_dlssd.dll", pathDlssDllSh2))
                        {
                            if (Path.Exists(engineIniSh2))
                            {
                                File.Delete(engineIniSh2);
                                File.Copy(defaultEngineIniSh2,engineIniSh2);
                            }
                        }

                        CleanupOthersMods("Intro Skip", "LoadingScreen.bk2", pathMoviesSh2);

                        CleanupOthersMods("Anti Stutter", "AntiStutter.txt", selectFolder, removeAntiStutterSh2);

                        CleanupOthersMods("Graphics Preset", "Silent hill dark.ini", selectFolder);

                        CleanupOthersMods2("FSR 3.1.1/DLSS FG RTX Custom", rtxCustomFilesSh2, selectFolder, "Do you want to remove the FSR 3.1.1/DLSS FG RTX Custom?");

                        CleanupOthersMods2("Others Mods Sh2", removeUnlockFpsSh2, selectFolder, "Do you want to remove the Unlock Cutscene Fps?");

                        if (Path.Exists(engineIniSh2))
                        {
                            // Post Processing
                            if (File.Exists(Path.Combine(folderEngineIniSh2, "PostProcessing.txt")))
                            {
                                if (CleanupOthersMods2("Others Mods Sh2", restorePostProcessingSh2, folderEngineIniSh2, "Do you want to restore the Post Processing Effects?"))
                                {
                                    File.Copy(defaultEngineIniSh2, engineIniSh2, true);
                                }                         
                            }

                            // FSR3 FG Native SH2 and FSR3 FG Native SH2 + Optimization
                            if (File.Exists(Path.Combine(folderEngineIniSh2, "NativeFSR3Opt.txt")) && MessageBox.Show("Do you want to remove the Native FSR3 FG + Optimization?", "Native FSR3 FG", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                File.Copy(defaultEngineIniSh2, engineIniSh2, true);
                                File.Delete(Path.Combine(folderEngineIniSh2, "NativeFSR3Opt.txt"));
                            }

                            if (File.Exists(Path.Combine(folderEngineIniSh2, "NativeFSR3.txt")) && MessageBox.Show("Do you want to remove the Native FSR3 FG?", "Native FSR3 FG", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (Path.Exists(Path.Combine(selectFolder,"DXD12.dll")) && MessageBox.Show("Do you have an RX 500/5000 or GTX?","GPU",MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    File.Delete(Path.Combine(selectFolder, "DXD12.dll"));
                                }

                                ConfigIni("r.FidelityFX.FI.Enabled", "0", engineIniSh2, "SystemSettings");
                                File.Delete(Path.Combine(folderEngineIniSh2, "NativeFSR3.txt"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error clearing Silent Hill 2 mods files, please try again or do it manually", "Silent Hill2");
                        Debug.WriteLine(ex);
                    }
                    #endregion
                }

                if (gameSelected == "Until Dawn")
                {
                    #region Cleanup Others Mods Until Dawn
                    try
                    {
                        string documentsPathUd = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string enginePathUd = Path.Combine(documentsPathUd, "My Games\\Bates\\Saved\\Config\\Windows\\Engine.ini");
                        string removeAntiStutterUd = "mods\\FSR3_UD\\Anti Stutter\\Uninstall UD High CPU Priority.reg";

                        // Remove Anti Stutter
                        CleanupOthersMods("Anti Stutter", "AntiStutter.txt", selectFolder, removeAntiStutterUd);

                        // Enable Depth of Field
                        if (Path.Exists(Path.Combine(selectFolder, "PostProcessing.txt")))
                        {
                            if (Path.Exists(enginePathUd))
                            {
                                if (MessageBox.Show("Do you want to enable Depth of Field?","Depth of Field",MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    ConfigIni("r.DepthOfFieldQuality", "1", enginePathUd, "SystemSettings");
                                    File.Delete(Path.Combine(selectFolder, "PostProcessing.txt"));

                                    MessageBox.Show("Depth of field activated successfully", "Depth of Field");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Path not found. The path to the Engine.ini file is something like this: Documents\\My Games\\Bates\\Saved\\Config\\Windows.","Not Found");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error clearing Until Dawn mods files, please try again or do it manually", "Error");
                    }
                    #endregion
                }

                if (gameSelected == "A Quiet Place: The Road Ahead")
                {
                    #region Cleanup Mods A Quiet Place: The Road Ahead
                    if (selectMod == "FSR 3.1.1/DLSS Quiet Place")
                    {
                        CleanupMod3(del_optiscaler, "FSR 3.1.1/DLSS Quiet Place");
                        runReg("mods\\Temp\\disable signature override\\DisableSignatureOverride.reg");
                    }
                    #endregion
                }

                if (gameSelected == "Hogwarts Legacy")
                {
                    #region Cleanup Others Mods Hogwarts Legacy
                    string removeAntiStutterHl = "mods\\FSR3_HL\\Anti Stutter\\Uninstall Hogwarts Legacy CPU Priority.reg";
                    string pathD3D12Hl = Path.Combine(selectFolder, "d3d12.dll");
                    string pathDxgiHl = Path.Combine(selectFolder, "dxgi.txt");

                    // Anti Stutter
                    CleanupOthersMods("Anti Stutter", "AntiStutter.txt", selectFolder, removeAntiStutterHl);

                    if (Path.Exists(Path.Combine(selectFolder, "Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt")))
                    {
                        if (MessageBox.Show("Do you want to remove the Graphic Preset file and restore the dxgi.dll file? To completely remove the Preset, it's necessary to remove the remaining files via ReShade.","Preset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(Path.Combine(selectFolder, "Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt"));

                            if (Path.Exists(pathDxgiHl) && Path.Exists(pathD3D12Hl))
                            {
                                File.Delete(pathD3D12Hl);
                                File.Move(pathDxgiHl, Path.Combine(selectFolder,"dxgi.dll"),true);
                            }
                        }
                    }

                    #endregion
                }

                if (gameSelected == "Alan Wake 2")
                {
                    #region Paths AW2
                    string path_aw2_en = @"mods\\FSR3_GOT\\DLSS FG\\RestoreNvidiaSignatureChecks.reg";
                    string aw2AppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string aw2FolderIni = Path.Combine(aw2AppData, "Remedy\\AlanWake2");
                    string aw2BackupFolder = Path.GetFullPath(Path.Combine(aw2FolderIni, ".."));
                    string modifiedIniAw2 = Path.Combine(aw2FolderIni, "renderer.ini");
                    string removeAntiStutterAw2 = "mods\\FSR3_AW2\\Anti Stutter\\Uninstall Alan Wake 2 CPU Priority.reg";
                    #endregion

                    #region Cleanup Default Mods Aw2
                    if (folderAw2.ContainsKey(selectMod))
                    {
                        CleanupMod(del_aw2, folderAw2);

                        #region RestoreNvidiaSignatureChecks
                        if (selectMod == "Alan Wake 2 FG RTX")
                        {
                            runReg(path_aw2_en);
                        }
                        #endregion
                    }
                    #endregion

                    #region Cleanup Othres ModsAW2

                    //Anti Stutter
                    CleanupOthersMods("Anti Stutter", "AntiStutter.txt", selectFolder, removeAntiStutterAw2);


                    // Preset
                    if (Path.Exists(Path.Combine(selectFolder, "Realistic Reshade.ini")))
                    {
                        if (MessageBox.Show("Do you want to remove the Realistc Preset? To completely uninstall, it is necessary to remove the ReShade files", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(Path.Combine(selectFolder, "Realistic Reshade.ini"));

                            if (Path.Exists(Path.Combine(selectFolder, "D3D12.dll")) && Path.Exists(Path.Combine(selectFolder, "dxgi.txt")))
                            {
                                File.Delete(Path.Combine(selectFolder, "D3D12.dll"));
                                File.Move(Path.Combine(selectFolder, "dxgi.txt"), Path.Combine(selectFolder, "dxgi.dll"), true);
                            }
                        }
                    }

                    // Control RT
                    if (File.Exists(Path.Combine(aw2BackupFolder, "renderer.ini")) && Path.Exists(Path.Combine(selectFolder, "VarRT.txt")))
                    {
                        if (MessageBox.Show("Do you want to remove the Control RT?", "RT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CopyFilesCustom4(Path.Combine(aw2BackupFolder, "renderer.ini"), Path.Combine(aw2FolderIni, "renderer.ini"));

                            File.Delete(Path.Combine(aw2BackupFolder, "renderer.ini"));
                            File.Delete(Path.Combine(selectFolder, "VarRT.txt"));
                        }
                     }

                    // Post Processing
                    if (File.Exists(Path.Combine(aw2BackupFolder, "renderer.ini")) && Path.Exists(Path.Combine(selectFolder, "VarPost.txt")))
                    {

                        if (MessageBox.Show("Do you want to restore post-processing effects?", "Restore", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CopyFilesCustom4(Path.Combine(aw2BackupFolder, "renderer.ini"), Path.Combine(aw2FolderIni, "renderer.ini"));

                            MessageBox.Show("Post-processing effects successfully restored", "Sucess", MessageBoxButtons.OK);
                        }

                        File.Delete(Path.Combine(aw2BackupFolder, "renderer.ini"));
                        File.Delete(Path.Combine(selectFolder, "VarPost.txt"));
                    }
                    #endregion

                }

                else if (rdr2_folder.ContainsKey(selectMod))
                {
                    CleanupMod(del_rdr2_custom_files, rdr2_folder);
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
                else if (gameSelected == "Black Myth: Wukong")
                {
                    string PathOptimizedDel = Path.Combine(selectFolder, "..\\..\\Content\\Paks");
                    string fullPathOptimizedDel = Path.GetFullPath(PathOptimizedDel);

                    CleanupMod3(del_dlss_to_fsr, "RTX DLSS FG Wukong");

                    #region Remove other mods

                    if (selectMod == "FSR 3.1 Custom Wukong")
                    {
                        if (File.Exists(selectFolder + "\\libxess.dll"))
                        {
                            string[] filesFsrWukong = { "libxess.dll", "nvngx.dll", "amd_fidelityfx_vk.dll", "amd_fidelityfx_dx12.dll" };

                            foreach (string fsrFilesWukong in Directory.GetFiles(selectFolder))
                            {
                                string wukongFsrName = Path.GetFileName(fsrFilesWukong);

                                if (filesFsrWukong.Contains(wukongFsrName))
                                {
                                    File.Delete(fsrFilesWukong);
                                }
                            }
                        }
                    }

                    if (File.Exists(fullPathOptimizedDel + "\\~mods\\pakchunk99-Mods_CustomMod_P.pak"))
                    {
                        DialogResult delWukongOptimized = MessageBox.Show("Do you want to remove the optimization mod?", "Remove", MessageBoxButtons.YesNo);

                        if (DialogResult.Yes == delWukongOptimized)
                        {
                            File.Delete(fullPathOptimizedDel + "\\~mods\\pakchunk99-Mods_CustomMod_P.pak");
                        }
                    }

                    if (File.Exists(selectFolder + "\\Anti-Stutter - Utility.txt"))
                    {
                        DialogResult delWukongAntiStutter = MessageBox.Show("Do you want to remove the Anti Stutter?", "Remove Anti Stutter", MessageBoxButtons.YesNo);

                        if (DialogResult.Yes == delWukongAntiStutter)
                        {
                            runReg("mods\\FSR3_WUKONG\\HIGH CPU Priority\\Uninstall Black Myth Wukong High Priority Processes.reg");

                            File.Delete(selectFolder + "\\Anti-Stutter - Utility.txt");
                        }
                    }

                    if (File.Exists(fullPathOptimizedDel + "\\~mods\\Force_HDR_Mode_P.pak"))
                    {
                        DialogResult delHdr = MessageBox.Show("Do you want to remove the HDR correction?", "HDR", MessageBoxButtons.YesNo);

                        if (DialogResult.Yes == delHdr)
                        {
                            File.Delete(fullPathOptimizedDel + "\\~mods\\Force_HDR_Mode_P.pak");
                        }
                    }

                    if (File.Exists(selectFolder + "\\dwmapi.dll"))
                    {
                        File.Delete(selectFolder + "\\dwmapi.dll");
                        Directory.Delete(selectFolder + "\\ue4ss",true);
                        Directory.Delete(fullPathOptimizedDel + "\\LogicMods", true);
                    }
                    #endregion
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

                RestoreBackup("Backup Files");

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
        List<string> uniscaler_list = new List<string> { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3", "Uniscaler V4", "Uniscaler FSR 3.1", "Uni Custom Miles", "Dlss Jedi" };
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
                { "Uniscaler V4", @"\mods\Temp\Uniscaler_V4\enable_fake_gpu\uniscaler.config.toml"},
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
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler V4" && selectMod != "Uniscaler FSR 3.1")
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
            else if (selectMod == "Uniscaler FSR 3.1" || selectMod == "Uniscaler V4")
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
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler FSR 3.1" && selectMod != "Uniscaler V4")
            {
                ConfigToml("upscaler", "\"fsr3\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler V4" || selectMod == "Uniscaler FSR 3.1")
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
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler V4" && selectMod != "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"dlss\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler V4" || selectMod == "Uniscaler FSR 3.1")
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
            else if (uniscaler_list.Contains(selectMod) && selectMod != "Uniscaler V3" && selectMod != "Uniscaler V4" && selectMod != "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"xess\"", folder_mod_operates, "general");
            }
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler V4" || selectMod == "Uniscaler FSR 3.1")
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
            else if (selectMod == "Uniscaler V3" || selectMod == "Uniscaler V4" || selectMod == "Uniscaler FSR 3.1")
            {
                ConfigToml("upscaler", "\"xess\"", folder_mod_operates, "general");
            }
        }

        private void modOpt5_Click(object sender, EventArgs e)
        {
            if (selectMod == "Uniscaler FSR 3.1" || selectMod == "Uniscaler V4")
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
                    panelAddOn.Top = label3.Top + label3.Height + 128;
                    panelAddOn.Left = label3.Left;
                    panel1.Location = new Point(10, 10);
                    panel1.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 20);
                }
                else
                {
                    label5.Top = label3.Top + label3.Height + 70;
                    label5.Left = label3.Left;
                    mainPanelUpsRes.Top = label3.Top + label3.Height + 128;
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
                panelFgMethod.Top = buttonAddOn.Top + 35;
            }
            else
            {
                buttonAddOn.Top = panelNvngx.Top + 72;
                panelAddOn2.Top = buttonAddOn.Top + 28;

                buttonFgMethod.Top = panelAddOn2.Top + 2;
                panelFgMethod.Top = buttonAddOn.Top + 62;
            }

            if (panelAddOn2.Visible == true)
            {
                buttonFgMethod.Top = panelAddOn2.Top + 52;
                panelFgMethod.Top = buttonAddOn.Top + 104;
            }
            else
            {
                panelFgMethod.Top = buttonAddOn.Top + 60;
            }
            ShowSubMenu(panelNvngx);
        }
        private void buttonFgMethod_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelFgMethod);
        }

        private void ShowSelectedNvngx(object sender, EventArgs e)
        {
            string[] optNvngx = { "Xess 1.3", "Dlss 3.7.0", "Dlss  3.7.0 FG", "Dlss 3.7.2", "Dlssg 3.7.2 FG", "Dlssd 3.7.2" };
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
                buttonFgMethod.Top = panelAddOn2.Top + 2;
                panelFgMethod.Top = buttonAddOn.Top + 57;
            }
            else if (panelAddOn2.Visible == true && panelNvngx.Visible == false)
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
            if (optionsAddOn.CheckedItems.Contains("Optiscaler") || selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
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
            if (selectMod == "Uniscaler FSR 3.1" || selectMod == "Uniscaler V4")
            {
                ConfigToml("frame_generator", "\"fsr3\"", uniscaler_fsr31, "general");
            }
            else
            {
                MessageBox.Show("Select Uniscaler FSR 3.1 or Uniscaler V4 to proceed", "Uniscaler", MessageBoxButtons.OK);
                return;
            }
        }

        private void buttonFg31_Click(object sender, EventArgs e)
        {
            if (selectMod == "Uniscaler FSR 3.1" || selectMod == "Uniscaler V4")
            {
                ConfigToml("frame_generator", "\"fsr3_1\"", uniscaler_fsr31, "general");
            }
            else
            {
                MessageBox.Show("Select Uniscaler FSR 3.1 or Uniscaler V4 to proceed", "Uniscaler", MessageBoxButtons.OK);
                return;
            }
        }
    }
}
