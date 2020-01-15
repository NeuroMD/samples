using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Neuro;

namespace CallibriFeatures.Devices
{
    public class DeviceTabControlPresenter
    {
        private readonly TabControl _tabControl;
        private readonly IList<DeviceTabPresenter> _deviceTabPresenters = new List<DeviceTabPresenter>();

        public DeviceTabControlPresenter(TabControl tabControl)
        {
            _tabControl = tabControl;
        }

        public IEnumerable<string> AddedDevices => _deviceTabPresenters.Select(presenter => presenter.DeviceAddress);

        public void AddDevice(Device selectedDevice)
        {
            var deviceTab = new DeviceTab();
            var deviceTabPresenter = new DeviceTabPresenter(deviceTab, selectedDevice);
            var tabText = $"{selectedDevice.ReadParam<string>(Parameter.Name)} [{selectedDevice.ReadParam<string>(Parameter.Address)}]";
            _tabControl.TabPages.Add(new TabPage{Text = tabText, Controls = { deviceTab }});
            _deviceTabPresenters.Add(deviceTabPresenter);
        }
    }
}