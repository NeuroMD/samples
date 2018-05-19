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
import android.widget.LinearLayout;
import android.widget.TextView;

import com.crashlytics.android.Crashlytics;
import io.fabric.sdk.android.Fabric;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.drawer.GraphicsView;
import com.neuromd.emotionsample.emotions.EmotionIndicator;
import com.neuromd.emotionsample.emotions.EmotionParametersPresenter;
import com.neuromd.emotionsample.emotions.IEmotionValuesView;
import com.neuromd.emotionsample.signal.EegDrawerPresenter;
import com.neuromd.emotionsample.signal.EegDrawingEngine;
import com.neuromd.emotionsample.signal.scale.ScaleControlView;
import com.neuromd.emotionsample.signal.scale.ScaleModel;
import com.neuromd.emotionsample.signal.scale.ScalePresenter;

public class MainActivity extends AppCompatActivity implements IEmotionValuesView {
    private EmotionParametersPresenter mEmotionParamsPresenter;
    private EegParamsPresenter mDeviceParamsPresenter;
    private EegDrawerPresenter mEegDrawerPresenter;
    private ScalePresenter mScalePresenter;
    private VelocityTracker mVelocityTracker;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Fabric.with(this, new Crashlytics());
        setContentView(R.layout.activity_main);

        requestPermissions();
        enableBtAndGeolocation();

        EegDeviceModel deviceModel = createAndInitModel();
        mDeviceParamsPresenter = new EegParamsPresenter(this, deviceModel);
        ScaleModel scaleModel = new ScaleModel();
        initScaleView(scaleModel);
        initDrawer(deviceModel, scaleModel);
        initParamLabels(deviceModel);
        initEmotionIndicator(deviceModel);
        
        initScanButtons();
        initSignalButtons();
        initParamsLabels();
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

    private void requestPermissions(){
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
        {
            requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.WRITE_EXTERNAL_STORAGE}, 1);
        }
    }

    private void enableBtAndGeolocation(){
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

    private EegDeviceModel createAndInitModel(){
        EegDeviceModel model = new EegDeviceModel(getApplicationContext());
        model.bluetoothAdapterEnableNeeded.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object sender, Object param/*null*/) {
                enableBtAndGeolocation();
            }
        });

        return model;
    }

    

    private void initParamsLabels(){
       /* final TextView batteryTextView = (TextView)findViewById(R.id.batteryTextView);
        mMainPresenter.batteryStateTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String batteryText) {
                batteryTextView.setText(batteryText);
            }
        });

        final TextView deviceStateTextView = (TextView)findViewById(R.id.stateTextView);
        mMainPresenter.deviceStateTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String stateText) {
                deviceStateTextView.setText(stateText);
            }
        });*/
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

    private void initScanButtons(){
       /* final Button startScanButton = (Button)findViewById(R.id.startScanButton);
        startScanButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onStartScanButtonClick();
            }
        });
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

    private void initParamLabels(EegDeviceModel deviceModel){
    
    }
    
    private void initScaleView(ScaleModel model){
        LinearLayout scaleLayout = findViewById(R.id.scaleControlsLayout);
        ScaleControlView scaleView = new ScaleControlView(this);
        mScalePresenter = new ScalePresenter(model, scaleView);
        scaleLayout.addView(scaleView);
    }
    
    private void initDrawer(EegDeviceModel deviceModel, ScaleModel scaleModel){
        EegDrawingEngine drawerEngine = new EegDrawingEngine();
        mEegDrawerPresenter = new EegDrawerPresenter(deviceModel, scaleModel, drawerEngine);
        GraphicsView drawerView = new GraphicsView(this, drawerEngine);
        LinearLayout drawLayout = findViewById(R.id.drawLayout);
        drawLayout.addView(drawerView);
    }
    
    private void initEmotionIndicator(EegDeviceModel deviceModel) {
        LinearLayout indicatorLayout = findViewById(R.id.indicatorLayout);
        EmotionIndicator indicatorView = new EmotionIndicator(this);
        mEmotionParamsPresenter = new EmotionParametersPresenter(deviceModel, indicatorView, this);
        indicatorLayout.addView(indicatorView);
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
}
