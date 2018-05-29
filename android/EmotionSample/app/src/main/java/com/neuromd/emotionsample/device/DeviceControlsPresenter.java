package com.neuromd.emotionsample.device;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.ChannelsModel;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.parameters.ParameterName;

public class DeviceControlsPresenter {
    private final EegDeviceModel mDeviceModel;
    private final ChannelsModel mChannelsModel;
    private final IDeviceControlsView mDeviceView;
    
    public DeviceControlsPresenter(EegDeviceModel deviceModel,
                                   ChannelsModel channelsModel,
                                   IDeviceControlsView deviceView){
        mDeviceModel = deviceModel;
        mChannelsModel = channelsModel;
        mDeviceView = deviceView;
        
        mDeviceModel.scanStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isScanning) {
                String deviceText = "No device";
                Device device = mChannelsModel.getSelectedDevice();
                if (device == null){
                    if (isScanning){
                        deviceText = "Waiting for device...";
                        mDeviceView.setReconnectButtonEnabled(false);
                    }
                    else {
                        mDeviceView.setReconnectButtonEnabled(true);
                    }
                }
                else {
                    String deviceAddress = device.readParam(ParameterName.Address);
                    deviceText = String.format("Connected to [%s]", deviceAddress);
                    mDeviceView.setReconnectButtonEnabled(true);
                }
                mDeviceView.setDeviceText(deviceText);
                mDeviceView.setProgressVisible(isScanning);
            }
        });
        
        mDeviceModel.bluetoothAdapterEnableNeeded.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mDeviceView.enableBluetooth();
            }
        });
        
        mDeviceModel.bluetoothPermissionsNeeded.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mDeviceView.requestPermissions();
            }
        });
        
        mDeviceModel.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, Device device) {
                mChannelsModel.setDevice(device);
                String deviceAddress = device.readParam(ParameterName.Address);
                mDeviceView.setDeviceText(String.format("Connected to [%s]", deviceAddress));
                mDeviceView.setReconnectButtonEnabled(true);
            }
        });
        
        mDeviceModel.findDevice();
    }
    
    public void onReconnectClicked() {
       // mChannelsModel.stopCalculations();
        mChannelsModel.removeDevice();
        mDeviceModel.findDevice();
    }
}
