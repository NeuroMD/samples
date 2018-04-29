package com.neuromd.device_info;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.content.Intent;
import android.location.LocationManager;
import android.os.Build;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.neuromd.common.INotificationCallback;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.Parameter;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

public class DeviceInfoActivity extends AppCompatActivity
{
    private DeviceScanner mScanner;
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_device_info);
    
        requestPermissions();
        enableBtAndGeolocation();
        initScanner();
        initButtons();
    }
    
    private void initScanner()
    {
        mScanner = new DeviceScanner(getApplicationContext());
        mScanner.scanStateChanged.subscribe(new INotificationCallback<Boolean>()
        {
            @Override
            public void onNotify(Object o, final Boolean isScanning)
            {
                runOnUiThread(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        Button startButton = findViewById(R.id.startScanButton);
                        Button stopButton = findViewById(R.id.stopScanButton);
                        startButton.setEnabled(!isScanning);
                        stopButton.setEnabled(isScanning);
                    }
                });
            }
        });
        mScanner.deviceFound.subscribe(new INotificationCallback<Device>()
        {
            @Override
            public void onNotify(Object o, Device device)
            {
                onDeviceFound(device);
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
    
    private void initButtons()
    {
        Button startButton = findViewById(R.id.startScanButton);
        startButton.setEnabled(true);
        startButton.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mScanner.startScan(0); //zero is for infinity
            }
        });
    
        Button stopButton = findViewById(R.id.stopScanButton);
        stopButton.setEnabled(false);
        stopButton.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mScanner.stopScan();
            }
        });
    }
    
    private void onDeviceFound(final Device device)
    {
        device.parameterChanged.subscribe(new INotificationCallback<ParameterName>()
        {
            @Override
            public void onNotify(Object o, ParameterName parameterName)
            {
                if (parameterName == ParameterName.State){
                    DeviceState state = device.readParam(ParameterName.State);
                    if (state == DeviceState.Connected){
                        onDeviceConnected(device);
                    }
                }
            }
        });
        device.connect();
    }
    
    private void onDeviceConnected(Device device)
    {
        EditText deviceInfoText = findViewById(R.id.infoText);
        
        String deviceName = device.readParam(ParameterName.Name);
        String deviceAddress = device.readParam(ParameterName.Address);
        deviceInfoText.append(String.format("Found device: %s [%s]\n", deviceName, deviceAddress));
    
        deviceInfoText.append("Supported params:\n");
        Parameter[] deviceParams = device.parameters();
        for (Parameter param : deviceParams)
        {
            String paramName = param.getName().toString();
            String accessMode = param.getAccess().toString();
            deviceInfoText.append(String.format("-%s {%s}\n", paramName, accessMode));
        }
    
        deviceInfoText.append("\nSupported commands:\n");
        Command[] deviceCommands = device.commands();
        for (Command cmd : deviceCommands)
        {
            deviceInfoText.append(String.format("-%s \n", cmd.toString()));
        }
    
        deviceInfoText.append("\nSupported channels:\n");
        ChannelInfo[] deviceChannels = device.channels();
        for (ChannelInfo channel : deviceChannels)
        {
            String channelName = channel.getName();
            deviceInfoText.append(String.format("-%s \n", channelName));
        }
    }
}
