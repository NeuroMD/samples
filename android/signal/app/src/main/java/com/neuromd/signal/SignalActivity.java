package com.neuromd.signal;

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
import android.widget.ProgressBar;
import android.widget.TextView;

import com.neuromd.bleconnection.exceptions.BluetoothAdapterException;
import com.neuromd.common.INotificationCallback;
import com.neuromd.neurosdk.Device;
import com.neuromd.neurosdk.DeviceScanner;
import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.channels.Filter;
import com.neuromd.neurosdk.channels.SignalChannel;
import com.neuromd.neurosdk.parameters.Command;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;
import com.neuromd.neurosdk.parameters.types.SamplingFrequency;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import static android.view.View.GONE;

public class SignalActivity extends AppCompatActivity {

    private final HashMap<String, Double> mSignalValues = new HashMap<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_signal);

        requestPermissions();
        enableBtAndGeolocation();

        final Button startButton = findViewById(R.id.startButton);
        startButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    findDevice();
                    startButton.setVisibility(GONE);
                    ProgressBar searchProgress = findViewById(R.id.deviceSearchProgressBar);
                    searchProgress.setVisibility(View.VISIBLE);
                }
                catch (BluetoothAdapterException e){
                    Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
                    startActivityForResult(enableBtIntent, 1);
                }
            }
        });
    }

    private void requestPermissions() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION}, 1);
        }
    }

    private void enableBtAndGeolocation() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            final LocationManager manager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
            if (!manager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
                Intent enableGeoIntent = new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
                startActivityForResult(enableGeoIntent, 1);
            }
        }
    }

    private void findDevice() {
        final DeviceScanner scanner = new DeviceScanner(getApplicationContext());
        scanner.deviceFound.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, Device device) {
                scanner.stopScan();
                onDeviceFound(device);
            }
        });
        scanner.startScan(0);
    }

    private void onDeviceFound(final Device device) {
        DeviceState state = device.readParam(ParameterName.State);
        if (state != DeviceState.Connected) {
            device.parameterChanged.subscribe(new INotificationCallback<ParameterName>() {
                @Override
                public void onNotify(Object o, ParameterName parameterName) {
                    if (parameterName == ParameterName.State) {
                        DeviceState state = device.readParam(ParameterName.State);
                        if (state == DeviceState.Connected) {
                            runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    onDeviceConnected(device);
                                }
                            });
                        }
                    }
                }
            });
            device.connect();
        } else {
            onDeviceConnected(device);
        }
    }

    private void onDeviceConnected(Device device) {
        ProgressBar searchProgress = findViewById(R.id.deviceSearchProgressBar);
        searchProgress.setVisibility(GONE);
        showDeviceInfo(device);
        startReceiveSignal(device);
    }

    private void showDeviceInfo(Device device) {
        String deviceName = device.readParam(ParameterName.Name);
        String deviceAddress = device.readParam(ParameterName.Address);
        TextView deviceInfoText = findViewById(R.id.deviceTextView);
        deviceInfoText.setText(String.format("Device: %s [%s]", deviceName, deviceAddress));
    }

    private void startReceiveSignal(Device device) {
        List<SignalChannel> signalChannelsCollection = createChannels(device);
        if (signalChannelsCollection.size() <= 0) {
            TextView signalValueTextView = findViewById(R.id.signalValueTextView);
            signalValueTextView.setText("Device does not have signal channels.");
            return;
        }

        for (final SignalChannel signalChannel : signalChannelsCollection) {
            signalChannel.dataLengthChanged.subscribe(new INotificationCallback<Long>() {
                @Override
                public void onNotify(Object sender, Long signalLength) {
                    //average signal amplitude in microvolts
                    final double averageValueUv = calculateAverageValueUv(signalChannel);
                    setChannelValue(signalChannel.info().getName(), averageValueUv);
                }
            });
        }

        //start receive signal
        device.execute(Command.StartSignal);
    }

    private List<SignalChannel> createChannels(Device device) {
        ChannelInfo[] deviceChannels = device.channels();
        List<SignalChannel> signalChannelsCollection = new ArrayList<>();
        for (ChannelInfo channel : deviceChannels) {
            if (channel.getType() == ChannelType.Signal) {
                signalChannelsCollection.add(new SignalChannel(device, channel));
            }
        }

        return signalChannelsCollection;
    }

    private double calculateAverageValueUv(SignalChannel signalChannel) {
        //calculate average signal amplitude with 1 second window
        float actualFrequency = signalChannel.samplingFrequency();
        long dataLengthToRead = (long) actualFrequency; //one second of data
        long signalLength = signalChannel.totalLength();

        //start calculate only if there is enough data
        if (signalLength >= dataLengthToRead) {
            double[] signalData = signalChannel.readFast(signalLength - dataLengthToRead, dataLengthToRead);
            return averageAmplitude(signalData) * 1000000; //returning value in microvolts
        }

        return 0.0;
    }

    private double averageAmplitude(double[] signal) {
        double average = 0.0;
        for (double sample : signal) {
            average += Math.abs(sample);
        }
        return average / signal.length;
    }

    private void setChannelValue(final String channelName, final double valueMv) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mSignalValues.put(channelName, valueMv);
                TextView signalValueTextView = findViewById(R.id.signalValueTextView);
                signalValueTextView.setText("");
                for (String channel : mSignalValues.keySet()) {
                    if (mSignalValues.size() > 1) {
                        signalValueTextView.append(String.format("%s: ", channel));
                    }
                    signalValueTextView.append(String.format("%.02f uV ", mSignalValues.get(channel)));
                }

            }
        });
    }
}
