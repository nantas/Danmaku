// ======================================================================================
// File         : Player.cs
// Author       : nantas 
// Last Change  : 06/01/2012 | 23:32:22 PM | Friday,June
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class Player : MonoBehaviour {

    public exSprite spShip;
    public exSprite spFX;
    // public float maxSpeed = 0.0f;
    public float mapScale = 0.0f;
    // public float brake = 0.0f;
    public float smooth = 0.0f;

    protected bool isAcceptInput = false;
    [System.NonSerialized] public bool isShielded = false;

    // protected Vector2 direction = Vector2.zero;
    // protected Vector2 speed = Vector2.zero;
    // protected Vector2 accelVector = Vector2.zero;
    protected Vector3 initPlayerPos = Vector3.zero;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        spFX.enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	// Use this for initialization
	void Start () {
        spShip.spanim.Play("idle");
	}
	
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset() {
        // accelVector = Vector2.zero;
        // speed = Vector2.zero;
        spShip.spanim.Play("idle");
        spShip.enabled = true;
        spFX.enabled = false;
        isShielded = false;
        transform.position = new Vector3(0.0f, 0.0f,
                                         transform.position.z);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void AcceptInput( bool _acceptInput ) {
        isAcceptInput = _acceptInput;
        enabled = _acceptInput;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void InitMapLocation() {
        initPlayerPos = transform.position;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void UpdateInputLocation( Vector3 _mappedPos ) {
        // accelVector = Vector2.Lerp(accelVector, _accel, Time.deltaTime * smooth);
        Vector3 dist = _mappedPos * mapScale;
        Vector2 playerPos = Vector2.Lerp(transform.position, initPlayerPos + dist, Time.deltaTime * smooth);
        transform.position = new Vector3( playerPos.x, playerPos.y,
                                          transform.position.z);

        // handle boundary
        float modX = transform.position.x;
        float modY = transform.position.y;
        if (transform.position.x > Screen.width/2) {
            modX = Screen.width/2;
        } else if (transform.position.x < -Screen.width/2) {
            modX = -Screen.width/2;
        }
            
        if (transform.position.y > Screen.height/2) {
            modY = Screen.height/2;
        } else if (transform.position.y < Game.rightBoundary) {
            modY = Game.rightBoundary;
        }

        transform.position = new Vector3(modX, modY,
                                             transform.position.z);	
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	// Update is called once per frame
	void Update () {
        // direction = Vector2.zero;

        // // get input direction
        // if (Application.isEditor && isAcceptInput ) {
        //     if (Input.GetKey(KeyCode.UpArrow)) {
        //         direction += new Vector2(0.0f, 1.0f);
        //     }

        //     if (Input.GetKey(KeyCode.DownArrow)) {
        //         direction += new Vector2(0.0f, -1.0f);
        //     }

        //     if (Input.GetKey(KeyCode.LeftArrow)) {
        //         direction += new Vector2(-1.0f, 0.0f);
        //     }

        //     if (Input.GetKey(KeyCode.RightArrow)) {
        //         direction += new Vector2(1.0f, 0.0f);
        //     }
        // }

        // // calculate speed
        // if (direction != Vector2.zero) {
        //     Vector2 accel = direction * acceleration; 
        //     speed += accel * Time.deltaTime;
        //     if (speed.magnitude > maxSpeed) {
        //         speed = speed.normalized * maxSpeed;
        //     }
        // } else if (accelVector != Vector2.zero) {
        //     speed += accelVector * acceleration * Time.deltaTime;
        //     if (speed.magnitude > maxSpeed) {
        //         speed = speed.normalized * maxSpeed;
        //     }
        // } else {
        //     speed -= brake * Time.deltaTime * speed.normalized;
        //     if (speed.magnitude < 1.0f) {
        //         speed = Vector2.zero;
        //     }
        // }

        // speed = accelVector * maxSpeed;
        // // handle movement 
        // transform.Translate( speed.x, speed.y, 0.0f );


	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartShield(float _duration) {
        isShielded = true;
        spFX.enabled = true;
        spShip.GetComponent<SphereCollider>().radius = 21;
        Invoke("StopShield", _duration);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopShield() {
        isShielded = false;
        spFX.enabled = false;
        spShip.GetComponent<SphereCollider>().radius = 19;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Destroy() {
        spShip.spanim.Play("destroy");
        AcceptInput(false);
        Game.instance.GameOver();
    }

}