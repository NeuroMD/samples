package com.neuromd.emotionsample.signal;

import android.os.Handler;

import com.neuromd.emotionsample.device.EegDeviceModel;
import com.neuromd.emotionsample.signal.scale.ScaleModel;


public class EegDrawerPresenter {
    
    private final int mRulerHeight = 25;
    private ChannelsModel mChannelsModel;
    private ScaleModel mScaleModel;
    private float[] mSignalBuffer;

    private double mSignalStartTime;
    private double mViewTime;
    private double mScrollVelocity = 0.0;

    public EegDrawerPresenter(ChannelsModel channelsModel, ScaleModel scaleModel, EegDrawer signalDrawer){

        mChannelsModel = channelsModel;
        mScaleModel = scaleModel;
        
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
                    if (mViewTime < 0.0)
                        mViewTime = 0.0;
                    else if (mViewTime > mSignalStartTime)
                        mViewTime = mSignalStartTime;
                }

                handler.postDelayed(this, 50);
            }
        };
        scrollVelocityRunnable.run();
    }
    
    public void updateSignalData(){

        /*if (mDeviceModel.getDeviceState() != DeviceState.Connected){
            mSignalStartTime = 0.0;
            mViewTime = 0.0;
            mSignalBuffer = null;
            //mRPeaks = null;
            return;
        }

        double signalTime = mDeviceModel.getTotalDuration() - mHorizontalScale.getScaleValue();
        if (mViewTime == mSignalStartTime){
            mViewTime = signalTime;
        }
        mSignalStartTime = signalTime;

        Double[] signalData = mDeviceModel.getSignalData(mViewTime, mHorizontalScale.getScaleValue());
        if (signalData == null || signalData.length==0){
            mSignalStartTime = 0.0;
            mViewTime = 0.0;
            mSignalBuffer = null;
            return;
        }
        
        int requestedDataLength = getRequestedSamplesCount();
        float[] buffer = new float[requestedDataLength];
        int start = requestedDataLength - signalData.length;
        if (start >= 0) {
            for (int i = start; i < requestedDataLength; ++i) {
                buffer[i] = (float) (signalData[i - start] * 1e6);
            }
        }

        mSignalBuffer = buffer;*/
    }

    public float[] getSignalBuffer(){
        return mSignalBuffer;
    }

    public double getSignalViewTime(){
        return mViewTime;
    }

    public int getRequestedSamplesCount(){
       // return mModel.getSamplingFrequency() * mHorizontalScale.getScaleValue();
        return 0;
    }

    public void setScrollVelocity(double xVelocity) {
        mScrollVelocity = xVelocity;
    }

    
}
