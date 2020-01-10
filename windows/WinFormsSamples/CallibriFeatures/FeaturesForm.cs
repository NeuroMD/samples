using System;
using System.Windows.Forms;
using CallibriFeatures.Devices;

namespace CallibriFeatures
{
    public partial class FeaturesForm : Form
    {
        private readonly DeviceTabControlPresenter _tabControlPresenter;
        public FeaturesForm()
        {
            InitializeComponent();
            _tabControlPresenter = new DeviceTabControlPresenter(DeviceTabControl);
        }

        private void AddDeviceButton_Click(object sender, EventArgs e)
        {
            try
            {
                new DeviceFinder(_tabControlPresenter).AddNewDevice(this);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot add device: {exc.Message}", "Device error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
