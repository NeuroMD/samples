using Neuro;

namespace CallibriFeatures.Devices
{
    public class ElectrodesControlPresenter : IModuleControlPresenter
    {
        private readonly ElectrodesControl _electrodesControl;
        private readonly Device _device;
        private readonly ElectrodeStateChannel _electrodeStateChannel;

        public ElectrodesControlPresenter(ElectrodesControl electrodesControl, Device device)
        {
            _electrodesControl = electrodesControl;
            _device = device;
            _electrodeStateChannel = new ElectrodeStateChannel(_device);

            _device.ParameterChanged += _device_ParameterChanged;
            _electrodesControl.ExternalSwitchComboSelectedChanged += _electrodesControl_ExternalSwitchComboSelectedChanged;
            _electrodesControl.ADCComboSelectedChanged += _electrodesControl_ADCComboSelectedChanged;
            _electrodeStateChannel.LengthChanged += _electrodeStateChannel_LengthChanged;

            ChangeControlStatesForConnection(device.ReadParam<DeviceState>(Parameter.State));
        }

        private void _electrodeStateChannel_LengthChanged(object sender, int length)
        {
            if (length <= 0)
                return;

            var electrodeState = _electrodeStateChannel.ReadData(length - 1, 1)[0];
            _electrodesControl.ElectrodeStateText = electrodeState.ToString();
        }

        private void _electrodesControl_ADCComboSelectedChanged(object sender, ADCInput adcInput)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.ADCInputState, adcInput);

            var updatedParam = _device.ReadParam<ADCInput>(Parameter.ADCInputState);
            if (updatedParam != adcInput)
            {
                _electrodesControl.ADCStateComboValue = updatedParam;
            }

            _electrodesControl.HintVisible = updatedParam != ADCInput.Resistance;
        }

        private void _electrodesControl_ExternalSwitchComboSelectedChanged(object sender, ExternalSwitchInput selectedInput)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.ExternalSwitchState, selectedInput);

            var updatedParam = _device.ReadParam<ExternalSwitchInput>(Parameter.ExternalSwitchState);
            if (updatedParam != selectedInput)
            {
                _electrodesControl.SwitchStateComboValue = updatedParam;
            }
        }

        private void _device_ParameterChanged(object sender, Parameter param)
        {
            if (param == Parameter.State)
            {
                ChangeControlStatesForConnection(_device.ReadParam<DeviceState>(Parameter.State));
            }
        }

        private void ChangeControlStatesForConnection(DeviceState connectionState)
        {
            _electrodesControl.SetEnabled(connectionState == DeviceState.Connected);
            if (connectionState == DeviceState.Connected)
            {
                _electrodesControl.SwitchStateComboValue = _device.ReadParam<ExternalSwitchInput>(Parameter.ExternalSwitchState);
                var adcState = _device.ReadParam<ADCInput>(Parameter.ADCInputState);
                _electrodesControl.ADCStateComboValue = adcState;
                _electrodesControl.HintVisible = adcState != ADCInput.Resistance;
            }
        }

        public bool Enabled
        {
            get => _electrodesControl.Enabled;
            set => _electrodesControl.SetEnabled(value);
        }
    }
}