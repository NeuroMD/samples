package ru.neurotech.brainbitpreview.activities;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.FragmentActivity;
import android.support.v4.view.ViewPager;
import android.widget.TextView;

import com.astuetz.PagerSlidingTabStrip;

import ru.neurotech.brainbitpreview.BrainbitModel;
import ru.neurotech.brainbitpreview.R;
import ru.neurotech.brainbitpreview.adapters.MainActivityPagerAdapter;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.neurodevices.eeg.EegDevice;


public class MainActivity extends FragmentActivity {


    public static final String EXTRA_VALUES = "EXTRA_VALUES";

    public static Intent createIntent(Context context) {
        return new Intent(context, MainActivity.class);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        PagerSlidingTabStrip tabs = (PagerSlidingTabStrip) findViewById(R.id.tabs);
        ViewPager viewPager = (ViewPager) findViewById(R.id.viewPager);

        MainActivityPagerAdapter adapter = new MainActivityPagerAdapter(getSupportFragmentManager());

        viewPager.setOffscreenPageLimit(2);
        viewPager.setAdapter(adapter);
        tabs.setViewPager(viewPager);

        final TextView percentTextView = (TextView)findViewById(R.id.percentsTextView);

        BrainbitModel.getInstance().selectedDeviceChanged.subscribe(new INotificationCallback<EegDevice>() {
            @Override
            public void onNotify(Object sender, final EegDevice device) {

                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        if (device != null) {
                            percentTextView.setText(String.format("%d %%", device.getBatteryLevel()));
                        }
                        else{
                            percentTextView.setText("0 %");
                        }
                    }
                });
            }
        });

        BrainbitModel.getInstance().batteryStateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer value) {
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        percentTextView.setText(String.format("%d %%", value));
                    }
                });
            }
        });
        percentTextView.setText(String.format("%d %%", BrainbitModel.getInstance().getBatteryLevel()));
    }


}
