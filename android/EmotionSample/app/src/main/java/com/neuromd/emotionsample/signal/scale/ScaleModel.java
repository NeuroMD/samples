package com.neuromd.emotionsample.signal.scale;

import com.neuromd.common.SubscribersNotifier;

import java.util.LinkedList;
import java.util.ListIterator;

public class ScaleModel {
    
    private ListIterator<ScaleValue> mVerticalScaleIterator;
    private ListIterator<ScaleValue> mHorizontalScaleIterator;
    private ScaleValue mVerticalScale;
    private ScaleValue mHorizontalScale;
    private final LinkedList<ScaleValue> mVerticalScales = new LinkedList<>();
    private final LinkedList<ScaleValue> mHorizontalScales = new LinkedList<>();
    
    public ScaleModel(){
        createVerticalScales();
        createHorizontalScales();
    }
    
    public SubscribersNotifier<ScaleValue> verticalScaleChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<ScaleValue> horizontalScaleChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> verticalScaleIncrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> verticalScaleDecrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> horizontalScaleIncrementButtonEnabledChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> horizontalScaleDecrementButtonEnabledChanged = new SubscribersNotifier<>();
    
    public ScaleValue getVerticalScale(){
        return mVerticalScale;
    }
    
    public ScaleValue getHorizontalScale(){
        return mHorizontalScale;
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
