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


    ///////////////////////////////////////////////////////////////////////////////
    // trident bullet
    ///////////////////////////////////////////////////////////////////////////////

    public int repeatTimes = 0;
    protected int currentCount = 0;
    


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
        currentCount = 0;
        CancelInvoke();
        StopAllCoroutines();
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
        Invoke("SpawnTridentPattern_CO", 10.0f);
        // Invoke("SpawnShield", 15.0f);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopBullets() {
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void UpdateSpeed() {
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
            NormalBullet bullet = Stage.instance.spawner.SpawnNormalBullet(GetSpawnPoint());
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
        if (Stage.instance.player.transform.position.x > 0) {
            posX = Random.Range(-300.0f, -250.0f);
        } else {
            posX = Random.Range(250.0f, 300.0f);
        }

        if (Stage.instance.player.transform.position.y > 0) {
            posY = Stage.rightBoundary;
        } else {
            posY = Screen.height/2;
        }

        PUShield shield = Stage.instance.spawner.SpawnPowerUp(new Vector2(posX, posY)) as PUShield;
        shield.Active();
        shield.EnterField();

        // Invoke("SpawnShield", 15.0f);
    }


    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void SpawnTridentPattern_CO() {
        StartCoroutine("SpawnTridentPattern");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public IEnumerator SpawnTridentPattern() {
        Vector2 startPos = new Vector2( Stage.instance.transform.position.x - 200.0f, Stage.instance.transform.position.y + 300.0f );
        currentCount = 0;
        while (currentCount < repeatTimes) {
            yield return new WaitForSeconds(0.2f);
            SpawnATridentBulletAt(startPos);
            currentCount++;
        }
        yield return new WaitForSeconds(0.5f);
        startPos = new Vector2( Stage.instance.transform.position.x, Stage.instance.transform.position.y + 300.0f );
        currentCount = 0;
        while (currentCount < repeatTimes) {
            yield return new WaitForSeconds(0.2f);
            SpawnATridentBulletAt(startPos);
            currentCount++;
        }
        yield return new WaitForSeconds(0.5f);
        startPos = new Vector2( Stage.instance.transform.position.x + 200.0f, Stage.instance.transform.position.y + 300.0f );
        currentCount = 0;
        while (currentCount < repeatTimes) {
            yield return new WaitForSeconds(0.2f);
            SpawnATridentBulletAt(startPos);
            currentCount++;
        }        
        Invoke("SpawnTridentPattern_CO", 20.0f);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void SpawnATridentBulletAt(Vector2 _startPos) {
        for (int i = 0; i < 3; ++i) {
            FastBullet bullet = Stage.instance.spawner.SpawnFastBullet(_startPos);
            bullet.Active();
            bullet.MoveWithDirection( new Vector2( i-1, -1) );
        }
    }

}
