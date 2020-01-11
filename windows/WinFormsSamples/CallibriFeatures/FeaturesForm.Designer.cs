namespace CallibriFeatures
{
    partial class FeaturesForm
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
            this.AddDeviceButton = new System.Windows.Forms.Button();
            this.DeviceTabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // AddDeviceButton
            // 
            this.AddDeviceButton.Location = new System.Drawing.Point(12, 12);
            this.AddDeviceButton.Name = "AddDeviceButton";
            this.AddDeviceButton.Size = new System.Drawing.Size(83, 23);
            this.AddDeviceButton.TabIndex = 0;
            this.AddDeviceButton.Text = "Add device";
            this.AddDeviceButton.UseVisualStyleBackColor = true;
            this.AddDeviceButton.Click += new System.EventHandler(this.AddDeviceButton_Click);
            // 
            // DeviceTabControl
            // 
            this.DeviceTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceTabControl.Location = new System.Drawing.Point(0, 41);
            this.DeviceTabControl.Name = "DeviceTabControl";
            this.DeviceTabControl.SelectedIndex = 0;
            this.DeviceTabControl.Size = new System.Drawing.Size(894, 551);
            this.DeviceTabControl.TabIndex = 1;
            // 
            // FeaturesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 591);
            this.Controls.Add(this.DeviceTabControl);
            this.Controls.Add(this.AddDeviceButton);
            this.Name = "FeaturesForm";
            this.Text = "Callibri";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddDeviceButton;
        private System.Windows.Forms.TabControl DeviceTabControl;
    }
}

