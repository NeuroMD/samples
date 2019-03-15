using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Indices;
using SignalView;

namespace Indices
{
    class SignalViewController
    {
        private readonly Control _invocationContext;
        private readonly SignalChart _signalChart;
        private readonly Label _durationLabel;
        private DoubleSignalChannelWrap _channel;

        public SignalViewController(Control invocationContext, SignalChart signalView, Label durationLabel)
        {
            _invocationContext = invocationContext;
            _signalChart = signalView;
            _durationLabel = durationLabel;
        }

        public void SetChannel(DoubleSignalChannelWrap channel)
        {
            if (_channel != null)
            {
                _channel.LengthChanged -= OnLengthChanged;
            }
            _channel = channel;
            if (_channel != null)
            {
                _channel.LengthChanged += OnLengthChanged;
                SetDuration(_channel.TotalLength / _channel.SamplingFrequency);
                SetChannelData(ReadLastData(_signalChart.SamplesOnScreen), _channel.SamplingFrequency, _channel.TotalLength);
            }
            else
            {
                SetDuration(0);
            }
        }

        private void SetDuration(double duration)
        {
            try
            {
                _invocationContext.Invoke((MethodInvoker) delegate { _durationLabel.Text = $@"{duration:0.00} s"; });
            }
            catch (Exception)
            {

            }
        }

        private void SetChannelData(double[] data, double samplingFrequency, int channelLength)
        {
            try
            {
                Task.Run(() => {
                    _invocationContext.Invoke((MethodInvoker)delegate
                    {
                        _signalChart.DrawSignal(data, data.Length, data.Length, channelLength, (int)samplingFrequency,
                            new[] { _channel.ToString() });
                    });
                });
                
            }
            catch (Exception)
            {
            }
        }

        private double[] ReadLastData(int length)
        {
            var offset = _channel.TotalLength - length;
            if (offset < 0)
            {
                offset = 0;
                length = _channel.TotalLength;
            }
            return _channel.ReadData(offset, length);
        }

        private void OnLengthChanged(object sender, int length)
        {
            if (_channel == null) return;

            SetDuration(length / _channel.SamplingFrequency);
            SetChannelData(ReadLastData(_signalChart.SamplesOnScreen), _channel.SamplingFrequency, length);
        }
    }
}

