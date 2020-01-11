using System;
using System.Windows.Forms;
using Neuro;

namespace CallibriFeatures.Devices
{
    public partial class SignalControl : UserControl
    {
        public SignalControl()
        {
            InitializeComponent();

            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz125);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz250);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz500);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz1000);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz2000);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz4000);
            SamplingFrequencyComboBox.Items.Add(SamplingFrequency.Hz8000);
            SamplingFrequencyComboBox.SelectedItem = SamplingFrequency.Hz125;
            
            GainComboBox.Items.Add(Gain.Gain1);
            GainComboBox.Items.Add(Gain.Gain2);
            GainComboBox.Items.Add(Gain.Gain3);
            GainComboBox.Items.Add(Gain.Gain4);
            GainComboBox.Items.Add(Gain.Gain6);
            GainComboBox.Items.Add(Gain.Gain8);
            GainComboBox.Items.Add(Gain.Gain12);
            GainComboBox.SelectedItem = Gain.Gain1;

            OffsetComboBox.Items.Add((byte)0);
            OffsetComboBox.Items.Add((byte)1);
            OffsetComboBox.Items.Add((byte)2);
            OffsetComboBox.Items.Add((byte)3);
            OffsetComboBox.Items.Add((byte)4);
            OffsetComboBox.Items.Add((byte)5);
            OffsetComboBox.Items.Add((byte)6);
            OffsetComboBox.Items.Add((byte)7);
            OffsetComboBox.Items.Add((byte)8);
            OffsetComboBox.SelectedItem = (byte)0;

            SamplingFrequencyComboBox.SelectedIndexChanged += SamplingFrequencyComboBox_SelectedIndexChanged;
            GainComboBox.SelectedIndexChanged += GainComboBox_SelectedIndexChanged;
            OffsetComboBox.SelectedIndexChanged += OffsetComboBox_SelectedIndexChanged;
            HardwareFilterCheckBox.CheckedChanged += HardwareFilterCheckBox_CheckedChanged;
        }

        private void HardwareFilterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HardwareFilterEnabledChanged?.Invoke(this, HardwareFilterCheckBox.Checked);
        }

        private void OffsetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OffsetSelectedChanged?.Invoke(this, (byte)OffsetComboBox.SelectedItem);
        }

        private void GainComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GainSelectedChanged?.Invoke(this, (Gain)GainComboBox.SelectedItem);
        }

        private void SamplingFrequencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SamplingFrequencySelectedChanged?.Invoke(this, (SamplingFrequency)SamplingFrequencyComboBox.SelectedItem);
        }

        public event EventHandler<SamplingFrequency> SamplingFrequencySelectedChanged;
        public event EventHandler<Gain> GainSelectedChanged;
        public event EventHandler<byte> OffsetSelectedChanged;
        public event EventHandler<bool> HardwareFilterEnabledChanged;

        public event EventHandler StartSignalButtonClicked
        {
            add { StartSignalButton.Click += value; }
            remove { StartSignalButton.Click -= value; }
        }
        public event EventHandler StopSignalButtonClicked
        {
            add { StopSignalButton.Click += value; }
            remove { StopSignalButton.Click -= value; }
        }

        public event EventHandler StartRespButtonClicked
        {
            add { StartRespirationButton.Click += value; }
            remove { StartRespirationButton.Click -= value; }
        }
        public event EventHandler StopRespButtonClicked
        {
            add { StopRespirationButton.Click += value; }
            remove { StopRespirationButton.Click -= value; }
        }

        public SamplingFrequency SamplingFrequencyComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { SamplingFrequencyComboBox.SelectedItem = value; });
                }
                else
                {
                    SamplingFrequencyComboBox.SelectedItem = value;
                }
            }
        }

        public Gain GainComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { GainComboBox.SelectedItem = value; });
                }
                else
                {
                    GainComboBox.SelectedItem = value;
                }
            }
        }

        public byte OffsetComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { OffsetComboBox.SelectedItem = value; });
                }
                else
                {
                    OffsetComboBox.SelectedItem = value;
                }
            }
        }

        public bool HardwareFilterEnabledValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { HardwareFilterCheckBox.Checked = value; });
                }
                else
                {
                    HardwareFilterCheckBox.Checked = value;
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Enabled = enabled; });
            }
            else
            {
                Enabled = enabled;
            }
        }

    }
}
