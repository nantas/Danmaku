// ======================================================================================
// File         : SNSMng.cs
// Author       : nantas 
// Last Change  : 08/18/2012 | 03:37:52 AM | Saturday,August
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System;

// using System.Text.RegularExpressions;

public class SNSMng: MonoBehaviour {

    public string appKey;
    public string appSecret;
    public string apiURL = "http://api.weibo.com/game/1/";
    [System.NonSerialized] public string sessionKey;
    [System.NonSerialized] public string userID;
    [System.NonSerialized] public string signature;
    [System.NonSerialized] public string url;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public void GetURL () {
        Application.ExternalCall("GetURL");
    }

    public void AssignURL(string _url) {
        url = _url;
        GetUserInfo();
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public void GetUserInfo(  ) {
        string[] splits = url.Split(new Char[] {'?','&'});
        // string display = "";

        // if (splits.Length < 7) {
        //     _targetText.text = url;
        // }

        if (splits[3].StartsWith("wyx_user_id=")) {
            int index = splits[3].IndexOf("=");
            userID = splits[3].Substring(index + 1);
            // display += userID + "\n";
        }

        if (splits[4].StartsWith("wyx_session_key=")) {
            int index = splits[4].IndexOf("=");
            sessionKey = splits[4].Substring(index + 1);
            // display += sessionKey + "\n";
        }

        if (splits[7].StartsWith("wyx_signature=")) {
            int index = splits[7].IndexOf("=");
            signature = splits[7].Substring(index + 1);
            // display += signature + "\n";
        }

        // _targetText.text = display;


        // Uri myUri = new Uri(_url);
        // sessionKey = HttpUtility.ParseQueryString(myUri.Query).Get("wyx_session_key");
        // userID = HttpUtility.ParseQueryString(myUri.Query).Get("wyx_user_id"); 
        // signature = HttpUtility.ParseQueryString(myUri.Query).Get("wyx_signature");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        if (Application.isEditor == false) { 
            GetURL();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // public void OnLeaderBoardSet( string _data ) {
    // }

    // public void OnLeaderBoardShow( string _data ) {
    // }

    public IEnumerator SetLeaderBoard(int _score) {
       string post_url = apiURL + "leaderboards/set?" + "source=" + appKey + "&timestamp=" + Time.time
                                        + "&signature=" + signature + "&session_key=" + sessionKey + "&rank_id=" + "1"
                                        + "&value=" + _score;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // public string GetParamValue( string _name )
    // {
    //   _name = _name.Replace(/[\[]/,"\\\[").Replace(/[\]]/,"\\\]");
    //   string regexS = "[\\?&]"+_name+"=([^&#]*)";
    //   string regex = new Regex( regexS );
    //   var results = regex.exec( window.location.href );
    //   if( results == null )
    //     return "";
    //   else
    //     return results[1];
    // }

}




