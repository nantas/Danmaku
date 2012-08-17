//
//  GameCenterEngine.m
//  SinaGameCenter
//
//  Created by Brian.Chen on 11-5-23.
//  Copyright 2011Âπ?__MyCompanyName__. All rights reserved.
//

#import "WBGameCenterManager.h"
#import "WBSDKBase.h"
#import "WBGameEngine.h"
#import "WBOauthCore.h"
#import "WBGameApiManager.h"
#import "ASIHTTPRequest.h"

static WBGameCenterManager *engine;



@implementation WBGameCenterManager

@synthesize currentUserId,parentViewController;
@synthesize delegate = _delegate;
@synthesize appkey = _appkey;
@synthesize secret = _secret;
@synthesize refreshToken = _refreshToken;
@synthesize description = _description;
@synthesize gamename = _gamename;
@synthesize iconurl = _iconurl;
@synthesize weiboid = _weiboid;
@synthesize isLandscape = _isLandscape;
@synthesize netWork = _netWork;
@synthesize sessionKey = _sessionKey;
@synthesize expirationDate = _expirationDate;
@synthesize gameEngine;
@synthesize achievementArray;


- (void) dealloc 
{
    self.achievementArray = nil;
    parentViewController = nil;
    currentUserId = nil;
    _delegate = nil;
    [_appkey release];
    [_secret release];
    [_refreshToken release];
    [_description release];
    [_gamename release];
    [_iconurl release];
    [_weiboid release];
    
    [super dealloc];
}

/*
 *  返回游戏中心实例
 **/
+ (WBGameCenterManager *)sharedInstance
{
    if (engine == nil) {
        engine = [[WBGameCenterManager alloc] init];
        
    }
    return engine;
}



/*
 *  读取 plist 信息
 **/
- (void)getPlistInfo{
    NSString *plistPath = [[NSBundle mainBundle] pathForResource:@"WBPropertyList" ofType:@"plist"];
    NSDictionary *dictionary = [[NSDictionary alloc] initWithContentsOfFile:plistPath];
    
    //init appkey , secret
    self.appkey = [dictionary objectForKey:@"appkey"];
    self.secret = [dictionary objectForKey:@"appsecret"];
    
    //init game name,game description,game icon url and game weibo ID
    self.gamename = [dictionary objectForKey:@"gamename"];
    self.iconurl = [dictionary objectForKey:@"iconurl"];
    
    //init achievement
    self.achievementArray = [NSMutableArray array];
    self.achievementArray = [dictionary objectForKey:@"achievement"];
    
}

- (void)startAuthorize{
    [self getPlistInfo];
    
    [self.gameEngine release];
    self.gameEngine = nil;
    self.gameEngine = [[WBGameEngine alloc] initWithAppKeyAndSecret:self.appkey AppSecret:self.secret];
    self.gameEngine.engineDelegate = self;

}

/*
 *  登录
 **/
- (BOOL)isSignning{
    self.sessionKey = [[NSUserDefaults standardUserDefaults] objectForKey:SESSION_KEY];
    self.expirationDate = [[NSUserDefaults standardUserDefaults] objectForKey:EXPIRATION_DATE];
    self.currentUserId = [[NSUserDefaults standardUserDefaults] objectForKey:CURRUNT_ID];
    
    return (self.sessionKey != nil && self.expirationDate != nil);
}


/*
 *  登出
 **/
- (void)signOut{
    NSHTTPCookie *cookie;
    NSHTTPCookieStorage *storage = [NSHTTPCookieStorage sharedHTTPCookieStorage];
    for (cookie in [storage cookies]) {
        [storage deleteCookie:cookie];
    }
    
    //[self.kWBAchievementController destroy];
    NSUserDefaults *defaults=[NSUserDefaults standardUserDefaults];
    [defaults removeObjectForKey:SESSION_KEY];
    [defaults removeObjectForKey:EXPIRATION_DATE];
    [defaults removeObjectForKey:REFRESH_TOKEN];
    [defaults removeObjectForKey:CURRUNT_ID];
    
    self.sessionKey = nil;
    self.expirationDate = nil;
    self.currentUserId = nil;
}



#pragma -
#pragma mark - - - - --  API Info  - - - -- - - -

- (void)showStartAuthorize:(UIViewController *)_parentView{
    [self.gameEngine startAuthorize];
}


#pragma -
#pragma mark - -- - - 用户个人信息接口 - - - -- - 

-(void)userInfo:(NSString *)_uid{
    [[WBGameApiManager sharedInstance] userInfo:_uid];
    [WBGameApiManager sharedInstance].delegate = self;
}

- (void)userFriends:(NSString *)page andCount:(NSString *) count andTrim:(NSString *)trim{
    [[WBGameApiManager sharedInstance] userFriends:page andCount:count andTrim:trim];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

- (void)userFriendIds:(NSString *)page andCount:(NSString *)count{
    [[WBGameApiManager sharedInstance] userFriendIds:page andCount:count];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

- (void)userAppFriends:(NSString *)trim{
    [[WBGameApiManager sharedInstance] userAppFriends:trim];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

- (void)userAppFriendIDS{
    [[WBGameApiManager sharedInstance] userAppFriendIDS];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

#pragma -
#pragma mark - - - - -  成就信息 - - - - - - 
/*
 *  成就信息
 **/
- (void)showAchievementAPI{
    [[WBGameApiManager sharedInstance] showAchievementAPI];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

/*
 *  获得成就
 **/
- (void)postAchievement:(NSString *)_achievementID{
    [[WBGameApiManager sharedInstance] postAchievement:_achievementID];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}


#pragma -
#pragma mark - - - - - -  排行信息 - - - - - - - -

/*
 *  上传排行
 **/
- (void)postUserScore:(NSInteger)_rankID andScore:(NSInteger)_score{
    [[WBGameApiManager sharedInstance] postUserScore:_rankID andScore:_score];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

/*
 *  获取好友排行榜
 **/
- (void)leaderboardsGetFriends:(NSString *)rankID{
    [[WBGameApiManager sharedInstance] leaderboardsGetFriends:rankID];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

/*
 *  排行技术累加
 **/
- (void)leaderboardsIncrement:(NSString *)rankID SCORE:(NSString *)score{
    [[WBGameApiManager sharedInstance] leaderboardsIncrement:rankID SCORE:score];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

/*
 *  获取总排行
 **/
- (void)leaderboardsGetTotal:(NSString *)rankID{
    [[WBGameApiManager sharedInstance] leaderboardsGetTotal:rankID];
    [[WBGameApiManager sharedInstance] setDelegate:self];
}

/*
 *  发Feed 
 **/

- (void)uploadStatuses:(NSString *)aStatus
               PicData:(NSData *)aPic 
                SECRET:(NSString *)aSecret{
    
    [[WBGameApiManager sharedInstance] uploadStatuses:aStatus
                                              PicData:aPic
                                               SECRET:aSecret];
    
    [[WBGameApiManager sharedInstance] setDelegate:self];
}




#pragma -
#pragma mark - - - - Auth Login delegate - - - - -- - 

- (void)didLoginSuccessed{
    if ([self isSignning]) {
        if ([self.delegate respondsToSelector:@selector(authLoginSuccessed)]) {
            [self.delegate authLoginSuccessed];
        }
    }
}

- (void)didNotLogin:(NSString *)_error{
    [[WBGameCenterManager sharedInstance] signOut];
    
    if ([self.delegate respondsToSelector:@selector(authLoginFailed)]) {
        [self.delegate authLoginFailed];
    }
}

- (void)loginOut{
    
}

#pragma -
#pragma mark - - - - --  获取 API JSon 信息 - -- - - - - -

//获取 API 信息完成
- (void)requestFinished:(ASIHTTPRequest *)request{
    [self.delegate requestFinished:request];
}

// 获取API 失败
- (void)requestFailed:(ASIHTTPRequest *)request{
    [self.delegate requestFailed:request];
}




@end
