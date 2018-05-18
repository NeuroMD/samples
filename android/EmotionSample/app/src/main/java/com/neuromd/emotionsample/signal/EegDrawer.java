package com.neuromd.emotionsample.signal;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;

public abstract class EegDrawer {
    protected ISignalFieldDrawable[] mDrawables;
    public void setDrawables(ISignalFieldDrawable[] drawables){
        mDrawables = drawables;
    }
    public SubscribersNotifier<UpdatePendingArgs> updatePending = new SubscribersNotifier<>();
}
