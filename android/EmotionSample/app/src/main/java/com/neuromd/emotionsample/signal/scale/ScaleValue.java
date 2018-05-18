package com.neuromd.emotionsample.signal.scale;

public class ScaleValue {
    private final String mLabel;
    private final int mScaleValue;

    public ScaleValue(String label, int scaleValue) {
        mLabel = label;
        mScaleValue = scaleValue;
    }

    public String getLabel() {
        return mLabel;
    }
    public int getScaleValue() {
        return mScaleValue;
    }
}
