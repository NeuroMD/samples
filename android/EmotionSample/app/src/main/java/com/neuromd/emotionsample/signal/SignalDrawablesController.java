package com.neuromd.emotionsample.signal;

import android.os.Handler;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.drawables.DrawableChannel;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;
import com.neuromd.emotionsample.signal.drawables.Ruler;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SignalDrawablesController extends DrawablesController {
    private final int mRulerHeight = 25;
    private Map<String, DrawableChannel> mChannels;
    private Ruler mRuler;
    private float mViewTime;
    private double mScrollVelocity = 0.0;
    private SignalLoader mSignalLoader;
    private float mSignalStartTime;
    
    public SignalDrawablesController(){
        enableSignalScrolling();
    }
    
    @Override
    public void setChannels(Collection<EegChannel> eegChannels){
        if (mSignalLoader != null) {
            mSignalLoader.signalLoaded.unsubscribe();
        }
        if (eegChannels == null){
            mDrawables = null;
            mSignalLoader = null;
        }
        else {
            mDrawables = generateDrawables(eegChannels);
            mSignalLoader = new SignalLoader(eegChannels);
            mSignalLoader.signalLoaded.subscribe(new INotificationCallback<Map<String, float[]>>() {
                @Override
                public void onNotify(Object o, Map<String, float[]> loadedSignals) {
                    onSignalLoaded(loadedSignals);
                }
            });
            mSignalLoader.onHorizontalScaleChanged(mHorizontalScale);
            mSignalLoader.onVerticalScaleChanged(mVerticalScale);
            int channelHeight = (int)((float) (mDrawableAreaHeight) / mChannels.size());
            mSignalLoader.onChannelHeightChanged(channelHeight);
        }
    }
    
    public void setScrollVelocity(double xVelocity) {
        mScrollVelocity = xVelocity;
    }
    
    @Override
    public void setViewTime(float time) {
        float signalTime = time - mHorizontalScale;
        if (mViewTime < mSignalStartTime + 0.1 && mViewTime > mSignalStartTime - 0.1){
            updateViewTime(signalTime);
        }
        mSignalStartTime = signalTime;
    }
    
    private void updateViewTime(float time){
        if (mViewTime != time){
            mViewTime = time;
            if (mSignalLoader!=null){
                mSignalLoader.onViewTimeChanged(mViewTime);
            }
            if (mRuler != null) {
                mRuler.setViewTime(mViewTime);
            }
        }
    }
    
    @Override
    protected void onSizeChanged(int width, int height) {
        setChannelSizes();
        setRulerSizes();
    }
    
    @Override
    protected void onVerticalScaleChanged(int vScale) {
        if (mSignalLoader != null){
            mSignalLoader.onVerticalScaleChanged(vScale);
        }
    }
    
    @Override
    protected void onHorizontalScaleChanged(int hScale) {
        if (mSignalLoader != null){
            mSignalLoader.onHorizontalScaleChanged(hScale);
        }
        setRulerSizes();
    }
    
    private List<ISignalFieldDrawable> generateDrawables(Collection<EegChannel> eegChannels) {
        List<ISignalFieldDrawable> drawables = new ArrayList<>();
        mChannels = new HashMap<>();
        for (EegChannel channel : eegChannels){
            DrawableChannel drawChannel = new DrawableChannel(channel.info().getName());
            mChannels.put(channel.info().getName(), drawChannel);
            drawables.add(drawChannel);
        }
        setChannelSizes();
    
        mRuler = new Ruler();
        mRuler.setSecondCount(mHorizontalScale);
        mRuler.setViewTime(mViewTime);
        setRulerSizes();
        drawables.add(mRuler);
        return drawables;
    }
    private void enableSignalScrolling() {
        final Handler handler = new Handler();
        final Runnable scrollVelocityRunnable = new Runnable() {
            @Override
            public void run() {
                if (mScrollVelocity > 0.0) {
                    mScrollVelocity /= 1.5;
                } else if (mScrollVelocity < 0.0) {
                    mScrollVelocity /= 1.5;
                }
                if (Math.abs(mScrollVelocity) < 0.1) {
                    mScrollVelocity = 0.0;
                }
                
                if (mScrollVelocity != 0.0) {
                    if (mViewTime >= 0.0) {
                        float viewTime = (float)(mViewTime - mScrollVelocity);
                        if (viewTime < 0.0f)
                            viewTime = 0.0f;
                        else if (viewTime > mSignalStartTime)
                            viewTime = mSignalStartTime;
                        updateViewTime(viewTime);
                    }
                }
                handler.postDelayed(this, 50);
            }
        };
        scrollVelocityRunnable.run();
    }
    
    private void setChannelSizes(){
        if (mChannels != null){
            int topY = 0;
            int channelHeight = (int)((float)(mDrawableAreaHeight) / mChannels.size());
            for (DrawableChannel channel : mChannels.values()){
                channel.setPosition(0, topY);
                channel.setSize(mDrawableAreaWidth, channelHeight);
                topY += channelHeight;
            }
            if (mSignalLoader!=null){
                mSignalLoader.onChannelHeightChanged(channelHeight);
            }
        }
    }
    
    private void setRulerSizes(){
        if (mRuler != null){
            mRuler.setViewTime(mViewTime);
            mRuler.setSecondCount(mHorizontalScale);
            mRuler.setPosition(0, mDrawableAreaHeight - mRulerHeight);
            mRuler.setSize(mDrawableAreaWidth, mRulerHeight);
        }
    }
    
    private void onSignalLoaded(Map<String, float[]> loadedSignals) {
        if (mChannels == null || loadedSignals == null){
            return;
        }
        for(String channel : loadedSignals.keySet()){
            if (!mChannels.containsKey(channel))
                continue;
            mChannels.get(channel).setSignal(loadedSignals.get(channel));
        }
    }
}
