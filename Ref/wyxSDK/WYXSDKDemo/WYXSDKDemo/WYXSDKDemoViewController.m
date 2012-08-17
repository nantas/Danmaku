//
//  WYXSDKDemoViewController.m
//  WYXSDKDemo
//
//  Created by kim zhutie on 民国100-12-16.
//  Copyright 100 __MyCompanyName__. All rights reserved.
//

#import "WYXSDKDemoViewController.h"

@implementation WYXSDKDemoViewController

#pragma mark - View lifecycle

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad
{
    [super viewDidLoad];
    
    if ([[WBGameCenterManager sharedInstance] isSignning]) {
        _loginLabel.text = @"登录成功！";
    }else{
        _loginLabel.text = @"请登录！";
    }
    
    /*
     * 初始化
     **/
    [[WBGameCenterManager sharedInstance] startAuthorize];
    [WBGameCenterManager sharedInstance].delegate = self;
    [WBGameCenterManager sharedInstance].isLandscape = YES;
    
    
}

/*
 *  登录
 **/
- (IBAction)showStartAuthorize{
    
    [[WBGameCenterManager sharedInstance] showStartAuthorize:nil]; 
}


#pragma -
#pragma mark - -- - - 用户个人信息接口 - - - -- - 

/*
 * 获取某用户的个人信息
 **/
-(IBAction)userInfo:(NSString *)_uid{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    
    [[WBGameCenterManager sharedInstance] userInfo:@"1824809523"];
}

/*
 * 获取当前用户的互粉好友信息
 **/
- (IBAction)userFriends:(NSString *)page andCount:(NSString *) count andTrim:(NSString *)trim{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] userFriends:page andCount:count andTrim:trim];
}

/*
 * 获取当前用户的互粉好友 ID
 **/
- (IBAction)userFriendIds:(NSString *)page andCount:(NSString *)count{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] userFriendIds:page andCount:count];
}

/*
 * 获取当前用户安装了当前应用的互粉好友信息
 **/
- (IBAction)userAppFriends:(NSString *)trim{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] userAppFriends:trim];
}

/*
 * 获取当前用户安装了当前应用的互粉好友ID（返回所有结果 不分页）
 **/
- (IBAction)userAppFriendIDS{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] userAppFriendIDS];
}

#pragma -
#pragma mark - - - - -  成就信息 - - - - - - 
/*
 *  成就信息
 **/
- (IBAction)showAchievementAPI{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] showAchievementAPI];
}

/*
 *  获得成就
 **/
- (IBAction)postAchievement:(NSString *)_achievementID{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    if ([WBGameCenterManager sharedInstance].isSignning) {
        [[WBGameCenterManager sharedInstance] postAchievement:@"1"];
    }
}


#pragma -
#pragma mark - - - - - -  排行信息 - - - - - - - -

/*
 *  上传排行
 **/
- (IBAction)postUserScore:(NSInteger)_rankID andScore:(NSInteger)_score{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    if ([WBGameCenterManager sharedInstance].isSignning) {
        [[WBGameCenterManager sharedInstance] postUserScore:1 andScore:80000];
    }
}

/*
 *  获取好友排行榜
 **/
- (IBAction)leaderboardsGetFriends:(NSString *)rankID{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] leaderboardsGetFriends:@"1"];
}

/*
 *  排行技术累加
 **/
- (IBAction)leaderboardsIncrement:(NSString *)rankID SCORE:(NSString *)score{
    if ([WBGameCenterManager sharedInstance].isSignning) {
        [[WBGameCenterManager sharedInstance] leaderboardsIncrement:@"1" SCORE:@"123"];
    }
}

/*
 *  获取总排行
 **/
- (IBAction)leaderboardsGetTotal:(NSString *)rankID{
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    [[WBGameCenterManager sharedInstance] leaderboardsGetTotal:@"1"];
}


#pragma -
#pragma mark - - - - - -  发 Feed  - - - - - - - -

/*
 *  发feed
 **/

- (IBAction)uploadStatuses:(NSString *)aStatus
                   IconUrl:(NSString *)aPic 
                    SECRET:(NSString *)aSecret{
    /*
     * Secret  指的是 是否需要转发到我的微博。1：不转发，0：转发（默认）
     **/
    if (![[WBGameCenterManager sharedInstance] isSignning]) {
        return;
    }
    
    UIImage *image = [UIImage imageNamed:@"bg.jpg"];
    NSData *imagedata=UIImageJPEGRepresentation(image, 1.0);
    
    [[WBGameCenterManager sharedInstance] uploadStatuses:@"这个是微博内容 ! ！！！！"
                                                 PicData:imagedata
                                                  SECRET:@"0"];
    
}


/*
 *  退出登录
 **/
- (IBAction)loginOut{
    _loginLabel.text = @"请登录";
    [[WBGameCenterManager sharedInstance] signOut];
}

- (void)dealloc
{
    [super dealloc];
}




#pragma -
#pragma mark - - - -  GameCenterManagerDelegate － － － － － －

- (void)authLoginSuccessed{
    _loginLabel.text = @"登录成功 ！";
    if ([[WBGameCenterManager sharedInstance] isSignning]) {
        _sesstionKey = [WBGameCenterManager sharedInstance].sessionKey;
        _expirationDate = [WBGameCenterManager sharedInstance].expirationDate;
        _refreshToken = [WBGameCenterManager sharedInstance].refreshToken;
        _currentUserId = [WBGameCenterManager sharedInstance].currentUserId;
    } 
}

- (void)authLoginFailed{
    _loginLabel.text = @"请登录";
}

- (void)requestFinished:(ASIHTTPRequest *)request{
    NSLog(@"%@",[request responseString]);
}

- (void)requestFailed:(ASIHTTPRequest *)request{
    NSLog(@"%@",[request responseString]);
}


///////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////

- (BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation{
    return toInterfaceOrientation = YES;
}


@end
