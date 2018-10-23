////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/10/21～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキン
/// </summary>
public class MySkin : MonoBehaviour
{
	/// <summary>
	/// アニメータ
	/// </summary>
	[SerializeField]
	Animator Anim;

	/// <summary>
	/// ウォーターゲージ
	/// </summary>
	[SerializeField]
	Transform WaterGauge;

	/// <summary>
	/// ボードの方向
	/// </summary>
	[SerializeField]
	Transform BoardDirection;

	/// <summary>
	/// ジェットウォータ
	/// </summary>
	[SerializeField]
	MyJetWater JetWater;

	/// <summary>
	/// チームカラーの為のレンダラー達
	/// </summary>
	[SerializeField]
	Renderer[] RendererForTeamColor;

	/// <summary>
	/// デフォルトカラー
	/// </summary>
	[SerializeField]
	Color m_defaultColor;

	#region 落下
	[Header("落下")]
	/// <summary>
	/// 落下用の当たり判定
	/// </summary>
	[SerializeField]
	Collider FallCollider;

	/// <summary>
	/// 落下エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem[] FallEffects;

	/// <summary>
	/// 落下エフェクト時間
	/// </summary>
	[SerializeField]
	float m_fallEffectTime;

	/// <summary>
	/// 落下エフェクト時間を数える
	/// </summary>
	float m_countFallEffectTime = float.MaxValue;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定フレーム
	/// </summary>
	void FixedUpdate()
	{
		//落下エフェクト時間
		if (m_countFallEffectTime < m_fallEffectTime)
		{
			m_countFallEffectTime += Time.deltaTime;

			//終了
			if (m_countFallEffectTime >= m_fallEffectTime)
				FallCollider.enabled = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スキン設定
	/// </summary>
	/// <param name="target">ターゲット</param>
	/// <param name="netTarget">ネット用ターゲット</param>
	public void SetSkin(MyPlayer target, MyNetPlayerSetting netTarget = null)
	{
		//プレイヤー
		target.SetSkin(Anim, WaterGauge, BoardDirection, JetWater);

		//ネットプレイヤー設定
		if (netTarget != null)
			netTarget.SetSkin(Anim);

		//ジェットウォータ設定
		JetWater.PlayerScript = target;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スキンカラーをデフォルトにする
	/// </summary>
	public void DefaultSkinColor()
	{
		//チームカラー用のメッシュレンダラー
		foreach (var r in RendererForTeamColor)
		{
			//デフォルトカラーにする
			r.material.color = m_defaultColor;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チームカラーを設定
	/// </summary>
	/// <param name="teamColor">チームカラー</param>
	public void SetTeamColor(Color teamColor)
	{
		//チームカラー用のメッシュレンダラー
		foreach (var r in RendererForTeamColor)
		{
			//色の変更
			r.material.color = teamColor;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 落下エフェクトの開始
	/// </summary>
	public void StartFallEffect()
	{
		foreach(var effect in FallEffects)
		{
			effect.Emit(1);
		}
		FallCollider.enabled = true;

		m_countFallEffectTime = 0;
	}
}
