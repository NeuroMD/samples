package com.neuromd.emotionsample;

import android.app.Activity;
import android.widget.Toast;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;

public class EegParamsPresenter {

    private final Activity mActivity;
    private final EegDeviceModel mModel;
    private String mSignalDurationText = "0 s";

    public EegParamsPresenter(Activity activity, EegDeviceModel model){
        mActivity = activity;
        mModel = model;
        mModel.signalDurationChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double signalDuration) {
                mSignalDurationText = String.format("%.2f s",signalDuration);
                notifySignalDurationTextChanged();
            }
        });
    }

    public SubscribersNotifier<String> signalDurationTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> electrodesStateTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> heartRateTextChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<String> stressIndexTextChanged = new SubscribersNotifier<>();

    public String getSignalDurationText(){
        return mSignalDurationText;
    }


    private void notifySignalDurationTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                signalDurationTextChanged.sendNotification(this, mSignalDurationText);
            }
        });
    }

    private void showTextMessage(String text){
        Toast.makeText(mActivity, text, Toast.LENGTH_SHORT).show();
    }
}
