namespace CallibriFeatures.Devices
{
    partial class DeviceInfoControl
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
            this.InfoGroupBox = new System.Windows.Forms.GroupBox();
            this.ConnectDisconnectButton = new System.Windows.Forms.Button();
            this.FindMeButton = new System.Windows.Forms.Button();
            this.ConncetionStateLabel = new System.Windows.Forms.Label();
            this.SerialNumberLabel = new System.Windows.Forms.Label();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.BatteryLabel = new System.Windows.Forms.Label();
            this.InfoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InfoGroupBox
            // 
            this.InfoGroupBox.Controls.Add(this.BatteryLabel);
            this.InfoGroupBox.Controls.Add(this.ConnectDisconnectButton);
            this.InfoGroupBox.Controls.Add(this.FindMeButton);
            this.InfoGroupBox.Controls.Add(this.ConncetionStateLabel);
            this.InfoGroupBox.Controls.Add(this.SerialNumberLabel);
            this.InfoGroupBox.Controls.Add(this.AddressLabel);
            this.InfoGroupBox.Controls.Add(this.NameLabel);
            this.InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoGroupBox.Location = new System.Drawing.Point(0, 0);
            this.InfoGroupBox.Name = "InfoGroupBox";
            this.InfoGroupBox.Size = new System.Drawing.Size(254, 131);
            this.InfoGroupBox.TabIndex = 0;
            this.InfoGroupBox.TabStop = false;
            this.InfoGroupBox.Text = "Info";
            // 
            // ConnectDisconnectButton
            // 
            this.ConnectDisconnectButton.Location = new System.Drawing.Point(171, 58);
            this.ConnectDisconnectButton.Name = "ConnectDisconnectButton";
            this.ConnectDisconnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectDisconnectButton.TabIndex = 11;
            this.ConnectDisconnectButton.Text = "Connect";
            this.ConnectDisconnectButton.UseVisualStyleBackColor = true;
            // 
            // FindMeButton
            // 
            this.FindMeButton.Location = new System.Drawing.Point(171, 23);
            this.FindMeButton.Name = "FindMeButton";
            this.FindMeButton.Size = new System.Drawing.Size(75, 23);
            this.FindMeButton.TabIndex = 10;
            this.FindMeButton.Text = "FindMe";
            this.FindMeButton.UseVisualStyleBackColor = true;
            // 
            // ConncetionStateLabel
            // 
            this.ConncetionStateLabel.AutoSize = true;
            this.ConncetionStateLabel.Location = new System.Drawing.Point(6, 63);
            this.ConncetionStateLabel.Name = "ConncetionStateLabel";
            this.ConncetionStateLabel.Size = new System.Drawing.Size(159, 13);
            this.ConncetionStateLabel.TabIndex = 9;
            this.ConncetionStateLabel.Text = "Connection state: Disconnected";
            // 
            // SerialNumberLabel
            // 
            this.SerialNumberLabel.AutoSize = true;
            this.SerialNumberLabel.Location = new System.Drawing.Point(6, 85);
            this.SerialNumberLabel.Name = "SerialNumberLabel";
            this.SerialNumberLabel.Size = new System.Drawing.Size(69, 13);
            this.SerialNumberLabel.TabIndex = 8;
            this.SerialNumberLabel.Text = "S/N: 100455";
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(6, 41);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(144, 13);
            this.AddressLabel.TabIndex = 7;
            this.AddressLabel.Text = "Address: [00:11:22:33:44:55]";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(6, 19);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(96, 13);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name: Callibri Gray";
            // 
            // BatteryLabel
            // 
            this.BatteryLabel.AutoSize = true;
            this.BatteryLabel.Location = new System.Drawing.Point(6, 107);
            this.BatteryLabel.Name = "BatteryLabel";
            this.BatteryLabel.Size = new System.Drawing.Size(72, 13);
            this.BatteryLabel.TabIndex = 12;
            this.BatteryLabel.Text = "Battery: 101%";
            // 
            // DeviceInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InfoGroupBox);
            this.Name = "DeviceInfoControl";
            this.Size = new System.Drawing.Size(254, 131);
            this.InfoGroupBox.ResumeLayout(false);
            this.InfoGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox InfoGroupBox;
        private System.Windows.Forms.Button FindMeButton;
        private System.Windows.Forms.Label ConncetionStateLabel;
        private System.Windows.Forms.Label SerialNumberLabel;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ConnectDisconnectButton;
        private System.Windows.Forms.Label BatteryLabel;
    }
}
