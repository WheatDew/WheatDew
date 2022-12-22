Shader "World Political Map 2D/Unlit Highlight" {
Properties {
    _Color ("Highlight Color", Color) = (1,0,0,0.5)
    _MainTex ("Texture", 2D) = "white" {}
}
SubShader {
    Tags {
        "Queue"="Geometry+5"
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
    }
			Cull Off			
			ZWrite Off			
			Offset -5, -5
			Blend SrcAlpha OneMinusSrcAlpha 
Pass {
			CGPROGRAM	
			#pragma fragment frag
			#pragma vertex vert	
			#include "UnityCG.cginc"

			fixed4 _Color;	
			sampler2D _MainTex;

			struct AppData {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			inline float getLuma(fixed3 rgb) {
				const fixed3 lum = float3(0.299, 0.587, 0.114);
				return dot(rgb, lum);
			}

			v2f vert(AppData v) {
				v2f o;						
				o.pos = UnityObjectToClipPos(v.vertex);	
				o.uv = v.texcoord;
				return o;								
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 pixel = tex2D(_MainTex, i.uv);
				fixed luma = getLuma(pixel.rgb);
				return fixed4(luma * _Color.rgb, _Color.a * pixel.a);			
			}

			ENDCG

		}	
		}	
}
