package com.neuromd.emotionsample.signal;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.scale.ScaleModel;
import com.neuromd.emotionsample.signal.scale.ScaleValue;
import com.neuromd.emotionsample.signal.spectrum.SpectrumDrawablesController;
import com.neuromd.neurosdk.Device;

public class EegDrawerPresenter {
    private final EegDrawer mEegDrawer;
    private final ChannelsModel mChannelsModel;
    private final ScaleModel mScaleModel;
    private final SignalDrawablesController mSignalController;
    private final SpectrumDrawablesController mSpectrumController;
    
    private DrawablesController mCurrentController;


    public EegDrawerPresenter(ChannelsModel channelsModel, ScaleModel scaleModel, EegDrawer signalDrawer){
        mChannelsModel = channelsModel;
        mScaleModel = scaleModel;
        mEegDrawer = signalDrawer;
        mSignalController = new SignalDrawablesController();
        mSpectrumController = new SpectrumDrawablesController();
        setViewMode(ViewMode.Signal);
        subscribeModelsEvents();
        subscribeDrawerEvents();
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
                mCurrentController.setViewTime(duration.floatValue());
            }
        });
        mScaleModel.horizontalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                mCurrentController.setHorizontalScale(scaleValue.getScaleValue());
            }
        });
        mScaleModel.verticalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object o, ScaleValue scaleValue) {
                mCurrentController.setVerticalScale(scaleValue.getScaleValue());
            }
        });
    }
    
    private void subscribeDrawerEvents(){
        mEegDrawer.updatePending.subscribe(new INotificationCallback<UpdatePendingArgs>() {
            @Override
            public void onNotify(Object o, UpdatePendingArgs updatePendingArgs) {
                mCurrentController.setDrawableAreaSize(
                        updatePendingArgs.width,
                        updatePendingArgs.height
                );
            }
        });
    }
    
    private void onSelectedDeviceChanged(Device device){
        if (device == null){
            mSignalController.setChannels(null);
            mSpectrumController.setChannels(null);
        }
        else {
            mSignalController.setChannels(mChannelsModel.getChannels());
            mSpectrumController.setChannels(mChannelsModel.getChannels());
        }
        mEegDrawer.setDrawables(mCurrentController.getDrawables());
    }

    public void setScrollVelocity(double xVelocity) {
        mSignalController.setScrollVelocity(xVelocity);
    }

    public void setViewMode(ViewMode mode){
        if (mode == ViewMode.Signal){
            mCurrentController = mSignalController;
        }
        else{
            mCurrentController = mSpectrumController;
        }
        mCurrentController.setVerticalScale(mScaleModel.getVerticalScale().getScaleValue());
        mCurrentController.setHorizontalScale(mScaleModel.getHorizontalScale().getScaleValue());
        mEegDrawer.setDrawables(mCurrentController.getDrawables());
    }
}
