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

    protected float spawnAreaMargin = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // EventType
    ///////////////////////////////////////////////////////////////////////////////

    public enum EventType {
        Sleep = fsm.Event.USER_FIELD + 1,
        Prepare,
        Run,
        Finish,
        Deactive,
        Pause,
        Resume
    }


    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public Danmaku.BulletType type = Danmaku.BulletType.Unknown; // DEBUG
    [System.NonSerialized] public float maxDuration = 0.0f; // set to -1 if there's no time limit 
    [System.NonSerialized] public int reps = 0; // should the pattern repeat itself if set to positive value
    [System.NonSerialized] public float speed = 0.0f; // can be override by individual bullet mng
    [System.NonSerialized] public float startTime = 0.0f; // if the pattern automatically starts at one poin
    
    protected float sleepTimer = 0.0f;
    protected float interval = 0.0f; // how often we run spawn bullet function in the run loop
    protected float spawnTimer = 0.0f; // timer for spawn bullet function

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
        sleep.onAction += UpdateSleep;

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

        fsm.State deactive = new fsm.State( "deactive", stateMachine );
        deactive.onEnter += EnterDeactive;

        // ======================================================== 
        // setup transitions 
        // ======================================================== 

        sleep.Add<fsm.EventTransition> ( prepare, (int)EventType.Prepare );
        sleep.Add<fsm.EventTransition> ( deactive, (int)EventType.Deactive );

        prepare.Add<fsm.EventTransition> ( run, (int)EventType.Run );
        prepare.Add<fsm.EventTransition> ( deactive, (int)EventType.Deactive );

        run.Add<fsm.EventTransition> ( finish, (int)EventType.Finish );
        run.Add<fsm.EventTransition> ( deactive, (int)EventType.Deactive );
        run.Add<fsm.EventTransition> ( pause, (int)EventType.Pause );

        finish.Add<fsm.EventTransition> ( deactive, (int)EventType.Deactive );

        pause.Add<fsm.EventTransition> ( run, (int)EventType.Resume );
        pause.Add<fsm.EventTransition> ( sleep, (int)EventType.Sleep );

        deactive.Add<fsm.EventTransition> ( sleep, (int)EventType.Sleep );

        //
        stateMachine.initState = sleep;
        // stateMachine.logDebugInfo = true;

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected new void Awake() {
        base.Awake();
        spawnAreaMargin = Stage.instance.challengeMng.spawnAreaMargin;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init( BulletInfo _info ) {
        type = _info.type;
        maxDuration = _info.maxDuration;
        reps = _info.reps;
        speed = _info.speed;
        startTime = _info.startTime;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartStateMachine() {
        if (stateMachine != null) {
            stateMachine.Start();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void Reset() {
        spawnTimer = 0.0f;
        sleepTimer = 0.0f;
        CancelInvoke();
        StopAllCoroutines();
        stateMachine.Send( (int)EventType.Sleep );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void StopBullets() {
        CancelInvoke();
        StopAllCoroutines();
        stateMachine.Send( (int)EventType.Deactive );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void StartBullets() {
        stateMachine.Send( (int)EventType.Prepare );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void UpdateSpeed() {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Update () {
        if ( stateMachine != null )
            stateMachine.Update();
    }

    ///////////////////////////////////////////////////////////////////////////////
    // states
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterSleep ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        sleepTimer = 0.0f;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateSleep ( fsm.State _curState ) {
        if (startTime > 0.0f) {
            sleepTimer += Time.deltaTime;
            if (sleepTimer > startTime) {
                StartBullets();
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void EnterPrepare( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        stateMachine.Send( (int)EventType.Run );
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

    protected virtual void EnterRun ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void UpdateRun ( fsm.State _curState ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterFinish ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        stateMachine.Send( (int) EventType.Deactive);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateFinish ( fsm.State _curState ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterDeactive ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
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
