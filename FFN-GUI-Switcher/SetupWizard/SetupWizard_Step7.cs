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
    public partial class SetupWizard_Step7 : Form
    {
        public SetupWizard_Step7()
        {
            InitializeComponent();
        }

        private void notAccepted_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accepted_Click(object sender, EventArgs e)
        {
            FFN_Switcher.Properties.Settings.Default.Wizard_Step7_Done = true;
            this.Close();
        }

        private void findfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile = Dialog.FileName.Replace('\\','/');;
            }
        }
    }
}
