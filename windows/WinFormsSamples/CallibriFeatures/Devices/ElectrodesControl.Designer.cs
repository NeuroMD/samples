namespace CallibriFeatures.Devices
{
    partial class ElectrodesControl
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
            this.ElectrodesGroupBox = new System.Windows.Forms.GroupBox();
            this.ElectrodeStateHintLabel = new System.Windows.Forms.Label();
            this.ADCComboBox = new System.Windows.Forms.ComboBox();
            this.AdcInputLabel = new System.Windows.Forms.Label();
            this.ElectrodesAttachStateLabel = new System.Windows.Forms.Label();
            this.ExternalSwitchComboBox = new System.Windows.Forms.ComboBox();
            this.ExternalSwitchLabel = new System.Windows.Forms.Label();
            this.ElectrodesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ElectrodesGroupBox
            // 
            this.ElectrodesGroupBox.Controls.Add(this.ElectrodeStateHintLabel);
            this.ElectrodesGroupBox.Controls.Add(this.ADCComboBox);
            this.ElectrodesGroupBox.Controls.Add(this.AdcInputLabel);
            this.ElectrodesGroupBox.Controls.Add(this.ElectrodesAttachStateLabel);
            this.ElectrodesGroupBox.Controls.Add(this.ExternalSwitchComboBox);
            this.ElectrodesGroupBox.Controls.Add(this.ExternalSwitchLabel);
            this.ElectrodesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElectrodesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ElectrodesGroupBox.Name = "ElectrodesGroupBox";
            this.ElectrodesGroupBox.Size = new System.Drawing.Size(259, 112);
            this.ElectrodesGroupBox.TabIndex = 0;
            this.ElectrodesGroupBox.TabStop = false;
            this.ElectrodesGroupBox.Text = "Electrodes settings";
            // 
            // ElectrodeStateHintLabel
            // 
            this.ElectrodeStateHintLabel.AutoSize = true;
            this.ElectrodeStateHintLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ElectrodeStateHintLabel.ForeColor = System.Drawing.Color.Red;
            this.ElectrodeStateHintLabel.Location = new System.Drawing.Point(135, 85);
            this.ElectrodeStateHintLabel.Name = "ElectrodeStateHintLabel";
            this.ElectrodeStateHintLabel.Size = new System.Drawing.Size(13, 19);
            this.ElectrodeStateHintLabel.TabIndex = 5;
            this.ElectrodeStateHintLabel.Text = "!";
            // 
            // ADCComboBox
            // 
            this.ADCComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ADCComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ADCComboBox.FormattingEnabled = true;
            this.ADCComboBox.Location = new System.Drawing.Point(123, 54);
            this.ADCComboBox.Name = "ADCComboBox";
            this.ADCComboBox.Size = new System.Drawing.Size(130, 21);
            this.ADCComboBox.TabIndex = 4;
            // 
            // AdcInputLabel
            // 
            this.AdcInputLabel.AutoSize = true;
            this.AdcInputLabel.Location = new System.Drawing.Point(3, 57);
            this.AdcInputLabel.Name = "AdcInputLabel";
            this.AdcInputLabel.Size = new System.Drawing.Size(84, 13);
            this.AdcInputLabel.TabIndex = 3;
            this.AdcInputLabel.Text = "ADC input state:";
            // 
            // ElectrodesAttachStateLabel
            // 
            this.ElectrodesAttachStateLabel.AutoSize = true;
            this.ElectrodesAttachStateLabel.Location = new System.Drawing.Point(3, 88);
            this.ElectrodesAttachStateLabel.Name = "ElectrodesAttachStateLabel";
            this.ElectrodesAttachStateLabel.Size = new System.Drawing.Size(136, 13);
            this.ElectrodesAttachStateLabel.TabIndex = 2;
            this.ElectrodesAttachStateLabel.Text = "Electrodes state: Detached";
            // 
            // ExternalSwitchComboBox
            // 
            this.ExternalSwitchComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExternalSwitchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExternalSwitchComboBox.FormattingEnabled = true;
            this.ExternalSwitchComboBox.Location = new System.Drawing.Point(123, 19);
            this.ExternalSwitchComboBox.Name = "ExternalSwitchComboBox";
            this.ExternalSwitchComboBox.Size = new System.Drawing.Size(130, 21);
            this.ExternalSwitchComboBox.TabIndex = 1;
            // 
            // ExternalSwitchLabel
            // 
            this.ExternalSwitchLabel.AutoSize = true;
            this.ExternalSwitchLabel.Location = new System.Drawing.Point(3, 22);
            this.ExternalSwitchLabel.Name = "ExternalSwitchLabel";
            this.ExternalSwitchLabel.Size = new System.Drawing.Size(114, 13);
            this.ExternalSwitchLabel.TabIndex = 0;
            this.ExternalSwitchLabel.Text = "Electrode switch state:";
            // 
            // ElectrodesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ElectrodesGroupBox);
            this.Name = "ElectrodesControl";
            this.Size = new System.Drawing.Size(259, 112);
            this.ElectrodesGroupBox.ResumeLayout(false);
            this.ElectrodesGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ElectrodesGroupBox;
        private System.Windows.Forms.Label ExternalSwitchLabel;
        private System.Windows.Forms.ComboBox ExternalSwitchComboBox;
        private System.Windows.Forms.Label ElectrodesAttachStateLabel;
        private System.Windows.Forms.ComboBox ADCComboBox;
        private System.Windows.Forms.Label AdcInputLabel;
        private System.Windows.Forms.Label ElectrodeStateHintLabel;
    }
}
