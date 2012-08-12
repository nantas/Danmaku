
// ======================================================================================
// File         : BGScroller.cs
// Author       : nantas 
// Last Change  : 05/14/2012 | 23:19:13 PM | Monday,May
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

public class BGScroller: MonoBehaviour {

    public GameObject[] tileRows;
    public float moveSpeed;
    
    private float botLinePosY = -320.0f;
    private bool isMovingDown = false;
    private float tileHeight = 0.0f;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake() {
        botLinePosY = tileRows[0].transform.position.y;
        tileHeight = tileRows[0].GetComponentInChildren<exSprite>().height;
        isMovingDown = true;
    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start() {
        for (int i = 0; i < tileRows.Length; ++i) {
            GameObject go = tileRows[i];
            exSprite[] sprites = go.GetComponentsInChildren<exSprite>();
            for (int j = 0; j < sprites.Length; ++j) {
                sprites[i].spanim.Play("bg");
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // public void StartMoveUp() {
    //     isMovingUp = true;
    // }

    // // ------------------------------------------------------------------ 
    // // Desc: 
    // // ------------------------------------------------------------------ 

    // public void StopMoveUp() {
    //     isMovingUp = false;
    // }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        if (isMovingDown) {
            float step = moveSpeed * Time.smoothDeltaTime;
            // do step
            foreach (GameObject row in tileRows) {
                row.transform.Translate( 0.0f, step, 0.0f );
            }
            //check topmost row position.
            if (tileRows[0].transform.position.y < botLinePosY) { //switch position
                RecycleRows();
            }
        }
    }
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void RecycleRows() {
        GameObject tempObj = tileRows[0]; //put the first row into temp obj.
        for (int i = 1; i < tileRows.Length; ++i) {
            tileRows[i-1] = tileRows[i];
        }
        tileRows[tileRows.Length-1] = tempObj;
        tileRows[tileRows.Length-1].transform.position = new Vector3( tileRows[tileRows.Length-2].transform.position.x,
                                                                    tileRows[tileRows.Length-2].transform.position.y + tileHeight,
                                                                    tileRows[tileRows.Length-1].transform.position.z );
    }

}
