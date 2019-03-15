using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neuro;

namespace Indices
{
    class DeviceModel : IDisposable
    {
        private readonly DeviceEnumerator _enumerator;
        private readonly IList<IDataChannel<double>> _deviceChannels = new List<IDataChannel<double>>();
        private Device _currentDevice;

        public IList<IDataChannel<double>> DeviceChannels => _deviceChannels;
        public IList<DeviceInfo> Devices => _enumerator.Devices;
        public event EventHandler DeviceListChanged;
        public event EventHandler ChannelListChanged;

        public DeviceModel()
        {
            _enumerator = new DeviceEnumerator(DeviceType.Any);
            _enumerator.DeviceListChanged += (sender, args) =>{ DeviceListChanged?.Invoke(this, null);};
        }

        public void SelectDevice(DeviceInfo deviceInfo)
        {
            _currentDevice?.Dispose();
            _deviceChannels.Clear();
            ChannelListChanged?.Invoke(this, null);
            _currentDevice = new Device(deviceInfo);
            _currentDevice.Connect();
            if (_currentDevice.ReadParam<DeviceState>(Parameter.State) != DeviceState.Connected)
            {
                _currentDevice.ParameterChanged += (sender, parameter) =>
                {
                    if (parameter == Parameter.State)
                    {
                        if (_currentDevice.ReadParam<DeviceState>(Parameter.State) == DeviceState.Connected)
                        {
                            OnDeviceConnected();
                        }
                    }
                };
            }
            else
            {
                OnDeviceConnected();
            }
        }

        private void OnDeviceConnected(){
            var deviceChannels = _currentDevice.Channels;
            foreach (var deviceChannel in deviceChannels)
            {
                if (deviceChannel.Type == ChannelType.Signal)
                {
                    _deviceChannels.Add(new SignalChannel(_currentDevice, deviceChannel));
                }
            }
            ChannelListChanged?.Invoke(this, null);
        }

        public void Dispose()
        {
            _enumerator?.Dispose();
            _currentDevice?.Dispose();
        }

        public void StartSignal()
        {
            _currentDevice?.Execute(Command.StartSignal);
        }

        public void StopSignal()
        {
            _currentDevice?.Execute(Command.StopSignal);
        }
    }
}
