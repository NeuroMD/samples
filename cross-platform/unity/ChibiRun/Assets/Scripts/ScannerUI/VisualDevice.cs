using System;
using UnityEngine;

public class VisualDevice
{
   public event EventHandler<EventArgs> DeviceConnectionLost; 

    public string Name { get; private set; }

    public Color DeviceColor { get; private set; }

    public VisualDevice(INeuroDevice hardwareDevice)
    {
        var devName = hardwareDevice.Name;
        //NeuroDevice = hardwareDevice;
        //NeuroDevice.StateChanged += NeuroDevice_StateChanged;
        switch (devName)
        {
            case "Neurotech_Colibri_R":
            {
                DeviceColor = Color.red;
                Name = "Callibri Red";
                break;
            }
            case "Neurotech_Colibri_B":
            {
                DeviceColor = Color.blue;
                Name = "Callibri Blue";
                break;
            }
            case "Neurotech_Colibri_Y":
            {
                DeviceColor = Color.yellow;
                Name = "Callibri Yellow";
                break;
            }
            case "Neurotech_Colibri_W":
            {
                DeviceColor = Color.white;
                Name = "Callibri White";
                break;
            }
            default: 
                throw new NotSupportedException(String.Format("Device {0} is not supported", devName));
        }
    }

   /* private void NeuroDevice_StateChanged(object sender, DeviceStateChangedEventArgs e)
    {
        if (e.State == DeviceState.ERROR || e.State == DeviceState.DISCONNECTED || e.State == DeviceState.CLOSED)
            if (DeviceConnectionLost != null) DeviceConnectionLost(this, null);
    }*/
}
