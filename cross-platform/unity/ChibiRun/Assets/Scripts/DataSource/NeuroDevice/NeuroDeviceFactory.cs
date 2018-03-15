using System;
using UnityEngine;

public class NeuroDeviceFactory
{
    public static INeuroDevice CreateDeviceForCurrentPlatform(IntPtr deviceNativePtr)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return new AndroidNeuroDevice(deviceNativePtr);
            case RuntimePlatform.IPhonePlayer:
                throw new NotImplementedException();
            case RuntimePlatform.WindowsEditor:
                return new WindowsEditorDevice();
            default:
                throw new NotSupportedException("Current platform not supported by NeuroDevice class");
        }
    }
}

public interface INeuroDevice
{
    event EventHandler<DeviceStateChangedEventArgs> StateChanged;
    DeviceState State { get; }
    //TODO Features
    string Name { get; }
    string Address { get; }
    void Connect();
    void Disconnect();
}

public enum DeviceState
{
    UNKNOWN = 0,
    ERROR = 1,
    DISCONNECTED = 2,
    CONNECTED = 3,
    READY = 4,
    WORKING = 5,
    CLOSED = 6
}

public class DeviceStateChangedEventArgs : EventArgs
{
    public DeviceState State { get; private set; }

    public DeviceStateChangedEventArgs(int state) : base()
    {
        State = (DeviceState)state;
    }
}