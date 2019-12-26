//
//  NTDeviceInfo.h
//  bluetoothle
//
//  Created by admin on 23.12.2019.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM (NSUInteger, NTChannelType);

@interface NTDeviceInfo : NSObject

@property (nonatomic, readonly, copy) NSString *_Nonnull name;
@property (nonatomic, readonly, copy) NSString *_Nonnull address;
@property (nonatomic, readonly) uint64_t serialNumber;
- (nonnull instancetype)initWithName:(NSString *_Nonnull)name address:(NSString *_Nonnull)address serialNumber:(uint64_t)serialNumber;
@end

@interface NTChannelInfo : NSObject
@property (nonatomic, readonly, copy) NSString *_Nonnull name;
@property (nonatomic, readonly) enum NTChannelType type;
@property (nonatomic, readonly) NSInteger index;
@end

typedef NS_ENUM (NSUInteger, NTParamAccess);
typedef NS_ENUM (NSUInteger, NTParameter);

@interface NTParameterInfo : NSObject
@property (nonatomic, readonly) enum NTParameter parameter;
@property (nonatomic, readonly) enum NTParamAccess access;
@end

typedef NS_ENUM (NSUInteger, NTParamAccess) {
    NTParamAccessRead       = 0,
    NTParamAccessReadWrite  = 1,
    NTParamAccessReadNotify = 2,
    NTParamAccessNone       = 3,
};

typedef NS_ENUM (NSUInteger, NTParameter) {
    NTParameterName                     = 0,
    NTParameterState                    = 1,
    NTParameterAddress                  = 2,
    NTParameterSerialNumber             = 3,
    NTParameterHardwareFilterState      = 4,
    NTParameterFirmwareMode             = 5,
    NTParameterSamplingFrequency        = 6,
    NTParameterGain                     = 7,
    NTParameterOffset                   = 8,
    NTParameterExternalSwitchState      = 9,
    NTParameterADCInputState            = 10,
    NTParameterAccelerometerSens        = 11,
    NTParameterGyroscopeSens            = 12,
    NTParameterStimulatorAndMAState     = 13,
    NTParameterStimulatorParamPack      = 14,
    NTParameterMotionAssistantParamPack = 15,
    NTParameterFirmwareVersion          = 16,
    NTParameterNone                     = 17,
};

typedef NS_ENUM (NSUInteger, NTCommand) {
    NTCommandStartSignal            = 0,
    NTCommandStopSignal             = 1,
    NTCommandStartResist            = 2,
    NTCommandStopResist             = 3,
    NTCommandStartMEMS              = 4,
    NTCommandStopMEMS               = 5,
    NTCommandStartRespiration       = 6,
    NTCommandStopRespiration        = 7,
    NTCommandStartStimulation       = 8,
    NTCommandStopStimulation        = 9,
    NTCommandEnableMotionAssistant  = 10,
    NTCommandDisableMotionAssistant = 11,
    NTCommandFindMe                 = 12,
    NTCommandNone                   = 13,
};

typedef NS_ENUM (NSUInteger, NTChannelType) {
    NTChannelTypeSignal          = 0,
    NTChannelTypeBattery         = 1,
    NTChannelTypeElectrodesState = 2,
    NTChannelTypeRespiration     = 3,
    NTChannelTypeMEMS            = 4,
    NTChannelTypeOrientation     = 5,
    NTChannelTypeConnectionStats = 6,
    NTChannelTypeResistance      = 7,
    NTChannelTypePedometer       = 8,
    NTChannelTypeCustom          = 9,
    NTChannelTypeNone            = 10,
};

NS_ASSUME_NONNULL_END
