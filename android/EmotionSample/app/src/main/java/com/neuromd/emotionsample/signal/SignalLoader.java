package com.neuromd.emotionsample.signal;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.channels.SignalChannel;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.BrokenBarrierException;
import java.util.concurrent.CyclicBarrier;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class SignalLoader {
    private final List<SignalChannel> mChannels;
    
    private float mViewTime;
    private int mVerticalScale;
    private int mHorizontalScale;
    private int mChannelHeight;
    
    private boolean mRunning;
    private Map<String, float[]> mSignals;
    private long mWorkerOffset;
    private int mWorkerLength;
    private int mWorkerVScale;
    private CyclicBarrier mWorkersBarrier;
    private ReentrantLock mWorkerLock = new ReentrantLock();
    private Condition mWorkerStartCondition = mWorkerLock.newCondition();
    private ReentrantLock mMainLock = new ReentrantLock();
    private Condition mMainCondition = mMainLock.newCondition();
    
    public SubscribersNotifier<Map<String, float[]>> signalLoaded = new SubscribersNotifier<>();
    
    public SignalLoader(List<SignalChannel> channels){
        mRunning = true;
        mChannels = channels;
        startWorkers();
        startLoaderThread();
    }
    
    public void onViewTimeChanged(float viewTime){
        if (mViewTime != viewTime) {
            mViewTime = viewTime;
            mMainLock.lock();
            mMainCondition.signal();
            mMainLock.unlock();
        }
    }
    
    public void onVerticalScaleChanged(int vScale){
        if (mVerticalScale != vScale){
            mVerticalScale = vScale;
            mMainLock.lock();
            mMainCondition.signal();
            mMainLock.unlock();
        }
    }
    
    public void onHorizontalScaleChanged(int hScale){
        if (mHorizontalScale != hScale){
            mHorizontalScale = hScale;
            mMainLock.lock();
            mMainCondition.signal();
            mMainLock.unlock();
        }
    }
    
    public void onChannelHeightChanged(int height){
        if (mChannelHeight != height){
            mChannelHeight = height;
            mMainLock.lock();
            mMainCondition.signal();
            mMainLock.unlock();
        }
    }
    
    private void startLoaderThread(){
        new Thread(new Runnable() {
            @Override
            public void run() {
                while(mRunning){
                    mMainLock.lock();
                    try {
                        mMainCondition.await();
                        mWorkersBarrier.reset();
                        reloadAll();
                    }
                    catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    finally {
                        mMainLock.unlock();
                    }
                }
            }
        }).start();
    }
    
    private void startWorkers(){
        mSignals = new HashMap<>();
        mWorkersBarrier = new CyclicBarrier(mChannels.size(), new Runnable() {
            @Override
            public void run() {
                signalLoaded.sendNotification(this, mSignals);
            }
        });
        for(final SignalChannel channel : mChannels){
            final String channelName = channel.info().getName();
            mSignals.put(channelName, null);
            new Thread(new Runnable() {
                @Override
                public void run() {
                    while (mRunning){
                        mWorkerLock.lock();
                        try {
                            mWorkerStartCondition.await();
                            float[] signal = reloadSignal(channel, mWorkerOffset, mWorkerLength, mWorkerVScale);
                            mSignals.put(channelName, signal);
                            mWorkersBarrier.await();
                        }
                        catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                        catch (BrokenBarrierException e) {
                            e.printStackTrace();
                        }
                        finally {
                            mWorkerLock.unlock();
                        }
                    }
                }
            }).start();
        }
    }
    
    private void reloadAll(){
        mWorkerLock.lock();
        
        mWorkerOffset = (long) (mViewTime * 250);
        mWorkerLength = mHorizontalScale * 250;
        if (mWorkerOffset < 0)
            mWorkerOffset = 0;
        mWorkerVScale = mVerticalScale;
        
        mWorkerStartCondition.signalAll();
        mWorkerLock.unlock();
    }
    
    private float[] reloadSignal(SignalChannel channel, long offset, int length, int vScale) {
        float[] buffer = new float[length];
        float[] signal = convertSignal(channel.readData(offset, length), vScale);
        int start = length - signal.length;
        if (start >= 0) {
            for (int i = start; i < length; ++i) {
                buffer[i] = (float) (signal[i - start] * 1e6);
            }
        }
        return buffer;
    }
    
    private float[] convertSignal(Double[] signal, int vScale) {
        float[] result = new float[signal.length];
        for (int i = 0; i < result.length; ++i){
            result[i] = (float)(signal[i] / vScale * mChannelHeight);
        }
        return result;
    }
}
