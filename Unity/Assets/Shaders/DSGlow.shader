// Simple alpha Additive Shader

Shader "GameJam/DS [Glow]"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Glossiness",Range(0,5)) = 0.5
		_GlowColor("GlowColor",Color) = (1,1,1,0)
		_GlowTex ("GlowTex (RGB)", 2D) = "white" {}
	}

SubShader {
		Tags { "Queue" = "Geometry" "RenderType"="Opaque" }
		Cull Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GlowTex;
		fixed4 _GlowColor;
		float _Shininess;


		struct Input {
			float2 uv_MainTex;

		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed3 glowTex = tex2D(_GlowTex,IN.uv_MainTex);
			
			o.Albedo = c.rgb;
		 	o.Specular = c.a;
		 	o.Gloss = _Shininess;
          	o.Emission = glowTex.rgb * _GlowColor.rgb * (exp(_GlowColor.a * 10.0f));;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}