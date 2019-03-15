using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Indices.Spectrum;
using Neuro;

namespace Indices
{
    class SpectrumChartController
    {
        private readonly SpectrumChart _spectrumChart;
        private readonly TrackBar _timeTrackBar;
        private readonly TrackBar _scaleTrackBar;
        private readonly SpectrumModel _model;
        private readonly Label _amplitudeLabel;
        private readonly Label _timeLabel;

        public SpectrumChartController(SpectrumChart chart, TrackBar timeTrackBar, TrackBar scaleTrackBar, SpectrumModel model, Label amplitudeLabel, Label timeLabel)
        {
            _spectrumChart = chart;
            _timeTrackBar = timeTrackBar;
            _timeTrackBar.ValueChanged += _timeTrackBar_ValueChanged;
            _scaleTrackBar = scaleTrackBar;
            _scaleTrackBar.ValueChanged += _scaleTrackBar_ValueChanged;
            _model = model;
            _amplitudeLabel = amplitudeLabel;
            _timeLabel = timeLabel;
            _spectrumChart.SigScale = 500;
        }

        public void Redraw()
        {
            _spectrumChart.FrequencyStep = _model.FrequencyStep;
            _spectrumChart.DrawSpectrum(_model.Spectrum);
        }

        private void _scaleTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            _spectrumChart.SigScale = _scaleTrackBar.Value * 10;
            _amplitudeLabel.Text = $"{_spectrumChart.SigScale} uV";
        }

        private void _timeTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            _model.WindowDuration = _timeTrackBar.Value;
            _timeLabel.Text = $"{_model.WindowDuration} s";
        }
    }
}
