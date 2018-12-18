using Biofeedback.Spectrum;

namespace Biofeedback
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Alpha",
            "awd",
            "ww",
            "www"}, -1);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._deviceLabel = new System.Windows.Forms.ToolStripLabel();
            this._reconnectButton = new System.Windows.Forms.ToolStripButton();
            this._startSignalButton = new System.Windows.Forms.ToolStripButton();
            this._stopButton = new System.Windows.Forms.ToolStripButton();
            this._channelsListBox = new System.Windows.Forms.CheckedListBox();
            this._createIndexButton = new System.Windows.Forms.Button();
            this._startFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._stopFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._customIndexRadio = new System.Windows.Forms.RadioButton();
            this._thetaRadio = new System.Windows.Forms.RadioButton();
            this._deltaRadio = new System.Windows.Forms.RadioButton();
            this._betaRadio = new System.Windows.Forms.RadioButton();
            this._alphaRadio = new System.Windows.Forms.RadioButton();
            this._indicesListView = new System.Windows.Forms.ListView();
            this.IndexColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChannelsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrequencyColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.WindowColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._removeIndexButton = new System.Windows.Forms.Button();
            this._indexNameTextBox = new System.Windows.Forms.TextBox();
            this._indexWindowTextBox = new System.Windows.Forms.TextBox();
            this._indexWindowOverlapTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._scaleTrackBar = new System.Windows.Forms.TrackBar();
            this._timeTrackBar = new System.Windows.Forms.TrackBar();
            this._spectrumChart = new SpectrumChart();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._scaleTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._deviceLabel,
            this._reconnectButton,
            this._startSignalButton,
            this._stopButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(734, 25);
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
            this._deviceLabel.Size = new System.Drawing.Size(60, 22);
            this._deviceLabel.Text = "No device";
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
            this._startSignalButton.Image = ((System.Drawing.Image)(resources.GetObject("_startSignalButton.Image")));
            this._startSignalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._startSignalButton.Name = "_startSignalButton";
            this._startSignalButton.Size = new System.Drawing.Size(70, 22);
            this._startSignalButton.Text = "Start Signal";
            this._startSignalButton.Click += new System.EventHandler(this._startSignalButton_Click);
            // 
            // _stopButton
            // 
            this._stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._stopButton.Image = ((System.Drawing.Image)(resources.GetObject("_stopButton.Image")));
            this._stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(35, 22);
            this._stopButton.Text = "Stop";
            this._stopButton.Click += new System.EventHandler(this._stopButton_Click);
            // 
            // _channelsListBox
            // 
            this._channelsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                 | System.Windows.Forms.AnchorStyles.Left)));
            this._channelsListBox.FormattingEnabled = true;
            this._channelsListBox.Location = new System.Drawing.Point(240, 30);
            this._channelsListBox.Name = "_channelsListBox";
            this._channelsListBox.Size = new System.Drawing.Size(120, 405);
            this._channelsListBox.TabIndex = 1;
            // 
            // _createIndexButton
            // 
            this._createIndexButton.Location = new System.Drawing.Point(363, 375);
            this._createIndexButton.Name = "_createIndexButton";
            this._createIndexButton.Size = new System.Drawing.Size(76, 23);
            this._createIndexButton.TabIndex = 2;
            this._createIndexButton.Text = "Create index";
            this._createIndexButton.UseVisualStyleBackColor = true;
            this._createIndexButton.Click += new System.EventHandler(this._createIndexButton_Click);
            // 
            // _removeIndexButton
            // 
            this._removeIndexButton.Location = new System.Drawing.Point(363, 403);
            this._removeIndexButton.Name = "_removeIndexButton";
            this._removeIndexButton.Size = new System.Drawing.Size(76, 23);
            this._removeIndexButton.TabIndex = 9;
            this._removeIndexButton.Text = "Remove index";
            this._removeIndexButton.UseVisualStyleBackColor = true;
            this._removeIndexButton.Click += new System.EventHandler(this._removeIndexButton_Click);
            // 
            // _startFrequencyTextBox
            // 
            this._startFrequencyTextBox.Location = new System.Drawing.Point(363, 181);
            this._startFrequencyTextBox.Name = "_startFrequencyTextBox";
            this._startFrequencyTextBox.Size = new System.Drawing.Size(76, 20);
            this._startFrequencyTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(360, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start frequency";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Stop frequency";
            // 
            // _stopFrequencyTextBox
            // 
            this._stopFrequencyTextBox.Location = new System.Drawing.Point(363, 224);
            this._stopFrequencyTextBox.Name = "_stopFrequencyTextBox";
            this._stopFrequencyTextBox.Size = new System.Drawing.Size(76, 20);
            this._stopFrequencyTextBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._customIndexRadio);
            this.groupBox1.Controls.Add(this._thetaRadio);
            this.groupBox1.Controls.Add(this._deltaRadio);
            this.groupBox1.Controls.Add(this._betaRadio);
            this.groupBox1.Controls.Add(this._alphaRadio);
            this.groupBox1.Location = new System.Drawing.Point(366, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(76, 137);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Index type";
            // 
            // _customIndexRadio
            // 
            this._customIndexRadio.AutoSize = true;
            this._customIndexRadio.Location = new System.Drawing.Point(7, 114);
            this._customIndexRadio.Name = "_customIndexRadio";
            this._customIndexRadio.Size = new System.Drawing.Size(60, 17);
            this._customIndexRadio.TabIndex = 4;
            this._customIndexRadio.Text = "Custom";
            this._customIndexRadio.UseVisualStyleBackColor = true;
            this._customIndexRadio.CheckedChanged += new System.EventHandler(this._customIndexRadio_CheckedChanged);
            // 
            // _thetaRadio
            // 
            this._thetaRadio.AutoSize = true;
            this._thetaRadio.Location = new System.Drawing.Point(6, 91);
            this._thetaRadio.Name = "_thetaRadio";
            this._thetaRadio.Size = new System.Drawing.Size(53, 17);
            this._thetaRadio.TabIndex = 3;
            this._thetaRadio.Text = "Theta";
            this._thetaRadio.UseVisualStyleBackColor = true;
            this._thetaRadio.CheckedChanged += new System.EventHandler(this._thetaRadio_CheckedChanged);
            // 
            // _deltaRadio
            // 
            this._deltaRadio.AutoSize = true;
            this._deltaRadio.Location = new System.Drawing.Point(7, 68);
            this._deltaRadio.Name = "_deltaRadio";
            this._deltaRadio.Size = new System.Drawing.Size(50, 17);
            this._deltaRadio.TabIndex = 2;
            this._deltaRadio.Text = "Delta";
            this._deltaRadio.UseVisualStyleBackColor = true;
            this._deltaRadio.CheckedChanged += new System.EventHandler(this._deltaRadio_CheckedChanged);
            // 
            // _betaRadio
            // 
            this._betaRadio.AutoSize = true;
            this._betaRadio.Location = new System.Drawing.Point(7, 44);
            this._betaRadio.Name = "_betaRadio";
            this._betaRadio.Size = new System.Drawing.Size(47, 17);
            this._betaRadio.TabIndex = 1;
            this._betaRadio.Text = "Beta";
            this._betaRadio.UseVisualStyleBackColor = true;
            this._betaRadio.CheckedChanged += new System.EventHandler(this._betaRadio_CheckedChanged);
            // 
            // _alphaRadio
            // 
            this._alphaRadio.AutoSize = true;
            this._alphaRadio.Checked = true;
            this._alphaRadio.Location = new System.Drawing.Point(7, 20);
            this._alphaRadio.Name = "_alphaRadio";
            this._alphaRadio.Size = new System.Drawing.Size(52, 17);
            this._alphaRadio.TabIndex = 0;
            this._alphaRadio.TabStop = true;
            this._alphaRadio.Text = "Alpha";
            this._alphaRadio.UseVisualStyleBackColor = true;
            this._alphaRadio.CheckedChanged += new System.EventHandler(this._alphaRadio_CheckedChanged);
            // 
            // _indicesListView
            // 
            this._indicesListView.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom);
            this._indicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IndexColumn,
            this.ChannelsColumn,
            this.FrequencyColumn,
            this.WindowColumn,
            this.ValueColumn});
            this._indicesListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this._indicesListView.Location = new System.Drawing.Point(447, 30);
            this._indicesListView.Name = "_indicesListView";
            this._indicesListView.Size = new System.Drawing.Size(410, 395);
            this._indicesListView.TabIndex = 8;
            this._indicesListView.UseCompatibleStateImageBehavior = false;
            this._indicesListView.View = System.Windows.Forms.View.Details;
            // 
            // IndexColumn
            // 
            this.IndexColumn.Text = "Index";
            // 
            // ChannelsColumn
            // 
            this.ChannelsColumn.Text = "Channels";
            this.ChannelsColumn.Width = 80;
            // 
            // FrequencyColumn
            // 
            this.FrequencyColumn.Text = "Frequency";
            this.FrequencyColumn.Width = 80;
            // 
            // WindowColumn
            // 
            this.WindowColumn.Text = "Window/Overlap";
            this.WindowColumn.Width = 100;
            // 
            // ValueColumn
            // 
            this.ValueColumn.Text = "Value";
            this.ValueColumn.Width = 80;
            // 
            // _indexNameTextBox
            // 
            this._indexNameTextBox.Location = new System.Drawing.Point(363, 269);
            this._indexNameTextBox.Name = "_indexNameTextBox";
            this._indexNameTextBox.Size = new System.Drawing.Size(76, 20);
            this._indexNameTextBox.TabIndex = 10;
            // 
            // _indexWindowTextBox
            // 
            this._indexWindowTextBox.Location = new System.Drawing.Point(363, 310);
            this._indexWindowTextBox.Name = "_indexWindowTextBox";
            this._indexWindowTextBox.Size = new System.Drawing.Size(76, 20);
            this._indexWindowTextBox.TabIndex = 10;
            this._indexWindowTextBox.Text = "8";
            // 
            // _indexWindowOverlapTextBox
            // 
            this._indexWindowOverlapTextBox.Location = new System.Drawing.Point(363, 350);
            this._indexWindowOverlapTextBox.Name = "_indexWindowOverlapTextBox";
            this._indexWindowOverlapTextBox.Size = new System.Drawing.Size(76, 20);
            this._indexWindowOverlapTextBox.TabIndex = 10;
            this._indexWindowOverlapTextBox.Text = "0.9";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Window";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(360, 335);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Overlap coeff";
            // 
            // _scaleTrackBar
            // 
            this._scaleTrackBar.LargeChange = 10;
            this._scaleTrackBar.Location = new System.Drawing.Point(5, 25);
            this._scaleTrackBar.Maximum = 100;
            this._scaleTrackBar.Minimum = 1;
            this._scaleTrackBar.Name = "_scaleTrackBar";
            this._scaleTrackBar.Size = new System.Drawing.Size(233, 45);
            this._scaleTrackBar.TabIndex = 12;
            this._scaleTrackBar.Value = 50;
            // 
            // _timeTrackBar
            // 
            this._timeTrackBar.LargeChange = 2;
            this._timeTrackBar.Location = new System.Drawing.Point(5, 70);
            this._timeTrackBar.Maximum = 16;
            this._timeTrackBar.Minimum = 1;
            this._timeTrackBar.Name = "_timeTrackBar";
            this._timeTrackBar.Size = new System.Drawing.Size(233, 45);
            this._timeTrackBar.TabIndex = 12;
            this._timeTrackBar.Value = 8;
            // 
            // spectrumChart1
            //
            this._spectrumChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._spectrumChart.FrequencyStep = 0D;
            this._spectrumChart.Location = new System.Drawing.Point(5, 105);
            this._spectrumChart.Name = "spectrumChart1";
            this._spectrumChart.SigScale = 100;
            this._spectrumChart.Size = new System.Drawing.Size(227, 330);
            this._spectrumChart.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 461);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._indexWindowTextBox);
            this.Controls.Add(this._indexWindowOverlapTextBox);
            this.Controls.Add(this._timeTrackBar);
            this.Controls.Add(this._scaleTrackBar);
            this.Controls.Add(this._spectrumChart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._indexNameTextBox);
            this.Controls.Add(this._removeIndexButton);
            this.Controls.Add(this._indicesListView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._stopFrequencyTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._startFrequencyTextBox);
            this.Controls.Add(this._createIndexButton);
            this.Controls.Add(this._channelsListBox);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(650, 320);
            this.Name = "MainForm";
            this.Text = "Biofeedback";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._scaleTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel _deviceLabel;
        private System.Windows.Forms.ToolStripButton _startSignalButton;
        private System.Windows.Forms.ToolStripButton _stopButton;
        private System.Windows.Forms.ToolStripButton _reconnectButton;
        private System.Windows.Forms.CheckedListBox _channelsListBox;
        private System.Windows.Forms.Button _createIndexButton;
        private System.Windows.Forms.TextBox _startFrequencyTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _stopFrequencyTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton _customIndexRadio;
        private System.Windows.Forms.RadioButton _thetaRadio;
        private System.Windows.Forms.RadioButton _deltaRadio;
        private System.Windows.Forms.RadioButton _betaRadio;
        private System.Windows.Forms.RadioButton _alphaRadio;
        private System.Windows.Forms.ListView _indicesListView;
        private System.Windows.Forms.ColumnHeader IndexColumn;
        private System.Windows.Forms.ColumnHeader ValueColumn;
        private System.Windows.Forms.ColumnHeader WindowColumn;
        private System.Windows.Forms.Button _removeIndexButton;
        private System.Windows.Forms.ColumnHeader FrequencyColumn;
        private System.Windows.Forms.TextBox _indexNameTextBox;
        private System.Windows.Forms.TextBox _indexWindowTextBox;
        private System.Windows.Forms.TextBox _indexWindowOverlapTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader ChannelsColumn;
        private Spectrum.SpectrumChart _spectrumChart;
        private System.Windows.Forms.TrackBar _scaleTrackBar;
        private System.Windows.Forms.TrackBar _timeTrackBar;
    }
}

