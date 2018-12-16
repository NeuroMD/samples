using System;
using Neuro;

namespace Biofeedback
{
    class ChannelAdapter : EegChannel
    {
        public override string ToString()
        {
            return $"{Info.Name}";
        }

        public ChannelAdapter(Device device, ChannelInfo info) : base(device, info)
        {
        }
    }
}
