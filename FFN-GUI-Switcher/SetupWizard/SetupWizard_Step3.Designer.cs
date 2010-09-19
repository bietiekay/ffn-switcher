namespace FFN_Switcher
{
    partial class SetupWizard_Step3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step3));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label39 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown9 = new System.Windows.Forms.NumericUpDown();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label39);
            this.groupBox4.Controls.Add(this.numericUpDown1);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.numericUpDown9);
            this.groupBox4.Controls.Add(this.checkBox5);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.numericUpDown7);
            this.groupBox4.Controls.Add(this.checkBox4);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.numericUpDown6);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.numericUpDown5);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.numericUpDown4);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.numericUpDown2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(118, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(416, 292);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TimeOuts und Channel Verhalten";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(115, 40);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(240, 13);
            this.label39.TabIndex = 28;
            this.label39.Text = "Sekunden auf den erfolgreichen Connect warten.";
            this.toolTip1.SetToolTip(this.label39, "Diese Zeit in Sekunden gibt an wie lange der\r\nSwitcher auf eine erfolgreiche Verb" +
                    "indung mit\r\ndem Teamspeak Server wartet.\r\n");
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "WaitForConnectSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.Location = new System.Drawing.Point(46, 38);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 26;
            this.toolTip1.SetToolTip(this.numericUpDown1, "Diese Zeit in Sekunden gibt an wie lange der\r\nSwitcher auf eine erfolgreiche Verb" +
                    "indung mit\r\ndem Teamspeak Server wartet.");
            this.numericUpDown1.Value = global::FFN_Switcher.Properties.Settings.Default.WaitForConnectSeconds;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(89, 241);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(301, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "mal überprüfen ob Teamspeak den Switcher anlügt oder nicht.";
            // 
            // numericUpDown9
            // 
            this.numericUpDown9.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "NumberOfChecksIfTeamspeakIsLying", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown9.Location = new System.Drawing.Point(46, 239);
            this.numericUpDown9.Name = "numericUpDown9";
            this.numericUpDown9.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown9.TabIndex = 24;
            this.toolTip1.SetToolTip(this.numericUpDown9, resources.GetString("numericUpDown9.ToolTip"));
            this.numericUpDown9.Value = global::FFN_Switcher.Properties.Settings.Default.NumberOfChecksIfTeamspeakIsLying;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = global::FFN_Switcher.Properties.Settings.Default.FlagChannelCommanderWhenSending;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "FlagChannelCommanderWhenSending", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox5.Location = new System.Drawing.Point(46, 136);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(293, 17);
            this.checkBox5.TabIndex = 22;
            this.checkBox5.Text = "Channel Commander Flag setzen wenn Gateway sendet.";
            this.toolTip1.SetToolTip(this.checkBox5, "Channel Commander Flag bedeutet wenn das Gateway\r\nsendet und Spache dabei übertra" +
                    "gen wird leuchtet die \r\n(sonst grüne Lampe) nun ROT auf und signalisiert den\r\nak" +
                    "tiven Betrieb des Gateways.");
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(89, 215);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(263, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Sekunden warten bevor ein Reconnect versucht wird.";
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "AutoReconnectSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown7.Location = new System.Drawing.Point(46, 213);
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown7.TabIndex = 17;
            this.toolTip1.SetToolTip(this.numericUpDown7, resources.GetString("numericUpDown7.ToolTip"));
            this.numericUpDown7.Value = global::FFN_Switcher.Properties.Settings.Default.AutoReconnectSeconds;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = global::FFN_Switcher.Properties.Settings.Default.AutoReconnect;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "AutoReconnect", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Location = new System.Drawing.Point(46, 113);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(275, 17);
            this.checkBox4.TabIndex = 16;
            this.checkBox4.Text = "bei Verbindungsabbruch automatisch neu verbinden.";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(253, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Minuten.";
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "WatchdogMinutes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown6.Location = new System.Drawing.Point(210, 185);
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown6.TabIndex = 13;
            this.toolTip1.SetToolTip(this.numericUpDown6, resources.GetString("numericUpDown6.ToolTip"));
            this.numericUpDown6.Value = global::FFN_Switcher.Properties.Settings.Default.WatchdogMinutes;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(42, 187);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(163, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Maximallänge eines Durchgangs:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(148, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Millisekunden auf Nachtasten.";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "DelayAfterSendingMilliSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown5.Location = new System.Drawing.Point(84, 87);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown5.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numericUpDown5, resources.GetString("numericUpDown5.ToolTip"));
            this.numericUpDown5.Value = global::FFN_Switcher.Properties.Settings.Default.DelayAfterSendingMilliSeconds;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Warte";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(253, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Sekunden.";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "DelaySeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown4.Location = new System.Drawing.Point(210, 159);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown4.TabIndex = 7;
            this.toolTip1.SetToolTip(this.numericUpDown4, "Der hier definierte Wert ist sozusagen die unterste Grenze\r\ndie ein Durchgang max" +
                    "imal von der Länge haben sollte.\r\n");
            this.numericUpDown4.Value = global::FFN_Switcher.Properties.Settings.Default.DelaySeconds;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Mindestlänge eines Durchgangs:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "UseServer2AfterNumberOfFailedReconnects", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown2.Location = new System.Drawing.Point(323, 59);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown2.TabIndex = 5;
            this.toolTip1.SetToolTip(this.numericUpDown2, "Nach dieser Menge an gescheiterten Verbindungsversuchen\r\nverwendet der Switcher a" +
                    "utomatisch den Alternativen Server 2.\r\n");
            this.numericUpDown2.Value = global::FFN_Switcher.Properties.Settings.Default.UseServer2AfterNumberOfFailedReconnects;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(274, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Verwende alternative Verbindung nach sovielen Fehlern:";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step3";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 3 (Timeout Einstellungen)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown9;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}