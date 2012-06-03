// ======================================================================================
// File         : GamePanel.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 15:40:30 PM | Saturday,June
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class GamePanel: MonoBehaviour {

    public exUIPanel panelGameOver;
    public exSpriteFont txtTime;
    public exSpriteFont txtScratch;
    public exSpriteFont txtStart;

    protected Vector3 worldMousePos = Vector3.zero;
    protected Vector3 lastWorldMousePos = Vector3.zero;
    protected Vector3 lastScreenMousePos = Vector3.zero;

    protected bool startDragging = false;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Awake() {
        // exUIPanel uiPanel = GetComponent<exUIPanel>();
        // uiPanel.OnButtonPress += OnButtonPress;
        // uiPanel.OnButtonRelease += OnButtonRelease;
        // uiPanel.OnPointerMove += OnPointerMove;
        txtTime.text = "0";
        txtScratch.text = "0";
        txtStart.enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        txtTime.text = "0";
        txtScratch.text = "0";
        txtStart.enabled = false;
        txtStart.text = "Get Ready";
        // initPlayerPos = Vector3.zero;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update() {
        HandleInput();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void LateUpdate() {
        if (Time.frameCount % 30 == 1) {
            txtTime.text = Mathf.FloorToInt(Game.instance.timer).ToString();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnScratchUpdate() {
        txtScratch.text = Game.instance.scratch.ToString();
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public IEnumerator ShowStart() {
        txtStart.enabled = true;
        yield return new WaitForSeconds(1.0f);
        txtStart.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        txtStart.enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void HandleInput () {
        // Debug.Log("mouse input!");
        Vector3 mappedPos = Vector3.zero;

#if UNITY_IPHONE
        if ( Application.isEditor == false ) {
            //
            foreach ( Touch touch in Input.touches ) {
                // int fingerID = touch.fingerId; 

                // press
                if ( touch.phase == TouchPhase.Began ) {
                    lastWorldMousePos = Camera.main.ScreenToWorldPoint( touch.position );
                    Game.instance.player.InitMapLocation();
                } else if (touch.phase == TouchPhase.Moved) {
                     worldMousePos = Camera.main.ScreenToWorldPoint ( touch.position );
                     mappedPos = worldMousePos - lastWorldMousePos;
                     Game.instance.player.UpdateInputLocation(mappedPos);
                     return;
                } else if (touch.phase == TouchPhase.Ended) {
                    return;
                }
            }
        } else {
#endif
            //
            // int fingerID = 0;

            if ( Input.GetMouseButtonDown(0) ) {
                // mouseClickStart = Time.time;
                lastWorldMousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
                Game.instance.player.InitMapLocation();
                startDragging = true;
            }

            // press
            if (startDragging) {
                if ( Input.GetMouseButton(0) ) {
                    // if (Input.mousePosition == lastScreenMousePos) {
                    //     mappedPos = Vector3.zero;
                    //     Game.instance.player.UpdateInputLocation(mappedPos);
                    //     return;
                    // } else {
                        worldMousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
                        mappedPos = worldMousePos - lastWorldMousePos;
                        Game.instance.player.UpdateInputLocation(mappedPos);
                        return;
                    // }
                    // lastScreenMousePos = Input.mousePosition;
                }
                // release
                else if ( Input.GetMouseButtonUp(0) ) {
                    startDragging = false;
                    return;
                }
            }

#if UNITY_IPHONE
        }
#endif

        // Game.instance.player.UpdateInputLocation(mappedPos);
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void ShowGameOver() {
        panelGameOver.transform.position = Vector3.zero;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void HideGameOver() {
        panelGameOver.transform.position = new Vector3(1000.0f, 0, 0);
    }

}

