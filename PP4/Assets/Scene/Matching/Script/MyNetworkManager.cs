////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/5/16～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

/// <summary>
/// ネットワークマネージャクラス
/// </summary>
public class MyNetworkManager : NetworkManager
{
	/// <summary>
	/// ネットワークマッチ
	/// </summary>
	NetworkMatch m_match;

	/// <summary>
	/// ネットワークマッチ数
	/// </summary>
	const uint MATCH_SIZE = 8U;

	/// <summary>
	/// 待機状態フラグ
	/// </summary>
	bool m_isStandbyState;
	public bool IsStandbyState
	{
		get { return m_isStandbyState; }
		set { m_isStandbyState = value; }
	}

	/// <summary>
	/// 部屋名
	/// </summary>
	const string ROOM_NAME = "Fountain";

	/// <summary>
	/// 部屋参加中フラグ
	/// </summary>
	bool m_isJoinRoom;

	/// <summary>
	/// 部屋作成中フラグ
	/// </summary>
	bool m_isCreateRoom;

	/// <summary>
	/// サーバーとの接続が切れた
	/// </summary>
	bool m_isConnectionWithServerIsBroken;
	public bool IsConnectionWithServerIsBroken
	{
		get { return m_isConnectionWithServerIsBroken; }
		set { m_isConnectionWithServerIsBroken = value; }
	}

	/// <summary>
	/// 使用中のネットワークID
	/// </summary>
	UInt64 m_networkIdInUse;

	/// <summary>
	/// 使用不可のネットワークID
	/// </summary>
	List<UInt64> m_unusableNetworkId = new List<UInt64>();

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーを足すときのメソッド
	/// </summary>
	/// <param name="conn">クライアントから接続を開始</param>
	/// <param name="playerControllerId">プレイヤーID</param>
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		var player = Instantiate(playerPrefab, GameObject.Find("Players").transform);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期メソッド
	/// </summary>
	void Start()
	{
		//マネージャーの設定
		StartMatchMaker();
		matchSize = MATCH_SIZE;
		m_match = matchMaker;
		matches = null;

		m_isStandbyState = true;
		m_isConnectionWithServerIsBroken = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレームメソッド
	/// </summary>
	void Update()
	{
		//待機状態
		if (m_isStandbyState)
			return;

		//部屋作成中
		if(m_isCreateRoom)
		{
			//更新
			m_match.ListMatches(0, 20, "", true, 0, 0, OnMatchList);

			//使用中のネットワークIDを取得
			if (matches.Count >= 1)
			{
				m_networkIdInUse = (UInt64)(matches[0].networkId);
				m_isStandbyState = true;
			}
			return;
		}

		//リストマッチの更新
		if (matches == null)
		{
			//更新
			m_match.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
		}
		else if (matches.Count >= 1 && IsMatch())
		{
			//ルームに参加
			m_match.JoinMatch((UnityEngine.Networking.Types.NetworkID)m_networkIdInUse, "", "", "", 0, 0, OnMatchJoined);
			m_isJoinRoom = true;
			m_isStandbyState = true;
		}
		else
		{
			//ルームの作成
			matchName = ROOM_NAME + DateTime.Now.ToLongTimeString();
			m_match.CreateMatch(matchName, matchSize, true, "", "", "", 0, 0, OnMatchCreate);
			m_isCreateRoom = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マッチできるか
	/// </summary>
	bool IsMatch()
	{
		//マッチした情報
		foreach (var match in matches)
		{
			m_networkIdInUse = (UInt64)match.networkId;

			//使えないネットワークID検索
			foreach (var unusable in m_unusableNetworkId)
			{
				if (m_networkIdInUse == unusable)
					m_networkIdInUse = 0;
			}

			if (m_networkIdInUse != 0)
				return true;
		}

		return false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続状態
	/// </summary>
	/// <returns>接続しているか</returns>
	public bool IsConnection()
	{
		return m_isCreateRoom || m_isJoinRoom || !m_isStandbyState;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// サーバとして接続が切れた
	/// </summary>
	/// <param name="connectionInfo">接続情報</param>
	public override void OnServerDisconnect(NetworkConnection connectionInfo)
	{
		NetworkServer.DestroyPlayersForConnection(connectionInfo);

		//使えないネットワークID
		m_unusableNetworkId.Add(m_networkIdInUse);

		//マッチングのリセット
		ResetMatching();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントとして接続が切れた
	/// </summary>
	/// <param name="connectionInfo">接続情報</param>
	public override void OnClientDisconnect(NetworkConnection connectionInfo)
	{
		//ホストの終了
		StopHost();

		//使えないネットワークID
		m_unusableNetworkId.Add(m_networkIdInUse);

		//マッチングのリセット
		ResetMatching();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マッチングのリセット
	/// </summary>
	void ResetMatching()
	{
		//マネージャーの設定
		StartMatchMaker();
		matchSize = MATCH_SIZE;
		m_match = matchMaker;
		matches = null;

		m_isStandbyState = true;
		m_isJoinRoom = false;
		m_isCreateRoom = false;
		m_isConnectionWithServerIsBroken = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続を切る
	/// </summary>
	public void StopConnection()
	{
		//ホストの終了
		StopHost();

		//使えないネットワークID
		if (m_isCreateRoom)
			m_unusableNetworkId.Add(m_networkIdInUse);

		//マッチングのリセット
		ResetMatching();
	}
}
