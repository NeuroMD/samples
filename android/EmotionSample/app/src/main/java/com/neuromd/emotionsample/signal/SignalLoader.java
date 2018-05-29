package com.neuromd.emotionsample.signal;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

public class SignalLoader {
    private final Collection<EegChannel> mChannels;
    
    private float mViewTime;
    private int mVerticalScale;
    private int mHorizontalScale;
    private int mChannelHeight;
    
    private boolean mRunning;
    private Map<String, float[]> mSignals;
    private long mWorkerOffset;
    private int mWorkerLength;
    private int mWorkerVScale;
    
    public SubscribersNotifier<Map<String, float[]>> signalLoaded = new SubscribersNotifier<>();
    
    public SignalLoader(Collection<EegChannel> channels){
        mRunning = true;
        mChannels = channels;
        startWorkers();
    }
    
    public void onViewTimeChanged(float viewTime){
        if (mViewTime != viewTime) {
            mViewTime = viewTime;
            mWorkerOffset = (long) (mViewTime * 250);
            if (mWorkerOffset < 0)
                mWorkerOffset = 0;
        }
    }
    
    public void onVerticalScaleChanged(int vScale){
        if (mVerticalScale != vScale){
            mVerticalScale = vScale;
            mWorkerVScale = vScale;
        }
    }
    
    public void onHorizontalScaleChanged(int hScale){
        if (mHorizontalScale != hScale){
            mHorizontalScale = hScale;
            mWorkerLength = mHorizontalScale * 250;
        }
    }
    
    public void onChannelHeightChanged(int height){
        if (mChannelHeight != height){
            mChannelHeight = height;
        }
    }
    
    private void startWorkers() {
        mSignals = new HashMap<>();
        for (final EegChannel channel : mChannels) {
            final String channelName = channel.info().getName();
            mSignals.put(channelName, null);
            new Thread(new Runnable() {
                @Override
                public void run() {
                    while (mRunning) {
                        float[] signal = reloadSignal(channel, mWorkerOffset, mWorkerLength, mWorkerVScale);
                        mSignals.put(channelName, signal);
                        signalLoaded.sendNotification(this, mSignals);
                    }
                }
            }).start();
        }
    }
    
    private float[] reloadSignal(EegChannel channel, long offset, int length, int vScale) {
        float[] buffer = new float[length];
        float[] signal = convertSignal(channel.readFast(offset, length), vScale);
        int start = length - signal.length;
        if (start >= 0) {
            for (int i = start; i < length; ++i) {
                buffer[i] = (float) (signal[i - start] * 1e6);
            }
        }
        return buffer;
    }
    
    private float[] convertSignal(double[] signal, int vScale) {
        float[] result = new float[signal.length];
        for (int i = 0; i < result.length; ++i){
            result[i] = (float)(signal[i] / vScale * mChannelHeight);
        }
        return result;
    }
}
