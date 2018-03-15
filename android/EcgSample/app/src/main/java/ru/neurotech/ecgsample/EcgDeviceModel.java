package ru.neurotech.ecgsample;

import android.content.Context;

import java.util.ArrayList;
import java.util.List;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.ArtifactZone;
import ru.neurotech.neurodevices.ecg.EcgDevice;
import ru.neurotech.neurodevices.ecg.EcgDeviceConnector;
import ru.neurotech.neurodevices.ecg.RPeak;
import ru.neurotech.neurodevices.exceptions.BluetoothAdapterException;
import ru.neurotech.neurodevices.exceptions.BluetoothPermissionException;
import ru.neurotech.neurodevices.features.Channel;
import ru.neurotech.neurodevices.state.NeuroDeviceError;
import ru.neurotech.neurodevices.state.NeuroDeviceState;

public class EcgDeviceModel {

    private final EcgDeviceConnector mDeviceConnector;
    private final List<EcgDevice> mDeviceList = new ArrayList<>();
    private EcgDevice mSelectedDevice = null;
    private Channel mEcgChannel = null;
    private NeuroDeviceState mDeviceState;
    private NeuroDeviceError mDeviceError;
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

        mDeviceConnector = new EcgDeviceConnector(context);
        mDeviceConnector.deviceFound.subscribe(new INotificationCallback<EcgDevice>() {
            @Override
            public void onNotify(Object o, EcgDevice ecgDevice) {
                    mDeviceList.add(ecgDevice);
                    deviceListChanged.sendNotification(this, mDeviceList);
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
    public SubscribersNotifier<List<EcgDevice>> deviceListChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<EcgDevice> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<NeuroDeviceState> deviceStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<NeuroDeviceError> deviceErrorChanged = new SubscribersNotifier<>();
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

    public List<EcgDevice> getDeviceList(){
        return  mDeviceList;
    }

    public void startScan(){

        removeDevice();
        for (EcgDevice device : mDeviceList){
            device.getNeuroDevice().disconnect();
            device.getNeuroDevice().close();
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

    public void selectDevice(EcgDevice device){

        if (mSelectedDevice!=null){
            mSelectedDevice.batteryLevelChanged.unsubscribe();
            mSelectedDevice.deviceStateChanged.unsubscribe();
            mSelectedDevice.electrodesAttachedStateChanged.unsubscribe();
            mSelectedDevice.totalSignalDurationChanged.unsubscribe();
            mSelectedDevice.heartRateChanged.unsubscribe();
            mSelectedDevice.stressIndexChanged.unsubscribe();
        }

        mSelectedDevice = device;

        if (mSelectedDevice!=null) {
            mEcgChannel = mSelectedDevice.getNeuroDevice().getSignalSubsystem().getChannels()[0];

            mSelectedDevice.deviceStateChanged.subscribe(new INotificationCallback<NeuroDeviceState>() {
                @Override
                public void onNotify(Object o, NeuroDeviceState neuroDeviceState) {
                    mDeviceState = neuroDeviceState;
                    deviceStateChanged.sendNotification(this, mDeviceState);
                }
            });
            mSelectedDevice.batteryLevelChanged.subscribe(new INotificationCallback<Integer>() {
                @Override
                public void onNotify(Object o, Integer level) {
                    mBatteryLevel = level;
                    batteryStateChanged.sendNotification(this, mBatteryLevel);
                }
            });
            mSelectedDevice.electrodesAttachedStateChanged.subscribe(new INotificationCallback<Boolean>() {
                @Override
                public void onNotify(Object o, Boolean isAttached) {
                    mIsElectrodesAttached = isAttached;
                    electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
                }
            });
            mSelectedDevice.totalSignalDurationChanged.subscribe(new INotificationCallback<Double>() {
                @Override
                public void onNotify(Object o, Double duration) {
                    mSignalDuration = duration;
                    signalDurationChanged.sendNotification(this, mSignalDuration);
                }
            });
            mSelectedDevice.heartRateChanged.subscribe(new INotificationCallback<Integer>() {
                @Override
                public void onNotify(Object o, Integer heartRate) {
                    mHeartRate = heartRate;
                    heartRateChanged.sendNotification(this, mHeartRate);
                }
            });
            mSelectedDevice.stressIndexChanged.subscribe(new INotificationCallback<Double>() {
                @Override
                public void onNotify(Object o, Double stressIndex) {
                    mStressIndex = stressIndex;
                    stressIndexChanged.sendNotification(this, mStressIndex);
                }
            });

            mBatteryLevel = mSelectedDevice.getBatteryLevel();
            batteryStateChanged.sendNotification(this, mBatteryLevel);

            mDeviceState = mSelectedDevice.getNeuroDevice().getState();
            deviceStateChanged.sendNotification(this, mDeviceState);

            mDeviceError = mSelectedDevice.getNeuroDevice().getError();
            deviceErrorChanged.sendNotification(this, mDeviceError);

            //mHpfEnabled = mSelectedDevice.getCallibriDevice().getNeuroDevice().getSignalSubsystem().getHpfEnabled();
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
            stressIndexChanged.sendNotification(this, mStressIndex);

            mSignalDuration = mSelectedDevice.getTotalSignalDuration();
            signalDurationChanged.sendNotification(this, mSignalDuration);

            mIsElectrodesAttached = mSelectedDevice.getElectrodesState();
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
        }
        else
        {
            mEcgChannel = null;

            mBatteryLevel = 0;
            batteryStateChanged.sendNotification(this, mBatteryLevel);

            mDeviceState = NeuroDeviceState.UNKNOWN;
            deviceStateChanged.sendNotification(this, mDeviceState);

            mDeviceError = NeuroDeviceError.NO_ERROR;
            deviceErrorChanged.sendNotification(this, mDeviceError);

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

        mSelectedDevice.startReceive();
    }

    public void signalStop() {
        if (mSelectedDevice==null)
            return;

        mSelectedDevice.stopReceive();
    }

    public void removeDevice() {
        if (mSelectedDevice == null)
            return;

        mSelectedDevice.getNeuroDevice().disconnect();
        mSelectedDevice.getNeuroDevice().close();
        mDeviceList.remove(mSelectedDevice);
        deviceListChanged.sendNotification(this, mDeviceList);
        selectDevice(null);
    }

    public EcgDevice getSelectedDevice(){
        return mSelectedDevice;
    }

    public NeuroDeviceState getDeviceState(){
        return mDeviceState;
    }

    public NeuroDeviceError getError(){
        return mDeviceError;
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

    public double[] getSignalData(double time, double duration){
        if (mSelectedDevice == null) {
            return null;
        }

        if (time < 0){
            time = 0;
            duration = getTotalDuration();
        }
        return mSelectedDevice.getEcgSignal(time, duration);
    }

    public double[] getRawSignal(int offset, int length){
        if (mSelectedDevice == null){
            return null;
        }

        return mEcgChannel.getRawData(offset, length);
    }

    public double getTotalDuration(){
        if (mSelectedDevice == null) {
            return 0;
        }

        return mSelectedDevice.getTotalSignalDuration();
    }

    public RPeak[] getRPeaks(double startTime, double endTime){
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
    }

    public ArtifactZone[] getArtifacts(double time, double duration){
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
    }
}
