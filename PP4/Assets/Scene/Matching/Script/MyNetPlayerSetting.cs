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
	/// ネットワークプレイヤーセッティングクラス達
	/// </summary>
	static List<MyNetPlayerSetting> m_netPlayerSettings = new List<MyNetPlayerSetting>();
	public static List<MyNetPlayerSetting> NetPlayerSettings
	{
		get { return m_netPlayerSettings; }
	}

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
	/// プレイヤー人数
	/// </summary>
	const int NUM_OF_PLAYERS = 2;

	/// <summary>
	/// プレイヤー番号
	/// </summary>
	int m_playerNum;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	string m_playerName;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	const string PLAYER_NAME = "Player";

	/// <summary>
	/// メンバーが変わったフラグ
	/// </summary>
	bool m_isMemberChanged;

	/// <summary>
	/// チーム番号
	/// </summary>
	Team m_teamNum;
	public Team TeamNum
	{
		get { return m_teamNum; }
		set { m_teamNum = value; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ローカルプレイヤーの初期
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyGame>();

		//必要スクリプトの追加と設定
		gameObject.AddComponent<MyPlayer>();
		GetComponent<MyPlayer>().SetPlayerParameters(PlayerToAcquireParameters);

		//ゲームに必要な設定
		Game.OperatingPlayerScript = GetComponent<MyPlayer>();
		transform.parent = Game.PlayersScript.transform;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントの初期
	/// </summary>
	public override void OnStartClient()
	{
		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyGame>();

		//接続が切れているプレイヤー確認
		m_isMemberChanged = false;
		for (var i = 0; i < m_netPlayerSettings.Count;)
		{
			//存在していない
			if (!m_netPlayerSettings[i])
			{
				m_netPlayerSettings.RemoveAt(i);
				m_isMemberChanged = true;
			}
			else
			{
				i++;
			}
		}

		m_netPlayerSettings.Add(this);

		//名前の登録
		m_playerName = PLAYER_NAME + m_netPlayerSettings.Count.ToString();

		//メンバーが入れ替わった
		if (m_isMemberChanged)
		{
			//プレイヤー名を初めから登録
			for (var i = 0; i < m_netPlayerSettings.Count; i++)
			{
				Game.MainUiScript.PlayerNamesTexts[i].text = m_netPlayerSettings[i].m_playerName;
			}
		}
		else
		{
			//追加されたプレイヤー名を登録
			Game.MainUiScript.PlayerNamesTexts[m_netPlayerSettings.Count - 1].text = m_playerName;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///	フレーム
	/// </summary>
	void Update()
	{
		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyGame>();

		if (Game.State == GameStatus.RecruitPeople)
		{
			//権限のあるプレイヤーを実行するタイミングで、プレイヤー人数が揃う
			if (isLocalPlayer)
				if (m_netPlayerSettings.Count >= NUM_OF_PLAYERS)
					Game.IsEndPeopleRecruitment = true;

			//権限がないプレイヤー
			if (!isLocalPlayer && isClient)
			{
				//親設定と不要なものを取り除く
				transform.parent = Game.PlayersScript.transform;
				GetComponent<Rigidbody>().isKinematic = true;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー番号を取得
	/// </summary>
	/// <returns>更新されたプレイヤー番号</returns>
	public int GetPlayerNum()
	{
		m_playerNum = m_netPlayerSettings.IndexOf(this);
		return m_playerNum;
	}
}
