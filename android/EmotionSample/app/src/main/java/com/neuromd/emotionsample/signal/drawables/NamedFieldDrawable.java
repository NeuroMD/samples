package com.neuromd.emotionsample.signal.drawables;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.graphics.RectF;

public abstract class NamedFieldDrawable extends DrawableWithDimensions {
    private String mName;
    private Paint mBackgroundPaint;
    private Paint mOutlinePaint;
    private Paint mTextPaint;
    
    protected NamedFieldDrawable(String name){
        mName = name;
        
        mBackgroundPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mBackgroundPaint.setColor(Color.GRAY);
        mBackgroundPaint.setStyle(Paint.Style.FILL);
        
        mOutlinePaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mOutlinePaint.setColor(Color.BLACK);
        mOutlinePaint.setStyle(Paint.Style.STROKE);
        mOutlinePaint.setStrokeWidth(2);
        
        mTextPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mTextPaint.setColor(Color.WHITE);
        mTextPaint.setTextSize(20);
        mTextPaint.setStrokeWidth(2);
    }
    
    public void setName(String name){
        mName = name;
    }
    
    protected void drawLabel(Canvas canvas, int x, int y){
        Rect bounds = new Rect();
        mTextPaint.getTextBounds(mName, 0, mName.length(), bounds);
        RectF labelRect = new RectF(x - bounds.width() / 2 - 15,
                                    y - bounds.height() / 2 - 10,
                                    x + bounds.width() / 2 + 15,
                                    y + bounds.height() / 2 + 10);
        
        canvas.drawRoundRect(labelRect, 10, 10, mBackgroundPaint);
        canvas.drawRoundRect(labelRect, 10, 10, mOutlinePaint);
        canvas.drawText(mName, x - bounds.width() / 2, y + bounds.height() / 2, mTextPaint);
    }
}
