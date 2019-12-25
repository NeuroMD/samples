//
//  NTSpectrumChannel.h
//  neurosdk
//
//  Created by admin on 24.12.2019.
//

#import "NTBaseChannel.h"

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, NTSpectrumWindow) {
  NTSpectrumWindowRectangular,
  NTSpectrumWindowSine,
  NTSpectrumWindowHamming,
  NTSpectrumWindowBlackman
};

@interface NTSpectrumChannel : NTBaseChannel

- (nonnull instancetype)initWithChannel:(NTBaseChannel *) channel windowType: (NTSpectrumWindow) windowType NS_DESIGNATED_INITIALIZER;

/// Read array of Double from spectrum channel
/// @param offset Offset from first received value
/// @param length Size of chunk that you will read
-(NSArray<NSNumber *> *) readDataWithOffset:(NSInteger)offset length:(NSInteger)length  NS_SWIFT_NAME(readData(offset:length:));

@property (NS_NONATOMIC_IOSONLY, getter=getHzPerSpectrumSample, readonly) double hzPerSpectrumSample;
@property (NS_NONATOMIC_IOSONLY, getter=getSpectrumlength, readonly) NSInteger spectrumlength;


@end

NS_ASSUME_NONNULL_END
