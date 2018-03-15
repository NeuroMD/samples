package ru.neurotech.simplebiofeedback;

import android.annotation.SuppressLint;
import android.app.Activity;

import java.util.List;

import ru.neurotech.common.CallibriVisualDevice;
import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.bfb.BfbDevice;
import ru.neurotech.neurodevices.bfb.BfbIndex;
import ru.neurotech.neurodevices.state.NeuroDeviceState;

public class MainActivityPresenter {

    private Activity mActivity;
    private BfbDeviceModel mModel;

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
    private boolean mCreateIndexButtonEnabled = false;
    private boolean mRemoveIndexButtonEnabled = false;
    private boolean mCalibrateButtonEnabled = false;
    private boolean mFrequencyBeginEditTextEnabled = false;
    private boolean mFrequencyEndEditTextEnabled = false;
    private boolean mWindowEditTextEnabled = false;
    private boolean mOverlappingEditTextEnabled = false;

    public MainActivityPresenter(final Activity activity, final BfbDeviceModel model){
        mActivity = activity;
        mModel = model;
        mModel.scanStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean scanState) {
                onScanStateChanged(scanState);
            }
        });
        mModel.deviceListChanged.subscribe(new INotificationCallback<List<BfbDevice>>() {
            @Override
            public void onNotify(Object sender, List<BfbDevice> callibriVisualDevices) {
                onDeviceListChanged(callibriVisualDevices);
            }
        });
        mModel.calibrationFinished.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                if (mModel.getSelectedDevice() != null && mModel.getSelectedIndex()!= null){
                    mCalibrateButtonEnabled = true;
                }else{
                    mCalibrateButtonEnabled = false;
                }
                notifyCalibrationButtonEnabledChanged();
            }
        });
        mModel.calibrationStarted.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mCalibrateButtonEnabled = false;
                notifyCalibrationButtonEnabledChanged();
            }
        });
        mModel.selectedDeviceChanged.subscribe(new INotificationCallback<BfbDevice>() {
            @Override
            public void onNotify(Object sender, BfbDevice selectedDevice) {
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
        mModel.selectedIndexChanged.subscribe(new INotificationCallback<BfbIndex>() {
            @Override
            public void onNotify(Object sender, BfbIndex selectedIndex) {
                if (selectedIndex == null){
                    mCreateIndexButtonEnabled = mModel.getSelectedDevice()!=null;
                    notifyCreateIndexButtonEnabledChanged();
                    mRemoveIndexButtonEnabled = false;
                    notifyRemoveIndexButtonEnabledChanged();
                    mFrequencyBeginEditTextEnabled = mModel.getSelectedDevice()!=null;
                    notifyFrequencyBeginEditTextEnabledChanged();
                    mFrequencyEndEditTextEnabled = mModel.getSelectedDevice()!=null;
                    notifyFrequencyEndEditTextEnabledChanged();
                    mWindowEditTextEnabled = mModel.getSelectedDevice()!=null;
                    notifyWindowEditTextEnabledChanged();
                    mOverlappingEditTextEnabled = mModel.getSelectedDevice()!=null;
                    notifyOverlappingEditTextEnabledChanged();
                    mCalibrateButtonEnabled = false;
                    notifyCalibrationButtonEnabledChanged();
                }
                else{
                    mCreateIndexButtonEnabled = false;
                    notifyCreateIndexButtonEnabledChanged();
                    mRemoveIndexButtonEnabled = mModel.getSelectedDevice()!=null;
                    notifyRemoveIndexButtonEnabledChanged();
                    mFrequencyBeginEditTextEnabled = false;
                    notifyFrequencyBeginEditTextEnabledChanged();
                    mFrequencyEndEditTextEnabled = false;
                    notifyFrequencyEndEditTextEnabledChanged();
                    mWindowEditTextEnabled = false;
                    notifyWindowEditTextEnabledChanged();
                    mOverlappingEditTextEnabled = false;
                    notifyOverlappingEditTextEnabledChanged();
                    mCalibrateButtonEnabled = true;
                    notifyCalibrationButtonEnabledChanged();
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
    public SubscribersNotifier<Boolean> createIndexButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> removeIndexButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> calibrateButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> frequencyBeginEditTextEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> frequencyEndEditTextEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> windowEditTextEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> overlappingEditTextEnabledChanged = new SubscribersNotifier<>();

    /**
     * DeviceList events
     */
    public SubscribersNotifier<List<BfbDevice>> deviceListChanged = new SubscribersNotifier<>();

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
    public List<BfbDevice> getDeviceList(){
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

    public boolean isCreateIndexButtonEnabled() {
        return mCreateIndexButtonEnabled;
    }

    public boolean isRemoveIndexButtonEnabled() {
        return mRemoveIndexButtonEnabled;
    }

    public boolean isCalibrateButtonEnabled() {
        return mCalibrateButtonEnabled;
    }

    public boolean isFrequencyBeginEditTextEnabled() {
        return mFrequencyBeginEditTextEnabled;
    }

    public boolean isFrequencyEndEditTextEnabled() {
        return mFrequencyEndEditTextEnabled;
    }

    public boolean isWindowEditTextEnabled(){
        return mWindowEditTextEnabled;
    }

    public boolean isOverlappingEditTextEnabled(){
        return mOverlappingEditTextEnabled;
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

    public void onCreateIndexButtonClick(String freqBeginText, String freqEndText, String window, String overlapping){
        mModel.createIndex(Integer.parseInt(freqBeginText), Integer.parseInt(freqEndText), Double.parseDouble(window),Integer.parseInt(overlapping));
    }

    public void onRemoveIndexButtonClick(){
        mModel.removeIndex();
    }

    public void onCalibrateButtonClick(){
        mModel.calibrate();
    }

    public void onDeviceListItemSelected(BfbDevice device) {
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

    private void onDeviceListChanged(final List<BfbDevice> callibriVisualDevices) {
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                deviceListChanged.sendNotification(this, callibriVisualDevices);
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

    private void notifyCreateIndexButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                createIndexButtonEnabledChanged.sendNotification(null, mCreateIndexButtonEnabled);
            }
        });
    }

    private void notifyRemoveIndexButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                removeIndexButtonEnabledChanged.sendNotification(null, mRemoveIndexButtonEnabled);
            }
        });
    }

    private void notifyFrequencyBeginEditTextEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                frequencyBeginEditTextEnabledChanged.sendNotification(null, mFrequencyBeginEditTextEnabled);
            }
        });
    }

    private void notifyFrequencyEndEditTextEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                frequencyEndEditTextEnabledChanged.sendNotification(null, mFrequencyEndEditTextEnabled);
            }
        });
    }

    private void notifyCalibrationButtonEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                calibrateButtonEnabledChanged.sendNotification(this, mCalibrateButtonEnabled);
            }
        });
    }

    private void notifyWindowEditTextEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                windowEditTextEnabledChanged.sendNotification(null, mWindowEditTextEnabled);
            }
        });
    }

    private void notifyOverlappingEditTextEnabledChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                overlappingEditTextEnabledChanged.sendNotification(this, mOverlappingEditTextEnabled);
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
