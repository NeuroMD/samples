package ru.neurotech.brainbitpreview.common;

/**
 * Created by movsesiv on 27/06/15.
 */
public enum ValueZoom {
    Twenty("20 μV", 20), Forty("40 μV", 40), OneHundred("100 μV", 100), TwoHundred("200 μV", 200), FourHundred("400 μV", 400), OneThousand("1000 μV", 1000), FiveThousand("5000 μV", 5000), TwentyFiveThousand("25000 μV", 25000), FiftyThousand("50000 μV", 50000);

    private final String label;
    private final int zoomValue;

    public String getLabel() {
        return label;
    }

    public int getZoomValue() {
        return zoomValue;
    }

    ValueZoom(String s, int zoomValue) {
        label = s;
        this.zoomValue = zoomValue;
    }
}
