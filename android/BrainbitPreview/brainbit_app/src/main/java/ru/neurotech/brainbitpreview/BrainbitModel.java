package ru.neurotech.brainbitpreview;

import android.util.Log;

import java.util.HashMap;
import java.util.Map;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.eeg.EegDevice;
import ru.neurotech.neurodevices.eeg.SpectrumData;
import ru.neurotech.neurodevices.features.Channel;
import ru.neurotech.neurodevices.state.NeuroDeviceState;

/**
 * Created by Danil Vlasenko on 9/13/2017.
 * Feel free to contact via email danny.vlasenko@gmail.com
 */

public class BrainbitModel {
    private static BrainbitModel mInstance;

    private BrainbitModel() {}
    public static BrainbitModel getInstance(){
        if (mInstance == null) mInstance = new BrainbitModel();
        return mInstance;
    }

    private EegDevice mSelectedDevice = null;
    private Map<String, Channel> mEegChannels;

    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<EegDevice> selectedDeviceChanged = new SubscribersNotifier<>();

    public void setDevice(EegDevice selectedDevice){

        if (selectedDevice == null){
            mEegChannels = null;
            mSelectedDevice.close();
        }

        mSelectedDevice = selectedDevice;

        if (mSelectedDevice != null) {
            mSelectedDevice.totalSignalDurationChanged.subscribe(new INotificationCallback<Double>() {
                @Override
                public void onNotify(Object sender, Double nParam) {

                }
            });

            mSelectedDevice.batteryStateChanged.subscribe(new INotificationCallback<Integer>() {
                @Override
                public void onNotify(Object sender, Integer value) {
                    batteryStateChanged.sendNotification(sender, value);
                }
            });

            mSelectedDevice.deviceStateChanged.subscribe(new INotificationCallback<NeuroDeviceState>() {
                @Override
                public void onNotify(Object sender, NeuroDeviceState state) {
                    if (state != NeuroDeviceState.READY && state != NeuroDeviceState.WORKING) {
                        Log.d("BrainbitModel", "Device lost, setting null device");
                        setDevice(null);
                    }
                }
            });

            mEegChannels = new HashMap<String, Channel>();
            Channel[] channels = mSelectedDevice.getChannels();
            for (Channel channel : channels){
                mEegChannels.put(channel.getName(), channel);
            }
        }

        selectedDeviceChanged.sendNotification(this, mSelectedDevice);
    }

    public int getBatteryLevel(){
        if (mSelectedDevice == null){
            return 0;
        }
        else{
            return mSelectedDevice.getBatteryLevel();
        }
    }

    public Map<String, Channel> getChannels(){
        return mEegChannels;
    }

    public int getSamplingFrequency(){
        if (mSelectedDevice == null){
            return 0;
        }
        else{
            return mSelectedDevice.getNeuroDevice().getSignalSubsystem().getSamplingFrequency();
        }
    }

   public  SpectrumData calculateSpectrumForChannel(String channelName, int offset, int length){

       if (mEegChannels == null)
           return null;

        return mSelectedDevice.calculateSpectrumForChannel(mEegChannels.get(channelName), offset, length);
   }

   public long getTotalDataLength(){
       if (mSelectedDevice==null)
           return 0;

       return mSelectedDevice.getSurveyLength();
   }

   public EegDevice getDevice(){
       return  mSelectedDevice;
   }

    public void startCalculateEegStates() {
        if (mSelectedDevice == null) return;
        mSelectedDevice.startEegStateCalculation();
    }

    public void stopCalculateEegStates() {
        if (mSelectedDevice == null) return;
        mSelectedDevice.startEegStateCalculation();
    }
}
