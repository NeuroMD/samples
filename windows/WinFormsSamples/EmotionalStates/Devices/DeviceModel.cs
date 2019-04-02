using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neuro;

namespace EmotionalStates
{
    class DeviceModel : IDisposable
    {
        private Device _currentDevice;

        public EegIndexChannel IndexChannel { get; private set; }

        public event EventHandler ChannelsChanged;

        public void SelectDevice(DeviceInfo deviceInfo)
        {
            _currentDevice?.Dispose();
            ChannelsChanged?.Invoke(this, null);
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
            var deviceChannels = _currentDevice.Channels
                .Where(x=>x.Type == ChannelType.Signal)
                .ToDictionary(channelInfo => channelInfo.Name, channelInfo => new EegChannel(_currentDevice, channelInfo));

            if (deviceChannels.ContainsKey("T3") && deviceChannels.ContainsKey("T4") &&
                deviceChannels.ContainsKey("O1") && deviceChannels.ContainsKey("O2"))
            {
                IndexChannel = new EegIndexChannel(deviceChannels["T3"], deviceChannels["T4"], deviceChannels["O1"], deviceChannels["O2"]);
                ChannelsChanged?.Invoke(this, null);
            }
        }

        public void Dispose()
        {
            _currentDevice?.Dispose();
        }

        public void StartSignal()
        {
           _currentDevice?.Execute(Command.StartSignal);
        }

        public void StopSignal()
        {
            try
            {
                _currentDevice?.Execute(Command.StopSignal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public EmotionStateChannel CreateEmotionChannel()
        {
            if (IndexChannel == null)
                throw new ArgumentNullException("Index channel is not initialized");

            return new EmotionStateChannel(IndexChannel);
        }
    }
}
