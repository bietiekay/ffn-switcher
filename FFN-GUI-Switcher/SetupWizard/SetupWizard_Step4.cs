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
    public partial class SetupWizard_Step4 : Form
    {
        public SetupWizard_Step4()
        {
            InitializeComponent();
        }

        private void notAccepted_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accepted_Click(object sender, EventArgs e)
        {
            FFN_Switcher.Properties.Settings.Default.Wizard_Step4_Done = true;
            this.Close();
        }
    }
}
