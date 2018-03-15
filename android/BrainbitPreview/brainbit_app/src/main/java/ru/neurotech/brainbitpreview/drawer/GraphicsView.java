package ru.neurotech.brainbitpreview.drawer;

import android.content.Context;
import android.os.Handler;
import android.view.SurfaceHolder;
import android.view.SurfaceView;


public class GraphicsView extends SurfaceView implements SurfaceHolder.Callback
{
    Context _context;

    private DisplayThread _displayThread;
    //private ScaleGestureDetector mScaleDetector;
    private Handler handler;

    public GraphicsView(Context context)
    {
        super(context);

        _context = context;

        InitView();
    }


    public void setHScale(int seconds)
    {
        DrawingEngine.getEngine().updateHScale(seconds);
    }

    public void setVScale(int uVolts)
    {
        DrawingEngine.getEngine().updateVScale(uVolts);
    }

    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height)
    {
       /*DO NOTHING*/
    }

    @Override
    public void surfaceCreated(SurfaceHolder arg0)
    {
        //Starts the display thread
        if (!_displayThread.IsRunning())
        {
            _displayThread = new DisplayThread(getHolder(), _context);
            _displayThread.start();
        } else
        {
            _displayThread.start();
        }
    }

    void InitView()
    {
        SurfaceHolder holder = getHolder();
        holder.addCallback(this);

        _displayThread = new DisplayThread(holder, _context);
        setFocusable(true);
    }


    @Override
    public void surfaceDestroyed(SurfaceHolder arg0)
    {
        //Stop the display thread
        _displayThread.SetIsRunning(false);

    }
}
