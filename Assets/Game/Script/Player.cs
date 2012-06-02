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
    public float maxSpeed = 0.0f;
    public float acceleration = 0.0f;
    public float brake = 0.0f;
    public float smooth = 0.0f;

    protected bool isAcceptInput = false;

    protected Vector2 direction = Vector2.zero;
    protected Vector2 speed = Vector2.zero;
    protected Vector2 accelVector = Vector2.zero;

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
        accelVector = Vector2.zero;
        speed = Vector2.zero;
        spShip.spanim.Play("idle");
        spShip.enabled = true;
        transform.position = new Vector3(-100.0f, 0.0f,
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

    public void UpdateAccelVector( Vector2 _accel ) {
        accelVector = Vector2.Lerp(accelVector, _accel, Time.deltaTime * smooth);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	// Update is called once per frame
	void Update () {
        direction = Vector2.zero;

        // get input direction
        if (Application.isEditor && isAcceptInput ) {
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
        }

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

        speed = accelVector * maxSpeed;
        // handle movement 
        transform.Translate( speed.x, speed.y, 0.0f );

        // handle boundary
        float modX = transform.position.x;
        float modY = transform.position.y;
        if (transform.position.x > Game.rightBoundary) {
            modX = Game.rightBoundary;
        } else if (transform.position.x < -Screen.width/2) {
            modX = -Screen.width/2;
        }
            
        if (transform.position.y > Screen.height/2) {
            modY = Screen.height/2;
        } else if (transform.position.y < -Screen.height/2) {
            modY = -Screen.height/2;
        }

        transform.position = new Vector3(modX, modY,
                                             transform.position.z);	
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
