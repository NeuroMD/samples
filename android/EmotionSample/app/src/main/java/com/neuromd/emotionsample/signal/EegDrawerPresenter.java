package com.neuromd.emotionsample.signal;

import android.os.Handler;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.drawables.DrawableChannel;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;
import com.neuromd.emotionsample.signal.drawables.Ruler;
import com.neuromd.emotionsample.signal.scale.ScaleModel;
import com.neuromd.emotionsample.signal.scale.ScaleValue;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class EegDrawerPresenter {
    private final int mRulerHeight = 25;
    private final ChannelsModel mChannelsModel;
    private final ScaleModel mScaleModel;
    private final EegDrawer mEegDrawer;
    
    private Map<String, DrawableChannel> mChannels;
    private Ruler mRuler;
    private SignalLoader mSignalLoader;
    private float mSignalStartTime;
    private float mViewTime;
    private double mScrollVelocity = 0.0;
    private int mDrawableAreaWidth = 0;
    private int mDrawableAreaHeight = 0;

    public EegDrawerPresenter(ChannelsModel channelsModel, ScaleModel scaleModel, EegDrawer signalDrawer){
        mChannelsModel = channelsModel;
        mScaleModel = scaleModel;
        mEegDrawer = signalDrawer;
    
        enableSignalScrolling();
        subscribeModelsEvents();
        subscribeDrawerEvents();
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
                        setViewTime(viewTime);
                    }
                }
                handler.postDelayed(this, 50);
            }
        };
        scrollVelocityRunnable.run();
    }
    
    private void subscribeModelsEvents(){
        mChannelsModel.selectedDeviceChanged.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, Device device) {
                onSelectedDeviceChanged(device);
            }
        });
        mChannelsModel.signalDurationChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double duration) {
                float signalTime = (float)(duration - mScaleModel.getHorizontalScale().getScaleValue());
                if (mViewTime < mSignalStartTime + 0.1 && mViewTime > mSignalStartTime - 0.1){
                    setViewTime(signalTime);
                }
                mSignalStartTime = signalTime;
            }
        });
        mScaleModel.horizontalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                setRulerSizes();
                if (mSignalLoader!=null){
                    mSignalLoader.onHorizontalScaleChanged(scaleValue.getScaleValue());
                }
            }
        });
        mScaleModel.verticalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                if (mSignalLoader!=null){
                    mSignalLoader.onVerticalScaleChanged(scaleValue.getScaleValue());
                }
            }
        });
    }
    
    private void subscribeDrawerEvents(){
        mEegDrawer.updatePending.subscribe(new INotificationCallback<UpdatePendingArgs>() {
            @Override
            public void onNotify(Object o, UpdatePendingArgs updatePendingArgs) {
                boolean needRedraw = false;
                if (mDrawableAreaWidth != updatePendingArgs.width) {
                    mDrawableAreaWidth = updatePendingArgs.width;
                    needRedraw = true;
                }
                if (mDrawableAreaHeight != updatePendingArgs.height) {
                    mDrawableAreaHeight = updatePendingArgs.height;
                    needRedraw = true;
                }
                if (needRedraw){
                    setChannelSizes();
                    setRulerSizes();
                }
            }
        });
    }
    
    private void onSelectedDeviceChanged(Device device){
        if (device == null){
            resetDrawableArea();
            if (mSignalLoader != null) {
                mSignalLoader.signalLoaded.unsubscribe();
                mSignalLoader = null;
            }
        }
        else {
            createDrawableArea();
            mSignalLoader = new SignalLoader(mChannelsModel.getChannels());
            mSignalLoader.signalLoaded.subscribe(new INotificationCallback<Map<String, float[]>>() {
                @Override
                public void onNotify(Object o, Map<String, float[]> loadedSignals) {
                    onSignalLoaded(loadedSignals);
                }
            });
            mSignalLoader.onHorizontalScaleChanged(mScaleModel.getHorizontalScale().getScaleValue());
            mSignalLoader.onVerticalScaleChanged(mScaleModel.getVerticalScale().getScaleValue());
            int channelHeight = (int)((float)(mDrawableAreaHeight) / mChannels.size());
            mSignalLoader.onChannelHeightChanged(channelHeight);
        }
    }
    
    private void onSignalLoaded(Map<String, float[]> loadedSignals) {
        for(String channel : loadedSignals.keySet()){
            mChannels.get(channel).setSignal(loadedSignals.get(channel));
        }
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
            mRuler.setSecondCount(mScaleModel.getHorizontalScale().getScaleValue());
            mRuler.setPosition(0, mDrawableAreaHeight - mRulerHeight);
            mRuler.setSize(mDrawableAreaWidth, mRulerHeight);
        }
    }
    
    private void createDrawableArea(){
        List<ISignalFieldDrawable> drawables = new ArrayList<>();
    
        List<EegChannel> eegChannels = mChannelsModel.getChannels();
        mChannels = new HashMap<>();
        for (EegChannel channel : eegChannels){
            DrawableChannel drawChannel = new DrawableChannel(channel.info().getName());
            mChannels.put(channel.info().getName(), drawChannel);
            drawables.add(drawChannel);
        }
        setChannelSizes();
        
        mRuler = new Ruler();
        mRuler.setSecondCount(mScaleModel.getHorizontalScale().getScaleValue());
        mRuler.setViewTime(mViewTime);
        setRulerSizes();
        drawables.add(mRuler);
        mEegDrawer.setDrawables(drawables);
    }
    
    private void setViewTime(float time){
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
    
    private void resetDrawableArea(){
        mEegDrawer.setDrawables(null);
        mChannels = null;
        mRuler = null;
    }

    public void setScrollVelocity(double xVelocity) {
        mScrollVelocity = xVelocity;
    }

    
}
