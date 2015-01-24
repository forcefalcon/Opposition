Shader "GameJam/Alpha/AlphaDNSR" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Glossiness",Range(0,5)) = 0.5
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Cube ("Cubemap", CUBE) = "" {}
		_ReflColor("Reflection Color",Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		ZWrite Off
		Cull Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert alpha
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		fixed3 _ReflColor;
		float _Shininess;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			fixed4 color : COLOR;
			float3 worldRefl;
			INTERNAL_DATA
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.color = v.color;
		}
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			fixed3 bump = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			fixed3 cubeMap = texCUBE (_Cube, WorldReflectionVector (IN, bump)).rgb;
			
			o.Albedo = c.rgb * IN.color.rgb;
		 	o.Normal = bump;
		 	o.Specular = c.a;
		 	o.Gloss = _Shininess;
          	o.Emission = cubeMap * _ReflColor;
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
