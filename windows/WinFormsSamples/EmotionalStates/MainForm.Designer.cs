using System.Windows.Forms;
using EmotionalStates.Drawable;

namespace EmotionalStates
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
            EmotionalStates.Drawable.EmptyDrawable emptyDrawable1 = new EmotionalStates.Drawable.EmptyDrawable();
            this.label1 = new System.Windows.Forms.Label();
            this._deviceLabel = new System.Windows.Forms.Label();
            this._findDeviceButton = new System.Windows.Forms.Button();
            this._startSignalButton = new System.Windows.Forms.Button();
            this._stopSignalButton = new System.Windows.Forms.Button();
            this._emotionsStartButton = new System.Windows.Forms.Button();
            this._broadcastCheckBox = new System.Windows.Forms.CheckBox();
            this._applyNetSettingsButton = new System.Windows.Forms.Button();
            this._ipAddressTextBox = new System.Windows.Forms.TextBox();
            this._portTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._statesStopButton = new System.Windows.Forms.Button();
            this._spectrumAmplitudeTrackBar = new System.Windows.Forms.TrackBar();
            this.ResistanceCheckButton = new System.Windows.Forms.Button();
            this.BatteryLabel = new System.Windows.Forms.Label();
            this._drawableControl = new EmotionalStates.Drawable.DrawableControl();
            ((System.ComponentModel.ISupportInitialize)(this._spectrumAmplitudeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(580, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Device:";
            // 
            // _deviceLabel
            // 
            this._deviceLabel.AutoSize = true;
            this._deviceLabel.Location = new System.Drawing.Point(630, 13);
            this._deviceLabel.Name = "_deviceLabel";
            this._deviceLabel.Size = new System.Drawing.Size(67, 13);
            this._deviceLabel.TabIndex = 4;
            this._deviceLabel.Text = "Not selected";
            // 
            // _findDeviceButton
            // 
            this._findDeviceButton.Location = new System.Drawing.Point(12, 8);
            this._findDeviceButton.Name = "_findDeviceButton";
            this._findDeviceButton.Size = new System.Drawing.Size(75, 23);
            this._findDeviceButton.TabIndex = 5;
            this._findDeviceButton.Text = "Find device";
            this._findDeviceButton.UseVisualStyleBackColor = true;
            this._findDeviceButton.Click += new System.EventHandler(this._findDeviceButton_Click);
            // 
            // _startSignalButton
            // 
            this._startSignalButton.Enabled = false;
            this._startSignalButton.Location = new System.Drawing.Point(233, 8);
            this._startSignalButton.Name = "_startSignalButton";
            this._startSignalButton.Size = new System.Drawing.Size(75, 23);
            this._startSignalButton.TabIndex = 6;
            this._startSignalButton.Text = "Signal start";
            this._startSignalButton.UseVisualStyleBackColor = true;
            this._startSignalButton.Click += new System.EventHandler(this._startSignalButton_Click);
            // 
            // _stopSignalButton
            // 
            this._stopSignalButton.Enabled = false;
            this._stopSignalButton.Location = new System.Drawing.Point(314, 8);
            this._stopSignalButton.Name = "_stopSignalButton";
            this._stopSignalButton.Size = new System.Drawing.Size(75, 23);
            this._stopSignalButton.TabIndex = 7;
            this._stopSignalButton.Text = "Signal stop";
            this._stopSignalButton.UseVisualStyleBackColor = true;
            this._stopSignalButton.Click += new System.EventHandler(this._stopSignalButton_Click);
            // 
            // _emotionsStartButton
            // 
            this._emotionsStartButton.Enabled = false;
            this._emotionsStartButton.Location = new System.Drawing.Point(395, 8);
            this._emotionsStartButton.Name = "_emotionsStartButton";
            this._emotionsStartButton.Size = new System.Drawing.Size(98, 23);
            this._emotionsStartButton.TabIndex = 8;
            this._emotionsStartButton.Text = "Calculate states";
            this._emotionsStartButton.UseVisualStyleBackColor = true;
            this._emotionsStartButton.Click += new System.EventHandler(this._emotionsStartButton_Click);
            // 
            // _broadcastCheckBox
            // 
            this._broadcastCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._broadcastCheckBox.AutoSize = true;
            this._broadcastCheckBox.Checked = true;
            this._broadcastCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._broadcastCheckBox.Location = new System.Drawing.Point(1478, 12);
            this._broadcastCheckBox.Name = "_broadcastCheckBox";
            this._broadcastCheckBox.Size = new System.Drawing.Size(74, 17);
            this._broadcastCheckBox.TabIndex = 10;
            this._broadcastCheckBox.Text = "Broadcast";
            this._broadcastCheckBox.UseVisualStyleBackColor = true;
            this._broadcastCheckBox.CheckedChanged += new System.EventHandler(this._broadcastCheckBox_CheckedChanged);
            // 
            // _applyNetSettingsButton
            // 
            this._applyNetSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._applyNetSettingsButton.Enabled = false;
            this._applyNetSettingsButton.Location = new System.Drawing.Point(1558, 8);
            this._applyNetSettingsButton.Name = "_applyNetSettingsButton";
            this._applyNetSettingsButton.Size = new System.Drawing.Size(75, 23);
            this._applyNetSettingsButton.TabIndex = 11;
            this._applyNetSettingsButton.Text = "Apply";
            this._applyNetSettingsButton.UseVisualStyleBackColor = true;
            this._applyNetSettingsButton.Click += new System.EventHandler(this._applyNetSettingsButton_Click);
            // 
            // _ipAddressTextBox
            // 
            this._ipAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ipAddressTextBox.Enabled = false;
            this._ipAddressTextBox.Location = new System.Drawing.Point(1276, 10);
            this._ipAddressTextBox.Name = "_ipAddressTextBox";
            this._ipAddressTextBox.Size = new System.Drawing.Size(100, 20);
            this._ipAddressTextBox.TabIndex = 12;
            this._ipAddressTextBox.Text = "192.168.1.1";
            this._ipAddressTextBox.TextChanged += new System.EventHandler(this._ipAddressTextBox_TextChanged);
            // 
            // _portTextBox
            // 
            this._portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._portTextBox.Location = new System.Drawing.Point(1412, 10);
            this._portTextBox.Name = "_portTextBox";
            this._portTextBox.Size = new System.Drawing.Size(60, 20);
            this._portTextBox.TabIndex = 13;
            this._portTextBox.Text = "8000";
            this._portTextBox.TextChanged += new System.EventHandler(this._portTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1382, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1253, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "IP";
            // 
            // _statesStopButton
            // 
            this._statesStopButton.Enabled = false;
            this._statesStopButton.Location = new System.Drawing.Point(499, 8);
            this._statesStopButton.Name = "_statesStopButton";
            this._statesStopButton.Size = new System.Drawing.Size(75, 23);
            this._statesStopButton.TabIndex = 16;
            this._statesStopButton.Text = "States stop";
            this._statesStopButton.UseVisualStyleBackColor = true;
            this._statesStopButton.Click += new System.EventHandler(this._statesStopButton_Click);
            // 
            // _spectrumAmplitudeTrackBar
            // 
            this._spectrumAmplitudeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._spectrumAmplitudeTrackBar.Location = new System.Drawing.Point(1592, 37);
            this._spectrumAmplitudeTrackBar.Maximum = 500;
            this._spectrumAmplitudeTrackBar.Minimum = 10;
            this._spectrumAmplitudeTrackBar.Name = "_spectrumAmplitudeTrackBar";
            this._spectrumAmplitudeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._spectrumAmplitudeTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._spectrumAmplitudeTrackBar.RightToLeftLayout = true;
            this._spectrumAmplitudeTrackBar.Size = new System.Drawing.Size(45, 937);
            this._spectrumAmplitudeTrackBar.TabIndex = 18;
            this._spectrumAmplitudeTrackBar.Value = 100;
            // 
            // ResistanceCheckButton
            // 
            this.ResistanceCheckButton.Enabled = false;
            this.ResistanceCheckButton.Location = new System.Drawing.Point(93, 8);
            this.ResistanceCheckButton.Name = "ResistanceCheckButton";
            this.ResistanceCheckButton.Size = new System.Drawing.Size(134, 23);
            this.ResistanceCheckButton.TabIndex = 19;
            this.ResistanceCheckButton.Text = "Resistance check";
            this.ResistanceCheckButton.UseVisualStyleBackColor = true;
            this.ResistanceCheckButton.Click += new System.EventHandler(this.ResistanceCheckButton_Click);
            // 
            // BatteryLabel
            // 
            this.BatteryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BatteryLabel.AutoSize = true;
            this.BatteryLabel.Location = new System.Drawing.Point(1164, 13);
            this.BatteryLabel.Name = "BatteryLabel";
            this.BatteryLabel.Size = new System.Drawing.Size(60, 13);
            this.BatteryLabel.TabIndex = 20;
            this.BatteryLabel.Text = "Battery: 0%";
            // 
            // _drawableControl
            // 
            this._drawableControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._drawableControl.Drawable = emptyDrawable1;
            this._drawableControl.Location = new System.Drawing.Point(12, 37);
            this._drawableControl.Name = "_drawableControl";
            this._drawableControl.Size = new System.Drawing.Size(1557, 984);
            this._drawableControl.TabIndex = 0;
            this._drawableControl.Text = "drawableControl1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1645, 1025);
            this.Controls.Add(this.BatteryLabel);
            this.Controls.Add(this.ResistanceCheckButton);
            this.Controls.Add(this._statesStopButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._portTextBox);
            this.Controls.Add(this._ipAddressTextBox);
            this.Controls.Add(this._applyNetSettingsButton);
            this.Controls.Add(this._broadcastCheckBox);
            this.Controls.Add(this._emotionsStartButton);
            this.Controls.Add(this._stopSignalButton);
            this.Controls.Add(this._startSignalButton);
            this.Controls.Add(this._findDeviceButton);
            this.Controls.Add(this._deviceLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._drawableControl);
            this.Controls.Add(this._spectrumAmplitudeTrackBar);
            this.Name = "MainForm";
            this.Text = "Emotional states";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._spectrumAmplitudeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DrawableControl _drawableControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _deviceLabel;
        private System.Windows.Forms.Button _findDeviceButton;
        private System.Windows.Forms.Button _startSignalButton;
        private System.Windows.Forms.Button _stopSignalButton;
        private System.Windows.Forms.Button _emotionsStartButton;
        private System.Windows.Forms.CheckBox _broadcastCheckBox;
        private System.Windows.Forms.Button _applyNetSettingsButton;
        private System.Windows.Forms.TextBox _ipAddressTextBox;
        private System.Windows.Forms.TextBox _portTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _statesStopButton;
        private System.Windows.Forms.TrackBar _spectrumAmplitudeTrackBar;
        private Button ResistanceCheckButton;
        private Label BatteryLabel;
        //        private SignalView.SignalChart signalChart1;
        //        private SignalView.SignalChart signalChart2;
    }
}

