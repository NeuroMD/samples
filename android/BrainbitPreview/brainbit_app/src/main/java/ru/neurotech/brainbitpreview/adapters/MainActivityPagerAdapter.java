package ru.neurotech.brainbitpreview.adapters;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import ru.neurotech.brainbitpreview.fragments.IndicatorsFragment;
import ru.neurotech.brainbitpreview.fragments.RawDataFragment;
import ru.neurotech.brainbitpreview.fragments.SpectrumFragment;

public class MainActivityPagerAdapter extends FragmentPagerAdapter {

    public static final int TYPE_RAW_DATA = 0;
    public static final int TYPE_SPECTRUM = 1;
    public static final int TYPE_INDICATORS = 2;
    public static final int TYPE_COUNT = 3;


    public MainActivityPagerAdapter(FragmentManager fragmentManager) {
        super(fragmentManager);
    }

    @Override
    public int getCount() {
        return TYPE_COUNT;
    }

    public int getItemViewType(int position) {
        switch (position) {
            case 0: {
                return TYPE_RAW_DATA;
            }
            case 1: {
                return TYPE_SPECTRUM;
            }
            default: {
                return TYPE_INDICATORS;
            }
        }
    }

    @Override
    public Fragment getItem(int position) {
        Fragment fragment = null;
        int itemViewType = getItemViewType(position);
        switch (itemViewType) {
            case TYPE_RAW_DATA: {
                fragment = new RawDataFragment();
                break;
            }
            case TYPE_SPECTRUM: {
                fragment = new SpectrumFragment();
                break;
            }
            case TYPE_INDICATORS: {
                fragment = new IndicatorsFragment();
                break;
            }
        }
        return fragment;
    }

    @Override
    public CharSequence getPageTitle(int position) {
        int viewType = getItemViewType(position);
        String title;

        switch (viewType) {
            case TYPE_RAW_DATA: {
                title = "Raw data";
                break;
            }
            case TYPE_SPECTRUM: {
                title = "Spectrum";
                break;
            }
            case TYPE_INDICATORS: {
                title = "Indicators";
                break;
            }
            default:
                title = "";
                break;
        }
        return title;
    }
}