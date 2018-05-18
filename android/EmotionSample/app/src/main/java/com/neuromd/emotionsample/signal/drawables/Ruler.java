package com.neuromd.emotionsample.signal.drawables;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;

public class Ruler implements ISignalFieldDrawable {
    private Paint mRulerPaint;
    private Paint mRulerBackgroundPaint;
    
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
    
    }
    
    private void drawRuler(Canvas canvas) {
        Rect rulerRect = new Rect(0, canvas.getHeight() - mRulerHeight, canvas.getWidth(), canvas.getHeight());
        canvas.drawRect(rulerRect, mRulerBackgroundPaint);
        float startDashTime = (float) Math.floor(mPresenter.getSignalViewTime());
        float screenStartTime = (float) mPresenter.getSignalViewTime();
        int secondsMaxCount = mPresenter.getHorizontalScale().getScaleValue() + 1;
        float dashY = canvas.getHeight() - mRulerHeight;
        for (int i = 0; i < secondsMaxCount; ++i) {
            float dashTime = startDashTime + i;
            float dashX = (dashTime - screenStartTime) / mPresenter.getHorizontalScale().getScaleValue() * canvas.getWidth();
            canvas.drawLine(dashX, dashY, dashX, dashY + mRulerHeight / 2, mRulerPaint);
            int minute = (int) dashTime / 60;
            int seconds = Math.abs((int) dashTime - minute * 60);
            String dashTimeText = String.format("%s%d:%02d", dashTime >= 0 ? "" : "-", minute, seconds);
            Rect timeTextBounds = new Rect();
            mRulerPaint.getTextBounds(dashTimeText, 0, dashTimeText.length(), timeTextBounds);
            canvas.drawText(dashTimeText,
                    dashX - timeTextBounds.width() / 2,
                    dashY + mRulerHeight / 2 + timeTextBounds.height(),
                    mRulerPaint
            );
        }
    }
}
