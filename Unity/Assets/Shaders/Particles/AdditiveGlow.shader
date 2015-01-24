// Simple alpha Additive Shader

Shader "GameJam/Particles/Additive [Glow]"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GlowColor("GlowColor",Color) = (1,1,1,0)
	}


	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		Pass {
			Cull Off
			ZWrite Off
			Blend srcAlpha One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _MainTex_ST;
			
			fixed4 _GlowColor;
			
			// Struct Input || VertOut
			struct appdata {
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR; 
			};
			
			//VertIn
			struct v2f {
				half4 pos : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.color = v.color;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}
			

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				col.rgb *= _GlowColor.rgb * (exp(_GlowColor.a * 10.0f));
				return i.color * col;
				
			}
			ENDCG			
		}
	}
	FallBack "Diffuse"
}

