//
//  GameApiManager.h
//  WYXSDKDemo
//
//  Created by Kimzhucher on 11-9-5.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ASIHTTPRequest.h"
@protocol WBGameApiDelegate <NSObject>

- (void)requestFinished:(ASIHTTPRequest *)request;
- (void)requestFailed:(ASIHTTPRequest *)request;

@end

@interface WBGameApiManager : NSObject {
    // 代理
    id<WBGameApiDelegate> delegate;
    
}

@property (nonatomic,assign) id<WBGameApiDelegate> delegate;

+(WBGameApiManager *)sharedInstance;




#pragma -
#pragma mark - -- - - 用户个人信息接口 - - - -- - 

/*
 *  获取某用户的个人信息
 **/
- (void)userInfo:(NSString *)_uid;

/*
 *  获取当前用户的互粉好友信息
 **/
- (void)userFriends:(NSString *)page andCount:(NSString *) count andTrim:(NSString *)trim;

/*
 *  获取当前用户的互粉好友ID
 **/
- (void)userFriendIds:(NSString *)page andCount:(NSString *)count;

/*
 *  获取当前用户安装了当前应用的互粉好友信息
 **/
- (void)userAppFriends:(NSString *)trim;

/*
 *  获取当前用户安装了当前应用的互粉好友ID  返回所有结果(不分页)
 **/
- (void)userAppFriendIDS;


#pragma -
#pragma mark - - - - -  成就信息 - - - - - - 
/*
 *  成就信息
 **/
- (void)showAchievementAPI;

/*
 *  获得成就
 **/
- (void)postAchievement:(NSString *)_achievementID;


#pragma -
#pragma mark - - - - - -  排行信息 - - - - - - - -

/*
 *  上传排行
 **/
- (void)postUserScore:(NSInteger)_rankID andScore:(NSInteger)_score;

/*
 *  获取好友排行榜
 **/
- (void)leaderboardsGetFriends:(NSString *)rankID;

/*
 *  排行技术累加
 **/
- (void)leaderboardsIncrement:(NSString *)rankID SCORE:(NSString *)score;

/*
 *  获取总排行
 **/
- (void)leaderboardsGetTotal:(NSString *)rankID;


/*
 *  发Feed 
 **/

- (void)uploadStatuses:(NSString *)_status
               PicData:(NSData *)_picData 
                SECRET:(NSString *)_secrets;


- (NSString*)getTimestamp;
- (NSString*)generateSig:(NSMutableDictionary*)_params;
- (NSString*)generateSig1:(NSMutableDictionary*)_params;
- (NSString*)generateSig2:(NSMutableDictionary*)_params;
+ (NSString*) digest:(NSString*)input;

@end
