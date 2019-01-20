////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/17～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ネットワークプレイヤーのセッティング２
/// </summary>
[NetworkSettings(channel = 1, sendInterval = 0.033f)]
public class MyNetPlayerSetting2 : NetworkBehaviour
{
	#region コンポーネント
	[Header("コンポーネント")]
	/// <summary>
	/// ネットプレイヤー設定
	/// </summary>
	[SerializeField]
	MyNetPlayerSetting NetPlayerSetting;
	#endregion

	/// <summary>
	/// SPゲージ
	/// </summary>
	[SyncVar(hook = "SyncSpGauge")]
	float m_spGauge;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//操作プレイヤーでない
		if (!isLocalPlayer)
			return;

		//SPゲージ
		CmdSpGauge(NetPlayerSetting.PlayerScript.GetPercentageOfRemainingSpGauge());
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// SPゲージの通知
	/// </summary>
	/// <param name="rate">率</param>
	[Command]
	void CmdSpGauge(float rate)
	{
		m_spGauge = rate;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// SPゲージの同期
	/// </summary>
	/// <param name="rate">率</param>
	[Client]
	void SyncSpGauge(float rate)
	{
		m_spGauge = rate;

		//UIに反映
		NetPlayerSetting.GameScript.MainUiScript.SetRankingSpGauge(NetPlayerSetting.GetNetPlayerNum(), m_spGauge);
	}
}
