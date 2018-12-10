using System.Collections.Generic;
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
            _deviceModel.SearchStateChanged += _deviceModel_SearchStateChanged;
            _deviceModel.Reconnect();
        }

        private void _deviceModel_SearchStateChanged(object sender, bool isScanning)
        {
            if (isScanning)
            {
                Invoke((MethodInvoker) delegate
                {
                    _deviceLabel.Text = @"Waiting for device...";
                });
            }
            else
            {
                Invoke((MethodInvoker) delegate
                {
                    _deviceLabel.Text = _deviceModel.Device == null ? @"No device" : _deviceModel.Device.ReadParam<string>(Parameter.Name);
                });
            }
        }

        private void _deviceModel_DeviceLost(object sender, System.EventArgs e)
        {
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

        private static IEnumerable<ChannelAdapter<double>> CreateChannels(Device device)
        {
            var channels = new List<ChannelAdapter<double>>();
            foreach (var channelInfo in DeviceTraits.GetChannelsWithType(device, ChannelType.Signal))
            {
                var signalChannel = new SignalChannel(device, channelInfo);
                channels.Add(new ChannelAdapter<double>(signalChannel));
            }
            return channels;
        }

        private void _reconnectButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Reconnect();
        }

        private void _signalStartButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Device?.Execute(Command.StartSignal);
        }

        private void _stopButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.Device?.Execute(Command.StopSignal);
        }

        private void _channelComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _signalViewController.SetChannel(_channelComboBox.SelectedItem as ChannelAdapter<double>);
        }
    }
}
