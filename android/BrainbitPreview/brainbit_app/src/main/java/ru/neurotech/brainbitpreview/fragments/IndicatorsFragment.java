package ru.neurotech.brainbitpreview.fragments;

import android.app.Activity;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import ru.neurotech.brainbitpreview.BrainbitModel;
import ru.neurotech.brainbitpreview.R;

import ru.neurotech.common.INotificationCallback;

public class IndicatorsFragment extends Fragment implements OnClickListener {

    private Button btnStart;
    private View green;
    private View yellow;
    private View red1;
    private View red2;
    private TextView stressTextView;
    private TextView attentionTextView;
    private TextView relaxTextView;
    private TextView meditationTextView;
    //private BrainiacManager brainiacManager;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //brainiacManager = BrainiacManager.getBrainiacManager();
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_indicators, container, false);

        btnStart = (Button) view.findViewById(R.id.btnStart);
        btnStart.setOnClickListener(this);

        green = view.findViewById(R.id.green);
        yellow = view.findViewById(R.id.yellow);
        red1 = view.findViewById(R.id.red1);
        red2 = view.findViewById(R.id.red2);

        stressTextView = (TextView)view.findViewById(R.id.stressTextView);
        attentionTextView = (TextView)view.findViewById(R.id.attentionTextView);
        relaxTextView = (TextView)view.findViewById(R.id.relaxTextView);
        meditationTextView = (TextView)view.findViewById(R.id.meditationTextView);

        return view;
    }

    @Override
    public void onPause() {
        super.onPause();
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.btnStart)
        {
            if (btnStart.getText().equals("Start"))
            {
                startIndicators((Activity) v.getContext());
                BrainbitModel.getInstance().startCalculateEegStates();
                btnStart.setText("Stop");
            }
            else if (btnStart.getText().equals("Stop"))
            {
                BrainbitModel.getInstance().stopCalculateEegStates();
                stopIndicators();
                btnStart.setText("Start");
            }

        }
    }

    private void stopIndicators()
    {

    }

    private void startIndicators(final Activity host) {
        final RelativeLayout rlIndicators = (RelativeLayout) host.findViewById(R.id.rlIndicators);
        final TextView tvSleep = (TextView) host.findViewById(R.id.tvSleep);
        final TextView tvDeepRelaxation = (TextView) host.findViewById(R.id.tvDeepRelaxation);
        final TextView tvRelaxation = (TextView) host.findViewById(R.id.tvRelaxation);
        final TextView tvNormalActivation = (TextView) host.findViewById(R.id.tvNormalActivation);
        final TextView tvExcitement = (TextView) host.findViewById(R.id.tvExcitement);
        final TextView tvDeepExcitement = (TextView) host.findViewById(R.id.tvDeepExcitement);
        final int widthLeft = (tvSleep.getWidth() + tvDeepRelaxation.getWidth() + tvRelaxation.getWidth());
        final int widthRight = (tvNormalActivation.getWidth() + tvExcitement.getWidth() + tvDeepExcitement.getWidth());
        BrainbitModel.getInstance().getDevice().eegStateChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer nParam) {
                host.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        boolean direction = nParam < 0;
                        float index = Math.abs(nParam);

                        if (index > 9.5) index = 9.5f;

                        if (rlIndicators.getChildCount() > 1) {
                            rlIndicators.removeViewAt(0);
                        }

                        View view = new View(getActivity());
                        if (direction) {
                            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams((int) ((float)widthRight * index / 10), rlIndicators.getHeight());
                            view.setX(tvNormalActivation.getX());
                            view.setY(tvNormalActivation.getY());
                            view.setLayoutParams(params);
                            view.setBackgroundColor(ContextCompat.getColor(getActivity(), R.color.green));
                            rlIndicators.addView(view, 0);
                        } else {
                            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams((int) (widthLeft * index / 10), rlIndicators.getHeight());
                            view.setX(tvNormalActivation.getX() - widthLeft * index / 10);
                            view.setY(tvNormalActivation.getY());
                            view.setLayoutParams(params);
                            view.setBackgroundColor(ContextCompat.getColor(getActivity(), R.color.green));
                            rlIndicators.addView(view, 0);
                        }
                /*
                green.setBackgroundResource(R.color.grey);
                yellow.setBackgroundResource(R.color.grey);
                red1.setBackgroundResource(R.color.grey);
                red2.setBackgroundResource(R.color.grey);
                for (String color :
                        indicatorsState.getColors()) {
                    switch (color) {
                        case "green":
                            green.setBackgroundResource(R.color.green);
                            break;
                        case "yellow":
                            yellow.setBackgroundResource(R.color.yellow);
                            break;
                        case "red1":
                            red1.setBackgroundResource(R.color.red1);
                            break;
                        case "red2":
                            red2.setBackgroundResource(R.color.red2);
                            break;
                    }
                }*/
                    }
                });
            }
        });
        BrainbitModel.getInstance().getDevice().stressValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer stress_index) {
                host.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        stressTextView.setText(String.format("Stress: %d %%", stress_index));
                    }
                });
            }
        });
        BrainbitModel.getInstance().getDevice().attentionValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer attention_index) {
                host.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        attentionTextView.setText(String.format("Attention: %d %%", attention_index));
                    }
                });
            }
        });
        BrainbitModel.getInstance().getDevice().relaxValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer relax_index) {
                host.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        relaxTextView.setText(String.format("Relax: %d %%", relax_index));
                    }
                });
            }
        });
        BrainbitModel.getInstance().getDevice().meditationValueChanged.subscribe(new INotificationCallback<Integer>() {
            @Override
            public void onNotify(Object sender, final Integer meditation_index) {
                host.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        meditationTextView.setText(String.format("Meditation: %d %%", meditation_index));
                    }
                });
            }
        });
    }

}