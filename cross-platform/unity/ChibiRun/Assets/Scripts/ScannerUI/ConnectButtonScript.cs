using System;
using UnityEngine;

class ConnectButtonScript : MonoBehaviour
{
    public VisualDevice Device { get; set; }

    public void OnClick()
    {
        if (Device == null)
        {
            Debug.LogError("Connect button. Attemt to connect on null device");
            return;
        }
        Debug.Log(String.Format("{0}: Connect click", gameObject.name));
        Debug.Log(String.Format("Device name: {0}", Device.Name));
    }
}
