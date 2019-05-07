#ifndef CLISTENER_H
#define CLISTENER_H

#include "lib_export.h"

typedef void* ListenerHandle;
SDK_SHARED void free_listener_handle(ListenerHandle handle);

#endif // CLISTENER_H
