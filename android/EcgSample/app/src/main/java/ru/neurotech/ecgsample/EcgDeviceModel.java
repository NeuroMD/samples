package ru.neurotech.ecgsample;

import android.content.Context;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import ru.neurotech.bleconnection.exceptions.BluetoothAdapterException;
import ru.neurotech.bleconnection.exceptions.BluetoothPermissionException;
import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurosdk.Device;
import ru.neurotech.neurosdk.DeviceScanner;
import ru.neurotech.neurosdk.channels.BatteryChannel;
import ru.neurotech.neurosdk.channels.ChannelInfo;
import ru.neurotech.neurosdk.channels.SignalChannel;
import ru.neurotech.neurosdk.parameters.Command;
import ru.neurotech.neurosdk.parameters.ParameterName;
import ru.neurotech.neurosdk.parameters.types.DeviceState;

public class EcgDeviceModel {

    private final DeviceScanner mDeviceConnector;
    private final List<Device> mDeviceList = new ArrayList<>();
    private Device mSelectedDevice = null;
    private SignalChannel mEcgChannel = null;
    private BatteryChannel mBatteryChannel = null;
    private DeviceState mDeviceState;
    private int mBatteryLevel;
    private boolean mHpfEnabled;
    private int mSamplingFrequency;
    private int mGain;
    private int mOffset;
    private int mChannelsCount;
    private int mHeartRate;
    private boolean mIsElectrodesAttached;
    private double mStressIndex;
    private double mSignalDuration;

    public EcgDeviceModel(Context context){

        mDeviceConnector = new DeviceScanner(context);
        mDeviceConnector.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, final Device device) {
                    DeviceState state = (DeviceState)device.readParam(ParameterName.State);
                    if (state == DeviceState.Connected) {
                        /*ChannelInfo[] channels = device.channels();
                        Arrays.asList(channels).contains(ChannelInfo)*/
                        mDeviceList.add(device);
                        deviceListChanged.sendNotification(this, mDeviceList);
                    }else{
                        device.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                            @Override
                            public void onNotify(Object o, ParameterName parameterName) {
                                if (parameterName == ParameterName.State){
                                    DeviceState state = (DeviceState)device.readParam(ParameterName.State);
                                    if (state == DeviceState.Connected) {
                                        device.parameterChanged.unsubscribe();
                                        mDeviceList.add(device);
                                        deviceListChanged.sendNotification(this, mDeviceList);
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
    public SubscribersNotifier<List<Device>> deviceListChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<DeviceState> deviceStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> hpfEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> samplingFrequencyChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> gainChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> offsetChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> channelsCountChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> heartRateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> electrodesStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> stressIndexChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> signalDurationChanged = new SubscribersNotifier<>();

    public List<Device> getDeviceList(){
        return  mDeviceList;
    }

    public void startScan(){

        removeDevice();
        for (Device device : mDeviceList){
            device.disconnect();
        }
        mDeviceList.clear();
        deviceListChanged.sendNotification(this, mDeviceList);
        try {
            mDeviceConnector.startScan(0);
        }
        catch (BluetoothAdapterException e){
            stopScan();
            bluetoothAdapterEnableNeeded.sendNotification(this, null);
        }
        catch (BluetoothPermissionException e){
            stopScan();
            bluetoothPermissionsNeeded.sendNotification(this, null);
        }
    }

    public void stopScan(){
        mDeviceConnector.stopScan();
    }

    public void selectDevice(Device device){

        if (mSelectedDevice!=null){
            mSelectedDevice.parameterChanged.unsubscribe();
        }

        mSelectedDevice = device;

        if (mSelectedDevice!=null) {
            mSelectedDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                @Override
                public void onNotify(Object o, ParameterName paramName) {
                    if (paramName == ParameterName.State) {
                        mDeviceState = (DeviceState)mSelectedDevice.readParam(paramName);
                        deviceStateChanged.sendNotification(this, mDeviceState);
                    }
                }
            });

            mEcgChannel = new SignalChannel(mSelectedDevice);
            mEcgChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    mSignalDuration = (double)mEcgChannel.totalLength() / mEcgChannel.samplingFrequency();
                }
            });

            mBatteryChannel = new BatteryChannel(mSelectedDevice);
            mBatteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (length > 0) {
                        mBatteryLevel = mBatteryChannel.readData(length - 1, 1)[0];
                        batteryStateChanged.sendNotification(this, mBatteryLevel);
                    }
                }
            });
            long batteryDataLength = mBatteryChannel.totalLength();
            if (batteryDataLength > 0) {
                mBatteryLevel = mBatteryChannel.readData(batteryDataLength - 1, 1)[0];
            }
            else{
                mBatteryLevel = 0;
            }
            batteryStateChanged.sendNotification(this, mBatteryLevel);

            mDeviceState = (DeviceState)mSelectedDevice.readParam(ParameterName.State);
            deviceStateChanged.sendNotification(this, mDeviceState);

            /*mHpfEnabled = mSelectedDevice.getCallibriDevice().getNeuroDevice().getSignalSubsystem().getHpfEnabled();
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);

            mSamplingFrequency = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getSamplingFrequency();
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);

            mGain = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getGain();
            gainChanged.sendNotification(this, mGain);

            mOffset = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getSignalOffset();
            offsetChanged.sendNotification(this, mOffset);

            mChannelsCount = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getChannels().length;
            channelsCountChanged.sendNotification(this, mChannelsCount);

            mHeartRate = mSelectedDevice.getCurrentHeartRate();
            heartRateChanged.sendNotification(this, mHeartRate);

            mStressIndex = mSelectedDevice.getCurrentStressIndex();
            stressIndexChanged.sendNotification(this, mStressIndex);*/

            mSignalDuration = (double)mEcgChannel.totalLength() / mEcgChannel.samplingFrequency();
            signalDurationChanged.sendNotification(this, mSignalDuration);

           /* mIsElectrodesAttached = mSelectedDevice.getElectrodesState();
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);*/
        }
        else
        {
            mEcgChannel = null;
            mBatteryChannel = null;

            mBatteryLevel = 0;
            batteryStateChanged.sendNotification(this, mBatteryLevel);

            mDeviceState = DeviceState.Disconnected;
            deviceStateChanged.sendNotification(this, mDeviceState);

            mHpfEnabled = false;
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);

            mSamplingFrequency = 0;
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);

            mGain = 0;
            gainChanged.sendNotification(this, mGain);

            mOffset = 0;
            offsetChanged.sendNotification(this, mOffset);

            mChannelsCount = 0;
            channelsCountChanged.sendNotification(this, mChannelsCount);

            mHeartRate = 0;
            heartRateChanged.sendNotification(this, mHeartRate);

            mStressIndex = 0.0;
            stressIndexChanged.sendNotification(this, mStressIndex);

            mSignalDuration = 0.0;
            signalDurationChanged.sendNotification(this, mSignalDuration);

            mIsElectrodesAttached = false;
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
        }
        selectedDeviceChanged.sendNotification(this, mSelectedDevice);
    }

    public void signalStart() {
        if (mSelectedDevice==null)
            return;

        mSelectedDevice.execute(Command.StartSignal);
    }

    public void signalStop() {
        if (mSelectedDevice==null)
            return;

        mSelectedDevice.execute(Command.StopSignal);
    }

    public void removeDevice() {
        if (mSelectedDevice == null)
            return;
        mSelectedDevice.disconnect();
        mDeviceList.remove(mSelectedDevice);
        deviceListChanged.sendNotification(this, mDeviceList);
        selectDevice(null);
    }

    public Device getSelectedDevice(){
        return mSelectedDevice;
    }

    public DeviceState getDeviceState(){
        return mDeviceState;
    }

    public boolean isHpfEnabled(){
        return mHpfEnabled;
    }

    public int getSamplingFrequency(){
        return mSamplingFrequency;
    }

    public int getGain(){
        return mGain;
    }

    public int getOffset(){
        return mOffset;
    }

    public int getBatteryLevel(){
        return mBatteryLevel;
    }

    public int getChannelsCount(){
        return mChannelsCount;
    }

    public Double[] getSignalData(double time, double duration){
        if (mSelectedDevice == null) {
            return null;
        }

        if (time < 0){
            time = 0;
            duration = getTotalDuration();
        }

        float samplingFreq = mEcgChannel.samplingFrequency();
        int offset = (int)(time * samplingFreq);
        int length = (int)(duration * samplingFreq);

        return getRawSignal(offset, length);
    }

    public Double[] getRawSignal(int offset, int length){
        if (mSelectedDevice == null){
            return null;
        }

        return mEcgChannel.readData(offset, length);
    }

    public double getTotalDuration(){
        if (mSelectedDevice == null) {
            return 0;
        }

        double length = mEcgChannel.totalLength();
        double duration = length / mEcgChannel.samplingFrequency();

        return duration;
    }

   /* public Object[] getRPeaks(double startTime, double endTime){
        if (mSelectedDevice == null) {
            return null;
        }
        if (startTime < 0){
            startTime = 0;
        }
        if (endTime > getTotalDuration()){
            endTime = getTotalDuration();
        }

        return mSelectedDevice.getRWavesOnInterval(startTime, endTime);
        return null;
    }*/

    /*public ArtifactZone[] getArtifacts(double time, double duration){
        if (mSelectedDevice == null) {
            return null;
        }
        if (time < 0){
            time = 0;
        }
        if (time + duration > getTotalDuration()){
            duration = getTotalDuration()-time;
        }

        return mSelectedDevice.getArtifacts(time, duration);
    }*/
}
