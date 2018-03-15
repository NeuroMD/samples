package ru.neurotech.simplebiofeedback;

import android.app.Activity;
import android.support.v4.content.ContextCompat;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import ru.neurotech.common.INotificationCallback;
import ru.neurotech.common.SubscribersNotifier;
import ru.neurotech.neurodevices.bfb.BfbIndex;

public class BfbBarPresenter {

    private final BfbDeviceModel mModel;
    private final Activity mActivity;
    private float mScaleFactor = 1.0f;
    private int mBottomMargin = 30;
    private float mLastTouchY;
    private float mDXY;

    private boolean mBarEnabled = false;
    private int mCalibrationBarVisibility = View.INVISIBLE;


    public BfbBarPresenter(final Activity activity, final BfbDeviceModel model){
        mActivity= activity;
        mModel = model;
        final Button frameButton = (Button) activity.findViewById(R.id.frameButton);
        ViewGroup.MarginLayoutParams layoutParams = (ViewGroup.MarginLayoutParams) frameButton.getLayoutParams();
        mBottomMargin = layoutParams.bottomMargin;
        mModel.selectedIndexChanged.subscribe(new INotificationCallback<BfbIndex>() {
            @Override
            public void onNotify(Object sender, BfbIndex selectedIndex) {
                if (selectedIndex == null){
                    mBarEnabled = false;
                    mActivity.runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            barEnabledChanged.sendNotification(this, mBarEnabled);
                        }
                    });
                }
                else{
                    mBarEnabled = true;
                    barEnabledChanged.sendNotification(this, mBarEnabled);
                    mModel.indexValueChanged.subscribe(new INotificationCallback<Integer>() {
                        @Override
                        public void onNotify(Object sender, final Integer indexValue) {
                            mActivity.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    onIndexValueChanged(indexValue);
                                }
                            });
                        }
                    });
                    mModel.calibrationFinished.subscribe(new INotificationCallback() {
                        @Override
                        public void onNotify(Object o, Object o2) {
                            mCalibrationBarVisibility = View.INVISIBLE;
                            mActivity.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    calibrationBarVisibilityChanged.sendNotification(this, mCalibrationBarVisibility);
                                }
                            });
                        }
                    });
                    mModel.calibrationStarted.subscribe(new INotificationCallback() {
                        @Override
                        public void onNotify(Object o, Object o2) {

                            mCalibrationBarVisibility = View.VISIBLE;
                            mActivity.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    calibrationBarVisibilityChanged.sendNotification(this, mCalibrationBarVisibility);
                                }
                            });
                        }
                    });
                }
            }
        });
    }

    public SubscribersNotifier<FrameParams> frameParametersChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> barHeightChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> barColorChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Integer> calibrationBarVisibilityChanged = new SubscribersNotifier<>();
    public SubscribersNotifier<Boolean> barEnabledChanged = new SubscribersNotifier<>();

    public void onScaleFactorChanged(float scaleFactor){

        mScaleFactor *= scaleFactor;
        mScaleFactor = Math.max(1.0f, Math.min(mScaleFactor, 5.0f));
    }

    public void onActionDown(float y){
        mLastTouchY = y;
    }

    public void onActionMove(float y){
        // Calculate the distance moved
        final Button frameButton = (Button) mActivity.findViewById(R.id.frameButton);
        final RelativeLayout barLayout = (RelativeLayout) mActivity.findViewById(R.id.barLayout);
        mDXY = (y - mLastTouchY)/barLayout.getHeight()*mActivity.findViewById(R.id.activity_main).getHeight();

        mBottomMargin -= mDXY;
        if (mBottomMargin + frameButton.getHeight() > barLayout.getHeight())
            mBottomMargin = barLayout.getHeight()-frameButton.getHeight();

        if (mBottomMargin < 0) mBottomMargin = 0;

        frameParametersChanged.sendNotification(this, new FrameParams(80, mBottomMargin));

        // Remember this touch position for the next move event
        mLastTouchY = y;
    }

    private void onIndexValueChanged(int level){

        Button frameButton = (Button)mActivity.findViewById(R.id.frameButton);
        RelativeLayout barLayout = (RelativeLayout) mActivity.findViewById(R.id.barLayout);
        RelativeLayout.LayoutParams frameParams = (RelativeLayout.LayoutParams) frameButton.getLayoutParams();
        final int bottom = (int) ((float) frameParams.bottomMargin / barLayout.getHeight() * 100f);
        final int top = bottom + (int) ((float) frameButton.getHeight() / barLayout.getHeight() * 100);


        int barColor = ContextCompat.getColor(mActivity, android.R.color.holo_green_dark);

        if (level > top) {
            if (level - top > 15) {
                barColor = ContextCompat.getColor(mActivity, android.R.color.holo_red_light);
            } else {
                barColor = ContextCompat.getColor(mActivity, android.R.color.holo_orange_light);
            }
        } else if (level < bottom) {
            if (bottom - level > 15) {
                barColor = ContextCompat.getColor(mActivity, android.R.color.holo_red_light);
            } else {
                barColor = ContextCompat.getColor(mActivity, android.R.color.holo_orange_light);
            }
        }

        barColorChanged.sendNotification(this, barColor);

        int barHeight = (int) ((float) barLayout.getHeight() / 100f * level);
        barHeightChanged.sendNotification(this, barHeight);
    }

    public class FrameParams{
        private int mHeight;
        private int mBottomMargin;

        public FrameParams(int height, int bottomMargin){
            this.mHeight = height;
            this.mBottomMargin = bottomMargin;
        }

        public int height(){return mHeight;}
        public int bottomMargin() {return mBottomMargin;}
    }
}
