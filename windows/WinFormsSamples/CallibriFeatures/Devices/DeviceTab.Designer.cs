using CallibriFeatures.GraphicsControl;

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
            CallibriFeatures.GraphicsControl.EmptyDrawable emptyDrawable1 = new CallibriFeatures.GraphicsControl.EmptyDrawable();
            this.MemsControlPanel = new CallibriFeatures.Devices.MEMSControl();
            this.SignalControlPanel = new CallibriFeatures.Devices.SignalControl();
            this.ElectrodesControlPanel = new CallibriFeatures.Devices.ElectrodesControl();
            this.DeviceInfoControl = new CallibriFeatures.Devices.DeviceInfoControl();
            this.SignalPlotDrawableControl = new CallibriFeatures.GraphicsControl.DrawableControl();
            this.SignalChartSettingsControl = new CallibriFeatures.Devices.SignalChartSettingsControl();
            this.SuspendLayout();
            // 
            // MemsControlPanel
            // 
            this.MemsControlPanel.Location = new System.Drawing.Point(4, 469);
            this.MemsControlPanel.Name = "MemsControlPanel";
            this.MemsControlPanel.Size = new System.Drawing.Size(282, 201);
            this.MemsControlPanel.TabIndex = 3;
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
            // SignalPlotDrawableControl
            // 
            this.SignalPlotDrawableControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignalPlotDrawableControl.Drawable = emptyDrawable1;
            this.SignalPlotDrawableControl.Location = new System.Drawing.Point(292, 4);
            this.SignalPlotDrawableControl.Name = "SignalPlotDrawableControl";
            this.SignalPlotDrawableControl.Size = new System.Drawing.Size(426, 168);
            this.SignalPlotDrawableControl.TabIndex = 4;
            this.SignalPlotDrawableControl.Text = "drawableControl1";
            // 
            // SignalChartSettingsControl
            // 
            this.SignalChartSettingsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SignalChartSettingsControl.Location = new System.Drawing.Point(724, 4);
            this.SignalChartSettingsControl.Name = "SignalChartSettingsControl";
            this.SignalChartSettingsControl.Size = new System.Drawing.Size(198, 168);
            this.SignalChartSettingsControl.TabIndex = 5;
            // 
            // DeviceTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SignalChartSettingsControl);
            this.Controls.Add(this.MemsControlPanel);
            this.Controls.Add(this.SignalControlPanel);
            this.Controls.Add(this.ElectrodesControlPanel);
            this.Controls.Add(this.DeviceInfoControl);
            this.Controls.Add(this.SignalPlotDrawableControl);
            this.MinimumSize = new System.Drawing.Size(0, 675);
            this.Name = "DeviceTab";
            this.Size = new System.Drawing.Size(925, 675);
            this.ResumeLayout(false);

        }

        #endregion
        
        private GraphicsControl.DrawableControl SignalPlotDrawableControl;
        private DeviceInfoControl DeviceInfoControl;
        private ElectrodesControl ElectrodesControlPanel;
        private SignalControl SignalControlPanel;
        private MEMSControl MemsControlPanel;
        private SignalChartSettingsControl SignalChartSettingsControl;
    }
}
