// ======================================================================================
// File         : BulletInfo.cs
// Author       : Wang Nan 
// Last Change  : 07/03/2012 | 23:06:32 PM | Tuesday,July
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
//
///////////////////////////////////////////////////////////////////////////////

public class BulletInfo : ScriptableObject {

    public Danmaku.BulletType type = Danmaku.BulletType.Unknown; // DEBUG

    public float maxDuration = 0.0f; // set to -1 if there's no time limit 
    public int reps = 0; // should the pattern repeat itself if set to positive value
    public float speed = 0.0f; // can be override by individual bullet mng
    public float startTime = 0.0f; // if the pattern automatically starts at one point

}

