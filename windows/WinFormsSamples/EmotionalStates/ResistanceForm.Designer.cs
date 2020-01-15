namespace EmotionalStates
{
    partial class ResistanceForm
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
            this.T3ResistanceLabel = new System.Windows.Forms.Label();
            this.T4ResistanceLabel = new System.Windows.Forms.Label();
            this.O2ResistanceLabel = new System.Windows.Forms.Label();
            this.O1ResistanceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // T3ResistanceLabel
            // 
            this.T3ResistanceLabel.AutoSize = true;
            this.T3ResistanceLabel.Location = new System.Drawing.Point(20, 29);
            this.T3ResistanceLabel.Name = "T3ResistanceLabel";
            this.T3ResistanceLabel.Size = new System.Drawing.Size(23, 13);
            this.T3ResistanceLabel.TabIndex = 0;
            this.T3ResistanceLabel.Text = "T3:";
            // 
            // T4ResistanceLabel
            // 
            this.T4ResistanceLabel.AutoSize = true;
            this.T4ResistanceLabel.Location = new System.Drawing.Point(20, 78);
            this.T4ResistanceLabel.Name = "T4ResistanceLabel";
            this.T4ResistanceLabel.Size = new System.Drawing.Size(23, 13);
            this.T4ResistanceLabel.TabIndex = 1;
            this.T4ResistanceLabel.Text = "T4:";
            // 
            // O2ResistanceLabel
            // 
            this.O2ResistanceLabel.AutoSize = true;
            this.O2ResistanceLabel.Location = new System.Drawing.Point(120, 78);
            this.O2ResistanceLabel.Name = "O2ResistanceLabel";
            this.O2ResistanceLabel.Size = new System.Drawing.Size(24, 13);
            this.O2ResistanceLabel.TabIndex = 3;
            this.O2ResistanceLabel.Text = "O2:";
            // 
            // O1ResistanceLabel
            // 
            this.O1ResistanceLabel.AutoSize = true;
            this.O1ResistanceLabel.Location = new System.Drawing.Point(120, 29);
            this.O1ResistanceLabel.Name = "O1ResistanceLabel";
            this.O1ResistanceLabel.Size = new System.Drawing.Size(24, 13);
            this.O1ResistanceLabel.TabIndex = 2;
            this.O1ResistanceLabel.Text = "O1:";
            // 
            // ResistanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 135);
            this.Controls.Add(this.O2ResistanceLabel);
            this.Controls.Add(this.O1ResistanceLabel);
            this.Controls.Add(this.T4ResistanceLabel);
            this.Controls.Add(this.T3ResistanceLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ResistanceForm";
            this.Text = "Resistance";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ResistanceForm_FormClosing);
            this.Shown += new System.EventHandler(this.ResistanceForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label T3ResistanceLabel;
        private System.Windows.Forms.Label T4ResistanceLabel;
        private System.Windows.Forms.Label O2ResistanceLabel;
        private System.Windows.Forms.Label O1ResistanceLabel;
    }
}