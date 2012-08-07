// ======================================================================================
// File         : GlobalSettings.cs
// Author       : Wu Jie 
// Last Change  : 04/07/2012 | 10:46:19 AM | Saturday,April
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// \class PlayerProfile 
// 
// \brief: store the historical highscore, gold and preferable selection
// 
///////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class PlayerProfile {

    // public int gamesPlayed = 0;
    public int token = 0;
    public string playerName = "molika";

    // options

    // NOTE: without this, we can't have profile
    public PlayerProfile () {
        // gamesPlayed = 0;
        token = 0;
        playerName = "molika";
    }
}

///////////////////////////////////////////////////////////////////////////////
// \class 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class GlobalSettings : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // static
    ///////////////////////////////////////////////////////////////////////////////

    protected static GlobalSettings instance_ = null; 
    public static GlobalSettings instance {
        get {
            if ( instance_ == null ) {
                instance_ = FindObjectOfType ( typeof(GlobalSettings) ) as GlobalSettings;
                if ( instance_ == null ) {
                    GameObject go = new GameObject("GlobalSettings_Temp");
                    instance_ = go.AddComponent<GlobalSettings>();
                }
            }
            return instance_;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // serialize
    ///////////////////////////////////////////////////////////////////////////////

    public bool dontDestroy = true; // NOTE: sometimes we test the GlobalSettings in single scene, this help us setup destroy method.
    
    ///////////////////////////////////////////////////////////////////////////////
    // non-serialize 
    ///////////////////////////////////////////////////////////////////////////////

    public bool isTemporary { get { return (dontDestroy == false); } }

    // easy interface for access player profile
    public string playerName { set { playerProfile.playerName = value; } get { return playerProfile.playerName; } }
    public int playerToken { set { playerProfile.token = value; } get { return playerProfile.token; } }

    // serialize files
    [System.NonSerialized] public GameProgress gameProgress;
    [System.NonSerialized] public PlayerProfile playerProfile;
    [System.NonSerialized] public HSController hsController;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {

        // ======================================================== 
        // system setup
        // ======================================================== 

        Application.targetFrameRate = 60;
        if ( dontDestroy )
            DontDestroyOnLoad(gameObject);


        // ======================================================== 
        // get cached components 
        // ======================================================== 

        hsController = GetComponent<HSController>();

        // ======================================================== 
        // load game progress and initialize 
        // ======================================================== 

        gameProgress = GetComponent<GameProgress>();
        if ( gameProgress != null ) {
            gameProgress.LoadPlayerProfile();
            // ApplyPlayerProfile();
        }

        // ======================================================== 
        // here we will create instance to get the default value and prevent NULL reference
        // ======================================================== 

        if ( playerProfile == null ) {
            playerProfile = new PlayerProfile();
        }

	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ApplyPlayerProfile () {
        if ( playerProfile != null ) {
            // //handle difficulty
            // if ( playerProfile.isCasualDifficulty ) {
            //     difficulty = casual;
            // } else {
            //     difficulty = arcade;
            // }
            // //DEBUG if playerprofile exist but couldn't init itemUnlockState, do it again here.
            // if ( playerProfile.itemUnlockState.Length == 0 ) {
            //     playerProfile.itemUnlockState = new bool[(int)ItemInfo.ID.Count];
            //     for ( int i = 0; i < playerProfile.itemUnlockState.Length; ++i ) {
            //         playerProfile.itemUnlockState[i] = false;
            //     }
            // }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void SavePlayerProfile () {
        if ( gameProgress != null )
            gameProgress.SavePlayerProfile();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnApplicationPause (bool pause) {
        if (pause) {
            SavePlayerProfile();
        }
    }


}
