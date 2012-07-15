// ======================================================================================
// File         : BulletMng.cs
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

public class BulletMng: MonoBehaviour {

    public int initMaxNormalBullet = 0;
    public float minInterval = 0.0f;
    public float maxInterval = 0.0f;
    public float initMinSpeed = 0.0f;
    public float initMaxSpeed = 0.0f;
    public float spawnAreaMargin = 0.0f;

    [System.NonSerialized] public int normalBulletCount = 0;
    // protected Vector2 spawnPoint = Vector2.zero;
    [System.NonSerialized] public float minSpeed = 0.0f;
    [System.NonSerialized] public float maxSpeed = 0.0f;
    [System.NonSerialized] public int maxNormalBullet = 0;
    protected int speedLvl = 0;


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset() {
        speedLvl = 0;
        CancelInvoke();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartNormalBullet() {
        float interval = Random.Range(minInterval, maxInterval);
        maxNormalBullet = initMaxNormalBullet;
        minSpeed = initMinSpeed;
        maxSpeed = initMaxSpeed;
        normalBulletCount = 0;
        Invoke("SpawnANormalBullet", interval);
        // Invoke("SpawnShield", 15.0f);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopNormalBullet() {
        CancelInvoke("SpawnANormalBullet");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void UpdateSpeed() {
        speedLvl = Mathf.FloorToInt(Game.instance.timer/20.0f);
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
            posX = Random.Range(Game.instance.boundingLeft - spawnAreaMargin/2, Game.instance.boundingRight + spawnAreaMargin/2);
            posY = Game.instance.boundingTop + spawnAreaMargin/2;
        } else if (borderPicker == 1) { //spawn from right
            posX = Game.instance.boundingRight + spawnAreaMargin/2;
            posY = Random.Range(Game.instance.boundingTop - spawnAreaMargin/2, Game.instance.boundingBot + spawnAreaMargin/2);
        } else if (borderPicker == 2) { //spawn from bot
            posX = Random.Range(Game.instance.boundingLeft - spawnAreaMargin/2, Game.instance.boundingRight + spawnAreaMargin/2);
            posY = Game.instance.boundingBot - spawnAreaMargin/2;
        } else if (borderPicker == 3) { //spawn from left
            posX = Game.instance.boundingLeft - spawnAreaMargin/2;
            posY = Random.Range(Game.instance.boundingTop - spawnAreaMargin/2, Game.instance.boundingBot + spawnAreaMargin/2);
        }
 
        // if (posX > -Camera.main.pixelWidth/2 - 20.0f && posX < Camera.main.pixelWidth/2 + 20.0f) { 
        //     int rand = Random.Range(0,2);
        //     if (rand == 0) {
        //         posY = Camera.main.pixelHeight/2 + spawnAreaMargin/2;
        //     } else {
        //         posY = -Camera.main.pixelHeight/2 - spawnAreaMargin/2;
        //     }
        // } else {
        //     posY = Random.Range( -Camera.main.pixelHeight/2 - spawnAreaMargin/2, Camera.main.pixelHeight/2 + spawnAreaMargin );
        // }
        return new Vector2(posX, posY);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void SpawnANormalBullet() {
        if (normalBulletCount <= maxNormalBullet) {
            NormalBullet bullet = Game.instance.spawner.SpawnNormalBullet(GetSpawnPoint());
            bullet.Active();
            bullet.StartMoving();
            normalBulletCount++;
        }
        Invoke("SpawnANormalBullet", Random.Range(minInterval, maxInterval));
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void SpawnShield() {
        float posX = 0.0f;
        float posY = 0.0f;
        if (Game.instance.player.transform.position.x > 0) {
            posX = Random.Range(-300.0f, -250.0f);
        } else {
            posX = Random.Range(250.0f, 300.0f);
        }

        if (Game.instance.player.transform.position.y > 0) {
            posY = Game.rightBoundary;
        } else {
            posY = Screen.height/2;
        }

        PUShield shield = Game.instance.spawner.SpawnPowerUp(new Vector2(posX, posY)) as PUShield;
        shield.Active();
        shield.EnterField();

        // Invoke("SpawnShield", 15.0f);
    }

}
