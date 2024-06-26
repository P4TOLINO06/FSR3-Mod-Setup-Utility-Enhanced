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
        public string select_mod;
        bool varLfz = false;
        private formEditorToml formEditor;
        private mainForm mainFormInstance;
        public System.Windows.Forms.TextBox fpsLimitTextBox;
        public System.Windows.Forms.Label labelFpsLimit;

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
            List<string> itensDelete = new List<string> { "Elden Ring FSR3", "Elden Ring V2", "Disable Anti Cheat" };

            List<string> gamesIgnore = new List<string> { "Elden Ring" };

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
            else if (listMods != null && !listMods.Items.Contains(items))
            {
                foreach (var item in items)
                {
                    if (!listMods.Items.Contains(item))
                    {
                        listMods.Items.Add(item);
                    }
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

        #region Fake Nvidia Gpu Toml File Path
        static Dictionary<string, string> folder_fake_gpu = new Dictionary<string, string>()
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
            {"The Callisto Protocol FSR3",@"\mods\FSR3_Callisto\enable_fake_gpu\fsr2fsr3.config.toml"}
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
            "Uniscaler V2"
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
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" }
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
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" }
            };
        #endregion

        #region Folder Uniscaler
        Dictionary<string, string> folder_uniscaler = new Dictionary<string, string>
            {
                { "Uniscaler", @"mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" }
            };
        #endregion

        #region Folder Uniscaler V2
        Dictionary<string, string> folder_uniscaler_v2 = new Dictionary<string, string>
        {
            {"Uniscaler V2",@"mods\\Temp\\Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml"}
        };
        #endregion

        #region Folder Elden Ring
        Dictionary<string, string[]> folderEldenRing = new Dictionary<string, string[]>
        {
            { "Disable Anti Cheat", new string[] { @"mods\Elden_Ring_FSR3\ToggleAntiCheat" } },
            { "Elden Ring FSR3", new string[] { @"mods\Elden_Ring_FSR3\EldenRing_FSR3" } },
            { "Elden Ring FSR3 V2", new string[] { @"mods\Elden_Ring_FSR3\EldenRing_FSR3 v2" } }
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

        #region Folder Disable Console
        Dictionary<string, string> folder_disable_console = new Dictionary<string, string>
            {
                { "0.10.3", @"mods\Temp\FSR2FSR3_0.10.3\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "0.10.4", @"mods\Temp\FSR2FSR3_0.10.4\enable_fake_gpu\fsr2fsr3.config.toml" },
                { "Uniscaler", @"mods\Temp\Uniscaler\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler + Xess + Dlss", @"mods\Temp\FSR2FSR3_Uniscaler_Xess_Dlss\enable_fake_gpu\uniscaler.config.toml" },
                { "Uniscaler V2", @"mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" }
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
            {"Uniscaler V2", new string[] {"mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod"}}
        };

        Dictionary<string, string[]> rdr2_folder = new Dictionary<string, string[]>
        {
            { "RDR2 Build_2", new string[] { "mods\\Red_Dead_Redemption_2_Build02" } },
            { "RDR2 Build_4", new string[] { "mods\\RDR2Upscaler-FSR3Build04" } },
            { "RDR2 Mix", new string[] { "mods\\RDR2_FSR3_mix" } },
            { "RDR2 Mix 2", new string[] { "mods\\RDR2_FSR3_mix" } },
            { "Red Dead Redemption V2", new string[] { "mods\\RDR2_FSR3_V2" } },
            { "RDR2 Non Steam FSR3", new string[] { "mods\\FSR3_RDR2_Non_Steam\\RDR2_FSR3" } }
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
            { "0.10.1h1", "mods\\FSR2FSR3_0.10.1h1\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.2h1", "mods\\FSR2FSR3_0.10.2h1\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.3", "mods\\FSR2FSR3_0.10.3\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "0.10.4", "mods\\FSR2FSR3_0.10.4\\enable_fake_gpu\\fsr2fsr3.config.toml" },
            { "Uniscaler", "mods\\FSR2FSR3_Uniscaler\\enable_fake_gpu\\uniscaler.config.toml" },
            { "Uniscaler + Xess + Dlss", "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\enable_fake_gpu\\uniscaler.config.toml" },
            { "The Callisto Protocol FSR3", "mods\\FSR3_Callisto\\enable_fake_gpu" },
            { "Uniscaler V2", "mods\\FSR2FSR3_Uniscaler_V2\\enable_fake_gpu\\uniscaler.config.toml" },
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

        //Ini Editor
        public void ConfigIni(string key, string value, Dictionary<string, string> DictionaryPath, string? section = null)
        {
            select_mod = listMods.SelectedItem as string;
            string pathToml = DictionaryPath[select_mod];
            if (DictionaryPath.ContainsKey(select_mod))
            {
                IniEditor iniEditor = new IniEditor(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + pathToml));

                iniEditor.Write(section, key, " " + value);
            }
        }
        public void ConfigIni2(string key, string value, string path, string? section = null)
        {
            string pathIni = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, path);
            IniEditor iniEditor = new IniEditor(pathIni);

            iniEditor.Write(section, key, value);
        }

        public void ReplaceIni()
        {
            if (folder_clean_ini.ContainsKey(select_mod) && folder_fake_gpu.ContainsKey(select_mod))
            {
                string path_clean_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder_clean_ini[select_mod]);
                string modified_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + folder_fake_gpu[select_mod]);
                File.Copy(path_clean_ini, modified_ini, true);
            }
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
            fpsLimitTextBox.Size = new System.Drawing.Size(25,25);
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
            fpsLimitTextBox.TextChanged+= textBoxFps_TextChanged;
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
                ConfigIni("original_frame_rate_limit", fpsLimitTextBox.Text.ToString(), uniscaler_path, "general");
            }
        }
        private void AddOptionsSelect_ItemCheck(object? sender, ItemCheckEventArgs e)
        {

            int index = e.Index;

            bool CheckedOption = e.NewValue == CheckState.Checked;

            string itemText = AddOptionsSelect.Items[index].ToString();

            string AddOptSelect = CheckedOption.ToString().ToLower();

            //Toml file configuration based on the checked box (AddOptionsSelect - CheckedListBox)

            select_mod = listMods.SelectedItem as string;

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
                if (select_Folder != null)
                {
                    if (folder.ContainsKey(select_mod))
                    {
                        string pathToml = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder[select_mod]);
                        ConfigIni(configKey, AddOptSelect, folder, section);
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
                if (select_mod != null)
                {
                    select_mod = listMods.SelectedItem as string;
                    if (select_mod != null && folder_fake_gpu.ContainsKey(select_mod))
                    {
                        string path1 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), folder_fake_gpu[select_mod]);
                        
                        ShowErrorMessage();//Uncheck the Toml Editor in AddOptionsSelect option.

                        ((mainForm)this.ParentForm).loadForm(typeof(formEditorToml), select_mod);
                    }
                }
                else
                { 
                    ShowErrorMessage("Select a mod version to proceed. (excluding specific versions, for exemple: Elden Ring FSR3");
                    return;
                }
            }

                if (itemText == "Fake Nvidia Gpu" && select_mod != null)
                {
                    string pathToml_f_gpu = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folder_fake_gpu[select_mod]);

                    if (edit_fake_gpu_list.Contains(select_mod))
                    {
                        ConfigIni("fake_nvidia_gpu", AddOptSelect, folder_fake_gpu, "compatibility");
                    }
                    else if (edit_old_fake_gpu.Contains(select_mod))
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
                ConfigureMod("disable_overlay_blocker", "Select Uniscaler V2 to use this option.", folder_uniscaler_v2, "general");
            }
            if (itemText == "Disable Console")
            {
                ConfigureMod("disable_console", "Select a mod version starting from 0.10.3.", folder_disable_console, "logging");
            }

            if(itemText == "Fps Limit")
            {
                if (select_mod != null && uniscaler_path.ContainsKey(select_mod))
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
            string pathToml = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folder_fake_gpu[select_mod]);

            string destFolder = Path.Combine(select_Folder, Path.GetFileName(pathToml));

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

        }

        public string select_Folder;
        public void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog searchFolderPath = new FolderBrowserDialog();
            searchFolderPath.RootFolder = Environment.SpecialFolder.Desktop;
            searchFolderPath.Description = "Select the folder where the game's exe is located (usually the exe ends with Win64-Shipping)";
            searchFolderPath.ShowNewFolderButton = false;

            if (searchFolderPath.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = searchFolderPath.SelectedPath;
                select_Folder = searchFolderPath.SelectedPath;
            }
        }

        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMods.SelectedItem != null)
            {
                select_mod = listMods.SelectedItem.ToString();
                SetTextModOp();
            }
            if (!folder_mod_operates.ContainsKey(select_mod))
            {
                panelModOp.Visible = false;
                panelRes.Visible = false;
            }
            if (!uniscaler_path.ContainsKey(select_mod))
            {
                panelResPreset.Visible = false;
            }

            if (select_mod == "Uniscaler + Xess + Dlss")
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
            "Everspace 2", "Evil West", "F1 2022", "F1 2023", "FIST: Forged In Shadow Torch", "Fort Solis", "Hellblade 2", "Hogwarts Legacy", "Kena: Bridge of Spirits", "Lies of P", "Loopmancer", "Manor Lords", "Metro Exodus Enhanced Edition", "Monster Hunter Rise", "Outpost: Infinity Siege", "Palworld", "Ready or Not", "Remnant II", "RoboCop: Rogue City",
            "Sackboy: A Big Adventure", "Satisfactory", "Shadow Warrior 3", "Smalland", "STAR WARS Jedi: Survivor", "Starfield", "Steelrising", "TEKKEN 8", "The Chant", "The Invincible", "The Medium", "Wanted: Dead"};

        List<string> fsr_2_1_opt = new List<string> { "Chernobylite", "Dead Space (2023)", "Hellblade: Senua's Sacrifice", "Hitman 3", "Horizon Zero Dawn", "Judgment", "Martha Is Dead", "Marvel's Spider-Man Remastered", "Marvel's Spider-Man Miles Morales", "Returnal", "Ripout", "Saints Row", "Uncharted Legacy of Thieves Collection" };

        List<string> fsr_2_0_opt = new List<string> { "Alone in the Dark", "Brothers: A Tale of Two Sons Remake", "Deathloop", "Dying Light 2", "Ghostrunner 2", "High On Life", "Jusant", "Layers of Fear", "Marvel's Guardians of the Galaxy", "Nightingale", "Rise of The Tomb Raider", "Shadow of the Tomb Raider", "The Outer Worlds: Spacer's Choice Edition", "The Witcher 3" };

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
            string path_dest = select_Folder;
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string selectedVersion = listMods.SelectedItem as string;
            string[] uniscalerVersion = { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2" };

            if (selectedVersion != null)
            {
                if (DictionaryFSR.TryGetValue(selectedVersion, out string[] paths))
                {
                    foreach (string relativePath in paths)
                    {
                        string path_final = Path.GetFullPath(Path.Combine(exeDirectory, relativePath));
                        string path_fsr_initial=  (path_final + "\\..\\..") + "\\FSR2FSR3_COMMON";
                        string path_fsr_common = Path.GetFullPath(path_fsr_initial);
                        Debug.WriteLine(path_fsr_common);
                        if (Directory.Exists(path_final))
                        {
                            await CopyModsAsync(path_final, path_dest);

                            if (uniscalerVersion.All(uniscalerVersion => !selectedVersion.Contains(uniscalerVersion) && !rdr2_folder.ContainsKey(select_mod) && !folderEldenRing.ContainsKey(select_mod)))
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
                File.Copy(files_fsr, select_Folder + "\\" + fileName, true);
            }
            foreach (var subPath in Directory.GetDirectories(pathFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = subPath.Substring(pathFolder.Length + 1);
                string fullPath = Path.Combine(select_Folder, relativePath);

                Directory.CreateDirectory(fullPath);

                foreach (string filePath in Directory.GetFiles(subPath))
                {
                    string relativeFilePath = filePath.Substring(subPath.Length + 1);
                    string destFilePath = Path.Combine(fullPath, relativeFilePath);
                    File.Copy(filePath, destFilePath, true);
                }
            }
        }
        public void fsr2_2()
        {
            string path_final;
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\Generic FSR\\FSR2FSR3_220" } },
                { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_220" } },
                { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_220" } },
                { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_220\\FSR2FSR3_220" } },
                { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod" } },
                { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod" } },
                { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } }
            };
            #endregion

            CopyFSR(origins_2_2_folder);
        }

        public void fsr2_1()
        {
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\Generic FSR\\FSR2FSR3_210" } },
                { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_210"} },
                { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_210" } },
                { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_210\\FSR2FSR3_210"} },
                { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod"} },
                { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod"} },
                { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } }
            };
            #endregion
            CopyFSR(origins_2_1_folder);
        }

        public void fsr_2_0()
        {
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\Generic FSR\\FSR2FSR3_200" } },
                { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_200"} },
                { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_200"} },
                { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_200\\FSR2FSR3_200"} },
                { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod"} },
                { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod"} },
                { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod"} }
            };
            #endregion
            CopyFSR(origins_2_0_folder);
        }

        public void fsr_sdk()
        {
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\Generic FSR\\FSR2FSR3_SDK" } },
                { "0.10.2h1", new string[] { "mods\\FSR2FSR3_0.10.2h1\\Generic FSR\\FSR2FSR3_SDK" } },
                { "0.10.3", new string[] { "mods\\FSR2FSR3_0.10.3\\Generic FSR\\FSR2FSR3_SDK" } },
                { "0.10.4", new string[] { "mods\\FSR2FSR3_0.10.4\\FSR2FSR3_SDK\\FSR2FSR3_SDK" } },
                { "Uniscaler", new string[] { "mods\\FSR2FSR3_Uniscaler\\Uniscaler_4\\Uniscaler mod" } },
                { "Uniscaler + Xess + Dlss", new string[] { "mods\\FSR2FSR3_Uniscaler_Xess_Dlss\\Uniscaler_mod\\Uniscaler_mod" } },
                { "Uniscaler V2", new string[] { "mods\\FSR2FSR3_Uniscaler_V2\\Uni_V2\\Uni_Mod" } }
            };
            #endregion
            CopyFSR(origins_sdk_folder);
        }

        public void fsr_rdr2()
        {
            CopyFSR(origins_rdr2_folder);
        }

        public void ac_valhalla_dlss()
        {
            CopyFolder("mods\\Ac_Valhalla_DLSS");
        }

        public async Task bdg3_fsr3()
        {
            CopyFSR(folderBdg3);

            #region Copy ini file for mods folder Baldur's Gate 3 FSR3 V3 
            if (select_mod == "Baldur's Gate 3 FSR3 V3")
            {
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string pathBdgIni = "mods\\FSR3_BDG_3\\BG3Upscaler.ini";
                string fullPath = Path.Combine(exeDirectory, pathBdgIni);
                try
                {
                    await Task.Delay((2000));
                    {                
                        File.Copy(fullPath, select_Folder + "\\mods\\BG3Upscaler.ini", true);
                    }
                }
                catch { }
            }
            #endregion
        }

        public async Task fsr_rdr2_build02()
        {

            if (select_mod == "RDR2 Non Steam FSR3")
            {
                string path_dll = "mods\\FSR3_RDR2_Non_Steam\\RDR2_DLL";
                string[] dll_files = Directory.GetFiles(path_dll);
                DialogResult var_rdr2_non_steam = MessageBox.Show("Do you want to copy the DLL files? Some users may receive a DLL error when running the game with the mod. (Only select \'Yes\' if you have received the error)", "DLL", MessageBoxButtons.YesNo);

                if (var_rdr2_non_steam == DialogResult.Yes)
                {
                    foreach (string dll_file in dll_files)
                    {
                        string dll_name = Path.GetFileName(dll_file);
                        string full_path_dll = Path.Combine(select_Folder, dll_name);
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

                    if (select_mod == "RDR2 Mix 2")
                    {
                        Directory.CreateDirectory(select_Folder + "\\mods");

                        File.Copy(path_ini, select_Folder + "\\mods\\RDR2Upscaler.ini", true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void elden_fsr3()
        {
            CopyFSR(folderEldenRing);
        }

        public void aw2_fsr3()
        {
            CopyFSR(folderAw2);

            //Disable Nvidia Signature Checks
            if (select_mod == "Alan Wake 2 FG RTX")
            {
                string path_aw2_over = @"mods\\FSR3_GOT\\DLSS FG\\DisableNvidiaSignatureChecks.reg";

                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "regedit.exe";
                    process.StartInfo.Arguments = "/s \"" + path_aw2_over + "\"";
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void CleanupMod(List<string> ListClean, Dictionary<string, string[]> DictionaryPath)
        {
            string[] DelFiles = Directory.GetFiles(select_Folder);
            try
            {
                if (DictionaryPath.ContainsKey(select_mod))
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    if (Directory.Exists(select_Folder + "\\mods"))
                    {
                        Directory.Delete(select_Folder + "\\mods", true);
                    }
                    if (Directory.Exists(select_Folder + "\\reshade-shaders"))
                    {
                        Directory.Delete(select_Folder + "\\reshade-shaders", true);
                    }
                    MessageBox.Show("Mod Successfully Removed", "Success", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please close the game or any other folders related to the game.", "Error", MessageBoxButtons.OK);
            }
        }

        #region CleanupMod2
        public void CleanupMod2(List<string> ListClean, Dictionary<string, string> DictionaryPath)
        {
            string[] DelFiles;
            if (select_Folder != null)
            {
                DelFiles = Directory.GetFiles(select_Folder);
            }
            else
            {
                MessageBox.Show("Select the folder where the mod is located before proceeding.", "Error", MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (DictionaryPath.ContainsKey(select_mod))
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    if (Directory.Exists(select_Folder + "\\uniscaler"))
                    {
                        Directory.Delete(select_Folder + "\\uniscaler", true);
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

        #region Cleanup Mod 3
        public void CleanupMod3(List<string> ListClean,string modName)
        {
            string[] DelFiles = Directory.GetFiles(select_Folder);
            try
            {
                if (select_mod == modName)
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);

                        if (ListClean.Contains(DelFileName))
                        {
                            File.Delete(filesDestFolder);
                        }
                    }

                    if (Directory.Exists(select_Folder + "\\mods"))
                    {
                        Directory.Delete(select_Folder + "\\mods", true);
                    }
                    if (Directory.Exists(select_Folder + "\\reshade-shaders"))
                    {
                        Directory.Delete(select_Folder + "\\reshade-shaders", true);
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
            select_mod = listMods.SelectedItem as string;
            if (select_Folder != null && select_mod != null && gameSelected != null)
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

                else if ((fsr_sct_rdr2.Contains(gameSelected) && origins_rdr2_folder.ContainsKey(select_mod)) || (fsr_sct_rdr2.Contains(fsrSelected) && origins_rdr2_folder.ContainsKey(select_mod)))
                {
                    fsr_rdr2();
                }

                else if ((fsr_sct_rdr2.Contains(gameSelected) && rdr2_folder.ContainsKey(select_mod) || fsr_sct_rdr2.Contains(fsrSelected) && rdr2_folder.ContainsKey(select_mod)))
                {
                    fsr_rdr2_build02();
                }
                if(folderEldenRing.ContainsKey(select_mod))
                {
                    elden_fsr3();
                }
                if (folderAw2.ContainsKey(select_mod))
                {
                    aw2_fsr3();
                }
                if(select_mod == "Ac Valhalla Dlss (Only RTX)")
                {
                    ac_valhalla_dlss();
                }
                if (folderBdg3.ContainsKey(select_mod))
                {
                    bdg3_fsr3();
                }

                select_mod = listMods.SelectedItem as string;

                if (select_mod != null)
                {
                    if (folder_fake_gpu.ContainsKey(select_mod))
                    {
                        CopyToml();
                    }
                }
                if (varLfz is true)
                {
                    string path_lfz = "mods\\Temp\\global _lfz\\lfz.sl.dlss.dll";
                    File.Copy(path_lfz, select_Folder + "\\lfz.sl.dlss.dll", true);
                }

                if (gameSelected == "Select FSR Version" && fsrSelected != null)
                {
                    MessageBox.Show("Successful installation", "Successful", MessageBoxButtons.OK);
                }
                else if (gameSelected != "Select FSR Version")
                {
                    MessageBox.Show("Successful installation", "Successful", MessageBoxButtons.OK);
                }

                if (select_mod != null)
                {
                    foreach (string optNvngx in optionsNvngx.CheckedItems)
                    {
                        string pathNvngx;
                        if (optNvngx.Contains("Default"))
                        {
                            if (File.Exists(select_Folder + "\\nvngx.dll"))
                            {
                                try
                                {
                                    string newNameNvngx = select_Folder + "\\nvngx.txt";
                                    string oldNameNvngx = select_Folder + "\\nvngx.dll";
                                    File.Move(oldNameNvngx, newNameNvngx);
                                }
                                catch { }
                            }
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx.dll";
                            File.Copy(pathNvngx, select_Folder + "\\nvngx.dll", true);
                        }
                        if (optNvngx.Contains("NVNGX Version 1"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx.ini";
                            File.Copy(pathNvngx, select_Folder + "\\nvngx.ini", true);
                        }
                        if (optNvngx.Contains("Xess 1.3"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\libxess.dll";
                            File.Copy(pathNvngx, select_Folder + "\\libxess.dll", true);
                        }
                        if (optNvngx.Contains("Dlss 3.7.0"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlss.dll";
                            File.Copy(pathNvngx, select_Folder + "\\nvngx_dlss.dll", true);
                        }
                        if (optNvngx.Contains("Dlss 3.7.0 FG"))
                        {
                            pathNvngx = "mods\\Temp\\nvngx_global\\nvngx\\nvngx_dlssg.dll";
                            File.Copy(pathNvngx, select_Folder + "\\nvngx_dlssg.dll", true);
                        }
                    }
                    foreach (string optDxgi in optionsDxgi.CheckedItems)
                    {
                        string pathDxgi;
                        if (optDxgi == "Dxgi.dll")
                        {
                            pathDxgi = "mods\\Temp\\dxgi_global\\dxgi.dll";
                            File.Copy(pathDxgi, select_Folder + "\\dxgi.dll", true);
                        }
                        if (optDxgi == "D3D12.dll")
                        {
                            pathDxgi = "mods\\Temp\\dxgi_global\\d3d12.dll";
                            File.Copy(pathDxgi, select_Folder + "\\d3d12.dll", true);
                        }
                    }
                    foreach (string optAddOn in optionsAddOn.CheckedItems)
                    {
                        string pathAddOn;
                        if (optAddOn == "Optiscaler")
                        {
                            pathAddOn = "mods\\Addons_mods\\OptiScaler";
                            string[] fileOptiscaler = Directory.GetFiles(pathAddOn);

                            foreach (string optFile in fileOptiscaler)
                            {
                                string nameOptiscaler = Path.GetFileName(optFile);
                                string fullPath = Path.Combine(select_Folder, nameOptiscaler);
                                string pathIni = "mods\\Temp\\OptiScaler\\nvngx.ini";
                                File.Copy(optFile, fullPath, true);
                                File.Copy(pathIni, select_Folder + "\\nvngx.ini", true);
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
                                string fullPath = Path.Combine(select_Folder, fileName);
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

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (select_mod != null)
            {
                try
                {
                    if (File.Exists(select_Folder + "\\nvngx.txt"))
                    {
                        DialogResult var_nvngx = MessageBox.Show("A backup of the file nvngx.dll has been found. Do you want to restore the backup?", "Nvngx.dll", MessageBoxButtons.YesNo);

                        if (var_nvngx == DialogResult.Yes)
                        {
                            string oldNvngx = select_Folder + "\\nvngx.txt";
                            string newNvngx = select_Folder + "\\nvngx.dll";
                            File.Delete(newNvngx);
                            File.Move(oldNvngx, newNvngx);
                        }
                    }
                }
                catch { }

                if (folder_fake_gpu.ContainsKey(select_mod))
                {
                    CleanupMod2(modCleanList, folder_fake_gpu);
                }
                else if (rdr2_folder.ContainsKey(select_mod))
                {
                    CleanupMod(del_rdr2_custom_files, rdr2_folder);
                }
                else if (folderAw2.ContainsKey(select_mod))
                {
                    CleanupMod(del_aw2, folderAw2);
                    #region RestoreNvidiaSignatureChecks
                    if (select_mod == "Alan Wake 2 FG RTX")
                    {
                        string path_aw2_en = @"mods\\FSR3_GOT\\DLSS FG\\RestoreNvidiaSignatureChecks.reg";
                        try
                        {
                            Process process = new Process();
                            process.StartInfo.FileName = "regedit.exe";
                            process.StartInfo.Arguments = "/s \"" + path_aw2_en + "\"";
                            process.Start();
                            process.WaitForExit();
                        }
                        catch { }
                    }
                    #endregion
                }
                else if (select_mod == "Ac Valhalla Dlss (Only RTX)")
                {
                    CleanupMod3(del_valhalla, "Ac Valhalla Dlss (Only RTX)");
                }
                else if (select_mod == null && select_Folder == null)
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
        List<string> uniscaler_list = new List<string> { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2", "Uni Custom Miles", "Dlss Jedi" };
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
                { "Uniscaler V2", @"\mods\Temp\Uniscaler_V2\enable_fake_gpu\uniscaler.config.toml" }
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
            if (uniscaler_path.ContainsKey(select_mod))
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
                        ConfigIni(tomlKey, value, uniscaler_path, "resolution_override");
                    }
                }
            }
        }

        public void SetTextModOp()
        {
            if (unlock_mod_operates_list.Contains(select_mod))
            {
                modOpt1.Text = "Default";
                modOpt2.Text = "Enable Upscaling Only";
                modOpt3.Text = "Use Game Upscaling";
                modOpt4.Text = "Replace dlss fg";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt4.Visible = true;
            }
            else if (select_mod == "0.9.0")
            {
                modOpt1.Text = "Enable Upscaling Only";
                modOpt2.Visible = false;
                modOpt3.Visible = false;
                modOpt4.Visible = false;
            }
            else if (uniscaler_list.Contains(select_mod))
            {
                modOpt1.Text = "FSR3";
                modOpt2.Text = "DLSS";
                modOpt3.Text = "XESS";
                modOpt2.Visible = true;
                modOpt3.Visible = true;
                modOpt4.Visible = false;
            }
            this.Invalidate();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            select_mod = listMods.SelectedItem as string;
            if (select_mod != null && folder_mod_operates.ContainsKey(select_mod))
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

        bool var_modop = false;

        private void button3_Click(object sender, EventArgs e)
        {
            select_mod = listMods.SelectedItem as string;
            if (var_modop == false)
            {
                var_modop = true;
            }
            else
            {
                var_modop = false;
            }

            if (select_mod == "0.9.0")
            {
                ConfigIni("enable_upscaling_only", var_modop.ToString().ToLower(), folder_mod_operates, "general");
            }

            else if (unlock_mod_operates_list.Contains(select_mod))
            {
                ConfigIni("mode", "\"default\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(select_mod))
            {
                ConfigIni("upscaler", "\"fsr3\"", folder_mod_operates, "general");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (unlock_mod_operates_list.Contains(select_mod))
            {
                ConfigIni("mode", "\"enable_upscaling_only\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(select_mod))
            {
                ConfigIni("upscaler", "\"dlss\"", folder_mod_operates, "general");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (unlock_mod_operates_list.Contains(select_mod))
            {
                ConfigIni("mode", "\"use_game_upscaling\"", folder_mod_operates, "general");
            }
            else if (uniscaler_list.Contains(select_mod))
            {
                ConfigIni("upscaler", "\"xess\"", folder_mod_operates, "general");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (unlock_mod_operates_list.Contains(select_mod))
            {
                ConfigIni("mode", "\"replace_dlss_fg\"", folder_mod_operates, "general");
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
            if (select_mod != null && folder_ue.ContainsKey(select_mod))
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
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueUltraQ != null)
            {
                decimal valueConvUltraQ = valueUltraQ.Value / 100;
                ConfigIni("ultra_quality", valueConvUltraQ.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }
        private void buttonQ_Click(object sender, EventArgs e)
        {
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueQ != null)
            {
                decimal valueConvQ = valueQ.Value / 100;
                ConfigIni("quality", valueConvQ.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }
        private void buttonBalanced_Click(object sender, EventArgs e)
        {
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueBalanced != null)
            {
                decimal valueConvBalanced = valueBalanced.Value / 100;
                ConfigIni("balanced", valueConvBalanced.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonPerf_Click(object sender, EventArgs e)
        {
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valuePerf != null)
            {
                decimal valueConvPerf = valuePerf.Value / 100;
                ConfigIni("performance", valueConvPerf.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonUltraP_Click(object sender, EventArgs e)
        {
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueUltraP != null)
            {
                decimal valueConvUltraP = valueUltraP.Value / 100;
                ConfigIni("ultra_performance", valueConvUltraP.ToString().Replace(',', '.'), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonNative_Click(object sender, EventArgs e)
        {
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueNative != null)
            {
                decimal valueConvNat = valueNative.Value / 100;
                ConfigIni("native", valueConvNat.ToString().Replace(',', '.'), folder_ue, "general");
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
            if (select_mod != null && folder_ue.ContainsKey(select_mod) && valueSharpOver != null)
            {
                ConfigIni("sharpness_override", valueConvSharpOver.ToString(CultureInfo.InvariantCulture), folder_ue, "general");
            }
            else
            {
                MessageBox.Show("Select a mod version starting from 0.9.0 and set a number in the counter next to Sharpness override", "Error", MessageBoxButtons.OK);
            }
        }

        private void buttonResPreset_Click(object sender, EventArgs e)
        {
            if (select_mod != null && uniscaler_path.ContainsKey(select_mod))
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
        }

        private void buttonNvngx_Click(object sender, EventArgs e)
        {
            if (panelNvngx.Visible == true)
            {
                buttonAddOn.Top = panelNvngx.Top + 3;
                panelAddOn2.Top = buttonAddOn.Top + 30;
            }
            else
            {
                buttonAddOn.Top = panelNvngx.Top + 72;
                panelAddOn2.Top = buttonAddOn.Top + 28;
            }
            ShowSubMenu(panelNvngx);
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
            ShowSubMenu(panelAddOn2);
        }

        private void buttonAddUps_Click(object sender, EventArgs e)
        {
            if (optionsAddOn.CheckedItems.Contains("Optiscaler"))
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
            ConfigIni2("Dx11Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }

        private void buttonAddUps2_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx11Upscaler", "fsr22_12", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }

        private void buttonAddUps3_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx11Upscaler", "fsr21_12", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }

        private void buttonAddUps4_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx11Upscaler", "xess", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }

        private void buttonAddUps5_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx12Upscaler", "fsr22", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }
        private void buttonAddUps6_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx12Upscaler", "fsr21", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }

        private void buttonAddUps7_Click(object sender, EventArgs e)
        {
            ConfigIni2("Dx12Upscaler", "xess", "mods\\Temp\\OptiScaler\\nvngx.ini", "Upscalers");
        }
    }
}
