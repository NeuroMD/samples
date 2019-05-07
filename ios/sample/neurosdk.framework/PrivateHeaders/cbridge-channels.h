#ifndef CBRIDGE_CHANNELS_H
#define CBRIDGE_CHANNELS_H

#include "cchannels.h"

typedef struct _BridgeDoubleChannel BridgeDoubleChannel;

typedef int(*ReadDataFunc)(size_t offset, size_t length, double *out_buffer);
typedef int(*GetFrequencyFunc)(float * out_frequency);
typedef int(*SetFrequencyFunc)(float frequency);
typedef int(*AddLengthCallbackFunc)(void(*callback)(BridgeDoubleChannel *, size_t), ListenerHandle *handle);
typedef int(*GetTotalLengthFunc)(size_t *out_length);
typedef int(*GetBufferSizeFunc)(size_t *out_buffer_size);
typedef Device*(*GetDeviceFunc)();

SDK_SHARED BridgeDoubleChannel* create_BridgeDoubleChannel_info(ChannelInfo, ReadDataFunc, GetFrequencyFunc, SetFrequencyFunc, AddLengthCallbackFunc, GetTotalLengthFunc, GetBufferSizeFunc, GetDeviceFunc);

#endif // CBRIDGE_CHANNELS_H
