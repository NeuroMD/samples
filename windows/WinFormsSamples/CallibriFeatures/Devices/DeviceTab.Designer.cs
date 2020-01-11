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
            this.SignalControlPanel = new CallibriFeatures.Devices.SignalControl();
            this.ElectrodesControlPanel = new CallibriFeatures.Devices.ElectrodesControl();
            this.DeviceInfoControl = new CallibriFeatures.Devices.DeviceInfoControl();
            this.MemsControlPanel = new CallibriFeatures.Devices.MEMSControl();
            this.SuspendLayout();
            // 
            // SignalControlPanel
            // 
            this.SignalControlPanel.Location = new System.Drawing.Point(4, 282);
            this.SignalControlPanel.Name = "SignalControlPanel";
            this.SignalControlPanel.Size = new System.Drawing.Size(282, 181);
            this.SignalControlPanel.TabIndex = 2;
            // 
            // ElectrodesControlPanel
            // 
            this.ElectrodesControlPanel.Location = new System.Drawing.Point(4, 163);
            this.ElectrodesControlPanel.Name = "ElectrodesControlPanel";
            this.ElectrodesControlPanel.Size = new System.Drawing.Size(282, 113);
            this.ElectrodesControlPanel.TabIndex = 1;
            // 
            // DeviceInfoControl
            // 
            this.DeviceInfoControl.Location = new System.Drawing.Point(4, 4);
            this.DeviceInfoControl.Name = "DeviceInfoControl";
            this.DeviceInfoControl.Size = new System.Drawing.Size(282, 153);
            this.DeviceInfoControl.TabIndex = 0;
            // 
            // MemsControlPanel
            // 
            this.MemsControlPanel.Location = new System.Drawing.Point(4, 469);
            this.MemsControlPanel.Name = "MemsControlPanel";
            this.MemsControlPanel.Size = new System.Drawing.Size(282, 201);
            this.MemsControlPanel.TabIndex = 3;
            // 
            // DeviceTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MemsControlPanel);
            this.Controls.Add(this.SignalControlPanel);
            this.Controls.Add(this.ElectrodesControlPanel);
            this.Controls.Add(this.DeviceInfoControl);
            this.MinimumSize = new System.Drawing.Size(0, 675);
            this.Name = "DeviceTab";
            this.Size = new System.Drawing.Size(925, 675);
            this.ResumeLayout(false);

        }

        #endregion

        private DeviceInfoControl DeviceInfoControl;
        private ElectrodesControl ElectrodesControlPanel;
        private SignalControl SignalControlPanel;
        private MEMSControl MemsControlPanel;
    }
}
