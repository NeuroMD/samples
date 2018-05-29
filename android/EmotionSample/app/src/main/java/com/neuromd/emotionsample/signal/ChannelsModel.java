package com.neuromd.emotionsample.signal;

import android.util.Log;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.SignalChannel;
import com.neuromd.neurosdk.channels.eeg.EegChannel;
import com.neuromd.neurosdk.channels.eeg.EmotionalStateChannel;
import com.neuromd.neurosdk.channels.eeg.EmotionalState;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

public class ChannelsModel {
    private Device mDevice = null;
    private BatteryChannel mBatteryChannel = null;
    private Map<String, EegChannel> mEegChannelCollection;
    private EmotionalStateChannel mEmotionChannel;
    private int mBatteryLevel;
    private long mSignalLength;
    private EmotionalState mEmotionState;
    
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> signalDurationChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> calculationStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<EmotionalState> emotionStateChanged = new SubscribersNotifier<>();
    
    public void setDevice(Device device) {
    
        if (mDevice != null) {
            stopCalculations();
            mDevice.parameterChanged.unsubscribe();
            mBatteryChannel.dataLengthChanged.unsubscribe();
            mDevice.execute(Command.StopSignal);
            for (EegChannel chan : mEegChannelCollection.values()){
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
    
            mEegChannelCollection = new HashMap<>();
            ChannelInfo[] channelInfo = mDevice.channels();
            for (ChannelInfo info : channelInfo) {
                if (info.getType() == ChannelType.Signal) {
                    Log.d("CreateChannel", String.format("Creating channel %s", info.getName()));
                    SignalChannel signalChannel = new SignalChannel(mDevice, info);
                    EegChannel eegChannel = new EegChannel(signalChannel);
                    eegChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                        @Override
                        public void onNotify(Object o, Long length) {
                            if (length > mSignalLength) {
                                mSignalLength = length;
                                signalDurationChanged.sendNotification(this, getTotalDuration());
                            }
                        }
                    });
                    mEegChannelCollection.put(eegChannel.info().getName(), eegChannel);
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
        if (mEmotionChannel != null){
            return;
        }
        
        EegChannel t3 = mEegChannelCollection.get("T3");
        EegChannel t4 = mEegChannelCollection.get("T4");
        EegChannel o1 = mEegChannelCollection.get("O1");
        EegChannel o2 = mEegChannelCollection.get("O2");
        
        if (t3 == null || t4 == null || o1 == null || o2 == null){
            Log.w("ChannelsModel", "Failed start calculations: one of the channels is null");
            return;
        }
        
        mEmotionChannel = new EmotionalStateChannel(t3, t4, o1, o2);
        mEmotionChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
            @Override
            public void onNotify(Object o, Long length) {
                if (length > 0) {
                    mEmotionState = mEmotionChannel.readData(length - 1, 1)[0];
                }
                else {
                    mEmotionState = null;
                }
                emotionStateChanged.sendNotification(this, mEmotionState);
            }
        });
        calculationStateChanged.sendNotification(this, true);
    }
    
    public void stopCalculations() {
        if (mEmotionChannel != null){
            mEmotionChannel.dataLengthChanged.unsubscribe();
        }
        mEmotionChannel = null;
        calculationStateChanged.sendNotification(this, false);
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
    
    public Collection<EegChannel> getChannels(){
        return mEegChannelCollection.values();
    }
    
    public boolean isCalculationInProc() {
        return mEmotionChannel != null;
    }
    
    public EmotionalState getEmotionState() {
        return mEmotionState;
    }
    
    private double lengthToDuration(long length){
        return ((double)length) / 250.0;
    }
}
