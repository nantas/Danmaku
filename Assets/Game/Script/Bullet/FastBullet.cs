// ======================================================================================
// File         : FastBullet.cs
// Author       : nantas 
// Last Change  : 08/11/2012 | 16:13:00 PM | Saturday,August
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

public class FastBullet: Bullet {

    protected TridentBulletMng inheritBulletMng;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void Init(BulletMng _mng) {
        base.Init(_mng);
        inheritBulletMng = _mng as TridentBulletMng;
    }

    // ------------------------------------------------------------------ 
    // Desc:  
    // ------------------------------------------------------------------ 

    public void MoveWithDirection(Vector2 _direction) { 
        velocity = _direction.normalized * bulletSpeed;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {

        Vector2 dist = velocity * Time.deltaTime;
        //handle movement 
        transform.Translate(dist.x, dist.y, 0.0f);

        if (transform.position.x > Stage.instance.boundingRight + challengeMng.spawnAreaMargin ||
            transform.position.x < Stage.instance.boundingLeft - challengeMng.spawnAreaMargin ||
            transform.position.y > Stage.instance.boundingTop + challengeMng.spawnAreaMargin ||
            transform.position.y < Stage.instance.boundingBot - challengeMng.spawnAreaMargin) {
            Stage.instance.spawner.DestroyFastBullet(this);
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void Destroy() {
        Stage.instance.spawner.DestroyFastBullet(this);
    }

}
