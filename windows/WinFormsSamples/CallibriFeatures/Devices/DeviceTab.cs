using System.Windows.Forms;

namespace CallibriFeatures.Devices
{
    public partial class DeviceTab : UserControl
    {
        public DeviceTab()
        {
            InitializeComponent();
        }

        public DeviceInfoControl InfoControl => DeviceInfoControl;
    }
}
