package ru.neurotech.brainbitpreview;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.SignalChannel;
import com.neuromd.neurosdk.channels.SpectrumChannel;
import com.neuromd.neurosdk.channels.eeg.EegChannel;
import com.neuromd.neurosdk.parameters.Command;

import java.util.HashMap;
import java.util.Map;

public class BrainbitModel {
    private static BrainbitModel mInstance;
    
    private BrainbitModel() {
    }
    
    public static BrainbitModel getInstance() {
        if (mInstance == null) mInstance = new BrainbitModel();
        return mInstance;
    }
    
    private Device mSelectedDevice = null;
    private Map<String, EegChannel> mEegChannels;
    private Map<String, SpectrumChannel> mSpectrumChannels;
    private BatteryChannel mBatteryChannel;
    private int mBatteryLevel;
    
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> eegStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> stressValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> relaxValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> attentionValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> meditationValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();
    
    public void setDevice(final Device selectedDevice) {
        mSelectedDevice = selectedDevice;
        if (mSelectedDevice == null) {
            mEegChannels = null;
            mSpectrumChannels = null;
            mBatteryChannel = null;
        }
        else {
            mEegChannels = new HashMap<>();
            mSpectrumChannels = new HashMap<>();
            ChannelInfo[] channels = mSelectedDevice.channels();
            for (ChannelInfo channelInfo : channels) {
                if (channelInfo.getType() == ChannelType.Signal) {
                    SignalChannel eegSignalChannel = new SignalChannel(mSelectedDevice, channelInfo);
                    EegChannel eegChannel = new EegChannel(eegSignalChannel);
                    mEegChannels.put(channelInfo.getName(), eegChannel);
                    SpectrumChannel specChannel = new SpectrumChannel(eegChannel);
                    mSpectrumChannels.put(channelInfo.getName(), specChannel);
                }
                else if (channelInfo.getType() == ChannelType.Battery) {
                    mBatteryChannel = new BatteryChannel(mSelectedDevice);
                    mBatteryChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                        @Override
                        public void onNotify(Object sender, Long length) {
                            if (length > 0) {
                                mBatteryLevel = mBatteryChannel.readData(length - 1, 1)[0];
                                batteryStateChanged.sendNotification(this, mBatteryLevel);
                            }
                        }
                    });
                }
            }
            mSelectedDevice.execute(Command.StartSignal);
        }
        selectedDeviceChanged.sendNotification(this, mSelectedDevice);
    }
    
    public int getBatteryLevel() {
        if (mSelectedDevice == null) {
            return 0;
        }
        else {
            return mBatteryLevel;
        }
    }
    
    public Map<String, EegChannel> getChannels() {
        return mEegChannels;
    }
    
    public Map<String, SpectrumChannel> getSpectrums() {
        return mSpectrumChannels;
    }
    
    
    public int getSamplingFrequency() {
        if (mSelectedDevice == null) {
            return 0;
        }
        else {
            return 250;
        }
    }
    
    public Device getDevice() {
        return mSelectedDevice;
    }
    
    public void startCalculateEegStates() {
        //if (mSelectedDevice == null) return;
        //mSelectedDevice.startEegStateCalculation();
    }
    
    public void stopCalculateEegStates() {
        //if (mSelectedDevice == null) return;
        //mSelectedDevice.startEegStateCalculation();
    }
}
