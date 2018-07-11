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
            while (Console.ReadLine() != "\n");
        }

        private static void Scanner_ScanStateChanged(object sender, bool isScanning)
        {
            Console.WriteLine(isScanning ? "Scan started" : "Scan stopped");
        }

        private static void Scanner_DeviceFound(object sender, Device device)
        {
            Console.WriteLine("Device found!");
            device.ParameterChanged += Device_ParameterChanged;
            device.Connect();
        }

        private static void Device_ParameterChanged(object sender, Parameter parameter)
        {
            try
            {
                var device = (Device) sender;
                device.Execute(Command.FindMe);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
