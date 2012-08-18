// ======================================================================================
// File         : PanelGameOver.cs
// Author       : nantas 
// Last Change  : 07/15/2012 | 21:47:19 PM | Sunday,July
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

public class PanelGameOver : MonoBehaviour {

    public exUIButton btnRetry;
    public exSpriteFont txtScores;
    public exSpriteFont txtCredit;
    //
    private bool showName = false;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnRetry() {
        ShowNamePrompt(false);
        Stage.instance.Restart();
        txtScores.enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public IEnumerator SubmitAndShowScore(string _name) {
        // bool result = GlobalSettings.instance.hsController.PostScores (_name + " ", Mathf.FloorToInt(Stage.instance.timer));
        // StartCoroutine(GlobalSettings.instance.hsController.PostScores (_name + " ", Mathf.FloorToInt(Stage.instance.timer)));

        yield return new WaitForSeconds(1.0f);

        // StartCoroutine(GlobalSettings.instance.hsController.GetScoresTo(txtScores));
        StartCoroutine(GlobalSettings.instance.hsController.GetUserScore(txtScores));

        // if (result) {
        //     StartCoroutine(GlobalSettings.instance.hsController.GetScoresTo(txtScores));
        // }
    }
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void ShowNamePrompt(bool _show) {
        showName = _show;
        txtCredit.enabled = _show;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            OnRetry();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected string m_player_name = "";
    void OnGUI () {
        if (showName) {
            GUI.Label (new Rect (330, 105, 325, 40), "Enter your name:");
            // string m_player_name = "";
            m_player_name = GUI.TextField (new Rect (330, 130, 100, 20), m_player_name, 25);

            //button.
            if (GUI.Button (new Rect (330,160,100,20), "Submit Score")) {
                // if (m_player_name != GlobalSettings.instance.playerProfile.playerName) {
                    GlobalSettings.instance.playerProfile.playerName = m_player_name;
                    StartCoroutine(SubmitAndShowScore(m_player_name));
                    GlobalSettings.instance.gameProgress.SavePlayerProfile();
                    ShowNamePrompt(false);
                // }
            }
        }
    }

}


