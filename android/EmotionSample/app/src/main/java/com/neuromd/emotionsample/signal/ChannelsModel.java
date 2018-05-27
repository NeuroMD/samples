package com.neuromd.emotionsample.signal;

import android.os.Handler;
import android.util.Log;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.SignalChannel;
//import com.neuromd.neurosdk.channels.eeg.EegChannel;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public class ChannelsModel {
    private Device mDevice = null;
    private BatteryChannel mBatteryChannel = null;
    private List<SignalChannel> mEegChannelCollection;
    private int mBatteryLevel;
    private long mSignalLength;
    private int mEmotionValue;
    
    public ChannelsModel(){
        final Handler handler = new Handler();
        final Runnable indicatorTestRunnable = new Runnable() {
            int direction = 1;
            @Override
            public void run() {
                if (direction > 0){
                    mEmotionValue += 1;
                    if (mEmotionValue >= 10){
                        direction = -1;
                    }
                }else if (direction < 0){
                    mEmotionValue -= 1;
                    if (mEmotionValue <= -10){
                        direction = 1;
                    }
                }
                emotionStateChanged.sendNotification(this, mEmotionValue);
                handler.postDelayed(this, 500);
            }
        };
        indicatorTestRunnable.run();
    }
    
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> signalDurationChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> emotionStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> attentionValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> productiveRelaxValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> stressValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> meditationValueChanged = new SubscribersNotifier<>();
    
    private Object mSync = new Object();
    
    public void setDevice(Device device) {
    
        if (mDevice != null) {
            mDevice.parameterChanged.unsubscribe();
            mBatteryChannel.dataLengthChanged.unsubscribe();
            mDevice.execute(Command.StopSignal);
            for (SignalChannel chan : mEegChannelCollection){
                chan.dataLengthChanged.unsubscribe();
            }
        }
    
        mDevice = device;
    
        if (mDevice != null) {
            mDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                @Override
                public void onNotify(Object o, ParameterName paramName) {
                    if (paramName == ParameterName.State) {
                        DeviceState deviceState = mDevice.readParam(paramName);
                        if (deviceState == DeviceState.Disconnected) {
                            removeDevice();
                        }
                    }
                }
            });
    
            mEegChannelCollection = new ArrayList<>();
            ChannelInfo[] channelInfo = mDevice.channels();
            for (ChannelInfo info : channelInfo) {
                if (info.getType() == ChannelType.Signal) {
                    Log.d("CreateChannel", String.format("Creating channel %s", info.getName()));
                    SignalChannel signalChannel = new SignalChannel(mDevice, info);
                    signalChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                        @Override
                        public void onNotify(Object o, Long length) {
                            if (length > mSignalLength) {
                                mSignalLength = length;
                                signalDurationChanged.sendNotification(this, getTotalDuration());
                            }
                        }
                    });
                    //EegChannel eegChannel = new EegChannel(signalChannel);
                    mEegChannelCollection.add(signalChannel);
                    //break;
                }
                else if (info.getType() == ChannelType.Battery) {
                    mBatteryChannel = new BatteryChannel(mDevice);
                    mBatteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                        @Override
                        public void onNotify(Object o, Long length) {
                            if (mBatteryChannel == null)
                                return;
                            if (length > 0) {
                                long offset = length - 1;
                                Integer[] data = mBatteryChannel.readData(offset, 1);
                                mBatteryLevel = data[0];
                                batteryStateChanged.sendNotification(this, getBatteryLevel());
                            }
                        }
                    });
                    long batteryDataLength = mBatteryChannel.totalLength();
                    if (batteryDataLength > 0) {
                        mBatteryLevel = mBatteryChannel.readData(batteryDataLength - 1, 1)[0];
                    }
                    else {
                        mBatteryLevel = 0;
                    }
                    batteryStateChanged.sendNotification(this, getBatteryLevel());
                }
            }
    
            mDevice.execute(Command.StartSignal);
        }
        else {
            mBatteryChannel = null;
        
            mBatteryLevel = 0;
            batteryStateChanged.sendNotification(this, getBatteryLevel());
        
            mSignalLength = 0;
            signalDurationChanged.sendNotification(this, getTotalDuration());
            
            mEegChannelCollection = null;
        }
        selectedDeviceChanged.sendNotification(this, getSelectedDevice());
    }
    
    public void removeDevice() {
        if (mDevice == null)
            return;
        mDevice.disconnect();
        setDevice(null);
    }
    
    public void startCalculations() {
    
    }
    
    public void stopCalculations() {
        mDevice.execute(Command.StopSignal);
    }
    
    public Device getSelectedDevice() {
        return mDevice;
    }
    
    public int getBatteryLevel() {
        return mBatteryLevel;
    }
    
    public double getTotalDuration() {
        if (mDevice == null) {
            return 0;
        }
        return lengthToDuration(mSignalLength);
    }
    
    public List<SignalChannel> getChannels(){
        return mEegChannelCollection;
    }
    
    public int getEmotionalValue() {
        return mEmotionValue;
    }
    
    public int getAttentionValue() {
        
        return 0;
    }
    
    public int getProductiveRelaxValue() {
        
        return 0;
    }
    
    public int getStressValue() {
        
        return 0;
    }
    
    public int getMeditationValue() {
        
        return 0;
    }
    
    private double lengthToDuration(long length){
        return ((double)length) / 250.0;
    }
}
