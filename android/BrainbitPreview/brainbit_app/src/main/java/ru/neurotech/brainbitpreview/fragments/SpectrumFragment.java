package ru.neurotech.brainbitpreview.fragments;

import android.app.Activity;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.formatter.ValueFormatter;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.github.mikephil.charting.utils.ViewPortHandler;


import ru.neurotech.brainbitpreview.BrainbitModel;
import ru.neurotech.brainbitpreview.R;

import java.util.ArrayList;
import java.util.List;

import ru.neurotech.neurodevices.eeg.SpectrumData;
import ru.neurotech.neurodevices.features.Channel;

public class SpectrumFragment extends Fragment {

    private List<LineChart> charts = new ArrayList<>();
    private List<String> xValues = new ArrayList<>();
    private List<List<Entry>> blueEntries = new ArrayList<>();
    private List<List<Entry>> redEntries = new ArrayList<>();
    private List<List<Entry>> greenEntries = new ArrayList<>();


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_spectrum, container, false);

        charts.add((LineChart) view.findViewById(R.id.chart0));
        charts.add((LineChart) view.findViewById(R.id.chart1));
        charts.add((LineChart) view.findViewById(R.id.chart2));
        charts.add((LineChart) view.findViewById(R.id.chart3));

        return view;
    }

    @Override
    public void onViewCreated(final View view, @Nullable Bundle savedInstanceState)
    {
        super.onViewCreated(view, savedInstanceState);


        final Activity host = (Activity) view.getContext();

        for (int i = 0; i < charts.size(); i++)
        {
            LineChart chart = charts.get(i);
            chart.setPinchZoom(false);
            chart.setTouchEnabled(false);
            chart.setDoubleTapToZoomEnabled(false);

            List<ILineDataSet> lineDataSets = new ArrayList<>();

            blueEntries.add(new ArrayList<Entry>());
            LineDataSet blue = new LineDataSet(blueEntries.get(i), "");
            blue.setDrawCircles(false);
            blue.setValueFormatter(new ValueFormatter() {
                @Override
                public String getFormattedValue(float value, Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler) {
                    return "";
                }
            });
            blue.setColor(Color.BLUE);
            blue.setDrawFilled(true);
            blue.setFillColor(Color.BLUE);
            lineDataSets.add(blue);

            greenEntries.add(new ArrayList<Entry>());
            LineDataSet green = new LineDataSet(greenEntries.get(i), "");
            green.setDrawCircles(false);
            green.setValueFormatter(new ValueFormatter() {
                @Override
                public String getFormattedValue(float value, Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler) {
                    return "";
                }
            });
            green.setColor(Color.GREEN);
            green.setDrawFilled(true);
            green.setFillColor(Color.GREEN);
            lineDataSets.add(green);


            redEntries.add(new ArrayList<Entry>());
            LineDataSet red = new LineDataSet(redEntries.get(i), "");
            red.setDrawCircles(false);
            red.setValueFormatter(new ValueFormatter() {
                @Override
                public String getFormattedValue(float value, Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler) {
                    return "";
                }
            });
            red.setColor(Color.RED);
            red.setDrawFilled(true);
            red.setFillColor(Color.RED);
            lineDataSets.add(red);

            LineData lineData = new LineData(xValues, lineDataSets);


            chart.setDescription("");
            chart.setData(lineData);
            chart.setGridBackgroundColor(ContextCompat.getColor(getActivity(), R.color.white));

            YAxis leftAxis = chart.getAxisLeft();
            leftAxis.setLabelCount(5, true);
            leftAxis.setAxisMinValue(0);
            leftAxis.setAxisMaxValue(100);

            YAxis rightAxis = chart.getAxisRight();
            rightAxis.setEnabled(false);

            XAxis xAxis = chart.getXAxis();
            xAxis.setEnabled(true);
            xAxis.setDrawLabels(true);
            xAxis.resetLabelsToSkip();
            List<String> lb = new ArrayList<>();
            lb.add("2.5");lb.add("5");lb.add("7.5");lb.add("10");lb.add("12.5");lb.add("15");
            lb.add("17.5");lb.add("20");lb.add("22.5");lb.add("25");lb.add("27.5");lb.add("30");
            xAxis.setValues(lb);

            Legend legend = chart.getLegend();
            legend.setEnabled(false);
        }

        requestSpectres();
    }

    private void requestSpectres()
    {
        int surveyLength = (int)BrainbitModel.getInstance().getTotalDataLength();

        int length = 3*BrainbitModel.getInstance().getSamplingFrequency();
        int offset = surveyLength - length;

        final SpectrumData spectrumDataT3 = BrainbitModel.getInstance().calculateSpectrumForChannel("T3", offset, length);
        final SpectrumData spectrumDataT4 = BrainbitModel.getInstance().calculateSpectrumForChannel("T4", offset, length);
        final SpectrumData spectrumDataO1 = BrainbitModel.getInstance().calculateSpectrumForChannel("O1", offset, length);
        final SpectrumData spectrumDataO2 = BrainbitModel.getInstance().calculateSpectrumForChannel("O2", offset, length);

        getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                setChannelGraphics(0, spectrumDataT3);
                setChannelGraphics(1, spectrumDataT4);
                setChannelGraphics(2, spectrumDataO1);
                setChannelGraphics(3, spectrumDataO2);
            }
        });

        final Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                requestSpectres();
            }
        }, 50);
    }

    private void setChannelGraphics(int channel_number, SpectrumData spectrum)
    {
        if (spectrum == null)
            return;

        xValues.clear();

        int samplesToDraw = (int)(60.0 / spectrum.getFrequencyStep()); //30 Hz band to draw
        int alphaStop = (int)(14.0 / spectrum.getFrequencyStep());
        int alphaStart = (int)(8.0 / spectrum.getFrequencyStep());


        List<Entry> blues = blueEntries.get(channel_number);
        blues.clear();

        List<Entry> greens = greenEntries.get(channel_number);
        greens.clear();

        List<Entry> reds = redEntries.get(channel_number);
        reds.clear();

        for (int i = 0; i < samplesToDraw; ++i)
        {
            xValues.add("");
            if (i <= alphaStart)
            {
                greens.add(new Entry((float) spectrum.getSpectrumData()[i]*1e6f, i));
            }
            else if(i >= alphaStop)
            {
                blues.add(new Entry((float) spectrum.getSpectrumData()[i]*1e6f, i));
            }
            if (i>=alphaStart && i <=alphaStop)
            {
                reds.add(new Entry((float) spectrum.getSpectrumData()[i]*1e6f, i));
            }
        }
        LineChart chart = charts.get(channel_number);
        chart.setVisibleXRange(samplesToDraw, samplesToDraw);
        chart.moveViewToX(samplesToDraw - 1);
        chart.notifyDataSetChanged();
        chart.invalidate();
    }

    @Override
    public void onDestroy()
    {
        super.onDestroy();
    }
}