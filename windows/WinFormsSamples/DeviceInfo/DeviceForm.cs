using System;
using System.Windows.Forms;
using Neuro;

namespace DeviceInfo
{
    public partial class DeviceForm : Form
    {
        private readonly DeviceScanner _scanner;

        private DeviceAdapter _selectedDevice;

        public DeviceForm()
        {
            InitializeComponent();
            _scanner = new DeviceScanner();
            _scanner.ScanStateChanged += _scanner_ScanStateChanged;
            _scanner.DeviceFound += _scanner_DeviceFound;
            _startScanButton.Enabled = true;
        }

        private void _scanner_DeviceFound(object sender, Device device)
        {
            var deviceAdapter = new DeviceAdapter(device);
            Invoke((MethodInvoker)delegate { _deviceListBox.Items.Add(deviceAdapter); });
            deviceAdapter.ConnectionStateChanged += (o, state) => Invoke((MethodInvoker) delegate
            {
                _deviceListBox.PerformLayout();
            });
        }

        private void _scanner_ScanStateChanged(object sender, bool isScanning)
        {
            Invoke((MethodInvoker) delegate
            {
                _startScanButton.Enabled = !_scanner.IsScanning;
                _stopScanButton.Enabled = _scanner.IsScanning;
                progressBar1.Visible = _scanner.IsScanning;
            });
        }

        private void _startScanButton_Click(object sender, EventArgs e)
        {
            _deviceListBox.Items.Clear();
            _selectedDevice = null;
            GC.Collect();
            UpdateDeviceControls();
            _scanner.StartScan();
        }

        private void _stopScanButton_Click(object sender, EventArgs e)
        {
            _scanner.StopScan();
        }

        private void _connectButton_Click(object sender, EventArgs e)
        {
            _connectButton.Enabled = false;
            _selectedDevice.Connect();
        }

        private void _disconnectButton_Click(object sender, EventArgs e)
        {
            _disconnectButton.Enabled = false;
            _selectedDevice.Disconnect();
        }

        private void _deviceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                _selectedDevice.ConnectionStateChanged -= _selectedDevice_ConnectionStateChanged;
                _selectedDevice.BatteryChargeChanged -= _selectedDevice_BatteryChargeChanged;
            }
            _selectedDevice = _deviceListBox.SelectedItem as DeviceAdapter;
            if (_selectedDevice != null)
            {
                _selectedDevice.ConnectionStateChanged += _selectedDevice_ConnectionStateChanged;
                _selectedDevice.BatteryChargeChanged += _selectedDevice_BatteryChargeChanged;
            }

            UpdateDeviceControls();
        }

        private void _selectedDevice_BatteryChargeChanged(object sender, int e)
        {
            Invoke((MethodInvoker) UpdateDeviceControls);
        }

        private void _selectedDevice_ConnectionStateChanged(object sender, DeviceState e)
        {
            Invoke((MethodInvoker) UpdateDeviceControls);
        }

        private void UpdateDeviceControls()
        {
            if (_selectedDevice == null)
            {
                _deviceInfoLabel.Text = string.Empty;
                _connectButton.Enabled = false;
                _disconnectButton.Enabled = false;
            }
            else
            {
                _deviceInfoLabel.Text = _selectedDevice.DeviceInfoText;
                _connectButton.Enabled = _selectedDevice.ConnectionState == DeviceState.Disconnected;
                _disconnectButton.Enabled = _selectedDevice.ConnectionState == DeviceState.Connected;
            }
        }
    }
}
