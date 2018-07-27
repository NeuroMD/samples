#include <iostream>
#include <vector>
#include "device_scanner/scanner_factory.h"
#include "device/param_values.h"
#include "channels/resistance_channel.h"
#include "channels/electrode_state_channel.h"

std::vector<std::shared_ptr<Neuro::Device>> FoundDevices;
std::vector<std::unique_ptr<Neuro::ResistanceChannel>> ResistanceChannels;
std::vector<Neuro::CommonChannelInterface::length_listener_ptr> ResistanceLengthListeners;
std::unique_ptr<Neuro::ElectrodeStateChannel> ElectrodeStateChannel;
Neuro::CommonChannelInterface::length_listener_ptr ElectrodeStateLengthListener;

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

template <typename T>
void startResistMeasure(T&& device_ptr){
	ResistanceChannels.clear();
	ResistanceLengthListeners.clear();
	std::cout << std::endl;
	auto deviceChannels = device_ptr->channels();
	bool hasResistance = false;
	for (auto& info : deviceChannels) {
		if (info.getType() == Neuro::ChannelInfo::Type::Resistance) {
			ResistanceChannels.push_back(std::make_unique<Neuro::ResistanceChannel>(device_ptr, info));
			auto listener = ResistanceChannels.back()->subscribeLengthChanged([](auto length) {
				redrawResist();
			});
			ResistanceLengthListeners.push_back(listener);
			hasResistance = true;
		}
		else if (info.getType() == Neuro::ChannelInfo::Type::ElectrodesState) {
			ElectrodeStateChannel = std::make_unique<Neuro::ElectrodeStateChannel>(device_ptr);
			ElectrodeStateLengthListener = ElectrodeStateChannel->subscribeLengthChanged([](auto length) {
				redrawResist();
			});
		}
	}
	if (hasResistance) {
		device_ptr->execute(Neuro::Command::StartResist);
	}
}

template <typename T>
void connectDevice(T&& device_ptr){
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

template <typename T>
void onDeviceFound(T&& device_ptr){
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
	else{
		FoundDevices.push_back(sharedDevice);
		startResistMeasure(sharedDevice);
	}
}

int main(int argc, char *argv[]){
    auto scanner = Neuro::createDeviceScanner();
    scanner->subscribeDeviceFound([&](auto&& device_ptr){
        onDeviceFound(std::forward<decltype(device_ptr)>(device_ptr));
    });
    scanner->startScan(0);//zero timeout for infinity
    while (std::cin.get() != '\n');
}
