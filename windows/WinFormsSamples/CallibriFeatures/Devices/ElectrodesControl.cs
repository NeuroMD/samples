using System;
using System.Windows.Forms;
using Neuro;

namespace CallibriFeatures.Devices
{
    public partial class ElectrodesControl : UserControl
    {
        public ElectrodesControl()
        {
            InitializeComponent();
            ExternalSwitchComboBox.Items.Add(ExternalSwitchInput.MioElectrodes);
            ExternalSwitchComboBox.Items.Add(ExternalSwitchInput.MioUSB);
            ExternalSwitchComboBox.Items.Add(ExternalSwitchInput.MioElectrodesRespUSB);
            ExternalSwitchComboBox.Items.Add(ExternalSwitchInput.RespUSB);
            ExternalSwitchComboBox.SelectedItem = ExternalSwitchInput.RespUSB;

            ADCComboBox.Items.Add(ADCInput.Electrodes);
            ADCComboBox.Items.Add(ADCInput.Resistance);
            ADCComboBox.Items.Add(ADCInput.Short);
            ADCComboBox.Items.Add(ADCInput.Test);
            ADCComboBox.SelectedItem = ADCInput.Test;

            ExternalSwitchComboBox.SelectedIndexChanged += ExternalSwitchComboBox_SelectedIndexChanged;
            ADCComboBox.SelectedIndexChanged += ADCComboBox_SelectedIndexChanged;
        }

        private void ADCComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ADCComboBox.SelectedIndex < 0)
                return;

            ADCComboSelectedChanged?.Invoke(sender, (ADCInput)ADCComboBox.SelectedItem);
        }

        private void ExternalSwitchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExternalSwitchComboBox.SelectedIndex < 0)
                return;

            ExternalSwitchComboSelectedChanged?.Invoke(sender, (ExternalSwitchInput)ExternalSwitchComboBox.SelectedItem);
        }

        public event EventHandler<ExternalSwitchInput> ExternalSwitchComboSelectedChanged;
        public event EventHandler<ADCInput> ADCComboSelectedChanged;

        public ExternalSwitchInput SwitchStateComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { ExternalSwitchComboBox.SelectedItem = value; });
                }
                else
                {
                    ExternalSwitchComboBox.SelectedItem = value;
                }
            }
        }
        
        public ADCInput ADCStateComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker) delegate
                    {
                        ADCComboBox.SelectedItem = value;
                    });
                }
                else
                {
                    ADCComboBox.SelectedItem = value;
                }
            }
        }

        public string ElectrodeStateText
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { ElectrodesAttachStateLabel.Text = $"Electrodes state: {value}"; });
                }
                else
                {
                    ElectrodesAttachStateLabel.Text = $"Electrodes state: {value}";
                }
            }
        }

        public bool HintVisible
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { ElectrodeStateHintLabel.Visible = value; });
                }
                else
                {
                    ElectrodeStateHintLabel.Visible = value;
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate { Enabled = enabled; });
            }
            else
            {
                Enabled = enabled;
            }
        }
    }
}
