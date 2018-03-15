using System;
using UnityEngine;

public class WindowsEditorStub : INeuroConnection
{
    public event EventHandler<DeviceFoundEventArgs> DeviceFound;
    public void StartScan(int timeout)
    {
        if (DeviceFound!=null) DeviceFound(this, new DeviceFoundEventArgs(new WindowsEditorDevice()));
    }

    public void StopScan()
    {
        Debug.Log("Stop scan");
    }
}
