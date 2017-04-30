// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SatWatchShaders/NightLights"
{
	Properties
	{
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white"
	}
	SubShader
	{
		Tags {  }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 lightDir : TEXCOORD1;
				float3 normal : NORMAL;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.lightDir = (_WorldSpaceLightPos0 - v.vertex).xyz;
				o.lightDir = WorldSpaceLightDir( v.vertex );         
				o.normal = mul(unity_WorldToObject, v.normal);
		
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				return o;
			}
			
			fixed4 frag (v2f IN) : COLOR
			{
				float3 N = normalize(IN.normal);
				float3 L = -normalize(IN.lightDir);

				float lightDot = dot(N, L) + .65;
				float intensity = min(0.65, lightDot);
				
				fixed4 col = tex2D(_MainTex, IN.uv);

				return col * intensity;
				
			}
			ENDCG
		}
	}
}
