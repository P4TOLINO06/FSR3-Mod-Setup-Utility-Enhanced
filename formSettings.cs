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
using System.Diagnostics.Eventing.Reader;
using System.Reflection;

namespace FSR3ModSetupUtilityEnhanced
{

    public partial class formSettings : Form
    {
        private mainForm mainFormInstance;
        public static string gameSelected { get; set; }
        public static string fsrSelected { get; set; }
        public string selectFolder { get; set; }
        public string PresetValueFromLibrary { get; set; }

        public bool FlagTextBox1 { get; set; }

        public string gpuNameSettings { get; set; }

        public string TextBox1Text
        {
            get => textBox1.Text;
            set
            {
                textBox1.Text = value;
                selectFolder = value;
            }
        }

        public string listModsValue
        {
            get => listMods.SelectedItem?.ToString();
            set
            {
                int indexListMods = listMods.FindStringExact(value);
                if (indexListMods != -1)
                {
                    listMods.SelectedIndex = indexListMods;
                }
                else
                {
                    listMods.SelectedIndex = -1;
                }
            }
        }

        public bool EnableSigChecked
        {
            set
            {
                CheckEnableSig(value, null);
            }
        }
        public bool EnableDlssOverlayChecked
        {
            set
            {
                CheckEnableSig(null, value);
            }
        }
        public formEditorToml EditorForm { get; set; }
        string path_fsr = "";

        private List<string> pendingItems = new List<string>();
        private static formSettings instance;
        public string selectMod;
        private formEditorToml formEditor;
        public System.Windows.Forms.TextBox fpsLimitTextBox;
        public System.Windows.Forms.Label labelFpsLimit;
        private System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
        Form screenMethod = new Form(); //Optiscaler installation method screen

        public formSettings(mainForm main)
        {
            InitializeComponent();
            this.mainFormInstance = main;

            this.DoubleBuffered = true;

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panel1, new object[] { true });

            this.mainFormInstance = mainFormInstance;

            AddOptionsSelect.ItemCheck += new ItemCheckEventHandler(AddOptionsSelect_ItemCheck);
            listMods.SelectedIndexChanged += listMods_SelectedIndexChanged;
            this.Resize += new EventHandler(formSettings_Resize);
            TextBoxFpsLimit();
            SubMenuClose();
        }
        public static formSettings Instance(mainForm main)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new formSettings(main);
            }
            return instance;
        }

        static async Task<string[]> RunPowerShellCommandAsync(string command)
        {
            using (var process = Process.Start(new ProcessStartInfo("powershell", $"-Command {command}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }))
            {
                return await Task.Run(() =>
                {
                    return process?.StandardOutput.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                });
            }
        }

        public void AddItemlistMods(List<string> items, List<string> defaultMods = null, bool cleanCtrl = false)
        {
            List<string> itensDelete = new List<string> { "Elden Ring FSR3", "Elden Ring FSR3 V2", "FSR 3.1.4/DLSS FG Custom Elden", "Disable Anti Cheat", "Unlock FPS Elden" };

            List<string> gamesIgnore = new List<string> { "Final Fantasy XVI", "Red Dead Redemption", "Dragon Age: Veilguard", "A Plague Tale Requiem", "Watch Dogs Legion", "Saints Row",
                "Lego Horizon Adventures", "Assassin's Creed Mirage", "Stalker 2", "The Last Of Us Part I" , "Returnal", "Marvel\'s Spider-Man Miles Morales", "Marvel\'s Spider-Man Remastered","Marvel\'s Spider-Man 2","Shadow of the Tomb Raider", "Gotham Knights", "Steelrising", "Control", "FIST: Forged In Shadow Torch", "Ghostrunner 2", "Senua's Saga: Hellblade II", "Alone in the Dark", "Evil West", "The First Berserker: Khazan",
                "Assetto Corsa Evo", "Watch Dogs Legion", "Soulstice", "Back 4 Blood", "Final Fantasy VII Rebirth", "Lies of P", "Kingdom Come: Deliverance II", "Atomic Heart", "Palworld", "Alan Wake 2", "Stalker 2", "Frostpunk 2", "Lost Records Bloom And Rage", "Choo-Choo Charles", "Bright Memory", "Five Nights at Freddy’s: Security Breach", "GreedFall II: The Dying World", "Pacific Drive", "Dying Light 2", "Kena: Bridge of Spirits", "The Witcher 3" }; //List of games that have custom mods (e.g., RDR2 Mix) and also have default mods (0.7.6, etc.)

            if (itensDelete.Any(item => listMods.Items.Contains(item)))
            {
                foreach (string itemDelete in itensDelete)
                {
                    listMods.Items.Remove(itemDelete);
                }
            }

            if (listMods == null)
            {
                pendingItems.AddRange(items.Where(i => !pendingItems.Contains(i)));
                return;
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
                        if (!listMods.Items.Contains(defMods)) 
                            listMods.Items.Add(defMods);
                    }
                }
            }
            Debug.WriteLine(textBox1.Text);

            listMods.Text = "";
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

        public void cleanCtrl(bool cleanCtrl = false)
        {
            if (cleanCtrl)
            {
                listMods.Text = "";
                textBox1.Text = "";

            }
        }

        public void ClearListMods()
        {
            listMods.Items.Clear();
        }

        public void CheckEnableSig(bool? enabledSigCheck, bool? enableDlssOverlayCheck)
        {
            int indexEnSig = AddOptionsSelect.Items.IndexOf("Enable Signature Over");
            int indexDisSig = AddOptionsSelect.Items.IndexOf("Disable Signature Over");
            int indexDlssOverlay = AddOptionsSelect.Items.IndexOf("DLSS Overlay");

            if (indexEnSig != -1 && enabledSigCheck.HasValue)
            {
                AddOptionsSelect.SetItemChecked(indexEnSig, enabledSigCheck.Value);
            }

            if (indexDisSig != -1 && enabledSigCheck.HasValue)
            {
                AddOptionsSelect.SetItemChecked(indexDisSig, !enabledSigCheck.Value);
            }

            if (indexDlssOverlay != -1 && enableDlssOverlayCheck.HasValue)
            {
                AddOptionsSelect.SetItemChecked(indexDlssOverlay, enableDlssOverlayCheck.Value);
            }

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
            {"FSR 3.1.4/DLSS FG Custom Elden", new string[]{@"mods\Elden_Ring_FSR3\EldenRing_FSR3 v3"}},
            {"Unlock FPS Elden", new string[]{@"mods\\Elden_Ring_FSR3\\Unlock_Fps"}}
        };
        #endregion

        #region Del Elden Files
        List<string> del_elden = new List<string>
        {
            "_steam_appid.txt", "_winhttp.dll", "anti_cheat_toggler_config.ini", "anti_cheat_toggler_mod_list.txt",
            "start_game_in_offline_mode.exe", "toggle_anti_cheat.exe", "ReShade.ini", "EldenRingUpscalerPreset.ini",
            "dxgi.dll", "d3dcompiler_47.dll","EldenRingUpscaler.dll"
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
            { "Baldur's Gate 3 FSR3 V2", new string[] { "mods\\FSR3_BDG", "mods\\FSR3_BDG\\FSR3_BDG_2" } },
            { "Baldur's Gate 3 FSR3 V3", new string[] { "mods\\FSR3_BDG", "mods\\FSR3_BDG\\FSR3_BDG_2" } }
        };
        #endregion

        #region Folder GTA V
        Dictionary<string, string[]> folderGtaV = new Dictionary<string, string[]>
        {
            { "GTA V Online", new string []{"mods\\FSR3_GTAV\\GtaV_B02_FSR3"} },
            { "GTA V FiveM", new string []{"mods\\FSR3_GTAV\\GtaV_B02_FSR3"} },
            { "GYA V Epic", new string[] { "mods\\FSR3_GTAV\\GtaV_B02_FSR3" } },
            { "GTA V Epic V2", new string[] { "mods\\FSR3_GTAV\\Gtav_Epic" } },
        };
        
        string[] delGtavFsr3 = { "GTAVUpscaler.org.asi", "GTAVUpscaler.asi", "d3d12.dll", "dxgi.asi", "GTAVUpscaler.dll", "GTAVUpscaler.org.dll", "dinput8.dll" };

        #endregion 

        #region Clean RDR2 Files
        List<string> del_rdr2_custom_files = new List<string>
        {
            "ReShade.ini", "RDR2UpscalerPreset.ini", "d3dcompiler_47.dll", "d3d12.dll", "dinput8.dll",
            "ScriptHookRDR2.dll", "NVNGX_Loader.asi", "d3dcompiler_47.dll", "nvngx.dll", "winmm.ini",
            "winmm.dll", "fsr2fsr3.config.toml", "FSR2FSR3.asi", "fsr2fsr3.log"
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

        #region Clean Lotf Files
        List<string> del_lotf_files = new List<string>
        {
            "winmm.ini","winmm.dll","Uniscaler.asi","launch.bat","DisableEasyAntiCheat.bat"

        };
        #endregion

        #region Folder Palworld
        Dictionary<string, string[]> folderPw = new Dictionary<string, string[]>
        {
            { "Palworld FG Build03", new string[] {"mods\\FSR3_PW\\FG"}},
        };
        #endregion

        #region Clean Palworld Files
        List<string> del_pw_files = new List<string>
        {
            "ReShade.ini", "PalworldUpscalerPreset.ini", "d3dcompiler_47.dll", "d3d12.dll",
            "nvngx.dll"
        };
        #endregion

        #region Clean TCP

        string[] delTcp = { "dlsstweaks.ini", "DLSSTweaksConfig.exe", "FSRBridge.asi", "winmm.dll", "winmm.ini", "nvngx.dll", "EnableNvidiaSigOverride.reg", "DisableNvidiaSigOverride.reg", "winmm.ini", "winmm.dll" };

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
                { "DLSS 3.8.10", "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlss.dll" },
                { "DLSSG 3.8.1", "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlssg.dll" },
                { "DLSS 4", "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll"},
                { "DLSSG 4", "mods\\Temp\\nvngx_global\\nvngx\\Dlssg_3_7_1\\nvngx_dlssg.dll" },
                { "DLSSD 4", "mods\\Temp\\nvngx_global\\nvngx\\Dlssd_3_7_1\\nvngx_dlssd.dll" }
            };
        #endregion

        #region Clean Dlss Global Files
        List<string> del_dlss_global_rtx = new List<string>
        {
            "dlss-enabler-upscaler.dll", "dlss-enabler.dll", "dlss-enabler.log", "dlssg_to_fsr3.log","nvapi64-proxy",
            "dlssg_to_fsr3_amd_is_better-3.0.dll", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "fakenvapi.log", "nvngx-wrapper.dll",
            "nvngx.ini", "unins000.dat","winmm.dll", "fakenvapi.ini", "nvapi64.dll"


        };

        List<string> del_dlss_global_amd = new List<string>
        {
            "dlss-enabler-upscaler.dll", "dlss-enabler.dll", "dlss-enabler.log", "dlssg_to_fsr3.log","nvapi64-proxy.dll",
            "dlssg_to_fsr3_amd_is_better-3.0.dll", "dlssg_to_fsr3_amd_is_better.dll", "dxgi.dll", "fakenvapi.ini", "fakenvapi.log",
            "nvapi64.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "unins000.dat"

        };
        #endregion

        #region Clean DLSS To FSR
        List<string> del_dlss_to_fsr = new List<string>
        {
            "dlssg_to_fsr3_amd_is_better.dll","version.dll","DisableNvidiaSignatureChecks.reg","RestoreNvidiaSignatureChecks.reg"
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
        List<string> delOptiscaler = new List<string>
        {
            "OptiScaler.ini", "nvngx.dll", "libxess.dll","winmm.dll","nvapi64.dll","fakenvapi.ini","dlssg_to_fsr3_amd_is_better.dll","version.dll", "nvngx.ini"
        };
        #endregion

        #region Clean Optiscaler Custom Files
        List<string> delOptiscalerCustom = new List<string>
        {
        "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "DisableNvidiaSignatureChecks.reg", "DisableSignatureOverride.reg", "dlss-enabler-upscaler.dll", "dlss-enabler.log", "dlss-finder.exe", "dlssg_to_fsr3.ini", "dlssg_to_fsr3.log", "dlssg_to_fsr3_amd_is_better.dll",
        "dxgi.dll", "EnableSignatureOverride.reg", "libxess.dll", "licenses", "nvapi64-proxy.dll", "nvngx-wrapper.dll", "nvngx.dll", "nvngx.ini", "RestoreNvidiaSignatureChecks.reg", "unins000.dat", "unins000.exe", "version.dll", "_nvngx.dll","dlss-enabler.dll",
            "dlssg_to_fsr3_amd_is_better-3.0.dll","fakenvapi.ini","fakenvapi.log","nvapi64.dll"
        };
        #endregion

        #region Clean Optiscaler Custom Files 2
        List<string> delOptiscalerCustom2 = new List<string>
        {
            "amd_fidelityfx_dx12.dll", "dxgi.dll", "nvngx.dll", "nvngx.ini",
            "Uniscaler.asi", "uniscaler.config.toml", "winmm.dll", "winmm.ini"
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
            { "Uniscaler V2", "mods\\FSR2FSR3_Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler V3", "mods\\FSR2FSR3_Uniscaler_V3\\enable_fake_gpu\\uniscaler.config.toml"},
            { "Uniscaler V4", "mods\\FSR2FSR3_Uniscaler_V4\\enable_fake_gpu\\uniscaler.config.toml"},
            { "Uniscaler FSR 3.1","mods\\FSR2FSR3_Uniscaler_FSR3\\enable_fake_gpu\\uniscaler.config.toml"},
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
            {"The Callisto Protocol FSR3",@"\mods\FSR3_Callisto\enable_fake_gpu\fsr2fsr3.config.toml"}
        };
        #endregion

        #region Folder Optiscaler
        static Dictionary<string, string> folder_optiscaler = new Dictionary<string, string>()
        {
            { "fsr22","mods\\Temp\\OptiScaler\\OptiScaler.ini" }
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
                        DialogResult varFolderIni = MessageBox.Show("Path not found, the path to the renderer.ini file is something like this: C:\\Users\\YourName\\AppData\\Local\\Remedy\\AlanWake2. Would you like to select the path manually?", "Path Not Found", MessageBoxButtons.YesNo);

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
                                        MessageBox.Show("renderer.ini was not found in the folder. Please select the folder containing the renderer.ini file.", "File Not Found", MessageBoxButtons.OK);
                                    }
                                }
                                else
                                {
                                    varFolderIni = MessageBox.Show("No path was selected. Would you like to try selecting the path again?", "Empty Path", MessageBoxButtons.YesNo);

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
                MessageBox.Show("An error occurred in the Utility. Try closing and reopening it", "Error", MessageBoxButtons.OK);
            }
        }

        public async Task Backup(Dictionary<string, string[]> pathModFiles = null)
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
                        File.Copy(selectFolder + "\\winmm.dll", selectFolder + "\\Backup Files", true);
                        File.Copy(selectFolder + "\\winmm.ini", selectFolder + "\\Backup Files", true);
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
                                MessageBox.Show("No identical files were found for backup. You can proceed with the mod installation", "No identical files", MessageBoxButtons.OK);
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
                    { "folderEldenRing", folderEldenRing },
                    { "origins_2_2_folder",origins_2_2_folder },
                    { "folderGtaV",folderGtaV },
                    { "folderGot",folderGot },
                    { "folderForza",folderForza },
                    { "folderCb2077",folderCb2077 },
                    { "folderPw",folderPw },
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
            if (itemText == "Enable Signature Over" && CheckedOption is true)
            {

                string pathEnOver = @"mods\\Temp\\enable signature override\\EnableSignatureOverride.reg";

                runReg(pathEnOver);
            }
            if (itemText == "Disable Signature Over" && CheckedOption is true)
            {
                string pathDisOver = @"mods\\Temp\\disable signature override\\DisableSignatureOverride.reg";

                runReg(pathDisOver);
            }
            if (itemText == "DLSS Overlay")
            {
                if (CheckedOption)
                {
                    runReg("mods\\Addons_mods\\DLSS Preset Overlay\\Enable Overlay.reg");
                }
                else
                {
                    runReg("mods\\Addons_mods\\DLSS Preset Overlay\\Disable Overlay.reg");
                }
            }
        }

        private void AddOptionsSelect_MouseMove(object sender, MouseEventArgs e)
        {
            Dictionary<int, string> tooltipsAOS = new Dictionary<int, string>
            {
                { 0, "Allows you to check which version of DLSS is active in the game. Check/Uncheck to enable/disable." },
                { 1, "Opens the editor for the toml file of the mods 0.x/Uniscaler." },
                { 2, "Performs a backup of important files; only some games have this option. Recommended for use only with 0.x/Uniscaler mods" },
                { 11, "Disables overlays from Epic Games, Steam, etc." },
                { 15, "Enables Nvidia Signature" },
                { 16, "Disables Nvidia Signature" }
            };

            int index = AddOptionsSelect.IndexFromPoint(e.Location);

            if (index >= 0 && tooltipsAOS.ContainsKey(index))
            {
                string tip = tooltipsAOS[index];

                if (toolTip1.GetToolTip(AddOptionsSelect) != tip)
                {
                    toolTip1.SetToolTip(AddOptionsSelect, tip);
                }
            }
            else
            {
                toolTip1.SetToolTip(AddOptionsSelect, string.Empty);
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

        private async void formSettings_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gpuNameSettings) &&
            !gpuNameSettings.Contains("rtx", StringComparison.OrdinalIgnoreCase) &&
            AddOptionsSelect != null)
            {
                AddOptionsSelect.Items.Remove("DLSS Overlay");
            }

            if (pendingItems != null && listMods != null)
            {
                foreach (var item in pendingItems)
                {
                    listMods.Items.Add(item);
                }
                pendingItems.Clear();
            }

            if (!string.IsNullOrEmpty(PresetValueFromLibrary) && FlagTextBox1 && textBox1 != null)
            {
                selectFolder = PresetValueFromLibrary;
                textBox1.Text = PresetValueFromLibrary;
                PresetValueFromLibrary = null;
            }

            if (buttonAddOn != null && buttonNvngx != null)
                buttonAddOn.Top = buttonNvngx.Top + 30;

            if (panelAddOn2 != null && panelNvngx != null)
                panelAddOn2.Top = panelNvngx.Top + 33;

            if (buttonAddUps != null && buttonNvngx != null)
                buttonAddUps.Top = buttonNvngx.Top + 30;

            if (panelAddOnUps != null && panelNvngx != null)
                panelAddOnUps.Top = panelNvngx.Top + 32;

            if (buttonFgMethod != null && buttonAddOn != null)
                buttonFgMethod.Top = buttonAddOn.Top + 30;

            if (panelFgMethod != null && panelAddOn != null)
                panelFgMethod.Top = panelAddOn.Top + 34;

        }

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

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
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
                string[] removeOptNvngx = { "Xess 1.3", "DLSS 3.8.10", "DLSSG 3.8.1" };

                foreach (string nvngxOpt in removeOptNvngx)
                {
                    optionsNvngx.Items.Remove(nvngxOpt);
                }
            }
            else
            {
                string[] addOptNvngx = { "Xess 1.3", "DLSS 3.8.10", "DLSSG 3.8.1" };

                foreach (string addNvngx in addOptNvngx)
                {
                    if (!optionsNvngx.Items.Contains(addNvngx))
                    {
                        optionsNvngx.Items.Add(addNvngx);
                    }
                }
            }

            toolTip.SetToolTip(listMods, listMods.Text);
        }

        List<string> fsr_2_2_opt = new List<string> {"A Plague Tale Requiem", "Achilles Legends Untold", "Alan Wake 2", "Assassin's Creed Mirage", "Atomic Heart", "Banishers: Ghosts of New Eden","Black Myth: Wukong","Blacktail", "COD Black Ops Cold War", "Control", "Crysis 3 Remastered", "Dakar Desert Rally", "Dead Island 2", "Death Stranding Director's Cut", "Dragon Age: Veilguard", "Dying Light 2",
            "Everspace 2", "Evil West", "F1 2022", "F1 2023","Final Fantasy XVI","FIST: Forged In Shadow Torch", "Fort Solis", "Senua's Saga: Hellblade II","Ghostwire: Tokyo","God of War Ragnarök", "Hogwarts Legacy", "Horizon Zero Dawn\\Remastered", "Kena: Bridge of Spirits", "Lies of P", "Lego Horizon Adventures", "Loopmancer", "Manor Lords","Marvel's Avengers", "Metro Exodus Enhanced Edition", "Microsoft Flight Simulator 24","Monster Hunter Rise","Nobody Wants To Die", "Outpost: Infinity Siege", "Palworld", "Ready or Not", "Remnant II", "RoboCop: Rogue City",
            "Sackboy: A Big Adventure", "Satisfactory", "Shadow Warrior 3", "Silent Hill 2", "Smalland", "STalker 2" ,"Star Wars: Jedi Survivor","Star Wars Outlaws", "Starfield", "Steelrising", "TEKKEN 8","Test Drive Unlimited Solar Crown", "The Chant","The Casting Of Frank Stone", "The Invincible", "The Medium","Until Dawn", "Unknown 9: Awakening", "Wanted: Dead","Warhammer: Space Marine 2"};

        List<string> fsr_2_1_opt = new List<string> { "Chernobylite", "Dead Space (2023)", "Hellblade: Senua's Sacrifice", "Hitman 3", "Judgment", "Martha Is Dead", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Returnal", "Ripout", "Saints Row", "The Callisto Protocol", "Uncharted Legacy of Thieves Collection" };

        List<string> fsr_2_0_opt = new List<string> { "Alone in the Dark", "Brothers: A Tale of Two Sons Remake", "Crime Boss: Rockay City", "Deathloop", "Dying Light 2", "Ghostrunner 2", "High On Life", "Jusant", "Layers of Fear", "Marvel's Guardians of the Galaxy", "Nightingale", "Rise of The Tomb Raider", "Shadow of the Tomb Raider", "The Outer Worlds: Spacer's Choice Edition", "The Witcher 3" };

        List<string> fsr_sdk_opt = new List<string> { "MOTO GP 24", "Pacific Drive", "Ratchet & Clank - Rift Apart" };

        List<string> fsr_sct_2_2 = new List<string> { "2.2" };

        List<string> fsr_sct_2_1 = new List<string> { "2.1" };

        List<string> fsr_sct_2_0 = new List<string> { "2.0" };

        List<string> fsr_sct_sdk = new List<string> { "SDK" };

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
            string[] uniscalerVersion = { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3", "Uniscaler V4", "Uniscaler FSR 3.1" };

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

                            if (uniscalerVersion.All(uniscalerVersion => !selectedVersion.Contains(uniscalerVersion) && !folderEldenRing.ContainsKey(selectMod)))
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
                string destFile = Path.Combine(selectFolder, fileName);

                using FileStream sourceStream = File.Open(files_fsr, FileMode.Open, FileAccess.Read, FileShare.Read);
                using FileStream destinationStream = File.Create(destFile);

                await sourceStream.CopyToAsync(destinationStream);
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

                    using FileStream sourceStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using FileStream destinationStream = File.Create(destFilePath);

                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
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
        public void BackupDxgi(string renameFile, string pathDxgi, string fileName)
        {
            if (File.Exists(pathDxgi))
            {
                string backupFolderDxgi = Path.Combine(selectFolder, "BackupDxgi");

                if (!Path.Exists(backupFolderDxgi))
                {
                    Directory.CreateDirectory(backupFolderDxgi);
                }

                File.Copy(pathDxgi, backupFolderDxgi + "\\" + fileName, true);

                File.Move(pathDxgi, selectFolder + "\\" + renameFile, true);
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

        public bool HandlePrompt(string windowTitle, string windowMessage, Action<bool> actionFunc = null)
        {
            var result = MessageBox.Show(windowMessage, windowTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            bool userChoice = (result == DialogResult.Yes);

            if (userChoice && actionFunc != null)
            {
                actionFunc(userChoice);
            }

            return userChoice;
        }

        public System.Windows.Forms.ProgressBar HandleProgressBar(bool isComplete, System.Windows.Forms.ProgressBar progressBar = null)
        {
            if (isComplete)
            {
                if (progressBar != null)
                {
                    this.Controls.Remove(progressBar);
                    progressBar = null;
                }
            }
            else
            {
                progressBar = new System.Windows.Forms.ProgressBar
                {
                    Minimum = 0,
                    Maximum = 100,
                    Value = 0,
                    Dock = DockStyle.Top
                };

                this.Controls.Add(progressBar);
            }

            return progressBar;
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

                        if (delOptiscalerCustom.Contains(filesOptsName))
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

        string[] modsToInstallOptiscalerFsrDlss = { "FSR 3.1.4/DLSS FG (Only Optiscaler)", "FSR 3.1.4/DLSSG FG (Only Optiscaler)", "FSR 3.1.4/DLSS Gow4" };
        private async Task optiscalerFsrDlss()
        {
            var progressBar = HandleProgressBar(false);

            #region Paths
            string pathOptiscaler = "mods\\Addons_mods\\OptiScaler";
            string pathOptiscalerDlss = "mods\\Addons_mods\\Optiscaler DLSS";
            string pathOptiscalerDlssg = "mods\\Addons_mods\\Optiscaler DLSSG\\OptiScaler.ini";
            string pathIniOlyUpscalers = "mods\\Addons_mods\\Optiscaler Only Upscalers\\OptiScaler.ini";
            string pathDlssToFsr = "mods\\Addons_mods\\Optiscaler DLSSG\\dlssg_to_fsr3_amd_is_better.dll";
            string nvapiIni = "mods\\Addons_mods\\Nvapi AMD\\Nvapi Ini\\OptiScaler.ini";
            string nvapiAmd = "mods\\Addons_mods\\Nvapi AMD\\Nvapi";
            string nvapiAntiLagDlssg = "mods\\Addons_mods\\Nvapi AMD\\DLSSG Nvapi Ini\\OptiScaler.ini";
            string nvapiFile = null;
            string destPathNvapi = Path.Combine(selectFolder, "OptiScaler.ini");
            string[] gamesToInstallNvapiAmd = { "Microsoft Flight Simulator 2024", "Death Stranding Director's Cut", "Shadow of the Tomb Raider", "The Witcher 3", "Rise of The Tomb Raider", "Uncharted Legacy of Thieves Collection", "Suicide Squad: Kill the Justice League", "Mortal Shell", "Steelrising", "FIST: Forged In Shadow Torch", "Final Fantasy XVI", "Sengoku Dynasty",
            "Stalker 2", "Monster Hunter Wilds", "AVOWED", "A Plague Tale Requiem", "Lost Records Bloom And Rage", "Frostpunk 2", "Star Wars: Jedi Survivor", "Deliver Us Mars", "Chernobylite 2: Exclusion Zone", "Grand Theft Auto V", "Assassin\'s Creed Shadows", "Star Wars Outlaws", "The Elder Scrolls IV: Oblivion Remastered", "Satisfactory", "The Alters", "Wuchang: Fallen Feathers" };
            string[] gamesToUseAntiLag2 = { "God of War Ragnarök", "God Of War 4", "Path of Exile II", "Hitman 3", "Marvel's Midnight Suns", "Hogwarts Legacy", "God Of War 4", "The First Berserker: Khazan" };
            string[] gamesOnlyUpscalers = { "The Last Of Us Part I" };
            string[] gamesWithDlssg = { "The First Berserker: Khazan", "Marvel\'s Spider-Man Remastered", "Marvel\'s Spider-Man Miles Morales", "Marvel\'s Spider-Man 2", "Alan Wake 2", "Stalker 2", "Eternal Strands", "Hogwarts Legacy", "Fort Solis", "Monster Hunter Wilds", "AVOWED", "A Plague Tale Requiem", "Lost Records Bloom And Rage", "Frostpunk 2", "God of War Ragnarök",
            "Star Wars: Jedi Survivor", "Deliver Us Mars", "Chernobylite 2: Exclusion Zone", "The Last of Us Part II", "Assassin\'s Creed Shadows", "The Elder Scrolls IV: Oblivion Remastered", "Star Wars Outlaws", "Satisfactory", "The Alters", "Wuchang: Fallen Feathers" };
            string[] gamesWithAntiCheat = { "Back 4 Blood", "Palworld", "Grand Theft Auto V" };
            string[] gamesNoNvngx = { "Red Dead Redemption 2", "Marvel\'s Spider-Man Remastered", "Marvel\'s Spider-Man Miles Morales", "Marvel\'s Spider-Man 2" }; // Games that don't need the file nvngx_dlss.dll renamed to nvngx.dll (Only RTX)
            string[] gpusVar = { "amd", "rx", "intel", "arc", "gtx" };
            #endregion

            Debug.WriteLine(gpuNameSettings);

            try
            {
                progressBar.Maximum = 3;
                progressBar.Value = 0;

                // Rename the dxgi.dll file from ReShade to d3d12.dll
                if (Path.Exists(Path.Combine(selectFolder, "dxgi.dll")) && Path.Exists(Path.Combine(selectFolder, "reshade-shaders")))
                {
                    File.Move(Path.Combine(selectFolder, "dxgi.dll"), Path.Combine(selectFolder, "d3d12.dll"));
                }

                // Rename the DLSS file (nvngx_dlss.dll) to nvngx.dll
                if (File.Exists(Path.Combine(selectFolder, "nvngx_dlss.dll")))
                {
                    await Task.Run(() => CopyFolder(pathOptiscaler));

                    progressBar.Value++;
                    Application.DoEvents();

                    await Task.Delay(300);

                    await Task.Run(() => File.Move(
                        Path.Combine(selectFolder, "nvngx.dll"),
                        Path.Combine(selectFolder, "dxgi.dll"),
                        true
                    ));

                    if (!gamesNoNvngx.Contains(gameSelected) || gpusVar.Contains(gpuNameSettings))
                    {
                        File.Copy(Path.Combine(selectFolder, "nvngx_dlss.dll"), Path.Combine(selectFolder, "nvngx.dll"), true);
                    }

                    progressBar.Value++;
                    Application.DoEvents();
                }
                else
                {
                    await Task.Run(() => CopyFolder(pathOptiscalerDlss));

                    if (gamesNoNvngx.Contains(gameSelected) && gpuNameSettings.Contains("rtx") && Path.Exists(Path.Combine(selectFolder, "nvngx.dll")))
                    {
                        File.Move(Path.Combine(selectFolder, "nvngx.dll"), Path.Combine(selectFolder, "nvngx_dlss.dll"));
                    }
                    progressBar.Value++;
                    Application.DoEvents();
                }

                if (selectMod == "FSR 3.1.4/DLSSG FG (Only Optiscaler)")
                {
                    File.Copy(pathOptiscalerDlssg, destPathNvapi, true);

                    if (((gpusVar.Any(gpuVar => gpuNameSettings.Contains(gpuVar)) || gpuNameSettings.Contains("rtx")) &&
                    !gamesOnlyUpscalers.Contains(gameSelected) &&
                    MessageBox.Show("Do you want to install the dlssg_to_fsr3_amd_is_better.dll file? It is recommended to install this only if you are unable to enable the game's DLSS Frame Generation (this mod does not have its own FG; the game's DLSS FG is used).",
                    "DLSS/FSR", MessageBoxButtons.YesNo) == DialogResult.Yes) ||
                    gamesWithDlssg.Contains(gameSelected))
                    {
                        File.Copy(pathDlssToFsr, Path.Combine(selectFolder, "dlssg_to_fsr3_amd_is_better.dll"), true);
                    }

                }

                if (gamesOnlyUpscalers.Contains(gameSelected))
                {
                    await Task.Delay(300);
                    File.Copy(pathIniOlyUpscalers, destPathNvapi, true);
                }

                // AMD Anti Lag 2
                if (gamesToUseAntiLag2.Contains(gameSelected) && MessageBox.Show($"Do you want to use AMD Anti Lag 2? Check the {gameSelected} guide in FSR Guide to see how to enable it.", "Anti Lag 2", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(nvapiAmd);

                    nvapiFile = selectMod == "FSR 3.1.4/DLSSG FG (Only Optiscaler)" ? nvapiAntiLagDlssg : nvapiIni;
                    File.Copy(nvapiFile, destPathNvapi, true);

                    progressBar.Value++;
                    Application.DoEvents();
                }
                // Nvapi for non-RTX users
                else if (gpusVar.Any(gpuVar => gpuNameSettings.Contains(gpuVar)))
                {
                    if (selectMod == "FSR 3.1.4/DLSSG FG (Only Optiscaler)" || gamesToInstallNvapiAmd.Contains(gameSelected) && MessageBox.Show("Do you want to install Nvapi? Only select \"Yes\" if the mod doesn't work with the default files.", "Nvapi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder(nvapiAmd);

                        nvapiFile = selectMod == "FSR 3.1.4/DLSSG FG (Only Optiscaler)" ? nvapiAntiLagDlssg : nvapiIni;

                        File.Copy(nvapiFile, destPathNvapi, true);

                        progressBar.Value++;
                        Application.DoEvents();
                    }
                }

                if (gamesWithAntiCheat.Contains(gameSelected))
                {
                    MessageBox.Show("Do not use the mod in Online mode, or you may be banned", "Anti Cheat");
                }
            }
            finally
            {
                HandleProgressBar(true, progressBar);
            }
        }
        public void UpdateUpscalers(string destPath, bool onlyDlss = false, bool copyDlssd = false, bool copyDlssDlssD = false, bool copyFsrDlss = false, bool copyDlssXess = false, bool copyDlssDlssg = false, bool copyDlssDlssdDlssg = false, string dlssgPath = null)
        {
            string pathOnlyDlss = "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll";
            string pathDlssd = "mods\\Temp\\nvngx_global\\nvngx\\Dlssd_3_7_1\\nvngx_dlssd.dll";
            string pathDlssg = "mods\\Temp\\nvngx_global\\nvngx\\Dlssg_3_7_1\\nvngx_dlssg.dll";
            string pathFsr = "mods\\Temp\\FSR_Update";
            string pathXess = "mods\\Temp\\nvngx_global\\nvngx\\libxess.dll";

            var pathUpscalers = new List<string>
            {
                "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll",
                "mods\\Temp\\nvngx_global\\nvngx\\Dlssg_3_7_1\\nvngx_dlssg.dll",
                "mods\\Temp\\nvngx_global\\nvngx\\libxess.dll"
            };

            if (onlyDlss)
            {
                HandlePrompt(
                "DLSS",
                "Do you want to update DLSS? DLSS 4 will be installed",
                _ =>
                {
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                });
            }

            else if (copyDlssDlssD)
            {
                HandlePrompt(
                "DLSS/DLSSD",
                "Do you want to update DLSS/DLSSD? DLSS 4 and DLSSD 4 will be installed.",
                _ =>
                {
                    File.Copy(pathDlssd, Path.Combine(destPath, "nvngx_dlssd.dll"), true);
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                });
            }

            else if (copyDlssDlssg)
            {
                HandlePrompt(
                "DLSS/DLSSG",
                "Do you want to update DLSS/DLSSG? DLSS 4 and DLSSG 4 will be installed.",
                _ =>
                {
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);

                    if (dlssgPath != null)
                    {
                        File.Copy(pathDlssg, Path.Combine(dlssgPath, "nvngx_dlssg.dll"), true);
                    }
                    else
                    {
                        File.Copy(pathDlssg, Path.Combine(destPath, "nvngx_dlssg.dll"), true);
                    }
                });
            }

            else if (copyDlssDlssdDlssg)
            {
                HandlePrompt(
                "DLSS/DLSSG/DLSSD",
                "Do you want to update DLSS/DLSSG/DLSSD? DLSS, DLSSG and DLSSD 4 will be installed.",
                _ =>
                {
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                    File.Copy(pathDlssd, Path.Combine(destPath, "nvngx_dlssd.dll"), true);

                    if (dlssgPath != null)
                    {
                        File.Copy(pathDlssg, Path.Combine(dlssgPath, "nvngx_dlssg.dll"), true);
                    }
                    else
                    {
                        File.Copy(pathDlssg, Path.Combine(destPath, "nvngx_dlssg.dll"), true);
                    }
                });
            }

            else if (copyFsrDlss)
            {
                HandlePrompt(
                "DLSS/DLSSD/DLSSG",
                "Do you want to update DLSS/DLSSD/DLSSG? DLSS, DLSSD and DLSSG 4 will be installed.",
                _ =>
                {
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                    File.Copy(pathDlssd, Path.Combine(destPath, "nvngx_dlssd.dll"), true);

                    if (pathDlssg != null)
                    {
                        File.Copy(pathDlssg, Path.Combine(dlssgPath, "nvngx_dlssg.dll"), true);
                    }
                    else
                    {
                        File.Copy(pathDlssg, Path.Combine(destPath, "nvngx_dlssg.dll"), true);
                    }
                });
            }

            else if (copyFsrDlss)
            {
                HandlePrompt(
                "FSR/DLSS",
                "Do you want to update DLSS/FSR? DLSS 4 and FSR 3.1.4 will be installed..",
                _ =>
                {
                    CopyFolder3(pathFsr, destPath);
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                });
            }

            else if (copyDlssXess)
            {
                HandlePrompt(
                "DLSS/XESS",
                "Do you want to update DLSS/XESS? DLSS 4 and XESS 2.0 will be installed.",
                _ =>
                {
                    File.Copy(pathDlssg, Path.Combine(destPath, "nvngx_dlssg.dll"), true);
                    File.Copy(pathXess, Path.Combine(destPath, "libxess.dll"), true);
                    File.Copy(pathOnlyDlss, Path.Combine(destPath, "nvngx_dlss.dll"), true);
                });
            }

            else if (MessageBox.Show("Do you want to update the upscalers? The latest version of all upscalers will be installed", "Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                CopyFolder3("mods\\Temp\\FSR_Update", destPath);

                foreach (var upscalersFiles in pathUpscalers)
                {
                    string upscalerName = Path.GetFileName(upscalersFiles);

                    string destinationUpscaler = Path.Combine(destPath, upscalerName);

                    File.Copy(upscalersFiles, destinationUpscaler, overwrite: true);
                }

                if (copyDlssd)
                {
                    File.Copy(pathDlssd, Path.Combine(destPath, "nvngx_dlssd.dll"), true);
                }
            }
        }

        public void gamesToUpdateUpscalers()
        {
            #region Mods/Paths
            string defaultDlssPath = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Runtime\\Nvidia\\DLSS\\Binaries\\ThirdParty\\Win64"));
            string defaultDlssgPath = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Runtime\\Nvidia\\Streamline\\Binaries\\ThirdParty\\Win64"));

            Dictionary<string, string> gamesToUpdateDlss = new Dictionary<string, string>
            {
                { "Others Mods Sifu", selectFolder},
                { "Others Mods Shadow Tomb", selectFolder},
                { "Others Mods Tlou", selectFolder},
                { "Others Mods Steel", selectFolder},
                { "Others Mods FFXVI", selectFolder},
                { "Others Mods Gow4", selectFolder},
                { "Others Mods ACE", selectFolder},
                { "Others Mods Legion", selectFolder},
                { "Others Mods AW2", selectFolder},
                { "Others Mods ATH", selectFolder},
                { "Others Mods LDPYH", selectFolder},
                { "Others Mods Greed 2", selectFolder},
                { "Others Mods GTA V", selectFolder},
                { "Others Mods CTS2", selectFolder},
                { "Others Mods Crysis", selectFolder},
                { "Others Mods WH", selectFolder},
                { "Others Mods STC", defaultDlssPath},
                { "Others Mods HB2", defaultDlssPath},
                { "Others Mods HL", defaultDlssPath},
                { "Others Mods Fist",defaultDlssPath},
                { "Others Mods GK", defaultDlssPath},
                { "Others Mods WOTH", defaultDlssPath},
                { "Others Mods 6Days", defaultDlssPath},
                { "Others Mods EW", defaultDlssPath},
                { "Others Mods TFBK", defaultDlssPath},
                { "Others GTA Trilogy", defaultDlssPath},
                { "Others Mods CCC", defaultDlssPath},
                { "Others Mods FNAF", defaultDlssPath},
                { "Others Mods Kena", defaultDlssPath},
                { "Others Mods DUTM", defaultDlssPath},
                { "Others Mods CBL", defaultDlssPath},
                { "Others Mods Chorus", defaultDlssPath},
                { "Others Mods Tot", defaultDlssPath},
                { "Others Mods Som", defaultDlssPath},
                { "Others Mods IV Oblivion", Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Marketplace\\nvidia\\DLSS\\DLSS\\Binaries\\ThirdParty"))},
                { "Others Mods Coe33", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", "Plugins\\NVIDIA\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods Brothers", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", "Brothers\\Plugins\\NVIDIA\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods PD", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", "Plugins\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods BM", Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Binaries\\ThirdParty\\NVIDIA\\NGX\\Win64"))},
                { "Others Mods Fobia", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", @"Plugins\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods PW", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", @"Plugins\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods KCD2", Path.GetFullPath(Path.Combine(selectFolder, "..", "Win64Shared"))},
                { "Others Mods LOP", Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", @"Engine\\Plugins\\Marketplace\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods FF7RBT", Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", @"Engine\\Plugins\\DLSSSubset\\Binaries\\ThirdParty\\Win6"))},
                { "Others Mods B4B", Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", @"Engine\\Binaries\\ThirdParty\\NVIDIA\\NGX\\Win64"))},
                { "Others Mods AITD", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", @"Plugins\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods GR2", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", @"Plugins\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods Remnant II", Path.GetFullPath(Path.Combine(selectFolder, "..\\..", @"Plugins\\Shared\\DLSS\\Binaries\\ThirdParty\\Win64"))},
                { "Others Mods POEII", Path.Combine(selectFolder, "Streamline") },
                { "Others Mods MShell" , Path.GetFullPath(Path.Combine(selectFolder, @"..\\..\\..\\", @"Engine\\Binaries\\ThirdParty\\NVIDIA\\NGX\\Win64"))},
            };

            Dictionary<string, string> gamesToUpdateDlssd = new Dictionary<string, string>
            {
                { "Others Mods Spider", selectFolder },
                { "Others Mods Gow Rag", selectFolder }
            };

            Dictionary<string, string[]> gamesToUpdateDlssDlssg = new Dictionary<string, string[]>
            {
                { "Others Mods Requiem", new string[] {selectFolder, null} },
                { "Others Mods TW3", new string[] {selectFolder, null} },
                { "Others Mods DL2", new string[] {selectFolder, null} },
                { "Others Mods Tlou2", new string[] {selectFolder, null} },
                { "Others Mods DUM", new string[] {defaultDlssPath, defaultDlssgPath} },
                { "Others Mods Jedi", new string[] {defaultDlssPath, defaultDlssgPath} },
                { "Others Mods Ac Shadows", new string[] {Path.Combine(selectFolder, "NVStreamline\\production"), null} },
                { "Others Mods FP2", new string[] {Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Elb\\DLSS\\Binaries\\ThirdParty\\Win64")), Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Elb\\Streamline\\Binaries\\ThirdParty\\Win64")) } },
                { "Others Mods LRBR", new string[] {Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\NvidiaDLSS\\Plugins\\DLSS\\Binaries\\ThirdParty\\Win64")), Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\NvidiaDLSS\\Plugins\\Streamline\\Binaries\\ThirdParty\\Win64"))} },
                { "Others Mods Stalker 2", new string[] {Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Marketplace\\DLSS\\Binaries\\ThirdParty\\Win64")), Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\Marketplace\\Streamline\\Binaries\\ThirdParty\\Win64"))} },
                { "Others Mods CBL2", new string[] { Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\NVIDIA\\DLSS\\Binaries\\ThirdParty\\Win64")), Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..", "Engine\\Plugins\\NVIDIA\\Streamline\\Binaries\\ThirdParty\\Win64"))} }
            };

            Dictionary<string, string[]> gamesToUpdateDlssDlssdDlssg = new Dictionary<string, string[]>
            {
                { "Others Mods Wukong", new string[] { defaultDlssPath, defaultDlssgPath } },
                { "Others Mods Indy", new string[] { Path.Combine(selectFolder, "streamline"), Path.Combine(selectFolder, "streamline") } }
            };

            Dictionary<string, string> gamesToUpdateDlssXess = new Dictionary<string, string>
            {
                { "Others Mods MHW", selectFolder }
            };

            Dictionary<string, string> gamesToUpdateFsrDlss = new Dictionary<string, string>
            {
                { "Others Mods Hitman 3", selectFolder },
                { "Others Mods Control", selectFolder},
                { "Others Mods HZD", selectFolder}
            };
            #endregion

            // Update DLSS
            if (gamesToUpdateDlss.ContainsKey(selectMod))
            {
                string pathDlss = gamesToUpdateDlss[selectMod];

                if (Path.Exists(pathDlss.ToLower()))
                {
                    UpdateUpscalers(pathDlss, true);
                }
                else
                {
                    MessageBox.Show("To update DLSS, select the .exe path", "DLSS");
                }
            }

            // Update DLSS/DLSSG/DLSSD
            else if (gamesToUpdateDlssDlssdDlssg.ContainsKey(selectMod))
            {
                string[] pathDlssDlssd = gamesToUpdateDlssDlssdDlssg[selectMod];
                string pathUpdateDlss = pathDlssDlssd[0];
                string pathUpdateDlssg = pathDlssDlssd[1];

                if (Path.Exists(pathUpdateDlss) & Path.Exists(pathUpdateDlssg))
                {
                    UpdateUpscalers(pathUpdateDlss, copyDlssDlssdDlssg: true, dlssgPath: pathUpdateDlssg);
                }
                else
                {
                    MessageBox.Show("To update DLSS/DLSSD/DLSSG, select the .exe path", "DLSS/DLSSD/DLSSG");
                }
            }

            // Update DLSS/DLSSG
            else if (gamesToUpdateDlssDlssg.ContainsKey(selectMod))
            {
                string[] pathDlssDlssg = gamesToUpdateDlssDlssg[selectMod];
                string pathUpdateDlss2 = pathDlssDlssg[0];

                string? pathUpdateDlssg2 = pathDlssDlssg.Length > 1 ? pathDlssDlssg[1] : null;

                if (Path.Exists(pathUpdateDlss2) && (pathUpdateDlssg2 == null || Path.Exists(pathUpdateDlssg2)))
                {
                    if (pathUpdateDlssg2 == null)
                    {
                        UpdateUpscalers(pathUpdateDlss2, copyDlssDlssg: true);
                    }
                    else
                    {
                        UpdateUpscalers(pathUpdateDlss2, copyDlssDlssg: true, dlssgPath: pathUpdateDlssg2);
                    }
                }
                else
                {
                    MessageBox.Show("To update DLSS/DLSSG, select the .exe path", "DLSS/DLSSG");
                }
            }

            // Update DLSS/XESS
            else if (gamesToUpdateDlssXess.ContainsKey(selectMod))
            {
                string pathDlssXess = gamesToUpdateDlssXess[selectMod];

                if (Path.Exists(pathDlssXess))
                {
                    UpdateUpscalers(pathDlssXess, copyDlssXess: true);
                }
                else
                {
                    MessageBox.Show("To update DLSS/XESS, select the .exe path", "DLSS/XESS");
                }
            }

            // Update FSR/DLSS
            else if (gamesToUpdateFsrDlss.ContainsKey(selectMod))
            {
                string pathFsrDlss = gamesToUpdateFsrDlss[selectMod];

                if (Path.Exists(pathFsrDlss))
                {
                    UpdateUpscalers(pathFsrDlss, copyFsrDlss: true);
                }
                else
                {
                    MessageBox.Show("To update FSR/DLSS, select the .exe path", "FSR/DLSS");
                }
            }

            // Update All Upscalers
            else if (gamesToUpdateDlssd.ContainsKey(selectMod))
            {
                string pathDlssd = gamesToUpdateDlssd[selectMod];

                if (Path.Exists(pathDlssd))
                {
                    UpdateUpscalers(pathDlssd, copyDlssd: true);
                }
                else
                {
                    MessageBox.Show("To update all upscalers, select the .exe path", "All Upscalers");
                }
            }
        }

        public static async Task<string> GetActiveGpu()
        {
            try
            {
                var gpuInfo = (await RunPowerShellCommandAsync("Get-CimInstance -ClassName Win32_VideoController | Select-Object Caption, DriverDate, DriverVersion, AdapterRAM"))
                    .Skip(2).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

                var gpuUsages = (await RunPowerShellCommandAsync("Get-CimInstance -ClassName Win32_PerfFormattedData_Counters_GPUUsage | Select-Object GPUTime"))
                    .Skip(2).Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => int.TryParse(line.Trim(), out var usage) ? usage : 0).ToList();

                if (!gpuInfo.Any()) return null;
                if (gpuInfo.Count == 1) return gpuInfo[0].ToLower();

                return gpuInfo[gpuUsages.IndexOf(gpuUsages.Max())].ToLower();
            }
            catch
            {
                return null;
            }
        }

        public async Task varCopyGpu(string pathRtx, string pathAmd)
        {
            await Task.Run(() =>
            {

                if (gpuNameSettings?.Contains("nvidia") == true || (gpuNameSettings is null && MessageBox.Show("Do you have an Nvidia GPU?", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    CopyFolder(pathRtx);
                }
                else
                {
                    CopyFolder(pathAmd);
                }

                runReg("mods\\Temp\\NvidiaChecks\\DisableNvidiaSignatureChecks.reg");
            });
        }

        private void dlssGlobal()
        {
            string pathRtx = "mods\\DLSS_Global\\RTX";
            string pathRtxReshade = "mods\\DLSS_Global\\RTX_Reshade";
            string pathAmd = "mods\\DLSS_Global\\AMD";
            string pathAmdReshade = "mods\\DLSS_Global\\AMD_Reshade";

            if (Path.Exists(Path.Combine(selectFolder, "reshade-shaders")))
            {
                varCopyGpu(pathRtxReshade, pathAmdReshade);
            }
            else
            {
                varCopyGpu(pathRtx, pathAmd);
            }


            runReg("mods\\Temp\\NvidiaChecks\\DisableNvidiaSignatureChecks.reg");
        }

        private void dlss_to_fsr()
        {
            string path_dlss_fsr = "mods\\DLSS_TO_FSR";

            CopyFolder(path_dlss_fsr);

            runReg("mods\\Temp\\disable signature override\\DisableSignatureOverride.reg");
        }

        public async Task rdr2Fsr3()
        {
            string rdr2Mix = "mods\\RDR2_FSR3_mix";
            string rdr2FgCustom = "mods\\FSR3_RDR2\\RDR2 FG Custom\\FG";
            string rdr2AmdIni = "mods\\FSR3_RDR2\\RDR2 FG Custom\\Amd Ini\\RDR2Upscaler.ini";
            string rdr2Optiscaler = "mods\\FSR3_RDR2\\Optiscaler_fsr_dlss";

            if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler RDR2)")
            {
                CopyFolder(rdr2Optiscaler);
            }

            if (selectMod == "RDR2 Mix")
            {
                CopyFolder(rdr2Mix);
            }

            if (selectMod == "RDR2 FG Custom")
            {
                CopyFolder(rdr2FgCustom);

                if (gpuNameSettings.Contains("amd") && Path.Exists(Path.Combine(selectFolder, "mods")))
                {
                    File.Copy(rdr2AmdIni, Path.Combine(selectFolder, "mods\\RDR2Upscaler.ini"), true);
                }
            }
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
            string originFolderJedi = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..\\SwGame"));

            if (selectMod == "Others Mods Jedi")
            {
                if (MessageBox.Show("Do you want to install Graphics Preset?", "Graphic Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(jediPreset, selectFolder + "\\STARWAR-ULTRA-REALISTA.ini", true);
                }
                if (Path.Exists(originFolderJedi + "\\Content\\Paks"))
                {
                    if (MessageBox.Show("Do you want to install fix Ray Tracing?", "Fix RT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(jediFixRt, originFolderJedi + "\\Content\\Paks\\pakchunk99-Mods_CustomMod_P.pak", true);
                    }

                    if (MessageBox.Show("Do you want to install Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(jediAntiStutter, originFolderJedi + "\\Content\\Paks\\SWJSFAI.pak", true);
                    }

                    if (Path.Exists(originFolderJedi + "\\Content\\Movies"))
                    {
                        if (MessageBox.Show("Do you want to skip the game\'s initial intro?", "intro SKip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Copy(jediIntroSkip, originFolderJedi + "\\Content\\Movies\\Default_Startup.mp4", true);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("If you want to install the other mods (Anti Stutter, Fix Rt, and Intro Skip), select the path to the game\\'s .exe file. The path should look like: Jedi Survivor\\SwGame\\Binaries\\Win64", "Path Not Found");
                }
            }
        }

        public async Task pwFSR3()
        {
            string appdataPw = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pathIniPw = Path.Combine(appdataPw, "Pal\\Saved\\Config\\WinGDK");
            string dx12IniPw = "mods\\FSR3_PW\\Dx12\\Engine.ini";

            #region FSR3 Palworld
            if (selectMod == "Palworld FG Build03")
            {
                CopyFSR(folderPw);

                if (gpuNameSettings.Contains("rtx"))
                {
                    ConfigIni2("mUpscaleType", "0", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", "Settings");
                }
                else
                {
                    ConfigIni2("mUpscaleType", "3", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", "Settings");
                }
                await Task.Delay((2000));
                {
                    File.Copy("mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", Path.Combine(selectFolder, "mods\\PalworldUpscaler.ini"), true);
                    File.Copy("mods\\FSR3_PW\\mods\\PalworldUpscaler.ini", "mods\\Temp\\FSR2FSR3_PW\\PalworldUpscaler.ini", true);
                }
            }

            try
            {
                if (Path.Exists(Path.Combine(selectFolder, "Palworld-WinGDK-Shipping.exe")))
                {
                    if (Path.Exists(pathIniPw))
                    {
                        if (!Path.Exists(Path.GetFullPath(Path.Combine(pathIniPw, "..", "Engine.ini"))))
                        {
                            Debug.WriteLine(appdataPw);
                            File.Copy(Path.Combine(pathIniPw, "Engine.ini"), Path.Combine(Path.GetFullPath(Path.Combine(pathIniPw, "..", "Engine.ini"))), true); // Engine.ini Backup
                            File.Copy(dx12IniPw, Path.Combine(pathIniPw, "Engine.ini"), true);
                        }
                    }
                    else
                    {
                        File.Copy(dx12IniPw, Path.Combine(selectFolder, "Engine.ini"), true);
                        MessageBox.Show("Unable to activate DX12 (it is required for the mod to work). Try reinstalling or copy the Engine.ini file, which was installed in the selected folder in Utility, to the following path:\"C:\\Users\\YourName\\AppData\\Local\\Pal\\Saved\\Config\\WinGDK\".", "Error");
                    }
                }
                else if (Path.Exists(Path.Combine(selectFolder, "Palworld-Win64-Shipping.exe")))
                {
                    MessageBox.Show("Check the \"Palworld\" guide in FSR Guide on how to enable DX12 (it is required for the mod to work).", "DX12");
                }
            }
            catch
            {
                File.Copy(dx12IniPw, Path.Combine(selectFolder, "Engine.ini"), true);
                MessageBox.Show("Unable to activate DX12 (it is required for the mod to work). Try reinstalling or copy the Engine.ini file, which was installed in the selected folder in Utility, to the following path:\"C:\\Users\\YourName\\AppData\\Local\\Pal\\Saved\\Config\\WinGDK\".", "Error");
            }
            #endregion
        }

        public async Task bdg3Fsr3()
        {
            string bdgV3Ini = "mods\\FSR3_BDG\\FSR3_BDG_3\\BG3Upscaler.ini";
            await Task.Run(() =>
            {
                CopyFSR(folderBdg3);
            });

            #region Copy ini file for mods folder Baldur's Gate 3 FSR3 V3 
            if (selectMod == "Baldur's Gate 3 FSR3 V3")
            {
                try
                {
                    await Task.Delay((1000));
                    {
                        if (Path.Exists(Path.Combine(selectFolder, "mods")))
                        {
                            File.Copy(bdgV3Ini, Path.Combine(selectFolder, "mods\\BG3Upscaler.ini"), true);
                        }
                    }
                }
                catch { }
            }
            #endregion
        }

        public void wukongFsr3()
        {
            string wukong_file_optimized = @"mods\FSR3_WUKONG\BMWK\BMWK - SPF\pakchunk99-Mods_CustomMod_P.pak";
            string wukongGraphicPreset = @"mods\FSR3_WUKONG\Graphic Preset\Black Myth Wukong.ini";
            string wukongUe4Map = @"mods\FSR3_WUKONG\Map\WukongUE4SS";
            string wukongMap = @"mods\FSR3_WUKONG\Map\LogicMods";
            string wukongHdr = @"mods\FSR3_WUKONG\HDR\Force_HDR_Mode_P.pak";
            string wukongCacheEnviroment = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string wukongCache = Path.Combine(wukongCacheEnviroment, "AppData");
            bool finalMessage = false;

            string fullPathWukong = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));

            if (selectMod == "DLSS FG (ALL GPUs) Wukong")
            {
                dlss_to_fsr();
            }

            if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)" || selectMod == "FSR 3.1.4/DLSSG FG (Only Optiscaler)")
            {
                if (Path.Exists(Path.Combine(wukongCache, "Local\\b1\\Saved")) && MessageBox.Show("Do you want to clear the game cache? (it may prevent possible texture errors caused by the mod)", "Cache", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (string cacheWukong in Directory.GetFiles(Path.Combine(wukongCache, "Local", "b1", "Saved"), "*.ushaderprecache"))
                    {
                        File.Delete(cacheWukong);
                    }

                }
            }

            #region Others Mods
            if (selectMod == "Others Mods Wukong")
            {
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
                        }
                        finalMessage = true;
                    }

                    // Preset
                    HandlePrompt(
                    "Preset",
                    "Do you want to apply the Graphics Preset? (ReShade must be installed for the preset to work, check the guide for more information).",
                    _ =>
                    {
                        CopyFilesCustom(Path.Combine(wukongGraphicPreset), Path.Combine(Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..")), "Black Myth Wukong.ini"), "BlackMythWukong\\b1\\Binaries\\Win64");
                    });

                    // Anti Stutter
                    HandlePrompt(
                    "Anti Stutter",
                    "Do you want to enable Anti-Stutter - High CPU Priority? (prevents possible stuttering in the game)",
                    _ =>
                    {
                        runReg("mods\\FSR3_WUKONG\\HIGH CPU Priority\\Install Black Myth Wukong High Priority Processes.reg");
                        File.Copy("mods\\FSR3_WUKONG\\HIGH CPU Priority\\Anti-Stutter - Utility.txt", selectFolder + "\\Anti-Stutter - Utility.txt", true);
                    });

                    // Mini Map
                    HandlePrompt(
                    "Mani Map",
                    "Would you like to install the mini map?",
                    _ =>
                    {
                        CopyFolder(wukongUe4Map);
                        CopyFolder2(wukongMap, Path.Combine(fullPathWukong, "b1", "Content", "Paks", "LogicMods"));
                        finalMessage = true;
                    });

                    // HDR Correction 
                    HandlePrompt(
                    "HDR",
                    "Would you like to install the HDR correction?",
                    _ =>
                    {
                        File.Copy(wukongHdr, modsPath + "\\Force_HDR_Mode_P.pak", true);
                        finalMessage = true;
                    });

                    if (finalMessage)
                        MessageBox.Show("To complete the installation, go to the game's page in your Steam library, click the gear icon 'Manage' to the right of 'Achievements', select 'Properties', and in 'Launch Options', enter -fileopenlog.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Path not found, please select the path: BlackMythWukong\\b1\\Binaries\\Win64 if you want to install additional mods (Mini Map, Anti Stuttering, etc.).", "Not Found", MessageBoxButtons.OK);
                }
            }
            #endregion
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
                    if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        runReg(gowRagAntiStutter);

                        File.Copy(gowRagAntiStutterVar, selectFolder + "\\Anti_Stutter.txt", true);
                    }

                    if (MessageBox.Show("Do you want to install the Graphics Preset?", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(gowRagPreset, selectFolder + "\\God of War Ragnarök.ini", true);
                    }

                    if (MessageBox.Show("Do you want to install the Intro Skip?", "intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder3(gowRagIntroSkip, selectFolder);
                    }

                    if (MessageBox.Show("Do you want to install the VRAM Unlocker?", "VRAM Fix", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("Do you have a 3050 or 1060 GPU?. If the game doesn\'t work, select the opposite option (if you selected \'yes\' the first time, select \'no\' so a different file will be installed)", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Copy(gowRag3050_2060, selectFolder + "\\dxgi.dll", true);
                        }
                        else
                        {
                            File.Copy(gowRagVram6gb, selectFolder + "\\dxgi.dll", true);
                        }

                        File.Copy(gowRegVramVar, selectFolder + "\\Vram.txt", true);
                    }
                }
                else
                {
                    MessageBox.Show("If you want to install the other mods (Anti Stutterr, Graphic Preset, etc.), select the path to the .exe, something like: God of War Ragnarök\\GoWR.exe", "Path Not Found");
                }
            }
            #endregion
        }

        public void dd2Fsr3()
        {
            string dinputDd2 = "mods\\FSR3_DD2\\dinput";

            if (selectMod == "Dinput8 DD2")
            {
                CopyFolder(dinputDd2);
            }
            else
            {
                MessageBox.Show("If you haven\'t installed the dinput8.dll file, check the DD2 guide in the FSR Guide for installation instructions. It is required for the mod to work", "Guide", MessageBoxButtons.OK);
            }

            if (Path.Exists(Path.Combine(selectFolder, "shader.cache2")))
            {
                if (MessageBox.Show("Do you want to remove the sharder_cache2? It is necessary for the mod to work", "Cache", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(Path.Combine(selectFolder, "shader.cache2"));
                }
            }
        }

        public void callistoFsr3()
        {
            string pathCallisto = "mods\\FSR3_Callisto\\FSR_Callisto";
            string pathTcp = "mods\\FSR3_Callisto\\Reshade\\TCP Cinematic\\TCP.ini";
            string pathRealLife = "mods\\FSR3_Callisto\\Reshade\\The Real Life\\The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini";
            string optiscalerCallisto = "mods\\FSR3_Callisto\\Optiscaler";
            string fsrCustomCallisto = "mods\\FSR3_DL2\\Custom_FSR";

            if (selectMod == "FSR 3.1.4/DLSS Custom Callisto")
            {
                CopyFolder2(fsrCustomCallisto, selectFolder);
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
                File.Copy(pathTcp, selectFolder + "\\TCP.ini", true);
            }

            DialogResult callistoRealLife = MessageBox.Show("Do you want to install the Real Life mod? (It is necessary to install ReShade for the mod to work, check the guide in FSR GUIDE for more information about the mod and how to install it.)", "Real Life", MessageBoxButtons.YesNo);

            if (DialogResult.Yes == callistoRealLife)
            {
                File.Copy(pathRealLife, selectFolder + "\\The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini", true);
            }
        }

        public async Task eldenFsr3()
        {
            string updateDlssElden = "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll";
            string updateFsrElden = "mods\\Temp\\FSR_Update";
            string[] otModsElden = { "Unlock FPS Elden", "Disable Anti Cheat" };

            string disable_eac_nightreign = "mods\\Elden_Ring_FSR3\\Nightreign FSR3\\Disable EAC\\steam_appid.txt";
            string nrss_nightreign = "mods\\Elden_Ring_FSR3\\Nightreign FSR3\\NRSS";
            string remove_vignette_aberration = "mods\\Elden_Ring_FSR3\\Nightreign FSR3\\Remove Vignette & Aberration";

            if (gameSelected == "Elden Ring")
            {

                if (folderEldenRing.ContainsKey(selectMod))
                {
                    CopyFSR(folderEldenRing);
                }

                await Task.Delay((2000));

                if (selectMod == "Unlock FPS Elden")
                {
                    CopyFolder("mods\\Elden_Ring_FSR3\\Unlock_Fps");
                }
            }
            else
            {
                CopyFolder(nrss_nightreign);
                CopyFolder(remove_vignette_aberration);
                File.Copy(disable_eac_nightreign, Path.Combine(selectFolder, "steam_appid.txt"), true);

                MessageBox.Show("It is recommended to check the guide to complete the installation (it includes additional steps, such as disabling the Anti-Cheat)", "Guide", MessageBoxButtons.OK);
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

        public void controlFsr3()
        {
            #region additional settings Control

            var HdrControlPaths = new Dictionary<string, string>
            {
                { "Steam", "mods\\FSR3_Control\\HDR\\Control HDR v1.5.1 (Steam)" },
                { "Epic", "mods\\FSR3_Control\\HDR\\Control HDR v1.5.1 (Epic Store)" },
                { "Others", "mods\\FSR3_Control\\HDR\\Control HDR v1.5.1 (No DRM)" }
            };

            string backupHdrControl = Path.Combine(selectFolder, "HDR Control");
            string pathToBackup = null;

            if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
            {
                var progressBar = HandleProgressBar(false);
                if (!Directory.Exists(backupHdrControl))
                {
                    Directory.CreateDirectory(backupHdrControl);
                }

                foreach (var store in HdrControlPaths.Keys)
                {
                    if (store != "Others" && MessageBox.Show($"Is your game on {store}?", store, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        pathToBackup = HdrControlPaths[store];
                        break;
                    }
                }

                if (pathToBackup == null)
                {
                    pathToBackup = HdrControlPaths["Others"];
                }

                var bkControlFiles = Directory.GetFiles(pathToBackup, "*.*", SearchOption.AllDirectories).ToList();
                progressBar.Maximum = bkControlFiles.Count;

                foreach (var sourceFile in bkControlFiles)
                {
                    string relativePath = Path.GetRelativePath(pathToBackup, sourceFile);
                    string destFile = Path.Combine(selectFolder, relativePath);
                    string backupFile = Path.Combine(backupHdrControl, relativePath);

                    if (File.Exists(destFile) && File.ReadAllBytes(sourceFile).SequenceEqual(File.ReadAllBytes(destFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(backupFile));
                        File.Copy(destFile, backupFile, true);
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                    File.Copy(sourceFile, destFile, true);

                    progressBar.Value++;
                    Application.DoEvents();
                }
                HandleProgressBar(true, progressBar);
            }
            #endregion
        }

        public void codFsr3()
        {
            MessageBox.Show("Do not use the mod in multiplayer, otherwise you may be banned. We are not responsible for any bans", "Ban", MessageBoxButtons.OK);

            dlssGlobal();
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

        public async void re4RemakeFsr3()
        {
            string fsrDlssRe4 = "mods\\FSR3_RE4Remake\\FSR_DLSS";

            if (selectMod == "FSR 3.1.4/DLSS RE4")
            {
                CopyFolder(fsrDlssRe4);

                if (Path.Exists(Path.Combine(selectFolder, "shader.cache2")))
                {
                    File.Delete(Path.Combine(selectFolder, "shader.cache2"));
                }
            }
        }

        public async void ldpyhFsr3()
        {
            string ldFgNvidia = "mods\\FSR2FSR3_LDPYH\\NVIDIA";
            string ldFgOthers = "mods\\FSR2FSR3_LDPYH\\Others";
            string ldNvidiaChecks = "mods\\Temp\\NvidiaChecks\\DisableNvidiaSignatureChecks.reg";

            if (selectMod == "DLSSG Yakuza")
            {
                if (gpuNameSettings.Contains("nvidia"))
                {
                    CopyFolder(ldFgNvidia);
                }
                else
                {
                    CopyFolder(ldFgOthers);
                }

                runReg(ldNvidiaChecks);
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

        public void rdr1Fsr3()
        {
            string antiStutterRdr1 = "mods\\FSR3_RDR1\\Anti Stutter\\RDR_PerformanceBoostENABLE.reg";
            string varAntiStutterRdr1 = "mods\\FSR3_SH2\\Anti_Stutter\\AntiStutter.txt";
            string textureRdr1 = "mods\\FSR3_RDR1\\4x Texture\\vfx.rpf";
            string presetRdr1 = "mods\\FSR3_RDR1\\Preset\\Red Dead Redemption.ini";
            string introSkipRdr1 = "mods\\FSR3_RDR1\\Intro Skip";
            string ds4ButtonsRdr1 = "mods\\FSR3_RDR1\\DS4";
            string unlockFpsRdr1 = "mods\\FSR3_RDR1\\Unlock FPS";

            if (selectMod.Contains("FSR 3.1.1/DLSS Optiscaler") || selectMod.Contains("FSR 3.1.2/DLSS FG Custom") || selectMod.Contains("FSR 3.1.4/DLSS FG (Only Optiscaler)"))
            {
                runReg("mods\\Temp\\NvidiaChecks\\DisableNvidiaSignatureChecks.reg");

                if (MessageBox.Show("Do you want to enable Nvidia Signature Checks.reg", "Enable", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    runReg("mods\\Temp\\NvidiaChecks\\RestoreNvidiaSignatureChecks.reg");
                }
            }

            if (selectMod == "Others Mods RDR")
            {
                // Anti Stutter
                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom4(varAntiStutterRdr1, Path.Combine(selectFolder, "AntiStutter.txt"), antiStutterRdr1);
                }

                // Preset
                if (MessageBox.Show("Do you want to install the Graphics Preset? ReShade is required to complete the mod installation. See the guide for instructions on how to install it.", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(presetRdr1, Path.Combine(selectFolder, "Red Dead Redemption 1.ini"), true);
                }

                // Unlock FPS
                if (MessageBox.Show("Do you want to install the Unlock FPS?", "Unlock FPS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(unlockFpsRdr1);
                }

                if (Path.Exists(Path.Combine(selectFolder, "game")))
                {
                    // Intro Skip
                    if (MessageBox.Show("Do you want to install the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder(introSkipRdr1);
                    }

                    // DS4 Buttons
                    if (MessageBox.Show("Would you like to install DS4 Buttons? It changes the in-game buttons to DualShock 4 buttons.", "DS4", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder(ds4ButtonsRdr1);
                    }
                }
                else
                {
                    MessageBox.Show("To install the other mods (Intro Skip and DS4 Buttons), select the correct path to the .exe file, and look for the .exe in the path \"Red Dead Redemption\\\\RDR.exe\".", "Not Found", MessageBoxButtons.OK);
                }

                // 4x Texture
                if (File.Exists(Path.Combine(selectFolder, "game\\vfx.rpf")))
                {
                    if (MessageBox.Show("Do you want to install the 4x Texture? Improves the texture to 4x its resolution.", "Texture", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Move(Path.Combine(selectFolder, "game\\vfx.rpf"), Path.Combine(selectFolder, "game\\vfx.txt"), true); // Create a backup of the vfx.rpf file and copy the vfx.rpf from the mod
                        File.Copy(textureRdr1, Path.Combine(selectFolder, "game\\vfx.rpf"), true);
                    }
                }
                else
                {
                    MessageBox.Show("Folder not found. Select the correct path if you want to install the 4x texture mod. The path is similar to common\\Red Dead Redemption 1", "Not Found", MessageBoxButtons.OK);
                }
            }
        }

        public void dgVeilFsr3()
        {
            string amdDgVeil = "mods\\DLSS_Global\\For games that have native FG\\AMD";
            string rtxDgVeil = "mods\\DLSS_Global\\For games that have native FG\\RTX";
            string antiStutterDgVeil = "mods\\FSR3_Dg_Veil\\Anti Stutter\\Install DATV High CPU Priority.reg";
            string varAntiStutterDgVeil = "mods\\FSR3_SH2\\Anti_Stutter\\AntiStutter.txt";
            string removePurpleFilterDgVeil = "mods\\FSR3_Dg_Veil\\Remove_Purple_Tones";

            if (selectMod == "FSR 3.1.4/DLSS DG Veil")
            {
                varCopyGpu(rtxDgVeil, amdDgVeil);
            }

            if (selectMod == "Others Mods DG Veil")
            {
                // Anti Stutter

                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Sttuter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom4(varAntiStutterDgVeil, Path.Combine(selectFolder, "AntiStutter.txt"), antiStutterDgVeil);
                }

                // Remove Purple Filter
                if (MessageBox.Show("Do you want to remove the purple color filter from the game?", "Purple Filter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(removePurpleFilterDgVeil);
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
                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom3(varAntiStutterUd, Path.Combine(selectFolder, "AntiStutter.txt"), "Path not found. Please try again", antiStutterUd);
                }

                // Post Processing
                if (Path.Exists(pathEngineUd))
                {
                    if (MessageBox.Show("Do you want to disable Depth of Field?", "Depth of Field", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                if (MessageBox.Show("Do you want to install the Graphics Preset?`It is recommended to view the guide before proceeding with the installation", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

            if (selectMod == "FSR 3.1.2/DLSS FG Custom" || selectMod == "Optiscaler FSR 3.1.1/DLSS" || selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
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

        public async Task sskjlFsr3()
        {
            string rootPathSskjl = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));
            string pathDxgiSskjl = Path.Combine(selectFolder, "dxgi.dll");
            string pathNvngxSskjl = Path.Combine(selectFolder, "nvngx.dll");
            string pathFolderDlssSskjl = Path.Combine(rootPathSskjl, "Engine\\Plugins\\Runtime\\Nvidia\\DLSS\\Binaries\\ThirdParty\\Win64");
            string pathDisableEac = "mods\\FSR3_SSKJL\\Disable_EAC\\EAC Bypass";

            try
            {
                if (selectMod == "Others Mods SSKJL")
                {
                    if (Path.Exists(pathFolderDlssSskjl))
                    {
                        UpdateUpscalers(pathFolderDlssSskjl, true);
                    }
                    else
                    {
                        MessageBox.Show("To update DLSS, select the path to the .exe (Stones\\Binaries\\Win64).", "DLSS", MessageBoxButtons.OK);
                    }
                }

                if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
                {
                    await Task.Delay(1000);
                    if (Path.Exists(pathDxgiSskjl))
                    {
                        File.Move(pathDxgiSskjl, Path.Combine(selectFolder, "winmm.dll"), true); // Necessary to rename the file so it won't be replaced by the Disable EAC files.
                    }

                    if (Path.Exists(pathNvngxSskjl) && gpuNameSettings.Contains("rtx"))
                    {
                        File.Delete(pathNvngxSskjl);
                    }

                    // Backup EAC
                    if (Path.Exists(Path.Combine(rootPathSskjl, "EasyAntiCheat")))
                    {
                        CopyFolder3(Path.Combine(rootPathSskjl, "EasyAntiCheat"), Path.Combine(rootPathSskjl, "Backup EAC\\EasyAntiCheat"));
                        File.Copy(Path.Combine(rootPathSskjl, "start_protected_game.exe"), Path.Combine(rootPathSskjl, "Backup EAC\\start_protected_game.exe"), true);
                    }

                    // Disable EAC
                    CopyFolder3(pathDisableEac, rootPathSskjl);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        public void b4bFsr3()
        {
            string pathDisableEac = "mods\\FSR3_SSKJL\\Disable_EAC\\EAC Bypass";
            string rootPathB4b = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));

            // Backup EAC
            if (Path.Exists(Path.Combine(rootPathB4b, "EasyAntiCheat")))
            {
                CopyFolder3(Path.Combine(rootPathB4b, "EasyAntiCheat"), Path.Combine(rootPathB4b, "Backup EAC\\EasyAntiCheat"));
                File.Copy(Path.Combine(rootPathB4b, "start_protected_game.exe"), Path.Combine(rootPathB4b, "Backup EAC\\start_protected_game.exe"), true);
            }

            // Disable EAC
            CopyFolder3(pathDisableEac, rootPathB4b);

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
                if (MessageBox.Show("Do you want to install the Graphics Preset? See the guide to learn how to complete the installation.", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(hlPreset, Path.Combine(selectFolder, "Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt"), true);

                    if (File.Exists(hlDxgiDllPath))
                    {
                        File.Copy(hlDxgiDllPath, Path.Combine(selectFolder, "dxgi.txt"), true);

                        File.Move(hlDxgiDllPath, hlD3D12DllPath);
                    }
                }

                // Anti Stutter
                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFilesCustom3(hlVarAntiStutter, Path.Combine(selectFolder, "AntiStutter.txt"), "File Not Found", hlAntiStutter);
                }
            }
        }

        public void indyFsr3()
        {
            string optiscalerIndy = "mods\\FSR3_Indy\\Optiscaler Indy";
            string smoothReshadeIndy = "mods\\FSR3_Indy\\Others Mods\\Reshade\\Normal\\TheGreatCircle smooth.ini";
            string normalReshadeIndy = "mods\\FSR3_Indy\\Others Mods\\Reshade\\Smooth\\TheGreatCircle .ini";
            string introSkipIndy = "mods\\FSR3_Indy\\Others Mods\\Intro Skip";
            string fgIndy = "mods\\FSR3_Indy\\FG\\Mod";
            string configFilePathIndy = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Saved Games\MachineGames\TheGreatCircle\base");
            string configFileIndy = "mods\\FSR3_Indy\\FG\\Config File\\TheGreatCircleConfig.local";
            string oldConfigFileIndy = Path.Combine(configFilePathIndy, "TheGreatCircleConfig.local");

            if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler Indy")
            {
                CopyFolder2(optiscalerIndy, selectFolder);
            }

            if (selectMod == "Indy FG (Only RTX)")
            {
                CopyFolder(fgIndy);

                if (Path.Exists(oldConfigFileIndy))
                {
                    File.Move(oldConfigFileIndy, Path.Combine(configFilePathIndy, "TheGreatCircleConfig.txt"), true);
                    File.Copy(configFileIndy, Path.Combine(configFilePathIndy, "TheGreatCircleConfig.local"), true);
                }
                else
                {
                    File.Copy(configFileIndy, Path.Combine(selectFolder, "TheGreatCircleConfig.local"), true);
                    MessageBox.Show("The file TheGreatCircleConfig.local was not found. Please check if it exists (C:\\Users\\YourName\\Saved Games\\MachineGames\\TheGreatCircle\\base). If it doesn\'t exist, open the game to have the file created. You can also manually copy the file to this path. The TheGreatCircleConfig.local file is in the folder selected in the Utility.", "Not Found", MessageBoxButtons.OK);
                }
            }

            if (selectMod == "Others Mods Indy")
            {
                // Intro SKip
                HandlePrompt(
                    "Intro SKip",
                    "Do you want to install the Intro Skip? Select the root folder of the game, Indiana Jones and the Great Circle.",
                    _ =>
                    {
                        CopyFolder(introSkipIndy);
                    }
                );

                // Reshade
                HandlePrompt(
                    "Smooth",
                    "Do you want to install Reshade (this is the smooth version; to install the full version, select \"No\" and choose \"Yes\" in the next window)? Check the FSR Guide for the full installation instructions.",
                    _ =>
                    {
                        File.Copy(smoothReshadeIndy, Path.Combine(selectFolder, "TheGreatCircle smooth.ini"), true);
                    }
                );

                HandlePrompt(
                    "Full",
                    "Do you want to install the full version Reshade?",
                    _ =>
                    {
                        File.Copy(normalReshadeIndy, Path.Combine(selectFolder, "TheGreatCircle .ini"), true);
                    }
                );
            }
        }

        public void stalkerFsr3()
        {
            string rootFolderStalker = Path.GetFullPath(Path.Combine(selectFolder, "..\\.."));
            string antiStutterStalker = "mods\\FSR3_Stalker2\\Anti Stutter";
            string presetStalker = "mods\\FSR3_Stalker2\\Preset";
            string dlssFgStalker = "mods\\FSR3_Stalker2\\FG DLSS";

            if (selectMod == "DLSS FG (Only RTX)")
            {
                CopyFolder(dlssFgStalker);
                runReg("mods\\Temp\\NvidiaChecks\\DisableNvidiaSignatureChecks.reg");
            }

            if (selectMod == "Others Mods Stalker 2")
            {
                if (Path.Exists(Path.Combine(rootFolderStalker, "Content\\Paks")))
                {
                    // Anti Stutter
                    if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Sutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CopyFolder2(antiStutterStalker, Path.Combine(rootFolderStalker, "Content\\Paks\\~mods"));
                    }
                }
                else
                {
                    MessageBox.Show("To install the Anti Stutter, select the path to the .exe file. The path looks like Stalker2\\\\Binaries\\\\Win64\".", "Not Found", MessageBoxButtons.OK);
                }

                // Preset
                if (MessageBox.Show("Do you want to install the Graphics Preset? Check the guide in FSR Guide to see how to perform the complete installation", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFolder(presetStalker);
                }
            }
        }
        public async Task returnalFsr3()
        {
            string rootPathReturnal = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));
            string pathDefaultFolderDlssReturnal = Path.Combine(rootPathReturnal, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NGX\\Win64");
            string pathDefaultDlssReturnal = Path.Combine(pathDefaultFolderDlssReturnal, "nvngx_dlss.dll");
            string pathDlssReturnal = "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll";
            string pathNvapiReturnal = "mods\\FSR3_Flight_Simulator24\\Amd";

            try
            {
                if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
                {
                    if (Path.Exists(pathDefaultDlssReturnal))
                    {
                        await Task.Delay(1000);
                        File.Copy(pathDefaultDlssReturnal, Path.Combine(selectFolder, "nvngx.dll"), true);

                        if (MessageBox.Show("Do you want to install nvapi.dll? Select \"Yes\" only if you are an AMD user and DLSS does not appear in the game after installing the mod.", "Nvapi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CopyFolder(pathNvapiReturnal);
                        }
                    }
                }

                if (selectMod == "Others Mods Returnal")
                {
                    // Update DLSS
                    if (Path.Exists(pathDefaultFolderDlssReturnal))
                    {
                        if (MessageBox.Show("Do you want to update DLSS? DLSS 4 will be installed", "DLSS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Copy(pathDlssReturnal, Path.Combine(pathDefaultFolderDlssReturnal, "nvngx_dlss.dll"), true);
                        }
                    }
                    else
                    {
                        MessageBox.Show("If you want to install the other Returnal mods, select the .exe folder; it should be something like \"Returnal\\\\Binaries\\\\Win64\".", "EXE", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void hb2Fsr3()
        {
            #region Others Mods Hellblade 2
            string fgRtxHb2 = "mods\\FSR3_GOT\\DLSS FG";
            string regRtxHb2 = "mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";
            string antiStutterHb2 = "mods\\FSR3_HB2\\Cpu_Hb2\\Install Hellblade 2 CPU Priority.reg";
            var postProcessingHb2 = new Dictionary<string, string>
            {
                {"r.DefaultFeature.MotionBlur", "0"},
                {"r.MotionBlurQuality", "0"},
                {"r.NT.Lens.ChromaticAberration.Intensity", "0"},
                {"r.Tonemapper.GrainQuantization", "0"},
                {"r.DepthOfFieldQuality", "0"},
                {"r.FilmGrain", "0"},
                {"r.NT.DOF.NTBokehTransform", "0"},
                {"r.NT.Lens.Distortion.Stretch", "0"},
                {"r.NT.Lens.Distortion.Intensity", "0"},
                {"r.SceneColorFringeQuality", "0"},
                {"r.NT.DOF.RotationalBokeh", "0"},
                {"r.NT.AllowAspectRatioHorizontalExtension", "0"},
                {"r.Tonemapper.Quality", "0"},
                {"r.NT.EnableConstrainAspectRatio", "0"}
            };
            string pathReplaceIniHb2 = "mods\\FSR3_HB2\\Replace_ini\\Engine.ini";
            string pathIniHb2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Hellblade2", "Saved", "Config", "Windows", "Engine.ini");
            string altPathHb2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Hellblade2", "Saved", "Config", "WinGDK", "Engine.ini");
            string pathFinal = null;

            if (selectMod == "HB2 FG (Only RTX)")
            {
                CopyFolder(fgRtxHb2);
                runReg(regRtxHb2);
            }

            if (selectMod == "Others Mods HB2")
            {
                // Remove Post Processings Effects
                if (Path.Exists(pathIniHb2))
                {
                    pathFinal = pathIniHb2;
                }
                else if (Path.Exists(altPathHb2))
                {
                    pathFinal = altPathHb2;
                }
                else
                {
                    HandlePrompt(
                    "Not Found",
                    "The Engine.ini file was not found. Would you like to manually select the path? (The path is similar to C:\\Users\\YourName\\AppData\\Local\\Hellblade2\\Saved\\Config\\Windows or WinGDK). It is required for the mod to be installed.\r\n\r\n\r\n\r\n\r\n\r\n\r\n",
                    _ =>
                    {
                        FolderBrowserDialog searchIniPathHb2 = new FolderBrowserDialog();
                        searchIniPathHb2.RootFolder = Environment.SpecialFolder.Desktop;
                        searchIniPathHb2.Description = "Select the path C:\\Users\\YourName\\AppData\\Local\\Hellblade2\\Saved\\Config\\Windows or WinGDK.";
                        searchIniPathHb2.ShowNewFolderButton = false;

                        if (searchIniPathHb2.ShowDialog() == DialogResult.OK)
                        {
                            string selectedIniPathHb2 = searchIniPathHb2.SelectedPath;

                            if (Path.Exists(Path.Combine(selectedIniPathHb2, "Engine.ini")))
                            {
                                pathFinal = Path.Combine(selectedIniPathHb2, "Engine.ini");
                            }
                            else
                            {
                                MessageBox.Show("Engine.ini not found. Please select the correct path to install the mod.", "Not Found", MessageBoxButtons.OK);
                                return;
                            }
                        }
                    });
                }

                // Anti Stutter
                HandlePrompt(
                "Anti Stutter",
                "Do you want to install the Anti Stutter?",
                _ =>
                {
                    runReg(antiStutterHb2);
                });

                if (pathFinal is not null)
                {
                    // Remove Black Bars
                    HandlePrompt(
                       "Black Bars",
                       "Do you want to remove the Black Bars?",
                       _ =>
                       {
                           ConfigIni("r.NT.EnableConstrainAspectRatio", "0", pathFinal, "SystemSettings");
                       });

                    // Remove all Post Processings Effects
                    HandlePrompt(
                        "Post Processings Effects",
                        "Do you want to remove all post-processing effects?",
                        _ =>
                        {
                            ConfigIni3(postProcessingHb2, pathFinal, "SystemSettings");
                        });

                    // Restore Post Processing
                    HandlePrompt(
                        "Restore Post Processing",
                        "Do you want to revert to the post-processing options? (Black bars, film grain, etc.)",
                        _ =>
                        {
                            File.Copy(pathReplaceIniHb2, pathFinal, true);
                        }
                    );

                }
            }
            #endregion
        }

        public void di2Fsr3()
        {
            if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
            {
                CopyFolder("mods\\FSR2FSR3_Uniscaler_V4\\Uni_V4\\Uni_Mod");
                CopyFolder("mods\\FSR3_DI2\\Uniscaler Config");
                runReg("mods\\FSR3_DI2\\TCP\\EnableNvidiaSigOverride.reg");
            }
        }

        public void legoHorizonFsr3()
        {
            string introSkipLegoHzd = "mods\\FSR3_Lego_HZD\\Intro_Skip";
            string rootPathLegoHzd = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));

            if (selectMod == "Others Mods Lego HZD")
            {
                // Intro Skip
                if (Path.Exists(Path.Combine(rootPathLegoHzd, "Glow\\Content\\Movies")))
                {
                    if (MessageBox.Show("Do you want to install the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(Path.Combine(rootPathLegoHzd, "Backup Lego HZD\\Movies"));

                        CopyFolder2(Path.Combine(rootPathLegoHzd, "Glow\\Content\\Movies"), Path.Combine(rootPathLegoHzd, "Backup Lego HZD\\Movies"));

                        CopyFolder3(introSkipLegoHzd, Path.Combine(rootPathLegoHzd, "Glow\\Content"));
                    }
                }
                else
                {
                    MessageBox.Show("To install the Intro Skip, select the folder containing the .exe file. The .exe file name is similar to \"game name-Win64-Shipping.exe\".", "Not Found");
                }
            }
        }

        public void acMirageFsr3()
        {
            string introSkipAcMirage = "mods\\FSR3_Ac_Mirage\\Intro_skip";
            string presetAcMirage = "mods\\FSR3_Ac_Mirage\\ReShade\\ACMirage lighting and package.ini";

            if (selectMod == "Others Mods Mirage")
            {
                // Intro Skip
                if (Path.Exists(Path.Combine(selectFolder, "videos")))
                {
                    if (MessageBox.Show("Do you want to install the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(Path.Combine(selectFolder, "Backup Ac Mirage"));
                        CopyFolder3(Path.Combine(selectFolder, "videos"), Path.Combine(selectFolder, "Backup Ac Mirage"));
                        CopyFolder(introSkipAcMirage);

                    }
                }
                else
                {
                    MessageBox.Show("If you want to install the Intro Skip, select the game\'s .exe folder", "Not Found");
                }

                // Preset

                if (MessageBox.Show("Do you want to install the Graphics Preset? See the game guide in FSR Guide to see how to complete the installation of the Preset", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(presetAcMirage, Path.Combine(selectFolder, "ACMirage lighting and package.ini"));
                }
            }
        }
        public bool drrFsr3()
        {
            string dlssToFsrDrr = "mods\\FSR3_DRR\\FSR3FG\\Dlss_to_Fsr";
            string dinputDrr = "mods\\FSR3_DRR\\FSR3FG\\Dinput\\dinput8.dll";
            string dlssDrr = "mods\\Temp\\nvngx_global\\nvngx\\Dlss_3_7_1\\nvngx_dlss.dll";
            string pathPluginsDrr = Path.Combine(selectFolder, "reframework\\plugins");

            if (selectMod == "Dinput8 DRR")
            {
                File.Copy(dinputDrr, Path.Combine(selectFolder, "dinput8.dll"), true);
            }

            if (selectMod == "FSR 3.1 FG DRR")
            {
                if (Path.Exists(Path.Combine(selectFolder, "reframework\\plugins")) && Path.Exists(Path.Combine(selectFolder, "dinput8.dll")))
                {
                    CopyFolder2(dlssToFsrDrr, Path.Combine(selectFolder, "reframework\\plugins"));
                    File.Copy(dlssDrr, Path.Combine(selectFolder, "nvngx_dlss.dll"), true);
                }
                else
                {
                    MessageBox.Show("First, install the \"Dinput8 DRR\" before installing the main mod. See the Dead Rising guide in the Guide to learn how to install the mod.", "Not Found");
                    return false;
                }
            }
            return true;
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
            string backupMarine = Path.Combine(selectFolder, "Backup DXGI");

            if (selectMod == "FSR 3.1.4/DLSS FG Marine")
            {
                dlssGlobal();
            }

            if (selectMod == "FSR 3.1 Space Marine")
            {
                if (Path.Exists(pathDxgiMarine))
                {
                    if (!Path.Exists(backupMarine))
                    {
                        Directory.CreateDirectory(backupMarine);
                    }

                    File.Copy(pathDxgiMarine, Path.Combine(backupMarine, "dxgi.dll"), true);

                    File.Move(pathDxgiMarine, pathRenameDxgiMarine, true);

                }

                if (gpuNameSettings.Contains("nvidia"))
                {
                    CopyFolder(pathFsr31MarineRtx);
                }
                else
                {
                    CopyFolder(pathFsr31MarineAmd);
                }
            }

            if (selectMod == "Others Mods Space Marine")
            {
                UpdateUpscalers(selectFolder, false, false, false, true);

                if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    runReg(antiStutterMarine);
                    File.Copy(txtStutterMarine, Path.Combine(selectFolder, "Marine_Anti_Stutter.txt"), true);
                }

                if (MessageBox.Show("Do you have to install the Graphic Preset?, select the path similar to: client_pc\\root\\bin\\pc for the mod to work. (It is necessary to install ReShade for the preset to work. " +
                    "See the guide in the Guide to learn how to install it.)", "Graphic Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                BackupDxgi("d3d12.dll", selectFolder + "\\dxgi.dll", "dxgi.dll");
            }


            if (selectMod == "Others Mods FFXVI")
            {
                if (Path.Exists(Path.Combine(selectFolder, "ffxvi.exe")))
                {
                    if (MessageBox.Show("Do you want to install the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        runReg(ffxviAntiStutter);
                        File.Copy(ffsxiVarAntiStutter, selectFolder + "\\Anti_Stutter.txt", true);
                    }

                    if (MessageBox.Show("Do you want to install the fixes mod? (It unlocks FPS in cutscenes, adds ultrawide support, etc. See all fixes in the Guide)", "FFXVI FIX", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
            string varStutterOutlaws = "mods\\FSR3_Outlaws\\Anti_Stutter\\Anti_Sttuter.txt";

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
            if (folderGtaV.ContainsKey(selectMod))
            {
                if (MessageBox.Show("We are not responsible for any bans that may occur when using the mod in Online mode. Do you wish to continue with the installation?", "Ban", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CopyFSR(folderGtaV);
                }
                else
                {
                    return;
                }
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
                await Task.Delay((2000));//Delay to rename files, to avoid the possibility of renaming before the files are copied
                File.Move(selectFolder + "\\dxgi.asi", selectFolder + "\\d3d12.dll", true);
                File.Move(selectFolder + "\\GTAVUpscaler.asi", selectFolder + "\\GTAVUpscaler.dll", true);
                File.Move(selectFolder + "\\GTAVUpscaler.org.asi", selectFolder + "\\GTAVUpscaler.org.dll", true);

                File.Copy(selectFolder + "\\d3d12.dll", selectFolder + "\\mods\\d3d12.dll", true);
                File.Copy(selectFolder + "\\GTAVUpscaler.dll", selectFolder + "\\mods\\GTAVUpscaler.dll", true);
                File.Copy(selectFolder + "\\GTAVUpscaler.org.dll", selectFolder + "\\mods\\GTAVUpscaler.org.dll", true);
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
            string pathGhostFix2077 = "mods\\FSR3_CYBER2077\\mods\\FrameGen Ghosting Fix";
            string pathDisableVignette2077 = "mods\\FSR3_CYBER2077\\mods\\Disable Vignette and Sharpening";
            string pathXessNvngx2077 = "mods\\FSR3_CYBER2077\\Xess_FSR_FG\\XESS Upscaler\\nvngx.ini";
            string pathFgAmd2077 = "mods\\FSR3_CYBER2077\\Xess_FSR_FG\\AMD";
            string pathFgNvidia2077 = "mods\\FSR3_CYBER2077\\Xess_FSR_FG\\Nvidia";
            string originPathCb2077 = Path.GetFullPath(Path.Combine(selectFolder, "..\\.."));
            #endregion

            if (selectMod == "RTX DLSS FG CB2077")
            {
                CopyFSR(folderCb2077);
                runReg(path_cb2077_over);
            }

            if (selectMod == "FSR 3.1.4/XESS FG 2077")
            {
                if (gpuNameSettings.Contains("amd") || gpuNameSettings.Contains("intel"))
                {
                    CopyFolder(pathFgAmd2077);
                }
                else
                {
                    CopyFolder(pathFgNvidia2077);
                }

                if (MessageBox.Show("Do you want to use XESS as the upscaler? The default is FSR 3.1.4.", "XESS/FSR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Copy(pathXessNvngx2077, Path.Combine(selectFolder, "nvngx.ini"), true);
                }
            }

            if (selectMod == "Others Mods 2077")
            {
                UpdateUpscalers(selectFolder, false, true);

                if (Path.Exists(originPathCb2077 + "\\bin"))
                {

                    // Nova Lut
                    HandlePrompt(
                        "Nova Lut",
                        "Do you want to install Nova LUT 2-1 and HD Reworked for Cyberpunk 2077?",
                        _ =>
                        {
                            foreach (string modsPathCb2077 in pathModsCb2077)
                            {
                                CopyFolder3(modsPathCb2077, originPathCb2077);
                            }
                            ;
                        }
                    );

                    // Real Life 2.0
                    HandlePrompt(
                        "Real Life 2.0",
                        "Do you want to install Reshade Real Life 2.0? (It is necessary to install Reshade for this mod to work. Please refer to the Guide for installation instructions.)",
                        _ =>
                        {
                            CopyFolder3("mods\\FSR3_CYBER2077\\mods\\V2.0 Real Life Reshade", originPathCb2077);
                        }
                    );

                    // FG Ghost Fix
                    HandlePrompt(
                        "FG Ghost Fix",
                        "Do you want to install the FG Ghost Fix? Only if you are using the FSR 3.1.4/XESS FG 2077 mod.",
                        _ =>
                        {
                            CopyFolder3(pathGhostFix2077, originPathCb2077);
                        }
                    );

                    // Disable Vignette
                    HandlePrompt(
                        "Disable Vignette",
                        "Do you want to remove the vignette from the game? This mod removes the black vignette that appears in the corners of the screen.",
                        _ =>
                        {
                            CopyFolder3(pathDisableVignette2077, originPathCb2077);
                        }
                    );
                }
                else
                {
                    MessageBox.Show("If you want to install the others mods (Nova Lut, Real Life and Ultra Realistic Textures), select the path to the .exe, it should be something like: Cyberpunk 2077/bin/x64", "Others Mods", MessageBoxButtons.OK);
                }
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

        public void mhwFsr3()
        {
            string dlssgRtxMhw = "mods\\FSR3_Wilds\\DLSSG RTX";

            if (selectMod == "DLSSG Wilds (Only RTX)")
            {
                CopyFolder(dlssgRtxMhw);
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

            if (selectMod == "DLSS FG LOTF2 (RTX)")
            {

                dlss_to_fsr();

                string disableEacLotf2 = """
                @echo off
                set SteamAppId=1501750
                set SteamGameId=1501750
                start LOTF2-Win64-Shipping.exe -DLSSFG
                """;

                File.WriteAllText(Path.Combine(selectFolder, "rungame.bat"), disableEacLotf2);

                MessageBox.Show("Run the game using the \"rungame.bat\" file that was created in the mod installation folder. It is recommended to check the guide if you need more information", "Guide");
            }
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

                    string[] folderModsReshade = { "Dragons Dogma 2 ", "Palworld" };
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
        public bool CleanupMod3(List<string> ListClean, string modName)
        {
            string[] DelFiles = Directory.GetFiles(selectFolder);
            bool deletedAnyFile = false;
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
                            deletedAnyFile = true;
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
            return deletedAnyFile;
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

                if (selectMod == "FSR 3.1.2/DLSS FG Custom")
                {
                    dlssGlobal();
                }
                if (modsToInstallOptiscalerFsrDlss.Contains(selectMod))
                {
                    optiscalerFsrDlss();
                }
                if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
                {
                    optiscaler_custom();
                }
                if (gameSelected == "Elden Ring" || gameSelected == "Elden Ring Nightreign")
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
                if (gameSelected == "Like a Dragon: Pirate Yakuza in Hawaii")
                {
                    ldpyhFsr3();
                }
                if (gameSelected == "Monster Hunter Wilds")
                {
                    mhwFsr3();
                }
                if (gameSelected == "Ghost of Tsushima")
                {
                    gotFsr3();
                }
                if (gameSelected == "Monster Hunter Wilds")
                {
                    mhwFsr3();
                }
                if (gameSelected == "The Medium")
                {
                    mediumFsr3();
                }
                if (gameSelected == "Lords of the Fallen")
                {
                    lotfFsr3();
                }
                if (gameSelected == "Star Wars: Jedi Survivor")
                {
                    jediFsr3();
                }
                if (gameSelected == "Palworld")
                {
                    pwFSR3();
                }
                if (selectMod == "Unlock FPS Tekken 8")
                {
                    tekkenFsr3();
                    MessageBox.Show("Run TekkenOverlay.exe for the mod to work, the .exe is in the folder where the mod was installed", "Run Overlay", MessageBoxButtons.OK);
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
                if (gameSelected == "Indiana Jones and the Great Circle")
                {
                    indyFsr3();
                }
                if (gameSelected == "Dragon Age: Veilguard")
                {
                    dgVeilFsr3();
                }
                if (gameSelected == "Red Dead Redemption")
                {
                    rdr1Fsr3();
                }
                if (gameSelected == "Metro Exodus Enhanced Edition")
                {
                    metroFsr3();
                }
                if (gameSelected == "A Quiet Place: The Road Ahead")
                {
                    quietPlaceFsr3();
                }
                if (gameSelected == "Resident Evil 4")
                {
                    re4RemakeFsr3();
                }
                if (gameSelected == "Hogwarts Legacy")
                {
                    hogLegacyFsr3();
                }
                if (gameSelected == "Suicide Squad: Kill the Justice League")
                {
                    sskjlFsr3();
                }
                if (gameSelected == "Star Wars Outlaws")
                {
                    outlawsFsr3();
                }
                if (gameSelected == "Lego Horizon Adventures")
                {
                    legoHorizonFsr3();
                }
                if (gameSelected == "Stalker 2")
                {
                    stalkerFsr3();
                }
                if (gameSelected == "Returnal")
                {
                    returnalFsr3();
                }
                if (gameSelected == "Senua's Saga: Hellblade II")
                {
                    hb2Fsr3();
                }
                if (gameSelected == "Dead Island 2")
                {
                    di2Fsr3();
                }
                if (gameSelected == "Red Dead Redemption 2")
                {
                    rdr2Fsr3();
                }
                if (gameSelected == "Assassin's Creed Mirage")
                {
                    acMirageFsr3();
                }
                if (gameSelected == "Dragons Dogma 2")
                {
                    dd2Fsr3();
                }
                if (gameSelected == "God of War Ragnarök")
                {
                    gowRagFsr3();
                }

                if (gameSelected == "Dead Rising Remaster")
                {
                    if (!drrFsr3())
                    {
                        return;
                    }
                }

                if (folderGtaV.ContainsKey(selectMod))
                {
                    gtavFsr3();
                }

                gamesToUpdateUpscalers();

                selectMod = listMods.SelectedItem as string;

                if (selectMod != null)
                {
                    if (folderFakeGpu.ContainsKey(selectMod))
                    {
                        CopyToml();
                    }
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
                                string pathIni = "mods\\Temp\\OptiScaler\\OptiScaler.ini";
                                File.Copy(optFile, fullPath, true);
                                File.Copy(pathIni, selectFolder + "\\nvngx.ini", true);
                            }
                            string oldIni = "mods\\Temp\\OptiScaler\\OptiScaler.ini";
                            string newIni = "mods\\Addons_mods\\OptiScaler\\OptiScaler.ini";
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

        public async Task CleanDlssGlobal(string modName)
        {
            if (gpuNameSettings.Contains("nvidia") || (gpuNameSettings is null && MessageBox.Show("Do you have an Nvidia GPU?", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                CleanupMod3(del_dlss_global_rtx, modName);
            }
            else
            {
                CleanupMod3(del_dlss_global_amd, modName);
            }

            if (Path.Exists(Path.Combine(selectFolder, "reshade-shaders")) && Path.Exists(Path.Combine(selectFolder, "d3d12.dll")))
            {
                File.Delete(Path.Combine(selectFolder, "d3d12.dll"));
            }

            runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");

            RestoreBackup("Backup Dlss");
        }

        #region Cleanup Others Mods
        public bool CleanupOthersMods(string modName, string fileName, string destPath, string regPath = null)
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

        #region Cleanup Others Mods 3
        public void CleanupOthersMods3(string modName, string[] fileNames, string destPath, bool viewMessage = true, string delFolder = null)
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
                foreach (string filePath in filesToRemove)
                {
                    File.Delete(filePath);
                    filesRemoved = true;
                }
            }

            if (delFolder is not null && Path.Exists(Path.Combine(destPath, delFolder)))
            {
                Directory.Delete(Path.Combine(destPath, delFolder), true);
            }

            if (viewMessage)
            {
                MessageBox.Show("Mod removed successfully", "Sucess");
            }

        }
        #endregion

        #region Cleanup Optiscaler FSR DLSS
        public bool CleanupOptiscalerFsrDlss(List<string> modList, string modName, bool removeDxgi = false, string searchFolderName = null)
        {
            try
            {
                string[] removeDlss = ["Senua's Saga: Hellblade II"];

                if (selectMod == modName && MessageBox.Show($"Do you want to remove the {modName}?", "Cleanup", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string nvngxPath = Path.Combine(selectFolder, "nvngx.dll");
                    string nvngxDlssPath = Path.Combine(selectFolder, "nvngx_dlss.dll");

                    if (File.Exists(nvngxPath))
                    {
                        if (!removeDlss.Contains(gameSelected))
                        {
                            File.Move(nvngxPath, nvngxDlssPath, true); // Reverts the original nvngx_dlss.dll file
                        }
                        else
                        {
                            File.Delete(nvngxPath);
                        }
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing FSR 3.1.4/DLSS FG (Only Optiscaler) mod files, please try again or do it manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
            return false;
        }
        #endregion

        private async void buttonDel_Click(object sender, EventArgs e)
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

                    CleanupMod2(delOptiscaler, folderFakeGpu, "Mod Successfully Removed");
                }

                if (selectMod == "Optiscaler FSR 3.1.1/DLSS")
                {
                    CleanupMod3(delOptiscalerCustom, "Optiscaler FSR 3.1.1/DLSS");
                    runReg("mods\\Addons_mods\\OptiScaler\\EnableSignatureOverride.reg");

                    RestoreBackup("Backup Optiscaler");
                }

                if (selectMod == "FSR 3.1.2/DLSS FG Custom")
                {
                    CleanDlssGlobal("FSR 3.1.2/DLSS FG Custom");
                }

                if (modsToInstallOptiscalerFsrDlss.Contains(selectMod))
                {
                    if (CleanupOptiscalerFsrDlss(delOptiscaler, selectMod, true))
                    {
                        MessageBox.Show("Mods removed successfully", "Sucess");
                    }
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

                    if (selectMod == "FSR 3.1.4/XESS FG 2077")
                    {
                        if (gpuNameSettings.Contains("amd") || gpuNameSettings.Contains("intel"))
                        {
                            CleanupMod3(del_dlss_global_amd, "FSR 3.1.4/XESS FG 2077");
                        }
                        else
                        {
                            CleanupMod3(del_dlss_global_rtx, "FSR 3.1.4/XESS FG 2077");
                        }
                    }

                    // Enable Viganette
                    CleanupOthersMods("Disable Vignette", "DisableVignetteAndSharpening.archive", Path.Combine(rootPathCb2077, "archive\\pc\\mod"));

                    //FG Ghost Fix
                    CleanupOthersMods("FG Ghost Fix", "framegenghostingfix_16_9.archive", Path.Combine(rootPathCb2077, "archive\\pc\\mod"));

                    // Remove the folder with HD Rework and Nova Lut
                    if (File.Exists(rootPathCb2077 + "\\archive\\pc\\mod\\HD Reworked Project.archive"))
                    {
                        if (MessageBox.Show("Would you like to remove the HD Reworked,Nova Lut 2-1 mods and Cyberpunk UltraPlus?", "Mods", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                        if (MessageBox.Show("Would you like to remove the V2.0 Real Life Reshade?", "Mods", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(rootPathCb2077 + "\\bin\\x64\\V2.0 Real Life Reshade.ini");
                            Directory.Delete(rootPathCb2077 + "\\bin\\x64\\reshade-shaders", true);
                        }
                    }
                    #endregion
                }

                if (gameSelected == "The Callisto Protocol")
                {
                    #region Remove others mods

                    CleanupOthersMods("TCP", "TCP.ini", selectFolder);

                    CleanupOthersMods("Real Life", "The Real Life The Callisto Protocol Reshade BETTER TEXTURES and Realism 2022.ini", selectFolder);

                    CleanupMod3(delOptiscalerCustom2, "FSR 3.1.4/DLSS Custom Callisto");
                    #endregion
                }

                if (gameSelected == "Metro Exodus Enhanced Edition")
                {
                    #region Remove Others Mods Metro

                    CleanupOthersMods("Graphics Preset", "DefinitiveEdition.ini", selectFolder);

                    #endregion
                }

                if (gameSelected == "A Plague Tale Requiem")
                {
                    #region Remove Custom Mods Requiem
                    if (selectMod == "FSR 3.1.1 Custom Requiem")
                    {
                        CleanupMod3(delOptiscaler, "FSR 3.1.1 Custom Requiem");
                    }
                    #endregion
                }

                if (gameSelected == "Senua's Saga: Hellblade II")
                {
                    #region Remove Others Mods Hellblade 2
                    string removeAntiStutterHb2 = "mods\\FSR3_HB2\\Cpu_Hb2\\Uninstall Hellblade 2 CPU Priority.reg";

                    if (selectMod == "HB2 FG (Only RTX)")
                    {
                        CleanupMod3(del_dlss_to_fsr, "HB2 FG (Only RTX)");
                    }

                    if (Path.Exists(Path.Combine(selectFolder, "Install Hellblade 2 CPU Priority.reg")))
                    {
                        HandlePrompt(
                            "Anti Stutter",
                            "Do you want to remove the Anti Stutter",
                            _ =>
                            {
                                runReg(removeAntiStutterHb2);
                                File.Delete(Path.Combine(selectFolder, "Install Hellblade 2 CPU Priority.reg"));
                            });
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

                if (gameSelected == "Suicide Squad: Kill the Justice League" || gameSelected == "Back 4 Blood")
                {
                    try
                    {
                        string sskjlRootPath = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));

                        if (Path.Exists(Path.Combine(sskjlRootPath, "Backup EAC")))
                        {
                            if (MessageBox.Show("Do you want to enable EAC (Easy Anti-Cheat)?", "EAC", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Directory.Delete(Path.Combine(sskjlRootPath, "EasyAntiCheat"), true);
                                CopyFolder3(Path.Combine(sskjlRootPath, "Backup EAC"), sskjlRootPath);
                            }
                            Directory.Delete(Path.Combine(sskjlRootPath, "Backup EAC"), true);
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"Error clearing {gameSelected} mods files, please try again or do it manually", "Error", MessageBoxButtons.OK);
                    }
                }

                if (gameSelected == "Resident Evil 4")
                {
                    #region Cleanup Mods Resident Evil 4 Remake                 

                    if (CleanupOthersMods("FSR 3.1.4/DLSS RE4", "dinput8.dll", selectFolder))
                    {
                        Directory.Delete(Path.Combine(selectFolder, "reframework"), true);
                    }

                    #endregion
                }

                if (gameSelected == "Warhammer: Space Marine 2")
                {
                    #region Del Mods Warhammer: Space Marine 2

                    if (gpuNameSettings.Contains("nvidia"))
                    {
                        CleanupMod3(del_dlss_global_rtx, "FSR 3.1.4/DLSS FG Marine");
                    }
                    else
                    {
                        CleanupMod3(del_dlss_global_amd, "FSR 3.1.4/DLSS FG Marine");
                    }

                    if (selectMod == "FSR 3.1 Space Marine")
                    {
                        if (gpuNameSettings.Contains("nvidia"))
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
                        if (MessageBox.Show("Do you want to remove the Anti Stutter?", "Anti Stutter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            runReg("mods\\FSR3_Outlaws\\Anti_Stutter\\Uninstall Star Wars Outlaws CPU Priority.reg");
                        }
                    }
                    #endregion
                }

                if (gameSelected == "Stalker 2")
                {
                    #region Cleanup Others Mods Stalker 2
                    string rootstalker = Path.GetFullPath(Path.Combine(selectFolder, "..\\.."));
                    string removeWinmmStalker = Path.Combine(selectFolder, "winmm.dll");

                    CleanupOthersMods("Anti Stutter", "~S2optimizedTweaksBASE_v1.31_P.pak", Path.Combine(rootstalker, "Content\\Paks\\~mods"));

                    if (CleanupOptiscalerFsrDlss(delOptiscalerCustom, "DLSS FG (Only Nvidia)", true))
                    {
                        if (Path.Exists(removeWinmmStalker)) File.Delete(removeWinmmStalker);
                    }
                    #endregion
                }

                if (gameSelected == "Red Dead Redemption")
                {
                    #region Cleanup Others Mods Red Dead Redemption
                    try
                    {
                        string removeAntiStutterRdr1 = "mods\\FSR3_RDR1\\Anti Stutter\\RDR_PerformanceBoostDISABLE.reg";
                        string[] delUnlockFpsRdr = { "dinput8.dll", "SUWSF.asi", "SUWSF.ini" };

                        if (selectMod == "Others Mods RDR")
                        {
                            // Anti Stutter
                            CleanupOthersMods("Anti Stutter", "AntiStutter.txt", selectFolder, removeAntiStutterRdr1);

                            // Intro Skip
                            CleanupOthersMods("Intro Skip", "tune_d11generic.rpf", Path.Combine(selectFolder, "game"));

                            //DS4 Buttons
                            CleanupOthersMods("DS4 Buttons", "fonts.rpf", Path.Combine(selectFolder, "game"));

                            // Unlock FPS 
                            CleanupOthersMods3("Unlock FPS", delUnlockFpsRdr, selectFolder, false);

                            // Preset
                            if (Path.Exists(Path.Combine(selectFolder, "Red Dead Redemption 1.ini")) && MessageBox.Show("Do you want to remove the Graphics Preset? It is necessary to remove the ReShade files to completely uninstall it", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                File.Delete(Path.Combine(selectFolder, "Red Dead Redemption 1.ini"));
                            }

                            // 4x Texture
                            if (Path.Exists(Path.Combine(selectFolder, "game\\vfx.txt")) && MessageBox.Show("Do you want to remove the 4x Texture?", "Texture", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                File.Move(Path.Combine(selectFolder, "game\\vfx.txt"), Path.Combine(selectFolder, "game\\vfx.rpf"), true);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error clearing Red Dead Redemption files, please try again or do it manually", "Error", MessageBoxButtons.OK);
                    }
                    #endregion
                }

                if (gameSelected == "Red Dead Redemption 2")
                {
                    #region Cleanup Others Mods Rdr2

                    CleanupMod3(del_rdr2_custom_files, "RDR2 Mix");
                    CleanupMod3(del_rdr2_custom_files, "RDR2 FG Custom");
                    if (CleanupOptiscalerFsrDlss(delOptiscaler, "FSR 3.1.4/DLSS FG (Only Optiscaler RDR2)"))
                    {
                        if (Path.Exists(Path.Combine(selectFolder, "winmm.dll")))
                        {
                            File.Delete(Path.Combine(selectFolder, "winmm.dll"));
                        }
                        MessageBox.Show("Mods removed successfully", "Sucess");
                    }
                    #endregion
                }

                if (gameSelected == "Palworld")
                {
                    string appDataPw = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string pathIniPw = Path.Combine(appDataPw, "Pal\\Saved\\Config\\WinGDK");

                    if (folderPw.ContainsKey(selectMod))
                    {
                        CleanupMod(del_pw_files, folderPw);
                    }

                    if (Path.Exists(Path.GetFullPath(Path.Combine(pathIniPw, "..", "Engine.ini"))))
                    {
                        File.Move(Path.GetFullPath(Path.Combine(pathIniPw, "..", "Engine.ini")), Path.Combine(pathIniPw, "Engine.ini"), true);
                    }
                }

                if (gameSelected == "TEKKEN 8")
                {
                    #region Cleanup Unlock FPS Tekken 8
                    string[] delUnlockFpsTekken8 = { "TekkenOverlay.exe", "Tekken8Overlay.dll", "Tekken7Overlay.dll" };

                    CleanupOthersMods3("Unlock FPS Tekken 8", delUnlockFpsTekken8, selectFolder);
                    #endregion
                }

                if (gameSelected == "Star Wars: Jedi Survivor")
                {
                    #region Cleanup others mods Jedi Survivor
                    string rootPathJedi = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\..\\SwGame"));

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
                        if (MessageBox.Show("Do you want to remove the FFXVI FIX?", "Remove FFXVI FIx", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach (string ffxviFixFiles in Directory.GetFiles(selectFolder))
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

                    if (File.Exists(Path.Combine(selectFolder, "BackupDxgi\\dxgi.dll")))
                    {
                        File.Copy(Path.Combine(selectFolder, "BackupDxgi\\dxgi.dll"), selectFolder + "\\dxgi.dll", true);

                        Directory.Delete(Path.Combine(selectFolder, "BackupDxgi"), true);

                        if (File.Exists(Path.Combine(selectFolder, "d3d12.dll")))
                        {
                            File.Delete(Path.Combine(selectFolder, "d3d12.dll"));
                        }
                    }

                    #endregion
                }

                if (gameSelected == "Lego Horizon Adventures")
                {
                    #region Cleanup Others Mods Lego HZD
                    string rootLegoHzd = Path.GetFullPath(Path.Combine(selectFolder, "..\\..\\.."));

                    try
                    {
                        if (Path.Exists(Path.Combine(rootLegoHzd, "Backup Lego HZD")))
                        {
                            if (Path.Exists(Path.Combine(rootLegoHzd, "Glow\\Content\\Movies")))
                            {
                                if (MessageBox.Show("Do you want to remove the Intro Skip?", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Directory.Delete(Path.Combine(rootLegoHzd, "Glow\\Content\\Movies"), true);
                                    CopyFolder3(Path.Combine(rootLegoHzd, "Backup Lego HZD"), Path.Combine(rootLegoHzd, "Glow\\Content"));
                                    Directory.Delete(Path.Combine(rootLegoHzd, "Backup Lego HZD"), true);
                                }
                            }
                            else
                            {
                                MessageBox.Show("To remove the Intro Skip, select the folder containing the .exe file. The .exe file name is similar to \"game name-Win64-Shipping.exe\"", "Exe", MessageBoxButtons.OK);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error clearing Lego Horizon Adventures mods files, please try again or do it manually", "Error");
                    }
                    #endregion
                }

                if (gameSelected == "Dead Island 2")
                {
                    #region Cleanup Others Mods DI2
                    if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)")
                    {
                        CleanupOthersMods3("FSR 3.1.4/DLSS FG (Only Optiscaler)", delTcp, selectFolder);
                        runReg("mods\\FSR3_DI2\\TCP\\DisableNvidiaSigOverride.reg");
                    }
                    #endregion
                }

                if (gameSelected == "Returnal")
                {
                    #region Cleanup Others Mods Returnal
                    if (selectMod == "FSR 3.1.4/DLSS FG (Only Optiscaler)" && Path.Exists(Path.Combine(selectFolder, "nvapi64.dll")))
                    {
                        File.Delete(Path.Combine(selectFolder, "nvapi64.dll"));
                    }
                    #endregion
                }

                if (gameSelected == "Dragon Age: Veilguard")
                {
                    #region Cleanup Others Mods Dg Veil
                    string removeAntiStutterDgVeil = "mods\\FSR3_Dg_Veil\\Anti Stutter\\Uninstall DATV High CPU Priority.reg";
                    string[] restorePurpleFilter = { "ReShade.ini", "dxgi.dll", "Dark_Fantasy_LUT.ini" };

                    if (selectMod == "FSR 3.1.4/DLSS DG Veil")
                    {
                        CleanDlssGlobal("FSR 3.1.4/DLSS DG Veil");
                    }

                    if (selectMod == "Others Mods DG Veil")
                    {

                        CleanupOthersMods("Others Mods DG Veil", "AntiStutter.txt", selectFolder, removeAntiStutterDgVeil);

                        if (CleanupOthersMods2("Others Mods DG Veil", restorePurpleFilter, selectFolder, "Do you want to restore the Purple Filter?"))
                        {
                            Directory.Delete(Path.Combine(selectFolder, "reshade-shaders"), true);
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
                        string[] rtxCustomFilesSh2 = { "amd_fidelityfx_dx12.dll", "amd_fidelityfx_vk.dll", "dlss-enabler.dll", "dxgi.dll", "libxess.dll", "nvngx.ini" };
                        string[] delDlssSh2 = { "dxgi.dll", "ReShade.ini", "SH2UpscalerPreset.ini" };

                        if (selectMod == "DLSS FG RTX")
                        {
                            if (CleanupOthersMods2("DLSS FG RTX", delDlssSh2, selectFolder, "Do you want to remove the DLSS FG RTX?"))
                            {
                                if (Path.Exists(Path.Combine(selectFolder, "mods")) && Path.Exists(Path.Combine(selectFolder, "reshade-shaders")))
                                {
                                    Directory.Delete(Path.Combine(selectFolder, "mods"), true);
                                    Directory.Delete(Path.Combine(selectFolder, "reshade-shaders"), true);
                                }
                            }
                        }

                        if (selectMod.Contains("Ultra Plus Complete") || selectMod.Contains("Ultra Plus Optimized"))
                        {
                            CleanupOthersMods("Ultra Plus Complete", "~UltraPlus_v0.8.0_P.pak", pathContentSh2);
                            CleanupOthersMods("Ultra Plus Optimized", "~UltraPlus_v1.0.4_P.pak", pathContentSh2);

                            if (Path.Exists(folderEngineIniSh2))
                            {
                                File.Copy(defaultEngineIniSh2, Path.Combine(folderEngineIniSh2, "Engine.ini"), true);
                            }
                        }

                        if (CleanupOthersMods("Ray Reconstruction", "nvngx_dlssd.dll", pathDlssDllSh2))
                        {
                            if (Path.Exists(engineIniSh2))
                            {
                                File.Delete(engineIniSh2);
                                File.Copy(defaultEngineIniSh2, engineIniSh2);
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
                                if (Path.Exists(Path.Combine(selectFolder, "DXD12.dll")) && MessageBox.Show("Do you have an RX 500/5000 or GTX?", "GPU", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                                if (MessageBox.Show("Do you want to enable Depth of Field?", "Depth of Field", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    ConfigIni("r.DepthOfFieldQuality", "1", enginePathUd, "SystemSettings");
                                    File.Delete(Path.Combine(selectFolder, "PostProcessing.txt"));

                                    MessageBox.Show("Depth of field activated successfully", "Depth of Field");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Path not found. The path to the Engine.ini file is something like this: Documents\\My Games\\Bates\\Saved\\Config\\Windows.", "Not Found");
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
                        CleanupMod3(delOptiscaler, "FSR 3.1.1/DLSS Quiet Place");
                        runReg("mods\\Temp\\disable signature override\\DisableSignatureOverride.reg");
                    }
                    #endregion
                }

                if (gameSelected == "Dead Rising Remaster")
                {
                    #region Cleanup Custom Mos Drr
                    string[] delDlssFgDrr = { "dlssg_to_fsr3_amd_is_better.dll", "version.dll" };
                    if (Path.Exists(Path.Combine(selectFolder, "reframework\\plugins\\dlssg_to_fsr3_amd_is_better.dll")))
                    {
                        CleanupOthersMods3("FSR 3.1 FG DRR", delDlssFgDrr, Path.Combine(selectFolder, "reframework\\plugins"), false);

                        if (Path.Exists(Path.Combine(selectFolder, "dinput8.dll")))
                        {
                            File.Delete(Path.Combine(selectFolder, "dinput8.dll"));
                        }
                    }
                    #endregion
                }

                if (gameSelected == "Assassin's Creed Mirage")
                {
                    #region Cleanup Mod Custom Ac Mirage

                    try
                    {
                        if (Path.Exists(Path.Combine(selectFolder, "Backup Ac Mirage")))
                        {
                            if (MessageBox.Show("Do you want to remove the Intro Skip", "Intro Skip", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                CopyFolder2(Path.Combine(selectFolder, "Backup Ac Mirage"), Path.Combine(selectFolder, "videos"));
                                Directory.Delete(Path.Combine(selectFolder, "Backup Ac Mirage"), true);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error clearing Assassin\'s Creed Mirage mods files, please try again or do it manuall", "Error");
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
                        if (MessageBox.Show("Do you want to remove the Graphic Preset file and restore the dxgi.dll file? To completely remove the Preset, it's necessary to remove the remaining files via ReShade.", "Preset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(Path.Combine(selectFolder, "Hogwarts Legacy Real Life DARKER HOGWARTS Reshade.txt"));

                            if (Path.Exists(pathDxgiHl) && Path.Exists(pathD3D12Hl))
                            {
                                File.Delete(pathD3D12Hl);
                                File.Move(pathDxgiHl, Path.Combine(selectFolder, "dxgi.dll"), true);
                            }
                        }
                    }

                    #endregion
                }

                if (gameSelected == "GTA V")
                {
                    #region Clean GTA V Mods

                    if (folderGtaV.ContainsKey(selectMod))
                    {

                        try
                        {
                            CleanupOthersMods3(selectMod, delGtavFsr3, selectFolder, true, "mods\\UpscalerBasePlugin");

                            if (Path.Exists(Path.Combine(selectFolder, "mods\\Shaders")))
                            {
                                Directory.Delete(Path.Combine(selectFolder, "mods\\Shaders"), true);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error clearing GTA V mods files, please try again or do it manually", "Error");
                        }
                    }

                    #endregion
                }

                if (gameSelected == "Monster Hunter Wilds")
                {
                    #region Clean Mods Indy
                    try
                    {
                        if (selectMod == "DLSSG Wilds (Only RTX)")
                        {
                            CleanupMod3(del_dlss_to_fsr, "DLSSG Wilds (Only RTX)");

                            if (Path.Exists(Path.Combine(selectFolder, "dinput8.dll")))
                            {
                                File.Delete(Path.Combine(selectFolder, "dinput8.dll"));
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error clearing Monster Hunter Wilds files, please try again or do it manually", "Error");
                    }
                    #endregion
                }

                if (gameSelected == "Indiana Jones and the Great Circle")
                {
                    #region Cleanup Others Mods Indiana Jones and the Great Circle

                    string indyConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Saved Games\MachineGames\TheGreatCircle\base");
                    string indyOldConfigPath = Path.Combine(indyConfigFilePath, "TheGreatCircleConfig.txt");

                    CleanupMod3(delOptiscaler, "FSR 3.1.4/DLSS FG (Only Optiscaler Indy");

                    if (selectMod == "Indy FG (Only RTX)")
                    {
                        if (CleanupMod3(del_dlss_to_fsr, "Indy FG (Only RTX)"))
                        {
                            if (Path.Exists(indyOldConfigPath))
                            {
                                File.Delete(Path.Combine(indyConfigFilePath, "TheGreatCircleConfig.local"));
                                File.Move(indyOldConfigPath, Path.Combine(indyConfigFilePath, "TheGreatCircleConfig.local"));
                            }

                        }
                    }

                    CleanupOthersMods("Intro Skip", "boot_sequence_pc.bk2", Path.Combine(selectFolder, "base\\video\\boot_sequence"));

                    #endregion
                }

                if (gameSelected == "Dragons Dogma 2")
                {
                    #region Cleanup Others Mods DD2
                    string[] remove_dinput8_dd2 = { "openvr_api.dll", "openxr_loader.dll", "DELETE_OPENVR_API_DLL_IF_YOU_WANT_TO_USE_OPENXR", "dinput8.dll", "reframework_revision.txt" };

                    CleanupOthersMods3("Dinput 8", remove_dinput8_dd2, selectFolder, false, "reframework");
                    #endregion
                }

                if (gameSelected == "Control")
                {
                    #region Clean Mods Control
                    string bakControlHdr = Path.Combine(selectFolder, "HDR Control");
                    {
                        try
                        {
                            // Remove the files from the HDR Path.
                            if (Path.Exists(bakControlHdr))
                            {
                                CopyFolder3(bakControlHdr, selectFolder);

                                Directory.Delete(bakControlHdr, true);
                            }
                        }
                        catch { }
                        ;
                    }
                    #endregion
                }

                if (gameSelected == "Like a Dragon: Pirate Yakuza in Hawaii")
                {
                    #region Clean Mods Yakuza
                    string enableNvidiaChecks = "mods\\Temp\\NvidiaChecks\\RestoreNvidiaSignatureChecks.reg";

                    if (gpuNameSettings.Contains("nvidia"))
                    {
                        CleanupMod3(del_dlss_global_rtx, "DLSSG Yakuza");
                    }
                    else
                    {
                        CleanupMod3(del_dlss_global_amd, "DLSSG Yakuza");
                    }

                    runReg(enableNvidiaChecks);
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

                if (gameSelected == "Elden Ring" || gameSelected == "Elden Ring Nightreign")
                {
                    #region Del Others Mods Elden Ring

                    string[] delOthersModsElden = { "mods\\EldenRingUpscaler.ini", "mods\\RDR2Upscaler.dll", "mods\\RDR2Upscaler.org" };

                    string[] del_elden_custom =
                    {
                        "dxgi.dll","ERSS-FG.dll"
                    };

                    string[] delEldenNightreign = { "dinput8.dll", "mod_loader.ini", "d3d12.dll", "NRSS.dll", "steam_appid.txt", "RemoveChromaticAberration.dll", "RemoveChromaticAberration.dl", "RemoveVignette.dll" };

                    if (selectMod == "Unlock FPS Elden")
                    {
                        string[] filesToDelete =
                        {
                        "UnlockFps.txt",
                        "mods\\UnlockTheFps.dll",
                        "dinput8.dll",
                        "mod_loader_config.ini"
                    };

                        if (File.Exists(Path.Combine(selectFolder, "UnlockFps.txt")))
                        {
                            HandlePrompt(
                            "Unlock FPS Elden",
                            "Do you want to remove the Unlock FPS mod?",
                            _ =>
                            {
                                CleanupOthersMods3("Unlock FPS", filesToDelete, selectFolder, false, Path.Combine(selectFolder, "mods\\UnlockTheFps"));
                            });
                        }
                    }

                    if (selectMod == "FSR 3.1.4/DLSS FG Custom Elden")
                    {
                        CleanupOthersMods3("FSR 3.1.2/DLSS FG Custom Elden", del_elden_custom, selectFolder, true, "ERSS2");
                    }
                    else if (folderEldenRing.ContainsKey(selectMod))
                    {
                        CleanupMod(del_elden, folderEldenRing);
                        CleanupOthersMods3("", delOthersModsElden, selectFolder, false, "mods\\UpscalerBasePlugin");

                        if (Directory.Exists(Path.Combine(selectFolder, "reshade-shaders"))) Directory.Delete(Path.Combine(selectFolder, "reshade-shaders"), true);

                    }

                    if (selectMod == "FSR 3.1.4/DLSS Nightreign RTX")
                    {
                        CleanupOthersMods3("FSR 3.1.4/DLSS Nightreign RTX", delEldenNightreign, selectFolder, true, "NRSS");

                        CleanupOthersMods3("FSR 3.1.4/DLSS Nightreign RTX", delEldenNightreign, Path.Combine(selectFolder, "mods"), true, "NRSS");
                    }
                    #endregion
                }

                else if (selectMod == "Ac Valhalla Dlss (Only RTX)")
                {
                    CleanupMod3(del_valhalla, "Ac Valhalla Dlss (Only RTX)");
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

                    CleanupMod3(del_dlss_to_fsr, "DLSS FG (ALL GPUs) Wukong");

                    #region Remove other mods

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
                        Directory.Delete(selectFolder + "\\ue4ss", true);
                        Directory.Delete(fullPathOptimizedDel + "\\LogicMods", true);
                    }
                    #endregion
                }

                else if (gameSelected == "Lords of the Fallen")
                {
                    #region Clean Mods Lotf
                    if (selectMod == "DLSS FG LOTF2 (RTX)")
                    {
                        CleanupMod3(del_dlss_to_fsr, "DLSS FG LOTF2 (RTX)");
                        if (File.Exists(Path.Combine(selectFolder, "rungame.bat")))
                        {
                            File.Delete(Path.Combine(selectFolder, "rungame.bat"));
                        }
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
        List<string> uniscaler_list = new List<string> { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uniscaler V3", "Uniscaler V4", "Uniscaler FSR 3.1" };
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

            if (panelNvngx.Visible)
            {
                panelFgMethod.Top = 160;
            }
            else
            {
                panelFgMethod.Top = 90;
            }
            if (panelAddOn2.Visible)
            {
                buttonFgMethod.Top = panelAddOn2.Top + 52;
                panelFgMethod.Top = buttonAddOn.Top + 110;
            }
        }

        private void ShowSelectedNvngx(object sender, EventArgs e)
        {
            string[] optNvngx = { "Xess 1.3", "DLSS 3.8.10", "Dlss  3.7.0 FG", "DLSS 4", "DLSSG 4", "DLSSD 4" };
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
                ConfigIni("Dx11Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx11Upscaler", "fsr22_12", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx11Upscaler", "fsr21_12", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx11Upscaler", "xess", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx11Upscaler", "fsr31_12 ", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx11Upscaler", "dlss ", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx12Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx12Upscaler", "fsr31", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx12Upscaler", "fsr21", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("Dx12Upscaler", "xess", "mods\\Temp\\OptiScaler\\Optiscalerini", "Upscalers");
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
                ConfigIni("Dx12Upscaler", "dlss", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("VulkanUpscaler", "fsr21", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("VulkanUpscaler", "fsr22", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("VulkanUpscaler", "fsr31", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
                ConfigIni("VulkanUpscaler", "dlss", "mods\\Temp\\OptiScaler\\OptiScaler.ini", "Upscalers");
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
