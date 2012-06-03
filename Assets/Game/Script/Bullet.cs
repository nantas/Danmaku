// ======================================================================================
// File         : Bullet.cs
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

public class Bullet: MonoBehaviour {


    public float aimingRange = 0.0f;

    protected Vector2 velocity = Vector2.zero;
    protected BulletMng bulletMng = null;
    protected exSprite spBullet;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        bulletMng = Game.instance.bulletMng;
        spBullet = GetComponent<exSprite>();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Inactive() {
        spBullet.enabled = false;
        collider.enabled = false;
        enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Active() {
        spBullet.enabled = true;
        collider.enabled = true;
        enabled = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnTriggerEnter( Collider _other ) {
        if (_other.gameObject.tag == "Player") {
            _other.GetComponent<Player>().Destroy();
        } else if (_other.gameObject.tag == "PlayerSkirt" && Game.instance.player.isShielded) {
            Game.instance.spawner.DestroyBullet(this);
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnTriggerExit ( Collider _other ) {
        if (_other.gameObject.tag == "PlayerSkirt") {
            Game.instance.Scratch();
        }
    }
}


