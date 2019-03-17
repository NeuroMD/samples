using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Indices.Spectrum;
using Neuro;

namespace Indices
{
    public partial class MainForm : Form
    {
        private readonly DeviceModel _deviceModel;
        private readonly Timer _timer = new Timer();
        private SpectrumChartController _spectrumChartController;
        private readonly SpectrumModel _spectrumModel = new SpectrumModel();
        private readonly SignalViewController _signalViewController;
        private readonly IList<EmulationChannel> _emulChannels = new List<EmulationChannel>();
        
        public MainForm()
        {
            InitializeComponent();
            _indicesListView.Items.Clear();
            SetIndexControls(EegStandardIndices.Alpha);
            SetIndexControlsEnabled(false);
            _timer.Interval = 20;
            _timer.Tick += _timer_Tick;
            _timer.Start();
            _filtersListBox.Items.AddRange(Enum.GetValues(typeof(Filter)).OfType<object>().ToArray());
            _deviceModel = new DeviceModel();
            _deviceModel.DeviceListChanged += _deviceModel_DeviceListChanged;
            _deviceModel.ChannelListChanged += _deviceModel_ChannelListChanged;
            _spectrumChartController = new SpectrumChartController(_spectrumChart, _timeTrackBar, _scaleTrackBar, _spectrumModel, _spectrumAmplitudeLabel, _spectrumTimeLabel, 
                _lowFreqTrackBar, _highFreqTrackBar, _lowFreqLabel, _highFreqLabel, _wattLabel,
                _rectangularWindowRadio, _sineWindowRadio, _hammingWindowRadio, _blackmanWindowRadio);
            _signalViewController = new SignalViewController(this, _signalChart, _durationLabek);
        }

        private void _deviceModel_ChannelListChanged(object sender, EventArgs e)
        {
            var channels = CreateChannels(_deviceModel.DeviceChannels);
            BeginInvoke((MethodInvoker)delegate
            {
                _channelsListBox.Items.AddRange(channels.ToArray());
            });
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _spectrumChartController?.Redraw();
        }

        private void _deviceModel_DeviceListChanged(object sender, EventArgs args)
        {
            var devices = _deviceModel.Devices;
            BeginInvoke((MethodInvoker)delegate
            {
                _deviceListBox.Items.Clear();
                _deviceListBox.Items.AddRange(devices.Select(x=>new DeviceWrapper(x)).ToArray());
            });
        }

        private IList<DoubleSignalChannelWrap> CreateChannels(IList<IDataChannel<double>> rawChannels)
        {
            var channels = new List<IDataChannel<double>>();
            var filters = GetSelectedFilters();
            foreach (var dataChannel in rawChannels)
            {
                if (filters.Length > 0)
                {
                    
                    channels.Add(new FilteredChannel(dataChannel, filters));
                }
                else
                {
                    channels.Add(dataChannel);
                }
            }
            
            var channelAdapters = new List<DoubleSignalChannelWrap>();
            var name = "";
            if (filters.Length > 0)
            {
                name = '\n' + filters.Select(x => x.ToString()).Aggregate((a, b) => a + '\n' + b);
            }
            foreach (var signalChannel in channels)
            {
                channelAdapters.Add(new DoubleSignalChannelWrap(signalChannel, signalChannel.Info.Name+name));
            }
            for (var i = 0; i < channels.Count - 1; ++i)
            {
                for (var j = i + 1; j < channels.Count; ++j)
                {
                    var bipolarChannel = new BipolarDoubleChannel(channels[i], channels[j]);
                    channelAdapters.Add(new DoubleSignalChannelWrap(bipolarChannel));
                }
            }
            return channelAdapters;
        }

        private void _startSignalButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.StartSignal();
                foreach (var emulChannel in _emulChannels)
                {
                    emulChannel.StartTimer();
                }
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
                _deviceModel.StopSignal();
                foreach (var emulChannel in _emulChannels)
                {
                    emulChannel.StopTimer();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot stop signal receiving: {exc.Message}", "Stop signal", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

            if (!double.TryParse(_indexWindowTextBox.Text, out var window))
            {
                MessageBox.Show("Wrong window duration value", "Index creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(_indexWindowOverlapTextBox.Text, out var overlap))
            {
                MessageBox.Show("Wrong overlap value", "Index creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var checkedChannels = _channelsListBox.CheckedItems.OfType<DoubleSignalChannelWrap>().ToList();
            var spectrumPowerChannel = new SpectrumPowerChannel(checkedChannels.Select(x => new SpectrumChannel(x)), startFreq, stopFreq, _indexNameTextBox.Text, window, overlap);
            _indicesListView.Items.Add(new IndexListItem(this, spectrumPowerChannel, checkedChannels.Select(x=>x.ToString()), startFreq, stopFreq, window, overlap));
        }

        private void _removeIndexButton_Click(object sender, System.EventArgs e)
        {
            var selectedIndices = _indicesListView.SelectedItems;
            foreach (ListViewItem selectedIndex in selectedIndices)
            {
                _indicesListView.Items.Remove(selectedIndex);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _deviceModel.Dispose();
        }

        private Filter[] GetSelectedFilters()
        {
            return _filtersListBox.CheckedItems.OfType<Filter>().ToArray();
        }

        private void _addEmulatorButton_Click(object sender, EventArgs e)
        {
            var componentStrings = _emulationParamsBox.Text.Split(';');
            var components = new List<EmulationSine>();
            foreach (var componentString in componentStrings)
            {
                var amplAndFreqStrings = componentString.Split('/');
                if (amplAndFreqStrings.Length != 2)
                {
                    MessageBox.Show("Wrong parameters string", "Emulation params string", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    var amplitudeUv = double.Parse(amplAndFreqStrings[0], NumberStyles.Number) * 1e-6;
                    var frequencyHz = double.Parse(amplAndFreqStrings[1], NumberStyles.Number);
                    components.Add(new EmulationSine { AmplitudeV = amplitudeUv, FrequencyHz = frequencyHz, PhaseShiftRad = 0});
                }
                catch
                {
                    MessageBox.Show("Wrong parameters string", "Emulation params string", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            
            var emulChannel = new EmulationChannel(components, 250, 1250);
            var filters = GetSelectedFilters();
            var name = _emulationParamsBox.Text;
            _emulChannels.Add(emulChannel);
            if (filters.Length > 0)
            {
                name += '\n';
                name += filters.Select(x => x.ToString()).Aggregate((a, b) => a + '\n' + b);
                _channelsListBox.Items.Add(new DoubleSignalChannelWrap(new FilteredChannel(emulChannel, filters), name));
            }
            else
            {
                _channelsListBox.Items.Add(new DoubleSignalChannelWrap(emulChannel, name));
            }
        }

        private void _channelsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_channelsListBox.SelectedItem is DoubleSignalChannelWrap channel)
            {
                _signalViewController.SetChannel(channel);
                _spectrumModel.SetChannels(new SpectrumChannel(channel));
            }
        }

        private void _addDeviceButton_Click(object sender, EventArgs e)
        {
            if (_deviceListBox.SelectedItem is DeviceWrapper deviceInfo)
            {
                _deviceModel.SelectDevice(deviceInfo);
            }
        }
    }
}
