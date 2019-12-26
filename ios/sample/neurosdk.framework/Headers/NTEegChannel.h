//
//  NTEegChannel.h
//  bluetoothle
//
//  Created by admin on 23.12.2019.
//

#import "NTBaseChannel.h"

NS_ASSUME_NONNULL_BEGIN

@interface NTEegChannel : NTBaseChannel

/// Read array of Double from eeg channel
/// @param offset Offset from first received value
/// @param length Size of chunk that you will read
- (NSArray<NSNumber *> *)readDataWithOffset:(NSInteger)offset length:(NSInteger) length  NS_SWIFT_NAME(readData(offset:length:));
@end

NS_ASSUME_NONNULL_END
