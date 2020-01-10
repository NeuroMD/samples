namespace CallibriFeatures.DrawableControl
{
    partial class DrawableControl
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
                _redrawTimer.Dispose();
                components.Dispose();
                _graphicsBuffer.Dispose();
                _graphicsContext.Dispose();
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
            this.SuspendLayout();
            // 
            // DrawableControl
            // 
            components = new System.ComponentModel.Container();

            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawableControl_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawableControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawableControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawableControl_MouseUp);
            this.MouseLeave += new System.EventHandler(this.DrawableControl_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
