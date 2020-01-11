using System;
using Neuro;

namespace CallibriFeatures.Devices
{
    public class SignalControlPresenter : IModuleControlPresenter
    {
        private readonly SignalControl _signalControl;
        private readonly Device _device;

        public SignalControlPresenter(SignalControl signalControl, Device device)
        {
            _signalControl = signalControl;
            _device = device;
            _device.ParameterChanged += _device_ParameterChanged;

            _signalControl.SamplingFrequencySelectedChanged += _signalControl_SamplingFrequencySelectedChanged;
            _signalControl.GainSelectedChanged += _signalControl_GainSelectedChanged;
            _signalControl.OffsetSelectedChanged += _signalControl_OffsetSelectedChanged;
            _signalControl.HardwareFilterEnabledChanged += _signalControl_HardwareFilterEnabledChanged;
            _signalControl.StartSignalButtonClicked += _signalControl_StartSignalButtonClicked;
            _signalControl.StopSignalButtonClicked += _signalControl_StopSignalButtonClicked;
            _signalControl.StartRespButtonClicked += _signalControl_StartRespButtonClicked;
            _signalControl.StopRespButtonClicked += _signalControl_StopRespButtonClicked;

            ChangeControlStatesForConnection(device.ReadParam<DeviceState>(Parameter.State));
        }

        private void _signalControl_StopRespButtonClicked(object sender, EventArgs e)
        {

            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StopRespiration);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StopRespiration operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _signalControl_StartRespButtonClicked(object sender, EventArgs e)
        {

            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StartRespiration);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StartRespiration operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _signalControl_StopSignalButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StopSignal);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StopSignal operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _signalControl_StartSignalButtonClicked(object sender, System.EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StartSignal);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StartSignal operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _signalControl_HardwareFilterEnabledChanged(object sender, bool isEnabled)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.HardwareFilterState, isEnabled);
            var updatedParam = _device.ReadParam<bool>(Parameter.HardwareFilterState);
            if (updatedParam != isEnabled)
            {
                _signalControl.HardwareFilterEnabledValue = updatedParam;
            }
        }

        private void _signalControl_OffsetSelectedChanged(object sender, byte offset)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.Offset, offset);
            var updatedParam = _device.ReadParam<byte>(Parameter.Offset);
            if (updatedParam != offset)
            {
                _signalControl.OffsetComboValue = updatedParam;
            }
        }

        private void _signalControl_SamplingFrequencySelectedChanged(object sender, SamplingFrequency samplingFrequency)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.SamplingFrequency, samplingFrequency);
            var updatedParam = _device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
            if (updatedParam != samplingFrequency)
            {
                _signalControl.SamplingFrequencyComboValue = updatedParam;
            }
        }

        private void _signalControl_GainSelectedChanged(object sender, Gain gain)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.Gain, gain);
            var updatedParam = _device.ReadParam<Gain>(Parameter.Gain);
            if (updatedParam != gain)
            {
                _signalControl.GainComboValue = updatedParam;
            }
        }

        public bool Enabled
        {
            get => _signalControl.Enabled;
            set => _signalControl.SetEnabled(value);
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
            _signalControl.SetEnabled(connectionState == DeviceState.Connected);
            if (connectionState == DeviceState.Connected)
            {
                _signalControl.SamplingFrequencyComboValue = _device.ReadParam<SamplingFrequency>(Parameter.SamplingFrequency);
                _signalControl.GainComboValue = _device.ReadParam<Gain>(Parameter.Gain);
                _signalControl.OffsetComboValue = _device.ReadParam<byte>(Parameter.Offset);
                _signalControl.HardwareFilterEnabledValue = _device.ReadParam<bool>(Parameter.HardwareFilterState);
            }
        }
    }
}