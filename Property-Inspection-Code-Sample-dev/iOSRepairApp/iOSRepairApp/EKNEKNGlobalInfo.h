//
//  EKNEKNGlobalInfo.h
//  EdKeyNote
//
//  Created by canviz on 9/24/14.
//  Copyright (c) 2014 canviz. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface EKNEKNGlobalInfo : UIViewController
+(NSDate *)converDateFromString:(NSString *)stringdate;
+(NSString *)converStringFromDate:(NSDate *)date;
+(NSString *)converStringToDateString:(NSString *)stringDate;
+(BOOL)isBlankString:(NSString *)string;
+(CGSize)getSizeFromStringWithFont:(NSString *)string font:(UIFont *)font;
+(NSString *)getString:(NSString *)string;
+(BOOL)requestSuccess:(NSURLResponse *)response;
@end
