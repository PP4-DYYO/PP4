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

	#region 着地
	[Header("着地")]
	/// <summary>
	/// 着地成功エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem LandingSuccessEffect;

	/// <summary>
	/// 着地失敗エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem LandingFailedEffect;

	/// <summary>
	/// 着地成功エフェクトフラグ
	/// </summary>
	bool m_isLandingSuccessEffect = true;

	/// <summary>
	/// 着地失敗エフェクト
	/// </summary>
	bool m_isLandingFailedEffect = true;
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
		foreach (var effect in FallEffects)
		{
			effect.Emit(1);
		}
		FallCollider.enabled = true;

		m_countFallEffectTime = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 着地エフェクトフラグをTrueにすることを試みる
	/// </summary>
	public void AttemptToSetLandingEffectFlagToTrue()
	{
		if (!m_isLandingSuccessEffect)
			m_isLandingSuccessEffect = true;
		if (!m_isLandingFailedEffect)
			m_isLandingFailedEffect = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 着地エフェクトフラグをFalseにすることを試みる
	/// </summary>
	public void AttemptToSetLandingEffectFlagToFalse()
	{
		if (m_isLandingSuccessEffect)
			m_isLandingSuccessEffect = false;
		if (m_isLandingFailedEffect)
			m_isLandingFailedEffect = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 着地成功エフェクトを試みる
	/// </summary>
	public void TryLandingSuccessEffect()
	{
		if(!m_isLandingSuccessEffect)
		{
			//着地成功エフェクト
			m_isLandingSuccessEffect = true;
			LandingSuccessEffect.Play();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 着地失敗エフェクトを試みる
	/// </summary>
	public void TryLandingFailedEffect()
	{
		if(!m_isLandingFailedEffect)
		{
			//着地失敗エフェクト
			m_isLandingFailedEffect = true;
			LandingFailedEffect.Play();
		}
	}
}
