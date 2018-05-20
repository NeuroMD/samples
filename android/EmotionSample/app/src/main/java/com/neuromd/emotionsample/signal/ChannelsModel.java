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
import com.neuromd.neurosdk.channels.eeg.EegChannel;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

public class ChannelsModel {
    private Device mDevice = null;
    private BatteryChannel mBatteryChannel = null;
    private DeviceState mDeviceState;
    private EegChannel mEegChannel;
    private int mBatteryLevel;
    private double mSignalDuration;
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
    
    
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> signalDurationChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> emotionStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> attentionValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> productiveRelaxValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> stressValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> meditationValueChanged = new SubscribersNotifier<>();
    
    public void setDevice(Device device) {
        
        if (mDevice != null) {
            mDevice.parameterChanged.unsubscribe();
        }
        
        mDevice = device;
        
        if (mDevice != null) {
            mDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                @Override
                public void onNotify(Object o, ParameterName paramName) {
                    if (paramName == ParameterName.State) {
                        mDeviceState = (DeviceState) mDevice.readParam(paramName);
                        //deviceStateChanged.sendNotification(this, mDeviceState);
                    }
                }
            });
            
            /*mSamplingFrequency = (SamplingFrequency) mDevice.readParam(ParameterName.SamplingFrequency);
            if (mSamplingFrequency != SamplingFrequency.Hz125) {
                mDevice.setParam(ParameterName.SamplingFrequency, SamplingFrequency.Hz125);
            }
            mSamplingFrequency = (SamplingFrequency) mDevice.readParam(ParameterName.SamplingFrequency);
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);*/
            
            
            
            String deviceName = mDevice.readParam(ParameterName.Name);
            if (deviceName.equals("BrainBit")) {
                ChannelInfo[] channelInfo = mDevice.channels();
                
                for (ChannelInfo info : channelInfo) {
                    if (info.getType() == ChannelType.Signal) {
                        Log.d("CreateChannel", String.format("Creating channel %s", info.getName()));
                        SignalChannel signalChannel = new SignalChannel(mDevice, info);
                        
                        break;
                    }
                }
            }
            else {
                mDevice.execute(Command.FindMe);
                SignalChannel signalChannel = new SignalChannel(mDevice);
                
            }
            /*mEegChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (mEcgChannel == null)
                        return;
                    mSignalDuration = (double) length / mEcgChannel.samplingFrequency();
                    signalDurationChanged.sendNotification(this, mSignalDuration);
                }
            });*/
            
            mBatteryChannel = new BatteryChannel(mDevice);
            mBatteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    //Log.d("BatteryChannel", String.format("Length changed %d", length));
                    //if (length > 0) {
                    if (mBatteryChannel == null)
                        return;
                    long offset = length - 1;
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
            else {
                mBatteryLevel = 0;
            }
            batteryStateChanged.sendNotification(this, mBatteryLevel);
            mBatteryChannel.setSamplingFrequency(0.3f);
            
            
            mDeviceState = (DeviceState) mDevice.readParam(ParameterName.State);
            //deviceStateChanged.sendNotification(this, mDeviceState);

            /*mHpfEnabled = mDevice.getCallibriDevice().getNeuroDevice().getSignalSubsystem().getHpfEnabled();
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);*/

            

            /*mGain = mDevice.readParam(ParameterName.Gain);
            gainChanged.sendNotification(this, mGain);

            mOffset = mDevice.readParam(ParameterName.Offset);
            offsetChanged.sendNotification(this, mOffset);

            mChannelsCount = mDevice.getNeuroDevice().getSignalSubsystem().getChannels().length;
            channelsCountChanged.sendNotification(this, mChannelsCount);

            mHeartRate = mDevice.getCurrentHeartRate();
            heartRateChanged.sendNotification(this, mHeartRate);

            mStressIndex = mDevice.getCurrentStressIndex();
            stressIndexChanged.sendNotification(this, mStressIndex);

            mSignalDuration = (double)mEcgChannel.totalLength() / mEcgChannel.samplingFrequency();
            signalDurationChanged.sendNotification(this, mSignalDuration);*/

           /* mIsElectrodesAttached = mDevice.getElectrodesState();
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);*/
        }
        else {
            mBatteryChannel = null;
            
            mBatteryLevel = 0;
            batteryStateChanged.sendNotification(this, mBatteryLevel);
            
            mDeviceState = DeviceState.Disconnected;
            //deviceStateChanged.sendNotification(this, mDeviceState);
            
            
            
            mSignalDuration = 0.0;
            signalDurationChanged.sendNotification(this, mSignalDuration);
            
        }
    }
    
    public void removeDevice() {
        if (mDevice == null)
            return;
        mDevice.disconnect();
        //mDeviceList.remove(mDevice);
        //deviceListChanged.sendNotification(this, mDeviceList);
        setDevice(null);
    }
    
    public void signalStart() {
        if (mDevice == null)
            return;
        
        mDevice.execute(Command.StartSignal);
    }
    
    public void signalStop() {
        if (mDevice == null)
            return;
        
        mDevice.execute(Command.StopSignal);
    }
    public Device getSelectedDevice() {
        return mDevice;
    }
    
    public int getBatteryLevel() {
        return mBatteryLevel;
    }
    
    public Double[] getSignalData(double time, double duration) {
        if (mDevice == null) {
            return null;
        }
        
        if (time < 0) {
            time = 0;
            duration = getTotalDuration();
        }
        
        float samplingFreq = mEegChannel.samplingFrequency();
        int offset = (int) (time * samplingFreq);
        int length = (int) (duration * samplingFreq);
        
        if (length <= 0)
            return new Double[0];
        
        return getRawSignal(offset, length);
    }
    
    public Double[] getRawSignal(int offset, int length) {
        if (mDevice == null) {
            return null;
        }
        
        return mEegChannel.readData(offset, length);
    }
    
    public double getTotalDuration() {
        if (mDevice == null) {
            return 0;
        }
        
        double length = mEegChannel.totalLength();
        double duration = length / mEegChannel.samplingFrequency();
        
        return duration;
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
}
