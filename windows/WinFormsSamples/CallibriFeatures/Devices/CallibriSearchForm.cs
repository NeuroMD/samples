using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neuro;

namespace CallibriFeatures.Devices
{
    public partial class CallibriSearchForm : Form
    {
        private DeviceEnumerator _enumerator;
        private DeviceWrapper _selectedDeviceInfo;

        public Device SelectedDevice { get; private set; }
        
        public CallibriSearchForm(IReadOnlyCollection<string> exclude)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Task.Run(() => {
                _enumerator = new DeviceEnumerator(DeviceType.Callibri);
                _enumerator.DeviceListChanged += (sender, args) =>
                {
                    if (!IsHandleCreated)
                        return;

                    BeginInvoke((MethodInvoker)delegate
                    {
                        var foundDevices = _enumerator.Devices
                            .Where(info => !exclude.Contains(new DeviceWrapper(info).AddressString))
                            .ToArray();

                        var disappearedDevices = _deviceListBox.Items
                            .OfType<DeviceWrapper>()
                            .Select(x=>(DeviceInfo)x)
                            .Except(foundDevices)
                            .ToArray();
                        foreach (var disappearedDevice in disappearedDevices)
                        {
                            _deviceListBox.Items.Remove(disappearedDevice);
                        }

                        var newDevices = foundDevices
                            .Except(_deviceListBox.Items
                                .OfType<DeviceWrapper>()
                                .Select(x => (DeviceInfo)x))
                            .Select(info=>new DeviceWrapper(info))
                            .ToArray();
                        if (newDevices.Length > 0)
                        {
                            _deviceListBox.Items.AddRange(newDevices);
                        }
                    });
                };
            });
        }

        private void DeviceSearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _enumerator?.Dispose();
        }

        private void _deviceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedDeviceInfo = _deviceListBox.SelectedItem as DeviceWrapper;
        }

        private void _deviceListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_deviceListBox.SelectedIndex >= 0)
            {
                if (_selectedDeviceInfo != null)
                {
                    SelectedDevice = _enumerator.CreateDevice(_selectedDeviceInfo);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
