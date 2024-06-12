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

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formSettings : Form
    {
        public static string gameSelected { get; set; }
        public static string fsrSelected { get; set; }
        string path_fsr = "";

        private List<string> pendingItems = new List<string>();
        private static formSettings instance;

        public string select_mod;
        bool varLfz = false;

        public formSettings()
        {
            InitializeComponent();

            AddOptionsSelect.ItemCheck += new ItemCheckEventHandler(AddOptionsSelect_ItemCheck);
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

        #region Folder RDR2
        Dictionary<string, string[]> origins_rdr2_folder = new Dictionary<string, string[]>
        {
            {"0.9.0", new string[] {"mods\\FSR2FSR3_0.9.0\\Red Dead Redemption 2",
                                    "mods\\FSR2FSR3_0.9.0\\FSR2FSR3_COMMON"}},

            {"0.10.0", new string[] {"mods\\FSR2FSR3_0.10.0\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.0\\Red Dead Redemption 2"}},

            {"0.10.1", new string[] {"mods\\FSR2FSR3_0.10.1\\FSR2FSR3_COMMON",
                                     "mods\\FSR2FSR3_0.10.1\\Red Dead Redemption 2"}},

            {"0.10.1h1", new string[] {"mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\FSR2FSR3_COMMON",
                                       "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Red Dead Redemption 2"}},

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

        public void ReplaceIni()
        {
            if (folder_clean_ini.ContainsKey(select_mod) && folder_fake_gpu.ContainsKey(select_mod))
            {
                string path_clean_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, folder_clean_ini[select_mod]);
                string modified_ini = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + folder_fake_gpu[select_mod]);
                File.Copy(path_clean_ini, modified_ini, true);
                Debug.WriteLine(modified_ini);
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

            void ShowErrorMessage(string message)
            {
                DialogResult = MessageBox.Show(message, "Error", MessageBoxButtons.OK);
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
            string path_mods = path_fsr;
            string path_dest = select_Folder;
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string selectedVersion = listMods.SelectedItem as string;
            string varDici;
            string[] uniscalerVersion = { "Uniscaler", "Uniscaler + Xess + Dlss", "Uniscaler V2" };

            if (selectedVersion != null)
            {
                if (DictionaryFSR.TryGetValue(selectedVersion, out string[] paths))
                {
                    foreach (string relativePath in paths)
                    {
                        string path_final = Path.GetFullPath(Path.Combine(exeDirectory, relativePath));
                        string path_fsr_common = Path.GetFullPath(Path.Combine(exeDirectory, "mods\\FSR2_FSR3_COMMON_GLOBAL"));
                        if (Directory.Exists(path_final))
                        {
                            await CopyModsAsync(path_final, path_dest);

                            if (uniscalerVersion.All(uniscalerVersion => !selectedVersion.Contains(uniscalerVersion) && !rdr2_folder.ContainsKey(select_mod)))
                            {
                                await CopyModsAsync(path_fsr_common, path_dest);
                            }
                        }
                    }
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_220" } },
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_210" } },
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_200" } },
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
                { "0.10.1h1", new string[] { "mods\\FSR2FSR3_0.10.1h1\\0.10.1h1\\Generic FSR\\FSR2FSR3_SDK" } },
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

        public void CleanupMod()
        {
            string[] DelFiles = Directory.GetFiles(select_Folder);
            try
            {
                if (folder_fake_gpu.ContainsKey(select_mod))
                {
                    foreach (string filesDestFolder in DelFiles)
                    {
                        string DelFileName = Path.GetFileName(filesDestFolder);
                        string fullPathDelFile = Path.Combine(select_Folder, DelFileName);
                        File.Delete(fullPathDelFile);
                    }

                    if (Directory.Exists(select_Folder + "\\uniscaler"))
                    {
                        Directory.Delete(select_Folder + "\\uniscaler",true);
                    }
                    MessageBox.Show("Mod Successfully Removed", "Success", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please close the game or any other folders related to the game.", "Error", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            select_mod = listMods.SelectedItem as string;
            if (select_Folder != null && select_mod != null)
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
                ReplaceIni();
            }
            else
            {
                MessageBox.Show("Please fill out the first 3 options, Select Game, Select Folder, and Mod Options.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            CleanupMod();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
