using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class formEditorToml : Form
    {
        public string pathT { get; set; }
        private string saveTomlPath;
        public formEditorToml()
        {
            InitializeComponent();
        }

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
        public void SetPathT(string path)
        {
            this.pathT = path;
            if (!string.IsNullOrEmpty(pathT) && folder_fake_gpu.ContainsKey(pathT))
            {
                string pathTo = folder_fake_gpu[pathT];
                LoadToml(pathTo);
            }
        }

        private void formEditorToml_Load(object sender, EventArgs e)
        {

        }

        public void LoadToml(string pathToml)
        {
            try
            {
                pathToml = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + pathToml);
                string tomlContent = File.ReadAllText(pathToml);
                saveTomlPath = pathToml;

                richToml.Text = tomlContent;
            }
            catch (Exception ex)
            {
               
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveTomlPath != null)
            {
                try
                {
                    File.WriteAllText(saveTomlPath, richToml.Text);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formEditorToml_Resize(object sender, EventArgs e)
        {
            panel1.Size = new Size(ClientSize.Width /2,ClientSize.Height / 2);
            int x = (this.ClientSize.Width - panel1.Width) / 2;
            int y = (this.ClientSize.Height - panel1.Height) / 2;
            panel1.Location = new Point(x, y);
        }
    }
}
