namespace CallibriFeatures.Devices
{
    partial class MEMSControl
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
            this.MemsGroupBox = new System.Windows.Forms.GroupBox();
            this.GyroscopeSensLabel = new System.Windows.Forms.Label();
            this.GyroscopeSensComboBox = new System.Windows.Forms.ComboBox();
            this.AccelerometerSensLabel = new System.Windows.Forms.Label();
            this.AccelerometerSensComboBox = new System.Windows.Forms.ComboBox();
            this.StartMemsButton = new System.Windows.Forms.Button();
            this.StopMemsButton = new System.Windows.Forms.Button();
            this.StopOrientationButton = new System.Windows.Forms.Button();
            this.StartOrientationButton = new System.Windows.Forms.Button();
            this.ResetQuternionButton = new System.Windows.Forms.Button();
            this.CalibrateMemsButton = new System.Windows.Forms.Button();
            this.CalibrationProgressBar = new System.Windows.Forms.ProgressBar();
            this.MemsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MemsGroupBox
            // 
            this.MemsGroupBox.Controls.Add(this.CalibrationProgressBar);
            this.MemsGroupBox.Controls.Add(this.CalibrateMemsButton);
            this.MemsGroupBox.Controls.Add(this.ResetQuternionButton);
            this.MemsGroupBox.Controls.Add(this.StopOrientationButton);
            this.MemsGroupBox.Controls.Add(this.StartOrientationButton);
            this.MemsGroupBox.Controls.Add(this.StopMemsButton);
            this.MemsGroupBox.Controls.Add(this.StartMemsButton);
            this.MemsGroupBox.Controls.Add(this.AccelerometerSensComboBox);
            this.MemsGroupBox.Controls.Add(this.AccelerometerSensLabel);
            this.MemsGroupBox.Controls.Add(this.GyroscopeSensComboBox);
            this.MemsGroupBox.Controls.Add(this.GyroscopeSensLabel);
            this.MemsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MemsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.MemsGroupBox.Name = "MemsGroupBox";
            this.MemsGroupBox.Size = new System.Drawing.Size(242, 203);
            this.MemsGroupBox.TabIndex = 0;
            this.MemsGroupBox.TabStop = false;
            this.MemsGroupBox.Text = "MEMS";
            // 
            // GyroscopeSensLabel
            // 
            this.GyroscopeSensLabel.AutoSize = true;
            this.GyroscopeSensLabel.Location = new System.Drawing.Point(6, 23);
            this.GyroscopeSensLabel.Name = "GyroscopeSensLabel";
            this.GyroscopeSensLabel.Size = new System.Drawing.Size(109, 13);
            this.GyroscopeSensLabel.TabIndex = 0;
            this.GyroscopeSensLabel.Text = "Gyroscope sensitivity:";
            // 
            // GyroscopeSensComboBox
            // 
            this.GyroscopeSensComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GyroscopeSensComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GyroscopeSensComboBox.FormattingEnabled = true;
            this.GyroscopeSensComboBox.Location = new System.Drawing.Point(138, 20);
            this.GyroscopeSensComboBox.Name = "GyroscopeSensComboBox";
            this.GyroscopeSensComboBox.Size = new System.Drawing.Size(98, 21);
            this.GyroscopeSensComboBox.TabIndex = 1;
            // 
            // AccelerometerSensLabel
            // 
            this.AccelerometerSensLabel.AutoSize = true;
            this.AccelerometerSensLabel.Location = new System.Drawing.Point(6, 50);
            this.AccelerometerSensLabel.Name = "AccelerometerSensLabel";
            this.AccelerometerSensLabel.Size = new System.Drawing.Size(126, 13);
            this.AccelerometerSensLabel.TabIndex = 2;
            this.AccelerometerSensLabel.Text = "Accelerometer sensitivity:";
            // 
            // AccelerometerSensComboBox
            // 
            this.AccelerometerSensComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccelerometerSensComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccelerometerSensComboBox.FormattingEnabled = true;
            this.AccelerometerSensComboBox.Location = new System.Drawing.Point(138, 47);
            this.AccelerometerSensComboBox.Name = "AccelerometerSensComboBox";
            this.AccelerometerSensComboBox.Size = new System.Drawing.Size(98, 21);
            this.AccelerometerSensComboBox.TabIndex = 3;
            // 
            // StartMemsButton
            // 
            this.StartMemsButton.Location = new System.Drawing.Point(6, 81);
            this.StartMemsButton.Name = "StartMemsButton";
            this.StartMemsButton.Size = new System.Drawing.Size(91, 23);
            this.StartMemsButton.TabIndex = 4;
            this.StartMemsButton.Text = "Start MEMS";
            this.StartMemsButton.UseVisualStyleBackColor = true;
            // 
            // StopMemsButton
            // 
            this.StopMemsButton.Location = new System.Drawing.Point(103, 81);
            this.StopMemsButton.Name = "StopMemsButton";
            this.StopMemsButton.Size = new System.Drawing.Size(96, 23);
            this.StopMemsButton.TabIndex = 5;
            this.StopMemsButton.Text = "Stop MEMS";
            this.StopMemsButton.UseVisualStyleBackColor = true;
            // 
            // StopOrientationButton
            // 
            this.StopOrientationButton.Location = new System.Drawing.Point(103, 110);
            this.StopOrientationButton.Name = "StopOrientationButton";
            this.StopOrientationButton.Size = new System.Drawing.Size(96, 23);
            this.StopOrientationButton.TabIndex = 7;
            this.StopOrientationButton.Text = "Stop orientation";
            this.StopOrientationButton.UseVisualStyleBackColor = true;
            // 
            // StartOrientationButton
            // 
            this.StartOrientationButton.Location = new System.Drawing.Point(6, 110);
            this.StartOrientationButton.Name = "StartOrientationButton";
            this.StartOrientationButton.Size = new System.Drawing.Size(91, 23);
            this.StartOrientationButton.TabIndex = 6;
            this.StartOrientationButton.Text = "Start orientation";
            this.StartOrientationButton.UseVisualStyleBackColor = true;
            // 
            // ResetQuternionButton
            // 
            this.ResetQuternionButton.Location = new System.Drawing.Point(6, 139);
            this.ResetQuternionButton.Name = "ResetQuternionButton";
            this.ResetQuternionButton.Size = new System.Drawing.Size(126, 23);
            this.ResetQuternionButton.TabIndex = 8;
            this.ResetQuternionButton.Text = "Reset quaternion";
            this.ResetQuternionButton.UseVisualStyleBackColor = true;
            // 
            // CalibrateMemsButton
            // 
            this.CalibrateMemsButton.Location = new System.Drawing.Point(6, 168);
            this.CalibrateMemsButton.Name = "CalibrateMemsButton";
            this.CalibrateMemsButton.Size = new System.Drawing.Size(126, 23);
            this.CalibrateMemsButton.TabIndex = 9;
            this.CalibrateMemsButton.Text = "Calibrate MEMS";
            this.CalibrateMemsButton.UseVisualStyleBackColor = true;
            // 
            // CalibrationProgressBar
            // 
            this.CalibrationProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CalibrationProgressBar.Location = new System.Drawing.Point(138, 170);
            this.CalibrationProgressBar.Name = "CalibrationProgressBar";
            this.CalibrationProgressBar.Size = new System.Drawing.Size(98, 19);
            this.CalibrationProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.CalibrationProgressBar.TabIndex = 10;
            // 
            // MEMSControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MemsGroupBox);
            this.Name = "MEMSControl";
            this.Size = new System.Drawing.Size(242, 203);
            this.MemsGroupBox.ResumeLayout(false);
            this.MemsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox MemsGroupBox;
        private System.Windows.Forms.ComboBox GyroscopeSensComboBox;
        private System.Windows.Forms.Label GyroscopeSensLabel;
        private System.Windows.Forms.Button StopMemsButton;
        private System.Windows.Forms.Button StartMemsButton;
        private System.Windows.Forms.ComboBox AccelerometerSensComboBox;
        private System.Windows.Forms.Label AccelerometerSensLabel;
        private System.Windows.Forms.Button CalibrateMemsButton;
        private System.Windows.Forms.Button ResetQuternionButton;
        private System.Windows.Forms.Button StopOrientationButton;
        private System.Windows.Forms.Button StartOrientationButton;
        private System.Windows.Forms.ProgressBar CalibrationProgressBar;
    }
}
