namespace EmotionalStates.IndexChart
{
    partial class IndexSettingsControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this._delayTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._applySettingsButton = new System.Windows.Forms.Button();
            this._betaWeightTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._alphaWeightTextBox = new System.Windows.Forms.TextBox();
            this._thetaWeightTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._deltaWeightTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._delayTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._applySettingsButton);
            this.groupBox1.Controls.Add(this._betaWeightTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this._alphaWeightTextBox);
            this.groupBox1.Controls.Add(this._thetaWeightTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this._deltaWeightTextBox);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 184);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rhythm weights";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "s";
            // 
            // _delayTextBox
            // 
            this._delayTextBox.Location = new System.Drawing.Point(76, 123);
            this._delayTextBox.Name = "_delayTextBox";
            this._delayTextBox.Size = new System.Drawing.Size(39, 20);
            this._delayTextBox.TabIndex = 10;
            this._delayTextBox.Text = "3.0";
            this._delayTextBox.TextChanged += new System.EventHandler(this.SettingTextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Delay";
            // 
            // _applySettingsButton
            // 
            this._applySettingsButton.Location = new System.Drawing.Point(32, 155);
            this._applySettingsButton.Name = "_applySettingsButton";
            this._applySettingsButton.Size = new System.Drawing.Size(75, 23);
            this._applySettingsButton.TabIndex = 8;
            this._applySettingsButton.Text = "Apply";
            this._applySettingsButton.UseVisualStyleBackColor = true;
            this._applySettingsButton.Click += new System.EventHandler(this._applySettingsButton_Click);
            // 
            // _betaWeightTextBox
            // 
            this._betaWeightTextBox.Location = new System.Drawing.Point(76, 97);
            this._betaWeightTextBox.Name = "_betaWeightTextBox";
            this._betaWeightTextBox.Size = new System.Drawing.Size(49, 20);
            this._betaWeightTextBox.TabIndex = 7;
            this._betaWeightTextBox.Text = "1.1";
            this._betaWeightTextBox.TextChanged += new System.EventHandler(this.SettingTextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Beta";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Alpha";
            // 
            // _alphaWeightTextBox
            // 
            this._alphaWeightTextBox.Location = new System.Drawing.Point(76, 71);
            this._alphaWeightTextBox.Name = "_alphaWeightTextBox";
            this._alphaWeightTextBox.Size = new System.Drawing.Size(49, 20);
            this._alphaWeightTextBox.TabIndex = 4;
            this._alphaWeightTextBox.Text = "1.00";
            this._alphaWeightTextBox.TextChanged += new System.EventHandler(this.SettingTextChanged);
            // 
            // _thetaWeightTextBox
            // 
            this._thetaWeightTextBox.Location = new System.Drawing.Point(76, 45);
            this._thetaWeightTextBox.Name = "_thetaWeightTextBox";
            this._thetaWeightTextBox.Size = new System.Drawing.Size(49, 20);
            this._thetaWeightTextBox.TabIndex = 3;
            this._thetaWeightTextBox.Text = "0.00";
            this._thetaWeightTextBox.TextChanged += new System.EventHandler(this.SettingTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Theta";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Delta";
            // 
            // _deltaWeightTextBox
            // 
            this._deltaWeightTextBox.Location = new System.Drawing.Point(76, 19);
            this._deltaWeightTextBox.Name = "_deltaWeightTextBox";
            this._deltaWeightTextBox.Size = new System.Drawing.Size(49, 20);
            this._deltaWeightTextBox.TabIndex = 0;
            this._deltaWeightTextBox.Text = "0.00";
            this._deltaWeightTextBox.TextChanged += new System.EventHandler(this.SettingTextChanged);
            // 
            // IndexSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "IndexSettingsControl";
            this.Size = new System.Drawing.Size(139, 184);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button _applySettingsButton;
        private System.Windows.Forms.TextBox _betaWeightTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _alphaWeightTextBox;
        private System.Windows.Forms.TextBox _thetaWeightTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _deltaWeightTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _delayTextBox;
        private System.Windows.Forms.Label label1;
    }
}
