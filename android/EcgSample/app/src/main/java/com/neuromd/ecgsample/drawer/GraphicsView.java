package com.neuromd.ecgsample.drawer;

import android.content.Context;
import android.view.SurfaceHolder;
import android.view.SurfaceView;


public class GraphicsView extends SurfaceView implements SurfaceHolder.Callback {

    private DisplayThread mDisplayThread;
    private IDrawerEngine mEngine;

    public GraphicsView(Context context, IDrawerEngine engine) {
        super(context);
        mEngine = engine;
        InitView();
    }


    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
       /*DO NOTHING*/
    }

    @Override
    public void surfaceCreated(SurfaceHolder arg0) {
        //Starts the display thread
        if (!mDisplayThread.IsRunning()) {
            mDisplayThread = new DisplayThread(getHolder(), mEngine);
            mDisplayThread.start();
        } else {
            mDisplayThread.start();
        }
    }

    void InitView() {
        SurfaceHolder holder = getHolder();
        holder.addCallback(this);
        mDisplayThread = new DisplayThread(holder, mEngine);
        setFocusable(true);
    }


    @Override
    public void surfaceDestroyed(SurfaceHolder arg0) {
        //Stop the display thread
        mDisplayThread.SetIsRunning(false);

    }
}
