//
//  GameCenterEngineDelegate.h
//  SinaGameCenter
//
//  Created by Brian.Chen on 11-5-25.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ASIHTTPRequest.h"
@class OAToken;

@protocol WBGameCenterDelegate <NSObject>

- (void)authLoginSuccessed;
- (void)authLoginFailed;
- (void)requestFinished:(ASIHTTPRequest *)request;
- (void)requestFailed:(ASIHTTPRequest *)request;

@end
