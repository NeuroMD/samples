package ru.neurotech.ecgsample;

import android.app.Activity;

import java.util.List;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.ecg.EcgDevice;
import ru.neurotech.neurodevices.state.NeuroDeviceState;

public class MainActivityPresenter {

    private Activity mActivity;
    private EcgDeviceModel mModel;

    private String mBatteryStateText = "0 %";
    private String mDeviceStateText = "UNKNOWN";
    private String mDeviceErrorText = "NO_ERROR";
    private String mHpfStateText = "OFF";
    private String mSamplingFrequencyText = "0 HZ";
    private String mGainText = "0";
    private String mOffsetText = "0";
    private String mChannelsText = "0";

    private boolean mStartScanButtonEnabled = false;
    private boolean mStopScanButtonEnabled = false;
    private boolean mStartSignalButtonEnabled = false;
    private boolean mStopSignalButtonEnabled = false;
    private boolean mRemoveDeviceButtonEnabled = false;

    public MainActivityPresenter(final Activity activity, final EcgDeviceModel model){
        mActivity = activity;
        mModel = model;
        mModel.scanStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean scanState) {
                onScanStateChanged(scanState);
            }
        });
        mModel.deviceListChanged.subscribe(new INotificationCallback<List<EcgDevice>>() {
            @Override
            public void onNotify(Object sender, List<EcgDevice> ecgDevices) {
                onDeviceListChanged(ecgDevices);
            }
        });

        mModel.selectedDeviceChanged.subscribe(new INotificationCallback<EcgDevice>() {
            @Override
            public void onNotify(Object sender, EcgDevice selectedDevice) {
                if (selectedDevice == null){
                    mStartSignalButtonEnabled = false;
                    notifyStartSignalButtonEnabledChanged();
                    mStopSignalButtonEnabled = false;
                    notifyStopSignalButtonEnabledChanged();
                    mRemoveDeviceButtonEnabled = false;
                    notifyRemoveDeviceButtonEnabledChanged();
                    setDefaultDeviceStateLabels();
                }
                else{
                    mStartSignalButtonEnabled = true;
                    notifyStartSignalButtonEnabledChanged();
                    mStopSignalButtonEnabled = true;
                    notifyStopSignalButtonEnabledChanged();
                    mRemoveDeviceButtonEnabled = true;
                    notifyRemoveDeviceButtonEnabledChanged();
                    mBatteryStateText = String.format("%s%%", mModel.getBatteryLevel());
                    notifyBatteryStateTextChanged();
                    mDeviceStateText = mModel.getDeviceState().name();
                    notifyDeviceStateTextChanged();
                    mDeviceErrorText = mModel.getError().name();
                    notifyDeviceErrorTextChanged();
                    mHpfStateText = mModel.isHpfEnabled() ? "ON" : "OFF";
                    notifyHpfStateTextChanged();
                    mSamplingFrequencyText = String.format("%d HZ", mModel.getSamplingFrequency());
                    notifySamplingFrequencyTextChanged();
                    mGainText = String.valueOf(mModel.getGain());
                    notifyGainTextChanged();
                    mOffsetText = String.valueOf(mModel.getOffset());
                    notifyOffsetTextChanged();
                    mChannelsText = String.valueOf(mModel.getChannelsCount());
                    notifyChannelsTextChanged();
                }
            }
        });

        mModel.deviceStateChanged.subscribe(new INotificationCallback<NeuroDeviceState>() {
            @Override
            public void onNotify(Object o, NeuroDeviceState neuroDeviceState) {
                mDeviceStateText = mModel.getDeviceState().name();
                notifyDeviceStateTextChanged();
            }
        });

        mModel.batteryStateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer integer) {
                mBatteryStateText = String.format("%s%%", mModel.getBatteryLevel());
                notifyBatteryStateTextChanged();
            }
        });

        mStartScanButtonEnabled = true;
    }

    /**
     * Controls enabled change events
     */
    public SubscribersNotifier<Boolean> startScanButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> stopScanButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> startSignalButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> stopSignalButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> removeDeviceButtonEnabledChanged = new SubscribersNotifier<>();

    /**
     * DeviceList events
     */
    public SubscribersNotifier<List<EcgDevice>> deviceListChanged = new SubscribersNotifier<>();

    /**
     * Device parameters events
     */
    public SubscribersNotifier<String> batteryStateTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> deviceStateTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> deviceErrorTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> hpfStateTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> samplingFrequencyTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> gainTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> offsetTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> channelsTextChanged = new SubscribersNotifier<>();


    /**
     * Getters
     */
    public List<EcgDevice> getDeviceList(){
        return mModel.getDeviceList();
    }
    public boolean isStartScanButtonEnabled() {
        return mStartScanButtonEnabled;
    }

    public boolean isStopScanButtonEnabled() {
        return mStopScanButtonEnabled;
    }

    public boolean isStartSignalButtonEnabled() {
        return mStartSignalButtonEnabled;
    }

    public boolean isStopSignalButtonEnabled() {
        return mStopSignalButtonEnabled;
    }

    public boolean isRemoveDeviceButtonEnabled() {
        return mRemoveDeviceButtonEnabled;
    }

    /**
     * Controls click handlers
     */

    public void onStartScanButtonClick(){
        mModel.startScan();
    }

    public void onStopScanButtonClick(){
        mModel.stopScan();
    }

    public void onStartSignalButtonClick(){
        mModel.signalStart();
    }

    public void onStopSignalButtonClick(){
        mModel.signalStop();
    }

    public void onRemoveDeviceButtonClick(){
        mModel.removeDevice();
    }

    public void onDeviceListItemSelected(EcgDevice device) {
        mModel.selectDevice(device);
    }


    /**
     * Event handlers
     */

    private void onScanStateChanged(boolean isScanning){
        mStartScanButtonEnabled = !isScanning;
        mStopScanButtonEnabled = isScanning;
        notifyStartScanButtonEnabledChanged();
        notifyStopScanButtonEnabledChanged();
    }

    private void onDeviceListChanged(final List<EcgDevice> ecgDevices) {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                deviceListChanged.sendNotification(this, ecgDevices);
            }
        });
    }


    /**
     * Controls state changed notifiers
     */

    private void notifyStartScanButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                startScanButtonEnabledChanged.sendNotification(null, mStartScanButtonEnabled);
            }
        });
    }

    private void notifyStopScanButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                stopScanButtonEnabledChanged.sendNotification(null, mStopScanButtonEnabled);
            }
        });
    }

    private void notifyStartSignalButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                startSignalButtonEnabledChanged.sendNotification(null, mStartSignalButtonEnabled);
            }
        });
    }

    private void notifyStopSignalButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                stopSignalButtonEnabledChanged.sendNotification(null, mStopSignalButtonEnabled);
            }
        });
    }

    private void notifyRemoveDeviceButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                removeDeviceButtonEnabledChanged.sendNotification(null, mRemoveDeviceButtonEnabled);
            }
        });
    }

    private void notifyBatteryStateTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                batteryStateTextChanged.sendNotification(null, mBatteryStateText);
            }
        });
    }

    private void notifyDeviceStateTextChanged() {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                deviceStateTextChanged.sendNotification(null, mDeviceStateText);
            }
        });
    }

    private void notifyDeviceErrorTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                deviceErrorTextChanged.sendNotification(null, mDeviceErrorText);
            }
        });
    }

    private void notifyHpfStateTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                hpfStateTextChanged.sendNotification(null, mHpfStateText);
            }
        });
    }

    private void notifySamplingFrequencyTextChanged() {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                samplingFrequencyTextChanged.sendNotification(null, mSamplingFrequencyText);
            }
        });
    }

    private void notifyGainTextChanged() {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                gainTextChanged.sendNotification(null, mGainText);
            }
        });
    }

    private void notifyOffsetTextChanged() {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                offsetTextChanged.sendNotification(null, mOffsetText);
            }
        });
    }

    private void notifyChannelsTextChanged() {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                channelsTextChanged.sendNotification(null, mChannelsText);
            }
        });
    }

    /**
     * Setters of labels text
     */

    private void setDefaultDeviceStateLabels(){

        mBatteryStateText = "0 %";
        notifyBatteryStateTextChanged();
        mDeviceStateText = "UNKNOWN";
        notifyDeviceStateTextChanged();
        mDeviceErrorText = "NO_ERROR";
        notifyDeviceErrorTextChanged();
        mHpfStateText = "OFF";
        notifyHpfStateTextChanged();
        mSamplingFrequencyText = "0 HZ";
        notifySamplingFrequencyTextChanged();
        mGainText = "0";
        notifyGainTextChanged();
        mOffsetText = "0";
        notifyOffsetTextChanged();
        mChannelsText = "0";
        notifyChannelsTextChanged();
    }
}
