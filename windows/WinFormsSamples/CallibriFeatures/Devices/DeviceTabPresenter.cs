using System.Collections.Generic;
using Neuro;

namespace CallibriFeatures.Devices
{
    class DeviceTabPresenter
    {
        private readonly IList<IModuleControlPresenter> _modulePresenters = new List<IModuleControlPresenter>();

        public string DeviceAddress { get; }

        public DeviceTabPresenter(DeviceTab tabControl, Device device)
        {
            DeviceAddress = device.ReadParam<string>(Parameter.Address);
            var deviceInfoPresenter = new DeviceInfoPresenter(tabControl.InfoControl, device);
            _modulePresenters.Add(deviceInfoPresenter);
        }
    }
}
