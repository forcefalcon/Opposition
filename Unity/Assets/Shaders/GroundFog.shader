﻿Shader "GameJam/GroundFog" {
Properties {
	_Color ("Color", Color) = (1,1,1,1)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	
	_TextureScale ("Scale",float) = 1
	_Speed("Speed",vector) = (1,1,1,1)
}


	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (1,1,1,1) }
	
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _TextureScale;
			float4 _Speed;
			
			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 projPos : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldPos2 : TEXCOORD3;
			};

			float4 _MainTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);

				o.worldPos = mul (_Object2World, v.vertex).xyz;
				o.worldPos2 = o.worldPos;
				o.worldPos.x += _Speed.x * _Time.x;
				o.worldPos.z += _Speed.y * _Time.x;
				
				o.worldPos2.x += _Speed.z * _Time.x;
				o.worldPos2.z += _Speed.w * _Time.x;
				
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : COLOR
			{

				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));

				fixed4 fog1 = tex2D (_MainTex, i.worldPos.xz * _TextureScale);
				fixed4 fog2 = tex2D (_MainTex, i.worldPos2.xz * _TextureScale);
				
				return (fog1 + fog2) * _Color * fade;
			}
			ENDCG 
		}	
}
}