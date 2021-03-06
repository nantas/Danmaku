// ======================================================================================
// File         : Stage.cs
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

public class Stage : FSMBase {

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
    public static Stage instance = null; 
    public static float rightBoundary = -240.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // Serialized
    ///////////////////////////////////////////////////////////////////////////////
    
    public float boundingLeft; 
    public float boundingRight;
    public float boundingTop;
    public float boundingBot;

    public AudioClip sfx_explode;
    public AudioClip sfx_scratch;
    public AudioClip sfx_powerup;
    public AudioSource sfxPlayer;

    // public HighScoreBoard hsBoard;

    ///////////////////////////////////////////////////////////////////////////////
    // NonSerialized
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public int tokenCollected = 0;
    [System.NonSerialized] public Spawner spawner = null;
    [System.NonSerialized] public Player player = null; 
    [System.NonSerialized] public GamePanel gamePanel = null;
    [System.NonSerialized] public ChallengeMng challengeMng = null;

    public float timer {
        get { return timer_; }
        set {
            if (value != timer_) {
                timer_ = value;
            }
        }
    }
    protected float timer_ = 0.0f;

    public float power {
        get { return power_; }
        set {
            if (value != power_) {
                power_ = value;
            }
        }
    }
    protected float power_ = 0;


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
        mainLoop.onExit += ExitMainLoopState;
        mainLoop.mode = fsm.State.Mode.Parallel;

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
        challengeMng = FindObjectOfType (typeof(ChallengeMng)) as ChallengeMng;
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
        challengeMng.Reset();
        player.Reset();

        timer = 0.0f;
        power = 0.0f;
        tokenCollected = 0;

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

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Pause() {
        //TODO
        // stateMachine.Send( (int)EventType.Pause );
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
        Invoke ( "StartGame", 2.0f );
    }
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void ExitNoBullet( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void StartGame () {
        stateMachine.Send( fsm.Event.NEXT );
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Main Loop State
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterMainLoopState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        challengeMng.StartChallenges();
        Screen.showCursor = false;
        AcceptInput(true);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void UpdateMainLoopState ( fsm.State _curState ) {
        timer += Time.deltaTime;
        if (Mathf.FloorToInt(timer)%10 == 1) {
            challengeMng.UpdateSpeed();
        }

    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void ExitMainLoopState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        Screen.showCursor = true;
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
        challengeMng.StopChallenges();
        gamePanel.panelGameOver.ShowNamePrompt(true);
        StartCoroutine(Game.instance.snsMng.SetLeaderBoard(Mathf.FloorToInt(Stage.instance.timer)));
        StartCoroutine(Game.instance.snsMng.GetUser());
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
        power += 2.0f;
        gamePanel.OnScratchUpdate();
        player.Scratch();
        if (Time.frameCount%4 == 1) {
            StartSlowMo();
        }
        if (power >= player.maxPower) {
            power = player.maxPower;
            PowerMaxed();
        }
        sfxPlayer.PlayOneShot(sfx_scratch);
        // }
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

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void PowerMaxed() {
        gamePanel.PowerMaxed();
    }

    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
    
    public void PowerReleased() {
        power = 0.0f;
        player.StartShield(4.0f);
    }



    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void ToggleMusic() {
        AudioSource musicPlayer = GetComponent<AudioSource>();
        if (musicPlayer.mute) {
            musicPlayer.mute = false;
        } else {
            musicPlayer.mute = true;
        }
    }



}




