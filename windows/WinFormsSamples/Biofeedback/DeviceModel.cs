using System;
using Neuro;

namespace Biofeedback
{
    class DeviceModel
    {
        private readonly DeviceScanner _scanner;

        public Device Device { get; private set; }
        public event EventHandler<Device> DeviceFound;
        public event EventHandler DeviceLost;
        public event EventHandler<bool> SearchStateChanged;

        public DeviceModel()
        {
            _scanner = new DeviceScanner();
            _scanner.ScanStateChanged += _scanner_ScanStateChanged;
            _scanner.DeviceFound += _scanner_DeviceFound;
        }

        public void Reconnect()
        {
            _scanner.StopScan();
            if (Device != null)
            {
                DeviceLost?.Invoke(this, null);
                Device.ParameterChanged -= OnDeviceParamChanged;
                Device.Dispose();
                Device = null;
            }
            _scanner.StartScan();
        }

        private void _scanner_ScanStateChanged(object sender, bool isScanning)
        {
            SearchStateChanged?.Invoke(this, isScanning);
        }

        private void OnDeviceStateChanged(DeviceState state)
        {
            if (state == DeviceState.Connected)
            {
                DeviceFound?.Invoke(this, Device);
            }
            else
            {
                DeviceLost?.Invoke(this, null);
            }
        }

        private void OnDeviceParamChanged(object sender, Parameter parameter)
        {
            if (parameter == Parameter.State)
            {
                var state = Device.ReadParam<DeviceState>(Parameter.State);
                OnDeviceStateChanged(state);
            }
        }

        private void _scanner_DeviceFound(object sender, Device device)
        {
            _scanner.StopScan();
            if (Device != null)
            {
                return;
            }

            Device = device;
            Device.ParameterChanged += OnDeviceParamChanged;
            Device.Connect();
        }
    }
}
