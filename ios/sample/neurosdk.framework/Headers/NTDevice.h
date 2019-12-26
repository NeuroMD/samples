//
//  NTDevice.h
//  bluetoothle
//
//  Created by admin on 23.12.2019.
//

#import <Foundation/Foundation.h>

#import "NTDeviceEnumerator.h"

NS_ASSUME_NONNULL_BEGIN

@interface NTDevice : NSObject

- (nonnull instancetype)initWithEnumerator:(NTDeviceEnumerator *_Nonnull) enumerator deviceInfo:(NTDeviceInfo *_Nonnull)deviceInfo;

- (void)subscribeParameterChangedWithSubscriber:(void (^_Nonnull)(enum NTParameter))subscriber;

- (void)connect;
- (void)disconnect;

- (void)executeWithCommand:(enum NTCommand) command NS_SWIFT_NAME(execute(command:));

@property (NS_NONATOMIC_IOSONLY, readonly, copy) NSArray<NTChannelInfo *> *_Nonnull channels;
@property (NS_NONATOMIC_IOSONLY, readonly, copy) NSArray<NSNumber *> *_Nonnull commands;
@property (NS_NONATOMIC_IOSONLY, readonly, copy) NSArray<NTParameterInfo *> *_Nonnull parameters;
@end
NS_ASSUME_NONNULL_END
