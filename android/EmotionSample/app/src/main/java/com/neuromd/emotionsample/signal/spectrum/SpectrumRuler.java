package com.neuromd.emotionsample.signal.spectrum;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;

import com.neuromd.emotionsample.signal.drawables.DrawableWithDimensions;
import com.neuromd.emotionsample.signal.drawables.ISignalFieldDrawable;

public class SpectrumRuler extends DrawableWithDimensions implements ISignalFieldDrawable {
    private Paint mRulerPaint;
    private Paint mRulerBackgroundPaint;
    private int mMaxFrequency;
    
    public SpectrumRuler(){
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
   
    public void setMaxFrequency(int freq){
        mMaxFrequency = freq;
    }
    
    private void drawRuler(Canvas canvas) {
        Rect rulerRect = new Rect(mLeft, mTop, mLeft + mWidth, mTop + mHeight);
        canvas.drawRect(rulerRect, mRulerBackgroundPaint);
        int dashesCount = mMaxFrequency * 2;
        float dashY = mTop;
        for (int i = 0; i <= dashesCount; i+=10) {
            float dashX = ((float)i/2) / mMaxFrequency * mWidth;
            canvas.drawLine(dashX, dashY, dashX, dashY + mHeight / 2, mRulerPaint);
            String dashText = String.valueOf(i%mMaxFrequency);
            Rect timeTextBounds = new Rect();
            mRulerPaint.getTextBounds(dashText, 0, dashText.length(), timeTextBounds);
            canvas.drawText(dashText,
                    dashX - timeTextBounds.width() / 2,
                    dashY + mHeight / 2 + timeTextBounds.height(),
                    mRulerPaint
            );
        }
    }
}
