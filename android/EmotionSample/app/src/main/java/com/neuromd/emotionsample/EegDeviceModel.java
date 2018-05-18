package com.neuromd.emotionsample;

import android.content.Context;
import android.util.Log;

import com.neuromd.bleconnection.exceptions.BluetoothAdapterException;
import com.neuromd.bleconnection.exceptions.BluetoothPermissionException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.channels.BatteryChannel;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.SignalChannel;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.List;


public class EegDeviceModel {
    
    private final DeviceScanner mDeviceConnector;
    private Device mDevice = null;
    private BatteryChannel mBatteryChannel = null;
    private DeviceState mDeviceState;
    private int mBatteryLevel;
    private double mSignalDuration;
    
    public EegDeviceModel(Context context) {
        
        mDeviceConnector = new DeviceScanner(context);
        mDeviceConnector.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, final Device device) {
                DeviceState state = (DeviceState) device.readParam(ParameterName.State);
                if (state == DeviceState.Connected) {
                }
                else {
                    device.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                        @Override
                        public void onNotify(Object o, ParameterName parameterName) {
                            if (parameterName == ParameterName.State) {
                                DeviceState state = (DeviceState) device.readParam(ParameterName.State);
                                if (state == DeviceState.Connected) {
                                    device.parameterChanged.unsubscribe();
                                }
                            }
                        }
                    });
                    device.connect();
                }
    
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
    public SubscribersNotifier<Device> selectedDeviceChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> batteryStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Double> signalDurationChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> emotionStateChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> attentionValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> productiveRelaxValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> stressValueChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> meditationValueChanged = new SubscribersNotifier<>();
    
    public void startScan() {
        
        removeDevice();
        for (Device device : mDeviceList) {
            device.disconnect();
        }
        mDeviceList.clear();
        deviceListChanged.sendNotification(this, mDeviceList);
        try {
            mDeviceConnector.startScan(0);
        }
        catch (BluetoothAdapterException e) {
            stopScan();
            bluetoothAdapterEnableNeeded.sendNotification(this, null);
        }
        catch (BluetoothPermissionException e) {
            stopScan();
            bluetoothPermissionsNeeded.sendNotification(this, null);
        }
    }
    
    public void stopScan() {
        mDeviceConnector.stopScan();
    }
    
    public void selectDevice(Device device) {
        
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
                        deviceStateChanged.sendNotification(this, mDeviceState);
                    }
                }
            });
            
            mSamplingFrequency = (SamplingFrequency) mDevice.readParam(ParameterName.SamplingFrequency);
            if (mSamplingFrequency != SamplingFrequency.Hz125) {
                mDevice.setParam(ParameterName.SamplingFrequency, SamplingFrequency.Hz125);
            }
            mSamplingFrequency = (SamplingFrequency) mDevice.readParam(ParameterName.SamplingFrequency);
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);
            
            mElectrodesStateChannel = new ElectrodesStateChannel(mDevice);
            mElectrodesStateChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (mElectrodesStateChannel == null)
                        return;
                    Log.d("ElectrodesStateChannel", String.format("Electrode length changed %d", length));
                    if (length > 0) {
                        long offset = length - 1;
                        ElectrodeState[] data = mElectrodesStateChannel.readData(offset, 1);
                        mIsElectrodesAttached = data[0] == ElectrodeState.Normal;
                        electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
                    }
                }
            });
            long electrodesStateDataLength = mElectrodesStateChannel.totalLength();
            if (electrodesStateDataLength > 0) {
                mIsElectrodesAttached = mElectrodesStateChannel.readData(electrodesStateDataLength - 1, 1)[0] == ElectrodeState.Normal;
            }
            else {
                mIsElectrodesAttached = false;
            }
            electrodesStateChanged.sendNotification(this, mIsElectrodesAttached);
            mElectrodesStateChannel.setSamplingFrequency(2f);
            
            String deviceName = mDevice.readParam(ParameterName.Name);
            if (deviceName.equals("BrainBit")) {
                ChannelInfo[] channelInfo = mDevice.channels();
                
                for (ChannelInfo info : channelInfo) {
                    if (info.getType() == ChannelType.Signal) {
                        Log.d("CreateChannel", String.format("Creating channel %s", info.getName()));
                        SignalChannel signalChannel = new SignalChannel(mDevice, info);
                        mEcgChannel = new EcgChannel(signalChannel);
                        mRChannel = new RPeakChannel(signalChannel);
                        break;
                    }
                }
            }
            else {
                mDevice.execute(Command.FindMe);
                SignalChannel signalChannel = new SignalChannel(mDevice);
                mEcgChannel = new EcgChannel(signalChannel);
                mRChannel = new RPeakChannel(signalChannel, mElectrodesStateChannel);
                mHeartRateChannel = new HeartRateChannel(mRChannel);
                mHeartRateChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                    @Override
                    public void onNotify(Object o, Long length) {
                        if (mHeartRateChannel == null)
                            return;
                        long offset = length - 1;
                        Integer[] data = mHeartRateChannel.readData(offset, 1);
                        mHeartRate = data[0];
                        heartRateChanged.sendNotification(this, mHeartRate);
                    }
                });
                mStressIndexChannel = new StressIndexChannel(mRChannel);
                mStressIndexChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                    @Override
                    public void onNotify(Object o, Long length) {
                        if (mStressIndexChannel == null)
                            return;
                        long offset = length - 1;
                        Double[] data = mStressIndexChannel.readData(offset, 1);
                        mStressIndex = data[0];
                        stressIndexChanged.sendNotification(this, mStressIndex);
                    }
                });
            }
            mEcgChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    if (mEcgChannel == null)
                        return;
                    mSignalDuration = (double) length / mEcgChannel.samplingFrequency();
                    signalDurationChanged.sendNotification(this, mSignalDuration);
                }
            });
            
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
            deviceStateChanged.sendNotification(this, mDeviceState);

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
            deviceStateChanged.sendNotification(this, mDeviceState);
            
            mHpfEnabled = false;
            hpfEnabledChanged.sendNotification(this, mHpfEnabled);
            
            mSamplingFrequency = SamplingFrequency.Hz1000;
            samplingFrequencyChanged.sendNotification(this, mSamplingFrequency);
            
            //mGain = 0;
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
        selectedDeviceChanged.sendNotification(this, mDevice);
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
    
    public void removeDevice() {
        if (mDevice == null)
            return;
        mDevice.disconnect();
        mDeviceList.remove(mDevice);
        deviceListChanged.sendNotification(this, mDeviceList);
        selectDevice(null);
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
        
        float samplingFreq = mEcgChannel.samplingFrequency();
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
        
        return mEcgChannel.readData(offset, length);
    }
    
    public double getTotalDuration() {
        if (mDevice == null) {
            return 0;
        }
        
        double length = mEcgChannel.totalLength();
        double duration = length / mEcgChannel.samplingFrequency();
        
        return duration;
    }
    
    public int getEmotionalValue() {
    
    }
    
    public int getAttentionValue() {
    
    }
    
    public int getProductiveRelaxValue() {
    
    }
    
    public int getStressValue() {
    
    }
    
    public int getMeditationValue() {
    
    }
}
