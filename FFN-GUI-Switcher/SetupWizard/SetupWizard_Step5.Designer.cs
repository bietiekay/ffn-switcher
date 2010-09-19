namespace FFN_Switcher
{
    partial class SetupWizard_Step5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step5));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.numericUpDown10 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).BeginInit();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox9);
            this.groupBox1.Controls.Add(this.checkBox8);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.numericUpDown10);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(118, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 292);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Verbindung 1";
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Checked = global::FFN_Switcher.Properties.Settings.Default.TSServer1_AlwaysChannelCommander;
            this.checkBox9.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_AlwaysChannelCommander", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox9.Location = new System.Drawing.Point(90, 172);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(154, 17);
            this.checkBox9.TabIndex = 14;
            this.checkBox9.Text = "immer Channel Commander";
            this.toolTip1.SetToolTip(this.checkBox9, "wenn aktiviert wird das Gateway immer\r\nals Channel-Commander eingeloggt\r\nsein.");
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Checked = global::FFN_Switcher.Properties.Settings.Default.TSServer1_WhisperBlocked;
            this.checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox8.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_WhisperBlocked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox8.Location = new System.Drawing.Point(90, 149);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(146, 17);
            this.checkBox8.TabIndex = 13;
            this.checkBox8.Text = "blocke Whisper Anfragen";
            this.toolTip1.SetToolTip(this.checkBox8, "wenn aktiviert wird automatisch das\r\nBlock-Whisper Flag von Teamspeak\r\naktiviert." +
                    "\r\n");
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(14, 126);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Channel PW:";
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_ChannelPW", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox5.Location = new System.Drawing.Point(90, 123);
            this.textBox5.MaxLength = 128;
            this.textBox5.Name = "textBox5";
            this.textBox5.PasswordChar = '*';
            this.textBox5.Size = new System.Drawing.Size(320, 20);
            this.textBox5.TabIndex = 11;
            this.textBox5.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer1_ChannelPW;
            this.toolTip1.SetToolTip(this.textBox5, "Hier muss das Gateway Kanal Passwort\r\n(falls vorhanden) eingegeben werden.\r\n\r\n");
            // 
            // numericUpDown10
            // 
            this.numericUpDown10.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_ChannelID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown10.Location = new System.Drawing.Point(90, 97);
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown10.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numericUpDown10, "Hier muss die Gateway-Kanal ID eingegeben werden.");
            this.numericUpDown10.Value = global::FFN_Switcher.Properties.Settings.Default.TSServer1_ChannelID;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 99);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 13);
            this.label17.TabIndex = 9;
            this.label17.Text = "Channel ID:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 74);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Passwort:";
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(90, 71);
            this.textBox4.MaxLength = 128;
            this.textBox4.Name = "textBox4";
            this.textBox4.PasswordChar = '*';
            this.textBox4.Size = new System.Drawing.Size(320, 20);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer1_Password;
            this.toolTip1.SetToolTip(this.textBox4, "Hier wird das Passwort passend zum Teamspeak\r\nLoginnamen eingegeben.");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(22, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Loginname:";
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_LoginName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(90, 45);
            this.textBox3.MaxLength = 128;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(320, 20);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer1_LoginName;
            this.toolTip1.SetToolTip(this.textBox3, "Hier den Loginnamen der als Nutzername für\r\nden Server verwendet werden soll eing" +
                    "eben.\r\nAchtung: Dies ist nicht der Gateway bzw.\r\nangezeigt Name sondern der Logi" +
                    "nname.\r\n");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Serveradresse:";
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FFN_Switcher.Properties.Settings.Default, "TSServer1_ServerURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(90, 19);
            this.textBox2.MaxLength = 128;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(320, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = global::FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL;
            this.toolTip1.SetToolTip(this.textBox2, "Hier die Serveradresse des Teamspeak Servers\r\nangeben. z.B: voice.ts-ffn.de\r\n");
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step5";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 5 (Teamspeak Server 1)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.NumericUpDown numericUpDown10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}