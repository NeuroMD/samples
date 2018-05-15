package ru.neurotech.brainbitpreview.activities;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.neuromd.bleconnection.exceptions.BluetoothAdapterException;
import com.neuromd.bleconnection.exceptions.BluetoothPermissionException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import ru.neurotech.brainbitpreview.BrainbitModel;
import ru.neurotech.brainbitpreview.R;

public class StartUpActivity extends FragmentActivity implements View.OnClickListener {
    
    public static final String TAG = StartUpActivity.class.getSimpleName();
    
    private TextView statusLabel;
    private TextView frequencyLabel;
    private TextView battery;
    private Button connectBtn;
    private Button stopScanBtn;
    private Button disconnectBtn;
    private Button plusFrequency;
    private Button minusFrequency;
    private Button startTest;

    private int frequency = 3;

    public static final int REQUEST_ENABLE_BT = 1;
    public static final String ACTION_NEW_VALUE = "ACTION_NEW_VALUE";
    public static final String ACTION_FFT_VALUE = "ACTION_FFT_VALUES";


    private DeviceScanner mEegDeviceScanner;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_startup);

        mEegDeviceScanner = new DeviceScanner(getApplicationContext());

        statusLabel = (TextView) findViewById(R.id.statusLabel);
        frequencyLabel = (TextView) findViewById(R.id.frequency);
        battery = (TextView) findViewById(R.id.battery);
        connectBtn = (Button) findViewById(R.id.connectBtn);
        stopScanBtn = (Button) findViewById(R.id.stopScanBtn);
        disconnectBtn = (Button) findViewById(R.id.disconnectBtn);
        plusFrequency = (Button) findViewById(R.id.plusFrequency);
        minusFrequency = (Button) findViewById(R.id.minusFrequency);
        startTest = (Button) findViewById(R.id.startTest);

        Button showPlotBtn = (Button) findViewById(R.id.showPlotBtn);
        showPlotBtn.setOnClickListener(this);

        disconnectBtn.setVisibility(View.GONE);
        stopScanBtn.setVisibility(View.GONE);

        connectBtn.setOnClickListener(this);
        disconnectBtn.setOnClickListener(this);
        stopScanBtn.setOnClickListener(this);
        minusFrequency.setOnClickListener(this);
        plusFrequency.setOnClickListener(this);
        startTest.setOnClickListener(this);

        mEegDeviceScanner.scanStateChanged.subscribe(new INotificationCallback<Boolean>() {
            @Override
            public void onNotify(Object sender, Boolean isScanning) {

                if (isScanning){
                    Log.d(TAG, "onScanStart()");
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            showStopScanView();
                        }
                    });
                }
                else{
                    Log.d(TAG, "onScanStop()");
                }
            }
        });

        mEegDeviceScanner.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, final Device eegDevice) {
                Log.d(TAG, "onDeviceFound()");
                String deviceName = eegDevice.readParam(ParameterName.Name);
                if (!deviceName.equals("BrainBit")) return;
                mEegDeviceScanner.stopScan();
                eegDevice.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                    @Override
                    public void onNotify(Object sender,ParameterName paramName) {
                        if (paramName == ParameterName.State) {
                            DeviceState state = eegDevice.readParam(ParameterName.State);
                            if (state == DeviceState.Disconnected) {
                                runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        Log.d(TAG, "onDeviceDisconnected()");
                                        showConnectView();
                                    }
                                });
                            }
                            else{
                                BrainbitModel.getInstance().setDevice(eegDevice);
                                runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        statusLabel.setText(R.string.device_found);
                                        Log.d(TAG, "onDeviceConnected()");
                                        showDisconnectView();
                                        BrainbitModel.getInstance().setDevice(eegDevice);
                                        battery.setText(String.format("Battery: %d %%", BrainbitModel.getInstance().getBatteryLevel()));
                                    }
                                });
                            }
                        }
                    }
                });
                eegDevice.connect();
            }
        });


        showConnectView();
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_ENABLE_BT) {
            if (resultCode == RESULT_OK) {
                connect();
            } else {
                Toast.makeText(this, getString(R.string.please_enable_bluetooth), Toast.LENGTH_LONG).show();
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
                return;
            }
            connect();
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.showPlotBtn: {
                startActivity(MainActivity.createIntent(this));
                break;
            }
            case R.id.connectBtn: {
                startTest.setText(R.string.start_test);
                //brainiacManager.stopTest();
                connect();
                break;
            }
            case R.id.disconnectBtn: {
                disconnect();
                break;
            }
            case R.id.stopScanBtn: {
                stopScan();
                break;
            }
            case R.id.minusFrequency: {
                if (frequency == 4) {
                    minusFrequency.setEnabled(false);
                } else if (frequency == 24) {
                    plusFrequency.setEnabled(true);
                }
                frequencyLabel.setText(String.valueOf(--frequency));
                break;
            }
            case R.id.plusFrequency: {
                if (frequency == 23) {
                    plusFrequency.setEnabled(false);
                } else if (frequency == 3) {
                    minusFrequency.setEnabled(true);
                }
                frequencyLabel.setText(String.valueOf(++frequency));
                break;
            }
            case R.id.startTest: {
                /*if (brainiacManager.isInTestMode()) {
                    startTest.setText(R.string.start_test);
                    brainiacManager.stopTest();
                } else {
                    startTest.setText(R.string.stop);
                    brainiacManager.startTest(frequency);
                }*/
                break;
            }
        }
    }

    private void connect() {
        try
        {
            mEegDeviceScanner.startScan(0);
            showStopScanView();
        }
        catch (BluetoothAdapterException e)
        {
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
        }
        catch (BluetoothPermissionException e)
        {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
            {
                requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION}, 1);
            }
            else
            {
                Toast.makeText(this, "Unable to connect device due to security permissions", Toast.LENGTH_LONG).show();
            }
        }
    }

    private void disconnect() {
        mEegDeviceScanner.stopScan();
        BrainbitModel.getInstance().setDevice(null);
        showConnectView();
    }

    private void stopScan() {
        mEegDeviceScanner.stopScan();
        showConnectView();
    }

    private void showStopScanView() {
        connectBtn.setVisibility(View.GONE);
        disconnectBtn.setVisibility(View.GONE);
        stopScanBtn.setVisibility(View.VISIBLE);
        statusLabel.setText(R.string.scanning_started);
    }

    private void showConnectView() {
        connectBtn.setVisibility(View.VISIBLE);
        disconnectBtn.setVisibility(View.GONE);
        stopScanBtn.setVisibility(View.GONE);
        statusLabel.setText(R.string.ready_to_scan);
    }

    private void showDisconnectView() {
        connectBtn.setVisibility(View.GONE);
        disconnectBtn.setVisibility(View.VISIBLE);
        stopScanBtn.setVisibility(View.GONE);
        statusLabel.setText(R.string.connected);
    }
}
