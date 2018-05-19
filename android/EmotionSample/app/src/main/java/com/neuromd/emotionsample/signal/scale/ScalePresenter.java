package com.neuromd.emotionsample.signal.scale;

import com.neuromd.common.INotificationCallback;

public class ScalePresenter {
    private final ScaleModel mModel;
    private final ScaleControlView mView;
    
    public ScalePresenter(ScaleModel model, ScaleControlView view){
        mModel = model;
        mView = view;
        
        initHorizontalScaleControls();
        initVerticalScaleControls();
    }
    
    private void initHorizontalScaleControls(){
        mView.setHorizontalScaleText(mModel.getHorizontalScale().getLabel());
        mModel.horizontalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                mView.setHorizontalScaleText(scaleValue.getLabel());
            }
        });
        mModel.horizontalScaleDecrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                mView.setHorizontalMinusButtonEnabled(isEnabled);
            }
        });
        mModel.horizontalScaleIncrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                mView.setHorizontalPlusButtonEnabled(isEnabled);
            }
        });
        
        mView.horizontalMinusClicked.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mModel.decrementHorizontalScale();
            }
        });
        mView.horizontalPlusClicked.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mModel.incrementHorizontalScale();
            }
        });
    }
    
    private void initVerticalScaleControls(){
        mView.setVerticalScaleText(mModel.getVerticalScale().getLabel());
        mModel.verticalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                mView.setVerticalScaleText(scaleValue.getLabel());
            }
        });
        mModel.verticalScaleDecrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                mView.setVerticalMinusButtonEnabled(isEnabled);
            }
        });
        mModel.verticalScaleIncrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                mView.setVerticalPlusButtonEnabled(isEnabled);
            }
        });
    
        mView.verticalMinusClicked.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mModel.decrementVerticalScale();
            }
        });
        mView.verticalPlusClicked.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                mModel.incrementVerticalScale();
            }
        });
    }
}
