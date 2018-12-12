#include <iostream>
#include <vector>
#include <memory>
#include "device/device.h"
#include "channels/device_channel.h"
#include "device_scanner/scanner_factory.h"
#include "device/param_values.h"

using ResistanceChannelType = Neuro::DeviceChannel<Neuro::ChannelInfo::Type::Resistance>;
using ElectrodeStateChannelType = Neuro::DeviceChannel<Neuro::ChannelInfo::Type::ElectrodesState>;

std::vector<std::shared_ptr<Neuro::Device>> FoundDevices;
std::vector<std::shared_ptr<ResistanceChannelType>> ResistanceChannels;
std::vector<ResistanceChannelType::LengthListenerType> ResistanceLengthListeners;
std::shared_ptr<ElectrodeStateChannelType> ElectrodeStateChannel;
ElectrodeStateChannelType::LengthListenerType ElectrodeStateLengthListener;

void redrawResist() {
	std::cout << "\r";
	if (ElectrodeStateChannel != nullptr) {
		auto length = ElectrodeStateChannel->totalLength();
		if (length > 0) {
			auto lastState = ElectrodeStateChannel->readData(length - 1, length)[0];
			std::cout << "Electrodes: " << (lastState == Neuro::ElectrodeState::Normal ? "Normal" : "Detached") << "\t";
		}
	}
	else {
		for (auto& channel : ResistanceChannels) {
			auto length = channel->totalLength();
			if (length > 0) {
				auto resistance = channel->readData(length - 1, length)[0];
				std::cout <<  channel->info().getName() << ": " << resistance << "\t";
			}
		}
	}
}
void startResistMeasure(std::shared_ptr<Neuro::Device> device_ptr){
	ResistanceChannels.clear();
	ResistanceLengthListeners.clear();
	std::cout << std::endl;
	auto deviceChannels = device_ptr->channels();
	bool hasResistance = false;
	for (auto& info : deviceChannels) {
		if (info.getType() == Neuro::ChannelInfo::Type::Resistance) {
			ResistanceChannels.push_back(std::make_shared<ResistanceChannelType>(device_ptr, info));
			auto listener = ResistanceChannels.back()->subscribeLengthChanged([](auto length) {
				redrawResist();
			});
			ResistanceLengthListeners.push_back(listener);
			hasResistance = true;
		}
		else if (info.getType() == Neuro::ChannelInfo::Type::ElectrodesState) {
			ElectrodeStateChannel = std::make_shared<ElectrodeStateChannelType>(device_ptr);
			ElectrodeStateLengthListener = ElectrodeStateChannel->subscribeLengthChanged([](auto length) {
				redrawResist();
			});
		}
	}
	if (hasResistance) {
		device_ptr->execute(Neuro::Command::StartResist);
	}
}

void connectDevice(std::shared_ptr<Neuro::Device> device_ptr){
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
					startResistMeasure(device);
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

    auto deviceName = device_ptr->readParam<Parameter::Name>();
    auto deviceAddress = device_ptr->readParam<Parameter::Address>();
    auto deviceState = device_ptr->readParam<Parameter::State>();
    std::cout << deviceName
              << " [" << deviceAddress << "] "
              << to_string(deviceState)
              << "\n";

	using device_t = typename std::remove_reference_t<decltype(device_ptr)>::element_type;
	auto sharedDevice = std::shared_ptr<device_t>(std::move(device_ptr));
	if (deviceState != DeviceState::Connected) {
		connectDevice(sharedDevice);
	}
	else{
		FoundDevices.push_back(sharedDevice);
		startResistMeasure(sharedDevice);
	}
}

int main(int argc, char *argv[]){
	LoggerFactory::getCurrentPlatformLogger()->setLogLevel(LogLevel::Info);
    auto scanner = Neuro::createDeviceScanner();
    scanner->subscribeDeviceFound([&](Neuro::DeviceUniquePtr device_ptr){
        onDeviceFound(std::move(device_ptr));
    });
    scanner->startScan(0);//zero timeout for infinity
    while (std::cin.get() != '\n');
}
