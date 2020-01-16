using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

            var deviceSearchForm = new DeviceSearchForm(new List<string>());
            if (deviceSearchForm.ShowDialog() == DialogResult.OK)
            {
                OnDeviceFound(deviceSearchForm.SelectedDevice);
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

        private void OnDeviceFound(Device device)
        {
            if (Device != null)
            {
                return;
            }

            Device = device;
            var state = Device.ReadParam<DeviceState>(Parameter.State);
            if (state == DeviceState.Disconnected)
            {
                Device.ParameterChanged += OnDeviceParamChanged;
                Device.Connect();
            }
            else
            {
                OnDeviceStateChanged(state);
            }
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
