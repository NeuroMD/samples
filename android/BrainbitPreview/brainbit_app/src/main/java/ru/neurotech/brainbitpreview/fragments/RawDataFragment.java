package ru.neurotech.brainbitpreview.fragments;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;

import ru.neurotech.brainbitpreview.R;
import ru.neurotech.brainbitpreview.common.TimeZoom;
import ru.neurotech.brainbitpreview.common.ValueZoom;
import ru.neurotech.brainbitpreview.drawer.GraphicsView;


public class RawDataFragment extends Fragment {


    GraphicsView _drawerView;
    private TextView timeZoomLabel;
    private TextView valueZoomLabel;
    private Button minusTimeZoom;
    private Button plusTimeZoom;
    private Button minusValueZoom;
    private Button plusValueZoom;

    private TimeZoom timeZoom = TimeZoom.Five;
    private ValueZoom valueZoom = ValueZoom.TwoHundred;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View view = inflater.inflate(R.layout.fragment_raw_data, container, false);

        //find layout for our drawer surface view
        LinearLayout drawLayout = (LinearLayout) view.findViewById(R.id.drawLayout);
        _drawerView = new GraphicsView(this.getActivity());
        drawLayout.addView(_drawerView);

        timeZoomLabel = (TextView) view.findViewById(R.id.timeZoomLabel);

        minusTimeZoom = (Button) view.findViewById(R.id.minusTimeZoom);
        minusTimeZoom.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                switch (timeZoom)
                {
                    case Two:
                    {
                        timeZoom = TimeZoom.One;
                        minusTimeZoom.setEnabled(false);
                        break;
                    }
                    case Four:
                    {
                        timeZoom = TimeZoom.Two;
                        break;
                    }
                    case Five:
                    {
                        timeZoom = TimeZoom.Four;
                        plusTimeZoom.setEnabled(true);
                        break;
                    }
                }
                timeZoomLabel.setText(timeZoom.getLabel());
                _drawerView.setHScale(timeZoom.getZoomValue());
            }
        });

        plusTimeZoom = (Button) view.findViewById(R.id.plusTimeZoom);
        plusTimeZoom.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                switch (timeZoom)
                {
                    case One:
                    {
                        timeZoom = TimeZoom.Two;
                        minusTimeZoom.setEnabled(true);
                        break;
                    }
                    case Two:
                    {
                        timeZoom = TimeZoom.Four;
                        break;
                    }
                    case Four:
                    {
                        timeZoom = TimeZoom.Five;
                        plusTimeZoom.setEnabled(false);
                        break;
                    }
                }
                timeZoomLabel.setText(timeZoom.getLabel());
                _drawerView.setHScale(timeZoom.getZoomValue());
            }
        });


        valueZoomLabel = (TextView) view.findViewById(R.id.valueZoomLabel);

        minusValueZoom = (Button) view.findViewById(R.id.minusValueZoom);
        minusValueZoom.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                switch (valueZoom)
                {
                    case Forty:
                    {
                        valueZoom = ValueZoom.Twenty;
                        minusValueZoom.setEnabled(false);
                        break;
                    }
                    case OneHundred:
                    {
                        valueZoom = ValueZoom.Forty;
                        break;
                    }
                    case TwoHundred:
                    {
                        valueZoom = ValueZoom.OneHundred;
                        break;
                    }
                    case FourHundred:
                    {
                        valueZoom = ValueZoom.TwoHundred;
                        break;

                    }
                    case OneThousand:
                    {
                        valueZoom = ValueZoom.FourHundred;
                        break;

                    }
                    case FiveThousand:
                    {
                        valueZoom = ValueZoom.OneThousand;
                        break;

                    }
                    case TwentyFiveThousand:
                    {
                        valueZoom = ValueZoom.FiveThousand;
                        break;

                    }
                    case FiftyThousand:
                    {
                        valueZoom = ValueZoom.TwentyFiveThousand;
                        plusValueZoom.setEnabled(true);
                        break;

                    }
                }
                valueZoomLabel.setText(valueZoom.getLabel());
                _drawerView.setVScale(valueZoom.getZoomValue());
            }
        });

        plusValueZoom = (Button) view.findViewById(R.id.plusValueZoom);
        plusValueZoom.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                switch (valueZoom)
                {
                    case Twenty:
                    {
                        valueZoom = ValueZoom.Forty;
                        minusValueZoom.setEnabled(true);
                        break;
                    }
                    case Forty:
                    {
                        valueZoom = ValueZoom.OneHundred;
                        break;
                    }
                    case OneHundred:
                    {
                        valueZoom = ValueZoom.TwoHundred;
                        break;
                    }
                    case TwoHundred:
                    {
                        valueZoom = ValueZoom.FourHundred;
                        break;
                    }
                    case FourHundred:
                    {
                        valueZoom = ValueZoom.OneThousand;
                        break;
                    }
                    case OneThousand:
                    {
                        valueZoom = ValueZoom.FiveThousand;
                        break;

                    }
                    case FiveThousand:
                    {
                        valueZoom = ValueZoom.TwentyFiveThousand;
                        break;

                    }
                    case TwentyFiveThousand:
                    {
                        valueZoom = ValueZoom.FiftyThousand;
                        plusValueZoom.setEnabled(false);
                        break;

                    }
                }
                valueZoomLabel.setText(valueZoom.getLabel());
                _drawerView.setVScale(valueZoom.getZoomValue());
            }
        });


        return view;
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
    }

    @Override
    public void onDestroy() {

        super.onDestroy();
    }
}
