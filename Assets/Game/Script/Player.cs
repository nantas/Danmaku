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
    public float maxPower = 10.0f;
    // public float maxSpeed = 0.0f;
    public float mapScale = 0.0f;
    // public float brake = 0.0f;
    public float damping = 0.0f;

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
        spShip.spanim.Play("move");
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
        transform.position = new Vector3(Game.instance.transform.position.x,
                                         Game.instance.transform.position.y,
                                         transform.position.z);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void AcceptInput( bool _acceptInput ) {
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
        // accelVector = Vector2.Lerp(accelVector, _accel, Time.deltaTime * damping);
        Vector3 dist = _mappedPos * mapScale;
        Vector2 playerPos = Vector2.Lerp(transform.position, initPlayerPos + dist, (Time.deltaTime / Time.timeScale) * damping );
        transform.position = new Vector3( playerPos.x, playerPos.y,
                                          transform.position.z);

        // handle boundary
        float modX = transform.position.x;
        float modY = transform.position.y;
        if (transform.position.x > Game.instance.boundingRight) {
            modX = Game.instance.boundingRight;
        } else if (transform.position.x < Game.instance.boundingLeft) {
            modX = Game.instance.boundingLeft;
        }
            
        if (transform.position.y > Game.instance.boundingTop) {
            modY = Game.instance.boundingTop;
        } else if (transform.position.y < Game.instance.boundingBot) {
            modY = Game.instance.boundingBot;
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
        GetComponent<SphereCollider>().enabled = false;
        spShip.GetComponent<SphereCollider>().enabled = true;
        Invoke("StopShield", _duration);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopShield() {
        isShielded = false;
        spFX.enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        spShip.GetComponent<SphereCollider>().enabled = false;
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
