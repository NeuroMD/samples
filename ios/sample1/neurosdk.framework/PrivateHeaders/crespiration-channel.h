#ifndef CRESPIRATION_CHANNEL_H
#define CRESPIRATION_CHANNEL_H

#include "cchannels.h"

typedef struct _RespirationDoubleChannel RespirationDoubleChannel;

SDK_SHARED RespirationDoubleChannel* create_RespirationDoubleChannel(Device *device_ptr);
SDK_SHARED int RespirationDoubleChannel_get_buffer_size(RespirationDoubleChannel *channel, size_t *out_buffer_size);

#endif // CRESISTANCE_CHANNEL_H
