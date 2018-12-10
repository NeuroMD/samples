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
        private static readonly Dictionary<string, SignalChannel> _channels = new Dictionary<string, SignalChannel>();

        static void Main(string[] args)
        {
            var scanner = new DeviceScanner();
            scanner.DeviceFound += Scanner_DeviceFound;
            scanner.StartScan();
            Console.ReadLine();
        }

        private static void Scanner_DeviceFound(object sender, Device device)
        {
            (sender as DeviceScanner)?.StopScan();

            DeviceName = device.ReadParam<string>(Parameter.Name);
            DeviceAddress = device.ReadParam<string>(Parameter.Address);
            ConnectionState = device.ReadParam<DeviceState>(Parameter.State);
            Console.WriteLine($"Found device {DeviceName} [{DeviceAddress}], state: {ConnectionState}");

            device.ParameterChanged += Device_ParameterChanged;
            ConnectionState = device.ReadParam<DeviceState>(Parameter.State);
            if (ConnectionState != DeviceState.Connected)
            {
                device.Connect();
            }
            else
            {
                OnDeviceConnected(device);
            }
        }

        private static void OnDeviceConnected(Device device)
        {
            Console.WriteLine("Device connected");
            Frequency = device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
            Rewrite();
            _batteryChannel = new BatteryChannel(device);
            _batteryChannel.LengthChanged += BatteryChannel_LengthChanged;
            if (Frequency != SamplingFrequency.Hz250)
            {
                device.SetParam(Parameter.SamplingFrequency, SamplingFrequency.Hz250);
                Frequency = device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
            }
            var channelInfos = device.Channels;
            foreach (var channelInfo in channelInfos)
            {
                if (channelInfo.Type != ChannelType.Signal)
                    continue;

                var signalChannel = new SignalChannel(device, channelInfo, new[] { Filter.LowPass_30Hz_SF250, Filter.HighPass_2Hz_SF250 });
                _channels[signalChannel.Info.Name] = signalChannel;
                signalChannel.LengthChanged += SignalChannel_LengthChanged;
            }
            device.Execute(Command.StartSignal);
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

        private static void Device_ParameterChanged(object sender, Parameter parameter)
        {
            if (parameter != Parameter.State)
                return;

            if (!(sender is Device))
                return;

            var device = (Device) sender;
            ConnectionState = device.ReadParam<DeviceState>(Parameter.State);
            if (ConnectionState == DeviceState.Connected)
            {
                OnDeviceConnected(device);
            }
            else
            {
                Rewrite();
            }
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
