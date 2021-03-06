// ======================================================================================
// File         : NormalBulletMng.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 15:47:25 PM | Saturday,June
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

public class NormalBulletMng: BulletMng {

    public int initMaxNormalBullet = 0;
    public float minInterval = 0.0f;
    public float maxInterval = 0.0f;
    public float initMinSpeed = 0.0f;
    public float initMaxSpeed = 0.0f;

    [System.NonSerialized] public int normalBulletCount = 0;
    [System.NonSerialized] public float minSpeed = 0.0f;
    [System.NonSerialized] public float maxSpeed = 0.0f;
    [System.NonSerialized] public int maxNormalBullet = 1;
    protected int speedLvl = 0;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void Reset() {
        base.Reset();
        speedLvl = 0;
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void StartBullets() {
        // interval = Random.Range(minInterval, maxInterval);
        maxNormalBullet = initMaxNormalBullet;
        minSpeed = initMinSpeed;
        maxSpeed = initMaxSpeed;
        normalBulletCount = 0;
        // Invoke("SpawnANormalBullet", interval);
        // Invoke("SpawnShield", 15.0f);
        base.StartBullets();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void StopBullets() {
        base.StopBullets();
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void UpdateSpeed() {
        base.UpdateSpeed();
        speedLvl = Mathf.FloorToInt(Stage.instance.timer/20.0f);
        maxSpeed = initMaxSpeed + speedLvl * 30.0f;
        maxNormalBullet = initMaxNormalBullet + speedLvl * 15;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    Vector2 GetSpawnPoint () {
        float posX = 0.0f;
        float posY = 0.0f;
        int borderPicker = Random.Range(0, 4);
        if (borderPicker == 0) { //spawn from top
            posX = Random.Range(Stage.instance.boundingLeft - spawnAreaMargin/2, Stage.instance.boundingRight + spawnAreaMargin/2);
            posY = Stage.instance.boundingTop + spawnAreaMargin/2;
        } else if (borderPicker == 1) { //spawn from right
            posX = Stage.instance.boundingRight + spawnAreaMargin/2;
            posY = Random.Range(Stage.instance.boundingTop - spawnAreaMargin/2, Stage.instance.boundingBot + spawnAreaMargin/2);
        } else if (borderPicker == 2) { //spawn from bot
            posX = Random.Range(Stage.instance.boundingLeft - spawnAreaMargin/2, Stage.instance.boundingRight + spawnAreaMargin/2);
            posY = Stage.instance.boundingBot - spawnAreaMargin/2;
        } else if (borderPicker == 3) { //spawn from left
            posX = Stage.instance.boundingLeft - spawnAreaMargin/2;
            posY = Random.Range(Stage.instance.boundingTop - spawnAreaMargin/2, Stage.instance.boundingBot + spawnAreaMargin/2);
        }
 
        return new Vector2(posX, posY);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void SpawnANormalBullet() {
        if (normalBulletCount <= maxNormalBullet) {
            NormalBullet bullet = Stage.instance.spawner.SpawnNormalBullet(GetSpawnPoint());
            bullet.Init(this);
            bullet.Active();
            bullet.StartMoving();
            normalBulletCount++;
        }
        // Invoke("SpawnANormalBullet", Random.Range(minInterval, maxInterval));
        interval = Random.Range(minInterval, maxInterval);
        spawnTimer = 0.0f;
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Main Loop State
    ///////////////////////////////////////////////////////////////////////////////


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void EnterRun ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        base.EnterRun( _from, _to, _event );
        interval = Random.Range(minInterval, maxInterval);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void UpdateRun ( fsm.State _curState ) {
        base.UpdateRun(_curState);
        spawnTimer += Time.deltaTime;
        if (spawnTimer > interval) {
            SpawnANormalBullet();
        }
    }


}
