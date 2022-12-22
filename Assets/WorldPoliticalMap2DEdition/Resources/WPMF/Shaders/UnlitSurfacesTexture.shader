Shader "World Political Map 2D/Unlit Surface Texture" {
 
Properties {
    _Color ("Color", Color) = (1,1,1)
    _MainTex ("Texture", 2D) = "white"
}
 
SubShader {
    Tags {
        "Queue"="Geometry+1"
        "RenderType"="Opaque"
    }
    	
    Blend SrcAlpha OneMinusSrcAlpha
    Material {
        Emission [_Color]
    }
   	ZWrite Off
    Pass {
    	SetTexture [_MainTex] {
            Combine Texture * Primary, Texture * Primary
        }
    }
}
 
}
