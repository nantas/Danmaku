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
    public float mapScale = 0.0f;
    public float damping = 0.0f;

    // keyboard control support
    public float acceleration = 0.0f;
    public float brake = 0.0f;
    public float maxSpeed = 0.0f;

    [System.NonSerialized] public bool isShielded = false;

    // keyboard control support
    protected Vector2 direction = Vector2.zero;
    protected Vector2 speed = Vector2.zero;
    protected Vector2 accelVector = Vector2.zero;

    protected Vector3 initPlayerPos = Vector3.zero;
    protected bool isShieldBlink;
    protected float initMapScale = 0.0f;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        spFX.enabled = false;
        initMapScale = mapScale;
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
        accelVector = Vector2.zero;
        speed = Vector2.zero;
        // spShip.spanim.Play("idle");
        spShip.spanim.Play("move");
        spShip.enabled = true;
        spFX.enabled = false;
        isShielded = false;
        isShieldBlink = false;
        transform.position = new Vector3(Stage.instance.transform.position.x,
                                         Stage.instance.transform.position.y,
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
        if (transform.position.x > Stage.instance.boundingRight) {
            modX = Stage.instance.boundingRight;
        } else if (transform.position.x < Stage.instance.boundingLeft) {
            modX = Stage.instance.boundingLeft;
        }
            
        if (transform.position.y > Stage.instance.boundingTop) {
            modY = Stage.instance.boundingTop;
        } else if (transform.position.y < Stage.instance.boundingBot) {
            modY = Stage.instance.boundingBot;
        }

        transform.position = new Vector3(modX, modY,
                                             transform.position.z);	
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        direction = Vector2.zero;

        // get input direction
        // if (Application.isEditor && isAcceptInput ) {
            if (Input.GetKey(KeyCode.UpArrow)) {
                direction += new Vector2(0.0f, 1.0f);
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                direction += new Vector2(0.0f, -1.0f);
            }

            if (Input.GetKey(KeyCode.LeftArrow)) {
                direction += new Vector2(-1.0f, 0.0f);
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                direction += new Vector2(1.0f, 0.0f);
            }
        // }

        // calculate speed
        if (direction != Vector2.zero) {
            Vector2 accel = direction * acceleration; 
            speed += accel * Time.deltaTime;
            if (speed.magnitude > maxSpeed) {
                speed = speed.normalized * maxSpeed;
            }
        // } else if (accelVector != Vector2.zero) {
        //     speed += accelVector * acceleration * Time.deltaTime;
        //     if (speed.magnitude > maxSpeed) {
        //         speed = speed.normalized * maxSpeed;
        //     }
        } else {
            speed -= brake * Time.deltaTime * speed.normalized;
            if (speed.magnitude < 1.0f) {
                speed = Vector2.zero;
            }
        }

        // speed = accelVector * maxSpeed;
        // handle movement 
        transform.Translate( speed.x, speed.y, 0.0f );


    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void LateUpdate() {
        if (isShieldBlink) {
            if (Time.frameCount%20 == 1) {
                spFX.enabled = false;
            } else if (Time.frameCount%20 == 11) {
                spFX.enabled = true;
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Scratch() {
        spFX.enabled = true;
        spFX.spanim.Play("scratch");
        if (isShielded)
            spFX.spanim.Play("shield");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StartShield(float _duration) {
        isShielded = true;
        isShieldBlink = false;
        spFX.enabled = true;
        spFX.spanim.Play("shield");
        Stage.instance.sfxPlayer.PlayOneShot(Stage.instance.sfx_powerup);
        GetComponent<SphereCollider>().enabled = false;
        // spShip.GetComponent<SphereCollider>().enabled = true;
        Invoke("WarningShield", _duration);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void WarningShield() {
        isShieldBlink = true;
        Invoke("StopShield", 1.5f);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void StopShield() {
        isShielded = false;
        isShieldBlink = false;
        spFX.enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        // spShip.GetComponent<SphereCollider>().enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Destroy() {
        spShip.spanim.Play("destroy");
        AcceptInput(false);
        Stage.instance.sfxPlayer.PlayOneShot(Stage.instance.sfx_explode);
        Stage.instance.GameOver();
    }

}
