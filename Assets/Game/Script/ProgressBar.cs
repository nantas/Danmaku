// ======================================================================================
// File         : ProgressBar.cs
// Author       : Wu Jie 
// Last Change  : 07/10/2012 | 01:39:53 AM | Tuesday,July
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
///
/// ProgressBar
///
///////////////////////////////////////////////////////////////////////////////

public class ProgressBar : MonoBehaviour {

    public enum RenderType {
        Clipping,
        Sprite,
        Border
    }

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    public bool isHorizontal = true;
    public RenderType renderType = RenderType.Clipping;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [SerializeField] float ratio_ = 1.0f;
    public float ratio {
        get { return ratio_; }
        set {
            ratio_ = Mathf.Clamp( ratio_, 0.0f, 1.0f );
            if ( ratio_ != value ) {
                ratio_ = value;
                float result = total * ratio_;

                if ( renderType == RenderType.Clipping ) {
                    if ( isHorizontal )
                        clipPlane.width = result;
                    else
                        clipPlane.height = result;
                }
                else if ( renderType == RenderType.Sprite ) {
                    if ( isHorizontal )
                        sprite.width = result;
                    else
                        sprite.height = result;
                }
                else {
                    if ( isHorizontal )
                        border.width = result;
                    else
                        border.height = result;
                }
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    exClipping clipPlane;
    exSprite sprite;
    exSpriteBorder border;
    float total = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake () {
        if ( renderType == RenderType.Clipping ) {
            clipPlane = GetComponent<exClipping>();
            if (isHorizontal) {
                total = clipPlane.width;
                clipPlane.width = total * ratio_;
            } else {
                total = clipPlane.height;
                clipPlane.height = total * ratio_;
            }
        }
        else if ( renderType == RenderType.Sprite ) {
            sprite = GetComponent<exSprite>();
            if (isHorizontal) {
                total = sprite.width;
                sprite.width = total * ratio_;
            } else {
                total = sprite.height;
                sprite.height = total * ratio_;
            }
        }
        else {
            border = GetComponent<exSpriteBorder>();
            if (isHorizontal) {
                total = border.width;
                border.width = total * ratio_;
            } else {
                total = border.height;
                border.height = total * ratio_;
            }
        } 
    }
}

