package com.neuromd.emotionsample.signal.spectrum;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.DrawablesController;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;
import com.neuromd.neurosdk.channels.eeg.EegChannel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SpectrumDrawablesController extends DrawablesController {
    private Map<String, SpectrumDrawable> mChannels;
    private final int mRulerHeight = 25;
    private final int mRulerMaxFreq = 70;
    private float mViewTime;
    private SpectrumRuler mRuler;
    private SpectrumLoader mSignalLoader;
    
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
            mSignalLoader = new SpectrumLoader(eegChannels);
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

    @Override
    public void setViewTime(float time) {
        time = time - mHorizontalScale;
        if (mViewTime != time){
            mViewTime = time;
            if (mSignalLoader!=null){
                mSignalLoader.onViewTimeChanged(mViewTime);
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
    }
    
    private List<ISignalFieldDrawable> generateDrawables(Collection<EegChannel> eegChannels) {
        List<ISignalFieldDrawable> drawables = new ArrayList<>();
        mChannels = new HashMap<>();
        for (EegChannel channel : eegChannels){
            SpectrumDrawable drawChannel = new SpectrumDrawable(channel.info().getName());
            mChannels.put(String.format("Spectrum %s",channel.info().getName()), drawChannel);
            drawables.add(drawChannel);
        }
        setChannelSizes();
        mRuler = new SpectrumRuler();
        mRuler.setMaxFrequency(mRulerMaxFreq);
        setRulerSizes();
        drawables.add(mRuler);
        return drawables;
    }
    
    private void setChannelSizes(){
        if (mChannels != null){
            int topY = 0;
            int channelHeight = (int)((float)(mDrawableAreaHeight - mRulerHeight) / (mChannels.size() / 2));
            int firstRowCount = 0;
            int x = 0;
            for (SpectrumDrawable channel : mChannels.values()){
                channel.setPosition(x, topY);
                channel.setSize(mDrawableAreaWidth /2, channelHeight);
                topY += channelHeight;
                firstRowCount++;
                if (firstRowCount == 2 ) {
                    x = mDrawableAreaWidth / 2;
                    topY = 0;
                }
            }
            if (mSignalLoader!=null){
                mSignalLoader.onChannelHeightChanged(channelHeight);
            }
        }
    }
    
    private void setRulerSizes(){
        if (mRuler != null){
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
