using System;
using Neuro;

namespace Indices
{
    class DoubleSignalChannelWrap : IDataChannel<double>
    {
        private readonly IDataChannel<double> _channel;
        private readonly string _name;

        public override string ToString()
        {
            return $"{_name ?? _channel.Info.Name}";
        }

        public DoubleSignalChannelWrap(IDataChannel<double> channel, string name = null)
        {
            _name = name;
            _channel = channel;
            _channel.LengthChanged += (sender, length) => { LengthChanged?.Invoke(sender, length); };
        }

        public ChannelInfo Info => _channel.Info;

        public int TotalLength => _channel.TotalLength;
        public float SamplingFrequency => _channel.SamplingFrequency;

        public event EventHandler<int> LengthChanged;

        public double[] ReadData(int offset, int length)
        {
            return _channel.ReadData(offset, length);
        }

        public IntPtr ChannelPtr => _channel.ChannelPtr;
    }
}
