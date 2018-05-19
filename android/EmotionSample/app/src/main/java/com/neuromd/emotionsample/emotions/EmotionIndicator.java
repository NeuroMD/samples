package com.neuromd.emotionsample.emotions;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.view.View;

public class EmotionIndicator extends View implements IEmotionIndicatorView {
    private int mValue;
    private String mName;
    private Paint mNamePaint;
    private Paint mBlockPaint;
    
    public EmotionIndicator(Context context) {
        super(context);
    
        initNamePaint();
        initBlockPaint();
    }
    
    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        drawIndicator(canvas);
    }
    
    @Override
    public void setValue(int value) {
        mValue = value;
        mName = valueToName(value);
        this.post(new Runnable() {
            @Override
            public void run() {
                invalidate();
            }
        });
    }
    
    private void initNamePaint(){
        mNamePaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mNamePaint.setColor(Color.DKGRAY);
        mNamePaint.setTextSize(18);
    }
    
    private void initBlockPaint(){
        mBlockPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mBlockPaint.setColor(Color.argb(255, 0, 180, 0));
        mBlockPaint.setStyle(Paint.Style.FILL);
    }
    
    private void drawIndicator(Canvas canvas){
        drawBar(canvas);
        drawName(canvas);
    }
    
    private void drawBar(Canvas canvas){
        canvas.drawColor(Color.argb(0xFF, 0xFF, 0xFF, 0xBB));
        int middleX = canvas.getWidth() / 2;
        int blockSpanWidth = 3;
        int totalBlockCount = 6;
        int centralBlockWidth = 6;
        int fullBlockWidth = (int)(((float)canvas.getWidth() -
                centralBlockWidth -
                totalBlockCount * blockSpanWidth) / totalBlockCount);
        Rect centralRect = new Rect(middleX - centralBlockWidth / 2,
                                    0,
                                    middleX + centralBlockWidth / 2,
                                    canvas.getHeight());
        canvas.drawRect(centralRect, mBlockPaint);
    }
    
    private void drawName(Canvas canvas){
        if (mName.isEmpty())
            return;
        
        Rect nameTextBounds = new Rect();
        mNamePaint.getTextBounds(mName, 0, mName.length(), nameTextBounds);
        canvas.drawText(
                mName,
                canvas.getWidth() / 2 - nameTextBounds.width() / 2,
                canvas.getHeight() / 2 + nameTextBounds.height() / 2,
                mNamePaint
        );
    }
    
    private String valueToName(int value){
        if (value == 0){
            return "NEUTRAL";
        }
        else if (value > 0 && value <= 4){
            return "RELAX";
        }
        else if (value > 4 && value <= 8){
            return "DEEP RELAX";
        }
        else if (value > 8){
            return "SLEEP";
        }
        else if (value < 0 && value >=-4){
            return "NORMAL ACTIVATION";
        }
        else if (value < -4 && value >= -8){
            return "EXCITEMENT";
        }
        else if (value < -8){
            return "DEEP EXCITEMENT";
        }
        else{
            return "UNKNOWN";
        }
    }
}
