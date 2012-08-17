//
//  WBOauthCore.h
//  DemoForIPHone
//
//  Created by Brian.Chen on 11-7-27.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
// - - - - -  URL  - - - - - - - - 
#define AUTHORIZE_URL       @"http://game.weibo.com/oauth/auth/"
#define REGISTER_URL        @"http://weibo.cn/dpool/ttt/h5/reg.php"
#define RESPONSE_TYPE       @"ios"


#define REGISTER_URL        @"http://weibo.cn/dpool/ttt/h5/reg.php"
#define REFRESHTOKEN_URL    @"http://game.weibo.com/oauth/auth/token"



// - - - - -  请求时候报错状态 - - - - - - 
#define ERROR_CODE          @"signal_code"
#define ERROR_CLOSE         @"close"
#define ERROR_NULLMSG       @"msgnull"
#define ERROR_ERRORMSG      @"msgerror"
#define REGISTER            @"signup"

// - - - - -  报错提示框  - - - - - -- 
#define NULL_MESSAGE        @"  你的账号或密码为空 ！  "
#define ERROR_MESSAGE       @"  你的账号有误 ！ "


#define kLOADIMAGE(file) [UIImage imageWithContentsOfFile:[[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:file]]



