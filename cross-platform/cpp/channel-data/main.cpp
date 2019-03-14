#include <iostream>
#include <vector>
#include "device_scanner/device_enumerator.h"
#include "device/device.h"
#include "device/param_values.h"
#include "channels/device_channel.h"
#include "logger.h"

using BatteryChannel = Neuro::DeviceChannel<Neuro::ChannelInfo::Type::Battery>;


std::vector<std::shared_ptr<Neuro::Device>> FoundDevices;
std::vector<std::shared_ptr<BatteryChannel>> BatteryChannels;
std::vector<BatteryChannel::LengthListenerType> BatteryListeners;

void createBatteryChannel(Neuro::DeviceSharedPtr device_ptr){
    using Neuro::to_string;

	auto batteryChannelPtr = std::make_shared<BatteryChannel>(device_ptr);
	auto listener = batteryChannelPtr->subscribeLengthChanged([batteryChannelPtr, device_ptr](auto batteryChannelLength) {
		const auto deviceName = device_ptr->readParam<Neuro::Parameter::Name>();
		const auto deviceAddress = device_ptr->readParam<Neuro::Parameter::Address>();
		const auto batteryLevel = batteryChannelPtr->readData(batteryChannelLength - 1, 1)[0];
		std::cout << deviceName << " [" << deviceAddress << "] battery level: " << batteryLevel << std::endl;
	});
	BatteryChannels.push_back(batteryChannelPtr);
	BatteryListeners.push_back(listener);
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

	const auto device_ptr = std::make_shared<Neuro::Device>(std::move(device));
	FoundDevices.push_back(device_ptr);
	createBatteryChannel(device_ptr);
}

void onDeviceListChanged(const std::vector<Neuro::DeviceInfo>& devices) {
	for (const auto &deviceInfo : devices) {
		onDeviceFound(Neuro::Device(deviceInfo));
	}
}

int main(int argc, char *argv[]){
	LoggerFactory::getCurrentPlatformLogger()->setLogLevel(LogLevel::Info);
	auto enumerator = Neuro::make_device_enumerator<Neuro::Device>();
	auto notifierHandle = enumerator.subscribeDeviceListChanged([&]() {
		onDeviceListChanged(enumerator.devices());
	});
	onDeviceListChanged(enumerator.devices());
	while (std::cin.get() != '\n');
}
