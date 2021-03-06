﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFN_Switcher
{
    public partial class SetupWizard_Step8 : Form
    {
        public SetupWizard_Step8()
        {
            InitializeComponent();
        }

        private void notAccepted_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accepted_Click(object sender, EventArgs e)
        {
            FFN_Switcher.Properties.Settings.Default.Wizard_Step8_Done = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.ConnectFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.DisconnectFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.RogerBeepFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }
    }
}
