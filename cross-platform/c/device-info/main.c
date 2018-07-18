#include "cscanner.h"
#include "cdevice.h"
#include "cparams.h"
#include <stdio.h>

void on_device_found(Device *device) {
	char nameBuffer[128];
	device_read_Name(device, nameBuffer, 128);
	//device_read_State(device, DeviceState *out_state);
	//device_read_Address(device, char *out_address, size_t length);
}

int main(int argc, char* argv[]) {
	DeviceScanner *scanner = create_device_scanner();
	scanner_set_device_found_callback(scanner, &on_device_found);
	scanner_start_scan(scanner, 0);//zero timeout for infinity
	getchar();
	scanner_stop_scan(scanner);
	scanner_delete(scanner);
}
