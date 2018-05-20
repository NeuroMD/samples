package com.neuromd.emotionsample.emotions;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.ChannelsModel;

public class EmotionParametersPresenter {
    private final ChannelsModel mModel;
    private final IEmotionIndicatorView mIndicatorView;
    private final IEmotionValuesView mValuesView;
    
    public EmotionParametersPresenter(ChannelsModel model,
                                      IEmotionIndicatorView indicatorView,
                                      IEmotionValuesView valuesView){
    
        mModel = model;
        mIndicatorView = indicatorView;
        mValuesView = valuesView;
        
        setInitialValues();
        subscribeModelEvents();
    }
    
    private void setInitialValues(){
        setEmotionalIndicatorValue();
        setAttentionText();
        setMeditationText();
        setProductiveRelaxText();
        setStressText();
    }
    
    private void subscribeModelEvents(){
        mModel.emotionStateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                setEmotionalIndicatorValue();
            }
        });
        mModel.attentionValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                setAttentionText();
            }
        });
        mModel.meditationValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                setMeditationText();
            }
        });
        mModel.productiveRelaxValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                setProductiveRelaxText();
            }
        });
        mModel.stressValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer value) {
                setStressText();
            }
        });
    }
    
    private void setEmotionalIndicatorValue(){
        int emotionValue = mModel.getEmotionalValue();
        mIndicatorView.setValue(emotionValue);
    }
    
    private void setAttentionText(){
        int attentionValue = mModel.getAttentionValue();    
        mValuesView.setAttentionLabel(String.format("%d %%", attentionValue));
    }
    
    private void setProductiveRelaxText(){
        int productiveRelaxValue = mModel.getProductiveRelaxValue();
        mValuesView.setProductiveRelaxLabel(String.format("%d %%", productiveRelaxValue));
    }
    
    private void setStressText(){
        int stressValue = mModel.getStressValue();
        mValuesView.setStressLabel(String.format("%d %%", stressValue));
    }
    
    private void setMeditationText(){
        int meditationValue = mModel.getMeditationValue();
        mValuesView.setMeditationLabel(String.format("%d %%", meditationValue));
    }
}
