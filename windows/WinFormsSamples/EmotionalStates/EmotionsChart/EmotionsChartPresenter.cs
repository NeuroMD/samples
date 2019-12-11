using System;
using System.Net;
using EmotionalStates.Network;
using Neuro;
using Exception = System.Exception;

namespace EmotionalStates.EmotionsChart
{
    public class EmotionsChartPresenter : IDisposable
    {
        private bool _isDisposed = false;
        private readonly IEmotionsChart _chart;
        private readonly EmotionStateChannel _stateChannel;
        private readonly EegIndexChannel _indexChannel;
        private readonly SpectrumPowerChannel _alphaLeft;
        private readonly SpectrumPowerChannel _betaLeft;
        private readonly SpectrumPowerChannel _alphaRight;
        private readonly SpectrumPowerChannel _betaRight;
        private IPEndPoint _oscDestination = new IPEndPoint(IPAddress.Broadcast, 8000);

        public EmotionsChartPresenter(
            IEmotionsChart chart, 
            EmotionStateChannel stateChannel, 
            SpectrumPowerChannel alphaLeft, 
            SpectrumPowerChannel betaLeft,
            SpectrumPowerChannel alphaRight,
            SpectrumPowerChannel betaRight, 
            EegIndexChannel indexChannel)
        {
            _stateChannel = stateChannel;
            _alphaLeft = alphaLeft;
            _betaLeft = betaLeft;
            _alphaRight = alphaRight;
            _betaRight = betaRight;
            _indexChannel = indexChannel;

            _chart = chart;
            _chart.Mode = EmotionBarMode.Wait;

            _stateChannel.LengthChanged += StateChannelLengthChanged;
            _indexChannel.LengthChanged += _indexChannel_LengthChanged;
            _alphaLeft.LengthChanged += _alphaLeft_LengthChanged;
            _betaLeft.LengthChanged += _betaLeft_LengthChanged;
            _alphaRight.LengthChanged += _alphaRight_LengthChanged;
            _betaRight.LengthChanged += _betaRight_LengthChanged;
        }

        private void _indexChannel_LengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                _chart.BasePower = _indexChannel.BasePower;
                _chart.Mode = _indexChannel.Mode == EegIndexMode.Artifacts
                    ? EmotionBarMode.Artifact
                    : (_stateChannel.TotalLength > 0 ? EmotionBarMode.Data : EmotionBarMode.Wait);
            }
        }

        private void _alphaLeft_LengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                var index = _alphaLeft.ReadData(length - 1, 1)[0];
                _chart.AlphaLeft = index;
            }
        }

        private void _betaLeft_LengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                var index = _betaLeft.ReadData(length - 1, 1)[0];
                _chart.BetaLeft = index;
            }
        }

        private void _alphaRight_LengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                var index = _alphaRight.ReadData(length - 1, 1)[0];
                _chart.AlphaRight = index;
            }
        }

        private void _betaRight_LengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                var index = _betaRight.ReadData(length - 1, 1)[0];
                _chart.BetaRight = index;
            }
        }

        public IPEndPoint Destination
        {
            set => _oscDestination = value;
        }

        private void StateChannelLengthChanged(object sender, int length)
        {
            if (_isDisposed) return;
            if (length > 0)
            {
                var state = _stateChannel.ReadData(length - 1, 1)[0];
                _chart.State = state;
                _chart.BaseValue = _stateChannel.BaseValue;
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
            _isDisposed = true;
            _stateChannel.LengthChanged -= StateChannelLengthChanged;
            _alphaLeft.LengthChanged -= _alphaLeft_LengthChanged;
            _betaLeft.LengthChanged -= _betaLeft_LengthChanged;
            _alphaRight.LengthChanged -= _alphaRight_LengthChanged;
            _betaRight.LengthChanged -= _betaRight_LengthChanged;
            _stateChannel.Dispose();
        }
    }
}