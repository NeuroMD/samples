using System;
using System.Threading;
using System.Threading.Tasks;
using Neuro;

namespace CallibriFeatures.Devices
{
    public class MemsControlPresenter : IModuleControlPresenter
    {
        private readonly MEMSControl _memsControl;
        private readonly Device _device;

        public MemsControlPresenter(MEMSControl memsControl, Device device)
        {
            _memsControl = memsControl;
            _device = device;
            _device.ParameterChanged += _device_ParameterChanged;

            _memsControl.SelectedGyroscopeSensChanged += _memsControl_SelectedGyroscopeSensChanged;
            _memsControl.SelectedAccelerometerSensChanged += _memsControl_SelectedAccelerometerSensChanged;
            _memsControl.StartMemsButtonClicked += _memsControl_StartMemsButtonClicked;
            _memsControl.StopMemsButtonClicked += _memsControl_StopMemsButtonClicked;
            _memsControl.StartOrientButtonClicked += _memsControl_StartOrientButtonClicked;
            _memsControl.StopOrientButtonClicked += _memsControl_StopOrientButtonClicked;
            _memsControl.ResetQuaternionButtonClicked += _memsControl_ResetQuaternionButtonClicked;
            _memsControl.CalibrateMemsButtonClicked += _memsControl_CalibrateMemsButtonClicked;
            _memsControl.CalibrationProgressBarVisible = false;

            ChangeControlStatesForConnection(device.ReadParam<DeviceState>(Parameter.State));
        }

        private void _memsControl_CalibrateMemsButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.CalibrateMEMS);
                _memsControl.Enabled = false;
                _memsControl.CalibrationProgressBarVisible = true;
                Task.Run(() =>
                {
                    while (_device.ReadParam<bool>(Parameter.MEMSCalibrationStatus))
                    {
                        Thread.Sleep(3000);
                    }
                    _memsControl.SetEnabled(true);
                    _memsControl.CalibrationProgressBarVisible = false;
                });
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform CalibrateMEMS operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _memsControl_ResetQuaternionButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.ResetQuaternion);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform ResetQuaternion operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _memsControl_StopOrientButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StopAngle);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StopAngle operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _memsControl_StartOrientButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StartAngle);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StartAngle operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _memsControl_SelectedAccelerometerSensChanged(object sender, AccelerometerSensitivity accelSens)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.AccelerometerSens, accelSens);
            var updatedParam = _device.ReadParam<AccelerometerSensitivity>(Parameter.AccelerometerSens);
            if (updatedParam != accelSens)
            {
                _memsControl.AccelerometerSensitivityComboValue = updatedParam;
            }
        }

        private void _memsControl_SelectedGyroscopeSensChanged(object sender, GyroscopeSensitivity gyroSens)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            _device.SetParam(Parameter.GyroscopeSens, gyroSens);
            var updatedParam = _device.ReadParam<GyroscopeSensitivity>(Parameter.GyroscopeSens);
            if (updatedParam != gyroSens)
            {
                _memsControl.GyroscopeSensitivityComboValue = updatedParam;
            }
        }

        private void _memsControl_StopMemsButtonClicked(object sender, EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StopMEMS);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StopMEMS operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
            }
        }

        private void _memsControl_StartMemsButtonClicked(object sender, System.EventArgs e)
        {
            if (_device.ReadParam<DeviceState>(Parameter.State) == DeviceState.Disconnected)
                return;

            try
            {
                _device.Execute(Command.StartMEMS);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Cannot perform StartMEMS operation on device {_device.ReadParam<string>(Parameter.Name)}: {exc.Message}", exc);
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
            _memsControl.SetEnabled(connectionState == DeviceState.Connected);
            if (connectionState == DeviceState.Connected)
            {
                _memsControl.GyroscopeSensitivityComboValue = _device.ReadParam<GyroscopeSensitivity>(Parameter.GyroscopeSens);
                _memsControl.AccelerometerSensitivityComboValue = _device.ReadParam<AccelerometerSensitivity>(Parameter.AccelerometerSens);
            }
        }

        public bool Enabled
        {
            get => _memsControl?.Enabled ?? false;
            set => _memsControl.Enabled = value;
        }
    }
}