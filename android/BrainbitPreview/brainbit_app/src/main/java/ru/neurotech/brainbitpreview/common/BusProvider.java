package ru.neurotech.brainbitpreview.common;

import com.squareup.otto.Bus;

/**
 * Created by movsesiv on 22/12/15.
 */
public class BusProvider {
    private static Bus bus;

    public static Bus getBus() {
        if (bus == null) {
            bus = new Bus();
        }
        return bus;
    }
}
