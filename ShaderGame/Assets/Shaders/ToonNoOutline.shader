


Shader "Custom/ToonNoOutline"{
	Properties
	{
		_MainTex("Texture",2D) = "white"{}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 200
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_base i)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv = i.texcoord;
		return o;
	}

	fixed4 frag(v2f o) : SV_Target
	{
		float4 textureColor = tex2D(_MainTex, o.uv);

		return textureColor;
	}


		ENDCG
	}
	}
		Fallback "VertexLit"
}
