// Used by cities material
Shader "World Political Map 2D/Unlit Alpha Texture Color" {
Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white"
}

SubShader {
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Tags {
        "Queue"="Transparent"
        "RenderType"="Transparent"
       }
    
    Pass {
        CGPROGRAM
        #pragma vertex vert 
        #pragma fragment frag               
        #include "UnityCG.cginc"

        fixed4 _Color;
        sampler2D _MainTex;

        struct AppData {
            float4 vertex : POSITION;
            float2 texcoord: TEXCOORD0;
        };
        
        void vert(inout AppData v) {
            v.vertex = UnityObjectToClipPos(v.vertex);
            #if UNITY_REVERSED_Z
                v.vertex.z += 0.0005;
            #else
                v.vertex.z -= 0.0005;
            #endif
        }
        
        fixed4 frag(AppData i) : SV_Target {
            return tex2D(_MainTex, i.texcoord) * _Color;                    
        }
            
        ENDCG
    }
    }
    
}
