using Indices.Spectrum;

namespace Indices
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
            this.WindowColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._removeIndexButton = new System.Windows.Forms.Button();
            this._indexNameTextBox = new System.Windows.Forms.TextBox();
            this._indexWindowTextBox = new System.Windows.Forms.TextBox();
            this._indexWindowOverlapTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._scaleTrackBar = new System.Windows.Forms.TrackBar();
            this._timeTrackBar = new System.Windows.Forms.TrackBar();
            this._signalChart = new SignalView.SignalChart();
            this._durationLabek = new System.Windows.Forms.Label();
            this._addEmulationChannelButton = new System.Windows.Forms.Button();
            this._spectrumAmplitudeLabel = new System.Windows.Forms.Label();
            this._spectrumTimeLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this._emulationParamsBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._deviceListBox = new System.Windows.Forms.ListBox();
            this._addDeviceButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this._filtersListBox = new System.Windows.Forms.CheckedListBox();
            this._spectrumChart = new Indices.Spectrum.SpectrumChart();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._scaleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._timeTrackBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._startSignalButton,
            this._stopButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1400, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _startSignalButton
            // 
            this._startSignalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._startSignalButton.Image = ((System.Drawing.Image)(resources.GetObject("_startSignalButton.Image")));
            this._startSignalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._startSignalButton.Name = "_startSignalButton";
            this._startSignalButton.Size = new System.Drawing.Size(35, 22);
            this._startSignalButton.Text = "Start";
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
            this._channelsListBox.FormattingEnabled = true;
            this._channelsListBox.Location = new System.Drawing.Point(6, 16);
            this._channelsListBox.Name = "_channelsListBox";
            this._channelsListBox.Size = new System.Drawing.Size(292, 379);
            this._channelsListBox.TabIndex = 1;
            this._channelsListBox.SelectedIndexChanged += new System.EventHandler(this._channelsListBox_SelectedIndexChanged);
            // 
            // _createIndexButton
            // 
            this._createIndexButton.Location = new System.Drawing.Point(878, 380);
            this._createIndexButton.Name = "_createIndexButton";
            this._createIndexButton.Size = new System.Drawing.Size(79, 23);
            this._createIndexButton.TabIndex = 2;
            this._createIndexButton.Text = "Create index";
            this._createIndexButton.UseVisualStyleBackColor = true;
            this._createIndexButton.Click += new System.EventHandler(this._createIndexButton_Click);
            // 
            // _startFrequencyTextBox
            // 
            this._startFrequencyTextBox.Location = new System.Drawing.Point(878, 186);
            this._startFrequencyTextBox.Name = "_startFrequencyTextBox";
            this._startFrequencyTextBox.Size = new System.Drawing.Size(78, 20);
            this._startFrequencyTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(875, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start frequency";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(875, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Stop frequency";
            // 
            // _stopFrequencyTextBox
            // 
            this._stopFrequencyTextBox.Location = new System.Drawing.Point(878, 229);
            this._stopFrequencyTextBox.Name = "_stopFrequencyTextBox";
            this._stopFrequencyTextBox.Size = new System.Drawing.Size(79, 20);
            this._stopFrequencyTextBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._customIndexRadio);
            this.groupBox1.Controls.Add(this._thetaRadio);
            this.groupBox1.Controls.Add(this._deltaRadio);
            this.groupBox1.Controls.Add(this._betaRadio);
            this.groupBox1.Controls.Add(this._alphaRadio);
            this.groupBox1.Location = new System.Drawing.Point(881, 30);
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
            this._indicesListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._indicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IndexColumn,
            this.ChannelsColumn,
            this.FrequencyColumn,
            this.WindowColumn,
            this.ValueColumn});
            this._indicesListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this._indicesListView.Location = new System.Drawing.Point(963, 35);
            this._indicesListView.Name = "_indicesListView";
            this._indicesListView.Size = new System.Drawing.Size(415, 396);
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
            // _removeIndexButton
            // 
            this._removeIndexButton.Location = new System.Drawing.Point(878, 408);
            this._removeIndexButton.Name = "_removeIndexButton";
            this._removeIndexButton.Size = new System.Drawing.Size(79, 23);
            this._removeIndexButton.TabIndex = 9;
            this._removeIndexButton.Text = "Remove index";
            this._removeIndexButton.UseVisualStyleBackColor = true;
            this._removeIndexButton.Click += new System.EventHandler(this._removeIndexButton_Click);
            // 
            // _indexNameTextBox
            // 
            this._indexNameTextBox.Location = new System.Drawing.Point(878, 274);
            this._indexNameTextBox.Name = "_indexNameTextBox";
            this._indexNameTextBox.Size = new System.Drawing.Size(79, 20);
            this._indexNameTextBox.TabIndex = 10;
            // 
            // _indexWindowTextBox
            // 
            this._indexWindowTextBox.Location = new System.Drawing.Point(878, 315);
            this._indexWindowTextBox.Name = "_indexWindowTextBox";
            this._indexWindowTextBox.Size = new System.Drawing.Size(79, 20);
            this._indexWindowTextBox.TabIndex = 10;
            this._indexWindowTextBox.Text = "8";
            // 
            // _indexWindowOverlapTextBox
            // 
            this._indexWindowOverlapTextBox.Location = new System.Drawing.Point(878, 355);
            this._indexWindowOverlapTextBox.Name = "_indexWindowOverlapTextBox";
            this._indexWindowOverlapTextBox.Size = new System.Drawing.Size(79, 20);
            this._indexWindowOverlapTextBox.TabIndex = 10;
            this._indexWindowOverlapTextBox.Text = "0.9";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(875, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(875, 299);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Window";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(875, 340);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Overlap coeff";
            // 
            // _scaleTrackBar
            // 
            this._scaleTrackBar.LargeChange = 10;
            this._scaleTrackBar.Location = new System.Drawing.Point(57, 25);
            this._scaleTrackBar.Maximum = 100;
            this._scaleTrackBar.Minimum = 1;
            this._scaleTrackBar.Name = "_scaleTrackBar";
            this._scaleTrackBar.Size = new System.Drawing.Size(297, 45);
            this._scaleTrackBar.TabIndex = 12;
            this._scaleTrackBar.Value = 50;
            // 
            // _timeTrackBar
            // 
            this._timeTrackBar.LargeChange = 2;
            this._timeTrackBar.Location = new System.Drawing.Point(57, 70);
            this._timeTrackBar.Maximum = 16;
            this._timeTrackBar.Minimum = 1;
            this._timeTrackBar.Name = "_timeTrackBar";
            this._timeTrackBar.Size = new System.Drawing.Size(297, 45);
            this._timeTrackBar.TabIndex = 12;
            this._timeTrackBar.Value = 8;
            // 
            // _signalChart
            // 
            this._signalChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._signalChart.BackColor = System.Drawing.SystemColors.Control;
            this._signalChart.Location = new System.Drawing.Point(360, 431);
            this._signalChart.MinimumSize = new System.Drawing.Size(500, 440);
            this._signalChart.Name = "_signalChart";
            this._signalChart.PeakDetector = false;
            this._signalChart.ScaleX = 14;
            this._signalChart.ScaleY = 10;
            this._signalChart.Size = new System.Drawing.Size(1028, 469);
            this._signalChart.TabIndex = 13;
            // 
            // _durationLabek
            // 
            this._durationLabek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._durationLabek.AutoSize = true;
            this._durationLabek.Location = new System.Drawing.Point(760, 870);
            this._durationLabek.Name = "_durationLabek";
            this._durationLabek.Size = new System.Drawing.Size(13, 13);
            this._durationLabek.TabIndex = 14;
            this._durationLabek.Text = "0";
            // 
            // _addEmulationChannelButton
            // 
            this._addEmulationChannelButton.Location = new System.Drawing.Point(51, 105);
            this._addEmulationChannelButton.Name = "_addEmulationChannelButton";
            this._addEmulationChannelButton.Size = new System.Drawing.Size(96, 23);
            this._addEmulationChannelButton.TabIndex = 15;
            this._addEmulationChannelButton.Text = "Add";
            this._addEmulationChannelButton.UseVisualStyleBackColor = true;
            this._addEmulationChannelButton.Click += new System.EventHandler(this._addEmulatorButton_Click);
            // 
            // _spectrumAmplitudeLabel
            // 
            this._spectrumAmplitudeLabel.AutoSize = true;
            this._spectrumAmplitudeLabel.Location = new System.Drawing.Point(12, 30);
            this._spectrumAmplitudeLabel.Name = "_spectrumAmplitudeLabel";
            this._spectrumAmplitudeLabel.Size = new System.Drawing.Size(41, 13);
            this._spectrumAmplitudeLabel.TabIndex = 16;
            this._spectrumAmplitudeLabel.Text = "500 uV";
            // 
            // _spectrumTimeLabel
            // 
            this._spectrumTimeLabel.AutoSize = true;
            this._spectrumTimeLabel.Location = new System.Drawing.Point(12, 73);
            this._spectrumTimeLabel.Name = "_spectrumTimeLabel";
            this._spectrumTimeLabel.Size = new System.Drawing.Size(21, 13);
            this._spectrumTimeLabel.TabIndex = 17;
            this._spectrumTimeLabel.Text = "8 s";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this._emulationParamsBox);
            this.groupBox2.Controls.Add(this._addEmulationChannelButton);
            this.groupBox2.Location = new System.Drawing.Point(360, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 134);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Emulation channel";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "uV/Hz;uV/Hz;...";
            // 
            // _emulationParamsBox
            // 
            this._emulationParamsBox.Location = new System.Drawing.Point(7, 40);
            this._emulationParamsBox.Multiline = true;
            this._emulationParamsBox.Name = "_emulationParamsBox";
            this._emulationParamsBox.Size = new System.Drawing.Size(192, 62);
            this._emulationParamsBox.TabIndex = 16;
            this._emulationParamsBox.Text = "50/5;100/8;25/16;30/50";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._deviceListBox);
            this.groupBox3.Controls.Add(this._addDeviceButton);
            this.groupBox3.Location = new System.Drawing.Point(360, 297);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(205, 134);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Devices";
            // 
            // _deviceListBox
            // 
            this._deviceListBox.FormattingEnabled = true;
            this._deviceListBox.Location = new System.Drawing.Point(5, 19);
            this._deviceListBox.Name = "_deviceListBox";
            this._deviceListBox.Size = new System.Drawing.Size(194, 82);
            this._deviceListBox.TabIndex = 1;
            // 
            // _addDeviceButton
            // 
            this._addDeviceButton.Location = new System.Drawing.Point(52, 107);
            this._addDeviceButton.Name = "_addDeviceButton";
            this._addDeviceButton.Size = new System.Drawing.Size(95, 23);
            this._addDeviceButton.TabIndex = 0;
            this._addDeviceButton.Text = "Select";
            this._addDeviceButton.UseVisualStyleBackColor = true;
            this._addDeviceButton.Click += new System.EventHandler(this._addDeviceButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this._channelsListBox);
            this.groupBox4.Location = new System.Drawing.Point(571, 30);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(304, 401);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Channels";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this._filtersListBox);
            this.groupBox5.Location = new System.Drawing.Point(360, 30);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(205, 131);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Filters";
            // 
            // _filtersListBox
            // 
            this._filtersListBox.FormattingEnabled = true;
            this._filtersListBox.Location = new System.Drawing.Point(5, 16);
            this._filtersListBox.Name = "_filtersListBox";
            this._filtersListBox.Size = new System.Drawing.Size(194, 109);
            this._filtersListBox.TabIndex = 0;
            // 
            // _spectrumChart
            // 
            this._spectrumChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._spectrumChart.FrequencyStep = 0D;
            this._spectrumChart.Location = new System.Drawing.Point(5, 116);
            this._spectrumChart.Name = "_spectrumChart";
            this._spectrumChart.SigScale = 100;
            this._spectrumChart.Size = new System.Drawing.Size(349, 452);
            this._spectrumChart.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 912);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._spectrumTimeLabel);
            this.Controls.Add(this._spectrumAmplitudeLabel);
            this.Controls.Add(this._durationLabek);
            this.Controls.Add(this._signalChart);
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
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox4);
            this.MinimumSize = new System.Drawing.Size(650, 320);
            this.Name = "MainForm";
            this.Text = "Signal indices";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._scaleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._timeTrackBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _startSignalButton;
        private System.Windows.Forms.ToolStripButton _stopButton;
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
        private SignalView.SignalChart _signalChart;
        private System.Windows.Forms.Label _durationLabek;
        private System.Windows.Forms.Button _addEmulationChannelButton;
        private System.Windows.Forms.Label _spectrumAmplitudeLabel;
        private System.Windows.Forms.Label _spectrumTimeLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _emulationParamsBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox _deviceListBox;
        private System.Windows.Forms.Button _addDeviceButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox _filtersListBox;
    }
}

