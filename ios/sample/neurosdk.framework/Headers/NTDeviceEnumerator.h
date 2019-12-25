//
//  NTDeviceEnumerator.h
//  bluetoothle
//
//  Created by admin on 23.12.2019.
//

#import <Foundation/Foundation.h>
#import "NTDeviceInfo.h"

typedef NS_ENUM(NSUInteger, NTDeviceType) {
    TypeBrainbit,
    TypeCallibri,
    TypeAny
};

NS_ASSUME_NONNULL_BEGIN

@interface NTDeviceEnumerator : NSObject
- (nonnull instancetype) initWithDeviceType: (enum NTDeviceType) deviceType NS_DESIGNATED_INITIALIZER;
- (void)subscribeFoundDeviceWithSubscriber:(void (^ _Nullable)(NTDeviceInfo * _Nonnull))subscriber;
@end

NS_ASSUME_NONNULL_END
