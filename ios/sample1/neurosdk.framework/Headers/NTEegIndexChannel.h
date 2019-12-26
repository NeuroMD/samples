//
//  NTEegIndexChannel.h
//  neurosdk
//
//  Created by admin on 24.12.2019.
//

#import "NTBaseChannel.h"

NS_ASSUME_NONNULL_BEGIN

@interface NTEegIndexValues : NSObject
@property (nonatomic, readonly) double AlphaRate;
@property (nonatomic, readonly) double BetaRate;
@property (nonatomic, readonly) double DeltaRate;
@property (nonatomic, readonly) double ThetaRate;
@end

typedef NS_ENUM (NSUInteger, NTEegIndexMode) {
    NTEegIndexModeLeftSide,
    NTEegIndexModeRightSide,
    NTEegIndexModeArtifacts
};

@interface NTEegIndexChannel : NTBaseChannel

- (nonnull instancetype)initWithT3:(NTBaseChannel *)t3 t4:(NTBaseChannel *)t3 o1:(NTBaseChannel *)o1 o2:(NTBaseChannel *)o2;

/// Read array of Double from eeg index channel

/// @param offset Offset from first received value
/// @param length Size of chunk that you will read
- (NSArray<NTEegIndexValues *> *)readDataWithOffset:(NSInteger)offset length:(NSInteger) length  NS_SWIFT_NAME(readData(offset:length:));

- (void)setDelayWithSeconds:(double)delay_seconds;
- (void)setWeightCoefficientsWithAlpha:(double)alpha beta:(double)beta delta:(double)delta theta:(double)theta;

@property (NS_NONATOMIC_IOSONLY, getter = getMode, readonly) NTEegIndexMode mode;
@property (NS_NONATOMIC_IOSONLY, getter = getBasePower, readonly) double basePower;

@end

NS_ASSUME_NONNULL_END
