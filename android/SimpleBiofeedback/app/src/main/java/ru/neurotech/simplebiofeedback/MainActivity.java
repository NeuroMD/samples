package ru.neurotech.simplebiofeedback;

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
import android.view.ScaleGestureDetector;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;

import java.util.List;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.neurodevices.bfb.BfbDevice;

public class MainActivity extends AppCompatActivity {

    private MainActivityPresenter mMainPresenter;
    private BfbBarPresenter mBarPresenter;
    private ScaleGestureDetector mScaleDetector;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        requestPermissions();
        enableBtAndGeolocation();

        BfbDeviceModel model = createAndInitModel();
        mMainPresenter = new MainActivityPresenter(this, model);
        mBarPresenter = new BfbBarPresenter(this, model);
        mScaleDetector = new ScaleGestureDetector(getApplicationContext(), new ScaleListener());

        initScanButtons();
        initSignalButtons();
        initIndexControls();
        initParamsLabels();
        initDeviceList();
        initFrameButton();
        initBarButton();
        initCalibrationBar();
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
            requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION}, 1);
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

    private BfbDeviceModel createAndInitModel(){
        BfbDeviceModel model = new BfbDeviceModel(getApplicationContext());
        model.bluetoothAdapterEnableNeeded.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object sender, Object param/*null*/) {
                enableBtAndGeolocation();
            }
        });

        return model;
    }

    private void initIndexControls() {

        //Index params EditText
        final EditText frequencyBeginEditText = (EditText) findViewById(R.id.frequencyBeginEditText);
        mMainPresenter.frequencyBeginEditTextEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                frequencyBeginEditText.setEnabled(isEnabled);
            }
        });
        frequencyBeginEditText.setEnabled(mMainPresenter.isFrequencyBeginEditTextEnabled());

        final EditText frequencyEndEditText = (EditText) findViewById(R.id.frequencyEndEditText);
        mMainPresenter.frequencyEndEditTextEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                frequencyEndEditText.setEnabled(isEnabled);
            }
        });
        frequencyEndEditText.setEnabled(mMainPresenter.isFrequencyEndEditTextEnabled());

        final EditText windowEditText = (EditText) findViewById(R.id.windowEditText);
        mMainPresenter.windowEditTextEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                windowEditText.setEnabled(isEnabled);
            }
        });
        windowEditText.setEnabled(mMainPresenter.isWindowEditTextEnabled());

        final EditText overlappingEditText = (EditText) findViewById(R.id.overlappingEditText);
        mMainPresenter.overlappingEditTextEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                overlappingEditText.setEnabled(isEnabled);
            }
        });
        overlappingEditText.setEnabled(mMainPresenter.isOverlappingEditTextEnabled());


        //Create index button
        final Button createIndexButton = (Button) findViewById(R.id.createIndexButton);
        createIndexButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onCreateIndexButtonClick(
                        frequencyBeginEditText.getText().toString(),
                        frequencyEndEditText.getText().toString(),
                        windowEditText.getText().toString(),
                        overlappingEditText.getText().toString()
                );
            }
        });
        mMainPresenter.createIndexButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                createIndexButton.setEnabled(isEnabled);
            }
        });
        createIndexButton.setEnabled(mMainPresenter.isCreateIndexButtonEnabled());

        //Remove index button
        final Button removeIndexButton = (Button) findViewById(R.id.removeIndexButton);
        removeIndexButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onRemoveIndexButtonClick();
            }
        });
        mMainPresenter.removeIndexButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                removeIndexButton.setEnabled(isEnabled);
            }
        });
        removeIndexButton.setEnabled(mMainPresenter.isRemoveIndexButtonEnabled());

        //Calibrate button
        final Button calibrateButton = (Button) findViewById(R.id.calibrateButton);
        calibrateButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mMainPresenter.onCalibrateButtonClick();
            }
        });
        mMainPresenter.calibrateButtonEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                calibrateButton.setEnabled(isEnabled);
            }
        });
        calibrateButton.setEnabled(mMainPresenter.isCalibrateButtonEnabled());

        final Button shadeButton = (Button) findViewById(R.id.shadeButton);
        mBarPresenter.barEnabledChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object o, Boolean isEnabled) {
                shadeButton.setVisibility(isEnabled ? View.INVISIBLE : View.VISIBLE);
            }
        });
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

        final TextView deviceErrorTextView = (TextView)findViewById(R.id.errorTextView);
        mMainPresenter.deviceErrorTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String errorText) {
                deviceErrorTextView.setText(errorText);
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

        final TextView channelsTextView = (TextView)findViewById(R.id.channelsTextView);
        mMainPresenter.channelsTextChanged.subscribe(new INotificationCallback<String>() {
            @Override
            public void onNotify(Object o, String channelsText) {
                channelsTextView.setText(channelsText);
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
        mMainPresenter.deviceListChanged.subscribe(new INotificationCallback<List<BfbDevice>>() {
            @Override
            public void onNotify(Object o, List<BfbDevice> devices) {
                deviceListAdapter.notifyDataSetChanged();
                deviceListView.clearChoices();
                deviceListView.requestLayout();
            }
        });
        deviceListView.setAdapter(deviceListAdapter);
        deviceListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                BfbDevice device = (BfbDevice)parent.getItemAtPosition(position);
                mMainPresenter.onDeviceListItemSelected(device);
            }
        });
    }

    private void initFrameButton(){

        final Button frameButton = (Button) findViewById(R.id.frameButton);
        mBarPresenter.frameParametersChanged.subscribe(new INotificationCallback<BfbBarPresenter.FrameParams>() {
            @Override
            public void onNotify(Object sender, BfbBarPresenter.FrameParams frameParams) {

                ViewGroup.MarginLayoutParams layoutParams = (ViewGroup.MarginLayoutParams) frameButton.getLayoutParams();
                layoutParams.bottomMargin = frameParams.bottomMargin();
                layoutParams.height = frameParams.height();
                frameButton.setLayoutParams(layoutParams);
            }
        });
        frameButton.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent ev) {

                // Let the ScaleGestureDetector inspect all events.
                mScaleDetector.onTouchEvent(ev);
                final int action = ev.getAction();

                switch (action) {
                    case MotionEvent.ACTION_DOWN: {
                        // Remember where we started (for dragging)
                        mBarPresenter.onActionDown(ev.getRawY());
                        break;
                    }

                    case MotionEvent.ACTION_MOVE: {
                        mBarPresenter.onActionMove(ev.getRawY());
                        break;
                    }
                }
                return true;
            }
        });
    }

    private void initBarButton(){

        final Button barButton = (Button)findViewById(R.id.barButton);
        mBarPresenter.barHeightChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, Integer barHeight) {
                RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams) barButton.getLayoutParams();
                params.height = barHeight;
                barButton.setLayoutParams(params);
            }
        });
        mBarPresenter.barColorChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, Integer barColor) {
                barButton.setBackgroundColor(barColor);
            }
        });
    }

    private void initCalibrationBar(){
        final ProgressBar calibrationBar = (ProgressBar)findViewById(R.id.calibrationBar);
        mBarPresenter.calibrationBarVisibilityChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, Integer visibility) {
                calibrationBar.setVisibility(visibility);
            }
        });
    }

    private class ScaleListener
            extends ScaleGestureDetector.SimpleOnScaleGestureListener {
        @Override
        public boolean onScale(ScaleGestureDetector detector) {

            mBarPresenter.onScaleFactorChanged(detector.getScaleFactor());
            return true;
        }
    }
}
