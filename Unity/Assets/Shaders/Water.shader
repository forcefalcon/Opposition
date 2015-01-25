Shader "GameJam/Water" {
	Properties {
		_Color ("Color",Color) = (1,1,1,1)
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Cube ("Cubemap", CUBE) = "" {}
		_ReflColor("Reflection Color",Color) = (1,1,1,1)
		_RimPower("Fresnel",Range(0,5)) = 1
		_Speed("Water Speed",vector) = (0.5,0.5,0.5,0.5)
	}
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType"="Opaque" }

		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0
		#include "UnityCG.cginc"

		sampler2D _BumpMap;
		half4 _BumpMap_ST;
		samplerCUBE _Cube;
		fixed4 _Color;
		fixed3 _ReflColor;
		float4 _Speed;
		float _RimPower;

		struct Input {
			float4 DistUV;
			float3 viewDir;
			float3 worldRefl;
			INTERNAL_DATA
		};


		inline fixed3 combineNormalMaps (fixed3 base, fixed3 detail) {
			base.z += 1.0;
			detail.xy *= - 1.0;
			return base * dot(base, detail) / base.z - detail;
		}
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.DistUV.xy = TRANSFORM_TEX(v.texcoord,_BumpMap);
			o.DistUV.zw = o.DistUV.xy;
			o.DistUV.xy += _Speed.xy * _Time.x;
			o.DistUV.zw -= _Speed.zw * _Time.x;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {

			
			fixed3 bump1 = UnpackNormal (tex2D (_BumpMap, IN.DistUV.xy));
			fixed3 bump2 = UnpackNormal (tex2D (_BumpMap, IN.DistUV.zw));
			fixed3 finalBump = combineNormalMaps( bump1 , bump2); 
			fixed3 cubeMap = texCUBE (_Cube, WorldReflectionVector (IN, finalBump)).rgb;
			
			fixed rim = saturate(dot(normalize(IN.viewDir),finalBump));
			
			o.Albedo = _Color.rgb;
		 	o.Normal = finalBump;
          	o.Emission = cubeMap * _ReflColor * pow(rim,_RimPower);
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
