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
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebar_Expand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
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

        public void loadForm(Type formType)
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
                    homeSettings = new formSettings();
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

            string hourNow = DateTime.Now.ToString("HH:mm:ss");
            if ((string.Compare(hourNow, "18:00:00") >= 0 && string.Compare(hourNow, "23:59:59") <= 0) ||
                (string.Compare(hourNow, "00:00:00") >= 0 && string.Compare(hourNow, "05:00:00") <= 0))
            {
                pictureBox1.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Wallpaper\\Rdr2_night.png"));
            }
            else if (string.Compare(hourNow, "12:00:00") >= 0 && string.Compare(hourNow, "17:59:59") <= 0)
            {
                pictureBox1.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Wallpaper\\rdr2_afternoon.png"));
            }
            else if (string.Compare(hourNow, "05:00:00") >= 0 && string.Compare(hourNow, "11:59:59") <= 0)
            {
                pictureBox1.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "Images\\Wallpaper\\rdr2_morning.png"));
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

