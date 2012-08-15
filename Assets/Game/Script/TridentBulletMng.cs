// ======================================================================================
// File         : TridentBulletMng.cs
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

public class TridentBulletMng: BulletMng {

    ///////////////////////////////////////////////////////////////////////////////
    // trident bullet
    ///////////////////////////////////////////////////////////////////////////////

    public int maxBulletCount = 0; // in each rep, how many bullets going to be fired
    
    protected int currentRepCount = 0; // 
    protected int currentBulletCount = 0; // current bullet count in current rep
    protected Vector2 startPos = Vector2.zero;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void Reset() {
        base.Reset();
        currentBulletCount = 0;
        currentRepCount = 0;
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void StartBullets() {
        interval = 0.2f;
        base.StartBullets();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // public IEnumerator SpawnTridentPattern() {
    //     Vector2 startPos = new Vector2( Stage.instance.transform.position.x - 200.0f, Stage.instance.transform.position.y + 300.0f );
    //     currentBulletCount = 0;
    //     while (currentBulletCount < maxBulletCount) {
    //         yield return new WaitForSeconds(0.2f);
    //         SpawnATridentBulletAt(startPos);
    //         currentBulletCount++;
    //     }
    //     yield return new WaitForSeconds(0.5f);
    //     startPos = new Vector2( Stage.instance.transform.position.x, Stage.instance.transform.position.y + 300.0f );
    //     currentBulletCount = 0;
    //     while (currentBulletCount < maxBulletCount) {
    //         yield return new WaitForSeconds(0.2f);
    //         SpawnATridentBulletAt(startPos);
    //         currentBulletCount++;
    //     }
    //     yield return new WaitForSeconds(0.5f);
    //     startPos = new Vector2( Stage.instance.transform.position.x + 200.0f, Stage.instance.transform.position.y + 300.0f );
    //     currentBulletCount = 0;
    //     while (currentBulletCount < maxBulletCount) {
    //         yield return new WaitForSeconds(0.2f);
    //         SpawnATridentBulletAt(startPos);
    //         currentBulletCount++;
    //     }        
    //     Invoke("SpawnTridentPattern_CO", 20.0f);
    // }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void SpawnATridentBulletAt(Vector2 _startPos) {
        for (int i = 0; i < 3; ++i) {
            FastBullet bullet = Stage.instance.spawner.SpawnFastBullet(_startPos);
            bullet.Init(this);
            bullet.Active();
            bullet.bulletSpeed = speed;
            bullet.MoveWithDirection( new Vector2( i-1, -1) );
        }
        currentBulletCount++;
        // Invoke("SpawnATridentBullet", 0.2f);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void StartARound() {
        if (currentRepCount <= reps) {
            currentBulletCount = 0;
            startPos = new Vector2( Stage.instance.transform.position.x + 200.0f * (currentRepCount%3 - 1),
                                    Stage.instance.transform.position.y + 300.0f); //HARDCODE
        } else {
            stateMachine.Send( (int) EventType.Finish );
        }
    }


    ///////////////////////////////////////////////////////////////////////////////
    // Main Loop State
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void EnterRun ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        base.EnterRun( _from, _to, _event );
        StartARound();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void UpdateRun ( fsm.State _curState ) {
        base.UpdateRun(_curState);
        spawnTimer += Time.deltaTime;
        if (spawnTimer > interval) {
            if (currentBulletCount < maxBulletCount) {
                SpawnATridentBulletAt(startPos);
                spawnTimer = 0.0f;
            } else {
                currentRepCount++;
                StartARound();
                spawnTimer = 0.0f;
            }
        }
    }

}
