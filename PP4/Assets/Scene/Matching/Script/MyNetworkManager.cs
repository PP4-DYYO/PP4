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
	/// 部屋に入った
	/// </summary>
	bool m_isIGotIntoTheRoom;

	/// <summary>
	/// 部屋名
	/// </summary>
	const string ROOM_NAME = "Fountain";

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
		m_match.ListMatches(0, 20, "", true, 0, 0, OnMatchList);

		m_isIGotIntoTheRoom = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレームメソッド
	/// </summary>
	void Update()
	{
		//部屋に入った
		if (m_isIGotIntoTheRoom)
			return;

		//リストマッチの更新
		if (matches == null)
		{
			//更新
			m_match.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
		}
		else if(matches.Count >= 1)
		{
			//ルームに参加
			m_match.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
			m_isIGotIntoTheRoom = true;
		}
		else
		{
			//ルームの作成
			matchName = ROOM_NAME + DateTime.Now.ToLongTimeString();
			m_match.CreateMatch(matchName, matchSize, true, "", "", "", 0, 0, OnMatchCreate);
			m_isIGotIntoTheRoom = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// サーバとして接続が切れた
	/// </summary>
	/// <param name="connectionInfo">接続情報</param>
	public override void OnServerDisconnect(NetworkConnection connectionInfo)
	{
		base.OnServerDisconnect(connectionInfo);
		Debug.Log("クライアントの接続が切れました");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントとして接続が切れた
	/// </summary>
	/// <param name="connectionInfo">接続情報</param>
	public override void OnClientDisconnect(NetworkConnection connectionInfo)
	{
		base.OnClientDisconnect(connectionInfo);
		Debug.Log("サーバとの接続が切れました");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続を切る
	/// </summary>
	public void StopConnection()
	{
		StopHost();
	}
}
