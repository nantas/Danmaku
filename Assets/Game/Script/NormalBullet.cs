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
        Vector2 playerPos = Game.instance.player.transform.position;
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

        if (transform.position.x > Game.instance.boundingRight + bulletMng.spawnAreaMargin ||
            transform.position.x < Game.instance.boundingLeft - bulletMng.spawnAreaMargin ||
            transform.position.y > Game.instance.boundingTop + bulletMng.spawnAreaMargin ||
            transform.position.y < Game.instance.boundingBot - bulletMng.spawnAreaMargin) {
            Game.instance.spawner.DestroyBullet(this);
            bulletMng.normalBulletCount -= 1;
        }

    }



}
