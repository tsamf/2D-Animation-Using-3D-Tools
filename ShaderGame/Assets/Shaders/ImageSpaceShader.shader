Shader "Custom/ImageSpaceShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
		SubShader
		{
			Tags{ "Queue" = "Transparent" }
			Cull Off ZWrite Off ZTest Always
			LOD 200 // Level of Detail on the shader


			Pass
			{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;
			float4 _CameraDepthTexture_TexelSize;
			sampler2D _MainTex;
			

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv: TEXCOORD0;
			};
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.uv = v.texcoord;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{

				float2 delta = .5 *_CameraDepthTexture_TexelSize.xy;
				float4 h = float4(0, 0, 0, 0);
				float4 v = float4(0, 0, 0, 0);

				h += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, -1.0) * delta)) * 1.0;
				h += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, -1.0) * delta)) * -1.0;
				h += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, 0.0) * delta)) * 2.0;
				h += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, 0.0) * delta)) * -2.0;
				h += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, 1.0) * delta)) * 1.0;
				h += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, 1.0) * delta)) * -1.0;

				v += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, -1.0) * delta)) * 1.0;
				v += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, -1.0) * delta)) * -1.0;
				v += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, 0.0) * delta)) * 2.0;
				v += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, 0.0) * delta)) * -2.0;
				v += tex2D(_CameraDepthTexture, (i.uv + float2(-1.0, 1.0) * delta)) * 1.0;
				v += tex2D(_CameraDepthTexture, (i.uv + float2(1.0, 1.0) * delta)) * -1.0;



				float s = sqrt(h*h + v*v);
				
				fixed4 sobel = float4(0, 0, 0, 0);

				if (s > .05)
				{
					s = 0;
					sobel = float4(s, s, s, 1);
				}
				else
				{
					s = 1;
					sobel = float4(s, s, s, 1);
				}

				fixed4 texcol = tex2D(_MainTex, i.uv);

				return texcol * sobel;
			}
			ENDCG
		}
	}
}
