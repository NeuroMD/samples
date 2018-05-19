package com.neuromd.emotionsample.signal.scale;

import android.content.Context;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.neuromd.common.SubscribersNotifier;
import com.neuromd.emotionsample.R;

public class ScaleControlView extends LinearLayout {
    
    public ScaleControlView(Context context) {
        super(context);
        View.inflate(context, R.layout.scale_controls, this);
    
        Button vScalePlusBtn = findViewById(R.id.vScalePlusButton);
        vScalePlusBtn.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                verticalPlusClicked.sendNotification(this, null);
            }
        });
    
        Button vScaleMinusBtn = findViewById(R.id.vScaleMinusButton);
        vScaleMinusBtn.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                verticalMinusClicked.sendNotification(this, null);
            }
        });
    
        Button hScalePlusBtn = findViewById(R.id.hScalePlusButton);
        hScalePlusBtn.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                horizontalPlusClicked.sendNotification(this, null);
            }
        });
    
        Button hScaleMinusBtn = findViewById(R.id.hScaleMinusButton);
        hScaleMinusBtn.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                horizontalMinusClicked.sendNotification(this, null);
            }
        });
    }
    
    public SubscribersNotifier verticalPlusClicked = new SubscribersNotifier();
    public SubscribersNotifier verticalMinusClicked = new SubscribersNotifier();
    public SubscribersNotifier horizontalPlusClicked = new SubscribersNotifier();
    public SubscribersNotifier horizontalMinusClicked = new SubscribersNotifier();
    
    public void setHorizontalScaleText(String text){
        TextView horizontalText = findViewById(R.id.hScaleTextView);
        horizontalText.setText(text);
    }
    
    public void setVerticalScaleText(String text){
        TextView verticalText = findViewById(R.id.vScaleTextView);
        verticalText.setText(text);
    }
    
    public void setVerticalPlusButtonEnabled(boolean isEnabled){
        Button vScalePlusBtn = findViewById(R.id.vScalePlusButton);
        vScalePlusBtn.setEnabled(isEnabled);
    }
    
    public void setVerticalMinusButtonEnabled(boolean isEnabled){
        Button vScaleMinusBtn = findViewById(R.id.vScaleMinusButton);
        vScaleMinusBtn.setEnabled(isEnabled);
    }
    
    public void setHorizontalPlusButtonEnabled(boolean isEnabled){
        Button hScalePlusBtn = findViewById(R.id.hScalePlusButton);
        hScalePlusBtn.setEnabled(isEnabled);
    }
    
    public void setHorizontalMinusButtonEnabled(boolean isEnabled){
        Button hScaleMinusBtn = findViewById(R.id.hScaleMinusButton);
        hScaleMinusBtn.setEnabled(isEnabled);
    }
}
