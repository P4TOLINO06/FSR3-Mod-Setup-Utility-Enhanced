﻿using System;
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

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formSettings : Form
    {
        public static string gameSelected { get; set; }
        public static string fsrSelected { get; set; }
        string path_fsr = "";

        public string select_mod;

        public formSettings()
        {
            InitializeComponent();

            AddOptionsSelect.ItemCheck += new ItemCheckEventHandler(AddOptionsSelect_ItemCheck);
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

        private void AddOptionsSelect_ItemCheck(object? sender, ItemCheckEventArgs e)
        {

            int index = e.Index;

            bool CheckedOption = e.NewValue == CheckState.Checked;

            string itemText = AddOptionsSelect.Items[index].ToString();

            string AddOptSelect = CheckedOption.ToString().ToLower();

            //Toml file configuration based on the checked box (AddOptionsSelect - CheckedListBox)

            select_mod = listMods.SelectedItem as string;

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

            if (itemText == "Ue Compatibility Mode" && select_mod != null)
            {
                if (folder_ue.ContainsKey(select_mod))
                {
                    string pathToml_Ue = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folder_ue[select_mod]);
                    ConfigIni("amd_unreal_engine_dlss_workaround", AddOptSelect, folder_ue, "compatibility");
                }
                else
                {
                    DialogResult = MessageBox.Show("Please select a mod version starting from 0.9.0", "Error", MessageBoxButtons.OK);

                    e.NewValue = CheckState.Unchecked;
                }
            }

            if (itemText == "Nvapi Results" && select_mod != null)
            {

                if (folder_nvapi.ContainsKey(select_mod))
                {
                    string pathToml_Nvapi = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)! + folder_nvapi[select_mod]);

                    ConfigIni("fake_nvapi_results", AddOptSelect, folder_nvapi, "compatibility");
                }
                else
                {
                    DialogResult = MessageBox.Show("Please select a mod version starting from 0.10.2h1.", "Error", MessageBoxButtons.OK);

                    e.NewValue = CheckState.Unchecked;
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
            Debug.WriteLine(selectedVersion);

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

                            if (uniscalerVersion.All(uniscalerVersion => !selectedVersion.Contains(uniscalerVersion)))
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

                if (fsr_2_1_opt.Contains(gameSelected) || fsr_sct_2_1.Contains(fsrSelected))
                {
                    fsr2_1();
                }

                if (fsr_2_0_opt.Contains(gameSelected) || fsr_sct_2_0.Contains(fsrSelected))
                {
                    fsr_2_0();
                }

                if (fsr_sdk_opt.Contains(gameSelected) || fsr_sct_sdk.Contains(fsrSelected))
                {
                    fsr_sdk();
                }

                select_mod = listMods.SelectedItem as string;

                if (select_mod != null)
                {
                    if (folder_fake_gpu.ContainsKey(select_mod))
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
            }
            else
            {
                MessageBox.Show("Please fill out the first 3 options, Select Game, Select Folder, and Mod Options.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
