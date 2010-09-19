using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;


namespace FFN_Switcher
{
    public partial class MainWindow : Form
    {
        public Switcher internalSwitcher;
        delegate void SetStatusCallback(String value);
        private Int32 Counter = 0;

        public void AddStatusText(String value)
        {
            if (this.StatusListBox.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(AddStatusText);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                if (Counter > 255)
                {
                    Counter = 0;
                    this.StatusListBox.Items.Clear();
                }

                Counter++;

                this.StatusListBox.Items.Add(value);
                this.StatusListBox.SelectedIndex = this.StatusListBox.Items.Count - 1;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

        }

        private void informationenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Informationen infodialog = new Informationen();

            infodialog.ShowDialog();
        }

        private void enableSaveSettings(object sender, EventArgs e)
        {
            einstellungenSpeichernToolStripMenuItem.Enabled = true;
        }

        private void einstellungenSpeichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FFN_Switcher.Properties.Settings.Default.Save();
            einstellungenSpeichernToolStripMenuItem.Enabled = false;
        }

        private void switcherBeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (einstellungenSpeichernToolStripMenuItem.Enabled)
            {
                SaveSettings SaveDialog = new SaveSettings();

                SaveDialog.ShowDialog();
            }

            Application.Exit();
        }

        private void gatewaybeaconfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void connectfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.ConnectFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void disconnectfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.DisconnectFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void gatewaydisabledfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void rogerbeepfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.RogerBeepFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void offlinefilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void teamspeakclientfilebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            Dialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.TeamspeakClientURL = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void logfilebutton_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog = new SaveFileDialog();

            Dialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            Dialog.FilterIndex = 1;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                FFN_Switcher.Properties.Settings.Default.Logfile = Dialog.FileName.Replace('\\', '/'); ;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private void connect_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.ConnectFile))
                {
                    MessageBox.Show("Connect Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile;

                    //player.Play();
                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.ConnectFile);

                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.ConnectFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();
                }
            }
            catch (Exception)
            {
            }
        }

        private void beacon_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile))
                {
                    MessageBox.Show("Baken Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                    //    internalSwitcher.tsProcessor.TeamspeakFlags.OutputMuted = true;

                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile;

                    //player.Play();
                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile);

                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();

                    //if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                    //    internalSwitcher.tsProcessor.TeamspeakFlags.OutputMuted = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void disconnect_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.DisconnectFile))
                {
                    MessageBox.Show("Disconnect Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.DisconnectFile;

                    //player.Play();
                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.DisconnectFile);

                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.DisconnectFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();
                }
            }
            catch (Exception)
            {
            }
        }

        private void deaktiviert_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile))
                {
                    MessageBox.Show("Deaktiviert Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile;

                    //player.Play();
                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile);

                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();
                }
            }
            catch (Exception)
            {
            }
        }

        private void rogerbeep_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.RogerBeepFile))
                {
                    MessageBox.Show("Rogerbeep Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.RogerBeepFile;

                    //player.Play();

                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.RogerBeepFile);

                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.RogerBeepFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();
                }
            }
            catch (Exception)
            {
            }
        }

        private void offline_playbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile))
                {
                    MessageBox.Show("Offline Soundfile konnte nicht gefunden werden.");
                    return;
                }
                else
                {
                    //SoundPlayer player = new SoundPlayer();
                    //player.SoundLocation = FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile;

                    //player.Play();

                    internalSwitcher.tsProcessor.AddPlayBacon(FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOn();
                    //internalSwitcher.tsProcessor.beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile);
                    //internalSwitcher.tsProcessor.serialportProcessor.SwitchOff();
                }
            }
            catch (Exception)
            {
            }
        }

        private void nachUpdateSuchenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateWizard updatewizarddialog = new UpdateWizard();
            updatewizarddialog.ShowDialog();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
