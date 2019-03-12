#include <iostream>
#include "device_scanner/device_enumerator.h"
#include "device/brainbit.h"
#include "device/callibri.h"
#include "device/device.h"
#include "logger.h"

void onDeviceListChanged(const std::vector<Neuro::DeviceInfo>& devices){
	std::cout << "=====================" << std::endl;
	for (const auto &deviceInfo : devices) {
		std::cout << deviceInfo << std::endl;
	}
	std::cout << "=====================" << std::endl;
}

int main(int argc, char *argv[]) {
	LoggerFactory::getCurrentPlatformLogger()->setLogLevel(LogLevel::Info);
	auto enumerator = Neuro::make_device_enumerator<Neuro::Device>();
	auto notifierHandle = enumerator.subscribeDeviceListChanged([&]() {
		onDeviceListChanged(enumerator.devices());
	});
	onDeviceListChanged(enumerator.devices());
	while (std::cin.get() != '\n');
}
