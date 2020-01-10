namespace CallibriFeatures.Devices
{
    partial class DeviceTab
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
            this.DeviceInfoControl = new CallibriFeatures.Devices.DeviceInfoControl();
            this.electrodesControl1 = new CallibriFeatures.Devices.ElectrodesControl();
            this.SuspendLayout();
            // 
            // DeviceInfoControl
            // 
            this.DeviceInfoControl.Location = new System.Drawing.Point(4, 4);
            this.DeviceInfoControl.Name = "DeviceInfoControl";
            this.DeviceInfoControl.Size = new System.Drawing.Size(259, 154);
            this.DeviceInfoControl.TabIndex = 0;
            // 
            // electrodesControl1
            // 
            this.electrodesControl1.Location = new System.Drawing.Point(4, 164);
            this.electrodesControl1.Name = "electrodesControl1";
            this.electrodesControl1.Size = new System.Drawing.Size(259, 86);
            this.electrodesControl1.TabIndex = 1;
            // 
            // DeviceTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.electrodesControl1);
            this.Controls.Add(this.DeviceInfoControl);
            this.Name = "DeviceTab";
            this.Size = new System.Drawing.Size(925, 405);
            this.ResumeLayout(false);

        }

        #endregion

        private DeviceInfoControl DeviceInfoControl;
        private ElectrodesControl electrodesControl1;
    }
}
