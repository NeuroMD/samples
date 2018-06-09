package com.neuromd.stateandbattery;

import android.content.Context;
import android.graphics.Color;
import android.support.annotation.LayoutRes;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.neuromd.neurosdk.channels.ChannelInfo;
import com.neuromd.neurosdk.channels.ChannelType;
import com.neuromd.neurosdk.parameters.ParameterName;
import com.neuromd.neurosdk.parameters.types.DeviceState;

import java.util.List;

public class DeviceItemAdapter extends ArrayAdapter<DeviceItem> {


        public DeviceItemAdapter(@NonNull Context context, @LayoutRes int resource, @NonNull List<DeviceItem> objects) {
            super(context, resource, objects);
        }

        @NonNull
        @Override
        public View getView(int position, View convertView, @NonNull ViewGroup parent) {
            DeviceItem deviceItem = getItem(position);
            if (convertView == null) {
                convertView = LayoutInflater.from(getContext()).inflate(R.layout.device_item_layout, parent, false);
            }            
            TextView deviceNameTextView = convertView.findViewById(R.id.deviceNameTextView);            
            deviceNameTextView.setText(deviceItem.name());
            deviceNameTextView.setTextColor(deviceItem.color());


            TextView addressTextView = convertView.findViewById(R.id.addressTextView);
            addressTextView.setText(String.format("[%s]",deviceItem.address()));

            TextView deviceStateTextView = convertView.findViewById(R.id.deviceStateTextView);
            deviceStateTextView.setText(deviceItem.state());

            TextView batteryTextView = convertView.findViewById(R.id.batteryTextView);
            batteryTextView.setText(String.format("Battery: %d%%", deviceItem.charge()));

            return convertView;
        }
    }