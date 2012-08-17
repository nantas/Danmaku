//
//  GameApiManager.m
//  WYXSDKDemo
//
//  Created by Kimzhucher on 11-9-5.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import "WBGameApiManager.h"
#import "WBOauthCore.h"
#import "WBSDKBase.h"
#import "ASIHTTPRequest.h"
#import "WBGameCenterManager.h"
#import "Reachability.h"
#import "ASIFormDataRequest.h"

@implementation WBGameApiManager

@synthesize delegate;


static WBGameApiManager* gameApiManagerSingerton;

/*
 读取单例
  */
+(WBGameApiManager *)sharedInstance
{
    if(gameApiManagerSingerton==nil)
    {
        gameApiManagerSingerton=[[WBGameApiManager alloc] init];
    }
    return gameApiManagerSingerton;
}

#pragma -
#pragma mark - - - - - 用户信息接口 - -- - - - -

/*
  获取用户的个人信息
 */
-(void)userInfo:(NSString *)_uid;
{
    
    NSString *source = [WBGameCenterManager sharedInstance].appkey;
    NSString *session = [WBGameCenterManager sharedInstance].sessionKey;
    NSMutableString *urlStr = [NSMutableString stringWithFormat:@"%@?session_key=%@&source=%@&uid=%@",
                               kBaseURLShow,session,source,_uid];
    NSLog(@"%@",urlStr);
    NSURL*  url = [NSURL URLWithString:urlStr];
    ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL:url];
    [request setDelegate:self];
    [request startAsynchronous];
}

//获取当前用户的互粉好友信息
-(void) userFriends:(NSString *)page andCount:(NSString *) count andTrim:(NSString *)trim
{
    NSURL *url=[NSURL URLWithString:kBaseURLFriends];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url];
    [request setDelegate:self];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [request setPostValue:page forKey:PAGE];
    [request setPostValue:count forKey:COUNT];
    [request setPostValue:trim forKey:TRIM];
    [request setRequestMethod:GET];
    [request startAsynchronous];
    
    
}

// 获取互粉好友 IDS
-(void) userFriendIds:(NSString *)page andCount:(NSString *)count{
    
    NSURL *url=[NSURL URLWithString:kBaseURLFriend_ids];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url];
    [request setDelegate:self];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:page forKey:PAGE];
    [request setPostValue:count forKey:COUNT];
    [request setRequestMethod:GET];
    [request startAsynchronous];
    
}

- (void)userAppFriends:(NSString *)trim{
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:[NSURL URLWithString:kBaseURLApp_friends]];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:trim forKey:TRIM];
    [request setRequestMethod:GET];
    request.delegate=self;
    [request startAsynchronous];
}

- (void)userAppFriendIDS{
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:[NSURL URLWithString:kBaseURLApp_friends_ids]];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setRequestMethod:GET];
    request.delegate=self;
    [request startAsynchronous];
}


#pragma -
#pragma mark - - - - -  成就信息 - - - - - -

/*
  获取成就信息
 */
- (void)showAchievementAPI{
    NSURL *url = [NSURL URLWithString:GET_ACHIEVEMENTCORE_URL];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url]; 
    [request setPostValue:@"true" forKey:@"detail"];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setRequestMethod:POST];
    [request setDelegate:self];
    [request startAsynchronous];
}

/*
 *  获得成就
 **/
- (void)postAchievement:(NSString *)_achievementID{
    NSString *timeString = [self getTimestamp];
    
    NSMutableDictionary *_params = [NSMutableDictionary dictionary];
    [_params setObject:_achievementID forKey:ACHIEVEMENT_ID];
    [_params setObject:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [_params setObject:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [_params setObject:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [_params setObject:timeString forKey:TIMESTAMP];
    
    NSString *sig = [self generateSig:_params];
    
    NSURL *url = [NSURL URLWithString:SET_ACHIEVEMENTCORE_URL];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url]; 
    [request setDelegate:self];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:sig forKey:SIGNATURE];
    [request setPostValue:timeString forKey:TIMESTAMP];
    [request setPostValue:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [request setPostValue:_achievementID forKey:ACHIEVEMENT_ID];
    [request setRequestMethod:POST];
    [request startAsynchronous];
}


#pragma -
#pragma mark  - - - - - -  排行信息  - - - - - - -

/*
 *  上传排行
 **/
- (void)postUserScore:(NSInteger)_rankID andScore:(NSInteger)_score{
    NSString *rank_Id = [NSString stringWithFormat:@"%d",_rankID];
    NSString *user_Score = [NSString stringWithFormat:@"%d",_score];
    
    NSString *timeString = [self getTimestamp];
    
    NSMutableDictionary *_params = [NSMutableDictionary dictionary];
    [_params setObject:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [_params setObject:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [_params setObject:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [_params setObject:timeString forKey:TIMESTAMP];
    [_params setObject:rank_Id forKey:HIGHSCORE_RANK_ID];
    [_params setObject:user_Score forKey:HIGHSCORE_VALUE];
    
    NSString *sig = [self generateSig1:_params];
    
    NSURL *url = [NSURL URLWithString:kBaseURLOfPostUserScore];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url]; 
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:rank_Id forKey:HIGHSCORE_RANK_ID];
    [request setPostValue:user_Score forKey:HIGHSCORE_VALUE];
    [request setPostValue:sig forKey:SIGNATURE];
    [request setPostValue:timeString forKey:TIMESTAMP];
    [request setPostValue:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [request setRequestMethod:POST];
    [request setDelegate:self];
    [request startAsynchronous];
}

- (void)leaderboardsGetFriends:(NSString *)rankID;{

    NSString *source = [WBGameCenterManager sharedInstance].appkey;
    NSString *session = [WBGameCenterManager sharedInstance].sessionKey;
    NSMutableString *urlStr = [NSMutableString stringWithFormat:@"%@?session_key=%@&source=%@&rank_id=%@",kBaseURLOfGetFriends,session,source,rankID];
    NSURL*  url = [NSURL URLWithString:urlStr];
    ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL:url];
    [request setDelegate:self];
    [request startAsynchronous];
}


- (void)leaderboardsGetTotal:(NSString *)rankID{
    
    NSString *source = [WBGameCenterManager sharedInstance].appkey;
    NSString *session = [WBGameCenterManager sharedInstance].sessionKey;
    NSMutableString *urlStr = [NSMutableString stringWithFormat:@"%@?session_key=%@&source=%@&rank_id=%@",kBaseURLOfGetTotal,session,source,rankID];
    NSURL*  url = [NSURL URLWithString:urlStr];
    ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL:url];
    [request setDelegate:self];
    [request startAsynchronous];
}

- (void)leaderboardsIncrement:(NSString *)rankID SCORE:(NSString *)score{

    NSString *timeString = [self getTimestamp];
    
    NSMutableDictionary *_params = [NSMutableDictionary dictionary];
    [_params setObject:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [_params setObject:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [_params setObject:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [_params setObject:timeString forKey:TIMESTAMP];
    [_params setObject:rankID forKey:HIGHSCORE_RANK_ID];
    [_params setObject:score forKey:HIGHSCORE_VALUE];
    
    NSString *sig = [self generateSig1:_params];
    NSURL *url = [NSURL URLWithString:kBaseURLOfIncrement];
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:url]; 
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:rankID forKey:HIGHSCORE_RANK_ID];
    [request setPostValue:score forKey:HIGHSCORE_VALUE];
    [request setPostValue:sig forKey:SIGNATURE];
    [request setPostValue:timeString forKey:TIMESTAMP];
    [request setPostValue:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [request setRequestMethod:POST];
    [request setDelegate:self];
    [request startAsynchronous];
}


/*
 *  发Feed 
 **/
- (void)uploadStatuses:(NSString *)_status
               PicData:(NSData *)_picData 
                SECRET:(NSString *)_secrets{
    
    
    NSString *timeString = [self getTimestamp];
    
    NSMutableDictionary *_params = [NSMutableDictionary dictionary];
    
    [_params setObject:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [_params setObject:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [_params setObject:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [_params setObject:_status forKey:STATUS];
    [_params setObject:[WBGameCenterManager sharedInstance].currentUserId forKey:UIDD];
    [_params setObject:_secrets forKey:SSECRET];
    [_params setObject:timeString forKey:TIMESTAMP];
    
    NSString *sig = [self generateSig2:_params];
    
    ASIFormDataRequest *request=[ASIFormDataRequest requestWithURL:[NSURL URLWithString:kBaseURLStatusUpload]]; 
    
    [request setPostValue:PLATFORM_NUM_ID forKey:PLATFORM_ID];
    [request setPostValue:[WBGameCenterManager sharedInstance].appkey forKey:SOURCE];
    [request setPostValue:[WBGameCenterManager sharedInstance].sessionKey forKey:SESSION_KEY];
    [request setPostValue:_status forKey:STATUS];
    [request setData:_picData forKey:PIC];
    [request setPostValue:[WBGameCenterManager sharedInstance].currentUserId forKey:UIDD];
    [request setPostValue:_secrets forKey:SSECRET];
    [request setPostValue:timeString forKey:TIMESTAMP];
    [request setPostValue:sig forKey:SIGNATURE];
    [request setTimeOutSeconds:100];
    [request setRequestMethod:POST];
    [request setDelegate:self];
    [request startAsynchronous];
}

- (NSString*)getTimestamp{
    NSDate* dat = [NSDate dateWithTimeIntervalSinceNow:0];
    NSTimeInterval a=[dat timeIntervalSince1970]*1000;
    NSString *timeString = [NSString  stringWithFormat:@"%f", a] ;
    return timeString;
}

#pragma  生成签名

- (NSString*)generateSig:(NSMutableDictionary*)_params {
	NSMutableString *_joined = [NSMutableString string]; 
	NSArray* keys = [_params.allKeys sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
	for (id key in [keys objectEnumerator]) {
		id value = [_params valueForKey:key];
		if ([value isKindOfClass:[NSString class]]) {
			[_joined appendString:key];
			[_joined appendString:@"="];
			[_joined appendString:value];
            if (![key isEqualToString:TIMESTAMP]) {
                [_joined appendString:@"&"];
            }
		}
	}
	[_joined appendString:[WBGameCenterManager sharedInstance].secret];
	return [WBGameApiManager digest:_joined];
}

- (NSString*)generateSig1:(NSMutableDictionary*)_params {
	NSMutableString *_joined = [NSMutableString string]; 
	NSArray* keys = [_params.allKeys sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
    
	for (id key in [keys objectEnumerator]) {
		id value = [_params valueForKey:key];
		if ([value isKindOfClass:[NSString class]]) {
			[_joined appendString:key];
			[_joined appendString:@"="];
			[_joined appendString:value];
            if (![key isEqualToString:HIGHSCORE_VALUE]) {
                [_joined appendString:@"&"];
            }
		}
	}
    
	[_joined appendString:[WBGameCenterManager sharedInstance].secret];
    
	return [WBGameApiManager digest:_joined];
}

- (NSString*)generateSig2:(NSMutableDictionary*)_params {
	NSMutableString *_joined = [NSMutableString string]; 
	NSArray* keys = [_params.allKeys sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
    
	for (id key in [keys objectEnumerator]) {
		id value = [_params valueForKey:key];
		if ([value isKindOfClass:[NSString class]]) {
			[_joined appendString:key];
			[_joined appendString:@"="];
            value= (NSString *)CFURLCreateStringByAddingPercentEscapes(    NULL,    (CFStringRef)value,    NULL,    (CFStringRef)@"!*'();:@&=+$,/?%#[]",    kCFStringEncodingUTF8 );
			[_joined appendString:value];
            if (![key isEqualToString:UIDD]) {
                [_joined appendString:@"&"];
            }
		}
	}
	[_joined appendString:[WBGameCenterManager sharedInstance].secret];
	return [WBGameApiManager digest:_joined];
}


+(NSString*) digest:(NSString*)input
{
    const char *cstr = [input cStringUsingEncoding:NSUTF8StringEncoding];
    NSData *data = [NSData dataWithBytes:cstr length:input.length];
    
    uint8_t digest[CC_SHA1_DIGEST_LENGTH];
    
    CC_SHA1(data.bytes, data.length, digest);
    
    NSMutableString* output = [NSMutableString stringWithCapacity:CC_SHA1_DIGEST_LENGTH * 2];
    
    for(int i = 0; i < CC_SHA1_DIGEST_LENGTH; i++)
        [output appendFormat:@"%02x", digest[i]];
    
    return output;
}

#pragma mark -
#pragma mark ------------   ASIHTTPRequestDelegate    -------------

- (void)requestFinished:(ASIHTTPRequest *)request
{
    [self.delegate requestFinished:request];
}

- (void)requestFailed:(ASIHTTPRequest *)request
{
    [self.delegate requestFailed:request];
}

@end


