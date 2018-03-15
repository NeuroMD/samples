using System;
using UnityEngine;

public class NeuroConnectionFactory
{
    public static INeuroConnection GetConnectionForCurrentPlarform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return new AndroidNeuroConnection();
            case RuntimePlatform.IPhonePlayer:
                throw new NotImplementedException();
            case RuntimePlatform.WindowsEditor:
                return new WindowsEditorStub();
            default:
                throw new NotSupportedException("Current platform not supported by NeuroConnection module");
        }
    }
}

public interface INeuroConnection
{
    event EventHandler<DeviceFoundEventArgs> DeviceFound; 
    void StartScan(int timeout);
    void StopScan();
}

public class DeviceFoundEventArgs : EventArgs
{
    public INeuroDevice Device
    {
        get; private set;
    }

    public DeviceFoundEventArgs(INeuroDevice foundDevice) : base()
    {
        Device = foundDevice;
    }
}


