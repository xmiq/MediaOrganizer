using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Media_Orgainizer.Classes;
using Media_Orgainizer.Forms;

namespace Media_Orgainizer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            if (!DataManagment.Loaded)
            {
                DataManagment.Load();
            }
        }

        private void mediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaManager mm = new MediaManager();
            mm.Show();
            mm.FormClosed += mm_FormClosed;
        }

        void mm_FormClosed(object sender, FormClosedEventArgs e)
        {
            flpMedia.Controls.Clear();
            AddMediaRadioButtons();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManagment.Save();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            AddMediaRadioButtons();
            if (flpMedia.Controls.Count > 0) rb_CheckedChanged(flpMedia.Controls[0], e);
        }

        private void AddMediaRadioButtons()
        {
            bool first = false;
            foreach (string s in DataManagment.Media)
            {
                RadioButton rb = new RadioButton();
                rb.Name = "rb" + s;
                rb.Text = s;
                if (!first)
                {
                    rb.Checked = true;
                    first = true;
                }
                rb.CheckedChanged += rb_CheckedChanged;
                flpMedia.Controls.Add(rb);
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                flpContent.Controls.Clear();
                foreach (Item i in DataManagment.Content[selectedMedia()])
                {
                    if (i.Type == Series.SeasonType)
                    {
                        NewSeries(i as Series);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataManagment.Save();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataManagment.Save(fbd.SelectedPath);
                MessageBox.Show("Exported Successfully");
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataManagment.Load(fbd.SelectedPath);
                DataManagment.Save();
                MessageBox.Show("Imported Successfully");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (flpMedia.Controls.Count > 0) NewSeries();
        }

        void pb_Click(object sender, EventArgs e)
        {
            PictureBox self = sender as PictureBox;
            Panel p = self.Parent as Panel;
            TextBox Name = p.Controls[1] as TextBox;
            NumericUpDown num = p.Controls[3] as NumericUpDown;
            NumericUpDown num2 = p.Controls[5] as NumericUpDown;

            int index = flpContent.Controls.IndexOf(p);
            string media = selectedMedia();
            if (index == DataManagment.Content[media].Count) DataManagment.SaveSeries(media, Name.Text, (int)num.Value, (int)num2.Value);
            else
            {
                DataManagment.Content[media][index] = new Series()
                {
                    Name = Name.Text,
                    Season = (int)num.Value,
                    Episode = (int)num2.Value
                };
                DataManagment.Save();
            }
        }

        void pb2_Click(object sender, EventArgs e)
        {
            PictureBox self = sender as PictureBox;
            Panel p = self.Parent as Panel;
            int index = flpContent.Controls.IndexOf(p);
            DataManagment.Content[selectedMedia()].RemoveAt(index);
            flpContent.Controls.RemoveAt(index);
        }

        private string selectedMedia()
        {
            foreach (Control c in flpMedia.Controls)
            {
                if ((c as RadioButton).Checked)
                {
                    return (c as RadioButton).Text;
                }
            }
            return "";
        }

        private void NewSeries()
        {
            NewSeries("", 0, 0);
        }

        private void NewSeries(Series s)
        {
            NewSeries(s.Name, s.Season, s.Episode);
        }

        private void NewSeries(string name, int season, int episode)
        {
            Panel p = new Panel();
            p.AutoSize = true;
            Label Name = new Label();
            Name.Text = "Name:";
            Name.AutoSize = true;
            Name.Top = 2;
            p.Controls.Add(Name);
            TextBox txt = new TextBox();
            txt.Size = new System.Drawing.Size(150, 20);
            txt.Left = Name.Width + 10;
            if (name != "") txt.Text = name;
            p.Controls.Add(txt);
            Label Season = new Label();
            Season.Text = "Season:";
            Season.AutoSize = true;
            Season.Top = 2;
            Season.Left = Name.Width + 170;
            p.Controls.Add(Season);
            NumericUpDown num = new NumericUpDown();
            num.Minimum = 1;
            num.AutoSize = true;
            num.Left = Name.Width + Season.Width + 180;
            num.Width = 30;
            if (season != 0) num.Value = season;
            p.Controls.Add(num);
            Label Episode = new Label();
            Episode.Text = "Episode:";
            Episode.AutoSize = true;
            Episode.Top = 2;
            Episode.Left = Name.Width + Season.Width + 230;
            p.Controls.Add(Episode);
            NumericUpDown num2 = new NumericUpDown();
            num2.Minimum = 1;
            num2.AutoSize = true;
            num2.Left = Name.Width + Season.Width + Episode.Width + 240;
            num2.Width = 30;
            if (episode != 0) num2.Value = episode;
            p.Controls.Add(num2);
            PictureBox pb = new PictureBox();
            pb.Size = new System.Drawing.Size(20, 20);
            pb.Image = Properties.Resources.ok;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Left = Name.Width + Season.Width + Episode.Width + 290;
            pb.Click += pb_Click;
            p.Controls.Add(pb);
            PictureBox pb2 = new PictureBox();
            pb2.Size = new System.Drawing.Size(20, 20);
            pb2.Image = Properties.Resources.delete;
            pb2.SizeMode = PictureBoxSizeMode.StretchImage;
            pb2.Left = Name.Width + Season.Width + Episode.Width + 320;
            pb2.Click += pb2_Click;
            p.Controls.Add(pb2);
            flpContent.Controls.Add(p);
        }
    }
}
