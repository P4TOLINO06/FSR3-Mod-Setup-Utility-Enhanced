using System.Diagnostics;
using System.Windows.Forms;

namespace FSR3ModSetupUtilityEnhanced
{
    public partial class mainForm : Form
    {
        bool sidebar_Expand;
        private formHome homeForm;
        private formSettings homeSettings;
        private formGuide homeGuide;
        private formEditorToml homeEditor;
        private static mainForm instance;
        public string nullHomeEditor { get; set; }

        public mainForm()
        {

            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebar_Expand)
            {
                sidebar.Width -= 15;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebar_Expand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 15;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebar_Expand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void loadForm(Type formType, string pathT = null)
        {
            Form formToShow = null;

            if (formType == typeof(formHome))
            {
                if (homeForm == null)
                {
                    homeForm = new formHome();
                    homeForm.TopLevel = false;
                    homeForm.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeForm);
                    this.mainPanel.Tag = homeForm;
                    homeForm.Show();
                }
                else
                {
                    homeForm.BringToFront();
                }
                formToShow = homeForm;
            }
            else if (formType == typeof(formSettings))
            {
                if (homeSettings == null)
                {
                    homeSettings = formSettings.Instance;
                    homeSettings.TopLevel = false;
                    homeSettings.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeSettings);
                    this.mainPanel.Tag = homeSettings;
                    homeSettings.Show();
                }
                else
                {
                    homeSettings.BringToFront();
                }
                formToShow = homeSettings;
            }
            else if (formType == typeof(formGuide))
            {
                if (homeGuide == null)
                {
                    homeGuide = new formGuide();
                    homeGuide.TopLevel = false;
                    homeGuide.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeGuide);
                    this.mainPanel.Tag = homeGuide;
                    homeGuide.Show();
                }
                else
                {
                    homeGuide.BringToFront();
                }
                formToShow = homeGuide;
            }

            else if (formType == typeof(formEditorToml))
            {
                if (homeEditor == null)
                {
                    homeEditor = new formEditorToml();
                    homeEditor.TopLevel = false;
                    homeEditor.Dock = DockStyle.Fill;
                    this.mainPanel.Controls.Add(homeEditor);
                    this.mainPanel.Tag = homeEditor;
                    homeEditor.Show();
                }
                else
                {
                    homeEditor.BringToFront();
                }

                if (pathT != null)
                {
                    homeEditor.SetPathT(pathT); 
                }

                formToShow = homeEditor;
                if (nullHomeEditor == null)
                {
                    homeEditor = null;
                }
            }
            else
            {
                if (this.mainPanel.Controls.Count > 0 && this.mainPanel.Controls[0].GetType() == formType)
                    return;

                if (this.mainPanel.Controls.Count > 0)
                    this.mainPanel.Controls.RemoveAt(0);

                Form f = (Form)Activator.CreateInstance(formType);
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                this.mainPanel.Controls.Add(f);
                this.mainPanel.Tag = f;
                f.Show();
                formToShow = f;
            }

            if (formToShow != null)
            {
                formToShow.BringToFront();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void sidebar_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)

        {
            sidebar.Height = this.ClientSize.Height - sidebar.Top;
            mainPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - mainPanel.Top);
            pictureBox1.Size = this.ClientSize;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void mainPanel_Load(object sender, EventArgs e)
        {
            clockTimer.Start();

            var imagePaths = new Dictionary<string, List<string>>()
            {
                { "morning", new List<string> {
                    "Images\\Wallpaper\\rdr2_morning.png",
                    "Images\\Wallpaper\\rdr2_morning2.png",
                    "Images\\Wallpaper\\rdr2_morning3.png",
                    "Images\\Wallpaper\\rdr2_morning4.png",
                    "Images\\Wallpaper\\rdr2_morning5.png"
                }},
                { "afternoon", new List<string> {
                    "Images\\Wallpaper\\rdr2_afternoon.png",
                    "Images\\Wallpaper\\rdr2_afternoon2.png",
                    "Images\\Wallpaper\\rdr2_afternoon3.png",
                    "Images\\Wallpaper\\rdr2_afternoon4.png",
                    "Images\\Wallpaper\\rdr2_afternoon5.png",
                }},
                { "night", new List<string> {
                    "Images\\Wallpaper\\rdr2_night.png",
                    "Images\\Wallpaper\\rdr2_night2.png",
                    "Images\\Wallpaper\\rdr2_night3.png",
                    "Images\\Wallpaper\\rdr2_night4.png",
                    "Images\\Wallpaper\\rdr2_night5.png",
                }}
            };

            string hourNow = DateTime.Now.ToString("HH:mm:ss");

            string period = hourNow switch
            {
                _ when (string.Compare(hourNow, "18:00:00") >= 0 && string.Compare(hourNow, "23:59:59") <= 0) ||
                             (string.Compare(hourNow, "00:00:00") >= 0 && string.Compare(hourNow, "05:00:00") <= 0) => "night",
                _ when string.Compare(hourNow, "12:00:00") >= 0 && string.Compare(hourNow, "17:59:59") <= 0 => "afternoon",
                _ => "morning"
            };

            if (imagePaths.ContainsKey(period))
            {
                var initialImages = imagePaths[period];
                var randomImage = initialImages[new Random().Next(initialImages.Count)];
                string fullPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, randomImage);

                if (File.Exists(fullPath))
                {
                    pictureBox1.Image = Image.FromFile(fullPath);
                }

            }
            mainPanel.BackgroundImage = pictureBox1.Image;
            this.BackgroundImage = pictureBox1.Image;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            loadForm(typeof(formHome));
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            loadForm(typeof(formSettings));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadForm(typeof(formGuide));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this)
                {
                    form.Hide();
                    if (form == homeForm)
                    {
                        homeForm = null;
                    }
                    else if (form == homeSettings)
                    {
                        homeSettings = null;
                    }
                    else if (form == homeGuide)
                    {
                        homeGuide = null;
                    }
                }
            }
        }
        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void clock_Click(object sender, EventArgs e)
        {

        }

        private void clockLabel_Click(object sender, EventArgs e)
        {

        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            DateOwLabel.Text = DateTime.Now.DayOfWeek.ToString();
            Date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}

