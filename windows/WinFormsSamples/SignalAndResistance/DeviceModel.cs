using System;
using Neuro;

namespace SignalAndResistance
{
    class DeviceModel : IDisposable
    {
        private readonly DeviceEnumerator _enumerator;

        public Device Device { get; private set; }
        public event EventHandler<Device> DeviceFound;
        public event EventHandler DeviceLost;

        public DeviceModel()
        {
            _enumerator = new DeviceEnumerator(DeviceType.Any);
            _enumerator.DeviceListChanged += _enumerator_DeviceListChanged;
        }

        private void _enumerator_DeviceListChanged(object sender, EventArgs e)
        {
            var devices = _enumerator.Devices;
            if (devices.Count > 0)
            {
                OnDeviceFound(devices[0]);
            }
        }

        public void Reconnect()
        {
            if (Device != null)
            {
                DeviceLost?.Invoke(this, null);
                Device.ParameterChanged -= OnDeviceParamChanged;
                try
                {
                    Device.Execute(Command.StopSignal);
                    Device.Disconnect();
                }
                catch { }
                Device.Dispose();
                Device = null;
            }
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

        private void OnDeviceFound(DeviceInfo deviceInfo)
        {
            if (Device != null)
            {
                return;
            }

            Device = _enumerator.CreateDevice(deviceInfo);
            Device.ParameterChanged += OnDeviceParamChanged;
            Device.Connect();
        }

        public void Dispose()
        {
            _enumerator?.Dispose();
            if (Device != null)
            {
                Device.ParameterChanged -= OnDeviceParamChanged;
                try
                {
                    Device.Execute(Command.StopSignal);
                    Device.Disconnect();
                }
                catch { }
                Device.Dispose();
            }
        }
    }
}
