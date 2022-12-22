Shader "World Political Map 2D/Scenic" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_CloudMap ("Cloud Map", 2D) = "black" {}
		_CloudSpeed ("Cloud Speed", Range(-1, 1)) = -0.04
		_CloudAlpha ("Cloud Alpha", Range(0, 1)) = 0.7
		_CloudShadowStrength ("Cloud Shadow Strength", Range(0, 1)) = 0.3
		_CloudElevation ("Cloud Elevation", Range(0.001, 0.1)) = 0.003
        _SunLightDirection("Light Direction", Vector) = (0,0,1)
	}
	
	Subshader {
		Tags { "RenderType"="Opaque" }
        Offset 50,50
			Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag   
                #pragma target 3.0
                #include "UnityCG.cginc"

              sampler2D _MainTex;
              sampler2D _NormalMap;
              sampler2D _CloudMap;
              float _CloudSpeed;
              float _CloudAlpha;
              float _CloudShadowStrength;
              float _CloudElevation;
              float3 _SunLightDirection;
                
                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv: TEXCOORD0;
                    float3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
                    float3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
                    float3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z        
                    float3 viewDir: TEXCOORD4;            
                };
                
                v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(vertex);
                    // Push back
                    #if UNITY_REVERSED_Z
                        o.pos.z -= 0.0005;
                    #else
                        o.pos.z += 0.0005;
                    #endif
                    o.uv = uv;
                    half3 wNormal = UnityObjectToWorldNormal(normal);
                    half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                    // compute bitangent from cross product of normal and tangent
                    half tangentSign = tangent.w * unity_WorldTransformParams.w;
                    half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                    // output the tangent space matrix
                    o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                    o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                    o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                    o.viewDir = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, vertex));  
                    return o;
                }

                half4 frag (v2f i) : SV_Target {
                  half4 earth = tex2D (_MainTex, i.uv);
                  half3 worldViewDir = normalize(i.viewDir);
                  
                  half3 tnormal = UnpackNormal(tex2D(_NormalMap, i.uv));
                  // transform normal from tangent to world space
                  half3 worldNormal;
                  worldNormal.x = dot(i.tspace0, tnormal);
                  worldNormal.y = dot(i.tspace1, tnormal);
                  worldNormal.z = dot(i.tspace2, tnormal);
                  half  LdotS = saturate(dot(_SunLightDirection, -normalize(worldNormal)));
                  half wrappedDiffuse = LdotS * 0.5 + 0.5;
                  earth.rgb *= wrappedDiffuse;
    
                  fixed2 t = fixed2(_Time[0] * _CloudSpeed, 0);
                  fixed2 disp = -worldViewDir * _CloudElevation;
                    
                  half3 cloud = tex2D (_CloudMap, i.uv + t - disp);
                  half3 shadows = tex2D (_CloudMap, i.uv + t + fixed2(0.998,0) + disp) * _CloudShadowStrength;
                  shadows *= saturate (dot(worldNormal, worldViewDir));
                  half3 color = earth.rgb + (cloud.rgb - clamp(shadows.rgb, shadows.rgb, 1-cloud.rgb)) * _CloudAlpha ;
                  return half4(color, 1.0);
                }
            
            ENDCG
        }
    }
}