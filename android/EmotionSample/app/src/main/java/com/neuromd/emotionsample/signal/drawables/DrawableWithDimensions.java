package com.neuromd.emotionsample.signal.drawables;

public abstract class DrawableWithDimensions {
    
    protected int mLeft;
    protected int mTop;
    protected int mWidth;
    protected int mHeight;
    
    public void setPosition(int x, int y){
        mLeft = x;
        mTop = y;
    }
    
    public void setSize(int width, int height){
        mWidth = width;
        mHeight = height;
    }
}
