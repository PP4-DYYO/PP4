////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/12/28～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

//フィールド泡
Shader "Custom/S_FieldFoam"
{
	Properties
	{
		_prev1("１フレーム前", 2D) = "white"{}
		_prev2("２フレーム前", 2D) = "white"{}
		_width("横幅ピクセル", Float) = 1024
		_height("縦幅ピクセル", Float) = 1024
		_scale("拡縮", Float) = 1
		_speed("速度", Float) = 10
		_bubbleGenerationPos0("泡発生位置1", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos1("泡発生位置2", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos2("泡発生位置3", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos3("泡発生位置4", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos4("泡発生位置5", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos5("泡発生位置6", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos6("泡発生位置7", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos7("泡発生位置8", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos8("泡発生位置9", Vector) = (-1, -1, -1 ,-1)
		_bubbleGenerationPos9("泡発生位置10", Vector) = (-1, -1, -1 ,-1)
		_bubbleRadius("泡の半径", Float) = 1
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

			sampler2D _prev1;
			sampler2D _prev2;

			fixed _width;
			fixed _height;
			fixed _scale;
			fixed _speed;

			fixed4 _bubbleGenerationPos0;
			fixed4 _bubbleGenerationPos1;
			fixed4 _bubbleGenerationPos2;
			fixed4 _bubbleGenerationPos3;
			fixed4 _bubbleGenerationPos4;
			fixed4 _bubbleGenerationPos5;
			fixed4 _bubbleGenerationPos6;
			fixed4 _bubbleGenerationPos7;
			fixed4 _bubbleGenerationPos8;
			fixed4 _bubbleGenerationPos9;
			fixed _bubbleRadius;

			//ランダム関数
			float random(fixed2 p)
			{
				return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
			}

			//Our Fragment Shader
			fixed4 frag(v2f i) : Color
			{
				fixed strideX = _speed / _width;
				fixed strideY = _speed / _height;

				//色情報の取得
				fixed4 prev1 = tex2D(_prev1, i.uv);
				fixed4 prev1N = tex2D(_prev1, half2(i.uv.x, i.uv.y + strideY));
				fixed4 prev1E = tex2D(_prev1, half2(i.uv.x + strideX, i.uv.y));
				fixed4 prev1S = tex2D(_prev1, half2(i.uv.x, i.uv.y - strideY));
				fixed4 prev1W = tex2D(_prev1, half2(i.uv.x - strideX, i.uv.y));
				fixed4 prev1NE = tex2D(_prev1, half2(i.uv.x + strideX, i.uv.y + strideY));
				fixed4 prev1SE = tex2D(_prev1, half2(i.uv.x + strideX, i.uv.y - strideY));
				fixed4 prev1SW = tex2D(_prev1, half2(i.uv.x - strideX, i.uv.y - strideY));
				fixed4 prev1NW = tex2D(_prev1, half2(i.uv.x - strideX, i.uv.y + strideY));
				fixed4 prev2 = tex2D(_prev2, i.uv);
				prev1.a = prev1N.a = prev1E.a = prev1S.a = prev1W.a = prev2.a = 1;

				//ラプラシアンフィルタ
				fixed4 retval = prev1 * 2 - prev2 + (prev1E + prev1W + prev1N + prev1S + prev1NE + prev1SE + prev1SW + prev1NW - prev1 * 8) * 0.125;

				//泡の入力あり
				if (_bubbleGenerationPos0.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos0 = fixed4(_bubbleGenerationPos0.x, _bubbleGenerationPos0.z, _bubbleGenerationPos0.y, _bubbleGenerationPos0.w);
					_bubbleGenerationPos0 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos0 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos0);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos1.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos1 = fixed4(_bubbleGenerationPos1.x, _bubbleGenerationPos1.z, _bubbleGenerationPos1.y, _bubbleGenerationPos1.w);
					_bubbleGenerationPos1 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos1 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos1);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos2.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos2 = fixed4(_bubbleGenerationPos2.x, _bubbleGenerationPos2.z, _bubbleGenerationPos2.y, _bubbleGenerationPos2.w);
					_bubbleGenerationPos2 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos2 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos2);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos3.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos3 = fixed4(_bubbleGenerationPos3.x, _bubbleGenerationPos3.z, _bubbleGenerationPos3.y, _bubbleGenerationPos3.w);
					_bubbleGenerationPos3 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos3 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos3);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos4.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos4 = fixed4(_bubbleGenerationPos4.x, _bubbleGenerationPos4.z, _bubbleGenerationPos4.y, _bubbleGenerationPos4.w);
					_bubbleGenerationPos4 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos4 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos4);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos5.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos5 = fixed4(_bubbleGenerationPos5.x, _bubbleGenerationPos5.z, _bubbleGenerationPos5.y, _bubbleGenerationPos5.w);
					_bubbleGenerationPos5 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos5 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos5);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos6.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos6 = fixed4(_bubbleGenerationPos6.x, _bubbleGenerationPos6.z, _bubbleGenerationPos6.y, _bubbleGenerationPos6.w);
					_bubbleGenerationPos6 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos6 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos6);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos7.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos7 = fixed4(_bubbleGenerationPos7.x, _bubbleGenerationPos7.z, _bubbleGenerationPos7.y, _bubbleGenerationPos7.w);
					_bubbleGenerationPos7 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos7 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos7);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos8.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos8 = fixed4(_bubbleGenerationPos8.x, _bubbleGenerationPos8.z, _bubbleGenerationPos8.y, _bubbleGenerationPos8.w);
					_bubbleGenerationPos8 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos8 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos8);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}
				if (_bubbleGenerationPos9.w != -1)
				{
					//テクスチャに対応した位置と距離
					_bubbleGenerationPos9 = fixed4(_bubbleGenerationPos9.x, _bubbleGenerationPos9.z, _bubbleGenerationPos9.y, _bubbleGenerationPos9.w);
					_bubbleGenerationPos9 = (fixed4(1, 1, 0, 0) - _bubbleGenerationPos9 / _scale) - fixed4(0.5, 0.5, 0, 0);
					fixed d = distance(i.uv, _bubbleGenerationPos9);

					//泡のリング
					fixed r = random(i.uv);
					if (d < _bubbleRadius * 1.0 / _scale && d >= _bubbleRadius * 0.9 / _scale)
						retval += fixed4(0.1 * r, 0.1 * r, 0.1 * r, 1.0);
					if (d < _bubbleRadius * 0.9 / _scale && d >= _bubbleRadius * 0.8 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.8 / _scale && d >= _bubbleRadius * 0.7 / _scale)
						retval += fixed4(0.3 * r, 0.3 * r, 0.3 * r, 1.0);
					if (d < _bubbleRadius * 0.7 / _scale && d >= _bubbleRadius * 0.6 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.6 / _scale && d >= _bubbleRadius * 0.5 / _scale)
						retval += fixed4(0.5 * r, 0.5 * r, 0.5 * r, 1.0);
					if (d < _bubbleRadius * 0.5 / _scale && d >= _bubbleRadius * 0.4 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.4 / _scale && d >= _bubbleRadius * 0.3 / _scale)
						retval += fixed4(0.7 * r, 0.7 * r, 0.7 * r, 1.0);
					if (d < _bubbleRadius * 0.3 / _scale && d >= _bubbleRadius * 0.2 / _scale)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
					if (d < _bubbleRadius * 0.2 / _scale && d >= _bubbleRadius * 0.1 / _scale)
						retval += fixed4(0.9 * r, 0.9 * r, 0.9 * r, 1.0);
					if (d < _bubbleRadius * 0.1 / _scale && d >= /*_bubbleRadius * 0.0 / _scale*/ 0)
						retval += fixed4(0.05 * r, 0.05 * r, 0.05 * r, 0.0);
				}

				return retval;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
