using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFN_Switcher
{
    public partial class SetupWizard_Step9 : Form
    {
        public SetupWizard_Step9()
        {
            InitializeComponent();
        }

        private void notAccepted_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accepted_Click(object sender, EventArgs e)
        {
            FFN_Switcher.Properties.Settings.Default.Wizard_Step9_Done = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.TeamspeakClientURL = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog = new SaveFileDialog();

            Dialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.Logfile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

    }
}
