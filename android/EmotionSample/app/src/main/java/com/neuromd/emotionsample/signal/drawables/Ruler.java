package com.neuromd.emotionsample.signal.drawables;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;

public class Ruler extends DrawableWithDimensions implements ISignalFieldDrawable {
    private Paint mRulerPaint;
    private Paint mRulerBackgroundPaint;
    private float mViewTime;
    private int mSecondsCount;
    
    public Ruler(){
        mRulerPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mRulerPaint.setStyle(Paint.Style.STROKE);
        mRulerPaint.setStrokeWidth(1);
        mRulerPaint.setColor(Color.DKGRAY);
        mRulerPaint.setTextSize(10);
    
        mRulerBackgroundPaint = new Paint();
        mRulerBackgroundPaint.setStyle(Paint.Style.FILL);
        mRulerBackgroundPaint.setColor(Color.LTGRAY);
    }
    
    @Override
    public void draw(Canvas canvas) {
        drawRuler(canvas);
    }
    
    public void setViewTime(float time){
        mViewTime = time;
    }
    
    public void setSecondCount(int seconds){
        mSecondsCount = seconds;
    }
    
    private void drawRuler(Canvas canvas) {
        Rect rulerRect = new Rect(mLeft, mTop, mLeft + mWidth, mTop + mHeight);
        canvas.drawRect(rulerRect, mRulerBackgroundPaint);
        float startDashTime = (float) Math.floor(mViewTime);
        float screenStartTime = mViewTime;
        int secondsMaxCount = mSecondsCount + 1;
        float dashY = mTop;
        for (int i = 0; i < secondsMaxCount; ++i) {
            float dashTime = startDashTime + i;
            float dashX = (dashTime - screenStartTime) / mSecondsCount * mWidth;
            canvas.drawLine(dashX, dashY, dashX, dashY + mHeight / 2, mRulerPaint);
            int minute = (int) dashTime / 60;
            int seconds = Math.abs((int) dashTime - minute * 60);
            String dashTimeText = String.format("%s%d:%02d", dashTime >= 0 ? "" : "-", minute, seconds);
            Rect timeTextBounds = new Rect();
            mRulerPaint.getTextBounds(dashTimeText, 0, dashTimeText.length(), timeTextBounds);
            canvas.drawText(dashTimeText,
                    dashX - timeTextBounds.width() / 2,
                    dashY + mHeight / 2 + timeTextBounds.height(),
                    mRulerPaint
            );
        }
    }
}
