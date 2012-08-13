// ======================================================================================
// File         : ChallengeMng.cs
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

public class ChallengeMng: MonoBehaviour {


    public class BulletPatternInfo {
        public BulletType type = BulletType.Unknown;
        public int repeat = 0;
    }


    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset() {
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopAllBullets() {
        CancelInvoke();
        StopAllCoroutines();
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
