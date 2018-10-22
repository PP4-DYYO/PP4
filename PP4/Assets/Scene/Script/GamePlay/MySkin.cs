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
		foreach(var r in RendererForTeamColor)
		{
			//色の変更
			r.material.color = teamColor;
		}
	}
}
