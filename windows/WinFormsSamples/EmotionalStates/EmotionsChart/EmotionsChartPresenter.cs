using System;
using System.Net;
using EmotionalStates.Network;
using Neuro;
using Exception = System.Exception;

namespace EmotionalStates.EmotionsChart
{
    public class EmotionsChartPresenter : IDisposable
    {
        private readonly IEmotionsChart _chart;
        private readonly EmotionStateChannel _channel;
        private IPEndPoint _oscDestination = new IPEndPoint(IPAddress.Broadcast, 8000);

        public EmotionsChartPresenter(IEmotionsChart chart, EmotionStateChannel channel)
        {
            _channel = channel;
            _chart = chart;
            _chart.Mode = EmotionBarMode.Wait;

            _channel.LengthChanged += _channel_LengthChanged;
        }

        public IPEndPoint Destination
        {
            set => _oscDestination = value;
        }

        private void _channel_LengthChanged(object sender, int length)
        {
            if (length > 0)
            {
                _chart.Mode = EmotionBarMode.Data;
                var state = _channel.ReadData(length - 1, 1)[0];
                _chart.State = state;
                try
                {
                    OSCDataSender.SendEmotionData(state, _oscDestination);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable send OSC packet: {e.Message}");
                }
            }
        }

        public void Dispose()
        {
            _channel.LengthChanged -= _channel_LengthChanged;
            _channel.Dispose();
        }
    }
}