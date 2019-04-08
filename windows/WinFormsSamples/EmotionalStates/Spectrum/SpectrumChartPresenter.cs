using System.Windows.Forms;

namespace EmotionalStates.Spectrum
{
    public class SpectrumChartPresenter
    {
        private readonly SpectrumChart _chart;
        private readonly TrackBar _amplitudeBar;
        private readonly SpectrumModel _model;

        public SpectrumChartPresenter(SpectrumChart chart, TrackBar amplitudeBar, SpectrumModel model)
        {
            _chart = chart;
            _amplitudeBar = amplitudeBar;
            _model = model;
            _chart.FrequencyStep = _model.FrequencyStep;
            _amplitudeBar.ValueChanged += _amplitudeBar_ValueChanged;
            _chart.SigScale = _amplitudeBar.Value;
            _model.SpectrumCalculated += _model_SpectrumCalculated;
        }

        private void _model_SpectrumCalculated(object sender, System.EventArgs e)
        {
            _chart.Spectrum = _model.Spectrum;
        }

        private void _amplitudeBar_ValueChanged(object sender, System.EventArgs e)
        {
            _chart.SigScale = _amplitudeBar.Value;
        }
    }
}