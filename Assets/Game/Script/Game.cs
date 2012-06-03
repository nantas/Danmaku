// ======================================================================================
// File         : Game.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 14:45:58 PM | Saturday,June
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

public class Game : FSMBase {

    ///////////////////////////////////////////////////////////////////////////////
    // EventType
    ///////////////////////////////////////////////////////////////////////////////

    public enum EventType {
        GameOver = fsm.Event.USER_FIELD + 1,
        Reset,
        Restart,
        Resume,
        Pause
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Static
    ///////////////////////////////////////////////////////////////////////////////
    public static Game instance = null; 
    public static float rightBoundary = -240.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // Serialized
    ///////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////////////
    // NonSerialized
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public Spawner spawner = null;
    [System.NonSerialized] public Player player = null; 
    [System.NonSerialized] public GamePanel gamePanel = null;
    [System.NonSerialized] public BulletMng bulletMng = null;

    public float timer {
        get { return timer_; }
        set {
            if (value != timer_) {
                timer_ = value;
            }
        }
    }
    protected float timer_ = 0.0f;

    public int scratch {
        get { return scratch_; }
        set {
            if (value != scratch_) {
                scratch_ = value;
            }
        }
    }
    protected int scratch_ = 0;


    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void InitStateMachine () {

        // ======================================================== 
        // init states 
        // ======================================================== 

        fsm.State getReady = new fsm.State( "getReady", stateMachine );
        getReady.onEnter += EnterGetReady;

        fsm.State noBullet = new fsm.State( "noBullet", stateMachine );
        noBullet.onEnter += EnterNoBullet;
        noBullet.onExit += ExitNoBullet;

        fsm.State mainLoop = new fsm.State( "mainLoop", stateMachine );
        mainLoop.onEnter += EnterMainLoopState;
        mainLoop.onAction += UpdateMainLoopState;
        mainLoop.mode = fsm.State.Mode.Parallel;

            fsm.State normalBullet = new fsm.State( "normalBullet", mainLoop );
            normalBullet.onAction += UpdateNormalBullet;

        fsm.State pause = new fsm.State( "pause", stateMachine );
        pause.onEnter += EnterPauseState;
        pause.onExit += ExitPauseState;

        fsm.State gameOver = new fsm.State( "gameOver", stateMachine );
        gameOver.onEnter += EnterGameOverState;
        gameOver.onAction += UpdateGameOverState;

        fsm.State restart = new fsm.State( "restart", stateMachine );
        restart.onEnter += EnterRestartState;
        restart.onExit += ExitRestartState;

        // ======================================================== 
        // setup transitions 
        // ======================================================== 

        getReady.Add<fsm.EventTransition> ( noBullet, fsm.Event.NEXT );

        noBullet.Add<fsm.EventTransition> ( mainLoop, fsm.Event.NEXT );

        mainLoop.Add<fsm.EventTransition> ( gameOver, (int)EventType.GameOver );
        mainLoop.Add<fsm.EventTransition> ( restart, (int)EventType.Reset );
        mainLoop.Add<fsm.EventTransition> ( pause, (int)EventType.Pause );

        pause.Add<fsm.EventTransition> ( mainLoop, (int)EventType.Resume );
        pause.Add<fsm.EventTransition> ( restart, (int)EventType.Reset );

        gameOver.Add<fsm.EventTransition> ( restart, (int)EventType.Reset );

        restart.Add<fsm.EventTransition> ( getReady, (int)EventType.Restart );

        //
        stateMachine.initState = getReady;
        // stateMachine.logDebugInfo = true;

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected new void Awake() {
        base.Awake();

        Application.targetFrameRate = 60;

        if (instance == null) {
            instance = this;
        }

        spawner = GetComponent<Spawner>();
        player = FindObjectOfType( typeof (Player) ) as Player;
        gamePanel = FindObjectOfType (typeof(GamePanel)) as GamePanel;
        if ( gamePanel == null ) {
            Debug.LogError ( "Can't find GamePanel in the scene" );
        }
        bulletMng = FindObjectOfType (typeof(BulletMng)) as BulletMng;
        spawner.Init();

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

    public override void Reset() {
        spawner.Reset();
        gamePanel.Reset();
        bulletMng.Reset();
        player.Reset();

        timer = 0.0f;
        scratch = 0;

        System.GC.Collect();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Update () {
        if ( stateMachine != null )
            stateMachine.Update();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void GameOver() {
        stateMachine.Send( (int)EventType.GameOver );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Restart() {
        stateMachine.Send( (int)EventType.Reset );
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterGetReady( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        if (_event.id == (int)EventType.Restart) {
            Reset();
        }

        StartCoroutine(gamePanel.ShowStart());
        AcceptInput(false);
        stateMachine.Send( fsm.Event.NEXT );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void EnterNoBullet( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        StartCoroutine(StartGame());
    }
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void ExitNoBullet( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected IEnumerator StartGame () {
        yield return new WaitForSeconds(2.0f);
        stateMachine.Send( fsm.Event.NEXT );
        bulletMng.StartNormalBullet();
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Main Loop State
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterMainLoopState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        AcceptInput(true);

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void UpdateMainLoopState ( fsm.State _curState ) {
        timer += Time.deltaTime;
        if (Time.frameCount%60 == 1) {
            bulletMng.UpdateSpeed();
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void UpdateNormalBullet ( fsm.State _curState ) {
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Pause state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void EnterPauseState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        AcceptInput(false);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void ExitPauseState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        AcceptInput(true);
    }

    ///////////////////////////////////////////////////////////////////////////////
    // GameOver state 
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void EnterGameOverState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        AcceptInput(false);
        gamePanel.ShowGameOver(); 
        bulletMng.StopNormalBullet();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void UpdateGameOverState ( fsm.State _curState ) {
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Restart state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void EnterRestartState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        Reset();
        stateMachine.Send( (int) EventType.Restart );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void ExitRestartState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        gamePanel.HideGameOver();
    }


    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void AcceptInput ( bool _accept ) {
        exUIPanel panelSelf = gamePanel.GetComponent<exUIPanel>();
        gamePanel.enabled = _accept;
        panelSelf.enabled = _accept;
        player.AcceptInput(_accept);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Scratch() {
        // Debug.Log("Scratch!");
        scratch += 1;
        gamePanel.OnScratchUpdate();
        if (Time.frameCount%2 == 1) {
            StartSlowMo();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void StartSlowMo() {
        Time.timeScale = 0.5f;
        Invoke("StopSlowMo", 0.5f);
        // isSlowMo = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void StopSlowMo() {
        Time.timeScale = 1.0f;
        // isSlowMo = false;
    }
}




