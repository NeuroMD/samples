using System;
using UnityEngine;

class WindowsEditorDevice:INeuroDevice
{
    public event EventHandler<DeviceStateChangedEventArgs> StateChanged;
    public string Name { get; private set; }
    public string Address { get; private set; }
    public DeviceState State { get { return DeviceState.READY;} }

    public WindowsEditorDevice()
    {
        var rnd = UnityEngine.Random.value;
        var intRnd = (int)Math.Round(rnd*3f);
        switch (intRnd)
        {
            case 0:
                Name = "Neurotech_Colibri_R";
                break;
            case 1:
                Name = "Neurotech_Colibri_B";
                break;
            case 2:
                Name = "Neurotech_Colibri_Y";
                break;
            case 3:
                Name = "Neurotech_Colibri_W";
                break;
        }
        Address = "00:00:00:00:00:00";
    }

    public void Connect()
    {
        Debug.Log("Device connect");
    }

    public void Disconnect()
    {
       Debug.Log("Device disconnect");
    }
}

