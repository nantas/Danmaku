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

public class BulletMng: FSMBase {

    ///////////////////////////////////////////////////////////////////////////////
    // EventType
    ///////////////////////////////////////////////////////////////////////////////

    public enum EventType {
        Sleep, = fsm.Event.USER_FIELD + 1,
        Prepare,
        Run,
        Finish,
        Pause,
        Resume
    }


    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public BulletInfo bulletInfo;
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void InitStateMachine () {

        // ======================================================== 
        // init states 
        // ======================================================== 

        fsm.State sleep = new fsm.State( "sleep", stateMachine );
        sleep.onEnter += EnterSleep;

        fsm.State prepare = new fsm.State( "prepare", stateMachine );
        prepare.onEnter += EnterPrepare;
        prepare.onExit += ExitPrepare;

        fsm.State run = new fsm.State( "run", stateMachine );
        run.onEnter += EnterRun;
        run.onAction += UpdateRun;

        fsm.State finish = new fsm.State( "finish", stateMachine );
        finish.onEnter += EnterFinish;
        finish.onAction += UpdateFinish;

        fsm.State pause = new fsm.State( "pause", stateMachine );
        pause.onEnter += EnterPause;
        pause.onExit += ExitPause;

        // ======================================================== 
        // setup transitions 
        // ======================================================== 

        sleep.Add<fsm.EventTransition> ( prepare, (int)EventType.Prepare );

        prepare.Add<fsm.EventTransition> ( run, (int)EventType.Run );

        run.Add<fsm.EventTransition> ( finish, (int)EventType.Finish );
        run.Add<fsm.EventTransition> ( sleep, (int)EventType.Sleep );
        run.Add<fsm.EventTransition> ( pause, (int)EventType.Pause );

        pause.Add<fsm.EventTransition> ( run, (int)EventType.Resume );
        pause.Add<fsm.EventTransition> ( sleep, (int)EventType.Sleep );

        //
        stateMachine.initState = sleep;
        // stateMachine.logDebugInfo = true;

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Start() {
        System.GC.Collect();
        if (stateMachine != null) {
            stateMachine.Start();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void Reset() {
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void StopBullets() {
        CancelInvoke();
        StopAllCoroutines();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // void SpawnShield() {
    //     float posX = 0.0f;
    //     float posY = 0.0f;
    //     if (Stage.instance.player.transform.position.x > 0) {
    //         posX = Random.Range(-300.0f, -250.0f);
    //     } else {
    //         posX = Random.Range(250.0f, 300.0f);
    //     }

    //     if (Stage.instance.player.transform.position.y > 0) {
    //         posY = Stage.rightBoundary;
    //     } else {
    //         posY = Screen.height/2;
    //     }

    //     PUShield shield = Stage.instance.spawner.SpawnPowerUp(new Vector2(posX, posY)) as PUShield;
    //     shield.Active();
    //     shield.EnterField();

    //     // Invoke("SpawnShield", 15.0f);
    // }

    ///////////////////////////////////////////////////////////////////////////////
    // states
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterSleep ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void EnterPrepare( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void ExitPrepare( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Main Loop State
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterRun ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void UpdateRun ( fsm.State _curState ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterFinish ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateFinish ( fsm.State _curState ) {
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Pause state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void EnterPause ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void ExitPause ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }
}
