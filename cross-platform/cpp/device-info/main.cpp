#include <iostream>
#include <vector>
#include "device_scanner/device_enumerator.h"
#include "device/device.h"
#include "device/param_values.h"

void displayDeviceFeatures(const Neuro::Device &device) {
	using Neuro::to_string;

	std::cout << "Device can execute:" << "\n";
	auto commands = device.commands();
	for (auto& cmd : commands) {
		std::cout << "-" << to_string(cmd) << "\n";
	}
	std::cout << "\n";

	std::cout << "Device has parameters:" << "\n";
	auto params = device.parameters();
	for (auto& paramPair : params) {
		std::cout << "-" << to_string(paramPair.first) << " {" << to_string(paramPair.second) << "}" << "\n";
	}
	std::cout << "\n";

	std::cout << "Device has channels:" << "\n";
	auto channels = device.channels();
	for (auto& channel : channels) {
		std::cout << "-" << channel.getName() << "\n";
	}
	std::cout << std::endl;
}

void onDeviceFound(Neuro::Device &&device) {
	using Neuro::Parameter;
	using Neuro::DeviceState;
	using Neuro::to_string;

	const auto deviceName = device.readParam<Parameter::Name>();
	const auto deviceAddress = device.readParam<Parameter::Address>();
	const auto deviceState = device.readParam<Parameter::State>();
	std::cout << deviceName
		<< " [" << deviceAddress << "] "
		<< to_string(deviceState)
		<< "\n";

	if (deviceState != DeviceState::Connected) {
		std::cout << "Connecting device [" << device.readParam<Parameter::Address>() << "]" << "\n";
		device.connect();
		while (device.readParam<Parameter::State>() != DeviceState::Connected);
	}

	displayDeviceFeatures(device);
	device.disconnect();
}

void onDeviceListChanged(const std::vector<Neuro::DeviceInfo>& devices) {
	for (const auto &deviceInfo : devices) {
		onDeviceFound(Neuro::Device(deviceInfo));
	}
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