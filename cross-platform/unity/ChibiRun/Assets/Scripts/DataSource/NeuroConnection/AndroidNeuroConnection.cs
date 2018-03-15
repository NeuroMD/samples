using System;
using System.Runtime.InteropServices;
using UnityEngine;


public class AndroidNeuroConnection : INeuroConnection
{
    public event EventHandler<DeviceFoundEventArgs> DeviceFound;

    private delegate void DeviceFoundCallbackDelegate([In] IntPtr subscriber, [In] IntPtr neuroDeviceSharedPtr);

    private readonly IntPtr _nativeNeuroConnection;
    private readonly IntPtr _callbackPointer;
    private readonly DeviceFoundCallbackDelegate _deviceFoundCallback;

    public AndroidNeuroConnection()
    {
        _nativeNeuroConnection = unityCreateNeuroConnection();
        _deviceFoundCallback = new DeviceFoundCallbackDelegate(OnDeviceFound);
        _callbackPointer = Marshal.GetFunctionPointerForDelegate(_deviceFoundCallback);
        neuroConnectionSubscribeDeviceFound(_nativeNeuroConnection, IntPtr.Zero, _callbackPointer);
    }

    ~AndroidNeuroConnection()
    {
        //Since we have only one connection object per application
        //we could use finalizer to free unmanaged resource of NeuroConnection
        //If we will need for several connections, one after another, in one programm
        //it's better to change finalizer to IDisosable implementation

        deleteNeuroConnectionObj(_nativeNeuroConnection);
    }

    public void StartScan(int timeout)
    {
        neuroConnectionStartScan(_nativeNeuroConnection, timeout);
    }

    public void StopScan()
    {
        neuroConnectionStopScan(_nativeNeuroConnection);
    }

    private void OnDeviceFound(IntPtr subscriber, IntPtr neuroDeviceSharedPtr)
    {
        var neuroDevice = NeuroDeviceFactory.CreateDeviceForCurrentPlatform(neuroDeviceSharedPtr);
        if (DeviceFound != null) DeviceFound(this, new DeviceFoundEventArgs(neuroDevice));
    }

    [DllImport("_neuro_android_wrapper")]
    private static extern IntPtr unityCreateNeuroConnection();

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroConnectionSubscribeDeviceFound(IntPtr neuroConnectionObj, IntPtr subscriber, IntPtr callbackFunctionPtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern void deleteNeuroConnectionObj(IntPtr neuroConnectionObj);

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroConnectionStartScan(IntPtr neuroConnectionObj, int timeout);//close your eyes and hope that native int is not less than 32 bits

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroConnectionStopScan(IntPtr neuroConnectionObj);

}
