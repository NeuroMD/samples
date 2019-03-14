using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Resources;
using System.Runtime.Remoting.Channels;
using Neuro;

namespace ChannelData
{
    class Program
    {
        private static object lockable = new object();
        private static string DeviceName { get; set; }
        private static string DeviceAddress { get; set; }
        private static int BatteryLevel { get; set; }
        private static DeviceState ConnectionState { get; set; }
        private static SamplingFrequency Frequency { get; set; }
        private static BatteryChannel _batteryChannel;
        private static Device _device;
        private static readonly Dictionary<string, SignalChannel> _channels = new Dictionary<string, SignalChannel>();

        static void Main(string[] args)
        {
            using (var enumerator = new DeviceEnumerator(DeviceType.Any))
            {
                enumerator.DeviceListChanged += Enumerator_DeviceListChanged;
                Console.ReadLine();
            }

            _device?.Dispose();
        }

        private static void Enumerator_DeviceListChanged(object sender, EventArgs e)
        {
            if (sender is DeviceEnumerator enumerator)
            {
                foreach (var device in enumerator.Devices)
                {
                    OnDeviceFound(device);
                }
            }
        }

        private static void OnDeviceFound(DeviceInfo deviceInfo)
        {
            if (_device != null)
                return;

            _device = new Device(deviceInfo);
            {
               DeviceName = _device.ReadParam<string>(Parameter.Name);
               DeviceAddress = _device.ReadParam<string>(Parameter.Address);
                var deviceState = _device.ReadParam<DeviceState>(Parameter.State);

                Console.WriteLine($"Found device {DeviceName} [{DeviceAddress}], state: {deviceState}");

                try
                {
                    _device.Connect();
                    while (_device.ReadParam<DeviceState>(Parameter.State) != DeviceState.Connected)
                    {
                    }
                    OnDeviceConnected();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    _device.Dispose();
                    _device = null;
                }
            }
        }

        private static void OnDeviceConnected()
        {
            Console.WriteLine("Device connected");
            Frequency = _device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
            Rewrite();
            _batteryChannel = new BatteryChannel(_device);
            _batteryChannel.LengthChanged += BatteryChannel_LengthChanged;
            if (Frequency != SamplingFrequency.Hz250)
            {
                _device.SetParam(Parameter.SamplingFrequency, SamplingFrequency.Hz250);
                Frequency = _device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
            }
            var channelInfos = _device.Channels;
            foreach (var channelInfo in channelInfos)
            {
                if (channelInfo.Type != ChannelType.Signal)
                    continue;

                var signalChannel = new SignalChannel(_device, channelInfo, new[] { Filter.LowPass_30Hz_SF250, Filter.HighPass_2Hz_SF250 });
                _channels[signalChannel.Info.Name] = signalChannel;
                signalChannel.LengthChanged += SignalChannel_LengthChanged;
            }
            _device.Execute(Command.StartSignal);
        }

        private static void SignalChannel_LengthChanged(object sender, int length)
        {
            Rewrite();
        }

        private static void BatteryChannel_LengthChanged(object sender, int length)
        {
            BatteryLevel = (sender as BatteryChannel)?.ReadData(length - 1, 1)[0] ?? 0;
            Rewrite();
        }

        private static void Rewrite()
        {
            lock (lockable)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"{DeviceName} [{DeviceAddress}] State: {ConnectionState} Battery: {BatteryLevel} Frequency: {Frequency}");
                foreach (var channelName in _channels.Keys)
                {
                    Console.Write($" Duration of {channelName}: {_channels[channelName].TotalLength / 250.0:0.00}");
                }
            }
            
        }
    }
}
