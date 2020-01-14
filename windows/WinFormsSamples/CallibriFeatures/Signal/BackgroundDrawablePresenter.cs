using System;
using System.Collections.Generic;
using System.Drawing;
using Neuro;

namespace CallibriFeatures.Signal
{
    public class BackgroundDrawablePresenter : IPlotDrawablePresenter
    {
        private readonly BackgroundDrawable _drawable;
        private readonly ElectrodeStateChannel _electrodeStateChannel;
        private readonly IChannel _dataChannel;

        public BackgroundDrawablePresenter(BackgroundDrawable drawable, ElectrodeStateChannel electrodeStateChannel)
        {
            _drawable = drawable;
            _electrodeStateChannel = electrodeStateChannel;
        }

        public ICalculationSpan ViewSpan
        {
            set
            {
                var zones = new List<IBackgroundZone>();
                if (value.StartTime < 0)
                {
                    var emptyZoneWidth = (float)Math.Min(-value.StartTime * _dataChannel.SamplingFrequency, _drawable.DrawableSize.Width);
                    zones.Add(new BackgroundZone(0, emptyZoneWidth, Color.AliceBlue));
                }
                var offset = (int)Math.Floor(value.StartTime * _electrodeStateChannel.SamplingFrequency);
                if (offset < 0) offset = 0;
                var length = (int)(value.Duration * _electrodeStateChannel.SamplingFrequency);
                var electrodeData = _electrodeStateChannel.ReadData(offset, length);
                
                for (var i = 0; i < electrodeData.Length; ++i)
                {
                    var time = (offset + i) / _dataChannel.SamplingFrequency;
                    var duration = 1.0 / _electrodeStateChannel.SamplingFrequency;
                    var color = electrodeData[i] == ElectrodeState.Detached ? Color.IndianRed : Color.AliceBlue;
                    var left = (float)(time - value.StartTime) / _dataChannel.SamplingFrequency;
                    var width = (float)duration / _dataChannel.SamplingFrequency;
                    zones.Add(new BackgroundZone(left, width, color));
                }

                _drawable.BackgroundZones = zones;
            }
        }
    }
}