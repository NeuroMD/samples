using System;
using System.Windows.Forms;
using Neuro;

namespace DeviceInfo
{
    public partial class DeviceForm : Form
    {
        private readonly DeviceEnumerator _enumerator;

        private DeviceAdapter _selectedDevice;

        public DeviceForm()
        {
            InitializeComponent();
            _enumerator = new DeviceEnumerator(DeviceType.Any);
            _enumerator.DeviceListChanged += _enumerator_DeviceListChanged;
        }

        private void _enumerator_DeviceListChanged(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker) delegate { _deviceListBox.Items.Clear(); });
            foreach (var deviceInfo in _enumerator.Devices)
            {
                var deviceAdapter = new DeviceAdapter(new Device(deviceInfo));
                BeginInvoke((MethodInvoker)delegate { _deviceListBox.Items.Add(deviceAdapter); });
                deviceAdapter.ConnectionStateChanged += (o, state) => Invoke((MethodInvoker)delegate
                {
                    _deviceListBox.PerformLayout();
                });
            }
        }

        private void _deviceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                _selectedDevice.ConnectionStateChanged -= _selectedDevice_ConnectionStateChanged;
                _selectedDevice.BatteryChargeChanged -= _selectedDevice_BatteryChargeChanged;
                _selectedDevice.Disconnect();
            }
            _selectedDevice = _deviceListBox.SelectedItem as DeviceAdapter;
            if (_selectedDevice != null)
            {
                _selectedDevice.ConnectionStateChanged += _selectedDevice_ConnectionStateChanged;
                _selectedDevice.BatteryChargeChanged += _selectedDevice_BatteryChargeChanged;
                _selectedDevice.Connect();
            }

            UpdateDeviceControls();
        }

        private void _selectedDevice_BatteryChargeChanged(object sender, int e)
        {
            BeginInvoke((MethodInvoker) UpdateDeviceControls);
        }

        private void _selectedDevice_ConnectionStateChanged(object sender, DeviceState e)
        {
            BeginInvoke((MethodInvoker) UpdateDeviceControls);
        }

        private void UpdateDeviceControls()
        {
            if (_selectedDevice == null)
            {
                _deviceInfoLabel.Text = string.Empty;
            }
            else
            {
                _deviceInfoLabel.Text = _selectedDevice.DeviceInfoText;
            }
        }
    }
}
