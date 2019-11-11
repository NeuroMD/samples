using System;
using System.Windows.Forms;
using Neuro;
using SignalView;

namespace EmotionalStates
{
    class SignalViewController
    {
        private readonly Control _invocationContext;
        private readonly SignalView.SignalChart _signalChart;
//        private readonly ToolStripLabel _durationLabel;
        private BipolarDoubleChannel _channel;

        public SignalViewController(Control invocationContext, SignalView.SignalChart signalView/*, ToolStripLabel durationLabel*/)
        {
            _invocationContext = invocationContext;
            _signalChart = signalView;
//            _durationLabel = durationLabel;
        }

        public void SetChannel(BipolarDoubleChannel channel)
        {
            if (channel == _channel) return;
            if (_channel != null)
            {
                _channel.LengthChanged -= OnLengthChanged;
            }
            _channel = channel;
            if (_channel != null)
            {
                _channel.LengthChanged += OnLengthChanged;
                SetDuration(_channel.TotalLength);
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
                //_invocationContext.Invoke((MethodInvoker) delegate { /*_durationLabel.Text = $@"{duration:0.00} s";*/ });
            }
            catch (Exception)
            {

            }
        }

        private void SetChannelData(double[] data, double samplingFrequency, int channelLength)
        {
            try
            {
                _invocationContext.Invoke((MethodInvoker) delegate
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

            //SetDuration(length / _channel.SamplingFrequency);
            SetChannelData(_channel.ReadData(_channel.TotalLength- _signalChart.SamplesOnScreen, _signalChart.SamplesOnScreen) , _channel.SamplingFrequency, length);
        }
    }
}

