package com.neuromd.emotionsample.signal;


import android.os.Handler;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.emotionsample.EegDeviceModel;
import com.neuromd.emotionsample.ScaleValue;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.ListIterator;
import java.util.LinkedList;

public class EegDrawerPresenter {

    private EegDeviceModel mModel;
    private float[] mSignalBuffer;
    private Long[] mRPeaks;
    //private ArtifactZone[] mArtifacts;
    private ListIterator<ScaleValue> mVerticalScaleIterator;
    private ListIterator<ScaleValue> mHorizontalScaleIterator;
    private ScaleValue mVerticalScale;
    private ScaleValue mHorizontalScale;
    private double mSignalStartTime;
    private double mViewTime;
    private double mScrollVelocity = 0.0;
    private final LinkedList<ScaleValue> mVerticalScales = new LinkedList<>();
    private final LinkedList<ScaleValue> mHorizontalScales = new LinkedList<>();

    EegDrawerPresenter(EegDeviceModel model){

        mModel = model;
        createVerticalScales();
        createHorizontalScales();
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
    
    public SubscribersNotifier<ScaleValue> verticalScaleChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<ScaleValue> horizontalScaleChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> verticalScaleIncrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> verticalScaleDecrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> horizontalScaleIncrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> horizontalScaleDecrementButtonEnabledChanged = new SubscribersNotifier<>();

    public void updateSignalData(){

        if (mModel.getDeviceState() != DeviceState.Connected){
            mSignalStartTime = 0.0;
            mViewTime = 0.0;
            mSignalBuffer = null;
            //mRPeaks = null;
            return;
        }

        double signalTime = mModel.getTotalDuration() - mHorizontalScale.getScaleValue();
        if (mViewTime == mSignalStartTime){
            mViewTime = signalTime;
        }
        mSignalStartTime = signalTime;

        Double[] signalData = mModel.getSignalData(mViewTime, mHorizontalScale.getScaleValue());
        if (signalData == null || signalData.length==0){
            mSignalStartTime = 0.0;
            mViewTime = 0.0;
            mSignalBuffer = null;
            mRPeaks = null;
            return;
        }
        mRPeaks = mModel.getRPeaks(mViewTime, mViewTime + mHorizontalScale.getScaleValue());
        int requestedDataLength = getRequestedSamplesCount();
        float[] buffer = new float[requestedDataLength];
        int start = requestedDataLength - signalData.length;
        if (start >= 0) {
            for (int i = start; i < requestedDataLength; ++i) {
                buffer[i] = (float) (signalData[i - start] * 1e6);
            }
        }

        //mArtifacts = mModel.getArtifacts(mViewTime, mHorizontalScale.getScaleValue());

        mSignalBuffer = buffer;
    }

    public float[] getSignalBuffer(){
        return mSignalBuffer;
    }

    public Long[] getRPeaks(){
        return mRPeaks;
    }

    /*public ArtifactZone[] getArtifacts() { return mArtifacts;}*/

    public ScaleValue getVerticalScale(){
        return mVerticalScale;
    }

    public ScaleValue getHorizontalScale(){
        return mHorizontalScale;
    }

    public double getSignalViewTime(){
        return mViewTime;
    }

    public int getRequestedSamplesCount(){
        return mModel.getSamplingFrequency()*mHorizontalScale.getScaleValue();
    }

    public void setScrollVelocity(double xVelocity) {
        mScrollVelocity = xVelocity;
    }

    public void incrementVerticalScale(){
        if (mVerticalScaleIterator.hasNext()){
            ScaleValue nextValue = mVerticalScaleIterator.next();
            mVerticalScale = nextValue != mVerticalScale ? nextValue : mVerticalScaleIterator.next();
            verticalScaleChanged.sendNotification(this, mVerticalScale);
            if (!mVerticalScaleIterator.hasNext()){
                verticalScaleIncrementButtonEnabledChanged.sendNotification(this, false);
            }
            if (mVerticalScaleIterator.hasPrevious()){
                verticalScaleDecrementButtonEnabledChanged.sendNotification(this, true);
            }
        }
    }

    public void decrementVerticalScale(){
        if (mVerticalScaleIterator.hasPrevious()){
            ScaleValue prevValue = mVerticalScaleIterator.previous();
            mVerticalScale = prevValue != mVerticalScale ? prevValue : mVerticalScaleIterator.previous();
            verticalScaleChanged.sendNotification(this, mVerticalScale);
            if (!mVerticalScaleIterator.hasPrevious()){
                verticalScaleDecrementButtonEnabledChanged.sendNotification(this, false);
            }
            if (mVerticalScaleIterator.hasNext()){
                verticalScaleIncrementButtonEnabledChanged.sendNotification(this, true);
            }
        }
    }

    public void incrementHorizontalScale(){
        if (mHorizontalScaleIterator.hasNext()){
            ScaleValue nextValue = mHorizontalScaleIterator.next();
            mHorizontalScale = nextValue != mHorizontalScale ? nextValue : mHorizontalScaleIterator.next();
            horizontalScaleChanged.sendNotification(this, mHorizontalScale);
            if (!mHorizontalScaleIterator.hasNext()){
                horizontalScaleIncrementButtonEnabledChanged.sendNotification(this, false);
            }
            if (mHorizontalScaleIterator.hasPrevious()){
                horizontalScaleDecrementButtonEnabledChanged.sendNotification(this, true);
            }
        }
    }

    public void decrementHorizontalScale(){
        if (mHorizontalScaleIterator.hasPrevious()){
            ScaleValue prevValue = mHorizontalScaleIterator.previous();
            mHorizontalScale = prevValue != mHorizontalScale ? prevValue : mHorizontalScaleIterator.previous();
            horizontalScaleChanged.sendNotification(this, mHorizontalScale);
            if (!mHorizontalScaleIterator.hasPrevious()){
                horizontalScaleDecrementButtonEnabledChanged.sendNotification(this, false);
            }
            if (mHorizontalScaleIterator.hasNext()){
                horizontalScaleIncrementButtonEnabledChanged.sendNotification(this, true);
            }
        }
    }

    private void createHorizontalScales() {
        mHorizontalScales.add(new ScaleValue("1 s", 1));
        mHorizontalScales.add(new ScaleValue("2 s", 2));
        mHorizontalScales.add(new ScaleValue("5 s", 5));
        mHorizontalScales.add(new ScaleValue("10 s", 10));
        mHorizontalScaleIterator = mHorizontalScales.listIterator(2);
        mHorizontalScale = mHorizontalScales.get(2);
    }

    private void createVerticalScales() {
        mVerticalScales.add(new ScaleValue("1 uV", 1));
        mVerticalScales.add(new ScaleValue("2 uV", 2));
        mVerticalScales.add(new ScaleValue("5 uV", 5));
        mVerticalScales.add(new ScaleValue("10 uV", 10));
        mVerticalScales.add(new ScaleValue("20 uV", 20));
        mVerticalScales.add(new ScaleValue("50 uV", 50));
        mVerticalScales.add(new ScaleValue("70 uV", 70));
        mVerticalScales.add(new ScaleValue("100 uV", 100));
        mVerticalScales.add(new ScaleValue("200 uV", 200));
        mVerticalScales.add(new ScaleValue("500 uV", 500));
        mVerticalScales.add(new ScaleValue("1 mV", 1000));
        mVerticalScales.add(new ScaleValue("2 mV", 2000));
        mVerticalScales.add(new ScaleValue("5 mV", 5000));
        mVerticalScales.add(new ScaleValue("10 mV", 10000));
        mVerticalScales.add(new ScaleValue("25 mV", 25000));
        mVerticalScales.add(new ScaleValue("50 mV", 50000));
        mVerticalScales.add(new ScaleValue("100 mV", 100000));
        mVerticalScaleIterator = mVerticalScales.listIterator(6);
        mVerticalScale = mVerticalScales.get(6);
    }
}
