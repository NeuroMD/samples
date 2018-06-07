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
    private boolean mDeviceFound;
    private boolean mPermissionsRequested;
    
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
                    if (mDeviceConnector != null) {
                        mDeviceConnector.stopScan();
                        mDeviceFound = true;
                        deviceFound.sendNotification(this, device);
                    }
                }
                else {
                    device.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                        @Override
                        public void onNotify(Object o, ParameterName parameterName) {
                            if (parameterName == ParameterName.State) {
                                DeviceState state = device.readParam(ParameterName.State);
                                if (state == DeviceState.Connected) {
                                    device.parameterChanged.unsubscribe();
                                    if (mDeviceConnector != null) {
                                        mDeviceConnector.stopScan();
                                        mDeviceFound = true;
                                        deviceFound.sendNotification(this, device);
                                    }
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
            public void onNotify(Object o, Boolean isScanning) {
                if (!isScanning && !mDeviceFound && !mPermissionsRequested) {
                    findDevice();
                }
                else{
                    scanStateChanged.sendNotification(this, isScanning);
                }
            }
        });
    }
    
    public SubscribersNotifier bluetoothAdapterEnableNeeded = new SubscribersNotifier<>();
    public SubscribersNotifier bluetoothPermissionsNeeded = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> scanStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Device> deviceFound = new SubscribersNotifier<>();
    
    public void findDevice() {
        try {
            mDeviceFound = false;
            mPermissionsRequested = false;
            mDeviceConnector.startScan(5000);
        }
        catch (BluetoothAdapterException e) {
            mDeviceConnector.stopScan();
            mPermissionsRequested = true;
            bluetoothAdapterEnableNeeded.sendNotification(this, null);
        }
        catch (BluetoothPermissionException e) {
            mDeviceConnector.stopScan();
            mPermissionsRequested = true;
            bluetoothPermissionsNeeded.sendNotification(this, null);
        }
        catch (Exception e){
            Log.e("EegDeviceModel", String.format("Scanning failed with exception: %s", e.getMessage()));
        }
    }
}
