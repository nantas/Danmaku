//
//  WBOauthView.m
//  DemoForIPHone
//
//  Created by Brian.Chen on 11-7-27.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import "WBOauthView.h"
#import "WBOauthCore.h"
#import "WBGameCenterManager.h"
#import "WBSDKBase.h"


static CGFloat kBlue[4] = {0.42578125, 0.515625, 0.703125, 1.0};
static CGFloat kBorderGray[4] = {0.3, 0.3, 0.3, 0.8};
static CGFloat kBorderBlack[4] = {0.3, 0.3, 0.3, 1};
static CGFloat kBorderBlue[4] = {0.23, 0.35, 0.6, 1.0};
static CGFloat kTransitionDuration = 0.1;
static CGFloat kPadding = 10;
static CGFloat kBorderWidth = 10;

BOOL RRIsDeviceIPad() {
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 30200
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
        return YES;
    }
#endif
    return NO;
}

@implementation WBOauthView

@synthesize paramsDic,serverURL,loadingURL;
@synthesize webView,webTopImgView,webTopLabel,webTopBtn,spinner,backgroundView;
@synthesize oauthDelegate;



- (void)dealloc
{
    self.paramsDic = nil;
    self.serverURL = nil;
    self.loadingURL = nil;
   
    self.spinner = nil;
    
    [self.webView removeFromSuperview];
    self.webView = nil;
    
    [self.webTopBtn removeFromSuperview];
    self.webTopBtn = nil;
    
    [self.webTopLabel removeFromSuperview];
    self.webTopLabel = nil;
    
    [self.webTopImgView  removeFromSuperview];
    self.webTopImgView = nil;
    
    [super dealloc];
}

- (id)init {
    self = [super initWithFrame:CGRectZero];
    
    if (self) {
        
        self.oauthDelegate = nil;
        self.loadingURL = nil;
        //_orientation = UIDeviceOrientationUnknown;
        //_showingKeyboard = NO;
        
        self.backgroundColor = [UIColor clearColor];
        self.autoresizesSubviews = YES;
        self.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
        self.contentMode = UIViewContentModeRedraw;
        
        
        if (RRIsDeviceIPad()) {
            self.webView = [[UIWebView alloc] initWithFrame:AUTHVIEW_FRAME_IPAD];
        }else{
            self.webView = [[UIWebView alloc] initWithFrame:AUTHVIEW_FRAME];
        }
        
        self.webView.delegate = self;
        self.webView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
                
    }
    return self;
}

- (id)initWithURL: (NSString *)_serverURL params: (NSMutableDictionary *)_params
{
    self = [self init];
    self.serverURL = [_serverURL retain];
    self.paramsDic = _params;
    
    return self;
}

- (void)loadURL:(NSString*)url get:(NSDictionary*)getParams{
    [self.loadingURL release];
    self.loadingURL = [[self generateURL:url params:getParams] retain];	
    NSMutableURLRequest* request = [NSMutableURLRequest requestWithURL:self.loadingURL];
    [self.webView loadRequest:request];
}

- (void)showRequstView {
    [self loadURL:self.serverURL get:self.paramsDic];
    [self sizeToFitOrientation:NO];
    [self boundWebView];
}

- (void)boundWebView{
           
    [self addSubview:self.webView];
    
    self.spinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
    self.spinner.autoresizingMask = UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin
    | UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin;
    [self.spinner startAnimating];
    [self addSubview:self.spinner];
    [self.spinner release];
    
    
    [self.spinner sizeToFit];
    self.spinner.center = self.webView.center;
    
    
    UIWindow* window = [UIApplication sharedApplication].keyWindow;
    if (!window) {
        window = [[UIApplication sharedApplication].windows objectAtIndex:0];
    }
    [window addSubview:self];
}


- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    if ([WBGameCenterManager sharedInstance].isLandscape == YES) {
        if (interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft) {
            return YES;
        }else{
            return NO;
        }
    }else{
        if (interfaceOrientation == UIInterfaceOrientationPortrait || interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown) {
            return YES;
        }else{
            return NO;
        }
    }
}

#pragma -
#pragma mark ------------ NSString NSArray NSDictionary 拼接 ---------------

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

/**
 * 解析 url 参数的function*/

- (NSDictionary*)parseURLParams:(NSString *)query {
	NSArray *pairs = [query componentsSeparatedByString:@"&"];
	NSMutableDictionary *params = [[[NSMutableDictionary alloc] init] autorelease];
	for (NSString *pair in pairs) {
		NSArray *kv = [pair componentsSeparatedByString:@"="];
		NSString *val =
		[[kv objectAtIndex:1]
		 stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
		[params setObject:val forKey:[kv objectAtIndex:0]];
	}
	return params;
}

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

#pragma -
#pragma mark -------------  加载响应  --------------


- (void)isSucceed:(NSURL *)url {
    
	NSString *q = [url absoluteString];
    NSString *session = [self getStringFromUrl:q needle:@"access_token="];
    NSString *expTime = [self getStringFromUrl:q needle:@"expires_in="];
    NSString *refreshToken = [self getStringFromUrl:q needle:@"refresh_token="];
    
    //设置sesstionKey过期时间
    NSArray *sesstionArr = [session componentsSeparatedByString:@"_"];
    NSString *createTime = [sesstionArr objectAtIndex:[sesstionArr count] - 2];
    int cTime = (int)[createTime floatValue];
    int sessionCycle = (int)[expTime floatValue];
    int expired = cTime + sessionCycle;
    NSString *expiredTime = [NSString stringWithFormat:@"%d",expired];
    
    if ((session == (NSString *) [NSNull null]) || (session.length == 0)) {
        
    } else {
        if ([self.oauthDelegate respondsToSelector:@selector(LoginSuccess:Date::)]) {
            [self.oauthDelegate LoginSuccess:session Date:expiredTime :refreshToken];
        }
    }
    
    if (self.webView) {
        [self dismissClean];
    }
}

- (void)isCancel:(NSString *)_error{
    if ([self.oauthDelegate respondsToSelector:@selector(NotLogin:)]) {
        [self.oauthDelegate NotLogin:_error];
    }
    
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDuration:kTransitionDuration];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(dismissClean)];
    self.alpha = 0;
    [UIView commitAnimations];
     
}

- (void)dismissClean{
    [self.webView removeFromSuperview];
    self.webView = nil;
    
    [self removeFromSuperview];
    
}

-(void) errormsg:(NSString *)errMsg{
	if (errMsg) {
		NSLog(@"%@",errMsg);
	}
}


#pragma -
#pragma mark -------------  系统的 delegate  --------------

// - - - - -  - - - - -   WebView Delegate   - -- - - - - - - - -- -
- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request
 navigationType:(UIWebViewNavigationType)navigationType {
    
    NSURL* url = request.URL;
    
    NSString *urlStr = [NSString stringWithFormat:@"%@",url];
    
	//NSString *errorReason=nil;
	NSString *query = [url fragment];
	if (!query) {
		query = [url query];
	}
    
	NSDictionary *params = [self parseURLParams:query];
	NSString *session = [params valueForKey:ACCESS_TOKEN];
    NSString *signal_code = [params valueForKey: @"signal_code"];
    
    if ([signal_code isEqualToString:@"close"]) {
        [self isCancel:@""];
    }
    
    if ([urlStr hasPrefix:REGISTER_URL]) {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:REGISTER_URL]];
        return NO;
    }
    
	NSString *q = [url absoluteString];
    NSString *scheme = [q substringToIndex:3];
    
	if([scheme isEqualToString:@"wyx"] && session != nil) {
		[self isSucceed:url];
	}
    return YES;
}

- (void)webViewDidFinishLoad:(UIWebView *)webView {
    [self.spinner stopAnimating];
    self.spinner.hidden = YES;
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error {
    // 102 == WebKitErrorFrameLoadInterruptedByPolicyChange
    [self.spinner stopAnimating];
    if (!([error.domain isEqualToString:@"WebKitErrorDomain"])) {
        [self isCancel:@"WebKitErrorDomain"];
    }
}

#pragma -
#pragma mark -------------- 边框图 ------------------

- (void)drawRect:(CGRect)rect {
    CGRect grayRect = CGRectOffset(rect, -0.5, -0.5);
    [self drawRect:grayRect fill:kBorderGray radius:10];
    
    CGRect headerRect = CGRectMake(
                                   ceil(rect.origin.x + kBorderWidth), ceil(rect.origin.y + kBorderWidth),
                                   rect.size.width - kBorderWidth*2,0);
    [self drawRect:headerRect fill:kBlue radius:0];
    [self strokeLines:headerRect stroke:kBorderBlue];
    
    CGRect webRect = CGRectMake(
                                ceil(rect.origin.x + kBorderWidth), headerRect.origin.y + headerRect.size.height,
                                rect.size.width - kBorderWidth*2, self.webView.frame.size.height+1);
    [self strokeLines:webRect stroke:kBorderBlack];
}

- (void)addRoundedRectToPath:(CGContextRef)context rect:(CGRect)rect radius:(float)radius {
    CGContextBeginPath(context);
    CGContextSaveGState(context);
    
    if (radius == 0) {
        CGContextTranslateCTM(context, CGRectGetMinX(rect), CGRectGetMinY(rect));
        CGContextAddRect(context, rect);
    } else {
        rect = CGRectOffset(CGRectInset(rect, 0.5, 0.5), 0.5, 0.5);
        CGContextTranslateCTM(context, CGRectGetMinX(rect)-0.5, CGRectGetMinY(rect)-0.5);
        CGContextScaleCTM(context, radius, radius);
        float fw = CGRectGetWidth(rect) / radius;
        float fh = CGRectGetHeight(rect) / radius;
        
        CGContextMoveToPoint(context, fw, fh/2);
        CGContextAddArcToPoint(context, fw, fh, fw/2, fh, 1);
        CGContextAddArcToPoint(context, 0, fh, 0, fh/2, 1);
        CGContextAddArcToPoint(context, 0, 0, fw/2, 0, 1);
        CGContextAddArcToPoint(context, fw, 0, fw, fh/2, 1);
    }
    
    CGContextClosePath(context);
    CGContextRestoreGState(context);
}

- (void)drawRect:(CGRect)rect fill:(const CGFloat*)fillColors radius:(CGFloat)radius {
    CGContextRef context = UIGraphicsGetCurrentContext();
    CGColorSpaceRef space = CGColorSpaceCreateDeviceRGB();
    
    if (fillColors) {
        CGContextSaveGState(context);
        CGContextSetFillColor(context, fillColors);
        if (radius) {
            [self addRoundedRectToPath:context rect:rect radius:radius];
            CGContextFillPath(context);
        } else {
            CGContextFillRect(context, rect);
        }
        CGContextRestoreGState(context);
    }
    
    CGColorSpaceRelease(space);
}

- (void)strokeLines:(CGRect)rect stroke:(const CGFloat*)strokeColor {
    CGContextRef context = UIGraphicsGetCurrentContext();
    CGColorSpaceRef space = CGColorSpaceCreateDeviceRGB();
    
    CGContextSaveGState(context);
    CGContextSetStrokeColorSpace(context, space);
    CGContextSetStrokeColor(context, strokeColor);
    CGContextSetLineWidth(context, 1.0);
    
    {
        CGPoint points[] = {{rect.origin.x+0.5, rect.origin.y-0.5},
            {rect.origin.x+rect.size.width, rect.origin.y-0.5}};
        CGContextStrokeLineSegments(context, points, 2);
    }
    {
        CGPoint points[] = {{rect.origin.x+0.5, rect.origin.y+rect.size.height-0.5},
            {rect.origin.x+rect.size.width-0.5, rect.origin.y+rect.size.height-0.5}};
        CGContextStrokeLineSegments(context, points, 2);
    }
    {
        CGPoint points[] = {{rect.origin.x+rect.size.width-0.5, rect.origin.y},
            {rect.origin.x+rect.size.width-0.5, rect.origin.y+rect.size.height}};
        CGContextStrokeLineSegments(context, points, 2);
    }
    {
        CGPoint points[] = {{rect.origin.x+0.5, rect.origin.y},
            {rect.origin.x+0.5, rect.origin.y+rect.size.height}};
        CGContextStrokeLineSegments(context, points, 2);
    }
    
    CGContextRestoreGState(context);
    
    CGColorSpaceRelease(space);
}



- (void)sizeToFitOrientation:(BOOL)transform {
    if (transform) {
        self.transform = CGAffineTransformIdentity;
    }
    
    CGRect frame = [UIScreen mainScreen].applicationFrame;
    CGPoint center = CGPointMake(
                                 frame.origin.x + ceil(frame.size.width/2),
                                 frame.origin.y + ceil(frame.size.height/2));
    
    CGFloat scale_factor = 1.0f;
    if (RRIsDeviceIPad()) {
        // On the iPad the dialog's dimensions should only be 60% of the screen's
        scale_factor = 0.9;
    }
    
    CGFloat width = floor(scale_factor * frame.size.width) - kPadding * 2;
    CGFloat height = floor(scale_factor * frame.size.height) - kPadding * 2;
    
    orientation = [UIApplication sharedApplication].statusBarOrientation;
    if (UIInterfaceOrientationIsLandscape(orientation)) {
    
        self.frame = CGRectMake(kPadding, kPadding, height, width);
        
    } else {
        self.frame = CGRectMake(kPadding, kPadding, width, height);
    }
    self.center = center;
    
    self.transform = [self transformForOrientation];
    
}

- (CGAffineTransform)transformForOrientation {
    UIInterfaceOrientation _orientation = [UIApplication sharedApplication].statusBarOrientation;
    if (_orientation == UIInterfaceOrientationLandscapeLeft) {
        return CGAffineTransformMakeRotation(M_PI*1.5);
    } else if (_orientation == UIInterfaceOrientationLandscapeRight) {
        
        
        return CGAffineTransformMakeRotation(M_PI/2);
    } else if (_orientation == UIInterfaceOrientationPortraitUpsideDown) {
        return CGAffineTransformMakeRotation(-M_PI);
    } else {
        return CGAffineTransformIdentity;
    }
}

- (BOOL)gestureRecognizer:(UIGestureRecognizer *)gestureRecognizer shouldRecognizeSimultaneouslyWithGestureRecognizer:(UIGestureRecognizer *)otherGestureRecognizer{
    return YES;
}


@end
