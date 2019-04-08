using Neuro;

namespace EmotionalStates.EmotionsChart
{
    public class StatesSettingsPresenter
    {
        private readonly EmotionCoefficientsControl _settingsControl;
        private readonly EmotionStateChannel _channel;

        public StatesSettingsPresenter(EmotionCoefficientsControl settingsControl, EmotionStateChannel channel)
        {
            _settingsControl = settingsControl;
            _channel = channel;
            var coefficients = channel.StateCoefficients;
            _settingsControl.PX1 = coefficients.PX1;
            _settingsControl.PX2 = coefficients.PX2;
            _settingsControl.PX3 = coefficients.PX3;
            _settingsControl.PX4 = coefficients.PX4;
            _settingsControl.NX1 = coefficients.NX1;
            _settingsControl.NX2 = coefficients.NX2;
            _settingsControl.NX3 = coefficients.NX3;
            _settingsControl.NX4 = coefficients.NX4;
            _settingsControl.PY1 = coefficients.PY1;
            _settingsControl.PY2 = coefficients.PY2;
            _settingsControl.PY3 = coefficients.PY3;
            _settingsControl.PY4 = coefficients.PY4;
            _settingsControl.NY1 = coefficients.NY1;
            _settingsControl.NY2 = coefficients.NY2;
            _settingsControl.NY3 = coefficients.NY3;
            _settingsControl.NY4 = coefficients.NY4;
            _settingsControl.CoefficientsChanged += _settingsControl_CoefficientsChanged;
        }

        private void _settingsControl_CoefficientsChanged(object sender, System.EventArgs e)
        {
            _channel.StateCoefficients = new StateCoefficients()
            {
                PX1 = _settingsControl.PX1,
                PX2 = _settingsControl.PX2,
                PX3 = _settingsControl.PX3,
                PX4 = _settingsControl.PX4,
                NX1 = _settingsControl.NX1,
                NX2 = _settingsControl.NX2,
                NX3 = _settingsControl.NX3,
                NX4 = _settingsControl.NX4,
                PY1 = _settingsControl.PY1,
                PY2 = _settingsControl.PY2,
                PY3 = _settingsControl.PY3,
                PY4 = _settingsControl.PY4,
                NY1 = _settingsControl.NY1,
                NY2 = _settingsControl.NY2,
                NY3 = _settingsControl.NY3,
                NY4 = _settingsControl.NY4
            };
        }
    }
}