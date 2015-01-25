Shader "GameJam/2x DNS" 
{

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SecTex ("Second Tex",2D) = "white" {}
		
		_BumpMap ("NormalMap",2D) = "bump" {}
		_BumpMap2 ("NormalMap2",2D) = "bump" {}
	
		_MaskTex ("Mask Tex",2D) = "white" {}
		
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Glossiness",Range(0,5)) = 0.5

	}

SubShader {
		Tags { "Queue" = "Geometry" "RenderType"="Opaque" }
		Cull Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0

		sampler2D _MainTex,_SecTex,_BumpMap,_BumpMap2,_MaskTex;
		float _Shininess;


		struct Input {
			float2 uv_MainTex;
			float2 uv_SecTex;
			float2 uv_BumpMap;
			float2 uv_BumpMap2;
			float2 uv_MaskTex;

		};

		void surf (Input IN, inout SurfaceOutput o) {
		
			fixed mask = saturate(tex2D (_MaskTex, IN.uv_MaskTex).r);
		
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 tex2 = tex2D (_SecTex, IN.uv_SecTex);
			
			fixed4 finalColor = lerp(tex,tex2,mask);
			
			fixed3 bump = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
			fixed3 bump2 = UnpackNormal(tex2D(_BumpMap2,IN.uv_BumpMap2));
			
			fixed3 finalBump = lerp(bump , bump2 , mask);		
			
			o.Albedo = finalColor.rgb;
		 	o.Specular = _Shininess;
		 	o.Gloss = finalColor.a;
		 	o.Normal = finalBump;
			o.Alpha = finalColor.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}