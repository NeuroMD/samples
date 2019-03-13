using System;
using Neuro;

namespace DeviceInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var enumerator = new DeviceEnumerator(DeviceType.Any))
            {
                enumerator.DeviceListChanged += Enumerator_DeviceListChanged;
                Console.ReadLine();
            }
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

        private static void ShowDeviceFeatures(Device device)
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

        private static void OnDeviceFound(Neuro.DeviceInfo deviceInfo)
        {
            using (var device = new Device(deviceInfo))
            {
                var deviceName = device.ReadParam<string>(Parameter.Name);
                var deviceAddress = device.ReadParam<string>(Parameter.Address);
                var deviceState = device.ReadParam<DeviceState>(Parameter.State);

                Console.WriteLine($"Found device {deviceName} [{deviceAddress}], state: {deviceState}");

                device.Connect();
                while (device.ReadParam<DeviceState>(Parameter.State) != DeviceState.Connected)
                {
                }

                device.Disconnect();
            }
        }
    }
}
