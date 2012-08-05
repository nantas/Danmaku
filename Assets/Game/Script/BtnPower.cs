// ======================================================================================
// File         : BtnPower.cs
// Author       : nantas 
// Last Change  : 08/05/2012 | 16:08:08 PM | Sunday,August
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

public class BtnPower: MonoBehaviour {

    public exSprite bg;
    public exSpriteFont text;
    public exTimebasedCurve blinkCurve;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    private bool isBlinking = false;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Active() {
        GetComponent<exUIButton>().enabled = true;
        bg.enabled = true;
        text.enabled = true;
        blinkCurve.Start(true);
        isBlinking = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Deactive() {
        GetComponent<exUIButton>().enabled = false;
        bg.enabled = false;
        text.enabled = false;
        isBlinking = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void LateUpdate() {
        if (isBlinking) {
            float ratio = blinkCurve.Step();
            bg.color = new Color (bg.color.r,
                                  bg.color.g,
                                  bg.color.b,
                                  ratio);
            text.topColor = new Color(0.0f, 0.0f, 0.0f, ratio);
            text.botColor = new Color(0.0f, 0.0f, 0.0f, ratio);
        }
    }

}




