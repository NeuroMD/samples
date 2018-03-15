package ru.neurotech.simplebiofeedback;

import android.content.Context;

import java.util.ArrayList;
import java.util.List;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.bfb.BfbDevice;
import ru.neurotech.neurodevices.bfb.BfbDeviceConnector;
import ru.neurotech.neurodevices.bfb.BfbIndex;
import ru.neurotech.neurodevices.exceptions.BluetoothAdapterException;
import ru.neurotech.neurodevices.exceptions.BluetoothPermissionException;
import ru.neurotech.neurodevices.state.NeuroDeviceError;
import ru.neurotech.neurodevices.state.NeuroDeviceState;

public class BfbDeviceModel {

    private final BfbDeviceConnector mDeviceConnector;
    private final List<BfbDevice> mDeviceList = new ArrayList<>();
    private BfbDevice mSelectedDevice = null;
    private BfbIndex mSelectedIndex = null;
    private NeuroDeviceState mDeviceState;
    private NeuroDeviceError mDeviceError;
    private int mBatteryLevel;
    private boolean mHpfEnabled;
    private int mSamplingFrequency;
    private int mGain;
    private int mOffset;
    private int mChannelsCount;

    public BfbDeviceModel(Context context){

        mDeviceConnector = new BfbDeviceConnector(context);
        mDeviceConnector.deviceFound.subscribe(new INotificationCallback<BfbDevice>() {
            @Override
            public void onNotify(Object o, BfbDevice bfbDevice) {
                    mDeviceList.add(bfbDevice);
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
    public SubscribersNotifier<List<BfbDevice>> deviceListChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<BfbDevice> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<BfbIndex> selectedIndexChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<NeuroDeviceState> deviceStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<NeuroDeviceError> deviceErrorChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> hpfEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> samplingFrequencyChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> gainChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> offsetChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> channelsCountChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> indexValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier calibrationFinished = new SubscribersNotifier();
    public SubscribersNotifier calibrationStarted = new SubscribersNotifier();

    public List<BfbDevice> getDeviceList(){
        return  mDeviceList;
    }

    public void startScan(){

        removeDevice();
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

    public void selectDevice(BfbDevice device){

        if (mSelectedDevice!=null){
            mSelectedDevice.batteryLevelChanged.unsubscribe();
            mSelectedDevice.deviceStateChanged.unsubscribe();
        }

        mSelectedDevice = device;
        removeIndex();

        if (mSelectedDevice!=null) {
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
        }
        else
        {
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
        }
        selectedDeviceChanged.sendNotification(this, mSelectedDevice);
    }

    public void createIndex(int frequencyStart, int frequencyStop, double window, int overlapping){
        if (mSelectedDevice == null)
            return;

        if (mSelectedIndex!=null){
            removeIndex();
        }

        mSelectedIndex = mSelectedDevice.createIndex(frequencyStart, frequencyStop, window, overlapping);
        mSelectedIndex.valueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                indexValueChanged.sendNotification(this, value);
            }
        });
        mSelectedIndex.calibrationFinished.subscribe(new INotificationCallback<Object>() {
            @Override
            public void onNotify(Object o, Object o2) {
                calibrationFinished.sendNotification(this, null);
            }
        });
        selectedIndexChanged.sendNotification(this, mSelectedIndex);
    }

    public void removeIndex(){
        if (mSelectedIndex!=null) {
            mSelectedIndex.calibrationFinished.unsubscribe();
            mSelectedIndex.valueChanged.unsubscribe();
        }
        mSelectedIndex = null;
        selectedIndexChanged.sendNotification(this, mSelectedIndex);
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
        mDeviceList.remove(mSelectedDevice);
        deviceListChanged.sendNotification(this, mDeviceList);
        selectDevice(null);
        if (mSelectedIndex!=null){
            removeIndex();
        }
    }

    public void calibrate() {
        if (mSelectedIndex == null)
            return;

        mSelectedIndex.calibrate();
        calibrationStarted.sendNotification(this, null);
    }

    public BfbDevice getSelectedDevice(){
        return mSelectedDevice;
    }

    public BfbIndex getSelectedIndex(){
        return mSelectedIndex;
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
}
