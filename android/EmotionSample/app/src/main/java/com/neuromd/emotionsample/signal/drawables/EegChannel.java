package com.neuromd.emotionsample.signal.drawables;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.PointF;

import java.util.ArrayList;
import java.util.List;

public class EegChannel extends NamedFieldDrawable implements ISignalFieldDrawable {
    private Paint mSignalPaint;
    private Paint mGridPaint;
    private float[] mSignal;
    
    public EegChannel(String name){
        super(name);
        initSignalPaint();
        initGridPaint();
    }
    
    @Override
    public void draw(Canvas canvas) {
        drawGrid(canvas);
        drawSignal(canvas);
        drawLabel(canvas, mLeft + 30, mTop / 2);
    }
 
    public void setSignal(float[] signal){
        mSignal = signal;
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
        List<PointF> points = getPoints(mSignal, mLeft, mTop, mWidth, mHeight);
        Path path = getSimplePathFromPoints(points);
        canvas.drawPath(path, mSignalPaint);
    }
    
    private Path getSimplePathFromPoints(List<PointF> points) {
        Path path = new Path();
        path.moveTo(points.get(0).x, points.get(0).y);
        for (int i = 1; i < points.size(); ++i) {
            path.lineTo(points.get(i).x, points.get(i).y);
        }
        return path;
    }
    
    private List<PointF> getPoints(float[] data, float left, float top, float width, float height) {
        List<PointF> points = new ArrayList<>();
        float x = left;
        for (float rawY : data) {
            float y = height / 2 - rawY * height + top;
            if (y >= height) y = height - 1;
            else if (y <= 0) y = 1;
            PointF newPoint = new PointF(x, y);
            x += width / data.length;
            points.add(newPoint);
        }
        return points;
    }
    
    private void drawGrid(Canvas canvas){
        canvas.drawLine(0, mHeight / 2f, canvas.getWidth(), mHeight / 2f, mGridPaint);
    }
}
