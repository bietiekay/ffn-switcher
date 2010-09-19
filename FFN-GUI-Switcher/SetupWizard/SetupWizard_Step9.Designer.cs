namespace FFN_Switcher
{
    partial class SetupWizard_Step9
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step9));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.ipAddressControl1 = new IPAddressControlLib.IPAddressControl();
            this.numericUpDown12 = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
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
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkBox12);
            this.groupBox6.Controls.Add(this.textBox10);
            this.groupBox6.Controls.Add(this.label26);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.ipAddressControl1);
            this.groupBox6.Controls.Add(this.numericUpDown12);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Location = new System.Drawing.Point(118, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(416, 97);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Webserver Einstellungen";
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Checked = global::FFN_Switcher.Properties.Settings.Default.WebserverVerboseLogging;
            this.checkBox12.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "WebserverVerboseLogging", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox12.Location = new System.Drawing.Point(136, 71);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(122, 17);
            this.checkBox12.TabIndex = 17;
            this.checkBox12.Text = "ausführliches Logfile";
            this.toolTip1.SetToolTip(this.checkBox12, "soll der Webserver ein Logfile führen dass");
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // textBox10
            // 
            this.textBox10.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "WebserverDocumentRoot", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox10.Location = new System.Drawing.Point(136, 45);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(274, 20);
            this.textBox10.TabIndex = 16;
            this.textBox10.Text = global::FFN_Switcher.Properties.Settings.Default.WebserverDocumentRoot;
            this.toolTip1.SetToolTip(this.textBox10, "Hier wird das Verzeichniss eingetragen in welchem");
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(7, 48);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(123, 13);
            this.label26.TabIndex = 15;
            this.label26.Text = "Webseiten Verzeichniss:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(14, 21);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(116, 13);
            this.label25.TabIndex = 14;
            this.label25.Text = "Webserver IP Adresse:";
            // 
            // ipAddressControl1
            // 
            this.ipAddressControl1.AllowInternalTab = false;
            this.ipAddressControl1.AutoHeight = true;
            this.ipAddressControl1.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "WebserverListeningIP", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ipAddressControl1.Location = new System.Drawing.Point(136, 18);
            this.ipAddressControl1.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddressControl1.Name = "ipAddressControl1";
            this.ipAddressControl1.ReadOnly = false;
            this.ipAddressControl1.Size = new System.Drawing.Size(87, 20);
            this.ipAddressControl1.TabIndex = 13;
            this.ipAddressControl1.Text = global::FFN_Switcher.Properties.Settings.Default.WebserverListeningIP;
            this.toolTip1.SetToolTip(this.ipAddressControl1, "Hier soll die IP Adresse eingegeben werden unter");
            // 
            // numericUpDown12
            // 
            this.numericUpDown12.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "WebserverTCPPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown12.Location = new System.Drawing.Point(353, 19);
            this.numericUpDown12.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown12.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown12.Name = "numericUpDown12";
            this.numericUpDown12.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown12.TabIndex = 12;
            this.toolTip1.SetToolTip(this.numericUpDown12, "Hier wird die Portnummer eingestellt unter");
            this.numericUpDown12.Value = global::FFN_Switcher.Properties.Settings.Default.WebserverTCPPort;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(263, 21);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(84, 13);
            this.label24.TabIndex = 11;
            this.label24.Text = "Webserver Port:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Controls.Add(this.textBox11);
            this.groupBox7.Controls.Add(this.label27);
            this.groupBox7.Controls.Add(this.checkBox13);
            this.groupBox7.Location = new System.Drawing.Point(118, 115);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(416, 79);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Teamspeak Client";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(378, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox11
            // 
            this.textBox11.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TeamspeakClientURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox11.Location = new System.Drawing.Point(112, 19);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(260, 20);
            this.textBox11.TabIndex = 18;
            this.textBox11.Text = global::FFN_Switcher.Properties.Settings.Default.TeamspeakClientURL;
            this.toolTip1.SetToolTip(this.textBox11, "Hier wird der komplette Pfad und Dateiname des");
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(11, 22);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(95, 13);
            this.label27.TabIndex = 17;
            this.label27.Text = "Teamspeak Client:";
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Checked = global::FFN_Switcher.Properties.Settings.Default.StartTeamspeakClientAtStartup;
            this.checkBox13.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox13.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "StartTeamspeakClientAtStartup", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox13.Location = new System.Drawing.Point(112, 45);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(142, 30);
            this.checkBox13.TabIndex = 0;
            this.checkBox13.Text = "beim Start des Switchers\r\nautomatisch starten.";
            this.toolTip1.SetToolTip(this.checkBox13, "wenn diese Option aktiviert ist wird der Switcher");
            this.checkBox13.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.button2);
            this.groupBox8.Controls.Add(this.checkBox15);
            this.groupBox8.Controls.Add(this.textBox12);
            this.groupBox8.Controls.Add(this.label28);
            this.groupBox8.Controls.Add(this.checkBox14);
            this.groupBox8.Location = new System.Drawing.Point(118, 200);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(416, 104);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Logfile";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(378, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(32, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Checked = global::FFN_Switcher.Properties.Settings.Default.LogToScreenEnabled;
            this.checkBox15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox15.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "LogToScreenEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox15.Location = new System.Drawing.Point(58, 68);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(304, 17);
            this.checkBox15.TabIndex = 21;
            this.checkBox15.Text = "sollen die Logmeldungen im Status-Tab angezeigt werden?";
            this.toolTip1.SetToolTip(this.checkBox15, "Sollen die Logmeldungen im Status-Tab des Switchers dargestellt werden?");
            this.checkBox15.UseVisualStyleBackColor = true;
            // 
            // textBox12
            // 
            this.textBox12.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "Logfile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox12.Location = new System.Drawing.Point(58, 19);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(314, 20);
            this.textBox12.TabIndex = 20;
            this.textBox12.Text = global::FFN_Switcher.Properties.Settings.Default.Logfile;
            this.toolTip1.SetToolTip(this.textBox12, "Hier wird der Pfad und Dateiname des Logfiles");
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(11, 22);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(41, 13);
            this.label28.TabIndex = 19;
            this.label28.Text = "Logfile:";
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Checked = global::FFN_Switcher.Properties.Settings.Default.LogToFileEnabled;
            this.checkBox14.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "LogToFileEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox14.Location = new System.Drawing.Point(58, 45);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(211, 17);
            this.checkBox14.TabIndex = 0;
            this.checkBox14.Text = "soll in das Logfile geschrieben werden?";
            this.toolTip1.SetToolTip(this.checkBox14, "Soll das Logfile auf Festplatte geschrieben werden?");
            this.checkBox14.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step9
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step9";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 9 (weiteres)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private IPAddressControlLib.IPAddressControl ipAddressControl1;
        private System.Windows.Forms.NumericUpDown numericUpDown12;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}