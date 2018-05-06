package com.neuromd.ecgsample;

import android.os.Environment;
import android.util.Log;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.concurrent.locks.ReentrantLock;

import com.neuromd.common.INotificationCallback;
import com.neuromd.common.SubscribersNotifier;
import com.neuromd.neurosdk.Device;

public class EcgSignalStorage {

    private final EcgDeviceModel mModel;
    private double[] mSignalBuffer;
    private ArrayList<Integer> mRPeakSamples;
    private ReentrantLock mBufferLock = new ReentrantLock();
    private int lastAdditionLength = 0;

    public EcgSignalStorage(EcgDeviceModel model){
        mModel = model;
        mModel.selectedDeviceChanged.subscribe(new INotificationCallback<Device>() {
            @Override
            public void onNotify(Object o, final Device ecgDevice) {
                if (ecgDevice != null){
                    createDataBuffers();
                    lastAdditionLength = 0;
                }
                else {
                    mSignalBuffer = null;
                    mRPeakSamples = null;
                }
            }
        });
        mModel.signalDurationChanged.subscribe(new INotificationCallback<Double>() {
            @Override
            public void onNotify(Object o, Double signalDuration) {
                onSignalDurationChanged(signalDuration);
            }
        });
    }

    public SubscribersNotifier signalFileSaved = new SubscribersNotifier();
    public SubscribersNotifier<String> fileSaveError = new SubscribersNotifier<>();

    public void save(){
        if (mSignalBuffer == null || mSignalBuffer.length == 0){
            return;
        }

        /*RPeak[] peaks = mModel.getRPeaks(0, mModel.getTotalDuration());
        for (RPeak p : peaks){
            mRPeakSamples.add((int)(p.time()*mModel.getSamplingFrequency()));
        }*/

        if (!isExternalStorageWritable()){
            return;
        }

        mBufferLock.lock();
        File signalStorageFile = new File(getSignalStorageDir(), "signal.spl");
        //File peaksStorageFile = new File(getSignalStorageDir(), "rpeaks.snb");
        try {
            FileOutputStream signalSaveStream = new FileOutputStream(signalStorageFile);
            for (int i = 0; i < lastAdditionLength; ++i) {
                double sample = mSignalBuffer[i];
                byte[] output = new byte[8];
                ByteBuffer.wrap(output).order(ByteOrder.LITTLE_ENDIAN).putDouble(sample);
                signalSaveStream.write(output);
            }

            /*FileOutputStream peaksSaveStream = new FileOutputStream(peaksStorageFile);
            for (int i = 0; i < mRPeakSamples.size(); ++i){
                byte[] output = new byte[4];
                ByteBuffer.wrap(output).order(ByteOrder.LITTLE_ENDIAN).putInt(mRPeakSamples.get(i));
                peaksSaveStream.write(output);
            }*/

            signalFileSaved.sendNotification(this, null);
        }
        catch(FileNotFoundException e){
            Log.e("EcgSignalStorage", "FileNotFound");
            fileSaveError.sendNotification(this, "File not found");
        }
        catch(IOException e){
            Log.e("EcgSignalStorage", "Write error");
            fileSaveError.sendNotification(this, "File write error");
        }
        mBufferLock.unlock();
    }

    private void onSignalDurationChanged(Double signalDuration) {
        if (mSignalBuffer == null)
            return;

        mBufferLock.lock();
        int newLength = (int)(signalDuration * mModel.getSamplingFrequency());
        int offset = lastAdditionLength;
        int length = newLength - lastAdditionLength;
        Double[] newSignal = mModel.getRawSignal(offset, length);
        if (newSignal == null || newSignal.length!=length) {
            mBufferLock.unlock();
            return;
        }
        for (int i = 0; i < newSignal.length; ++i){
            mSignalBuffer[lastAdditionLength+i] = newSignal[i];
        }
        lastAdditionLength = newLength;
        mBufferLock.unlock();
    }

    private void createDataBuffers() {
        mSignalBuffer = new double[300000];
        mRPeakSamples = new ArrayList<>();
    }

    private boolean isExternalStorageWritable() {
        String state = Environment.getExternalStorageState();
        if (Environment.MEDIA_MOUNTED.equals(state)) {
            return true;
        }
        return false;
    }

    private File getSignalStorageDir() {
        // Get the directory for the user's public pictures directory.
        File file = new File(Environment.getExternalStorageDirectory().getAbsolutePath() + "/Ecg/");
        SimpleDateFormat formatter = new SimpleDateFormat("dd-MM-yy_HH-mm-ss");
        String dateString = formatter.format(Calendar.getInstance().getTime());
        File storageDir = new File(file, dateString);
        if (!storageDir.mkdirs()) {
            Log.e("EcgSignalStorage", "Directory not created");
        }
        return storageDir;
    }
}
