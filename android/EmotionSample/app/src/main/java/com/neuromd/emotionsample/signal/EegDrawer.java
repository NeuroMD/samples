package com.neuromd.emotionsample.signal;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;

import java.util.List;

public abstract class EegDrawer {
    protected List<ISignalFieldDrawable> mDrawables;
    public void setDrawables(List<ISignalFieldDrawable> drawables){
        mDrawables = drawables;
    }
    public SubscribersNotifier<UpdatePendingArgs> updatePending = new SubscribersNotifier<>();
}
