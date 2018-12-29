Shader "Custom/S_AlphaChannel"
{
	Properties
	{
		_mainTex("テクスチャ", 2D) = "black"{}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			//Our Vertex Shader
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return o;
			}

			sampler2D _mainTex;

			//Our Fragment Shader
			fixed4 frag(v2f i) : Color
			{
				//テクスチャ情報
				fixed4 col = tex2D(_mainTex, i.uv);

				//透過処理
				col.a = (col.x * 0.2 + col.y * 0.7 + col.z * 0.1) / 8 + 0.875;
				col = fixed4(col.a, col.a, col.a, (col.a - 0.875) * 8);
				if (col.x <= 0.88)
					col.a = 0;

				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
