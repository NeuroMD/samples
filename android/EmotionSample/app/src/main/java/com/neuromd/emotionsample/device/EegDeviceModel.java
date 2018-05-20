package com.neuromd.emotionsample.device;

import android.content.Context;
import android.util.Log;

import com.neuromd.bleconnection.exceptions.BluetoothAdapterException;
import com.neuromd.bleconnection.exceptions.BluetoothPermissionException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

public class EegDeviceModel {
    private final DeviceScanner mDeviceConnector;
    
    public EegDeviceModel(Context context) {
        mDeviceConnector = new DeviceScanner(context);
        mDeviceConnector.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, final Device device) {
                String deviceName = device.readParam(ParameterName.Name);
                if (!deviceName.equals("BrainBit") && !deviceName.equals("Brainbit")){
                    Log.d("DeviceModel", String.format("Skip device %s", deviceName));
                    return;
                }
                
                DeviceState state = device.readParam(ParameterName.State);
                if (state == DeviceState.Connected) {
                    mDeviceConnector.stopScan();
                    deviceFound.sendNotification(this, device);
                }
                else {
                    device.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                        @Override
                        public void onNotify(Object o, ParameterName parameterName) {
                            if (parameterName == ParameterName.State) {
                                DeviceState state = device.readParam(ParameterName.State);
                                if (state == DeviceState.Connected) {
                                    device.parameterChanged.unsubscribe();
                                    mDeviceConnector.stopScan();
                                    deviceFound.sendNotification(this, device);
                                }
                            }
                        }
                    });
                    device.connect();
                }
            }
        });
        mDeviceConnector.scanStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean state) {
                scanStateChanged.sendNotification(this, state);
            }
        });
    }
    
    public SubscribersNotifier bluetoothAdapterEnableNeeded = new SubscribersNotifier<>();
    public SubscribersNotifier bluetoothPermissionsNeeded = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> scanStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Device> deviceFound = new SubscribersNotifier<>();
    
    public void findDevice() {
        try {
            mDeviceConnector.startScan(0);
        }
        catch (BluetoothAdapterException e) {
            mDeviceConnector.stopScan();
            bluetoothAdapterEnableNeeded.sendNotification(this, null);
        }
        catch (BluetoothPermissionException e) {
            mDeviceConnector.stopScan();
            bluetoothPermissionsNeeded.sendNotification(this, null);
        }
    }
}
