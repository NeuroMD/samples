#ifndef SCANNER_H
#define SCANNER_H

#include <stdbool.h>
#include <stddef.h>
#include <stdint.h>
#include "lib_export.h"
#include "clistener.h"

#ifdef __ANDROID__
#include <jni.h>
typedef char AddressType[256];
#elif __APPLE__
typedef char AddressType[256];
#else
typedef uint64_t AddressType;
#endif

typedef struct _DevScanner DeviceScanner;
typedef struct _DeviceEnumerator DeviceEnumerator;
typedef struct _Device Device;

typedef enum _DeviceType {
	DeviceTypeBrainbit,
	DeviceTypeCallibri,
	DeviceTypeAny
} DeviceType;

typedef struct _DeviceInfo {
	char Name[256];
	AddressType Address;
} DeviceInfo;

typedef struct _DeviceInfoArray {
	DeviceInfo *info_array;
	size_t info_count;
} DeviceInfoArray;

#ifdef __ANDROID__
SDK_SHARED DeviceScanner* create_device_scanner(jobject context);
SDK_SHARED DeviceEnumerator* create_device_enumerator(DeviceType, jobject context);
#else
SDK_SHARED DeviceScanner* create_device_scanner();
SDK_SHARED DeviceEnumerator* create_device_enumerator(DeviceType);
#endif

SDK_SHARED void scanner_delete(DeviceScanner *);
SDK_SHARED int scanner_start_scan(DeviceScanner *, int timeout_ms);
SDK_SHARED int scanner_stop_scan(DeviceScanner *);
SDK_SHARED int scanner_is_scanning(DeviceScanner *, bool* out_is_scanning);
SDK_SHARED int scanner_set_device_found_callback(DeviceScanner *, void(*)(DeviceScanner *, Device *, void *), ListenerHandle *, void *user_data);
SDK_SHARED int scanner_set_scan_state_callback(DeviceScanner *, void(*)(DeviceScanner *, bool, void *), ListenerHandle *, void *user_data);
SDK_SHARED Device* scanner_get_device_by_address(DeviceScanner *, const char *address);

SDK_SHARED void enumerator_delete(DeviceEnumerator *);
SDK_SHARED int enumerator_set_device_list_changed_callback(DeviceEnumerator *, void(*)(DeviceEnumerator *, void *), ListenerHandle *, void *user_data);
SDK_SHARED int enumerator_get_device_list(DeviceEnumerator *, DeviceInfoArray *out_device_array);

SDK_SHARED void free_DeviceInfoArray(DeviceInfoArray);

#endif // SCANNER_H
