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

    public BtnPower btnPower;
    public PanelGameOver panelGameOver;
    public exSpriteFont txtTime;
    // public exSpriteFont txtScratch;
    public exSpriteFont txtStart;
    public exSpriteFont txtToken;
    public ProgressBar powerBar;

    protected Vector3 worldMousePos = Vector3.zero;
    protected Vector3 lastWorldMousePos = Vector3.zero;
    protected Vector3 lastScreenMousePos = Vector3.zero;

    protected bool startDragging = false;
    protected bool isPowerReady = false;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Awake() {
        txtTime.text = "0.0";
        // txtScratch.text = "0";
        txtToken.text = "0";
        txtStart.enabled = false;
        btnPower.Deactive();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        txtTime.text = "0.0";
        txtToken.text = "0";
        // txtScratch.text = "0";
        txtStart.enabled = false;
        txtStart.text = "Get Ready";
        powerBar.ratio = 0;
        isPowerReady = false;
        btnPower.Deactive();
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
        if (Time.frameCount % 6 == 1) {
            txtTime.text = Stage.instance.timer.ToString("0.0");
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnScratchUpdate() {
        // txtScratch.text = Stage.instance.scratch.ToString();
        float ratio = Mathf.Min(Stage.instance.power/Stage.instance.player.maxPower, 1.0f);
        powerBar.ratio = ratio; 
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

    public void PowerMaxed() {
        btnPower.Active();
        isPowerReady = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void PowerRelease() {
        btnPower.Deactive();
        Stage.instance.PowerReleased();
        OnScratchUpdate();
        isPowerReady = false;
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void HandleInput () {

        //keyboard

        if (isPowerReady && Input.GetKeyDown("space")) {
            PowerRelease();
        }

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
                    Stage.instance.player.InitMapLocation();
                } else if (touch.phase == TouchPhase.Moved) {
                     worldMousePos = Camera.main.ScreenToWorldPoint ( touch.position );
                     mappedPos = worldMousePos - lastWorldMousePos;
                     Stage.instance.player.UpdateInputLocation(mappedPos);
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
                Stage.instance.player.InitMapLocation();
                startDragging = true;
            }

            // DELME: nantas when you read this, remove it { 
            // jwu comment: I play Cave's game and Shogun (a cave like STG, free to play), all of them are moving
            // flight use 1:1 touch ratio. when you grab TouchBegan, you will use this point as anchor, and remember current flight
            // position, the rest of thing is always get distance of touchpoint-anchor, and apply the same distance to srcFlightPos 
            // to get it to the right place. 
            // e.g.
            // result = (touchPoint - touchAnchor) + sourceFlightPos; 
            // NOTE: touchPoint and touchAnchor will **not** transform from screenPos to worldPos??? need it??? may be I'm wrong.
            // } DELME end 

            // press
            if (startDragging) {
                if ( Input.GetMouseButton(0) ) {
                    // if (Input.mousePosition == lastScreenMousePos) {
                    //     mappedPos = Vector3.zero;
                    //     Stage.instance.player.UpdateInputLocation(mappedPos);
                    //     return;
                    // } else {
                        worldMousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
                        mappedPos = worldMousePos - lastWorldMousePos;
                        Stage.instance.player.UpdateInputLocation(mappedPos);
                        return;
                    // }
                    // lastScreenMousePos = Input.mousePosition;
                }
                // release
                else if ( Input.GetMouseButtonUp(0) ) {
                    startDragging = false;
                    return;
                }
            } else {
                Stage.instance.player.InitMapLocation();
                Stage.instance.player.UpdateInputLocation(Vector3.zero);
            }
                

#if UNITY_IPHONE
        }
#endif

        // Stage.instance.player.UpdateInputLocation(mappedPos);
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void ShowGameOver() {
        panelGameOver.transform.position = Vector3.zero;
        panelGameOver.btnRetry.transform.position = new Vector3 ( Stage.instance.transform.position.x,
                                                                  Stage.instance.transform.position.y - 250,
                                                                  panelGameOver.btnRetry.transform.position.z);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void HideGameOver() {
        panelGameOver.transform.position = new Vector3(0.0f, 1000.0f, 0);
    }

}

