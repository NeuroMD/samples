using System;
using System.Threading.Tasks;
using Neuro;

namespace CallibriFeatures.Devices
{
    public class DeviceInfoPresenter : IModuleControlPresenter
    {
        private readonly DeviceInfoControl _infoControl;
        private readonly BatteryChannel _batteryChannel;
        private readonly Device _device;

        public DeviceInfoPresenter(DeviceInfoControl infoControl, Device device)
        {
            _infoControl = infoControl ?? throw new ArgumentNullException(nameof(infoControl), "infoControl tab control cannot be null");
            _device = device ?? throw new ArgumentNullException(nameof(device), "Device cannot be null"); ;

            _infoControl.FindMeClicked += _infoControl_FindMeClicked;
            _infoControl.ConnectDisconnectClicked += _infoControl_ConnectDisconnectClicked; ;
            _infoControl.NameText = _device.ReadParam<string>(Parameter.Name);
            _infoControl.AddressText = _device.ReadParam<string>(Parameter.Address);
            UpdateConnectionDependent(_device.ReadParam<DeviceState>(Parameter.State));

            _device.ParameterChanged += _device_ParameterChanged;

            _batteryChannel = new BatteryChannel(device);
            _batteryChannel.LengthChanged += _batteryChannel_LengthChanged;
        }

        private void _batteryChannel_LengthChanged(object sender, int length)
        {
            if (length == 0)
                return;

            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
            {
                _infoControl.BatteryChargeText = "N/A";
                return;
            }

            var charge = _batteryChannel.ReadData(length - 1, 1)[0];
            _infoControl.BatteryChargeText = $"{charge.ToString()}%";
        }

        private void _infoControl_ConnectDisconnectClicked(object sender, EventArgs e)
        {
            try
            {
                if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                {
                    Task.Run(()=>{ _device.Connect(); });
                }
                else
                {
                    Task.Run(() => { _device.Disconnect(); });
                }
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot change connection status for device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _infoControl_FindMeClicked(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() => { _device.Execute(Command.FindMe); });
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform FindME operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _device_ParameterChanged(object sender, Parameter parameter)
        {
            if (parameter == Parameter.State)
            {
                UpdateConnectionDependent(_device.ReadParam<DeviceState>(Parameter.State));
            }
        }

        private void UpdateConnectionDependent(DeviceState state)
        {
            _infoControl.ConnectionStateText = state.ToString();
            if (state == DeviceState.Connected)
            {
                _infoControl.SerialNumberText = _device.ReadParam<string>(Parameter.SerialNumber);
                _infoControl.ConnectDisconnectText = "Disconnect";
                _infoControl.FindMeEnabled = true;
            }
            else
            {
                _infoControl.SerialNumberText = "N/A";
                _infoControl.BatteryChargeText = "N/A";
                _infoControl.ConnectDisconnectText = "Connect";
                _infoControl.FindMeEnabled = false;
            }
        }

        public bool Enabled
        {
            get => _infoControl?.Enabled ?? false;
            set => _infoControl.Enabled = value;
        }
    }
}