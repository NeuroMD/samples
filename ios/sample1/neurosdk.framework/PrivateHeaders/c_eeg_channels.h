#ifndef EEG_EXTENSIONS_CHANNELS_H
#define EEG_EXTENSIONS_CHANNELS_H

#include "lib_export.h"
#include "cdevice.h"
#include "cchannels.h"

typedef struct _EegDoubleChannel EegDoubleChannel;
typedef struct _EegIndexChannel EegIndexChannel;
typedef struct _EegArtifactChannel EegArtifactChannel;
typedef struct _EmotionalStateChannel EmotionalStateChannel;

SDK_SHARED EegDoubleChannel* create_EegDoubleChannel(Device *device_ptr);
SDK_SHARED EegDoubleChannel* create_EegDoubleChannel_info(Device *device_ptr, ChannelInfo info);
SDK_SHARED int EegDoubleChannel_get_buffer_size(EegDoubleChannel *channel, size_t *out_buffer_size);

typedef enum _ArtifactType {
	ArtifactTypeNone,
	ArtifactTypeNoise,
	ArtifactTypeBlink,
	ArtifactTypeBrux
} ArtifactType;

typedef struct _ArtifactZone {
	double time;
	double duration;
	ArtifactType type;
} ArtifactZone;

SDK_SHARED EegArtifactChannel* create_EegArtifactChannel_eeg_channels(EegDoubleChannel *t3, EegDoubleChannel *t4, EegDoubleChannel *o1, EegDoubleChannel *o2);
SDK_SHARED int EegArtifactChannel_read_data(EegArtifactChannel *channel, size_t offset, size_t length, ArtifactZone *out_buffer, size_t buffer_size, size_t *samples_read);
SDK_SHARED int EegArtifactChannel_get_buffer_size(EegArtifactChannel *channel, size_t *out_buffer_size);

typedef struct _EegIndexValues {
	double AlphaRate;
	double BetaRate;
	double DeltaRate;
	double ThetaRate;
} EegIndexValues;

SDK_SHARED EegIndexChannel* create_EegIndexChannel(EegDoubleChannel *t3, EegDoubleChannel *t4, EegDoubleChannel *o1, EegDoubleChannel *o2);
SDK_SHARED int EegIndexChannel_read_data(EegIndexChannel *channel, size_t offset, size_t length, EegIndexValues *out_buffer, size_t buffer_size, size_t *samples_read);
SDK_SHARED int EegIndexChannel_get_buffer_size(EegIndexChannel *channel, size_t *out_buffer_size);
SDK_SHARED int EegIndexChannel_set_delay(EegIndexChannel *channel, double delay_seconds);
SDK_SHARED int EegIndexChannel_set_weight_coefficients(EegIndexChannel *channel, double alpha, double beta, double delta, double theta);

typedef struct _EmotionalState {
	int State;
	int Stress;
	int Attention;
	int Relax;
	int Meditation;
} EmotionalState;

typedef struct _StateCoefficients {
	double PX1;
	double PX2;
	double PX3;
	double PX4;
	double NX1;
	double NX2;
	double NX3;
	double NX4;
	double PY1;
	double PY2;
	double PY3;
	double PY4;
	double NY1;
	double NY2;
	double NY3;
	double NY4;
} StateCoefficients;

SDK_SHARED EmotionalStateChannel* create_EmotionalStateChannel(EegIndexChannel *index_channel);
SDK_SHARED int EmotionalStateChannel_read_data(EmotionalStateChannel *channel, size_t offset, size_t length, EmotionalState *out_buffer, size_t buffer_size, size_t *samples_read);
SDK_SHARED int EmotionalStateChannel_get_buffer_size(EmotionalStateChannel *channel, size_t *out_buffer_size);
SDK_SHARED int EmotionalStateChannel_get_state_coefficients(EmotionalStateChannel *channel, StateCoefficients *out_state_coeffs);
SDK_SHARED int EmotionalStateChannel_set_state_coefficients(EmotionalStateChannel *channel, StateCoefficients state_coeffs);

#endif
