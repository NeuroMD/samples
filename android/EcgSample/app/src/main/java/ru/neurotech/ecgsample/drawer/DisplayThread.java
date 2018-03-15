package ru.neurotech.ecgsample.drawer;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.view.SurfaceHolder;


public class DisplayThread extends Thread
{
    private SurfaceHolder mSurfaceHolder;
    private Paint mBackgroundPaint;
    private IDrawerEngine mEngine;
    private boolean mIsOnRun;

    DisplayThread(SurfaceHolder surfaceHolder, IDrawerEngine engine)
    {
        mSurfaceHolder = surfaceHolder;
        mEngine = engine;

        //black painter below to clear the screen before the game is rendered
        mBackgroundPaint = new Paint();
        mBackgroundPaint.setARGB(255, 0, 0, 0);
        mIsOnRun = true;
    }

    /**
     * This is the main nucleus of our program.
     * From here will be called all the method that are associated with the display in GameEngine object
     * */
    @Override
    public void run()
    {
        //Looping until the boolean is false
        while (mIsOnRun)
        {
            //Updates the game objects business logic
            mEngine.Update();

            //locking the canvas
            Canvas canvas = mSurfaceHolder.lockCanvas(null);

            if (canvas != null)
            {
                //Clears the screen with black paint and draws object on the canvas
                synchronized (mSurfaceHolder)
                {
                    canvas.drawRect(0, 0, canvas.getWidth(), canvas.getHeight(), mBackgroundPaint);
                    mEngine.Draw(canvas);
                }

                //unlocking the Canvas
                mSurfaceHolder.unlockCanvasAndPost(canvas);
            }

            Thread.yield();
        }
    }

    /**
     * @return whether the thread is running
     * */
    public boolean IsRunning()
    {
        return mIsOnRun;
    }
    /**
     * Sets the thread state, false = stoped, true = running
     **/
    public void SetIsRunning(boolean state)
    {
        mIsOnRun = state;
    }
}
