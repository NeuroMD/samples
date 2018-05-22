package com.neuromd.emotionsample.signal;

import android.os.Handler;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.drawables.DrawableChannel;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;
import com.neuromd.emotionsample.signal.drawables.Ruler;
import com.neuromd.emotionsample.signal.scale.ScaleModel;
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
        
        mChannelsModel.selectedDeviceChanged.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, Device device) {
                if (device == null){
                    resetDrawableArea();
                }
                else {
                    createDrawableArea();
                }
            }
        });
        mEegDrawer.updatePending.subscribe(new INotificationCallback<UpdatePendingArgs>() {
            @Override
            public void onNotify(Object o, UpdatePendingArgs updatePendingArgs) {
                //if (mDrawableAreaWidth != updatePendingArgs.width ||
                 //       mDrawableAreaHeight != updatePendingArgs.height) {
                    mDrawableAreaWidth = updatePendingArgs.width;
                    mDrawableAreaHeight = updatePendingArgs.height;
                    onDrawableAreaSizeChanged();
                //}
            }
        });
    }
    
    private void onDrawableAreaSizeChanged() {
        if (mChannels != null){
            int topY = 0;
            int channelHeight = (int)((float)(mDrawableAreaHeight - mRulerHeight) / mChannels.size());
            for (DrawableChannel channel : mChannels.values()){
                channel.setPosition(0, topY);
                channel.setSize(mDrawableAreaWidth, channelHeight);
                topY += channelHeight;
            }
        }
        if (mRuler != null){
            mRuler.setViewTime(mViewTime);
            mRuler.setSecondCount(mScaleModel.getHorizontalScale().getScaleValue());
            mRuler.setPosition(0, mDrawableAreaHeight - mRulerHeight);
            mRuler.setSize(mDrawableAreaWidth, mRulerHeight);
        }
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
            
                if (mViewTime>=0.0) {
                    mViewTime -= mScrollVelocity;
                    if (mViewTime < 0.0f)
                        mViewTime = 0.0f;
                    else if (mViewTime > mSignalStartTime)
                        mViewTime = mSignalStartTime;
                }
            
                handler.postDelayed(this, 50);
            }
        };
        scrollVelocityRunnable.run();
    }
    
    private void createDrawableArea(){
        List<EegChannel> eegChannels = mChannelsModel.getChannels();
        List<ISignalFieldDrawable> drawables = new ArrayList<>();
        mChannels = new HashMap<>();
        for (final EegChannel channel : eegChannels){
            DrawableChannel drawChannel = new DrawableChannel(channel.info().getName());
            mChannels.put(channel.info().getName(), drawChannel);
            channel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object o, Long length) {
                    int seconds = mScaleModel.getHorizontalScale().getScaleValue();
                    mSignalStartTime = (float)length / 250f;
                    mRuler.setViewTime(mViewTime);
                }
            });
            drawables.add(drawChannel);
        }
        mRuler = new Ruler();
        mRuler.setSecondCount(mScaleModel.getHorizontalScale().getScaleValue());
        mRuler.setViewTime(mViewTime);
        drawables.add(mRuler);
        mEegDrawer.setDrawables(drawables);
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
