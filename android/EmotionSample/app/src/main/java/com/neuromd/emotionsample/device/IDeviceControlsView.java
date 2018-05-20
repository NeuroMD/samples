package com.neuromd.emotionsample.device;

public interface IDeviceControlsView {
    void setDeviceText(String text);
    void setProgressVisible(boolean isVisible);
    void setReconnectButtonEnabled(boolean isEnabled);
    void enableBluetooth();
    void requestPermissions();
}
