namespace EmotionalStates.Devices
{
    partial class DeviceSearchForm
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this._deviceListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 136);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(222, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // _deviceListBox
            // 
            this._deviceListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._deviceListBox.FormattingEnabled = true;
            this._deviceListBox.Location = new System.Drawing.Point(0, 0);
            this._deviceListBox.Name = "_deviceListBox";
            this._deviceListBox.Size = new System.Drawing.Size(222, 136);
            this._deviceListBox.TabIndex = 1;
            this._deviceListBox.SelectedIndexChanged += new System.EventHandler(this._deviceListBox_SelectedIndexChanged);
            this._deviceListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._deviceListBox_MouseDoubleClick);
            // 
            // DeviceSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 159);
            this.Controls.Add(this._deviceListBox);
            this.Controls.Add(this.progressBar1);
            this.Name = "DeviceSearchForm";
            this.Text = "Devices";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceSearchForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListBox _deviceListBox;
    }
}