using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;

class AndroidNeuroDevice:INeuroDevice, IDisposable
{
    #region IDisposable
    private bool disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            neuroDeviceDeleteDevice(_nativeObjPtr);
            disposed = true;
        }
    }

    ~AndroidNeuroDevice()
    {
        Dispose(false);
    }
    #endregion

    private delegate void StateChangedCallbackDelegate([In] IntPtr subscriber, [In] int state);
    private readonly IntPtr _nativeObjPtr;
    private readonly StateChangedCallbackDelegate _stateChangedCallback;


    public event EventHandler<DeviceStateChangedEventArgs> StateChanged; 

    public string Name
    {
        get
        {
            Debug.Log("Device get name");
            var constCharString = unityNeuroDeviceGetName(_nativeObjPtr);
            return Marshal.PtrToStringAnsi(constCharString);
        }
    }

    public string Address { get; private set; }

    public DeviceState State
    {
        get { return (DeviceState)neuroDeviceGetState(_nativeObjPtr); }
    }

    public AndroidNeuroDevice(IntPtr devicePtr)
    {
        _nativeObjPtr = devicePtr;
        _stateChangedCallback = OnStateChanged;
        var callbackPointer = Marshal.GetFunctionPointerForDelegate(_stateChangedCallback);
        neuroDeviceSubscribeStateChanged(_nativeObjPtr, IntPtr.Zero, callbackPointer);
    }

    private void OnStateChanged(IntPtr subscriber, int state)
    {
        if (StateChanged!=null) StateChanged(this, new DeviceStateChangedEventArgs(state));
    }

    public void Connect()
    {
        neuroDeviceConnect(_nativeObjPtr);
    }

    public void Disconnect()
    {
        neuroDeviceDisconnect(_nativeObjPtr);
    }


    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroDeviceDeleteDevice(IntPtr devicePtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroDeviceConnect(IntPtr devicePtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroDeviceDisconnect(IntPtr devicePtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern IntPtr unityNeuroDeviceGetName(IntPtr devicePtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern void neuroDeviceSubscribeStateChanged(IntPtr objPtr, IntPtr subscriber, IntPtr callbackPtr);

    [DllImport("_neuro_android_wrapper")]
    private static extern int neuroDeviceGetState(IntPtr devicePtr);
}

