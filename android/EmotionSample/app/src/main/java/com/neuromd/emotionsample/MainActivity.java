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
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.crashlytics.android.Crashlytics;
import io.fabric.sdk.android.Fabric;
import java.util.List;

import com.neuromd.common.INotificationCallback;
import com.neuromd.emotionsample.drawer.GraphicsView;
import com.neuromd.emotionsample.drawer.IDrawerEngine;
import com.neuromd.emotionsample.signal.EegDrawerPresenter;
import com.neuromd.emotionsample.signal.EegDrawingEngine;
import com.neuromd.neurosdk.Device;

public class MainActivity extends AppCompatActivity {

    private MainActivityPresenter mMainPresenter;
    private EegParamsPresenter mEcgPresenter;
    private EegDrawerPresenter mEegDrawerPresenter;
    private VelocityTracker mVelocityTracker;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Fabric.with(this, new Crashlytics());
        setContentView(R.layout.activity_main);

        requestPermissions();
        enableBtAndGeolocation();

        EegDeviceModel model = createAndInitModel();
        mMainPresenter = new MainActivityPresenter(this, model);
        mEcgPresenter = new EegParamsPresenter(this, model);
        mEegDrawerPresenter = new EegDrawerPresenter(model);

        initDrawer();
        initScanButtons();
        initSignalButtons();
        initParamsLabels();
        initDeviceList();
        initScaleControls();
        initEcgLabels();
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

    private void initDrawer(){
        IDrawerEngine drawerEngine = new EegDrawingEngine(mEegDrawerPresenter);
        GraphicsView drawerView = new GraphicsView(this, drawerEngine);
        LinearLayout drawLayout = (LinearLayout) findViewById(R.id.drawLayout);
        drawLayout.addView(drawerView);
    }

    private void initParamsLabels(){
        final TextView batteryTextView = (TextView)findViewById(R.id.batteryTextView);
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
        });

        final TextView hpfTextView = (TextView)findViewById(R.id.hardwareHpfStateTextView);
        mMainPresenter.hpfStateTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String hpfText) {
                hpfTextView.setText(hpfText);
            }
        });

        final TextView samplingFrequencyTextView = (TextView)findViewById(R.id.samplingFrequencyTextView);
        mMainPresenter.samplingFrequencyTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String samplingFrequencyText) {
                samplingFrequencyTextView.setText(samplingFrequencyText);
            }
        });

        final TextView gainTextView = (TextView)findViewById(R.id.gainTextView);
        mMainPresenter.gainTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String gainText) {
                gainTextView.setText(gainText);
            }
        });

        final TextView offsetTextView = (TextView)findViewById(R.id.offsetTextView);
        mMainPresenter.offsetTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String offsetText) {
                offsetTextView.setText(offsetText);
            }
        });

    }

    private void initSignalButtons(){
        final Button startSignalButton = (Button)findViewById(R.id.startSignalButton);
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
        removeDeviceButton.setEnabled(mMainPresenter.isRemoveDeviceButtonEnabled());
    }

    private void initScanButtons(){
        final Button startScanButton = (Button)findViewById(R.id.startScanButton);
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
        stopScanButton.setEnabled(mMainPresenter.isStopScanButtonEnabled());
    }

    private void initDeviceList(){

        final VisualDeviceAdapter deviceListAdapter = new VisualDeviceAdapter(getApplicationContext(), R.layout.device_layout, mMainPresenter.getDeviceList());
        final ListView deviceListView = (ListView)findViewById(R.id.deviceListView);
        mMainPresenter.deviceListChanged.subscribe(new INotificationCallback<List<Device>>() {
            @Override
            public void onNotify(Object o, List<Device> devices) {
                deviceListAdapter.notifyDataSetChanged();
                deviceListView.clearChoices();
                deviceListView.requestLayout();
            }
        });
        deviceListView.setAdapter(deviceListAdapter);
        deviceListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Device device = (Device)parent.getItemAtPosition(position);
                mMainPresenter.onDeviceListItemSelected(device);
            }
        });
    }

    private void initScaleControls(){
        //Vertical scale
        final Button verticalScaleIncrementButton = (Button) findViewById(R.id.vScalePlusButton);
        verticalScaleIncrementButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mEegDrawerPresenter.incrementVerticalScale();
            }
        });
        mEegDrawerPresenter.verticalScaleIncrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                verticalScaleIncrementButton.setEnabled(isEnabled);
            }
        });
        final Button verticalScaleDecrementButton = (Button) findViewById(R.id.vScaleMinusButton);
        verticalScaleDecrementButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mEegDrawerPresenter.decrementVerticalScale();
            }
        });
        mEegDrawerPresenter.verticalScaleDecrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                verticalScaleDecrementButton.setEnabled(isEnabled);
            }
        });
        final TextView verticalTextView = (TextView)findViewById(R.id.vScaleTextView);
        verticalTextView.setText(mEegDrawerPresenter.getVerticalScale().getLabel());
        mEegDrawerPresenter.verticalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object sender, ScaleValue scaleValue) {
                verticalTextView.setText(scaleValue.getLabel());
            }
        });

        //Horizontal scale
        final Button horizontalScaleIncrementButton = (Button) findViewById(R.id.hScalePlusButton);
        horizontalScaleIncrementButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mEegDrawerPresenter.incrementHorizontalScale();
            }
        });
        mEegDrawerPresenter.horizontalScaleIncrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                horizontalScaleIncrementButton.setEnabled(isEnabled);
            }
        });
        final Button horizontalScaleDecrementButton = (Button) findViewById(R.id.hScaleMinusButton);
        horizontalScaleDecrementButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mEegDrawerPresenter.decrementHorizontalScale();
            }
        });
        mEegDrawerPresenter.horizontalScaleDecrementButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isEnabled) {
                horizontalScaleDecrementButton.setEnabled(isEnabled);
            }
        });
        final TextView horizontalTextView = (TextView)findViewById(R.id.hScaleTextView);
        horizontalTextView.setText(mEegDrawerPresenter.getHorizontalScale().getLabel());
        mEegDrawerPresenter.horizontalScaleChanged.subscribe(new INotificationCallback<ScaleValue>() {
            @Override
            public void onNotify(Object sender, ScaleValue scaleValue) {
                horizontalTextView.setText(scaleValue.getLabel());
            }
        });
    }

    private void initEcgLabels(){
        final TextView signalDurationTextView = (TextView)findViewById(R.id.surveyDurationTextView);
        signalDurationTextView.setText(mEcgPresenter.getSignalDurationText());
        mEcgPresenter.signalDurationTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String durationText) {
                signalDurationTextView.setText(durationText);
            }
        });

        final TextView electrodesStateTextView = (TextView)findViewById(R.id.electrodesStateTextView);
        electrodesStateTextView.setText(mEcgPresenter.getElectrodesStateText());
        mEcgPresenter.electrodesStateTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String electrodesStateText) {
                electrodesStateTextView.setText(electrodesStateText);
            }
        });

        final TextView heartRateTextView = (TextView)findViewById(R.id.heartRateTextView);
        heartRateTextView.setText(mEcgPresenter.getHeartRateText());
        mEcgPresenter.heartRateTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String heartRateText) {
                heartRateTextView.setText(heartRateText);
            }
        });

        final TextView stressIndexTextView = (TextView)findViewById(R.id.stressIndexTextView);
        stressIndexTextView.setText(mEcgPresenter.getStressIndexText());
        mEcgPresenter.stressIndexTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String stressIndexText) {
                stressIndexTextView.setText(stressIndexText);
            }
        });
    }
}
