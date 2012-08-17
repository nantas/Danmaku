//
//  WYXSDKDemoViewController.h
//  WYXSDKDemo
//
//  Created by kim zhutie on 民国100-12-16.
//  Copyright 100 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "WBGameCenterManager.h"

@interface WYXSDKDemoViewController : UIViewController <WBGameCenterDelegate> {
    IBOutlet UILabel *_loginLabel;
    
    NSString *_sesstionKey;
    NSString *_refreshToken;
    NSString *_expirationDate;
    NSString *_currentUserId; 
}

//开始授权
- (IBAction)showStartAuthorize;

#pragma -
#pragma mark - -- - - 用户个人信息接口 - - - -- - 

/*
 *  获取某用户的个人信息
 **/
- (IBAction)userInfo:(NSString *)_uid;

/*
 *  获取当前用户的互粉好友信息
 **/
- (IBAction)userFriends:(NSString *)page andCount:(NSString *) count andTrim:(NSString *)trim;

/*
 *  获取当前用户的互粉好友ID
 **/
- (IBAction)userFriendIds:(NSString *)page andCount:(NSString *)count;

/*
 *  获取当前用户安装了当前应用的互粉好友信息
 **/
- (IBAction)userAppFriends:(NSString *)trim;

/*
 *  获取当前用户安装了当前应用的互粉好友ID  返回所有结果(不分页)
 **/
- (IBAction)userAppFriendIDS;


#pragma -
#pragma mark - - - - -  成就信息 - - - - - - 
/*
 *  成就信息
 **/
- (IBAction)showAchievementAPI;

/*
 *  获得成就
 **/
- (IBAction)postAchievement:(NSString *)_achievementID;




#pragma -
#pragma mark - - - - - -  排行信息 - - - - - - - -

/*
 *  上传排行
 **/
- (IBAction)postUserScore:(NSInteger)_rankID andScore:(NSInteger)_score;

/*
 *  获取好友排行榜
 **/
- (IBAction)leaderboardsGetFriends:(NSString *)rankID;

/*
 *  排行技术累加
 **/
- (IBAction)leaderboardsIncrement:(NSString *)rankID SCORE:(NSString *)score;

/*
 *  获取总排行
 **/
- (IBAction)leaderboardsGetTotal:(NSString *)rankID;

#pragma -
#pragma mark - - - - - -  发 Feed  - - - - - - - -

/*
 *  发feed
 **/
- (IBAction)uploadStatuses:(NSString *)aStatus
                   IconUrl:(NSString *)aPic 
                    SECRET:(NSString *)aSecret;

/*
 *  退出登录
 **/
- (IBAction)loginOut;

@end
