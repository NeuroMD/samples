package com.neuromd.ecgsample;

import android.content.Context;
import android.util.Log;

import com.neuromd.bleconnection.exceptions.BluetoothAdapterException;
import com.neuromd.bleconnection.exceptions.BluetoothPermissionException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.ElectrodeState;
import com.neuromd.neurosdk.channels.ElectrodesStateChannel;
import com.neuromd.neurosdk.channels.SignalChannel;
import com.neuromd.neurosdk.channels.ecg.EcgChannel;
import com.neuromd.neurosdk.channels.ecg.RPeakChannel;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;
import com.neuromd.neurosdk.parameters.types.SamplingFrequency;

import java.util.ArrayList;
import java.util.List;


public class EcgDeviceModel {

    private final DeviceScanner mDeviceConnector;
    private final List<Device> mDeviceList = new ArrayList<>();
    private Device mSelectedDevice = null;
    private EcgChannel mEcgChannel = null;
    private RPeakChannel mRChannel = null;
    private BatteryChannel mBatteryChannel = null;
    private ElectrodesStateChannel mElectrodesStateChannel = null;
    private DeviceState mDeviceState;
    private int mBatteryLevel;
    private boolean mHpfEnabled;
    private SamplingFrequency mSamplingFrequency;
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
    public SubscribersNotifier<SamplingFrequency> samplingFrequencyChanged = new SubscribersNotifier<>();
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
            
            mSamplingFrequency = (SamplingFrequency)mSelectedDevice.readParam(ParameterName.SamplingFrequency);
            if (mSamplingFrequency != SamplingFrequency.Hz125){
                mSelectedDevice.setParam(ParameterName.SamplingFrequency, SamplingFrequency.Hz125);
            }
            mSamplingFrequency = (SamplingFrequency)mSelectedDevice.readParam(ParameterName.SamplingFrequency);
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);
            
            String deviceName = mSelectedDevice.readParam(ParameterName.Name);
            if (deviceName.equals("BrainBit")){
                ChannelInfo[] channelInfo = mSelectedDevice.channels();
                
                for (ChannelInfo info : channelInfo){
                    if (info.getType() == ChannelType.Signal) {
                        Log.d("CreateChannel", String.format("Creating channel %s", info.getName()));
                        SignalChannel signalChannel = new SignalChannel(mSelectedDevice, info);
                        mEcgChannel = new EcgChannel(signalChannel);
                        mRChannel = new RPeakChannel(signalChannel);
                        break;
                    }
                }
            }
            else{
                mSelectedDevice.execute(Command.FindMe);
                SignalChannel signalChannel = new SignalChannel(mSelectedDevice);
                mEcgChannel = new EcgChannel(signalChannel);
                mRChannel = new RPeakChannel(signalChannel);
            }
            mEcgChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (mEcgChannel == null)
                        return;
                    mSignalDuration = (double)mEcgChannel.totalLength() / mEcgChannel.samplingFrequency();
                }
            });

            mBatteryChannel = new BatteryChannel(mSelectedDevice);
            mBatteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    //Log.d("BatteryChannel", String.format("Length changed %d", length));
                    //if (length > 0) {
                    if (mBatteryChannel == null)
                        return;
                        long len = mBatteryChannel.totalLength();
                        long offset =  len - 1;
                        Integer[] data = mBatteryChannel.readData(offset, 1);
                        mBatteryLevel = data[0];
                        batteryStateChanged.sendNotification(this, mBatteryLevel);
                    //}
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
            mBatteryChannel.setSamplingFrequency(0.3f);
    
            mElectrodesStateChannel = new ElectrodesStateChannel(mSelectedDevice);
            mElectrodesStateChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (mElectrodesStateChannel == null)
                        return;
                    long len = mElectrodesStateChannel.totalLength();
                    Log.d("ElectrodesStateChannel", String.format("Electrode length changed %d", len));
                    if (len > 0){
                        long offset = len - 1;
                        ElectrodeState[] data = mElectrodesStateChannel.readData(offset, 1);
                        mIsElectrodesAttached = data[0] == ElectrodeState.Normal;
                        electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
                    }
                }
            });
            long electrodesStateDataLength = mElectrodesStateChannel.totalLength();
            if (electrodesStateDataLength > 0) {
                mIsElectrodesAttached = mElectrodesStateChannel.readData(electrodesStateDataLength - 1, 1)[0]== ElectrodeState.Normal;
            }
            else{
                mIsElectrodesAttached = false;
            }
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
            mElectrodesStateChannel.setSamplingFrequency(2f);
            
            mDeviceState = (DeviceState)mSelectedDevice.readParam(ParameterName.State);
            deviceStateChanged.sendNotification(this, mDeviceState);

            /*mHpfEnabled = mSelectedDevice.getCallibriDevice().getNeuroDevice().getSignalSubsystem().getHpfEnabled();
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);*/

            

            /*mGain = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getGain();
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
            mElectrodesStateChannel = null;

            mBatteryLevel = 0;
            batteryStateChanged.sendNotification(this, mBatteryLevel);

            mDeviceState = DeviceState.Disconnected;
            deviceStateChanged.sendNotification(this, mDeviceState);

            mHpfEnabled = false;
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);

            mSamplingFrequency = SamplingFrequency.Hz1000;
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
        switch (mSamplingFrequency) {
            case Hz125: return 125;
            case Hz250: return 250;
            case Hz500: return 500;
            case Hz1000: return 1000;
            case Hz2000: return 2000;
            case Hz4000: return 4000;
            case Hz8000: return 8000;
            default: return 0;
        }
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

        if (length <=0 )
            return new Double[0];

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

    public Long[] getRPeaks(double startTime, double endTime){
        if (mRChannel == null) {
            return null;
        }
        if (startTime < 0){
            startTime = 0;
        }
        if (endTime > getTotalDuration()){
            endTime = getTotalDuration();
        }
    
        float samplingFreq = mEcgChannel.samplingFrequency();
        int offset = (int)(startTime * samplingFreq);
        int length = (int)((endTime-startTime) * samplingFreq);
        
        return mRChannel.readData(offset, length);
    }

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
