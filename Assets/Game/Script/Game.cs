// ======================================================================================
// File         : Game.cs
// Author       : Wu Jie 
// Last Change  : 06/20/2012 | 01:28:18 AM | Wednesday,June
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// class AssetMng
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class Game : FSMBase {

    public enum EventType {
        LoadStage = fsm.Event.USER_FIELD + 1,
        ShowStage,
        QuitApp,
    }

    public class LoadStageEvent : fsm.Event {
        public string scene;

        public LoadStageEvent ( string _sceneName ) 
            : base ( (int)EventType.LoadStage )
        {
            scene = _sceneName;
        }
    } 

    ///////////////////////////////////////////////////////////////////////////////
    // static variables
    ///////////////////////////////////////////////////////////////////////////////

    public static Game instance = null; 
    public static GlobalSettings settings = null; 
    public static bool isTestStage { get { return instance == null; } }

    [System.NonSerialized] public SNSMng snsMng;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void LoadSettings () {
        if ( settings == null ) {
            GameObject settingsPrefab = Resources.Load ( "prefab/settings", typeof(GameObject) ) as GameObject;
            GameObject settingsGO = GameObject.Instantiate( settingsPrefab, Vector3.zero, Quaternion.identity ) as GameObject;
            settings = settingsGO.GetComponent<GlobalSettings>();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    static public void LoadStage ( string _sceneName ) {
        if ( isTestStage ) {
            Debug.LogWarning ( "You can not change stage in test stage environment" );
        }
        else {
            instance.stateMachine.Send( new LoadStageEvent(_sceneName) );
        }
    } 

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    // public LoadingIndicator loadingIndicator;

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

        fsm.State launch = new fsm.State( "launch", stateMachine );
        launch.onEnter += EnterLaunchState;
        launch.onExit += ExitLaunchState;

        fsm.State loading = new fsm.State( "loading", stateMachine );
        loading.onEnter += EnterLoadingState;
        loading.onExit += ExitLoadingState;

        fsm.State stage = new fsm.State( "stage", stateMachine );
        stage.onEnter += EnterStageState;
        stage.onExit += ExitStageState;

        // ======================================================== 
        // setup transitions 
        // ======================================================== 

        launch.Add<fsm.EventTransition> ( stage, (int)EventType.ShowStage );

        loading.Add<fsm.EventTransition> ( stage, (int)EventType.ShowStage );

        stage.Add<fsm.EventTransition> ( loading, (int)EventType.LoadStage );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected new void Awake () {
        if ( instance != null )
            return;

        instance = this;
        snsMng = GetComponent<SNSMng>();
        LoadSettings();

        //
        // loadingIndicator.gameObject.SetActiveRecursively(false);

        //
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        stateMachine.Start();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        stateMachine.Update();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnApplicationQuit () {
        if ( Stage.instance != null ) {
            Stage.instance.Pause();
            // Stage.instance.SaveBattle(); // TODO: still have problem when saving GridStatus
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // launch state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterLaunchState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        Application.LoadLevel( "MainMenu" );
        stateMachine.Send( (int)EventType.ShowStage );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitLaunchState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    ///////////////////////////////////////////////////////////////////////////////
    // loading state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterLoadingState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        LoadStageEvent e = _event as LoadStageEvent;
        if ( e != null ) {
            StartCoroutine( StartLoading( e.scene, 1.0f ) );
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitLoadingState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        // loadingIndicator.gameObject.SetActiveRecursively(false);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator StartLoading( string _sceneName, float _delay ) {
        // loadingIndicator.gameObject.SetActiveRecursively(true);
        // Animation animLoading = loadingIndicator.animation;
        // animLoading.Play("loadingShowUp");

        yield return new WaitForSeconds(_delay);

        Application.LoadLevel(_sceneName);
        stateMachine.Send( (int)EventType.ShowStage );
    }

    ///////////////////////////////////////////////////////////////////////////////
    // stage state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator DelayInit () {
        yield return 1; // NOTE: this will wait for all scene Awake been called, so that loadingIndicator can get Camera.main
        // loadingIndicator.Init();
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterStageState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        StartCoroutine( DelayInit() );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitStageState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }
}


