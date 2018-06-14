package com.neuromd.stateandbattery;

import android.graphics.Color;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

public class DeviceItem {

    private final Device mDevice;
    private String mName = "Device";
    private String mAddress = "Address";
    private String mStateString = "Disconnected";
    private int mColor = Color.BLACK;
    private int mBatteryLevel = 0;

    public SubscribersNotifier deviceParamsChanged = new SubscribersNotifier();

    public DeviceItem(Device device){
        mDevice = device;
        parseNameAndColor();
        parseAddress();
        subscribeDeviceState();
    }

    public String name(){
        return mName;
    }

    public String address(){
        return mAddress;
    }

    public String state(){
        return mStateString;
    }

    public int charge(){
        return mBatteryLevel;
    }

    public int color(){
        return mColor;
    }

    public void close(){
        mDevice.disconnect();
    }

    private void parseNameAndColor(){
        String rawDeviceName = mDevice != null ? (String)mDevice.readParam(ParameterName.Name) : null;
        String deviceName = rawDeviceName;
        int deviceColor = Color.BLACK;
        if (rawDeviceName!=null) {
            switch (rawDeviceName) {
                case "Neurotech_Colibri_R":
                case "Neurotech_Callibri_R":
                case "Callibri_Red":{
                    deviceName = "Callibri Red";
                    deviceColor = Color.RED;
                    break;
                }
                case "Neurotech_Colibri_B":
                case "Neurotech_Callibri_B":
                case "Callibri_Blue": {
                    deviceName = "Callibri Blue";
                    deviceColor = Color.BLUE;
                    break;
                }
                case "Neurotech_Colibri_Y":
                case "Neurotech_Callibri_Y":
                case "Callibri_Yellow":{
                    deviceName = "Callibri Yellow";
                    deviceColor = Color.YELLOW;
                    break;
                }
                case "Neurotech_Colibri_W":
                case "Neurotech_Callibri_W":
                case "Callibri_White":{
                    deviceName = "Callibri White";
                    deviceColor = Color.WHITE;
                    break;
                }
            }
        }

        if (deviceName == null || deviceName.isEmpty()) {
            deviceName = "null";
        }

        mName = deviceName;
        mColor = deviceColor;
    }

    private void parseAddress(){
        String deviceAddress = mDevice != null ? (String)mDevice.readParam(ParameterName.Address) : null;
        if (deviceAddress == null || deviceAddress.isEmpty()) {
            deviceAddress = "null";
        }
        mAddress = deviceAddress;
    }

    private void subscribeDeviceState(){
        DeviceState state = mDevice.readParam(ParameterName.State);
        mStateString = state.toString();
        mDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
            @Override
            public void onNotify(Object o, ParameterName parameterName) {
                if (parameterName == ParameterName.State) {
                    DeviceState state = mDevice.readParam(ParameterName.State);
                    mStateString = state.toString();
                    deviceParamsChanged.sendNotification(this, null);
                    if (state == DeviceState.Connected){
                        subscribeBatteryLevel();
                    }
                }
            }
        });
        if (state != DeviceState.Connected){
            mDevice.connect();
        }
        else{
            subscribeBatteryLevel();
        }
    }

    private void subscribeBatteryLevel(){
        final BatteryChannel batteryChannel = new BatteryChannel(mDevice);
        batteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
            @Override
            public void onNotify(Object sender, Long length) {
                mBatteryLevel = batteryChannel.readData(length - 1, 1)[0];
                deviceParamsChanged.sendNotification(this, null);
            }
        });
    }
}
