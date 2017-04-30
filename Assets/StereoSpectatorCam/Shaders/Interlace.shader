// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//(C) 2017 Andrew Guagliardo
//Laboratory for Advanced Visualization & Applications, Academy for Creative Media
//University of Hawaii at Manoa
//Original version: 1/23/17
//Current version: 2/2/17

Shader "Custom/Interlace" {

	Properties {
		_MainTex ("InterlaceMask", 2D) = "" {}
		_Texture1 ("Left Eye", 2D) = "" {}
		_Texture2 ("Right Eye", 2D) = "" {}
		_Texture3 ("Interlaced Texture", 2D) = "" {}
		
	}

	SubShader {

		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _Flip = 1;

			sampler2D _Texture1, _Texture2;

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvSplat : TEXCOORD1;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.uvSplat = v.uv;
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				float4 splat = tex2D(_MainTex, i.uvSplat);
				if ( _Flip < 1)
				{
					return
						tex2D(_Texture1, i.uv) * splat.g +
						tex2D(_Texture2, i.uv) * splat.r;
				}
				return
					tex2D(_Texture1, i.uv) * splat.r +
					tex2D(_Texture2, i.uv) * splat.g;
			}

			ENDCG
		}
	}
}