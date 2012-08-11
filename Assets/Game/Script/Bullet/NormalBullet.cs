// ======================================================================================
// File         : NormalBullet.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 16:28:16 PM | Saturday,June
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

public class NormalBullet: Bullet {


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartMoving() {
        Vector2 playerPos = Stage.instance.player.transform.position;
        Vector2 target = Random.insideUnitCircle * aimingRange + playerPos; 
        Vector2 direction = target - new Vector2(transform.position.x, transform.position.y);
        velocity = direction.normalized * Random.Range(bulletMng.minSpeed, bulletMng.maxSpeed);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {

        Vector2 dist = velocity * Time.deltaTime;
        //handle movement 
        transform.Translate(dist.x, dist.y, 0.0f);

        if (transform.position.x > Stage.instance.boundingRight + bulletMng.spawnAreaMargin ||
            transform.position.x < Stage.instance.boundingLeft - bulletMng.spawnAreaMargin ||
            transform.position.y > Stage.instance.boundingTop + bulletMng.spawnAreaMargin ||
            transform.position.y < Stage.instance.boundingBot - bulletMng.spawnAreaMargin) {
            Stage.instance.spawner.DestroyNormalBullet(this);
            bulletMng.normalBulletCount -= 1;
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void Destroy() {
        Stage.instance.spawner.DestroyNormalBullet(this);
    }


}
