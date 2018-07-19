#include <stdio.h>
#include <stdlib.h>
#include "cscanner.h"
#include "cdevice.h"
#include "cparams.h"
#include "sdk_error.h"

void on_device_connected(Device* device) {
	CommandArray commands;
	if (device_available_commands(device, &commands) == SDK_NO_ERROR) {
		printf("Device can execute:\n");
		for (size_t i = 0; i < commands.cmd_array_size; ++i) {
			Command cmd = commands.cmd_array[i];
			char commandName[32];
			command_to_string(cmd, commandName, 32);
			printf("    -%s\n", commandName);
		}
		free(commands.cmd_array);
	}

	ParamInfoArray parameters;
	if (device_available_parameters(device, &parameters) == SDK_NO_ERROR) {
		printf("Device has parameters:\n");
		for (size_t i = 0; i < parameters.info_count; ++i) {
			ParameterInfo paramInfo = parameters.info_array[i];
			char paramName[32];
			parameter_to_string(paramInfo.parameter, paramName, 32);
			char access[32];
			parameter_access_to_string(paramInfo.access, access, 32);
			printf("    -%s {%s}\n", paramName, access);
		}
		free(parameters.info_array);
	}

	ChannelInfoArray channels;
	if (device_available_channels(device, &channels) == SDK_NO_ERROR) {
		printf("Device has channels:\n");
		for (size_t i = 0; i < channels.info_count; ++i) {
			ChannelInfo channelInfo = channels.info_array[i];
			printf("    -%s\n", channelInfo.name);
		}
		free(channels.info_array);
	}
}

void on_param_changed(Device *device, Parameter param) {
	if (param == ParameterState) {
		DeviceState state;
		device_read_State(device, &state);
		if (state == DeviceStateConnected) {
			printf("Device connected\n");
			on_device_connected(device);
		}
	}
}

void on_device_found(Device *device) {
	char nameBuffer[128];
	device_read_Name(device, nameBuffer, 128);

	DeviceState state;
	device_read_State(device, &state);

	char addressBuffer[64];
	device_read_Address(device, addressBuffer, 64);

	printf("Found device %s [%s], state: %s\n", nameBuffer, addressBuffer, state == DeviceStateConnected ? "Connected" : "Disconnected");

	device_subscribe_param_changed(device, &on_param_changed);
	if (state != DeviceStateConnected) {
		device_connect(device);
	}
	else {
		on_device_connected(device);
	}
}

int main(int argc, char* argv[]) {
	DeviceScanner *scanner = create_device_scanner();
	scanner_set_device_found_callback(scanner, &on_device_found);
	scanner_start_scan(scanner, 0);//zero timeout for infinity
	getchar();
	scanner_stop_scan(scanner);
	scanner_delete(scanner);
}
