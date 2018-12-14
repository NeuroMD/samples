using System;
using Neuro;

namespace Biofeedback
{
    class ChannelAdapter<T>
    {
        private readonly IBaseChannel<T> _channel;

        public event EventHandler<int> LengthChanged;

        public ChannelAdapter(IBaseChannel<T> channel)
        {
            _channel = channel;
            _channel.LengthChanged += _channel_LengthChanged;
        }

        private void _channel_LengthChanged(object sender, int length)
        {
            LengthChanged?.Invoke(this, Length);
        }

        public T[] ReadLastData(int length)
        {
            var offset = _channel.TotalLength - length;
            if (offset < 0)
            {
                offset = 0;
                length = _channel.TotalLength;
            }
            return _channel.ReadData(offset, length);
        }

        public int Length => _channel.TotalLength;
        public double SamplingFrequency => _channel.SamplingFrequency;

        public override string ToString()
        {
            return $"{_channel.Info.Type}-{_channel.Info.Name}";
        }
    }
}
