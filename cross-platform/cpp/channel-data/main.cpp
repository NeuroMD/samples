#include <iostream>
#include <vector>
#include "device_scanner/scanner_factory.h"
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

void connectDevice(Neuro::DeviceSharedPtr device_ptr){
    using Neuro::Parameter;
	std::cout << "Connecting device [" 
              << device_ptr->readParam<Parameter::Address>()<< "]" << "\n";

	using device_t = typename std::remove_reference_t<decltype(device_ptr)>::element_type;
	auto weakDevice = std::weak_ptr<device_t>(device_ptr);
    device_ptr->setParamChangedCallback([weakDevice](auto param){
        if (param == Parameter::State){
			auto device = weakDevice.lock();
			if (device != nullptr) {
				auto state = device->readParam<Parameter::State>();
				if (state == Neuro::DeviceState::Connected) {
					std::cout << "Device ["
						<< device->readParam<Parameter::Address>()
						<< "] connected" << "\n";
					createBatteryChannel(device);
				}
			}
        }
    });
	device_ptr->connect();
	FoundDevices.push_back(device_ptr);
}

void onDeviceFound(Neuro::DeviceUniquePtr device_ptr){
    using Neuro::Parameter;
    using Neuro::DeviceState;
    using Neuro::to_string;


    auto deviceState = device_ptr->readParam<Parameter::State>();
	using device_t = typename std::remove_reference_t<decltype(device_ptr)>::element_type;
	auto sharedDevice = std::shared_ptr<device_t>(std::move(device_ptr));
	if (deviceState != DeviceState::Connected) {
		connectDevice(sharedDevice);
	}
	else{
		createBatteryChannel(sharedDevice);
	}
}

int main(int argc, char *argv[]){
	LoggerFactory::getCurrentPlatformLogger()->setLogLevel(LogLevel::Info);
    auto scanner = Neuro::createDeviceScanner();
    scanner->subscribeDeviceFound([&](Neuro::DeviceUniquePtr device_ptr){
        onDeviceFound(std::forward<decltype(device_ptr)>(device_ptr));
    });
    scanner->startScan(0);//zero timeout for infinity
    while (std::cin.get() != '\n');
}
