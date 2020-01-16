using System;
using Neuro;

namespace SignalAndResistance
{
    public class DeviceWrapper
    {
        private readonly DeviceInfo _deviceInfo;
        public DeviceWrapper(DeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }

        public static implicit operator DeviceInfo(DeviceWrapper wrapper)
        {
            return wrapper._deviceInfo;
        }

        public string AddressString =>
            $"{(_deviceInfo.Address & 0xFF0000000000) >> 40:X2}:" +
            $"{(_deviceInfo.Address & 0x00FF00000000) >> 32:X2}:" +
            $"{(_deviceInfo.Address & 0x0000FF000000) >> 24:X2}:" +
            $"{(_deviceInfo.Address & 0x000000FF0000) >> 16:X2}:" +
            $"{(_deviceInfo.Address & 0x00000000FF00) >> 8:X2}:" +
            $"{ _deviceInfo.Address & 0x0000000000FF:X2}";

        public override string ToString()
        {
            return $"{_deviceInfo.Name} [{AddressString}] S/N:{_deviceInfo.SerialNumber}";
        }
    }
}