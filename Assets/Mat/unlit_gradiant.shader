// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GrediantFade"
{
	Properties
	{
		_Color ("Color", Color) = (.34, .85, .92, 1)	
		_H ("Height", Float) = 1
		_OF ("Offset", Range (-1, 1)) = 0
		_Alpha("OverallFlpha", Range (0, 1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching" = "true"}
		LOD 100

		//Cull off
		ZTest always

		Pass
		{

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag alpha
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex: SV_POSITION;
				float3 pos: TEXCOORD2;
			};

			float4 _Color;
			float _H;
			float _OF;
			float _Alpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color;

				col.a  = 1-  (i.pos.y / _H + _OF);
				col.a = col.a * _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
