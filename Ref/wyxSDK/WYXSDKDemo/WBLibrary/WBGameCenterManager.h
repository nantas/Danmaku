//
//  GameCenterEngine.h
//  SinaGameCenter
//
//  Created by Brian.Chen on 11-5-23.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "WBGameApiManager.h"
#import "WBGameCenterDelegate.h"
#import "WBGameEngine.h"


#define WB_ISLANDSCAPE  [WBGameCenterManager sharedInstance].isLandscape

//for initWithDelegate screen orientation
typedef enum
{
    orPortrait = 1,
    orLandscape = 2
    
}wbOrientation;

typedef enum {
    WBInterfaceOrientationPortrait           = 1 << UIDeviceOrientationPortrait - 1,
    WBInterfaceOrientationPortraitUpsideDown = 1 << UIDeviceOrientationPortraitUpsideDown - 1,
    WBInterfaceOrientationLandscapeLeft      = 1 << UIDeviceOrientationLandscapeLeft - 1,
    WBInterfaceOrientationLandscapeRight     = 1 << UIDeviceOrientationLandscapeRight - 1
} WBInterfaceOrientation;



@class WBNotification;
@class WBAchievementManager;

@interface WBGameCenterManager : NSObject <WBEngineDelegate,WBGameApiDelegate>{  
    @private

    NSString *currentUserId;
    id<WBGameCenterDelegate> _delegate;
    UIViewController *parentViewController;    
    
    NSString *_appkey;
    NSString *_secret;
    NSString *_description;
    NSString *_gamename;
    NSString *_iconurl;
    NSString *_weiboid;
    NSString *_sessionKey;
    
    NSMutableArray *achievementArray;
    
    BOOL _isLandscape;
    BOOL _netWork;    
}

@property (nonatomic ,retain) WBGameEngine *gameEngine;
@property (assign,nonatomic)  id<WBGameCenterDelegate> delegate;
@property (assign,nonatomic)  UIViewController *parentViewController;

@property (nonatomic, retain) NSString *currentUserId;
@property (nonatomic, retain) NSString *appkey;
@property (nonatomic, retain) NSString *secret;
@property (nonatomic, retain) NSString *description;
@property (nonatomic, retain) NSString *gamename;
@property (nonatomic, retain) NSString *iconurl;
@property (nonatomic, retain) NSString *weiboid;
@property (nonatomic, retain) NSString *refreshToken;
@property (nonatomic, retain) NSString *sessionKey;
@property (nonatomic, retain) NSString *expirationDate;

@property (nonatomic, retain) NSMutableArray *achievementArray;
@property (nonatomic)         BOOL isLandscape;
@property (nonatomic)         BOOL netWork; 




/*
 *  返回游戏中心实例
 **/
+ (WBGameCenterManager *)sharedInstance;


/*
 *  读取 plist 信息
 **/
- (void)getPlistInfo;

/*
 *  登录于登出
 **/
- (BOOL)isSignning;
- (void)signOut;
- (void)startAuthorize;

#pragma -
#pragma mark - --   API Info  - - - - -

/*
 * 开始授权
 **/
- (void)showStartAuthorize:(UIViewController *)_parentView;


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

- (void)uploadStatuses:(NSString *)aStatus
               PicData:(NSData *)aPic 
                SECRET:(NSString *)aSecret;











@end
