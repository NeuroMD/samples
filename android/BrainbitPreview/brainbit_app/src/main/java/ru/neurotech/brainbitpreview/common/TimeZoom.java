package ru.neurotech.brainbitpreview.common;

public enum TimeZoom {
    One("1 sec", 1), Two("2 sec", 2), Four("4 sec", 4), Five("5 sec", 5);
    private String label;
    private int zoomValue;

    public String getLabel() {
        return label;
    }

    public int getZoomValue() {
        return zoomValue;
    }

    TimeZoom(String s, int zoomValue) {
        this.label = s;
        this.zoomValue = zoomValue;
    }
}
