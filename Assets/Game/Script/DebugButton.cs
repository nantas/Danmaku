// ======================================================================================
// File         : DebugButton.cs
// Author       : nantas 
// Last Change  : 02/11/2012 | 00:11:02 AM | Saturday,February
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class DebugButton: MonoBehaviour {

    public enum ValueControl {
        Accel,
        Brake,
        SpeedModifier,
        Smooth
    }


    public ValueControl valueControl;

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonPress += OnButtonPress;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonPress () {


    }

}


