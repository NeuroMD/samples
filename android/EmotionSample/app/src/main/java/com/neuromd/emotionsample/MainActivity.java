package com.neuromd.emotionsample;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.LocationManager;
import android.os.Build;
import android.support.annotation.NonNull;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.VelocityTracker;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.crashlytics.android.Crashlytics;
import io.fabric.sdk.android.Fabric;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.device.DeviceControlsPresenter;
import com.neuromd.emotionsample.device.EegDeviceModel;
import com.neuromd.emotionsample.device.IDeviceControlsView;
import com.neuromd.emotionsample.drawer.GraphicsView;
import com.neuromd.emotionsample.emotions.EmotionIndicator;
import com.neuromd.emotionsample.emotions.EmotionParametersPresenter;
import com.neuromd.emotionsample.emotions.IEmotionValuesView;
import com.neuromd.emotionsample.params.EegParamsPresenter;
import com.neuromd.emotionsample.params.IDeviceParamsView;
import com.neuromd.emotionsample.signal.ChannelsModel;
import com.neuromd.emotionsample.signal.EegDrawerPresenter;
import com.neuromd.emotionsample.signal.EegDrawingEngine;
import com.neuromd.emotionsample.signal.scale.ScaleControlView;
import com.neuromd.emotionsample.signal.scale.ScaleModel;
import com.neuromd.emotionsample.signal.scale.ScalePresenter;

public class MainActivity extends AppCompatActivity implements IEmotionValuesView,
                                                                IDeviceControlsView,
                                                                IDeviceParamsView {
    private EmotionParametersPresenter mEmotionParamsPresenter;
    private EegParamsPresenter mDeviceParamsPresenter;
    private DeviceControlsPresenter mDeviceControlsPresenter;
    private EegDrawerPresenter mEegDrawerPresenter;
    private ScalePresenter mScalePresenter;
    private VelocityTracker mVelocityTracker;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Fabric.with(this, new Crashlytics());
        setContentView(R.layout.activity_main);

        requestPermissions();
        enableBluetooth();
    
        EegDeviceModel deviceModel = new EegDeviceModel(getApplicationContext());
        ChannelsModel channelsModel = new ChannelsModel();
        ScaleModel scaleModel = new ScaleModel();
        initDeviceControls(deviceModel, channelsModel);
        initScaleView(scaleModel);
        initDrawer(channelsModel, scaleModel);
        initEmotionIndicator(channelsModel);
        initDeviceButtons();
        initSignalButtons();
    }
  
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        int index = event.getActionIndex();
        int action = event.getActionMasked();
        int pointerId = event.getPointerId(index);

        switch (action) {
            case MotionEvent.ACTION_DOWN:
                if (mVelocityTracker == null) {
                    mVelocityTracker = VelocityTracker.obtain();
                } else {
                    mVelocityTracker.clear();
                }
                mVelocityTracker.addMovement(event);
                break;
            case MotionEvent.ACTION_MOVE:
                mVelocityTracker.addMovement(event);
                mVelocityTracker.computeCurrentVelocity(1);
                mEegDrawerPresenter.setScrollVelocity(mVelocityTracker.getXVelocity(pointerId));
                break;
            case MotionEvent.ACTION_UP:
                break;
            case MotionEvent.ACTION_CANCEL:
                mVelocityTracker.recycle();
                break;
        }
        return true;
    }

    private void initSignalButtons(){
        /*final Button startSignalButton = (Button)findViewById(R.id.startSignalButton);
        startSignalButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onStartSignalButtonClick();
            }
        });
        mMainPresenter.startSignalButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                startSignalButton.setEnabled(isEnabled);
            }
        });
        startSignalButton.setEnabled(mMainPresenter.isStartSignalButtonEnabled());


        final Button stopSignalButton = (Button)findViewById(R.id.stopSignalButton);
        stopSignalButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onStopSignalButtonClick();
            }
        });
        mMainPresenter.stopSignalButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                stopSignalButton.setEnabled(isEnabled);
            }
        });
        stopSignalButton.setEnabled(mMainPresenter.isStopSignalButtonEnabled());


        final Button removeDeviceButton = (Button)findViewById(R.id.removeDeviceButton);
        removeDeviceButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onRemoveDeviceButtonClick();
            }
        });
        mMainPresenter.removeDeviceButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                removeDeviceButton.setEnabled(isEnabled);
            }
        });
        removeDeviceButton.setEnabled(mMainPresenter.isRemoveDeviceButtonEnabled());*/
    }

    private void initDeviceButtons(){
       /*
        mMainPresenter.startScanButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                startScanButton.setEnabled(isEnabled);
            }
        });
        startScanButton.setEnabled(mMainPresenter.isStartScanButtonEnabled());

        final Button stopScanButton = (Button)findViewById(R.id.stopScanButton);
        stopScanButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onStopScanButtonClick();
            }
        });
        mMainPresenter.stopScanButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                stopScanButton.setEnabled(isEnabled);
            }
        });
        stopScanButton.setEnabled(mMainPresenter.isStopScanButtonEnabled());*/
    }
    
    private void initScaleView(ScaleModel model){
        LinearLayout scaleLayout = findViewById(R.id.scaleControlsLayout);
        ScaleControlView scaleView = new ScaleControlView(this);
        mScalePresenter = new ScalePresenter(model, scaleView);
        scaleLayout.addView(scaleView);
    }
    
    private void initDrawer(ChannelsModel channelsModel, ScaleModel scaleModel){
        EegDrawingEngine drawerEngine = new EegDrawingEngine();
        mEegDrawerPresenter = new EegDrawerPresenter(channelsModel, scaleModel, drawerEngine);
        GraphicsView drawerView = new GraphicsView(this, drawerEngine);
        LinearLayout drawLayout = findViewById(R.id.drawLayout);
        drawLayout.addView(drawerView);
    }
    
    private void initEmotionIndicator(ChannelsModel channelsModel) {
        LinearLayout indicatorLayout = findViewById(R.id.indicatorLayout);
        EmotionIndicator indicatorView = new EmotionIndicator(this);
        mEmotionParamsPresenter = new EmotionParametersPresenter(channelsModel, indicatorView, this);
        indicatorLayout.addView(indicatorView);
    }
    
    private void initDeviceControls(EegDeviceModel deviceModel, ChannelsModel channelsModel){
        mDeviceControlsPresenter = new DeviceControlsPresenter(deviceModel, channelsModel, this);
        mDeviceParamsPresenter = new EegParamsPresenter(channelsModel, this);
    }
    
    @Override
    public void setProductiveRelaxLabel(String text) {
    
    }
    
    @Override
    public void setStressLabel(String text) {
    
    }
    
    @Override
    public void setAttentionLabel(String text) {
    
    }
    
    @Override
    public void setMeditationLabel(String text) {
    
    }
    
    @Override
    public void setDurationText(String text) {
    
    }
    
    @Override
    public void setBatteryLevelText(String text) {
    
    }
    
    @Override
    public void setDeviceText(final String text) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                TextView deviceTextView = findViewById(R.id.deviceTextView);
                deviceTextView.setText(text);
            }
        });
    }
    
    @Override
    public void setProgressVisible(final boolean isVisible) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                ProgressBar connectionProgressBar = findViewById(R.id.connectionProgressBar);
                connectionProgressBar.setVisibility(isVisible ? View.VISIBLE : View.GONE);
            }
        });
    }
    
    @Override
    public void setReconnectButtonEnabled(final boolean isEnabled) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Button reconnectButton = findViewById(R.id.reconnectButton);
                reconnectButton.setEnabled(isEnabled);
            }
        });
    }
    
    @Override
    public void enableBluetooth() {
        Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
        startActivityForResult(enableBtIntent, 1);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M){
            final LocationManager manager = (LocationManager) getSystemService(Context.LOCATION_SERVICE );
            if ( !manager.isProviderEnabled( LocationManager.GPS_PROVIDER ) ) {
                Intent enableGeoIntent = new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
                startActivityForResult(enableGeoIntent, 1);
            }
        }
    }
    
    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String permissions[], @NonNull int[] grantResults)
    {
        if (requestCode == 1)
        {
            if (grantResults[0] != PackageManager.PERMISSION_GRANTED)
            {
                final AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.setTitle("Functionality limited");
                builder.setMessage("Since location access has not been granted, this app will not be able to discover beacons when in the background.");
                builder.setPositiveButton(android.R.string.ok, null);
                builder.setOnDismissListener(new DialogInterface.OnDismissListener()
                {
                    @Override
                    public void onDismiss(DialogInterface dialog)
                    {
                    
                    }
                });
                builder.show();
            }
        }
    }
    
    @Override
    public void requestPermissions(){
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
        {
            requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.WRITE_EXTERNAL_STORAGE}, 1);
        }
    }
}
