package com.neuromd.ecgsample;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.PointF;
import android.graphics.Rect;

import java.util.ArrayList;
import java.util.List;

import com.neuromd.ecgsample.drawer.IDrawerEngine;


public class EcgDrawingEngine implements IDrawerEngine
{
    private final EcgDrawerPresenter mPresenter;
    private double mTime;
    private int mFrames = 0;
    private double mFps = 0.0;
    private final int mRulerHeight = 25;
    private Paint mSignalPaint;
    private Paint mMessagePaint;
    private Paint mGridPaint;
    private Paint mRPeakPaint;
    private Paint mArtifactPaint;
    private Paint mRulerPaint;
    private Paint mRulerBackgroundPaint;
    private Paint mBorderPaint;

    public EcgDrawingEngine(EcgDrawerPresenter presenter)
    {
        mPresenter = presenter;
        mSignalPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mSignalPaint.setStyle(Paint.Style.STROKE);
        mSignalPaint.setStrokeWidth(3);
        mSignalPaint.setColor(Color.DKGRAY);
        mSignalPaint.setTextSize(20);

        mMessagePaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mMessagePaint.setColor(Color.argb(0x30, 0x80, 0x80, 0x80));
        mMessagePaint.setTextSize(50);

        mGridPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mGridPaint.setStyle(Paint.Style.STROKE);
        mGridPaint.setStrokeWidth(2);
        mGridPaint.setColor(Color.LTGRAY);

        mRPeakPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mRPeakPaint.setStyle(Paint.Style.STROKE);
        mRPeakPaint.setStrokeWidth(4);
        mRPeakPaint.setColor(Color.argb(0xAA, 0xFF, 0x22, 0x22));

        mArtifactPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mRPeakPaint.setStyle(Paint.Style.FILL);
        mArtifactPaint.setColor(Color.argb(0x44, 0xAA, 0xAA, 0xAA));

        mRulerPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mRulerPaint.setStyle(Paint.Style.STROKE);
        mRulerPaint.setStrokeWidth(1);
        mRulerPaint.setColor(Color.DKGRAY);
        mRulerPaint.setTextSize(10);

        mRulerBackgroundPaint = new Paint();
        mRulerBackgroundPaint.setStyle(Paint.Style.FILL);
        mRulerBackgroundPaint.setColor(Color.LTGRAY);

        mBorderPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        mBorderPaint.setStyle(Paint.Style.STROKE);
        mBorderPaint.setStrokeWidth(2);
        mBorderPaint.setColor(Color.argb(0x55, 0x22, 0x22, 0xAA));
    }

    @Override
    public void Update() {
        mPresenter.updateSignalData();
    }

    @Override
    public void Draw(Canvas canvas)
    {
        double currentTime = System.currentTimeMillis();
        if (mFrames >= 10) {
            mFps = 1.0 / ((currentTime - mTime) / 1e4);
            mTime = currentTime;
            mFrames = 0;
        }

        canvas.drawColor(Color.argb(0xFF, 0xFF, 0xFF, 0xBB));

        float[] signalData = mPresenter.getSignalBuffer();
        if (signalData != null) {
            drawGrid(canvas);
            drawSignalData(canvas, signalData);
            drawRPeaks(canvas, canvas.getWidth(), canvas.getHeight()-mRulerHeight);
            //drawArtifacts(canvas, canvas.getWidth(), canvas.getHeight()-mRulerHeight);
        }
        else{
            drawNoSignalMessage(canvas);
        }

        Rect borderRect = new Rect(0,0,canvas.getWidth(),canvas.getHeight());
        canvas.drawRect(borderRect, mBorderPaint);
        canvas.drawText(String.valueOf((int) mFps) + " fps", 10, 25, mSignalPaint);
        mFrames++;
    }

    private void drawSignalData(Canvas canvas, float[] signalData){

        List<PointF> points = getPoints(
                signalData,
                canvas.getWidth(),
                canvas.getHeight() - mRulerHeight
        );
        Path path = getSimplePathFromPoints(points);
        canvas.drawPath(path, mSignalPaint);
    }

    private void drawRPeaks(Canvas canvas, float width, float height) {
        Long[] peaks = mPresenter.getRPeaks();
        if (peaks == null) return;
        for (int i = 0; i < peaks.length; ++i) {
            float peakX = (float) ((float)(peaks[i])/125.0 - mPresenter.getSignalViewTime()) / mPresenter.getHorizontalScale().getScaleValue() * width;
            canvas.drawLine(peakX, 0, peakX, height, mRPeakPaint);
        }
        
        for (int i = 1; i < peaks.length -1 ; ++i){
            float peakX = (float) ((float)(peaks[i])/125.0 - mPresenter.getSignalViewTime()) / mPresenter.getHorizontalScale().getScaleValue() * width;
            String prevTimeText = String.format("%.02f", (float)(peaks[i] - peaks[i-1])/125.0);
            Rect prevTextBounds = new Rect();
            mRulerPaint.getTextBounds(prevTimeText, 0, prevTimeText.length(), prevTextBounds);
            canvas.drawText(prevTimeText, peakX - prevTextBounds.width()-2,
                    prevTextBounds.height() + 1,
                    mRulerPaint
            );
    
    
            String nextTimeText = String.format("%.02f", (float)(peaks[i+1]-peaks[i])/125.0);
            Rect nextTextBounds = new Rect();
            mRulerPaint.getTextBounds(nextTimeText, 0, nextTimeText.length(), nextTextBounds);
            canvas.drawText(nextTimeText,
                    peakX+2,
                    nextTextBounds.height()+1,
                    mRulerPaint
            );
        }
    }

    /*private void drawArtifacts(Canvas canvas, int width, int height) {

        ArtifactZone[] artifacts = mPresenter.getArtifacts();
        if (artifacts == null) return;
        for (int i = 0; i < artifacts.length; ++i) {
            float artifactX1 = (float) (artifacts[i].time() - mPresenter.getSignalViewTime()) / mPresenter.getHorizontalScale().getScaleValue() * width;
            float artifactX2 = (float) (artifacts[i].time()+artifacts[i].duration() - mPresenter.getSignalViewTime()) / mPresenter.getHorizontalScale().getScaleValue() * width;
            canvas.drawRect(artifactX1, 0, artifactX2, height, mArtifactPaint);
        }
    }*/

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

    private void drawGrid(Canvas canvas){
        //Ruler
        Rect rulerRect = new Rect(0, canvas.getHeight() - mRulerHeight, canvas.getWidth(), canvas.getHeight());
        canvas.drawRect(rulerRect, mRulerBackgroundPaint);
        float startDashTime = (float)Math.floor(mPresenter.getSignalViewTime());
        float screenStartTime = (float)mPresenter.getSignalViewTime();
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
                    dashY + mRulerHeight/2 + timeTextBounds.height(),
                    mRulerPaint
            );
        }
        //Grid
        canvas.drawLine(0, (canvas.getHeight()-mRulerHeight)/2, canvas.getWidth(), (canvas.getHeight()-mRulerHeight)/2, mGridPaint);
    }

    private Path getSimplePathFromPoints(List<PointF> points){

        Path path = new Path();
        path.moveTo(points.get(0).x, points.get(0).y);
        for (int i = 1; i < points.size(); ++i) {
            path.lineTo(points.get(i).x, points.get(i).y);
        }
        return path;
    }

    private List<PointF> getPoints(float[] data, float width, float height){

        List<PointF> points = new ArrayList<>();
        float x = 0;
        for (float rawY : data) {
            float y = height / 2 - rawY / mPresenter.getVerticalScale().getScaleValue() * height;
            if (y >= height) y = height - 1;
            else if (y <= 0) y = 1;
            PointF newPoint = new PointF(x, y);
            x += width / data.length;
            points.add(newPoint);
        }
        return points;
    }
}
