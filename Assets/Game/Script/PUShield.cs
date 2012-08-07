// ======================================================================================
// File         : PUShield.cs
// Author       : nantas 
// Last Change  : 06/02/2012 | 16:28:16 PM | Saturday,June
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

public class PUShield: PowerUp {

    public float duration = 0.0f;


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void OnTriggerEnter( Collider _other ) {
        if (_other.gameObject.tag == "Player") {
            _other.GetComponent<Player>().StartShield(duration);
            Stage.instance.spawner.DestroyPowerUp(this);
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void EnterField () {
        if (transform.position.y > 0) {
            float vX = Random.Range(-5.0f, 5.0f);
            velocity = new Vector2( vX, -moveSpeed );
        } else {
            float vX = Random.Range(-5.0f, 5.0f);
            velocity = new Vector2( vX, moveSpeed );
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update() {
        Vector2 dist = velocity * Time.deltaTime;
        transform.Translate(dist.x, dist.y, 0);

        //handle bump
        bool hasBump = false;
        float modX = transform.position.x;
        float modY = transform.position.y;

        if (transform.position.x > Screen.width/2) {
            modX = Screen.width/2;
            velocity = new Vector2 ( -velocity.x, velocity.y);
            hasBump = true;
        } else if (transform.position.x < -Screen.width/2) {
            modX = -Screen.width/2;
            velocity = new Vector2 ( -velocity.x, velocity.y);
            hasBump = true;
        }
            
        if (transform.position.y > Screen.height/2) {
            modY = Screen.height/2;
            velocity = new Vector2( velocity.x, -velocity.y );
            hasBump = true;
        } else if (transform.position.y < Stage.rightBoundary) {
            modY = Stage.rightBoundary;
            velocity = new Vector2( velocity.x, -velocity.y );
            hasBump = true;
        }

        transform.position = new Vector3(modX, modY,
                                             transform.position.z);	

        if (hasBump) {
            bumpCount++;
            if (bumpCount > maxBumpCount) {
                Stage.instance.spawner.DestroyPowerUp(this);
            }
        }

    }

}


