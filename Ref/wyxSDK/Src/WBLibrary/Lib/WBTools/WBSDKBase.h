//
//  WBConstants.h
//
//  Created by  weiyouxi on 11-7-4.
//  Copyright 2011年 __MyCompanyName__. All rights reserved.
//

#import "WBGameCenterManager.h"

#define kNumberOfDownload       25
#define kNoRankTag              @"-1"

//#define WB_IPHONE   1
#define WB_IPAD     1


#define WB_ISDICT(X)  [X isKindOfClass:[NSDictionary class]]
#define WB_ISARRAY(X) [X isKindOfClass:[NSArray class]]

#define kIPADScreenHeight       864
#define kIPADScreenWidth        548
#define kIPADScreenHeight_ls    kIPADScreenWidth
#define kIPADScreenWidth_ls     706

#define kIPADCellHeight         73



// 
//Cache Path
//
#define DCUMENT                             [NSHomeDirectory() stringByAppendingString:@"/Documents"]
#define ACHIEVEMENT_CACHE_IMGPATH           [NSHomeDirectory() stringByAppendingString:@"/Documents/Achiement"]
#define DOCUMENT_IMG_CACHE                  [NSHomeDirectory() stringByAppendingString:@"/Documents/Achiement/header.jpg"]

#define FRIENDINFO_CACHE                    [NSHomeDirectory() stringByAppendingString:@"/Documents/FriendInfo"]
#define LANDINGPAGE_CACHE                   [NSHomeDirectory() stringByAppendingString:@"/Documents/LandingPage"]

#define HIGHSCORE_TOTAL_CACHE               [NSHomeDirectory() stringByAppendingString:@"/Documents/ToTal"]
#define HIGHSCORE_FRIEND_CACHE              [NSHomeDirectory() stringByAppendingString:@"/Documents/Friend"]
#define HIGHSCORE_DOCUMENT_CACHE            [NSHomeDirectory() stringByAppendingString:@"/Documents/HighScore"]


#define TOTAL_RANK1_CACHE                   [NSHomeDirectory() stringByAppendingString:@"/Documents/ToTal/TotalRank1"]
#define TOTAL_RANK2_CACHE                   [NSHomeDirectory() stringByAppendingString:@"/Documents/ToTal/TotalRank2"]
#define TOTAL_RANK3_CACHE                   [NSHomeDirectory() stringByAppendingString:@"/Documents/ToTal/TotalRank3"]

#define FRIEND_RANK1_CACHE                  [NSHomeDirectory() stringByAppendingString:@"/Documents/Friend/FriendRank1"]
#define FRIEND_RANK2_CACHE                  [NSHomeDirectory() stringByAppendingString:@"/Documents/Friend/FriendRank2"]
#define FRIEND_RANK3_CACHE                  [NSHomeDirectory() stringByAppendingString:@"/Documents/Friend/FriendRank3"]

#define POST_ACHIEMENT_CACHE                [NSHomeDirectory() stringByAppendingString:@"/Documents/postAchiementData"]
#define POST_HIGHSCORE_CACHE                [NSHomeDirectory() stringByAppendingString:@"/Documents/postHighScoreData"]

#define NO_USER_CACHE_HIGH                  [NSHomeDirectory() stringByAppendingString:@"/Documents/NoUserDataHighscore"]
#define NO_USER_CACHE_ACH                   [NSHomeDirectory() stringByAppendingString:@"/Documents/NoUserDataAchiment"]


//
//File Path 
//
#define kUserScoreRecordsFile   @"WBUserRecords"



//用户信息相关接口
#define kBaseURLShow                        @"http://game.weibo.com/api/1/user/show"
#define kBaseURLFriends                     @"http://game.weibo.com/game/1/user/friends"
#define kBaseURLFriend_ids                  @"http://game.weibo.com/api/1/user/friend_ids"
#define kBaseURLApp_friends                 @"http://game.weibo.com/api/1/user/app_friends"
#define kBaseURLApp_friends_ids             @"http://game.weibo.com/api/1/user/app_friend_ids"
#define kBaseURLAre_friends                 @"http://api.weibo.com/game/1/user/are_friends"

// 互粉好友排行页面 
#define TABLEVIEWCONTROLLER_RANKREQUEST_URL @"http://game.weibo.com/api/1/achievements/rank.json"

//成就
#define GET_ACHIEVEMENTCORE_URL @"http://api.weibo.com/game/1/achievements/get.json"
#define SET_ACHIEVEMENTCORE_URL @"http://api.weibo.com/game/1/achievements/set.json"

//排行信息
#define kBaseURLOfPostUserScore  @"http://api.weibo.com/game/1/leaderboards/set.json"
#define kBaseURLOfGetFriends     @"http://api.weibo.com/game/1/leaderboards/get_friends.json"
#define kBaseURLOfIncrement      @"http://api.weibo.com/game/1/leaderboards/increment.json"
#define kBaseURLOfGetTotal       @"http://api.weibo.com/game/1/leaderboards/get_total.json"

//微博
#define kBaseURLStatusUpload     @"http://game.weibo.com/api/1/statuses/upload.json"

#define ACHIEVEMENT_ID           @"achv_id"
#define HIGHSCORE_RANK_ID        @"rank_id"
#define HIGHSCORE_VALUE          @"value"

#define SOURCE                  @"source"
#define SIGNATURE               @"signature"
#define ACCESS_TOKEN            @"access_token"
#define SECRET_KEY              @"secret_key"
#define SESSION_KEY             @"session_key"
#define EXPIRATION_DATE         @"expiration_Date"
#define REFRESH_TOKEN           @"refresh_token"
#define CURRUNT_ID              @"currentID"
#define TIMESTAMP               @"timestamp"
#define PLATFORM_ID             @"platform_id"
#define PLATFORM_NUM_ID         @"3"
#define TRIM                    @"trim"
#define PAGE                    @"page"
#define COUNT                   @"count"
#define STATUS                  @"status"
#define PIC                     @"pic"
#define UIDD                    @"uid"
#define EVENTID                 @"event_id"
#define SSECRET                 @"secret"


#define UID1                     @"uid1"
#define UID2                     @"uid2"

#define GET                     @"GET"
#define POST                    @"POST"

#define ACHIEVEMENTID_ID            @"id"
#define ACHIEVEMENTID_TITLE         @"title"
#define ACHIEVEMENTID_DESCRIPTION   @"description"
#define ACHIEVEMENTID_ICONURL       @"iconUrl"
#define ACHIEVEMENTID_POINT         @"point"
#define ACHIEVEMENTID_ZANTIMES      @"zanTimes"
#define ACHIEVEMENTID_COMMENTIMES   @"commentimes"
#define ACHIEVEMENTID_UNLOCK        @"unlock"


#define ORIENTATION_LAND  [WBGameCenterManager sharedInstance].isLandscape
#define AUTHVIEW_FRAME  ORIENTATION_LAND?AUTHVIEW_LAND:AUTHVIEW_POR
#define AUTHVIEW_LAND CGRectMake(10, 10, 440, 280)
#define AUTHVIEW_POR CGRectMake(10, 10, 280, 440)

#define AUTHVIEW_FRAME_IPAD  ORIENTATION_LAND?AUTHVIEW_LAND_IPAD:AUTHVIEW_POR_IPAD
#define AUTHVIEW_LAND_IPAD CGRectMake(10, 10, 875, 650)
#define AUTHVIEW_POR_IPAD CGRectMake(10, 10, 650, 875)

#define ORIENTATION_LAND  [WBGameCenterManager sharedInstance].isLandscape
#define AUTHVIEW_TOP_IMAGE  ORIENTATION_LAND?AUTHVIEW_TOP_IMAGE_LAND:AUTHVIEW_TOP_IMAGE_POR
#define AUTHVIEW_TOP_IMAGE_LAND CGRectMake(10, 10,440 , 20)
#define AUTHVIEW_TOP_IMAGE_POR CGRectMake(10, 10,280 , 20)

#define ORIENTATION_LAND  [WBGameCenterManager sharedInstance].isLandscape
#define AUTHVIEW_TOP_CANCEL  ORIENTATION_LAND?AUTHVIEW_TOP_CANCEL_LAND:AUTHVIEW_TOP_CANCEL_POR
#define AUTHVIEW_TOP_CANCEL_LAND CGRectMake(10, 30, 440, 260)
#define AUTHVIEW_TOP_CANCEL_POR CGRectMake(10, 10, 280, 440)

