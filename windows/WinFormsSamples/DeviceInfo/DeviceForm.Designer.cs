namespace DeviceInfo
{
    partial class DeviceForm
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
            this._deviceListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this._connectButton = new System.Windows.Forms.Button();
            this._disconnectButton = new System.Windows.Forms.Button();
            this._startScanButton = new System.Windows.Forms.Button();
            this._stopScanButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this._deviceInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _deviceListBox
            // 
            this._deviceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._deviceListBox.FormattingEnabled = true;
            this._deviceListBox.Location = new System.Drawing.Point(9, 63);
            this._deviceListBox.Name = "_deviceListBox";
            this._deviceListBox.Size = new System.Drawing.Size(234, 511);
            this._deviceListBox.TabIndex = 0;
            this._deviceListBox.SelectedIndexChanged += new System.EventHandler(this._deviceListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Devices";
            // 
            // _connectButton
            // 
            this._connectButton.Enabled = false;
            this._connectButton.Location = new System.Drawing.Point(249, 63);
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new System.Drawing.Size(75, 23);
            this._connectButton.TabIndex = 2;
            this._connectButton.Text = "Connect";
            this._connectButton.UseVisualStyleBackColor = true;
            this._connectButton.Click += new System.EventHandler(this._connectButton_Click);
            // 
            // _disconnectButton
            // 
            this._disconnectButton.Enabled = false;
            this._disconnectButton.Location = new System.Drawing.Point(330, 63);
            this._disconnectButton.Name = "_disconnectButton";
            this._disconnectButton.Size = new System.Drawing.Size(75, 23);
            this._disconnectButton.TabIndex = 3;
            this._disconnectButton.Text = "Disconnect";
            this._disconnectButton.UseVisualStyleBackColor = true;
            this._disconnectButton.Click += new System.EventHandler(this._disconnectButton_Click);
            // 
            // _startScanButton
            // 
            this._startScanButton.Enabled = false;
            this._startScanButton.Location = new System.Drawing.Point(9, 12);
            this._startScanButton.Name = "_startScanButton";
            this._startScanButton.Size = new System.Drawing.Size(75, 23);
            this._startScanButton.TabIndex = 4;
            this._startScanButton.Text = "Start scan";
            this._startScanButton.UseVisualStyleBackColor = true;
            this._startScanButton.Click += new System.EventHandler(this._startScanButton_Click);
            // 
            // _stopScanButton
            // 
            this._stopScanButton.Enabled = false;
            this._stopScanButton.Location = new System.Drawing.Point(91, 12);
            this._stopScanButton.Name = "_stopScanButton";
            this._stopScanButton.Size = new System.Drawing.Size(75, 23);
            this._stopScanButton.TabIndex = 5;
            this._stopScanButton.Text = "Stop scan";
            this._stopScanButton.UseVisualStyleBackColor = true;
            this._stopScanButton.Click += new System.EventHandler(this._stopScanButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(155, 13);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // _deviceInfoLabel
            // 
            this._deviceInfoLabel.AutoSize = true;
            this._deviceInfoLabel.Location = new System.Drawing.Point(255, 93);
            this._deviceInfoLabel.Name = "_deviceInfoLabel";
            this._deviceInfoLabel.Size = new System.Drawing.Size(0, 13);
            this._deviceInfoLabel.TabIndex = 7;
            // 
            // DeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 592);
            this.Controls.Add(this._deviceInfoLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this._stopScanButton);
            this.Controls.Add(this._startScanButton);
            this.Controls.Add(this._disconnectButton);
            this.Controls.Add(this._connectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._deviceListBox);
            this.Name = "DeviceForm";
            this.Text = "Device information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _deviceListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _connectButton;
        private System.Windows.Forms.Button _disconnectButton;
        private System.Windows.Forms.Button _startScanButton;
        private System.Windows.Forms.Button _stopScanButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label _deviceInfoLabel;
    }
}

