package com.neuromd.emotionsample.signal;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;

import com.neuromd.emotionsample.drawer.IDrawerEngine;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;


public class EegDrawingEngine extends EegDrawer implements IDrawerEngine {
    private double mTime;
    private int mFrames = 0;
    private double mFps = 0.0;
    private Paint mMessagePaint;
    private Paint mBorderPaint;
    private Paint mFpsPaint;
    
    public EegDrawingEngine() {
        mFpsPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mFpsPaint.setStyle(Paint.Style.STROKE);
        mFpsPaint.setStrokeWidth(3);
        mFpsPaint.setColor(Color.DKGRAY);
        mFpsPaint.setTextSize(20);
        
        mMessagePaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mMessagePaint.setColor(Color.argb(0x30, 0x80, 0x80, 0x80));
        mMessagePaint.setTextSize(50);
        
        mBorderPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mBorderPaint.setStyle(Paint.Style.STROKE);
        mBorderPaint.setStrokeWidth(2);
        mBorderPaint.setColor(Color.argb(0x55, 0x22, 0x22, 0xAA));
    }
    
    @Override
    public void Update(int width, int height) {
        updatePending.sendNotification(this, new UpdatePendingArgs(width, height));
    }
    
    @Override
    public void Draw(Canvas canvas) {
        canvas.drawColor(Color.argb(0xFF, 0xFF, 0xFF, 0xBB));
        if (mDrawables != null && mDrawables.size() > 0) {
            for (ISignalFieldDrawable drawable : mDrawables) {
                drawable.draw(canvas);
            }
        }
        else {
            drawNoSignalMessage(canvas);
        }
        
        showFps(canvas);
        drawBorder(canvas);
    }
    
    private void showFps(Canvas canvas) {
        double currentTime = System.currentTimeMillis();
        if (mFrames >= 10) {
            mFps = 1.0 / ((currentTime - mTime) / 1e4);
            mTime = currentTime;
            mFrames = 0;
        }
        canvas.drawText(String.valueOf((int) mFps) + " fps", 10, 25, mFpsPaint);
        mFrames++;
    }
    
    private void drawBorder(Canvas canvas) {
        Rect borderRect = new Rect(0, 0, canvas.getWidth(), canvas.getHeight());
        canvas.drawRect(borderRect, mBorderPaint);
    }
    
    private void drawNoSignalMessage(Canvas canvas) {
        int canvasWidth = canvas.getWidth();
        int canvasHeight = canvas.getHeight();
        String noSignalMsg = "NO SIGNAL";
        Rect msgTextBounds = new Rect();
        mMessagePaint.getTextBounds(noSignalMsg, 0, noSignalMsg.length(), msgTextBounds);
        canvas.drawText(
                noSignalMsg,
                canvasWidth / 2 - msgTextBounds.width() / 2,
                canvasHeight / 2 + msgTextBounds.height() / 2,
                mMessagePaint
        );
    }
}
