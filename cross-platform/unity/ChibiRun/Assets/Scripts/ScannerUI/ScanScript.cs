using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ScanScript : MonoBehaviour
{
    private RectTransform _devicesPanel;
    private GameObject _templateText;
    private GameObject _templateButton;

    private const float TEXT_STEP_Y = 70f;
    private const float BUTTON_STEP_Y = 70f;
    private float _nextTextY = 0;
    private float _nextButtonY = 0;

    void Start()
    {
        _devicesPanel = GameObject.Find("FoundDevicePanel").GetComponent<RectTransform>();

        _templateText = GameObject.Find("TemplateText");
        _templateButton = GameObject.Find("TemplateButton");

        _templateText.SetActive(false);
        _templateButton.SetActive(false);
    }

    public void OnClick()
    {
        for (var i = 0; i < _devicesPanel.childCount; ++i)
        {
            var childObj = _devicesPanel.GetChild(i).gameObject;
            if (childObj.name == "TemplateText" || childObj.name == "TemplateButton") continue;
            GameObject.Destroy(childObj);
        }
        _nextTextY = 0;
        _nextButtonY = 0;
        ScannerModel.Instance.OnScanButtonClicked();
    }

    void Update()
    {
        if (ScannerModel.Instance.AwaitingDevicesCount <= 0) return;

            var visualDevice = ScannerModel.Instance.NextAwaitingDevice;
        if (visualDevice != null)
        {
            {
                var newText = (GameObject) Instantiate(_templateText, _devicesPanel);
                newText.name = visualDevice.Name;
                newText.GetComponent<Text>().color = visualDevice.DeviceColor;
                newText.GetComponent<Text>().text = visualDevice.Name;
                newText.transform.position += new Vector3(0, _nextTextY, 0);
                newText.SetActive(true);
                _nextTextY -= TEXT_STEP_Y;
            }

            {
                var newButton = (GameObject) Instantiate(_templateButton, _devicesPanel);
                newButton.name = visualDevice.Name + "Button";
                newButton.transform.position += new Vector3(0, _nextButtonY, 0);
                newButton.GetComponent<ConnectButtonScript>().Device = visualDevice;
                newButton.SetActive(true);
                _nextButtonY -= BUTTON_STEP_Y;
            }
        }

    }
}
