//
//  WBOauthView.h
//  DemoForIPHone
//
//  Created by Brian.Chen on 11-7-27.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol OauthViewDelegate <NSObject>

//  登录成功   取消登录时执行
- (void)LoginSuccess:(NSString *)_token Date:(NSString *)_expirationDate :(NSString *)_refreshToken;
- (void)NotLogin:(NSString *)_error;

@end


@interface WBOauthView : UIView <UIWebViewDelegate,UIGestureRecognizerDelegate>{
    NSMutableDictionary     *paramsDic;
    NSString                *serverURL;
    NSURL                   *loadingURL;
    
    UIWebView               *webView;
    UIImageView             *webTopImgView;
    UILabel                 *webTopLabel;
    UIButton                *webTopBtn;
    UIActivityIndicatorView *spinner;
    UIView                  *backgroundView;
    
    id<OauthViewDelegate>oauthDelegate;
    UIDeviceOrientation orientation;
}

@property (nonatomic, retain) NSMutableDictionary *paramsDic;
@property (nonatomic, copy) NSString *serverURL;
@property (nonatomic, retain) NSURL *loadingURL;

@property (nonatomic, retain) UIWebView *webView;
@property (nonatomic, retain) UIImageView *webTopImgView;
@property (nonatomic, retain) UILabel *webTopLabel;
@property (nonatomic, retain) UIButton *webTopBtn;
@property (nonatomic, retain) UIActivityIndicatorView *spinner;
@property (nonatomic, retain) UIView *backgroundView;
@property (nonatomic, assign) id<OauthViewDelegate>oauthDelegate;

- (id)initWithURL: (NSString *)_serverURL params: (NSMutableDictionary *)_params;

- (void)loadURL:(NSString*)url get:(NSDictionary*)getParams;
- (void)showRequstView;
- (void)boundWebView;

/*
  NSString 接拼 
 */
- (NSURL*)generateURL:(NSString*)baseURL params:(NSDictionary*)params;
- (NSString *) getStringFromUrl: (NSString*) url needle:(NSString *) needle;
- (NSDictionary*)parseURLParams:(NSString *)query;
/*
  加载响应
 */
- (void)isSucceed:(NSURL *)url;
- (void)isCancel:(NSString *)_error;
- (void) errormsg:(NSString *)errMsg;
- (void)dismissClean;
/*
  画图
 */
- (void)addRoundedRectToPath:(CGContextRef)context rect:(CGRect)rect radius:(float)radius;
- (void)drawRect:(CGRect)rect fill:(const CGFloat*)fillColors radius:(CGFloat)radius;
- (void)strokeLines:(CGRect)rect stroke:(const CGFloat*)strokeColor;

/*
  CGAffineTransform
 */
- (void)sizeToFitOrientation:(BOOL)transform;
- (CGAffineTransform)transformForOrientation;

@end
