using System;
using Neuro;

namespace DeviceInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            var scanner = new DeviceScanner();
            scanner.DeviceFound += Scanner_DeviceFound;
            scanner.ScanStateChanged += Scanner_ScanStateChanged;
            scanner.StartScan();
            Console.ReadLine();
        }

        private static void OnDeviceConnected(Device device)
        {
            Console.WriteLine();
            Console.WriteLine("Device can execute:");
            foreach(var cmd in device.Commands)
            {
                Console.WriteLine($"    -{cmd.ToString()}");
            }
            Console.WriteLine();

            Console.WriteLine("Device has parameters:");
            foreach (var paraminfo in device.Parameters)
            {
                Console.WriteLine($"    -{paraminfo.Parameter} {{{paraminfo.Access}}}");
            }
            Console.WriteLine();

            Console.WriteLine("Device has channels:");
            foreach (var channel in device.Channels)
            {
                Console.WriteLine($"    -{channel.Name}");
            }
            Console.WriteLine();
        }

        private static void Scanner_ScanStateChanged(object sender, bool isScanning)
        {
            Console.WriteLine(isScanning ? "Scan started" : "Scan stopped");
        }

        private static void Scanner_DeviceFound(object sender, Device device)
        {
            var deviceName = device.ReadParam<string>(Parameter.Name);
            var deviceAddress = device.ReadParam<string>(Parameter.Address);
            var deviceState = device.ReadParam<DeviceState>(Parameter.State);

            Console.WriteLine($"Found device {deviceName} [{deviceAddress}], state: {deviceState}");

            device.ParameterChanged += Device_ParameterChanged;
            var state = device.ReadParam<DeviceState>(Parameter.State);
            if (state == DeviceState.Connected)
            {
                OnDeviceConnected(device);
            }
            else
            {
                device.Connect();
            }
        }

        private static void Device_ParameterChanged(object sender, Parameter parameter)
        {
            try
            {
                if (parameter == Parameter.State)
                {
                    var device = (Device)sender;
                    var state = device.ReadParam<DeviceState>(Parameter.State);
                    if (state == DeviceState.Connected)
                    {
                        OnDeviceConnected(device);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
