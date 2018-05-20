package com.neuromd.emotionsample.params;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.signal.ChannelsModel;

public class EegParamsPresenter {

    private final IDeviceParamsView mView;
    private final ChannelsModel mModel;

    public EegParamsPresenter(ChannelsModel model, IDeviceParamsView view){
        mView = view;
        mModel = model;
        mModel.signalDurationChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double signalDuration) {
               mView.setDurationText(String.format("%.2f s",signalDuration));
            }
        });
        mModel.batteryStateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer batteryLevel) {
                mView.setBatteryLevelText(String.format("%d %%", batteryLevel));
            }
        });
    }
}
