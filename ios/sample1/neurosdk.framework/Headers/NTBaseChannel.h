//
//  NTBaseChannel.h
//  bluetoothle
//
//  Created by admin on 23.12.2019.
//

#import <Foundation/Foundation.h>

#import "NTDevice.h"
#import "NTDeviceInfo.h"

NS_ASSUME_NONNULL_BEGIN

@interface NTBaseChannel : NSObject

- (nonnull instancetype)initWithDevice:(NTDevice *) device channelInfo: (NTChannelInfo *__nullable) channelInfo NS_DESIGNATED_INITIALIZER;


- (void)subscribeLengthChangedWithSubscribe:(void (^ _Nonnull)(NSInteger))subscribe;

@property (NS_NONATOMIC_IOSONLY, readonly) NSInteger totalLength;
@property (NS_NONATOMIC_IOSONLY, readonly) float samplingFrequency;

@property (NS_NONATOMIC_IOSONLY, readonly, strong) NTChannelInfo * _Nullable info;
@end

NS_ASSUME_NONNULL_END
