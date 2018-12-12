#include <iostream>
#include <vector>
#include "device_scanner/scanner_factory.h"
#include "device/param_values.h"

std::vector<std::shared_ptr<Neuro::Device>> FoundDevices;

template <typename T>
void displayDeviceFeatures(T&& device_ptr) {
	using Neuro::to_string;

	std::cout << "Device can execute:" << "\n";
	auto commands = device_ptr->commands();
	for (auto& cmd : commands) {
		std::cout << "-" << to_string(cmd) << "\n";
	}
	std::cout << "\n";

	std::cout << "Device has parameters:" << "\n";
	auto params = device_ptr->parameters();
	for (auto& paramPair : params) {
		std::cout << "-" << to_string(paramPair.first) << " {" << to_string(paramPair.second) << "}" << "\n";
	}
	std::cout << "\n";

	std::cout << "Device has channels:" << "\n";
	auto channels = device_ptr->channels();
	for (auto& channel : channels) {
		std::cout << "-" << channel.getName() << "\n";
	}
	std::cout << std::endl;
}

template <typename T>
void connectDevice(T&& device_ptr) {
	using Neuro::Parameter;
	std::cout << "Connecting device ["
		<< device_ptr->readParam<Parameter::Address>() << "]" << "\n";

	using device_t = typename std::remove_reference_t<decltype(device_ptr)>::element_type;
	auto weakDevice = std::weak_ptr<device_t>(device_ptr);
	device_ptr->setParamChangedCallback([weakDevice](auto param) {
		if (param == Parameter::State) {
			auto device = weakDevice.lock();
			if (device != nullptr) {
				auto state = device->readParam<Parameter::State>();
				if (state == Neuro::DeviceState::Connected) {
					std::cout << "Device ["
						<< device->readParam<Parameter::Address>()
						<< "] connected" << "\n";
					displayDeviceFeatures(device);
				}
			}
		}
	});
	device_ptr->connect();
	FoundDevices.push_back(device_ptr);
}

template <typename T>
void onDeviceFound(T&& device_ptr) {
	using Neuro::Parameter;
	using Neuro::DeviceState;
	using Neuro::to_string;

	auto deviceName = device_ptr->readParam<Parameter::Name>();
	auto deviceAddress = device_ptr->readParam<Parameter::Address>();
	auto deviceState = device_ptr->readParam<Parameter::State>();
	std::cout << deviceName
		<< " [" << deviceAddress << "] "
		<< to_string(deviceState)
		<< "\n";

	using device_t = typename std::remove_reference_t<decltype(device_ptr)>::element_type;
	auto sharedDevice = std::shared_ptr<device_t>(std::forward<T>(device_ptr));
	if (deviceState != DeviceState::Connected) {
		connectDevice(sharedDevice);
	}
	else {
		displayDeviceFeatures(sharedDevice);
	}
}

int main(int argc, char *argv[]) {
	LoggerFactory::getCurrentPlatformLogger()->setLogLevel(LogLevel::Info);
	auto scanner = Neuro::createDeviceScanner();
	scanner->subscribeDeviceFound([&](auto&& device_ptr) {
		onDeviceFound(std::forward<decltype(device_ptr)>(device_ptr));
	});
	scanner->startScan(0);//zero timeout for infinity
	while (std::cin.get() != '\n');
}
