using Neuro;

namespace EmotionalStates.IndexChart
{
    public class IndexSettingsPresenter
    {
        private readonly IndexSettingsControl _control;
        private readonly EegIndexChannel _channel;

        public IndexSettingsPresenter(IndexSettingsControl control, EegIndexChannel channel)
        {
            _control = control;
            _channel = channel;
            _control.SettingsChanged += _control_SettingsChanged;
        }

        private void _control_SettingsChanged(object sender, System.EventArgs e)
        {
            _channel.Delay = _control.Delay;
            _channel.SetWeights(_control.AlphaWeight, _control.BetaWeight, _control.DeltaWeight, _control.ThetaWeight);
        }
    }
}