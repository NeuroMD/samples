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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this._deviceInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _deviceListBox
            // 
            this._deviceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._deviceListBox.FormattingEnabled = true;
            this._deviceListBox.Location = new System.Drawing.Point(9, 37);
            this._deviceListBox.Name = "_deviceListBox";
            this._deviceListBox.Size = new System.Drawing.Size(234, 537);
            this._deviceListBox.TabIndex = 0;
            this._deviceListBox.SelectedIndexChanged += new System.EventHandler(this._deviceListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Devices";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 25);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(234, 13);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6;
            // 
            // _deviceInfoLabel
            // 
            this._deviceInfoLabel.AutoSize = true;
            this._deviceInfoLabel.Location = new System.Drawing.Point(255, 42);
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
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label _deviceInfoLabel;
    }
}

