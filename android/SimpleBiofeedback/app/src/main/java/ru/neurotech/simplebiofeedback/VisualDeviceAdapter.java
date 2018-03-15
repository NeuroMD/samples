package ru.neurotech.simplebiofeedback;

import android.content.Context;
import android.graphics.Color;
import android.support.annotation.LayoutRes;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.List;

import ru.neurotech.common.CallibriVisualDevice;
import ru.neurotech.neurodevices.bfb.BfbDevice;


public class VisualDeviceAdapter extends ArrayAdapter<BfbDevice> {


    public VisualDeviceAdapter(@NonNull Context context, @LayoutRes int resource, @NonNull List<BfbDevice> objects) {
        super(context, resource, objects);
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        BfbDevice device = getItem(position);

        if (convertView == null) {
            convertView = LayoutInflater.from(getContext()).inflate(R.layout.device_layout, parent, false);
        }

        TextView deviceNameTextView = (TextView) convertView.findViewById(R.id.deviceNameTextView);
        String rawDeviceName = device != null ? device.getName() : null;

        String deviceName = rawDeviceName;
        int deviceColor = Color.BLACK;
        if (rawDeviceName!=null) {
            switch (rawDeviceName) {
                case "Neurotech_Colibri_R":
                case "Neurotech_Callibri_R": {
                    deviceName = "Callibri Red";
                    deviceColor = Color.RED;
                    break;
                }
                case "Neurotech_Colibri_B":
                case "Neurotech_Callibri_B": {
                    deviceName = "Callibri Blue";
                    deviceColor = Color.BLUE;
                    break;
                }
                case "Neurotech_Colibri_Y":
                case "Neurotech_Callibri_Y": {
                    deviceName = "Callibri Yellow";
                    deviceColor = Color.YELLOW;
                    break;
                }
                case "Neurotech_Colibri_W":
                case "Neurotech_Callibri_W": {
                    deviceName = "Callibri White";
                    deviceColor = Color.WHITE;
                    break;
                }
            }
        }

        if (deviceName == null || deviceName.isEmpty()) {
            deviceName = "null";
        }
        String deviceAddress = device != null ? device.getAddress() : null;
        if (deviceAddress == null || deviceAddress.isEmpty()) {
            deviceAddress = "null";
        }
        String deviceInfo = String.format("%s [%s]", deviceName, deviceAddress);

        deviceNameTextView.setText(deviceInfo);
        if (device!=null) {
            deviceNameTextView.setTextColor(deviceColor);
        }

        return convertView;
    }
}
