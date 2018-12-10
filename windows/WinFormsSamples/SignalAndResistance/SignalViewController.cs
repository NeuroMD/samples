using System;
using System.Windows.Forms;
using SignalView;

namespace SignalAndResistance
{
    class SignalViewController
    {
        private readonly Control _invokationContext;
        private readonly SignalChart _signalChart;
        private readonly ToolStripLabel _durationLabel;
        private ChannelAdapter<double> _channel;

        public SignalViewController(Control invokationContext, SignalChart signalView, ToolStripLabel durationLabel)
        {
            _invokationContext = invokationContext;
            _signalChart = signalView;
            _durationLabel = durationLabel;
        }

        public void SetChannel(ChannelAdapter<double> channel)
        {
            if (_channel != null)
            {
                _channel.LengthChanged -= OnLengthChanged;
            }
            _channel = channel;
            if (_channel != null)
            {
                _channel.LengthChanged += OnLengthChanged;
            }
            else
            {
                SetDuration(0);
            }
        }

        private void SetDuration(double duration)
        {
            _invokationContext.Invoke((MethodInvoker) delegate { _durationLabel.Text = $@"{duration:0.00} s"; });
        }

        private void SetChannelData(double[] data, double samplingFrequency, int channelLength)
        {
            try
            {
                _invokationContext.Invoke((MethodInvoker) delegate
                {
                    _signalChart.DrawSignal(data, data.Length, data.Length, channelLength, (int) samplingFrequency,
                        new[] {_channel.ToString()});
                });
            }
            catch (Exception)
            {
            }
        }

        private void OnLengthChanged(object sender, int length)
        {
            if (_channel == null) return;

            SetDuration(length / _channel.SamplingFrequency);
            SetChannelData(_channel.ReadLastData(_signalChart.SamplesOnScreen), _channel.SamplingFrequency, length);
        }
    }
}

