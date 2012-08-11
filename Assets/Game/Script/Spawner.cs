// ======================================================================================
// File         : Spawner.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 00:05:46 AM | Saturday,June
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

public class Spawner : MonoBehaviour {

    public float bulletPosZ = 0.0f;
    public exGameObjectPool normalBulletPool = new exGameObjectPool();
    public exGameObjectPool fastBulletPool = new exGameObjectPool();
    public exGameObjectPool powerUpPool = new exGameObjectPool();

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init() {
        // init bullet pool
        normalBulletPool.Init();
        for ( int i = 0; i < normalBulletPool.initData.Length; ++i ) {
            GameObject normalBulletGO = normalBulletPool.initData[i];
            normalBulletGO.transform.position = new Vector3( 0.0f, 0.0f, bulletPosZ);
            normalBulletGO.GetComponent<NormalBullet>().Inactive();
        }

        // init bullet pool
        fastBulletPool.Init();
        for ( int i = 0; i < fastBulletPool.initData.Length; ++i ) {
            GameObject fastBulletGO = fastBulletPool.initData[i];
            fastBulletGO.transform.position = new Vector3( 0.0f, 0.0f, bulletPosZ);
            fastBulletGO.GetComponent<FastBullet>().Inactive();
        }

        // init powerup pool
        powerUpPool.Init();
        for ( int i = 0; i < powerUpPool.initData.Length; ++i ) {
            GameObject powerUpGO = powerUpPool.initData[i];
            powerUpGO.transform.position = new Vector3( 0.0f, 0.0f, bulletPosZ);
            powerUpGO.GetComponent<PowerUp>().Inactive();
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset() {
        // reset bullet pool
        for ( int i = 0; i < normalBulletPool.initData.Length; ++i ) {
            GameObject normalBulletGO = normalBulletPool.initData[i];
            normalBulletGO.GetComponent<NormalBullet>().Inactive();
        }
        normalBulletPool.Reset();

        // reset bullet pool
        for ( int i = 0; i < fastBulletPool.initData.Length; ++i ) {
            GameObject fastBulletGO = fastBulletPool.initData[i];
            fastBulletGO.GetComponent<FastBullet>().Inactive();
        }
        fastBulletPool.Reset();

        // reset powerup pool
        for ( int i = 0; i < powerUpPool.initData.Length; ++i ) {
            GameObject powerUpGO = powerUpPool.initData[i];
            powerUpGO.GetComponent<PowerUp>().Inactive();
        }
        powerUpPool.Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public NormalBullet SpawnNormalBullet(Vector2 _pos) {
        return normalBulletPool.Request<NormalBullet>( _pos );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void DestroyNormalBullet( Bullet _normalBullet ) {
        _normalBullet.Inactive();
        normalBulletPool.Return(_normalBullet.gameObject);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public FastBullet SpawnFastBullet(Vector2 _pos) {
        return fastBulletPool.Request<FastBullet>(_pos);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void DestroyFastBullet( Bullet _fastBullet ) {
        _fastBullet.Inactive();
        fastBulletPool.Return(_fastBullet.gameObject);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public PowerUp SpawnPowerUp( Vector2 _pos ) {
        return powerUpPool.Request<PowerUp>(_pos);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void DestroyPowerUp( PowerUp _powerUp ) {
        _powerUp.Inactive();
        powerUpPool.Return(_powerUp.gameObject);
    }

}

