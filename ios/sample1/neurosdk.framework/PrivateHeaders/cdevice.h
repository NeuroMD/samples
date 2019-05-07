#ifndef CDEVICE_H
#define CDEVICE_H

#include "lib_export.h"
#include <stdbool.h>
#include <stddef.h>
#include "clistener.h"
#include "cscanner.h"

typedef struct _Device Device;

typedef enum _Command {
	CommandStartSignal,
	CommandStopSignal,
	CommandStartResist,
	CommandStopResist,
	CommandStartMEMS,
	CommandStopMEMS,
	CommandStartRespiration,
	CommandStopRespiration,
	CommandStartStimulation,
	CommandStopStimulation,
	CommandEnableMotionAssistant,
	CommandDisableMotionAssistant,
	CommandFindMe
}Command;

SDK_SHARED int command_to_string(Command cmd, char *buffer, size_t buffer_length);

typedef struct _CommandArray {
	Command *cmd_array;
	size_t cmd_array_size;
}CommandArray;

typedef enum _Parameter{
	ParameterName,
	ParameterState,
	ParameterAddress,
	ParameterSerialNumber,
	ParameterHardwareFilterState,
	ParameterFirmwareMode,
	ParameterSamplingFrequency,
	ParameterGain,
	ParameterOffset,
	ParameterExternalSwitchState,
	ParameterADCInputState,
	ParameterAccelerometerSens,
	ParameterGyroscopeSens,
	ParameterStimulatorAndMAState,
	ParameterStimulatorParamPack,
	ParameterMotionAssistantParamPack,
	ParameterFirmwareVersion
}Parameter;

SDK_SHARED int parameter_to_string(Parameter param, char *buffer, size_t buffer_length);

typedef enum _ParamAccess {
	Read,
	ReadWrite,
	ReadNotify
}ParamAccess;

SDK_SHARED int parameter_access_to_string(ParamAccess access, char *buffer, size_t buffer_length);

typedef struct _ParameterInfo {
	Parameter parameter;
	ParamAccess access;
}ParameterInfo;

typedef struct _ParamInfoArray{
	ParameterInfo *info_array;
	size_t info_count;
} ParamInfoArray;

typedef enum _ChannelType {
	ChannelTypeSignal,
	ChannelTypeBattery,
	ChannelTypeElectrodesState,
	ChannelTypeRespiration,
	ChannelTypeMEMS,
	ChannelTypeOrientation,
	ChannelTypeConnectionStats,
	ChannelTypeResistance,
	ChannelTypePedometer,
	ChannelTypeCustom
} ChannelType;

typedef struct _ChannelInfo {
	char name[128];
	ChannelType type;
	size_t index;
} ChannelInfo;

typedef struct _ChanInfoArray{
	ChannelInfo *info_array;
	size_t info_count;
} ChannelInfoArray;

typedef struct _DoubleDataArray {
	double *data_array;
	size_t samples_count;
} DoubleDataArray;

typedef struct _IntDataArray {
	int *data_array;
	size_t samples_count;
} IntDataArray;

SDK_SHARED Device* create_Device(DeviceInfo);
SDK_SHARED int device_connect(Device *);
SDK_SHARED int device_disconnect(Device *);
SDK_SHARED void device_delete(Device *);
SDK_SHARED int device_available_channels(const Device *, ChannelInfoArray *);
SDK_SHARED int device_available_commands(const Device *, CommandArray *);
SDK_SHARED int device_available_parameters(const Device *, ParamInfoArray *);
SDK_SHARED int device_execute(Device *, Command); 
SDK_SHARED int device_subscribe_param_changed(Device*, void(*)(Device*, Parameter, void *), ListenerHandle *, void *user_data);
SDK_SHARED int device_subscribe_double_channel_data_received(Device*, ChannelInfo, void(*)(Device*, ChannelInfo, DoubleDataArray, void *), ListenerHandle *, void *user_data);
SDK_SHARED int device_subscribe_int_channel_data_received(Device*, ChannelInfo, void(*)(Device*, ChannelInfo, IntDataArray, void *), ListenerHandle *, void *user_data);
SDK_SHARED void free_ParamInfoArray(ParamInfoArray);
SDK_SHARED void free_CommandArray(CommandArray);
SDK_SHARED void free_ChannelInfoArray(ChannelInfoArray);
SDK_SHARED void free_DoubleDataArray(DoubleDataArray);
SDK_SHARED void free_IntDataArray(IntDataArray);

#endif // CDEVICE_H
