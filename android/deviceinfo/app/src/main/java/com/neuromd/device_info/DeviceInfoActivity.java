package com.neuromd.device_info;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.content.Intent;
import android.location.LocationManager;
import android.os.Build;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.EditText;

import com.neuromd.neurosdk.DeviceInfo;
import com.neuromd.neurosdk.DeviceType;
import com.neuromd.neurosdk.INotificationCallback;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceEnumerator;
import com.neuromd.neurosdk.ChannelInfo;
import com.neuromd.neurosdk.Command;
import com.neuromd.neurosdk.Parameter;
import com.neuromd.neurosdk.ParameterName;
import com.neuromd.neurosdk.DeviceState;

public class DeviceInfoActivity extends AppCompatActivity
{
    private DeviceEnumerator mScanner;
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_device_info);
    
        requestPermissions();
        enableBtAndGeolocation();
    }

    @Override
    protected void onStart() {
        super.onStart();
        initScanner();
    }

    private void initScanner()
    {
        mScanner = new DeviceEnumerator(getApplicationContext(), DeviceType.Any);
        mScanner.deviceListChanged.subscribe(new INotificationCallback()
        {
            @Override
            public void onNotify(Object o, Object a)
            {
                for (DeviceInfo deviceInfo : mScanner.devices())
                {
                    onDeviceFound(mScanner.createDevice(deviceInfo));
                }
            }
        });
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

    private void onDeviceFound(final Device device)
    {
        runOnUiThread(new Runnable()
        {
            @Override
            public void run()
            {
                EditText deviceInfoText = findViewById(R.id.infoText);

                String deviceName = device.readParam(ParameterName.Name);
                String deviceAddress = device.readParam(ParameterName.Address);
                deviceInfoText.append(String.format("Found device: %s [%s]\n", deviceName, deviceAddress));
            }
        });
    }
}
