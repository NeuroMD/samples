package com.neuromd.emotionsample.signal;

import android.util.Log;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SignalLoader {
    private final Collection<EegChannel> mChannels;
    
    private float mViewTime;
    private int mVerticalScale;
    private int mHorizontalScale;
    private int mChannelHeight;
    
    private boolean mRunning;
    private List<Thread> mWorkers = new ArrayList<>();
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
    
    @Override
    public void finalize() throws Throwable {
        mRunning = false;
        Log.d("SignalLoader", "Finalizing workers");
        for (Thread worker : mWorkers){
            worker.join();
        }
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
        for (final EegChannel channel : mChannels) {
            final String channelName = channel.info().getName();
            mSignals.put(channelName, null);
        }
        Thread worker = new Thread(new Runnable() {
            @Override
            public void run() {
                while (mRunning) {
                    for (EegChannel channel : mChannels) {
                        float[] signal = reloadSignal(channel, mWorkerOffset, mWorkerLength, mWorkerVScale);
                        mSignals.put(channel.info().getName(), signal);
                    }
                    signalLoaded.sendNotification(this, mSignals);
                }
            }
        });
        worker.start();
        mWorkers.add(worker);
    }
    
    private float[] reloadSignal(EegChannel channel, long offset, long length, int vScale) {
        float[] buffer = new float[(int)length];
        long channelLength = channel.totalLength();
        long requestLength = length;
        if (offset + requestLength > channelLength){
            requestLength = channelLength - offset;
        }
        if (requestLength > 0) {
            float[] signal = convertSignal(channel.readFast(offset, requestLength), vScale);
            int start = (int) (length - signal.length);
            if (start >= 0) {
                for (int i = start; i < length; ++i) {
                    buffer[i] = (float) (signal[i - start]);
                }
            }
        }
        return buffer;
    }
    
    private float[] convertSignal(double[] signal, int vScale) {
        float[] result = new float[signal.length];
        double max = 0;
        for (int i = 0; i < result.length; ++i){
            result[i] = (float)(signal[i] * 1e6 / vScale * mChannelHeight);
            if (result[i]>max) max = result[i];
        }
        return result;
    }
}
