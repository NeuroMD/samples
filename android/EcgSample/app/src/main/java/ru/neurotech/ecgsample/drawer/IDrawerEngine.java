package ru.neurotech.ecgsample.drawer;

import android.graphics.Canvas;

public interface IDrawerEngine {

    /**
     * Updates business logic
     */
    void Update();

    /**
     * Draws all objects according to their parameters
     * @param canvas
     * 			canvas on which will be drawn the objects
     * */
    void Draw(Canvas canvas);
}
