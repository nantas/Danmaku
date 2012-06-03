// ======================================================================================
// File         : PowerUp.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 21:02:56 PM | Saturday,June
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

public class PowerUp: MonoBehaviour {

    public int maxBumpCount = 0;
    public float moveSpeed = 0.0f;

    protected Vector2 velocity = Vector2.zero;
    protected exSprite spPowerUp;
    protected int bumpCount = 0;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        spPowerUp = GetComponent<exSprite>();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Inactive() {
        spPowerUp.enabled = false;
        collider.enabled = false;
        enabled = false;
        bumpCount = 0;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Active() {
        spPowerUp.enabled = true;
        collider.enabled = true;
        enabled = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void OnTriggerEnter( Collider _other ) {
        // if (_other.gameObject.tag == "Player") {
        //     _other.GetComponent<Player>().Destroy();
        // }
    }


}



