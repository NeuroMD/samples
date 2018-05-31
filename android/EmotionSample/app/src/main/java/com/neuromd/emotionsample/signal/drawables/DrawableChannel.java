package com.neuromd.emotionsample.signal.drawables;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.PointF;

import java.util.ArrayList;
import java.util.List;

public class DrawableChannel extends NamedFieldDrawable implements ISignalFieldDrawable {
    private Paint mSignalPaint;
    private Paint mGridPaint;
    private Path mSignalPath;
    
    public DrawableChannel(String name){
        super(name);
        initSignalPaint();
        initGridPaint();
    }
    
    @Override
    public void draw(Canvas canvas) {
        drawGrid(canvas);
        drawSignal(canvas);
        drawLabel(canvas, mLeft + 30, mTop + mHeight / 2);
    }
 
    public void setSignal(float[] signal){
        List<PointF> points = getPoints(signal, mLeft, mTop, mWidth, mHeight);
       mSignalPath = getSimplePathFromPoints(points);
    }
    
    private void initSignalPaint(){
        mSignalPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mSignalPaint.setStyle(Paint.Style.STROKE);
        mSignalPaint.setStrokeWidth(3);
        mSignalPaint.setColor(Color.DKGRAY);
        mSignalPaint.setTextSize(20);
    }
    
    private void initGridPaint(){
        mGridPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mGridPaint.setStyle(Paint.Style.STROKE);
        mGridPaint.setStrokeWidth(2);
        mGridPaint.setColor(Color.LTGRAY);
    }
    
    private void drawSignal(Canvas canvas){
        if (mSignalPath!=null) {
            canvas.drawPath(mSignalPath, mSignalPaint);
        }
    }
    
    private Path getSimplePathFromPoints(List<PointF> points) {
        Path path = new Path();
        if (points.size() > 0) {
            path.moveTo(points.get(0).x, points.get(0).y);
            for (int i = 1; i < points.size(); ++i) {
                path.lineTo(points.get(i).x, points.get(i).y);
            }
        }
        return path;
    }
    
    private List<PointF> getPoints(float[] data, float left, float top, float width, float height) {
        List<PointF> points = new ArrayList<>();
        if (data == null){
            return points;
        }
        
        float x = left;
        for (float rawY : data) {
            float y = top + height / 2 - rawY;
            if (y >= top + height) y = top + height - 1;
            else if (y <= top) y = top;
            PointF newPoint = new PointF(x, y);
            x += width / data.length;
            points.add(newPoint);
        }
        return points;
    }
    
    private void drawGrid(Canvas canvas){
        canvas.drawLine(0, mTop + mHeight / 2f, canvas.getWidth(), mTop + mHeight / 2f, mGridPaint);
    }
}
