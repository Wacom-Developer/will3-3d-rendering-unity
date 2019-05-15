Shader "Unlit/ParticleInkShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_InkColor ("Ink Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Cull Back
		ZWrite Off
		ZTest LEqual // On
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _InkColor;
			float4 _MainTex_ST;

			// Tranforms position from object to clip space
			//inline float4 ObjectSpaceToClipSpace(in float3 pos)
			//{
			//	float4 tr = float4(pos, 1.0f);
			//	tr = mul(unity_ObjectToWorld, tr);
			//	tr = mul(UNITY_MATRIX_V, tr);
			//	tr = mul(UNITY_MATRIX_P, tr);
			//	return tr;
			//	// More efficient than computing M*VP matrix product
			//	//return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
			//}

			inline float4 IndividualQuadsBillboard(in float3 pos, in float3 offset)
			{
				float4 wrldPos = mul(unity_ObjectToWorld, float4(pos, 1.0f));
				float4 viewPos = mul(UNITY_MATRIX_V, wrldPos);

				viewPos.x += offset.x;
				viewPos.y += offset.y;

				float4 projPos = mul(UNITY_MATRIX_P, viewPos);
				return projPos;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = IndividualQuadsBillboard(v.vertex, v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * _InkColor;
			}
			ENDCG
		}

		// paste in forward rendering passes from Transparent/Diffuse
		UsePass "Transparent/Diffuse/FORWARD"
	}
}
