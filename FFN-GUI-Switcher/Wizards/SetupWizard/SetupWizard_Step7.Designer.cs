namespace FFN_Switcher
{
    partial class SetupWizard_Step7
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step7));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.findfile = new System.Windows.Forms.Button();
            this.label32 = new System.Windows.Forms.Label();
            this.numericUpDown14 = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.numericUpDown13 = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown13)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(454, 310);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(80, 23);
            this.next.TabIndex = 0;
            this.next.Text = "weiter";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.accepted_Click);
            // 
            // abort
            // 
            this.abort.Location = new System.Drawing.Point(368, 310);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(80, 23);
            this.abort.TabIndex = 1;
            this.abort.Text = "abbrechen";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.notAccepted_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FFN_Switcher.Properties.Resources.ffn_switcher_logo_90;
            this.pictureBox1.Location = new System.Drawing.Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(99, 292);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.findfile);
            this.groupBox9.Controls.Add(this.label32);
            this.groupBox9.Controls.Add(this.numericUpDown14);
            this.groupBox9.Controls.Add(this.label33);
            this.groupBox9.Controls.Add(this.label31);
            this.groupBox9.Controls.Add(this.numericUpDown13);
            this.groupBox9.Controls.Add(this.label30);
            this.groupBox9.Controls.Add(this.checkBox18);
            this.groupBox9.Controls.Add(this.checkBox17);
            this.groupBox9.Controls.Add(this.checkBox16);
            this.groupBox9.Controls.Add(this.label29);
            this.groupBox9.Controls.Add(this.textBox13);
            this.groupBox9.Location = new System.Drawing.Point(118, 12);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(416, 292);
            this.groupBox9.TabIndex = 7;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Gateway Bake";
            // 
            // findfile
            // 
            this.findfile.Location = new System.Drawing.Point(378, 21);
            this.findfile.Name = "findfile";
            this.findfile.Size = new System.Drawing.Size(32, 23);
            this.findfile.TabIndex = 15;
            this.findfile.Text = "...";
            this.toolTip1.SetToolTip(this.findfile, "Hier die Sounddatei angeben die als Gateway-Bake\r\ngespielt werden soll.");
            this.findfile.UseVisualStyleBackColor = true;
            this.findfile.Click += new System.EventHandler(this.findfile_Click);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(270, 163);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(95, 13);
            this.label32.TabIndex = 14;
            this.label32.Text = "Sekunden spielen.";
            // 
            // numericUpDown14
            // 
            this.numericUpDown14.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "MaximumBeaconPlayTimeSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown14.Location = new System.Drawing.Point(221, 160);
            this.numericUpDown14.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown14.Name = "numericUpDown14";
            this.numericUpDown14.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown14.TabIndex = 13;
            this.toolTip1.SetToolTip(this.numericUpDown14, "Wie lang darf die Bake maximal sein. Diese\r\nEinstellung wird aus Sicherheitsgründ" +
                    "en\r\ngetroffen um ein zu langes Träger-Halten \r\nzu verhindern.");
            this.numericUpDown14.Value = global::FFN_Switcher.Properties.Settings.Default.MaximumBeaconPlayTimeSeconds;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(143, 163);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(72, 13);
            this.label33.TabIndex = 12;
            this.label33.Text = "Bake maximal";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(270, 137);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(108, 13);
            this.label31.TabIndex = 11;
            this.label31.Text = "Minuten wiederholen.";
            // 
            // numericUpDown13
            // 
            this.numericUpDown13.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "GatewayBeaconReplayInMinutes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown13.Location = new System.Drawing.Point(221, 134);
            this.numericUpDown13.Name = "numericUpDown13";
            this.numericUpDown13.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown13.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numericUpDown13, "In welchem Zeitabstand in Minuten soll\r\ndie Gateway Bake abgespielt werden?\r\n");
            this.numericUpDown13.Value = global::FFN_Switcher.Properties.Settings.Default.GatewayBeaconReplayInMinutes;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(119, 137);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(96, 13);
            this.label30.TabIndex = 9;
            this.label30.Text = "Gateway Bake alle";
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Checked = global::FFN_Switcher.Properties.Settings.Default.ReplayGatewayBeaconOnlyWhenActivity;
            this.checkBox18.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "ReplayGatewayBeaconOnlyWhenActivity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox18.Enabled = false;
            this.checkBox18.Location = new System.Drawing.Point(122, 95);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(203, 30);
            this.checkBox18.TabIndex = 8;
            this.checkBox18.Text = "Gateway Bake nur wiederholen wenn\r\nAktivität auf dem Gateway";
            this.toolTip1.SetToolTip(this.checkBox18, "Soll die Bake nur dann gesendet und wiederholt\r\nwerden wenn Aktivität auf dem Gat" +
                    "eway stattfindet?\r\n");
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Checked = global::FFN_Switcher.Properties.Settings.Default.ReplayGatewayBeacon;
            this.checkBox17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox17.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "ReplayGatewayBeacon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox17.Location = new System.Drawing.Point(122, 72);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(156, 17);
            this.checkBox17.TabIndex = 7;
            this.checkBox17.Text = "Gateway Bake wiederholen";
            this.toolTip1.SetToolTip(this.checkBox17, "Soll die Bake nur einmal beim Starten abgespielt\r\nwerden (deaktiviert) oder soll " +
                    "die Bake immer\r\nwiederholt werden (aktiviert). Natürlich bei\r\nWiederholung der B" +
                    "ake die gewählte Zeit in\r\nMinuten.\r\n");
            this.checkBox17.UseVisualStyleBackColor = true;
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Checked = global::FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon;
            this.checkBox16.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox16.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "MuteOutputWhenPlayingBeacon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox16.Location = new System.Drawing.Point(122, 49);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(270, 17);
            this.checkBox16.TabIndex = 6;
            this.checkBox16.Text = "Gateway stummschalten wenn Bake gesendet wird.";
            this.toolTip1.SetToolTip(this.checkBox16, "Soll das Gateway in Teamspeak solange stumm\r\ngeschaltet werden während die Bake a" +
                    "bgespielt\r\nwird. Dies wird auch durch das \"Kopfhörersymbol\"\r\nin Teamspeak andere" +
                    "n Nutzern signalisiert.");
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(7, 26);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(109, 13);
            this.label29.TabIndex = 5;
            this.label29.Text = "Gateway Bakendatei:";
            // 
            // textBox13
            // 
            this.textBox13.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "GatewayBeaconFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox13.Location = new System.Drawing.Point(122, 23);
            this.textBox13.MaxLength = 128;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(250, 20);
            this.textBox13.TabIndex = 4;
            this.textBox13.Text = global::FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile;
            this.toolTip1.SetToolTip(this.textBox13, "Hier die Sounddatei angeben die als Gateway-Bake");
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step7";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 7 (Baken)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown13)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown numericUpDown14;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown numericUpDown13;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Button findfile;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}