using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rss;
using System.Net;

namespace FFN_Switcher
{
    public partial class UpdateWizard : Form
    {
        public UpdateWizard()
        {
            InitializeComponent();
        }

        private bool UpdateAvailable;
        private String UpdateURL2;

        private void UpdateWizard_Load(object sender, EventArgs e)
        {
            Version.Text = FFN_Switcher.Version.VersionString;

            try
            {
                String UpdateURL = "http://dropbox.schrankmonster.de/dropped/FFNSwitcher.xml";
                RssFeed feed = RssFeed.Read(UpdateURL);

                if (Convert.ToInt32(feed.Channels[0].Items[0].Title) <= FFN_Switcher.Version.VersionNumber)
                //if (Convert.ToInt32(feed.Channels[0].Items[0].Title) <= 90)
                {
                    // no update available
                    availableVersion.Text = feed.Channels[0].Items[0].Comments + " (installierte Version)";
                    UpdateAvailable = false;
                    UpdateInformationTextbox.Enabled = false;
                    UpdateInformationTextbox.Text = feed.Channels[0].Items[0].Description.Replace("\n", "\r\n");
                    UpdateButton.Text = "kein Update verfügbar";
                    UpdateURL2 = feed.Channels[0].Items[0].Link.AbsoluteUri;
                    UpdateButton.Enabled = false;
                }
                else
                {
                    // if newer version available
                    availableVersion.Text = feed.Channels[0].Items[0].Comments + " (neue Version)";
                    UpdateAvailable = true;
                    UpdateInformationTextbox.Text = feed.Channels[0].Items[0].Description.Replace("\n", "\r\n"); 
                    UpdateInformationTextbox.Enabled = true;

                    UpdateURL2 = feed.Channels[0].Items[0].Link.AbsoluteUri;


                    // download ...
                    //WebClient Client = new WebClient();
                    //Client.DownloadFile(feed.Channels[0].Items[0].Link, "Update.7z");

                    UpdateButton.Enabled = true;
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (UpdateAvailable)
            {
                SaveFileDialog Dialog = new SaveFileDialog();

                Dialog.Title = "Wohin soll das Archiv der neuen Version gespeichert werden.";
                Dialog.Filter = "All files (*.zip)|*.zip";
                Dialog.FilterIndex = 0;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    WebClient Client = new WebClient();
                    Client.DownloadFile(UpdateURL2, Dialog.FileName);
                    MessageBox.Show("Download fertig. Bitte selbst entpacken und die Dateien entsprechend ersetzen.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
