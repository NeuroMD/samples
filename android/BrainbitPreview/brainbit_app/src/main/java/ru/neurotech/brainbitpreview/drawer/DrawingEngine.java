package ru.neurotech.brainbitpreview.drawer;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.PointF;
import android.graphics.Rect;
import android.graphics.RectF;

import com.neuromd.neurosdk.channels.eeg.EegChannel;

import ru.neurotech.brainbitpreview.BrainbitModel;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;


public class DrawingEngine
{
    /*MEMBERS*/
    static final Object _sync = new Object();
    static float _lastTouchedX, _lastTouchedY;

    //EegChannel[] _channels = {EegChannel.T3, EegChannel.T4, EegChannel.O1, EegChannel.O2};

    //private NeuroSdkModel _model = NeuroSdkModel.GetSingleInstance();

    private double time;
    private int frames=0;
    double fps=0.0;

    public int vScale;

    public int hScale;

    private static DrawingEngine engine;
    public static DrawingEngine getEngine()
    {
        if (engine==null) engine = new DrawingEngine();
        return engine;
    }


    private DrawingEngine()
    {
        paint = new Paint(Paint.ANTI_ALIAS_FLAG);
        paint.setStyle(Paint.Style.STROKE);
        paint.setStrokeWidth(2);
        paint.setColor(Color.DKGRAY);
        paint.setTextSize(20);

        gridPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        gridPaint.setStyle(Paint.Style.STROKE);
        gridPaint.setStrokeWidth(2);
        gridPaint.setColor(Color.LTGRAY);
        gridPaint.setTextSize(15);

        artiPaint = new Paint();
        artiPaint.setStyle(Paint.Style.FILL);
        artiPaint.setColor(Color.LTGRAY);

        vScale = 200;
        hScale = 5;
    }

    private Paint paint;
    private Paint gridPaint;
    private Paint artiPaint;

    /**
     * Updates all relevant objects business logic
     * */
    public void Update()
    {
        //Здесь обновляется масштаб и прочее...
    }


    public void updateVScale(int uVolts)
    {
        vScale=uVolts;
    }

    public void updateHScale(int seconds)
    {
        hScale=seconds;
        //IEegFacade system = BrainiacManager.getBrainiacManager().eegSystem();
        //system.setScreenDuration(seconds);
    }

    /**
     * Draws all objects according to their parameters
     * @param canvas
     * 			canvas on which will be drawn the objects
     * */
    public void Draw(Canvas canvas)
    {
        double currentTime = System.currentTimeMillis();
        if (frames>=10) {
            fps = 1.0 / ((currentTime - time) / 1e4);
            time = currentTime;
            frames=0;
        }
        canvas.drawColor(Color.WHITE);

        /*//List<Long> artifacts = currentProcessor.getArtiTime();
        long screenEnd = BrainiacManager.getBrainiacManager().eegSystem().getSurveyDuration();
        long screenStart =  screenEnd - hScale/250*1000;

        for (int i=0; i<artifacts.size(); i++)
        {
            long arti = artifacts.get(i);
            if (arti>=screenStart-1000 && arti<screenEnd)
            {
                int x = ((int)(arti-screenStart))*canvasWidth/((int)(screenEnd-screenStart));
                int right = x+(canvasWidth/(hScale/250));
                if (x<0) x=0;
                if (right>canvasWidth) right = canvasWidth;
               // Log.i("Drawer", "arti height: " + String.valueOf(artiRect.height())+", arti width: " + String.valueOf(artiRect.width()));
                canvas.drawRect(x,0,right,canvas.getHeight(), artiPaint);
            }
        }*/
        int canvasWidth = canvas.getWidth();
        canvas.drawText(String.valueOf((int) fps) + " fps", 10, 25, paint);


        //IEegFacade system = BrainiacManager.getBrainiacManager().eegSystem();
        //float surveyDuration = system.getSurveyDuration();
        //if (surveyDuration==0) return;
        //float time = surveyDuration - hScale;

        Map<String, EegChannel> channels = BrainbitModel.getInstance().getChannels();

        if (channels!=null) {
            float y = 0;
            float frameHeight = (float) canvas.getHeight() / 4f;
            for (EegChannel channel :channels.values()) {

                long dataLength = channel.totalLength();
                int length = hScale*BrainbitModel.getInstance().getSamplingFrequency();
                int offset = (int)(dataLength - length);

                Double[] signal = channel.readData(offset, length);

                drawGrid(canvas, y, frameHeight);
                if (signal != null) {
                    Path path = getSimplePath(signal, y, frameHeight, canvasWidth);
                    canvas.drawPath(path, paint);
                    drawLabel(canvas, y + frameHeight / 2, channel.info().getName());
                }
                y += frameHeight;
            }
        }
        frames++;
    }

    private void drawGrid(Canvas canvas, float start_y, float height)
    {
        float y = 0;
        float yStep = height/4.0f;
        drawHorizontalLine(canvas, start_y + height / 2);
        for (int i=1; i<=30; i++)
        {
            y+=yStep*vScale*2.5f;
            if (y>height/2) break;
            drawHorizontalLine(canvas, start_y + height / 2 + y);
            drawHorizontalLine(canvas, start_y + height / 2 - y);
        }
    }

    private void drawLabel(Canvas canvas, float y, String label)
    {
        Rect bounds = new Rect();
        gridPaint.getTextBounds(label, 0, label.length(), bounds);
        RectF labelRect = new RectF(canvas.getWidth() - bounds.width() - 40, y - bounds.height() / 2 - 10, canvas.getWidth() - 15, y + bounds.height() / 2 + 15);
        Paint labelPaint = new Paint(Paint.ANTI_ALIAS_FLAG);

        labelPaint.setColor(Color.GRAY);
        labelPaint.setStyle(Paint.Style.FILL);
        canvas.drawRoundRect(labelRect, 10, 10, labelPaint);

        labelPaint.setColor(Color.BLACK);
        labelPaint.setStyle(Paint.Style.STROKE);
        labelPaint.setStrokeWidth(2);
        canvas.drawRoundRect(labelRect, 10, 10, labelPaint);

        Paint textPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        labelPaint.setColor(Color.WHITE);
        labelPaint.setTextSize(20);
        labelPaint.setStrokeWidth(2);

        canvas.drawText(String.valueOf(label), canvas.getWidth() - bounds.width() - 25, y + bounds.height()/2, textPaint);
    }

    private void drawHorizontalLine(Canvas canvas, float y)
    {
        canvas.drawLine(0, y, canvas.getWidth(), y, gridPaint);
    }

    private Path getSimplePath(Double[] data, float start_y, float height, int width)
    {
        Path path = new Path();
        List<PointF> points = getPoints(data, start_y, height, width);
        boolean first = true;
        for(PointF point : points){
            if(first){
                first = false;
                path.moveTo(point.x, point.y);
            }
            else{
                path.lineTo(point.x, point.y);
            }
        }
        return path;
    }

    private List<PointF> getPoints(Double[] data, float start_y, float height, int width)
    {
        List<PointF> points = new ArrayList<>();
        float x = 0;
        for (int i = 0; i<data.length; i++)
        {
            int y = (int)(data[i] * 1e6 * height / vScale)+(int)(start_y+height/2);
            if (y>=(int)(start_y+height)) y = (int)(start_y+height)-1;
            if (y<=(int)(start_y)) y = (int)(start_y)+1;
            PointF newPoint = new PointF((int)x, y);
            x+=((float)width/data.length);
            points.add(newPoint);
        }
        return points;
    }

    /**
     * Sets previous touch coordinates
     * @param x
     * 		current touch x coordinate
     * @param y
     * 		current touch y coordinate
     * */
    public void SetLastTouch(float x, float y)
    {
        _lastTouchedX = x;
        _lastTouchedY = y;
    }



}
