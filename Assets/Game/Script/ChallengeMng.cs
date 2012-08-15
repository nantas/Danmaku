// ======================================================================================
// File         : ChallengeMng.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 15:47:25 PM | Saturday,June
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class ChallengeMng: MonoBehaviour {


    public BulletInfo[] bulletInfos;
    public float spawnAreaMargin = 0.0f;
    [System.NonSerialized] public BulletMng[] bulletMngs;
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        // initialize bulletMngs
        bulletMngs = new BulletMng[bulletInfos.Length];
        for (int i = 0; i < bulletInfos.Length; ++i) {
            string bulletName = bulletInfos[i].type.ToString().ToLower();
            string path = "prefab/bulletmng/" + bulletName;
            GameObject goPrefab = Resources.Load( path, typeof(GameObject) ) as GameObject;
            GameObject go = GameObject.Instantiate( goPrefab, Vector3.zero, Quaternion.identity ) as GameObject; 
            go.transform.parent = transform;
            bulletMngs[i] = go.GetComponent<BulletMng>();
        }
        Init();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init() {
        for (int i = 0; i < bulletMngs.Length; ++i) {
            bulletMngs[i].Init(bulletInfos[i]);
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset() {
        for (int i = 0; i < bulletMngs.Length; ++i) {
            bulletMngs[i].Reset();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartChallenges() {
        System.GC.Collect();
        for (int i = 0; i < bulletMngs.Length; ++i) {
            bulletMngs[i].StartStateMachine();
        }
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopChallenges() {
        CancelInvoke();
        StopAllCoroutines();
        for (int i = 0; i < bulletMngs.Length; ++i) {
            bulletMngs[i].StopBullets();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void UpdateSpeed() {
        for (int i = 0; i < bulletMngs.Length; ++i) {
            bulletMngs[i].UpdateSpeed();
        }
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

}
