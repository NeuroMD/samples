using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Neuro;

namespace SignalAndResistance
{
    public partial class MainForm : Form
    {
        private readonly DeviceModel _deviceModel;
        private readonly SignalViewController _signalViewController;

        public MainForm()
        {
            InitializeComponent();
            _signalViewController = new SignalViewController(this, _signalChart, _durationLabel);
            _deviceModel = new DeviceModel();
            _deviceModel.DeviceFound += _deviceModel_DeviceFound;
            _deviceModel.DeviceLost += _deviceModel_DeviceLost;
            _deviceModel.Reconnect();
        }

        private void _deviceModel_DeviceLost(object sender, System.EventArgs e)
        {
            if (!IsHandleCreated)
                return;

            Invoke((MethodInvoker) delegate
            {
                _startSignalButton.Enabled = false;
                _stopButton.Enabled = false;
                _channelComboBox.Items.Clear();
                _channelComboBox.Enabled = false;
            });
        }

        private void _deviceModel_DeviceFound(object sender, Device device)
        {
            if (device == null) return;

            Invoke((MethodInvoker) delegate
            {
                _deviceLabel.Text = device.ReadParam<string>(Parameter.Name);
                _startSignalButton.Enabled = DeviceTraits.HasChannelsWithType(device, ChannelType.Signal);
                _stopButton.Enabled = _startSignalButton.Enabled;
                _channelComboBox.Items.Clear();
                _channelComboBox.Items.AddRange(CreateChannels(device).ToArray());
                if (_channelComboBox.Items.Count > 0) _channelComboBox.SelectedIndex = 0;
                _channelComboBox.Enabled = true;
            });
        }

        private static SignalChannel CreateChannelForDevice(Device device, ChannelInfo info)
        {
            var deviceName = device.ReadParam<string>(Parameter.Name);
            if (deviceName.Equals("Brainbit") || deviceName.Equals("BrainBit"))
            {
                var filters = new[]
                    {Filter.LowPass_30Hz_SF250, Filter.HighPass_2Hz_SF250, Filter.BandStop_45_55Hz_SF250};
                return new SignalChannel(device, info, filters);
            }
            return new SignalChannel(device, info);
        }

        private static IEnumerable<ChannelAdapter<double>> CreateChannels(Device device)
        {
            var channels = DeviceTraits.GetChannelsWithType(device, ChannelType.Signal)
                                        .Select(channelInfo => CreateChannelForDevice(device, channelInfo))
                                        .Cast<IDataChannel<double>>()
                                        .ToList();

            var channelAdapters = new List<ChannelAdapter<double>>();
            foreach (var signalChannel in channels)
            {
                channelAdapters.Add(new ChannelAdapter<double>(signalChannel));
            }
            for (var i = 0; i < channels.Count - 1; ++i)
            {
                for (var j = i + 1; j < channels.Count; ++j)
                {
                    var bipolarChannel = new BipolarDoubleChannel(channels[i], channels[j]);
                    channelAdapters.Add(new ChannelAdapter<double>(bipolarChannel));
                }
            }
            return channelAdapters;
        }

        private void _reconnectButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Reconnect();
        }

        private void _signalStartButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.Device?.Execute(Command.StartSignal);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot start signal: {exception.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void _stopButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Device?.Execute(Command.StopSignal);
        }

        private void _channelComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _signalViewController.SetChannel(_channelComboBox.SelectedItem as ChannelAdapter<double>);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            _deviceModel?.Dispose();
        }
    }
}
