namespace FFN_Switcher
{
    partial class SetupWizard_Step6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step6));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.numericUpDown11 = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).BeginInit();
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox10);
            this.groupBox2.Controls.Add(this.checkBox11);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.numericUpDown11);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Location = new System.Drawing.Point(118, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 292);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Verbindung 2";
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Checked = global::FFN_Switcher.Properties.Settings.Default.TSServer2_AlwaysChannelCommander;
            this.checkBox10.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_AlwaysChannelCommander", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox10.Location = new System.Drawing.Point(90, 172);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(154, 17);
            this.checkBox10.TabIndex = 26;
            this.checkBox10.Text = "immer Channel Commander";
            this.toolTip1.SetToolTip(this.checkBox10, "wenn aktiviert wird das Gateway immer\r\nals Channel-Commander eingeloggt\r\nsein.");
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Checked = global::FFN_Switcher.Properties.Settings.Default.TSServer2_WhisperBlocked;
            this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox11.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_WhisperBlocked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox11.Location = new System.Drawing.Point(90, 149);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(146, 17);
            this.checkBox11.TabIndex = 25;
            this.checkBox11.Text = "blocke Whisper Anfragen";
            this.toolTip1.SetToolTip(this.checkBox11, "wenn aktiviert wird automatisch das\r\nBlock-Whisper Flag von Teamspeak\r\naktiviert." +
                    "\r\n");
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(14, 126);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 13);
            this.label19.TabIndex = 24;
            this.label19.Text = "Channel PW:";
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_ChannelPW", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox6.Location = new System.Drawing.Point(90, 123);
            this.textBox6.MaxLength = 128;
            this.textBox6.Name = "textBox6";
            this.textBox6.PasswordChar = '*';
            this.textBox6.Size = new System.Drawing.Size(320, 20);
            this.textBox6.TabIndex = 23;
            this.textBox6.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer2_ChannelPW;
            this.toolTip1.SetToolTip(this.textBox6, "Hier muss das Gateway Kanal Passwort\r\n(falls vorhanden) eingegeben werden.\r\n\r\n");
            // 
            // numericUpDown11
            // 
            this.numericUpDown11.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_ChannelID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown11.Location = new System.Drawing.Point(90, 97);
            this.numericUpDown11.Name = "numericUpDown11";
            this.numericUpDown11.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown11.TabIndex = 22;
            this.toolTip1.SetToolTip(this.numericUpDown11, "Hier muss die Gateway-Kanal ID eingegeben werden.");
            this.numericUpDown11.Value = global::FFN_Switcher.Properties.Settings.Default.TSServer2_ChannelID;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(21, 99);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(63, 13);
            this.label20.TabIndex = 21;
            this.label20.Text = "Channel ID:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(31, 74);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "Passwort:";
            // 
            // textBox7
            // 
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox7.Location = new System.Drawing.Point(90, 71);
            this.textBox7.MaxLength = 128;
            this.textBox7.Name = "textBox7";
            this.textBox7.PasswordChar = '*';
            this.textBox7.Size = new System.Drawing.Size(320, 20);
            this.textBox7.TabIndex = 19;
            this.textBox7.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer2_Password;
            this.toolTip1.SetToolTip(this.textBox7, "Hier wird das Passwort passend zum Teamspeak\r\nLoginnamen eingegeben.");
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(22, 48);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(62, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "Loginname:";
            // 
            // textBox8
            // 
            this.textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_LoginName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox8.Location = new System.Drawing.Point(90, 45);
            this.textBox8.MaxLength = 128;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(320, 20);
            this.textBox8.TabIndex = 17;
            this.textBox8.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer2_LoginName;
            this.toolTip1.SetToolTip(this.textBox8, "Hier den Loginnamen der als Nutzername für\r\nden Server verwendet werden soll eing" +
                    "eben.\r\nAchtung: Dies ist nicht der Gateway bzw.\r\nangezeigt Name sondern der Logi" +
                    "nname.\r\n");
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 22);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(78, 13);
            this.label23.TabIndex = 16;
            this.label23.Text = "Serveradresse:";
            // 
            // textBox9
            // 
            this.textBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer2_ServerURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox9.Location = new System.Drawing.Point(90, 19);
            this.textBox9.MaxLength = 128;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(320, 20);
            this.textBox9.TabIndex = 15;
            this.textBox9.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer2_ServerURL;
            this.toolTip1.SetToolTip(this.textBox9, "Hier die Serveradresse des Teamspeak Servers\r\nangeben. z.B: voice.ts-ffn.de");
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step6";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 6 (Teamspeak Server 2)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.NumericUpDown numericUpDown11;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}