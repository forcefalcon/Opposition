Shader "GameJam/LightmapDNS" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("LM (RGB)", 2D) = "white" {}
		_BumpMap ("NormalMap (RGB)", 2D) = "bump" {}
		
		_SpecColor ("SpecColor",Color) = (1,1,1,1)
		_Shininess ("Glossiness",Range(0,5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong nolightmap nodirlightmap halfasview

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _LightMap;
		float _Shininess;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv2_LightMap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed lm = tex2D (_LightMap, IN.uv2_LightMap);
			fixed3 bump = UnpackNormal(tex2D (_BumpMap, IN.uv_BumpMap));
			
			o.Albedo = c.rgb * lm;
			o.Normal = bump;
			o.Specular = _Shininess;
			o.Gloss = c.a;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
