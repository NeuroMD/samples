using System;
using System.Windows.Forms;

namespace CallibriFeatures.Devices
{
    public partial class DeviceInfoControl : UserControl
    {
        public DeviceInfoControl()
        {
            InitializeComponent();
            FindMeButton.Click += (sender, args) =>
            {
                try
                {
                    FindMeClicked?.Invoke(this, null);
                }
                catch (Exception e)
                {
                    MessageBox.Show
                    ($"Cannot execute FindMe command: {e.Message}", 
                        "FindMe error", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            };
            ConnectDisconnectButton.Click += (sender, args) =>
            {
                try
                {
                    ConnectDisconnectClicked?.Invoke(this, null);
                }
                catch (Exception e)
                {
                    MessageBox.Show
                    ($"Cannot change connection status: {e.Message}",
                        "Connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            };
        }

        public event EventHandler FindMeClicked;
        public event EventHandler ConnectDisconnectClicked;

        public string NameText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker) delegate { NameLabel.Text = $"Name: {value}"; });
                }
                else
                {
                    NameLabel.Text = $"Name: {value}";
                }
            }

        }

        public string SerialNumberText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { SerialNumberLabel.Text = $"S/N: {value}"; });
                }
                else
                {
                    SerialNumberLabel.Text = $"S/N: {value}";
                }
            }
        }

        public string AddressText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { AddressLabel.Text = $"Address: {value}"; });
                }
                else
                {
                    AddressLabel.Text = $"Address: {value}";
                }
            }
        }

        public string ConnectionStateText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { ConncetionStateLabel.Text = $"Connection state: {value}"; });
                }
                else
                {
                    ConncetionStateLabel.Text = $"Connection state: {value}";
                }
            }
        }

        public string BatteryChargeText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { BatteryLabel.Text = $"Battery: {value}"; });
                }
                else
                {
                    BatteryLabel.Text = $"Battery: {value}";
                }
            }
        }

        public string FirmwareVersionText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { FirmwareVersionLabel.Text = $"Firmware version: {value}"; });
                }
                else
                {
                    FirmwareVersionLabel.Text = $"Firmware version: {value}";
                }
            }
        }

        public bool FindMeEnabled
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { FindMeButton.Enabled = value; });
                }
                else
                {
                    FindMeButton.Enabled = value;
                }
            }
        }

        public string ConnectDisconnectText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { ConnectDisconnectButton.Text = value; });
                }
                else
                {
                    ConnectDisconnectButton.Text = value;
                }
            }
        }
    }
}
