using System;
using System.Data;
using System.Linq;
using Neuro;

namespace DeviceInfo
{
    class DeviceAdapter
    {
        private readonly Device _device;
        private int _batteryCharge;
        private bool _needBatteryUnsubscribe = false;

        public event EventHandler<DeviceState> ConnectionStateChanged;
        public event EventHandler<int> BatteryChargeChanged;

        public DeviceAdapter(Device device)
        {
            _device = device;
            _device.ParameterChanged += _device_ParameterChanged;
            SetInfoText();
        }

        public DeviceState ConnectionState => _device.ReadParam<DeviceState>(Parameter.State);

        public string DeviceInfoText { get; private set; }

        public void Connect()
        {
            _device.Connect();
        }

        public void Disconnect()
        {
            _device.Disconnect();
        }

        public override string ToString()
        {
            var name = _device.ReadParam<string>(Parameter.Name);
            var address = _device.ReadParam<string>(Parameter.Address);
            return name + " [" + address + "] - " + (ConnectionState == DeviceState.Connected ? "Connected" : "Disconnected");
        }

        private void _device_ParameterChanged(object sender, Parameter parameter)
        {
            if (parameter == Parameter.State)
            {
                UpdateBatterySubscription();
                SetInfoText();
                ConnectionStateChanged?.Invoke(this, ConnectionState);
            }
        }

        private void UpdateBatterySubscription()
        {
            if (ConnectionState == DeviceState.Connected)
            {
                if (DeviceTraits.HasChannelsWithType(_device, ChannelType.Battery))
                {
                    _device.AddIntChannelDataListener(
                        OnBatteryChargeChanged,
                        DeviceTraits.GetChannelsWithType(_device, ChannelType.Battery)[0]
                    );
                    _needBatteryUnsubscribe = true;
                }
            }
            else if (_needBatteryUnsubscribe)
            {
                _needBatteryUnsubscribe = false;
                _device.RemoveIntChannelDataListener(OnBatteryChargeChanged);
            }
        }

        private void OnBatteryChargeChanged(object device, Device.ChannelData<int> data)
        {
            if (data.ChannelInfo.Type == ChannelType.Battery)
            {
                _batteryCharge = data.DataArray[0];
                SetInfoText();
                BatteryChargeChanged?.Invoke(this, _batteryCharge);
            }
        }

        private void SetInfoText()
        {
            var infoText = $"Name: {_device.ReadParam<string>(Parameter.Name)}\n";
            infoText += $"Address: [{_device.ReadParam<string>(Parameter.Address)}]\n";
            if (ConnectionState == DeviceState.Connected)
            {
                infoText += $"Battery charge: {_batteryCharge}%\n";
                var firmwareVersion = _device.ReadParam<FirmwareVersion>(Parameter.FirmwareVersion);
                infoText += $"Firmware version: {firmwareVersion.Version}.{firmwareVersion.Build}\n";
                infoText += "Device can execute:\n";
                foreach (var cmd in _device.Commands)
                {
                   infoText += $"    -{cmd.ToString()}\n";
                }

                infoText += "\n";

                infoText += "Device has parameters:\n";
                foreach (var paraminfo in _device.Parameters)
                {
                    infoText += $"    -{paraminfo.Parameter} {{{paraminfo.Access}}}\n";
                }

                infoText += "\n";

                infoText += "Device has channels:\n";
                foreach (var channel in _device.Channels)
                {
                    infoText += $"    -{channel.Name}\n";
                }
            }

            DeviceInfoText = infoText;
        }
    }
}
