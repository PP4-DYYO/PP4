////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/17～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ネットワークプレイヤーのセッティング
/// </summary>
public class MyNetPlayerSetting : NetworkBehaviour
{
	/// <summary>
	/// ネットワークプレイヤーセッティングクラスのリスト
	/// </summary>
	static List<MyNetPlayerSetting> NetPlayerSettingList = new List<MyNetPlayerSetting>();

	/// <summary>
	/// パラメータを取得するためのプレイヤー
	/// </summary>
	[SerializeField]
	MyPlayer PlayerToAcquireParameters;

	/// <summary>
	/// ゲーム
	/// </summary>
	MyGame Game;

	/// <summary>
	/// 設定完了フラグ
	/// </summary>
	bool m_isSettingComplete;

	/// <summary>
	/// プレイヤー人数
	/// </summary>
	const int NUM_OF_PLAYERS = 2;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ローカルプレイヤーの初期
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		//必要スクリプトの追加と設定
		gameObject.AddComponent<MyPlayer>();
		GetComponent<MyPlayer>().SetPlayerParameters(PlayerToAcquireParameters);

		//ゲームに必要な設定
		Game = GameObject.Find("Game").GetComponent<MyGame>();
		Game.OperatingPlayerScript = GetComponent<MyPlayer>();
		transform.parent = Game.PlayersScript.transform;

		m_isSettingComplete = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントの初期
	/// </summary>
	public override void OnStartClient()
	{
		NetPlayerSettingList.Add(this);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///	フレーム
	/// </summary>
	void Update()
	{
		//権限のあるプレイヤーのプレイヤー人数が揃う
		if (isLocalPlayer)
			if (NetPlayerSettingList.Count >= NUM_OF_PLAYERS)
				Game.IsEndPeopleRecruitment = true;

		//権限がないプレイヤーの親設定
		if (!isLocalPlayer && isClient)
			transform.parent = GameObject.Find("Game").GetComponent<MyGame>().PlayersScript.transform;

		//(設定完了or不必要なクライアント)だと削除
		if (m_isSettingComplete || (!isLocalPlayer && isClient))
			Destroy(this);
	}
}
