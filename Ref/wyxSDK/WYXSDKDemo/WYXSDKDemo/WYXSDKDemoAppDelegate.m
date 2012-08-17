//
//  WYXSDKDemoAppDelegate.m
//  WYXSDKDemo
//
//  Created by kim zhutie on 民国100-12-16.
//  Copyright 100 __MyCompanyName__. All rights reserved.
//

#import "WYXSDKDemoAppDelegate.h"

#import "WYXSDKDemoViewController.h"

#import "WBGameCenterManager.h"

@implementation WYXSDKDemoAppDelegate


@synthesize window=_window;

@synthesize viewController=_viewController;

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    self.window.rootViewController = self.viewController;
    [self.window makeKeyAndVisible];
    return YES;
}

/*
 *  通过safari授权完成后调用此方法
 **/
- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url {
    
    return [[WBGameCenterManager sharedInstance].gameEngine handleOpenURL:url];
}

- (void)dealloc
{
    [_window release];
    [_viewController release];
    [super dealloc];
}

@end
