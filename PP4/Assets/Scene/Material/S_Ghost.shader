Shader "Custom/S_Ghost"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Normal (RGB)", 2D) = "white" {}
		_EmissionTex("Emission (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Blightness("Metallic", Range(0,3)) = 0.0
	}

	SubShader
	{

		Pass
		{
			//透明でZ値だけ書き込み
			Zwrite On
			ColorMask 0
			Lighting OFF
		}

		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Zwrite Off
		ZTest LEqual //既に描画されているオブジェクトと距離が同じか近い時描画

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _EmissionTex;
		half _Blightness;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic * _Color.a;
			o.Smoothness = _Glossiness * _Color.a;
			o.Normal = tex2D(_NormalTex, IN.uv_MainTex) * _Color.a;
			o.Emission = (tex2D(_EmissionTex, IN.uv_MainTex) * _Color.a) * _Blightness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
