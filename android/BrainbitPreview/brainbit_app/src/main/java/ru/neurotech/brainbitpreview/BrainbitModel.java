package ru.neurotech.brainbitpreview;

import android.util.Log;

import java.nio.channels.Channel;
import java.util.HashMap;
import java.util.Map;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurosdk.Device;
import ru.neurotech.neurosdk.parameters.ParameterName;
import ru.neurotech.neurosdk.parameters.types.DeviceState;

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

    private Device mSelectedDevice = null;
    private Map<String, Channel> mEegChannels;

    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();

    public void setDevice(final Device selectedDevice){

        if (selectedDevice == null){
            mEegChannels = null;
            mSelectedDevice.close();
        }

        mSelectedDevice = selectedDevice;

        if (mSelectedDevice != null) {
            mSelectedDevice.batteryStateChanged.subscribe(new INotificationCallback<Integer>() {
                @Override
                public void onNotify(Object sender, Integer value) {
                    batteryStateChanged.sendNotification(sender, value);
                }
            });

            mSelectedDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                @Override
                public void onNotify(Object sender, ParameterName parameter) {
                    if (parameter == ParameterName.State) {
                        DeviceState = (DeviceState)selectedDevice.
                        if ( !=DeviceState.READY && state != NeuroDeviceState.WORKING){
                            Log.d("BrainbitModel", "Device lost, setting null device");
                            setDevice(null);
                        }
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
