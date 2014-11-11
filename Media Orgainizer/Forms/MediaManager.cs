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

namespace Media_Orgainizer.Forms
{
    public partial class MediaManager : Form
    {
        public MediaManager()
        {
            InitializeComponent();
        }

        private int index = -1;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataManagment.Media.Remove(DataManagment.Media.Last());
            this.Close();
        }

        private void MediaManager_Load(object sender, EventArgs e)
        {
            if (DataManagment.Loaded)
            {
                DataManagment.Load();
            }
            DataManagment.Media.Add("");
            cmbMedia.DataSource = DataManagment.Media;
            cmbMedia.SelectedIndex = cmbMedia.Items.Count - 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (index != -1)
            {
                if (index < (DataManagment.Media.Count - 1))
                {
                    DataManagment.Media.Remove(DataManagment.Media.Last());
                    List<Item> oldKey = DataManagment.Content[DataManagment.Media[index]];
                    DataManagment.Content.Remove(DataManagment.Media[index]);
                    DataManagment.Content.Add(cmbMedia.Text, oldKey);
                }
                else
                {
                    DataManagment.Content.Add(cmbMedia.Text, new List<Item>());
                }
                DataManagment.Media[index] = cmbMedia.Text;
                DataManagment.Save();
                this.Close();
            }
        }

        private void cmbMedia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMedia.SelectedIndex != -1) index = cmbMedia.SelectedIndex;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (index != -1)
            {
                DataManagment.Media.RemoveAt(index);
            }
            DataManagment.Media.Remove(DataManagment.Media.Last());
            this.Close();
        }
    }
}
