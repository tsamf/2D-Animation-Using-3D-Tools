Shader "Custom/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		
		_OutlineColor("OutlineColor",Color) =  (0,0,0,0)
		_ExtrudeAmount("Extrude Amount", Range(0,1)) = 1
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 position : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _OutlineColor;
			float4 _MainTex_ST;
			float _ExtrudeAmount;

			
			v2f vert (appdata IN)
			{
				v2f OUT;
				
				IN.vertex.xyz += IN.normal.xyz * _ExtrudeAmount;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				return OUT;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
	
				return col * _OutlineColor;
			}
			ENDCG
		}



			Pass
			{
				Cull Back

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 position : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _OutlineColor;
			float4 _MainTex_ST;
			float _ExtrudeAmount;


			v2f vert(appdata IN)
			{
				v2f OUT;
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				return OUT;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
				ENDCG
			}
	}
}
