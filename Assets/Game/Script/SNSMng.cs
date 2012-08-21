// ======================================================================================
// File         : SNSMng.cs
// Author       : nantas 
// Last Change  : 08/18/2012 | 03:37:52 AM | Saturday,August
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

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

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public string GetHashString( string _baseString ) {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(_baseString);
        // encrypt bytes
        System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        byte[] hashBytes = sha1.ComputeHash(bytes);
     
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
     
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
     
        return hashString.PadLeft(32, '0');
 

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    /// <summary>
    /// 计算参数签名
    /// </summary>
    /// <param name="params">请求参数集，所有参数必须已转换为字符串类型</param>
    /// <param name="secret">签名密钥</param>
    /// <returns>签名</returns>
    public string GetSignature(IDictionary<string, string> parameters, string secret)
    {
        // 先将参数以其参数名的字典序升序进行排序
        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
        IEnumerator<KeyValuePair<string, string>> iterator= sortedParams.GetEnumerator();
     
        // 遍历排序后的字典，将所有参数按"key=value"格式拼接在一起
        StringBuilder basestring= new StringBuilder();
        while (iterator.MoveNext()) {
                string key = iterator.Current.Key;
                string value = iterator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)){
                    if (basestring.Length > 0) {
                        basestring.Append("&");
                    }
                    basestring.Append(key).Append("=").Append(value);
                }
        }
        basestring.Append(secret);
        Debug.Log(basestring.ToString());
     
        // 使用MD5对待签名串求签
        System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        byte[] bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(basestring.ToString()));
     
        // 将MD5输出的二进制结果转换为小写的十六进制
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++) {
                string hex = bytes[i].ToString("x");
                if (hex.Length == 1) {
                    result.Append("0");
                }
                result.Append(hex);
        }
     
        return result.ToString();
}


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 


    public IEnumerator SetLeaderBoard(int _score) {
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("session_key", sessionKey);
        param.Add("source", appKey);
        param.Add("timestamp", timeStamp);
        param.Add("rank_id", "1");
        param.Add("value", _score.ToString());

        // string base_string = "rank_id=" + "1" + "&session_key=" + sessionKey + "&source=" + appKey
        //                     + "&timestamp=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")
        //                     + "&value=" + _score + appSecret; 

        string signature = GetSignature( param, appSecret ); 
        string post_url = apiURL + "leaderboards/set?" + "source=" + appKey + "&timestamp=" + timeStamp
                                        + "&signature=" + signature + "&session_key=" + sessionKey + "&rank_id=" + "1"
                                        + "&value=" + _score.ToString();

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        Debug.Log(post_url);
        Debug.Log(hs_post.text);

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
    }
 
// ------------------------------------------------------------------ 
// Desc: 
// ------------------------------------------------------------------ 

    public IEnumerator GetUser() {
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("session_key", sessionKey);
        param.Add("source", appKey);
        param.Add("timestamp", timeStamp);

        // string base_string = "rank_id=" + "1" + "&session_key=" + sessionKey + "&source=" + appKey
        //                     + "&timestamp=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")
        //                     + "&value=" + _score + appSecret; 

        string signature = GetSignature( param, appSecret ); 
        string post_url = apiURL + "user/show?" + "source=" + appKey + "&timestamp=" + timeStamp
                                        + "&signature=" + signature + "&session_key=" + sessionKey;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        Debug.Log(post_url);
        Debug.Log(hs_post.text);
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




