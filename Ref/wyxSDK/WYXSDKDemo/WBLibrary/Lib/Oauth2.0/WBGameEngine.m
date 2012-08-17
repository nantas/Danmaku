//
//  WBGameEngine.m
//  DemoForIPHone
//
//  Created by Brian.Chen on 11-7-27.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import "WBGameEngine.h"
#import "WBOauthCore.h"
#import "WBGameCenterManager.h"
#import <CommonCrypto/CommonHMAC.h>
#import <CommonCrypto/CommonCryptor.h>

#import "Reachability.h"
#import "ASIHTTPRequest.h"
#import "ASIFormDataRequest.h"

#import "WBSDKBase.h"


#define kFBAppAuthURLPath @"wyxauth"

@implementation WBGameEngine

@synthesize appSecret,appKey;
@synthesize accessToken,secret,sessionKey,expirationDate,curruntID;
@synthesize oauthView;
@synthesize engineDelegate;

- (void)dealloc{
    self.appSecret = nil;
    self.appKey = nil;
    self.accessToken = nil;
    self.secret = nil;
    self.curruntID = nil;
    self.sessionKey = nil;
    self.expirationDate = nil;
    self.oauthView = nil;
    [super dealloc];
}

static WBGameEngine *engineSingerton;
+ (WBGameEngine *)sharedInstance
{
    if (engineSingerton == nil) {
        engineSingerton = [[WBGameEngine alloc] init];
    }
    return engineSingerton;
}


- (id)initWithAppKeyAndSecret:(NSString *)_appKey AppSecret:(NSString *)_appSecret{
    self = [super init];
    if (self) {
        [self.appKey release];
        [self.appSecret release];
        self.appKey = [_appKey copy];
        self.appSecret = [_appSecret copy];
    }
    return self;
}


- (void)startAuthorize{
    [self authorizeWithWBGame:YES safariAuth:YES];
}

- (void)authorizeWithWBGame:(BOOL)tryWYXAppAuth safariAuth:(BOOL)trySafariAuth{
    NSMutableDictionary* params = [NSMutableDictionary dictionaryWithObjectsAndKeys:
                                   self.appKey, @"source",
                                   RESPONSE_TYPE, @"response_type",
                                   nil];
    
     
    
    BOOL didOpenOtherApp = NO;
//    UIDevice *device = [UIDevice currentDevice];
//    if ([device respondsToSelector:@selector(isMultitaskingSupported)] && [device isMultitaskingSupported]) {
//        
//        if (tryWYXAppAuth) {
//            
//            NSString *scheme = kFBAppAuthURLPath;
//            NSString *urlPrefix = [NSString stringWithFormat:@"%@://", scheme];
//            NSURL    *appURL = [[self generateURL:urlPrefix params:params] retain]; 
//            didOpenOtherApp = [[UIApplication sharedApplication] openURL:appURL];
//            [appURL release];
//        }
//        
//        if (trySafariAuth && !didOpenOtherApp) {
//
//            NSURL *loadingURL = [[self generateURL:AUTHORIZE_URL params:params] retain]; 
//            
//            didOpenOtherApp = [[UIApplication sharedApplication] openURL:loadingURL];
//            [loadingURL release];
//        }
//    }
    
    if (!didOpenOtherApp) {
        self.oauthView = [[[WBOauthView alloc] initWithURL:AUTHORIZE_URL params:params]autorelease];
        self.oauthView.oauthDelegate = self;
        [self.oauthView showRequstView];
    }
}

- (BOOL)handleOpenURL:(NSURL *)url {
    // If the URL's structure doesn't match the structure used for Facebook authorization, abort.
    if (![url absoluteString]) {
        return NO;
    }
    
    NSString *query = [url fragment];
    
    if (!query) {
        query = [url query];
    }
    
    NSString *q = [url absoluteString];
    NSString *session = [self getStringFromUrl:q needle:@"access_token="];
    NSString *expTime = [self getStringFromUrl:q needle:@"expires_in="];
    NSString *signal_code = [self getStringFromUrl:q needle:@"signal_code="];
    
    //设置sesstionKey过期时间
    NSArray *sesstionArr = [session componentsSeparatedByString:@"_"];
    NSString *createTime = [sesstionArr objectAtIndex:[sesstionArr count] - 2];
    int cTime = (int)[createTime floatValue];
    int sessionCycle = (int)[expTime floatValue];
    int expired = cTime + sessionCycle;
    NSString *expiredTime = [NSString stringWithFormat:@"%d",expired];
    
    if ([signal_code isEqualToString:@"close"]) {
        return NO;
    }
    
    if ((session == (NSString *) [NSNull null]) || (session.length == 0)) {
        if ([self.engineDelegate respondsToSelector:@selector(NotLogin:)]) {
            [self.engineDelegate didNotLogin:@"请查看网络！"];
        }
        
    } else {
        if ([self.engineDelegate respondsToSelector:@selector(didLoginSuccessed)]) {
            self.expirationDate = expiredTime;
            self.sessionKey = session;
            
            //用户信息保存到本地
            [self saveLoginInfo];
            [self.engineDelegate didLoginSuccessed];
        }
    }
    return YES;
}

- (void)getRefreshToken{
    if ([self isSignning]) {
        
        NSURL *url = [NSURL URLWithString:REFRESHTOKEN_URL];
        ASIFormDataRequest *request = [[ASIFormDataRequest alloc] initWithURL:url];
        [request setDelegate:self];
        [request setPostValue:self.appKey forKey:SOURCE];
        [request setRequestMethod:@"POST"];
        [request startAsynchronous];
    }
}

#pragma -
#pragma mark ---------   字符串拼接    -----------

- (NSString *) getStringFromUrl: (NSString*) url needle:(NSString *) needle {
    NSString * str = nil;
    NSRange start = [url rangeOfString:needle];
    if (start.location != NSNotFound) {
        NSRange end = [[url substringFromIndex:start.location+start.length] rangeOfString:@"&"];
        NSUInteger offset = start.location+start.length;
        str = end.location == NSNotFound
        ? [url substringFromIndex:offset]
        : [url substringWithRange:NSMakeRange(offset, end.location)];
        str = [str stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    }
    return str;
}

- (NSURL*)generateURL:(NSString*)baseURL params:(NSDictionary*)params {
    if (params) {
        NSMutableArray* pairs = [NSMutableArray array];
        for (NSString* key in params.keyEnumerator) {
            NSString* value = [params objectForKey:key];
            NSString* escaped_value = (NSString *)CFURLCreateStringByAddingPercentEscapes(
                                                                                          NULL, /* allocator */
                                                                                          (CFStringRef)value,
                                                                                          NULL, /* charactersToLeaveUnescaped */
                                                                                          (CFStringRef)@"!*'();:@&=+$,/?%#[]",
                                                                                          kCFStringEncodingUTF8);
            
            [pairs addObject:[NSString stringWithFormat:@"%@=%@", key, escaped_value]];
            [escaped_value release];
        }
        
        NSString* query = [pairs componentsJoinedByString:@"&"];
        NSString* url = [NSString stringWithFormat:@"%@?%@", baseURL, query];
        
        return [NSURL URLWithString:url];
    } else {
        return [NSURL URLWithString:baseURL];
    }
}
 
#pragma -
#pragma mark ----------  关于登录于退出  ------------

// 判断登录状态
- (BOOL)isSignning{
    NSUserDefaults *defaults=[NSUserDefaults standardUserDefaults];
	if (defaults) {
        self.expirationDate = [defaults objectForKey:EXPIRATION_DATE];
		self.sessionKey = [defaults objectForKey:SESSION_KEY];
	}
    return (self.sessionKey != nil && self.expirationDate != nil);
}

// 退出
- (void)signOut{
    self.expirationDate = nil;
    self.sessionKey = nil;
    self.curruntID = nil;
    // 清空 default
    [self deleteLoginInfo];
    
    if ([self.engineDelegate respondsToSelector:@selector(LoginOut)]) {
        //[self.engineDelegate LoginOut];
    }
}

//  保存用户
- (void)saveLoginInfo{
   
    NSArray *sesstionArr = [self.sessionKey componentsSeparatedByString:@"_"];
    self.curruntID = [sesstionArr lastObject];
    

    NSUserDefaults *defaults=[NSUserDefaults standardUserDefaults];	
	if (self.expirationDate) {
		[defaults setObject:self.expirationDate forKey:EXPIRATION_DATE];
	}	
    if (self.sessionKey) {
        [defaults setObject:self.sessionKey forKey:SESSION_KEY];
    }
    
    if (self.curruntID) {
        [defaults setObject:self.curruntID forKey:CURRUNT_ID];
    }
    
    [defaults synchronize];	
}

//删除用户
- (void)deleteLoginInfo{
    NSUserDefaults *defaults=[NSUserDefaults standardUserDefaults];
    [defaults removeObjectForKey:SESSION_KEY];
    [defaults removeObjectForKey:EXPIRATION_DATE];
    [defaults removeObjectForKey:REFRESH_TOKEN];
    [defaults removeObjectForKey:CURRUNT_ID];
    
    //[ASIHTTPRequest setSessionCookies:nil];
}


#pragma -
#pragma mark --------  自己的 delegate  -----------

//  login success
- (void)LoginSuccess:(NSString *)_token Date:(NSString *)_expirationDate :(NSString *)_refreshToken {
    self.expirationDate = _expirationDate;
    self.sessionKey = _token;
    
    // save 
    [self saveLoginInfo];
	if ([self.engineDelegate respondsToSelector:@selector(didLoginSuccessed)]) {
        [self.engineDelegate didLoginSuccessed];
    }
}

- (void)NotLogin:(NSString *)_error{
    if ([self.engineDelegate respondsToSelector:@selector(didNotLogin:)]) {
        [self.engineDelegate didNotLogin:_error];
    }
}


@end
