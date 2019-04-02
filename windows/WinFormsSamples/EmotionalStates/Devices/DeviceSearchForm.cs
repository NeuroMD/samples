using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neuro;

namespace EmotionalStates.Devices
{
    public partial class DeviceSearchForm : Form
    {
        private DeviceEnumerator _enumerator;

        public DeviceWrapper SelectedDevice { get; private set; }
        
        public DeviceSearchForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Task.Run(() => {
                _enumerator = new DeviceEnumerator(DeviceType.Brainbit);
                _enumerator.DeviceListChanged += (sender, args) =>
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        _deviceListBox.Items.Clear();
                        _deviceListBox.Items.AddRange(_enumerator.Devices.Select(x => new DeviceWrapper(x))
                            .ToArray<object>());
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
            SelectedDevice = _deviceListBox.SelectedItem as DeviceWrapper;
        }

        private void _deviceListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_deviceListBox.SelectedIndex >= 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
