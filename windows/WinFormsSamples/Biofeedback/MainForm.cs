using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Biofeedback.Spectrum;
using Neuro;

namespace Biofeedback
{
    public partial class MainForm : Form
    {
        private readonly DeviceModel _deviceModel;
        private readonly Timer _timer = new Timer();
        private SpectrumChartController _spectrumChartController;

        public MainForm()
        {
            InitializeComponent();
            _indicesListView.Items.Clear();
            SetIndexControls(EegStandardIndices.Alpha);
            SetIndexControlsEnabled(false);
            _timer.Interval = 20;
            _timer.Tick += _timer_Tick;
            _timer.Start();
            _deviceModel = new DeviceModel();
            _deviceModel.DeviceFound += _deviceModel_DeviceFound;
            _deviceModel.DeviceLost += _deviceModel_DeviceLost;
            _deviceModel.SearchStateChanged += _deviceModel_SearchStateChanged;
            _deviceModel.Reconnect();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _spectrumChartController?.Redraw();
        }

        private void _deviceModel_SearchStateChanged(object sender, bool isScanning)
        {
            if (isScanning)
            {
                Invoke((MethodInvoker)delegate
                {
                    _deviceLabel.Text = @"Waiting for device...";
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    _deviceLabel.Text = _deviceModel.Device == null ? @"No device" : _deviceModel.Device.ReadParam<string>(Parameter.Name);
                });
            }
        }

        private void _deviceModel_DeviceLost(object sender, System.EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                _spectrumChartController = null;
                _indicesListView.Items.Clear();
                _startSignalButton.Enabled = false;
                _stopButton.Enabled = false;
            });
        }

        private void _deviceModel_DeviceFound(object sender, Device device)
        {
            if (device == null) return;

            var channels = CreateChannels(device);
            _spectrumChartController = new SpectrumChartController(_spectrumChart, _timeTrackBar, _scaleTrackBar, new SpectrumModel(CreateSpectrumChannels(channels)));
            Invoke((MethodInvoker)delegate
            {
                _indicesListView.Items.Clear();
                _deviceLabel.Text = device.ReadParam<string>(Parameter.Name);
                _startSignalButton.Enabled = DeviceTraits.HasChannelsWithType(device, ChannelType.Signal);
                _stopButton.Enabled = _startSignalButton.Enabled;
                _channelsListBox.Items.AddRange(channels.ToArray());
            });
        }

        private static IList<ChannelAdapter> CreateChannels(Device device)
        {
            return DeviceTraits.GetChannelsWithType(device, ChannelType.Signal).Select(channelInfo => new ChannelAdapter(device, channelInfo)).ToList();
        }

        private static IList<SpectrumChannel> CreateSpectrumChannels(IList<ChannelAdapter> channels)
        {
            return channels.Select(x => new SpectrumChannel(x)).ToList();
        }

        private void _startSignalButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.Device?.Execute(Command.StartSignal);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot start signal receiving: {exc.Message}", "Start signal", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void _stopButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.Device?.Execute(Command.StopSignal);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot stop signal receiving: {exc.Message}", "Stop signal", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void _reconnectButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Reconnect();
        }

        private void _customIndexRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_customIndexRadio.Checked)
            {
                SetIndexControlsEnabled(true);
            }
        }

        private void _alphaRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (_alphaRadio.Checked)
            {
                SetIndexControls(EegStandardIndices.Alpha);
                SetIndexControlsEnabled(false);
            }
        }

        private void _betaRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (_betaRadio.Checked)
            {
                SetIndexControls(EegStandardIndices.Beta);
                SetIndexControlsEnabled(false);
            }
        }

        private void _deltaRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (_deltaRadio.Checked)
            {
                SetIndexControls(EegStandardIndices.Delta);
                SetIndexControlsEnabled(false);
            }
        }

        private void _thetaRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (_thetaRadio.Checked)
            {
                SetIndexControls(EegStandardIndices.Theta);
                SetIndexControlsEnabled(false);
            }
        }

        private void SetIndexControlsEnabled(bool isEnabled)
        {
            _startFrequencyTextBox.Enabled = isEnabled;
            _stopFrequencyTextBox.Enabled = isEnabled;
            _indexNameTextBox.Enabled = isEnabled;
        }

        private void SetIndexControls(EegIndex index)
        {
            _startFrequencyTextBox.Text = index.FrequencyBottom.ToString("F4");
            _stopFrequencyTextBox.Text = index.FrequencyTop.ToString("F4");
            _indexNameTextBox.Text = index.Name;
        }

        private void _createIndexButton_Click(object sender, System.EventArgs e)
        {
            if (!float.TryParse(_startFrequencyTextBox.Text, out var startFreq))
            {
                MessageBox.Show("Wrong low frequency value", "Index creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!float.TryParse(_stopFrequencyTextBox.Text, out var stopFreq))
            {
                MessageBox.Show("Wrong high frequency value", "Index creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (stopFreq <= startFreq)
            {
                MessageBox.Show("Wrong frequencies: high frequency must be greater than low", "Index creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var index = new EegIndex {Name = _indexNameTextBox.Text, FrequencyBottom = startFreq, FrequencyTop = stopFreq};
            var checkedChannels = _channelsListBox.CheckedItems.OfType<ChannelAdapter>();
            var indexChannel = new EegIndexChannel(index, checkedChannels);
            _indicesListView.Items.Add(new IndexListItem(this, indexChannel, index, checkedChannels.Select(x=>x.ToString())));
        }

        private void _removeIndexButton_Click(object sender, System.EventArgs e)
        {
            var selectedIndices = _indicesListView.SelectedItems;
            foreach (ListViewItem selectedIndex in selectedIndices)
            {
                _indicesListView.Items.Remove(selectedIndex);
            }
        }
    }
}
