package com.neuromd.emotionsample.signal.spectrum;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.channels.SpectrumChannel;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

public class SpectrumLoader {
    private final Collection<SpectrumChannel> mChannels;
    
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
    
    public SpectrumLoader(Collection<EegChannel> channels){
        mRunning = true;
        mChannels = new ArrayList<>();
        for(EegChannel chan : channels){
            SpectrumChannel spectrumChan = new SpectrumChannel(chan);
            mChannels.add(spectrumChan);
        }
        startWorkers();
    }
    
    @Override
    public void finalize() throws Throwable {
        mRunning = false;
        super.finalize();
    }
    
    public void onViewTimeChanged(float viewTime){
        if (mViewTime != viewTime) {
            mViewTime = viewTime;
            mWorkerOffset = (long) (mViewTime * 250);
            if (mWorkerOffset < 0) {
                mWorkerOffset = 0;
            }
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
        for (final SpectrumChannel channel : mChannels) {
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
    
    private float[] reloadSignal(SpectrumChannel channel, long offset, long length, int vScale) {
        double hzPerSample = channel.hzPerSpectrumSample();
        int samplesCount = (int) (70.0 / hzPerSample);
        float[] buffer = new float[samplesCount];
        if (length > 0) {
            float[] signal = convertSignal(channel.readFast(offset, length), vScale);
            if (signal.length >= samplesCount) {
                for (int i = 0; i < samplesCount; ++i) {
                    buffer[i] = (float) (signal[i] * 1e6);
                }
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
