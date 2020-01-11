namespace CallibriFeatures.Devices
{
    partial class SignalControl
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
            this.SignalSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.HardwareFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.StopSignalButton = new System.Windows.Forms.Button();
            this.StartSignalButton = new System.Windows.Forms.Button();
            this.OffsetComboBox = new System.Windows.Forms.ComboBox();
            this.OffsetLabel = new System.Windows.Forms.Label();
            this.GainComboBox = new System.Windows.Forms.ComboBox();
            this.GainLabel = new System.Windows.Forms.Label();
            this.SamplingFrequencyComboBox = new System.Windows.Forms.ComboBox();
            this.SamplingFrequencyLabel = new System.Windows.Forms.Label();
            this.StopRespirationButton = new System.Windows.Forms.Button();
            this.StartRespirationButton = new System.Windows.Forms.Button();
            this.SignalSettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SignalSettingsGroupBox
            // 
            this.SignalSettingsGroupBox.Controls.Add(this.StopRespirationButton);
            this.SignalSettingsGroupBox.Controls.Add(this.StartRespirationButton);
            this.SignalSettingsGroupBox.Controls.Add(this.HardwareFilterCheckBox);
            this.SignalSettingsGroupBox.Controls.Add(this.StopSignalButton);
            this.SignalSettingsGroupBox.Controls.Add(this.StartSignalButton);
            this.SignalSettingsGroupBox.Controls.Add(this.OffsetComboBox);
            this.SignalSettingsGroupBox.Controls.Add(this.OffsetLabel);
            this.SignalSettingsGroupBox.Controls.Add(this.GainComboBox);
            this.SignalSettingsGroupBox.Controls.Add(this.GainLabel);
            this.SignalSettingsGroupBox.Controls.Add(this.SamplingFrequencyComboBox);
            this.SignalSettingsGroupBox.Controls.Add(this.SamplingFrequencyLabel);
            this.SignalSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignalSettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.SignalSettingsGroupBox.Name = "SignalSettingsGroupBox";
            this.SignalSettingsGroupBox.Size = new System.Drawing.Size(216, 181);
            this.SignalSettingsGroupBox.TabIndex = 0;
            this.SignalSettingsGroupBox.TabStop = false;
            this.SignalSettingsGroupBox.Text = "Signal";
            // 
            // HardwareFilterCheckBox
            // 
            this.HardwareFilterCheckBox.AutoSize = true;
            this.HardwareFilterCheckBox.Location = new System.Drawing.Point(10, 102);
            this.HardwareFilterCheckBox.Name = "HardwareFilterCheckBox";
            this.HardwareFilterCheckBox.Size = new System.Drawing.Size(128, 17);
            this.HardwareFilterCheckBox.TabIndex = 9;
            this.HardwareFilterCheckBox.Text = "Enable hardware filter";
            this.HardwareFilterCheckBox.UseVisualStyleBackColor = true;
            // 
            // StopSignalButton
            // 
            this.StopSignalButton.Location = new System.Drawing.Point(107, 125);
            this.StopSignalButton.Name = "StopSignalButton";
            this.StopSignalButton.Size = new System.Drawing.Size(89, 23);
            this.StopSignalButton.TabIndex = 8;
            this.StopSignalButton.Text = "Stop signal";
            this.StopSignalButton.UseVisualStyleBackColor = true;
            // 
            // StartSignalButton
            // 
            this.StartSignalButton.Location = new System.Drawing.Point(8, 125);
            this.StartSignalButton.Name = "StartSignalButton";
            this.StartSignalButton.Size = new System.Drawing.Size(93, 23);
            this.StartSignalButton.TabIndex = 7;
            this.StartSignalButton.Text = "Start signal";
            this.StartSignalButton.UseVisualStyleBackColor = true;
            // 
            // OffsetComboBox
            // 
            this.OffsetComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OffsetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OffsetComboBox.FormattingEnabled = true;
            this.OffsetComboBox.Location = new System.Drawing.Point(116, 71);
            this.OffsetComboBox.Name = "OffsetComboBox";
            this.OffsetComboBox.Size = new System.Drawing.Size(94, 21);
            this.OffsetComboBox.TabIndex = 6;
            // 
            // OffsetLabel
            // 
            this.OffsetLabel.AutoSize = true;
            this.OffsetLabel.Location = new System.Drawing.Point(7, 74);
            this.OffsetLabel.Name = "OffsetLabel";
            this.OffsetLabel.Size = new System.Drawing.Size(38, 13);
            this.OffsetLabel.TabIndex = 5;
            this.OffsetLabel.Text = "Offset:";
            // 
            // GainComboBox
            // 
            this.GainComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GainComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GainComboBox.FormattingEnabled = true;
            this.GainComboBox.Location = new System.Drawing.Point(116, 44);
            this.GainComboBox.Name = "GainComboBox";
            this.GainComboBox.Size = new System.Drawing.Size(94, 21);
            this.GainComboBox.TabIndex = 4;
            // 
            // GainLabel
            // 
            this.GainLabel.AutoSize = true;
            this.GainLabel.Location = new System.Drawing.Point(7, 47);
            this.GainLabel.Name = "GainLabel";
            this.GainLabel.Size = new System.Drawing.Size(32, 13);
            this.GainLabel.TabIndex = 3;
            this.GainLabel.Text = "Gain:";
            // 
            // SamplingFrequencyComboBox
            // 
            this.SamplingFrequencyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SamplingFrequencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SamplingFrequencyComboBox.FormattingEnabled = true;
            this.SamplingFrequencyComboBox.Location = new System.Drawing.Point(116, 17);
            this.SamplingFrequencyComboBox.Name = "SamplingFrequencyComboBox";
            this.SamplingFrequencyComboBox.Size = new System.Drawing.Size(94, 21);
            this.SamplingFrequencyComboBox.TabIndex = 1;
            // 
            // SamplingFrequencyLabel
            // 
            this.SamplingFrequencyLabel.AutoSize = true;
            this.SamplingFrequencyLabel.Location = new System.Drawing.Point(7, 20);
            this.SamplingFrequencyLabel.Name = "SamplingFrequencyLabel";
            this.SamplingFrequencyLabel.Size = new System.Drawing.Size(103, 13);
            this.SamplingFrequencyLabel.TabIndex = 0;
            this.SamplingFrequencyLabel.Text = "Sampling frequency:";
            // 
            // StopRespirationButton
            // 
            this.StopRespirationButton.Location = new System.Drawing.Point(107, 152);
            this.StopRespirationButton.Name = "StopRespirationButton";
            this.StopRespirationButton.Size = new System.Drawing.Size(89, 23);
            this.StopRespirationButton.TabIndex = 11;
            this.StopRespirationButton.Text = "Stop respiration";
            this.StopRespirationButton.UseVisualStyleBackColor = true;
            // 
            // StartRespirationButton
            // 
            this.StartRespirationButton.Location = new System.Drawing.Point(8, 152);
            this.StartRespirationButton.Name = "StartRespirationButton";
            this.StartRespirationButton.Size = new System.Drawing.Size(93, 23);
            this.StartRespirationButton.TabIndex = 10;
            this.StartRespirationButton.Text = "Start respiration";
            this.StartRespirationButton.UseVisualStyleBackColor = true;
            // 
            // SignalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SignalSettingsGroupBox);
            this.Name = "SignalControl";
            this.Size = new System.Drawing.Size(216, 181);
            this.SignalSettingsGroupBox.ResumeLayout(false);
            this.SignalSettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SignalSettingsGroupBox;
        private System.Windows.Forms.ComboBox SamplingFrequencyComboBox;
        private System.Windows.Forms.Label SamplingFrequencyLabel;
        private System.Windows.Forms.ComboBox OffsetComboBox;
        private System.Windows.Forms.Label OffsetLabel;
        private System.Windows.Forms.ComboBox GainComboBox;
        private System.Windows.Forms.Label GainLabel;
        private System.Windows.Forms.Button StopSignalButton;
        private System.Windows.Forms.Button StartSignalButton;
        private System.Windows.Forms.CheckBox HardwareFilterCheckBox;
        private System.Windows.Forms.Button StopRespirationButton;
        private System.Windows.Forms.Button StartRespirationButton;
    }
}
