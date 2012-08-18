// ======================================================================================
// File         : MainMenu.cs
// Author       : nantas 
// Last Change  : 07/22/2012 | 14:49:17 PM | Sunday,July
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;


///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class MainMenu: MonoBehaviour {

    public exUIButton btnStart;
    public exSpriteFont testText;
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start() {

        // string url2 = "http://abitgames.com/webgames/Danmaku/PureDanmaku.html?leftnav=1&wvr=4&wyx_user_id=1861698380&wyx_session_key=59076c04d06f02039f98e7312a2489071c445eea_1345291982_1861698380&wyx_create=1345291982&wyx_expire=1345327982&wyx_signature=49fb66d88da47edf5c23652767b09f835af9d260";
        // Game.instance.snsMng.GetUserInfo();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnLoadLevel() {
        Application.LoadLevel("scene02");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel("scene02");
        }
    }
}

