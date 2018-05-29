package com.neuromd.emotionsample.emotions;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.emotionsample.signal.ChannelsModel;
import com.neuromd.neurosdk.channels.eeg.EmotionalState;

public class EmotionParametersPresenter {
    private final ChannelsModel mModel;
    private final IEmotionIndicatorView mIndicatorView;
    private final IEmotionValuesView mValuesView;
    
    public SubscribersNotifier<Boolean> calculationStateChanged = new SubscribersNotifier<>();
    
    public EmotionParametersPresenter(ChannelsModel model,
                                      IEmotionIndicatorView indicatorView,
                                      IEmotionValuesView valuesView){
    
        mModel = model;
        mIndicatorView = indicatorView;
        mValuesView = valuesView;
        
        setEmotionLabelsText();
        subscribeModelEvents();
    }
    
    public void onStartStopCalcClicked(){
        if (mModel.isCalculationInProc()){
            mModel.stopCalculations();
        }
        else{
            mModel.startCalculations();
        }
    }
    
    private void subscribeModelEvents(){
        mModel.calculationStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isCalculating) {
                mValuesView.setCalculationButtonText(isCalculating ? "STOP CALCULATION" : "START CALCULATION");
            }
        });
        mModel.emotionStateChanged.subscribe(new INotificationCallback<EmotionalState>() {
            @Override
            public void onNotify(Object o, EmotionalState state) {
                setEmotionLabelsText();
            }
        });
    }
    
    private void setEmotionLabelsText(){
        EmotionalState emotionValue = mModel.getEmotionState();
        if (emotionValue != null) {
            mIndicatorView.setValue(emotionValue.State);
            mValuesView.setAttentionLabel(String.format("%d %%", emotionValue.Attention));
            mValuesView.setProductiveRelaxLabel(String.format("%d %%", emotionValue.Relax));
            mValuesView.setStressLabel(String.format("%d %%", emotionValue.Stress));
            mValuesView.setMeditationLabel(String.format("%d %%", emotionValue.Meditation));
        }
        else{
            mIndicatorView.setValue(0);
            mValuesView.setAttentionLabel(String.format("%d %%", 0));
            mValuesView.setProductiveRelaxLabel(String.format("%d %%", 0));
            mValuesView.setStressLabel(String.format("%d %%", 0));
            mValuesView.setMeditationLabel(String.format("%d %%", 0));
        }
    }
}
