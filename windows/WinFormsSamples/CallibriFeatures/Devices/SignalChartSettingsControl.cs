using System;
using System.Windows.Forms;
using CallibriFeatures.Signal;

namespace CallibriFeatures.Devices
{
    public partial class SignalChartSettingsControl : UserControl
    {
        public SignalChartSettingsControl()
        {
            InitializeComponent();
            VerticalScanComboBox.SelectedIndexChanged += VerticalScanComboBox_SelectedIndexChanged;
            HorizontalScanComboBox.SelectedIndexChanged += HorizontalScanComboBox_SelectedIndexChanged;
        }

        private void HorizontalScanComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HorizontalScanComboBox.SelectedItem != null)
            {
                HorizontalScanChanged?.Invoke(this, (IHorizontalScan)HorizontalScanComboBox.SelectedItem);
            }
        }

        private void VerticalScanComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VerticalScanComboBox.SelectedItem != null)
            {
                VerticalScanChanged?.Invoke(this, (IVerticalScan)VerticalScanComboBox.SelectedItem);
            }
        }

        public event EventHandler<IVerticalScan> VerticalScanChanged;
        public event EventHandler<IHorizontalScan> HorizontalScanChanged;

        public IVerticalScan[] VerticalScans
        {
            set
            {
                VerticalScanComboBox.Items.Clear();
                VerticalScanComboBox.Items.AddRange(value);
                VerticalScanComboBox.SelectedItem = value[0];
            }
        }

        public IVerticalScan SelectedVerticalScan
        {
            get => (IVerticalScan) VerticalScanComboBox.SelectedItem;
            set => VerticalScanComboBox.SelectedItem = value;
        }

        public IHorizontalScan[] HorizontalScans
        {
            set
            {
                HorizontalScanComboBox.Items.Clear();
                HorizontalScanComboBox.Items.AddRange(value);
                HorizontalScanComboBox.SelectedItem = value[0];
            }
        }

        public IHorizontalScan SelectedHorizontalScan
        {
            get => (IHorizontalScan)HorizontalScanComboBox.SelectedItem;
            set => HorizontalScanComboBox.SelectedItem = value;
        }
    }
}
