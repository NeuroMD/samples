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
            this.ExternalSwitchComboBox = new System.Windows.Forms.ComboBox();
            this.ExternalSwitchLabel = new System.Windows.Forms.Label();
            this.ElectrodesAttachStateLabel = new System.Windows.Forms.Label();
            this.ElectrodesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ElectrodesGroupBox
            // 
            this.ElectrodesGroupBox.Controls.Add(this.ElectrodesAttachStateLabel);
            this.ElectrodesGroupBox.Controls.Add(this.ExternalSwitchComboBox);
            this.ElectrodesGroupBox.Controls.Add(this.ExternalSwitchLabel);
            this.ElectrodesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElectrodesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ElectrodesGroupBox.Name = "ElectrodesGroupBox";
            this.ElectrodesGroupBox.Size = new System.Drawing.Size(259, 86);
            this.ElectrodesGroupBox.TabIndex = 0;
            this.ElectrodesGroupBox.TabStop = false;
            this.ElectrodesGroupBox.Text = "Electrodes settings";
            // 
            // ExternalSwitchComboBox
            // 
            this.ExternalSwitchComboBox.FormattingEnabled = true;
            this.ExternalSwitchComboBox.Location = new System.Drawing.Point(126, 23);
            this.ExternalSwitchComboBox.Name = "ExternalSwitchComboBox";
            this.ExternalSwitchComboBox.Size = new System.Drawing.Size(121, 21);
            this.ExternalSwitchComboBox.TabIndex = 1;
            // 
            // ExternalSwitchLabel
            // 
            this.ExternalSwitchLabel.AutoSize = true;
            this.ExternalSwitchLabel.Location = new System.Drawing.Point(6, 26);
            this.ExternalSwitchLabel.Name = "ExternalSwitchLabel";
            this.ExternalSwitchLabel.Size = new System.Drawing.Size(114, 13);
            this.ExternalSwitchLabel.TabIndex = 0;
            this.ExternalSwitchLabel.Text = "Electrode switch state:";
            // 
            // ElectrodesAttachStateLabel
            // 
            this.ElectrodesAttachStateLabel.AutoSize = true;
            this.ElectrodesAttachStateLabel.Location = new System.Drawing.Point(6, 59);
            this.ElectrodesAttachStateLabel.Name = "ElectrodesAttachStateLabel";
            this.ElectrodesAttachStateLabel.Size = new System.Drawing.Size(136, 13);
            this.ElectrodesAttachStateLabel.TabIndex = 2;
            this.ElectrodesAttachStateLabel.Text = "Electrodes state: Detached";
            // 
            // ElectrodesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ElectrodesGroupBox);
            this.Name = "ElectrodesControl";
            this.Size = new System.Drawing.Size(259, 86);
            this.ElectrodesGroupBox.ResumeLayout(false);
            this.ElectrodesGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ElectrodesGroupBox;
        private System.Windows.Forms.Label ExternalSwitchLabel;
        private System.Windows.Forms.ComboBox ExternalSwitchComboBox;
        private System.Windows.Forms.Label ElectrodesAttachStateLabel;
    }
}
