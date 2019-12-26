//
//  NTDevice+Extension.h
//  neurosdk
//
//  Created by admin on 26.12.2019.
//  Copyright © 2019 NeuroMD. All rights reserved.
//

#import "NTDevice.h"

typedef NS_ENUM (NSUInteger, NTState) {
    NTStateDisconnected,
    NTStateConnected
};

typedef NS_ENUM (NSUInteger, NTFirmwareMode) {
    NTFirmwareModeApplication,
    NTFirmwareModeBootloader
};

typedef NS_ENUM (NSUInteger, NTSamplingFrequency) {
    NTSamplingFrequencyHz125,
    NTSamplingFrequencyHz250,
    NTSamplingFrequencyHz500,
    NTSamplingFrequencyHz1000,
    NTSamplingFrequencyHz2000,
    NTSamplingFrequencyHz4000,
    NTSamplingFrequencyHz8000
};

typedef NS_ENUM (NSUInteger, NTGain) {
    NTGainG1,
    NTGainG2,
    NTGainG3,
    NTGainG4,
    NTGainG6,
    NTGainG8,
    NTGainG12
};

typedef NS_ENUM (NSUInteger, NTExternalSwitchInput) {
    NTExternalSwitchInputMioElectrodesRespUSB,
    NTExternalSwitchInputMioElectrodes,
    NTExternalSwitchInputMioUSB,
    NTExternalSwitchInputRespUSB
};

typedef NS_ENUM (NSUInteger, NTADCInput) {
    NTADCInputElectrodes,
    NTADCInputShort,
    NTADCInputTest,
    NTADCInputResistance
};

typedef NS_ENUM (NSUInteger, NTAccelerometerSensitivity) {
    NTAccelerometerSensitivitySens2g,
    NTAccelerometerSensitivitySens4g,
    NTAccelerometerSensitivitySens8g,
    NTAccelerometerSensitivitySens16g
};

typedef NS_ENUM (NSUInteger, NTGyroscopeSensitivity) {
    NTGyroscopeSensitivitySens250Grad,
    NTGyroscopeSensitivitySens500Grad,
    NTGyroscopeSensitivitySens1000Grad,
    NTGyroscopeSensitivitySens2000Grad
};

typedef NS_ENUM (NSUInteger, NTStimulationDeviceState) {
    NTStimulationDeviceStateNoParams,
    NTStimulationDeviceStateDisabled,
    NTStimulationDeviceStateEnabled
};

typedef NS_ENUM (NSUInteger, NTMotionAssistantLimb) {
    NTMotionAssistantLimbRightLeg,
    NTMotionAssistantLimbLeftLeg,
    NTMotionAssistantLimbRightArm,
    NTMotionAssistantLimbLeftArm
};

NS_ASSUME_NONNULL_BEGIN

@interface NTStimulatorAndMaState : NSObject
@property (nonatomic, readonly) enum NTStimulationDeviceState StimulatorState;
@property (nonatomic, readonly) enum NTStimulationDeviceState MAState;

- (nonnull instancetype)initWithStimulatorState:(enum NTStimulationDeviceState) stimulatorState andMAState:(enum NTStimulationDeviceState)MAState;

@end

@interface NTStimulationParams : NSObject
@property (nonatomic, readonly) int current;
@property (nonatomic, readonly) int pulseWidth;
@property (nonatomic, readonly) int frequency;
@property (nonatomic, readonly) int stimulusDuration;

- (nonnull instancetype)initWithCurrent:(int)current pulseWidth:(int)pulseWidth frequency:(int)frequency stimulusDuration:(int)stimulusDuration;

@end

@interface NTMotionAssistantParams : NSObject
@property (nonatomic, readonly) int gyroStart;
@property (nonatomic, readonly) int gyroStop;
@property (nonatomic, readonly) enum NTMotionAssistantLimb limb;
@property (nonatomic, readonly) int minPause;

- (nonnull instancetype)initWithGyroStart:(int)gyroStart gyroStop:(int)gyroStop limb:(enum NTMotionAssistantLimb) limb minPause:(int)minPause;
@end

@interface NTFirmwareVersion : NSObject
@property (nonatomic, readonly) unsigned int version;
@property (nonatomic, readonly) unsigned int build;

- (nonnull instancetype)initWithVersion:(int)version build:(int)build;
@end

@interface NTDevice (Extension)

- (NSString *)                 readName                 NS_SWIFT_NAME(name());
- (enum NTState)               readState                NS_SWIFT_NAME(state());
- (NSString *)                 readAddress              NS_SWIFT_NAME(address());
- (NSString *)                 readSerialNumber         NS_SWIFT_NAME(serialNumber());
- (BOOL)                       readHardwareFilterState  NS_SWIFT_NAME(hardwareFilterState());
- (NTFirmwareMode)             readFirmwareMode         NS_SWIFT_NAME(firmwareMode());
- (NTSamplingFrequency)        readSamplingFrequency    NS_SWIFT_NAME(samplingFrequency());
- (NTGain)                     readGain                 NS_SWIFT_NAME(gain());
- (unsigned char)              readOffset               NS_SWIFT_NAME(offset());
- (NTExternalSwitchInput)      readExternalSwitchState  NS_SWIFT_NAME(externalSwitchState());
- (NTADCInput)                 readADCInputState        NS_SWIFT_NAME(ADCInputState());
- (NTAccelerometerSensitivity) readAccelerometerSens    NS_SWIFT_NAME(accelerometerSens());
- (NTGyroscopeSensitivity)     readGyroscopeSens        NS_SWIFT_NAME(gyroscopeSens());
- (NTStimulatorAndMaState *)   readStimulatorAndMAState NS_SWIFT_NAME(stimulatorAndMAState());
- (NTStimulationParams *)      readStimulatorParamPack  NS_SWIFT_NAME(stimulatorParamPack());
- (NTMotionAssistantParams *)  readMotionAssistantParamPack NS_SWIFT_NAME(motionAssistantParamPack());
- (NTFirmwareVersion *)        readFirmwareVersion NS_SWIFT_NAME(firmwareVersion());


- (void) setName: (NSString *) name;
- (void) setState: (enum NTState) state;
- (void) setAddress: (NSString *) address;
- (void) setSerialNumber: (NSString *) serialNumber;
- (void) setHardwareFilterState: (BOOL) hardwareFilterState;
- (void) setFirmwareMode: (enum NTFirmwareMode) firmwareMode;
- (void) setSamplingFrequency: (enum NTSamplingFrequency) samplingFrequency;
- (void) setGain: (enum NTGain) gain;
- (void) setOffset: (unsigned char) offset;
- (void) setExternalSwitchInput: (enum NTExternalSwitchInput) externalSwitchInput;
- (void) setADCInputState: (enum NTADCInput) ADCInput;
- (void) setAccelerometerSens: (enum NTAccelerometerSensitivity) accelerometerSensitivity;
- (void) setGyroscopeSens: (enum NTGyroscopeSensitivity) gyroscopeSensitivity;
- (void) setStimulatorParamPack: (NTStimulationParams *) stimulationParams;
- (void) setMotionAssistantParamPack: (NTMotionAssistantParams *) motionAssistantParams;
@end

NS_ASSUME_NONNULL_END
