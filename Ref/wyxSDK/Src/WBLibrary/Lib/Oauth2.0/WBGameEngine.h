//
//  WBGameEngine.h
//  DemoForIPHone
//
//  Created by Brian.Chen on 11-7-27.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "WBOauthView.h"
#import <CommonCrypto/CommonDigest.h>


@protocol WBEngineDelegate <NSObject>

//  - - - － 用户的登录成功
- (void)didLoginSuccessed;

//  - - - - 用户退出
- (void)loginOut;

//  - - - - 登录时候退出
- (void)didNotLogin:(NSString *)_error;

@end

@interface WBGameEngine : NSObject <OauthViewDelegate>{
    NSString    *appKey;
    NSString    *appSecret;
    
    NSString    *accessToken;
    NSString    *secret;
    NSString    *sessionKey;
    NSString      *expirationDate;
    NSString    *curruntID;
    
    WBOauthView *oauthView;
    id<WBEngineDelegate>engineDelegate;;
}

@property (nonatomic, copy) NSString *appSecret;
@property (nonatomic, copy) NSString *appKey;

@property (nonatomic, copy) NSString *accessToken;
@property (nonatomic, copy) NSString *secret;
@property (nonatomic, copy) NSString *sessionKey;
@property (nonatomic, copy) NSString *curruntID;
@property (nonatomic, retain) NSString *expirationDate;

@property (nonatomic, retain) WBOauthView *oauthView;
@property (nonatomic, assign) id<WBEngineDelegate>engineDelegate;


+ (WBGameEngine *)sharedInstance;
- (id)initWithAppKeyAndSecret:(NSString *)_appKey AppSecret:(NSString *)_appSecret;

//  开始授权
- (void)startAuthorize;
- (void)authorizeWithWBGame:(BOOL)tryWYXAppAuth safariAuth:(BOOL)trySafariAuth;
- (void)getRefreshToken;
- (BOOL)handleOpenURL:(NSURL *)url;

//  关于登录，状态，退出
- (BOOL)isSignning;
- (void)signOut;
- (void)saveLoginInfo;
- (void)deleteLoginInfo;

- (NSString *) getStringFromUrl: (NSString*) url needle:(NSString *) needle;
- (NSURL*)generateURL:(NSString*)baseURL params:(NSDictionary*)params;

@end
