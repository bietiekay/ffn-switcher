namespace FFN_Switcher
{
    partial class SetupWizard_Step4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard_Step4));
            this.next = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.checkBox7);
            this.groupBox5.Controls.Add(this.numericUpDown8);
            this.groupBox5.Controls.Add(this.checkBox6);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(118, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(416, 292);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Strafbank - wenn das Gateway in einen anderen Channel verschoben wurde";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 65);
            this.label1.TabIndex = 27;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Checked = global::FFN_Switcher.Properties.Settings.Default.JumpBackToChannelAfterWaitPenaltyMinutes;
            this.checkBox7.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "JumpBackToChannelAfterWaitPenaltyMinutes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox7.Location = new System.Drawing.Point(42, 173);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(355, 17);
            this.checkBox7.TabIndex = 26;
            this.checkBox7.Text = "nach der Wartezeit wieder zurück in den Gateway Channel wechseln.";
            this.toolTip1.SetToolTip(this.checkBox7, resources.GetString("checkBox7.ToolTip"));
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FFN_Switcher.Properties.Settings.Default, "WaitPenaltyMinutes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown8.Location = new System.Drawing.Point(42, 113);
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown8.TabIndex = 20;
            this.toolTip1.SetToolTip(this.numericUpDown8, resources.GetString("numericUpDown8.ToolTip"));
            this.numericUpDown8.Value = global::FFN_Switcher.Properties.Settings.Default.WaitPenaltyMinutes;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = global::FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute;
            this.checkBox6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FFN_Switcher.Properties.Settings.Default, "WhenMovedToOtherChannelOutputMute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox6.Location = new System.Drawing.Point(42, 150);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(270, 17);
            this.checkBox6.TabIndex = 23;
            this.checkBox6.Text = "Gateway stummschalten wenn verschoben worden.";
            this.toolTip1.SetToolTip(this.checkBox6, resources.GetString("checkBox6.ToolTip"));
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(85, 110);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(284, 26);
            this.label12.TabIndex = 21;
            this.label12.Text = "Minuten die Position halten wenn Gateway in einen andern\r\nChannel verschoben wurd" +
                "e.";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // SetupWizard_Step4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 345);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.next);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 381);
            this.MinimumSize = new System.Drawing.Size(557, 381);
            this.Name = "SetupWizard_Step4";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFN Switcher Einstellungs Assistent - Schritt 4 (Strafbank Einstellungen)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.NumericUpDown numericUpDown8;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}