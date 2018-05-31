package com.neuromd.emotionsample.signal;

import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.Collection;
import java.util.List;

public abstract class DrawablesController {
    protected List<ISignalFieldDrawable> mDrawables;
    protected int mVerticalScale;
    protected int mHorizontalScale;
    protected int mDrawableAreaWidth;
    protected int mDrawableAreaHeight;
    
    public List<ISignalFieldDrawable> getDrawables(){
        return mDrawables;
    }
    
    public abstract void setViewTime(float time);
    
    public void setVerticalScale(int vScale){
        mVerticalScale = vScale ;
        onVerticalScaleChanged(mVerticalScale);
    }
    
    public void setHorizontalScale(int hScale){
        mHorizontalScale = hScale;
        onHorizontalScaleChanged(mHorizontalScale);
    }
    
    public void setDrawableAreaSize(int width, int height){
        boolean notify = false;
        if (mDrawableAreaWidth != width) {
            mDrawableAreaWidth = width;
            notify = true;
        }
        if (mDrawableAreaHeight != height) {
            mDrawableAreaHeight = height;
            notify = true;
        }
        if (notify){
            onSizeChanged(mDrawableAreaWidth, mDrawableAreaHeight);
        }
    }
    
    public abstract void setChannels(Collection<EegChannel> eegChannels);

    protected abstract void onSizeChanged(int width, int height);
    protected abstract void onVerticalScaleChanged(int vScale);
    protected abstract void onHorizontalScaleChanged(int hScale);
}
