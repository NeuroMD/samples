using System.Windows.Forms;
using CallibriFeatures.GraphicsControl;

namespace CallibriFeatures.Devices
{
    public partial class DeviceTab : UserControl
    {
        public DeviceTab()
        {
            InitializeComponent();
        }

        public DeviceInfoControl InfoControl => DeviceInfoControl;
        public ElectrodesControl ElectrodesControl => ElectrodesControlPanel;
        public SignalControl SignalControl => SignalControlPanel;
        public MEMSControl MemsControl => MemsControlPanel;
        public SignalChartSettingsControl SignalSettingsControl => SignalChartSettingsControl;
        public DrawableControl SignalPlotControl => SignalPlotDrawableControl;
    }
}
