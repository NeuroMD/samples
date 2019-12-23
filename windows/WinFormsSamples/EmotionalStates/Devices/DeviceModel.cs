using System;
using System.Collections.Generic;
using System.Linq;
using Neuro;

namespace EmotionalStates
{
    class DeviceModel : IDisposable
    {
        private Device _currentDevice;

        public SpectrumPowerChannel AlphaLeftPowerChannel { get; private set; }
        public SpectrumPowerChannel BetaLeftPowerChannel { get; private set; }
        public SpectrumPowerChannel AlphaRightPowerChannel { get; private set; }
        public SpectrumPowerChannel BetaRightPowerChannel { get; private set; }
        public EegIndexChannel IndexChannel { get; private set; }
        public SpectrumChannel T3O1SpectrumChannel { get; private set; }
        public SpectrumChannel T4O2SpectrumChannel { get; private set; }

        public BipolarDoubleChannel T3O1SignalChannel { get; private set; }
        public BipolarDoubleChannel T4O21SignalChannel { get; private set; }

        public event EventHandler ChannelsChanged;

        public void SelectDevice(DeviceInfo deviceInfo)
        {
            _currentDevice?.Dispose();
            ChannelsChanged?.Invoke(this, null);
            _currentDevice = new DeviceEnumerator(DeviceType.Brainbit).CreateDevice(deviceInfo);
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

            if (deviceChannels.ContainsKey("T3") && deviceChannels.ContainsKey("O1"))
            {
                T3O1SignalChannel = new BipolarDoubleChannel(deviceChannels["T3"], deviceChannels["O1"]);
                T3O1SpectrumChannel = new SpectrumChannel(T3O1SignalChannel);
                if (deviceChannels.ContainsKey("T4") && deviceChannels.ContainsKey("O2"))
                {
                    T4O21SignalChannel = new BipolarDoubleChannel(deviceChannels["T4"], deviceChannels["O2"]);
                    T4O2SpectrumChannel = new SpectrumChannel(T4O21SignalChannel);
                    IndexChannel = new EegIndexChannel(deviceChannels["T3"], deviceChannels["T4"], deviceChannels["O1"],
                        deviceChannels["O2"]);
                    IndexChannel.SetWeights(1.00, 1.00, 0.00, 0.00);
                    IndexChannel.Delay = 0.0;

                    AlphaLeftPowerChannel = new SpectrumPowerChannel(new List<SpectrumChannel>{T3O1SpectrumChannel}, 8, 14, "AlphaLeft");
                    BetaLeftPowerChannel = new SpectrumPowerChannel(new List<SpectrumChannel>{T3O1SpectrumChannel}, 14, 34, "BetaLeft");
                    AlphaRightPowerChannel = new SpectrumPowerChannel(new List<SpectrumChannel>{T4O2SpectrumChannel}, 8, 14, "AlphaRight");
                    BetaRightPowerChannel = new SpectrumPowerChannel(new List<SpectrumChannel>{T4O2SpectrumChannel}, 14, 34, "BetaRight");

                    ChannelsChanged?.Invoke(this, null);
                }
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

        public void StartResist()
        {
            _currentDevice?.Execute(Command.StartResist);
        }

        public void StopResist()
        {
            _currentDevice?.Execute(Command.StopResist);
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

        public IDictionary<string, ResistanceChannel> CreateResistanceChannels()
        {
            return _currentDevice?
                .Channels?
                .Where(x => x.Type == ChannelType.Resistance)?
                .ToDictionary(channelInfo => channelInfo.Name, channelInfo => new ResistanceChannel(_currentDevice, channelInfo));
        }
    }
}
