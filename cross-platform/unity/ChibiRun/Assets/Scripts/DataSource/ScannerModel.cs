using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Debug = UnityEngine.Debug;

class ScannerModel
{
    private static ScannerModel _instance;

    public static ScannerModel Instance
    {
        get { return _instance ?? (_instance = new ScannerModel()); }
    }
    
    private readonly INeuroConnection _neuroConnection;
    private readonly ReaderWriterLockSlim _visualDevicesLock = new ReaderWriterLockSlim();
    private readonly Queue<VisualDevice> _foundDevicesQueue = new Queue<VisualDevice>();

    private ScannerModel()
    {
        _neuroConnection = NeuroConnectionFactory.GetConnectionForCurrentPlarform();
        _neuroConnection.DeviceFound += _neuroConnection_DeviceFound;
    }

    private void _neuroConnection_DeviceFound(object sender, DeviceFoundEventArgs e)
    {
        var device = e.Device;
        device.StateChanged += Device_StateChanged;
        Debug.Log(String.Format("Device found: {0}. Connecting...", device.Name));
        var state = device.State;
        Debug.Log(String.Format("Device state: {0}", state));
        if (state != DeviceState.ERROR && state != DeviceState.CLOSED && state != DeviceState.READY)
        {
            device.Connect();
        }
    }

    private void Device_StateChanged(object sender, DeviceStateChangedEventArgs e)
    {
        var device = (INeuroDevice) sender;
        Debug.Log(String.Format("{0} state changed: {1}", device.Name, e.State));
        if (e.State == DeviceState.READY)
        {
            OnNewDevice(device);
        }
    }

    private void OnNewDevice(INeuroDevice device)
    {
        _visualDevicesLock.EnterWriteLock();
        try
        {
            var visualDevice = new VisualDevice(device);
            _foundDevicesQueue.Enqueue(visualDevice);
        }
        catch (NotSupportedException nse)
        {
            Debug.LogWarning(String.Format("Can't create visual device: {0}", nse.Message));
        }
        finally
        {
            _visualDevicesLock.ExitWriteLock();
        }
    }

    public int AwaitingDevicesCount
    {
        get
        {
            _visualDevicesLock.EnterReadLock();
            try
            {
                return _foundDevicesQueue.Count;
            }
            finally
            {
                _visualDevicesLock.ExitReadLock();
            }
        }
    }

    public VisualDevice NextAwaitingDevice
    {
        get
        {
            if (_foundDevicesQueue.Count == 0) return null;
            _visualDevicesLock.EnterWriteLock();
            try
            {
                return _foundDevicesQueue.Dequeue();
            }
            finally
            {
                _visualDevicesLock.ExitWriteLock();
            }
        }
    }

    public void OnScanButtonClicked()
    {
        _neuroConnection.StartScan(10000);
    }
}
