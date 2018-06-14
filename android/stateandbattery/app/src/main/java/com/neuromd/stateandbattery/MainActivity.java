package com.neuromd.stateandbattery;

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
import android.view.View;
import android.widget.Button;
import android.widget.ListView;

import com.neuromd.bleconnection.exceptions.BluetoothPermissionException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private DeviceScanner mScanner;
    private DeviceItemAdapter mItemAdapter;
    private List<DeviceItem> mItemList = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        requestPermissions();
        enableBtAndGeolocation();

        initDeviceList();
        initScanner();
        initResetButton();
        try {
            mScanner.startScan(0);
        }
        catch (BluetoothPermissionException permExc){
            requestPermissions();
        }
    }

    private void requestPermissions() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
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

    private void initDeviceList(){
        mItemAdapter = new DeviceItemAdapter(getApplicationContext(), R.layout.device_item_layout, mItemList);
        ListView deviceListView = findViewById(R.id.deviceList);
        deviceListView.setAdapter(mItemAdapter);
    }

    private void initScanner() {
        mScanner = new DeviceScanner(getApplicationContext());
        mScanner.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, Device device) {
                onDeviceFound(device);
            }
        });
    }

    private void initResetButton(){
        Button resetButton = findViewById(R.id.resetButton);
        resetButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mScanner.stopScan();
                for (DeviceItem item : mItemList){
                    item.close();
                }
                mItemList.clear();
                refreshDeviceListView();
                mScanner.startScan(0);
            }
        });
    }

    private void onDeviceFound(Device device) {
        String deviceAddress = device.readParam(ParameterName.Address);
        for (DeviceItem item : mItemList){
            if (deviceAddress.equals(item.address())){
                return;
            }
        }
        DeviceItem deviceItem = new DeviceItem(device);
        deviceItem.deviceParamsChanged.subscribe(new INotificationCallback() {
            @Override
            public void onNotify(Object sender, Object nParam) {
                refreshDeviceListView();
            }
        });
        mItemList.add(deviceItem);
        refreshDeviceListView();
    }

    private void refreshDeviceListView() {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mItemAdapter.notifyDataSetChanged();
                ListView deviceListView = findViewById(R.id.deviceList);
                deviceListView.clearChoices();
                deviceListView.requestLayout();
            }
        });
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
}
