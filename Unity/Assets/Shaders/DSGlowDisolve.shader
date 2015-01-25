// Simple alpha Additive Shader

Shader "GameJam/DS [Glow][Disolve]"
{
	Properties {
		_Color("Color",Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Glossiness",Range(0,5)) = 0.5
		_GlowColor("GlowColor",Color) = (1,1,1,0)
		_GlowTex ("GlowTex (RGB)", 2D) = "white" {}
		
		_EdgeColor ("Edge Color", Color) = (1,0,0)
    	_EdgeWidth ("Edge Width", Range(0,0.25)) = 0.1
		_DisolveTex("Disolve Texture", 2D) = "white" {}
		_Cutoff("Disolve Power",Range(0,1)) = 0
	}

SubShader {
		Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"  }
		Cull Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong alphatest:Zero
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GlowTex;
		sampler2D _DisolveTex;
		float _Shininess;
		

		float _Cutoff;
		float _EdgeWidth;
		fixed4 _EdgeColor;
		fixed4 _GlowColor;

		struct Input {
			float2 uv_MainTex;

		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 glowTex = tex2D(_GlowTex,IN.uv_MainTex);
			fixed dis = tex2D (_DisolveTex, IN.uv_MainTex).r;
			
			o.Albedo = c.rgb;
		 	o.Specular = _Shininess;
		 	o.Gloss = c.a;
          	o.Emission = glowTex.rgb * _GlowColor.rgb * (exp(_GlowColor.a * 10.0f));
          	
          	
          	o.Alpha = dis - _Cutoff;
          	if ( o.Alpha < 0 && o.Alpha > -_EdgeWidth ){
				o.Alpha = 1.0;
				o.Emission += _EdgeColor.rgb * (exp(_EdgeColor.a * 10.0f));
			}

		}
		ENDCG
	} 
	FallBack "Diffuse"
}