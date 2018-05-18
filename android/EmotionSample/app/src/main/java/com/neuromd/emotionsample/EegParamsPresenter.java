package com.neuromd.emotionsample;

import android.app.Activity;
import android.widget.Toast;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;

public class EegParamsPresenter {

    private final Activity mActivity;
    private final EegDeviceModel mModel;
    private String mSignalDurationText = "0 s";
    private String mElectrodesStateText = "DETACHED";
    private String mHeartRateText = "0";
    private String mStressIndexText = "0";

    public EegParamsPresenter(Activity activity, EegDeviceModel model){
        mActivity = activity;
        mModel = model;
        mSignalStorage = new EcgSignalStorage(model);
        mSignalStorage.signalFileSaved.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object o, Object o2) {
                onSignalSaved();
            }
        });
        mSignalStorage.fileSaveError.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String error) {
                onSaveError(error);
            }
        });
        mModel.signalDurationChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double signalDuration) {
                mSignalDurationText = String.format("%.2f s",signalDuration);
                notifySignalDurationTextChanged();
            }
        });
        mModel.electrodesStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isAttached) {
                mElectrodesStateText = isAttached ? "ATTACHED" : "DETACHED";
                notifyElectrodesStateTextChanged();
            }
        });
        mModel.heartRateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object o, Integer heartRate) {
                mHeartRateText = String.valueOf(heartRate);
                notifyHeartRateTextChanged();
            }
        });
        mModel.stressIndexChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double stressIndex) {
                mStressIndexText = String.format("%.2f",stressIndex);
                notifyStressIndexTextChanged();
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

    public String getElectrodesStateText(){
        return mElectrodesStateText;
    }

    public String getHeartRateText(){
        return mHeartRateText;
    }

    public String getStressIndexText(){
        return mStressIndexText;
    }


    public void onSaveButtonClick(){
        mSignalStorage.save();
    }


    private void notifySignalDurationTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                signalDurationTextChanged.sendNotification(this, mSignalDurationText);
            }
        });
    }

    private void notifyElectrodesStateTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                electrodesStateTextChanged.sendNotification(this, mElectrodesStateText);
            }
        });
    }

    private void notifyHeartRateTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                heartRateTextChanged.sendNotification(this, mHeartRateText);
            }
        });
    }

    private void notifyStressIndexTextChanged(){
        mActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                stressIndexTextChanged.sendNotification(this, mStressIndexText);
            }
        });
    }

    private void onSignalSaved(){
        showTextMessage("Signal data saved");
    }

    private void onSaveError(String error){
        showTextMessage(error);
    }

    private void showTextMessage(String text){
        Toast.makeText(mActivity, text, Toast.LENGTH_SHORT).show();
    }
}
