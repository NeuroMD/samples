using System;
using System.Linq;
using System.Windows.Forms;

namespace CallibriFeatures.Devices
{
    public class DeviceFinder
    {
        private readonly DeviceTabControlPresenter _tabControlPresenter;

        public DeviceFinder(DeviceTabControlPresenter tabControlPresenter)
        {
            _tabControlPresenter = tabControlPresenter;
        }

        public void AddNewDevice(IWin32Window control)
        {
            var deviceSearchForm = new CallibriSearchForm(_tabControlPresenter.AddedDevices.ToArray());
            if (deviceSearchForm.ShowDialog(control) == DialogResult.OK)
            {
                if (deviceSearchForm.SelectedDevice != null)
                {
                    _tabControlPresenter.AddDevice(deviceSearchForm.SelectedDevice);
                }
            }
        }
    }
}