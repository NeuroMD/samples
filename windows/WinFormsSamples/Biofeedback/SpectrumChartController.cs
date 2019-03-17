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

        private readonly TrackBar _lowFreqTrackBar;
        private readonly TrackBar _highFreqTrackBar;
        private readonly Label _lowFreqLabel;
        private readonly Label _highFreqLabel;
        private readonly Label _powerLabel;

        private readonly RadioButton _rectangularWindowRadio;
        private readonly RadioButton _sineWindowRadio;
        private readonly RadioButton _hammingWindowRadio;
        private readonly RadioButton _blackmanWindowRadio;


        public SpectrumChartController(SpectrumChart chart, TrackBar timeTrackBar, TrackBar scaleTrackBar, SpectrumModel model, Label amplitudeLabel, Label timeLabel, 
            TrackBar lowFreqTrackBar, TrackBar highFreqTrackBar, Label lowFreqLabel, Label highFreqLabel, Label powerLabel,
            RadioButton rectangleWindowRadio, RadioButton sineWindowRadio, RadioButton hammingWindowRadio, RadioButton blackmanWindowRadio)
        {
            _spectrumChart = chart;
            _timeTrackBar = timeTrackBar;
            _timeTrackBar.ValueChanged += _timeTrackBar_ValueChanged;
            _scaleTrackBar = scaleTrackBar;
            _scaleTrackBar.ValueChanged += _scaleTrackBar_ValueChanged;
            _model = model;
            _amplitudeLabel = amplitudeLabel;
            _timeLabel = timeLabel;
            _spectrumChart.SigScale = 100;

            _lowFreqTrackBar = lowFreqTrackBar;
            _highFreqTrackBar = highFreqTrackBar;
            _lowFreqLabel = lowFreqLabel;
            _highFreqLabel = highFreqLabel;
            _powerLabel = powerLabel;

            _lowFreqTrackBar.ValueChanged += _lowFreqTrackBar_ValueChanged;
            _highFreqTrackBar.ValueChanged += _highFreqTrackBar_ValueChanged;

            _rectangularWindowRadio = rectangleWindowRadio;
            _sineWindowRadio = sineWindowRadio;
            _hammingWindowRadio = hammingWindowRadio;
            _blackmanWindowRadio = blackmanWindowRadio;
            _rectangularWindowRadio.CheckedChanged += _rectangularWindowRadio_CheckedChanged;
            _sineWindowRadio.CheckedChanged += _sineWindowRadio_CheckedChanged;
            _hammingWindowRadio.CheckedChanged += _hammingWindowRadio_CheckedChanged;
            _blackmanWindowRadio.CheckedChanged += _blackmanWindowRadio_CheckedChanged;
        }

        private void _blackmanWindowRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_blackmanWindowRadio.Checked) _model.WindowType = SpectrumWindow.Blackman;
        }

        private void _hammingWindowRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_hammingWindowRadio.Checked) _model.WindowType = SpectrumWindow.Hamming;
        }

        private void _sineWindowRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_sineWindowRadio.Checked) _model.WindowType = SpectrumWindow.Sine;
        }

        private void _rectangularWindowRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_rectangularWindowRadio.Checked) _model.WindowType = SpectrumWindow.Rectangular;
        }

        private void _highFreqTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            if (_highFreqTrackBar.Value < _lowFreqTrackBar.Value + 2)
            {
                _highFreqTrackBar.Value = _lowFreqTrackBar.Value + 2;
            }
            _model.HighFrequency = _highFreqTrackBar.Value * 0.5f;
            _highFreqLabel.Text = $"{_model.HighFrequency:F1} Hz";
            _spectrumChart.SetFreqMarkers(_model.LowFrequency, _model.HighFrequency);
        }

        private void _lowFreqTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            if (_lowFreqTrackBar.Value > _highFreqTrackBar.Value - 2)
            {
                _lowFreqTrackBar.Value = _highFreqTrackBar.Value - 2;
            }

            _model.LowFrequency = _lowFreqTrackBar.Value * 0.5f;
            _lowFreqLabel.Text = $"{_model.LowFrequency:F1} Hz";
            _spectrumChart.SetFreqMarkers(_model.LowFrequency, _model.HighFrequency);
        }

        public void Redraw()
        {
            _spectrumChart.FrequencyStep = _model.FrequencyStep;
            _spectrumChart.DrawSpectrum(_model.Spectrum);
            _powerLabel.Text = $" = {_model.SpectrumPower * 1e6:F4} uW";
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
