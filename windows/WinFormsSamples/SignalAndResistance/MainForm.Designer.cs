using System.ComponentModel;
using System.Windows.Forms;
using SignalView;

namespace SignalAndResistance
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._signalChart = new SignalView.SignalChart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._deviceLabel = new System.Windows.Forms.ToolStripLabel();
            this._reconnectButton = new System.Windows.Forms.ToolStripButton();
            this._startSignalButton = new System.Windows.Forms.ToolStripButton();
            this._stopButton = new System.Windows.Forms.ToolStripButton();
            this._channelComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this._durationLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _signalChart
            // 
            this._signalChart.BackColor = System.Drawing.SystemColors.Control;
            this._signalChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._signalChart.Location = new System.Drawing.Point(0, 25);
            this._signalChart.MinimumSize = new System.Drawing.Size(500, 440);
            this._signalChart.Name = "_signalChart";
            this._signalChart.PeakDetector = false;
            this._signalChart.ScaleX = 18;
            this._signalChart.ScaleY = 10;
            this._signalChart.Size = new System.Drawing.Size(820, 481);
            this._signalChart.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._deviceLabel,
            this._reconnectButton,
            this._startSignalButton,
            this._stopButton,
            this._channelComboBox,
            this.toolStripLabel2,
            this._durationLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(820, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Device:";
            // 
            // _deviceLabel
            // 
            this._deviceLabel.Name = "_deviceLabel";
            this._deviceLabel.Size = new System.Drawing.Size(112, 22);
            this._deviceLabel.Text = "Waiting for device...";
            // 
            // _reconnectButton
            // 
            this._reconnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._reconnectButton.Image = ((System.Drawing.Image)(resources.GetObject("_reconnectButton.Image")));
            this._reconnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._reconnectButton.Name = "_reconnectButton";
            this._reconnectButton.Size = new System.Drawing.Size(67, 22);
            this._reconnectButton.Text = "Reconnect";
            this._reconnectButton.Click += new System.EventHandler(this._reconnectButton_Click);
            // 
            // _startSignalButton
            // 
            this._startSignalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._startSignalButton.Enabled = false;
            this._startSignalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._startSignalButton.Name = "_startSignalButton";
            this._startSignalButton.Size = new System.Drawing.Size(69, 22);
            this._startSignalButton.Text = "Start signal";
            this._startSignalButton.Click += new System.EventHandler(this._signalStartButton_Click);
            // 
            // _stopButton
            // 
            this._stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._stopButton.Enabled = false;
            this._stopButton.Image = ((System.Drawing.Image)(resources.GetObject("_stopButton.Image")));
            this._stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(35, 22);
            this._stopButton.Text = "Stop";
            this._stopButton.Click += new System.EventHandler(this._stopButton_Click);
            // 
            // _channelComboBox
            // 
            this._channelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._channelComboBox.Enabled = false;
            this._channelComboBox.Name = "_channelComboBox";
            this._channelComboBox.Size = new System.Drawing.Size(121, 25);
            this._channelComboBox.SelectedIndexChanged += new System.EventHandler(this._channelComboBox_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel2.Text = "Duration:";
            // 
            // _durationLabel
            // 
            this._durationLabel.Name = "_durationLabel";
            this._durationLabel.Size = new System.Drawing.Size(13, 22);
            this._durationLabel.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 506);
            this.Controls.Add(this._signalChart);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "SignalAndResistance";
            this.Closing += new CancelEventHandler(this.MainForm_Closing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SignalView.SignalChart _signalChart;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel _deviceLabel;
        private System.Windows.Forms.ToolStripButton _reconnectButton;
        private System.Windows.Forms.ToolStripButton _startSignalButton;
        private System.Windows.Forms.ToolStripButton _stopButton;
        private System.Windows.Forms.ToolStripComboBox _channelComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel _durationLabel;
    }
}

