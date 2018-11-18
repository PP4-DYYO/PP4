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
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 落下用の当たり判定
	/// </summary>
	[SerializeField]
	Collider FallCollider;
	
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
	/// 垂直加速エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem VerticalAccelerationEffect;

	/// <summary>
	/// 水平加速エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem HorizontalAccelerationEffect;

	/// <summary>
	/// 落下エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem[] FallEffects;

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
	/// ターゲット
	/// </summary>
	[SerializeField]
	MyPlayer Target;
	#endregion

	#region 状況
	[Header("状況")]
	/// <summary>
	/// 行動状況
	/// </summary>
	PlayerBehaviorStatus m_state;
	#endregion

	#region 色
	[Header("色")]
	/// <summary>
	/// デフォルトカラー
	/// </summary>
	[SerializeField]
	Color m_defaultColor;
	#endregion

	#region 着地
	[Header("着地")]
	/// <summary>
	/// 着地成功エフェクトフラグ
	/// </summary>
	bool m_isLandingSuccessEffect = true;

	/// <summary>
	/// 着地失敗エフェクト
	/// </summary>
	bool m_isLandingFailedEffect = true;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用のMainModule
	/// </summary>
	ParticleSystem.MainModule m_workMainModule;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定フレーム
	/// </summary>
	void FixedUpdate()
	{
		//アニメーション処理
		AnimProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーション処理
	/// </summary>
	void AnimProcess()
	{
		//対象
		if (!Target)
			return;

		//行動状況が変わってない
		if (m_state == Target.State)
			return;

		//アニメーションが変わった時の共通処理
		CommonProcessingWhenAnimChanges();

		//行動状況
		switch (Target.State)
		{
			case PlayerBehaviorStatus.JetAccelerationRise:
				//再生
				VerticalAccelerationEffect.Play();
				//ループの有効化
				m_workMainModule = VerticalAccelerationEffect.main;
				m_workMainModule.loop = true;
				break;
			case PlayerBehaviorStatus.HorizontalAccelerationMovement:
				//再生
				HorizontalAccelerationEffect.Play();
				//ループの有効化
				m_workMainModule = HorizontalAccelerationEffect.main;
				m_workMainModule.loop = true;
				break;
			case PlayerBehaviorStatus.Falling:
				//全落下エフェクト
				foreach (var effect in FallEffects)
				{
					//再生
					effect.Play();
					//ループの有効か
					m_workMainModule = effect.main;
					m_workMainModule.loop = true;
				}
				//当たり判定の有効化
				FallCollider.enabled = true;
				break;
		}
		
		m_state = Target.State;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーションが変わった時の共通処理
	/// </summary>
	void CommonProcessingWhenAnimChanges()
	{
		//垂直加速エフェクトのループの無効化
		m_workMainModule = VerticalAccelerationEffect.main;
		m_workMainModule.loop = false;

		//水平加速エフェクトのループの無効化
		m_workMainModule = HorizontalAccelerationEffect.main;
		m_workMainModule.loop = false;

		//落下エフェクトのループの無効化
		foreach (var effect in FallEffects)
		{
			m_workMainModule = effect.main;
			m_workMainModule.loop = false;
		}

		//落下時の当たり判定の無効化
		FallCollider.enabled = false;
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
		Target = target;
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
		if (!m_isLandingSuccessEffect)
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
		if (!m_isLandingFailedEffect)
		{
			//着地失敗エフェクト
			m_isLandingFailedEffect = true;
			LandingFailedEffect.Play();
		}
	}
}
