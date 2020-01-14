namespace CallibriFeatures.Devices
{
    partial class SignalChartSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.HorizontalScanComboBox = new System.Windows.Forms.ComboBox();
            this.HorizontalScanLabel = new System.Windows.Forms.Label();
            this.VerticalScanComboBox = new System.Windows.Forms.ComboBox();
            this.VerticalScanLabel = new System.Windows.Forms.Label();
            this.SettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.HorizontalScanComboBox);
            this.SettingsGroupBox.Controls.Add(this.HorizontalScanLabel);
            this.SettingsGroupBox.Controls.Add(this.VerticalScanComboBox);
            this.SettingsGroupBox.Controls.Add(this.VerticalScanLabel);
            this.SettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(198, 78);
            this.SettingsGroupBox.TabIndex = 1;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Plot settings";
            // 
            // HorizontalScanComboBox
            // 
            this.HorizontalScanComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HorizontalScanComboBox.FormattingEnabled = true;
            this.HorizontalScanComboBox.Location = new System.Drawing.Point(96, 44);
            this.HorizontalScanComboBox.Name = "HorizontalScanComboBox";
            this.HorizontalScanComboBox.Size = new System.Drawing.Size(96, 21);
            this.HorizontalScanComboBox.TabIndex = 3;
            // 
            // HorizontalScanLabel
            // 
            this.HorizontalScanLabel.AutoSize = true;
            this.HorizontalScanLabel.Location = new System.Drawing.Point(7, 47);
            this.HorizontalScanLabel.Name = "HorizontalScanLabel";
            this.HorizontalScanLabel.Size = new System.Drawing.Size(83, 13);
            this.HorizontalScanLabel.TabIndex = 2;
            this.HorizontalScanLabel.Text = "Horizontal scan:";
            // 
            // VerticalScanComboBox
            // 
            this.VerticalScanComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VerticalScanComboBox.FormattingEnabled = true;
            this.VerticalScanComboBox.Location = new System.Drawing.Point(96, 17);
            this.VerticalScanComboBox.Name = "VerticalScanComboBox";
            this.VerticalScanComboBox.Size = new System.Drawing.Size(96, 21);
            this.VerticalScanComboBox.TabIndex = 1;
            // 
            // VerticalScanLabel
            // 
            this.VerticalScanLabel.AutoSize = true;
            this.VerticalScanLabel.Location = new System.Drawing.Point(7, 20);
            this.VerticalScanLabel.Name = "VerticalScanLabel";
            this.VerticalScanLabel.Size = new System.Drawing.Size(71, 13);
            this.VerticalScanLabel.TabIndex = 0;
            this.VerticalScanLabel.Text = "Vertical scan:";
            // 
            // SignalChartSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsGroupBox);
            this.Name = "SignalChartSettingsControl";
            this.Size = new System.Drawing.Size(198, 78);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.ComboBox HorizontalScanComboBox;
        private System.Windows.Forms.Label HorizontalScanLabel;
        private System.Windows.Forms.ComboBox VerticalScanComboBox;
        private System.Windows.Forms.Label VerticalScanLabel;
    }
}
