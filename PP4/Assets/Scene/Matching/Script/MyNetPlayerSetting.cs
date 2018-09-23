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
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// ネットワークプレイヤーセッティングクラス達
	/// </summary>
	static List<MyNetPlayerSetting> m_netPlayerSettings = new List<MyNetPlayerSetting>();
	public static List<MyNetPlayerSetting> NetPlayerSettings
	{
		get { return m_netPlayerSettings; }
	}

	/// <summary>
	/// ゲーム
	/// </summary>
	MyGame Game;
	#endregion

	#region プレイヤーの情報
	/// <summary>
	/// チーム番号
	/// </summary>
	Team m_teamNum;
	public Team TeamNum
	{
		get { return m_teamNum; }
		set { m_teamNum = value; }
	}

	/// <summary>
	/// プレイヤー番号
	/// </summary>
	int m_playerNum;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	[SyncVar(hook = "SyncPlayerName")]
	string m_playerName;
	public string PlayerName
	{
		get { return m_playerName; }
	}

	/// <summary>
	/// 追加されたプレイヤー数
	/// </summary>
	int m_numOfPlayersAdded;
	#endregion

	/// <summary>
	/// 準備完了フラグ
	/// </summary>
	[SyncVar(hook = "SyncIsReady")]
	bool m_isReady;
	public bool IsReady
	{
		get { return m_isReady; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントの初期
	/// </summary>
	public override void OnStartClient()
	{
		base.OnStartClient();

		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyGame>();

		m_netPlayerSettings.Add(this);
		
		//プレイヤー数が増えた
		m_numOfPlayersAdded++;
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

		//ゲームに必要な設定
		Game.OperatingPlayerScript = GetComponent<MyPlayer>();
		transform.parent = Game.PlayersScript.transform;

		//名前の登録
		CmdRegisterPlayerName(MyGameInfo.Instance.PlayerName);

		//準備完了
		CmdNotifyOfIsReady(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー名登録のサーバ通知
	/// </summary>
	/// <param name="playerName">プレイヤー名</param>
	[Command]
	void CmdRegisterPlayerName(string playerName)
	{
		m_playerName = playerName;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー名を同期する
	/// </summary>
	/// <param name="playerName">プレイヤー名</param>
	[Client]
	void SyncPlayerName(string playerName)
	{
		m_playerName = playerName;

		//自分の名の同期
		if(isLocalPlayer)
		{
			//既存プレイヤーのリストアップ
			Game.MainUiScript.ListConnectedPlayers(m_netPlayerSettings.ToArray());
			m_numOfPlayersAdded = 0;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 準備完了フラグのサーバ通知
	/// </summary>
	/// <param name="isReady">準備完了</param>
	[Command]
	public void CmdNotifyOfIsReady(bool isReady)
	{
		m_isReady = isReady;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 準備完了フラグを同期する
	/// </summary>
	/// <param name="isReady">準備完了フラグ</param>
	[Client]
	void SyncIsReady(bool isReady)
	{
		m_isReady = isReady;
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

		//ゲームが人材募集状態
		if (Game.State == GameStatus.RecruitPeople)
		{
			//権限がないプレイヤー
			if (!isLocalPlayer && isClient)
			{
				//親設定と不要なものを取り除く
				transform.parent = Game.PlayersScript.transform;
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<MyPlayer>().enabled = false;
			}
			else
			{
				//追加されたプレイヤーがいる
				if(m_numOfPlayersAdded > 0)
				{
					//追加されたプレイヤーをUIに登録
					for(var i = m_netPlayerSettings.Count - m_numOfPlayersAdded; i < m_netPlayerSettings.Count; i++)
					{
						Game.MainUiScript.RegisterPlayerName(i, m_netPlayerSettings[i].m_playerName);
					}
				}
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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルスタートできるかか
	/// </summary>
	/// <returns>バトル開始できる</returns>
	public bool IsBattleStart()
	{
		//接続が切れているプレイヤー確認
		ConfirmationOfPlayersWhoAreDisconnected();

		//全てのプレイヤーにアクセス
		for (var i = 0; i < m_netPlayerSettings.Count; i++)
		{
			//準備完了でない
			if (!m_netPlayerSettings[i].m_isReady)
				return false;
		}

		//プレイヤー人数が揃うか
		return (m_netPlayerSettings.Count >= MyGame.NUM_OF_PLAYERS);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続が切れたプレイヤーの確認
	/// </summary>
	void ConfirmationOfPlayersWhoAreDisconnected()
	{
		for (var i = 0; i < m_netPlayerSettings.Count;)
		{
			//存在していない
			if (!m_netPlayerSettings[i])
			{
				//接続が切れた設定
				m_netPlayerSettings.RemoveAt(i);
				Game.MainUiScript.ListConnectedPlayers(m_netPlayerSettings.ToArray());
			}
			else
			{
				i++;
			}
		}
	}
}
