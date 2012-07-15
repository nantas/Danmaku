// ======================================================================================
// File         : SpriteOpaque.shader
// Author       : Wu Jie 
// Last Change  : 07/13/2012 | 22:47:34 PM | Friday,July
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
//
///////////////////////////////////////////////////////////////////////////////

Shader "ex2D/Opaque" {
    Properties {
        _MainTex ("Atlas Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { 
            "Queue"="Geometry" 
            "IgnoreProjector"="True" 
            "RenderType"="Opaque" 
        }
        Cull Off 
        Lighting Off 
		ZWrite On ZTest Less Cull Off
        Fog { Mode Off }

        BindChannels {
            Bind "Color", color
            Bind "Vertex", vertex
            Bind "TexCoord", texcoord
        }

        Pass {
            SetTexture [_MainTex] {
                combine texture * primary
            }
        }
    }
}
