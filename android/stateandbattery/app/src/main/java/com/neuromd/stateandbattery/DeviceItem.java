package com.neuromd.stateandbattery;

import android.graphics.Color;
import android.support.annotation.NonNull;

import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.parameters.ParameterName;

public class DeviceItem {

    private final Device mDevice;
    private String mName = "Device";
    private String mAddress = "Address";
    private String mStateString = "Disconnected";
    private int mColor = Color.BLACK;
    private int mBatteryLevel = 0;

    public DeviceItem(Device device){
        mDevice = device;
        parseNameAndColor();
        parseAddress();
        subscribeDeviceState();
        subscribeBatteryLevel();
        device.connect();
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

    private void syncBatteryLevel(){

    }
}
