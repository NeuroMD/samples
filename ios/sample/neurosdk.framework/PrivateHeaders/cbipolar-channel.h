#ifndef CBIPOLAR_CHANNEL_H
#define CBIPOLAR_CHANNEL_H

#include "cchannels.h"

typedef struct _BipolarDoubleChannel BipolarDoubleChannel;

SDK_SHARED BipolarDoubleChannel* create_BipolarDoubleChannel(DoubleChannel *first, DoubleChannel *second);

#endif // CBIPOLAR_CHANNEL_H
