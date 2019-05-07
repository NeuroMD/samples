#ifndef CRESISTANCE_CHANNEL_H
#define CRESISTANCE_CHANNEL_H

#include "cchannels.h"

typedef struct _ResistanceDoubleChannel ResistanceDoubleChannel;

SDK_SHARED ResistanceDoubleChannel* create_ResistanceDoubleChannel_info(Device *device_ptr, ChannelInfo info);
SDK_SHARED int ResistanceDoubleChannel_get_buffer_size(ResistanceDoubleChannel *channel, size_t *out_buffer_size);

#endif // CRESISTANCE_CHANNEL_H
