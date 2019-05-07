#ifndef CELECTRODE_STATE_CHANNEL_H
#define CELECTRODE_STATE_CHANNEL_H

#include "cchannels.h"

typedef struct _ElectrodeStateChannel ElectrodeStateChannel;

typedef enum _ElectrodeState {
	ElectrodeStateNormal,
	ElectrodeStateHighResistance,
	ElectrodeStateDetached
} ElectrodeState;

SDK_SHARED ElectrodeStateChannel* create_ElectrodeStateChannel(Device *device_ptr);
SDK_SHARED int ElectrodeStateChannel_get_buffer_size(ElectrodeStateChannel *channel, size_t *out_buffer_size);
SDK_SHARED int ElectrodeStateChannel_read_data(ElectrodeStateChannel *channel, size_t offset, size_t length, ElectrodeState *out_buffer, size_t buffer_size, size_t *samples_read);

#endif // CRESISTANCE_CHANNEL_H
