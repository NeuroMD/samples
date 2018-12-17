using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Biofeedback.Spectrum;
using Neuro;

namespace Biofeedback
{
    class SpectrumChartController
    {
        private readonly SpectrumChart _spectrumChart;
        private readonly TrackBar _timeTrackBar;
        private readonly TrackBar _scaleTrackBar;
        private readonly SpectrumModel _model;

        public SpectrumChartController(SpectrumChart chart, TrackBar timeTrackBar, TrackBar scaleTrackBar, SpectrumModel model)
        {
            _spectrumChart = chart;
            _timeTrackBar = timeTrackBar;
            _timeTrackBar.ValueChanged += _timeTrackBar_ValueChanged;
            _scaleTrackBar = scaleTrackBar;
            _scaleTrackBar.ValueChanged += _scaleTrackBar_ValueChanged;
            _model = model;
            _spectrumChart.FrequencyStep = _model.FrequencyStep;
            _spectrumChart.SigScale = 500;
        }

        public void Redraw()
        {
            _spectrumChart.DrawSpectrumList(_model.Spectrums);
        }

        private void _scaleTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            _spectrumChart.SigScale = _scaleTrackBar.Value * 10;
        }

        private void _timeTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            _model.WindowDuration = _timeTrackBar.Value;
        }
    }
}
