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
		set { m_netPlayerSettings = value; }
	}

	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// ゲーム
	/// </summary>
	MyMainGame Game;
	#endregion

	#region コンポーネント
	[Header("コンポーネント")]
	/// <summary>
	/// プレイヤー
	/// </summary>
	[SerializeField]
	MyPlayer Player;

	/// <summary>
	/// 名札たち
	/// </summary>
	[SerializeField]
	TextMesh[] Nameplates;
	
	/// <summary>
	/// アニメーター
	/// </summary>
	[SerializeField]
	Animator Anim;

	/// <summary>
	/// 選択されたスキン
	/// </summary>
	MySkin m_selectSkin;
	public MySkin SelectSkin
	{
		get { return m_selectSkin; }
	}
	#endregion

	#region プレイヤーのタイプ
	[Header("プレイヤーのタイプ")]
	/// <summary>
	/// スキン達
	/// </summary>
	[SerializeField]
	GameObject[] Skin;
	#endregion

	#region プレイヤーの情報
	[Header("プレイヤーの情報")]
	/// <summary>
	/// 自分自身の表示名
	/// </summary>
	[SerializeField]
	string m_yourOwnDisplayName;

	/// <summary>
	/// 状態
	/// </summary>
	[SyncVar(hook = "SyncState")]
	PlayerBehaviorStatus m_state;

	/// <summary>
	/// フレーム前の状態
	/// </summary>
	PlayerBehaviorStatus m_statePrev;

	/// <summary>
	/// プレイヤー番号
	/// </summary>
	int m_playerNum;
	public int PlayerNum
	{
		get { return m_playerNum; }
		set { m_playerNum = value; }
	}

	/// <summary>
	/// 順位
	/// </summary>
	int m_rank;
	public int Rank
	{
		get { return m_rank; }
		set { m_rank = value; }
	}

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
	/// プレイヤータイプ
	/// </summary>
	[SyncVar(hook = "SyncPlayerType")]
	int m_playerType = int.MaxValue;

	/// <summary>
	/// パワー
	/// </summary>
	[SyncVar(hook = "SyncPower")]
	int m_power;
	public int Power
	{
		get { return m_power; }
	}

	/// <summary>
	/// レベル
	/// </summary>
	[SyncVar(hook = "SyncLevel")]
	int m_level;
	public int Level
	{
		get { return m_level; }
		set { m_level = value; }
	}

	/// <summary>
	/// 最終高度
	/// </summary>
	[SyncVar(hook = "SyncFinalAltitude")]
	float m_finalAltitude;
	public float FinalAltitude
	{
		get { return m_finalAltitude; }
		set { m_finalAltitude = value; }
	}

	/// <summary>
	/// コイン枚数
	/// </summary>
	[SyncVar(hook = "SyncNumOfCoins")]
	int m_numOfCoins;
	public int NumOfCoins
	{
		get { return m_numOfCoins; }
		set { m_numOfCoins = value; }
	}

	/// <summary>
	/// スコア
	/// </summary>
	[SyncVar(hook = "SyncScore")]
	int m_score;
	public int Score
	{
		get { return m_score; }
		set { m_score = value; }
	}
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

	#region 作業用
	/// <summary>
	/// 変更される番号
	/// </summary>
	static int m_numToBeChanged;

	/// <summary>
	/// 対象の番号
	/// </summary>
	static int m_targetNum;

	/// <summary>
	/// 作業用Int
	/// </summary>
	int m_workInt;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// クライアントの初期
	/// </summary>
	public override void OnStartClient()
	{
		base.OnStartClient();

		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyMainGame>();

		//権限のないプレイヤーになる
		Player.BecomeUnauthorizedPlayer();

		m_netPlayerSettings.Add(this);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ローカルプレイヤーの初期
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		//ゲームに必要な設定
		Game.OperatingPlayerScript = Player;
		transform.parent = Game.PlayersScript.transform;

		//名前の登録
		CmdRegisterPlayerName(MyGameInfo.Instance.PlayerName);

		//タイプの登録
		CmdRegisterPlayerType(MyGameInfo.Instance.TypeNum);

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
		if (isLocalPlayer)
		{
			//既存プレイヤーの名前登録
			foreach (var player in m_netPlayerSettings)
			{
				foreach(var nameplate in player.Nameplates)
				{
					nameplate.text = player.m_playerName;
				}
			}

			//自分の名を切り替える
			ChangeDisplayNameOfNameplate();
		}
		else
		{
			//プレイヤー名の登録
			foreach(var nameplate in Nameplates)
			{
				nameplate.text = playerName;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤータイプ登録のサーバ通知
	/// </summary>
	/// <param name="typeNum">タイプ番号</param>
	[Command]
	void CmdRegisterPlayerType(int typeNum)
	{
		m_playerType = typeNum;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤータイプを同期する
	/// </summary>
	/// <param name="typeNum">タイプ番号</param>
	[Client]
	void SyncPlayerType(int typeNum)
	{
		m_playerType = typeNum;

		//自分のタイプの同期
		if (isLocalPlayer)
		{
			//既存プレイヤーのタイプ登録
			foreach (var player in m_netPlayerSettings)
			{
				//スキンの生成と設定
				player.m_selectSkin = Instantiate(player.Skin[player.m_playerType], player.transform).GetComponent<MySkin>();
				ResetTransform(player.m_selectSkin.transform);
				player.m_selectSkin.SetSkin(player.GetComponent<MyPlayer>(), player.GetComponent<MyNetPlayerSetting>());
			}
		}
		else
		{
			//スキンの生成と設定
			m_selectSkin = Instantiate(Skin[typeNum], transform).GetComponent<MySkin>();
			ResetTransform(m_selectSkin.transform);
			m_selectSkin.SetSkin(GetComponent<MyPlayer>(), GetComponent<MyNetPlayerSetting>());
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// Transformのリセット
	/// </summary>
	/// <param name="trans"></param>
	void ResetTransform(Transform trans)
	{
		trans.localPosition = Vector3.zero;
		trans.localRotation = Quaternion.identity;
		trans.localScale = Vector3.one;
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
	///	定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//インスタンスの取得
		if (!Game)
			Game = GameObject.Find("Game").GetComponent<MyMainGame>();

		//名札の方向
		foreach(var nameplate in Nameplates)
		{
			nameplate.transform.LookAt(nameplate.transform.position + (nameplate.transform.position - Camera.main.transform.position));
		}

		//アニメーション処理
		AnimProcess();

		//ジェットウォータ処理
		JetWaterProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーション処理
	/// </summary>
	void AnimProcess()
	{
		//操作プレイヤー
		if (isLocalPlayer)
		{
			//状態が変わった
			if (m_state != Player.State)
			{
				//状態の通知
				m_state = Player.State;
				CmdState(m_state);
			}
			return;
		}

		//未設定
		if (Anim == null)
			return;

		//状態とアニメーション遷移が同じ
		if ((int)m_state == Anim.GetInteger(PlayerInfo.ANIM_PARAMETER_NAME))
		{
			//状態遷移を無効に
			Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)PlayerBehaviorStatus.Non);
			return;
		}

		//現在のアニメーションと状態が同じ
		if (Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains(m_state.ToString()))
			return;

		//遷移変更
		Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)m_state);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 状態の通知
	/// </summary>
	/// <param name="state">状態</param>
	[Command]
	void CmdState(PlayerBehaviorStatus state)
	{
		m_state = state;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 状態を同期する
	/// </summary>
	/// <param name="state">状態</param>
	[Client]
	void SyncState(PlayerBehaviorStatus state)
	{
		m_state = state;
	}
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ジェットウォータ処理
	/// </summary>
	void JetWaterProcess()
	{
		//未設定
		if (Player.JetWaterScript == null)
			return;

		//状態が変わった
		if (m_state != m_statePrev)
		{
			//状態によって、ジェットウォータの起動と停止
			Player.LaunchJetWater(
				!(m_state == PlayerBehaviorStatus.Idle || m_state == PlayerBehaviorStatus.Falling || m_state == PlayerBehaviorStatus.Stand
				|| m_state == PlayerBehaviorStatus.HoldBoardInHand || m_state == PlayerBehaviorStatus.HoldBoardInHand2
				|| m_state == PlayerBehaviorStatus.Result
				|| m_state == PlayerBehaviorStatus.SuccessfulLanding || m_state == PlayerBehaviorStatus.LandingFailed
				|| m_state == PlayerBehaviorStatus.ResultsRanked1st || m_state == PlayerBehaviorStatus.ResultsRanked2nd
				|| m_state == PlayerBehaviorStatus.ResultsRanked3rd || m_state == PlayerBehaviorStatus.ResultsRanked4th
				|| m_state == PlayerBehaviorStatus.Win || m_state == PlayerBehaviorStatus.Defeat
				|| m_state == PlayerBehaviorStatus.Run || m_state == PlayerBehaviorStatus.Select));

			m_statePrev = m_state;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スキンを設定
	/// </summary>
	/// <param name="anim">アニメータ</param>
	public void SetSkin(Animator anim)
	{
		Anim = anim;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 名札の表示
	/// </summary>
	/// <param name="isDisplay">表示するか</param>
	public void NameplateDisplay(bool isDisplay = true)
	{
		foreach (var nameplate in Nameplates)
		{
			nameplate.gameObject.SetActive(isDisplay);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ネットプレイヤー番号を取得
	/// </summary>
	/// <returns>ネットプレイヤー番号</returns>
	public int GetNetPlayerNum()
	{
		return m_netPlayerSettings.IndexOf(this);
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
		for (m_workInt = 0; m_workInt < m_netPlayerSettings.Count; m_workInt++)
		{
			//準備完了でない
			if (!m_netPlayerSettings[m_workInt].m_isReady)
				return false;
		}

		//プレイヤー人数が揃うか
		return (m_netPlayerSettings.Count >= Game.NumOfPlayers);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// レベルの通知
	/// </summary>
	/// <param name="level">レベル</param>
	[Command]
	public void CmdLevel(int level)
	{
		m_level = level;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// レベルの同期
	/// </summary>
	/// <param name="level">レベル</param>
	[Client]
	public void SyncLevel(int level)
	{
		m_level = level;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// パワーの通知
	/// </summary>
	/// <param name="power">パワー</param>
	[Command]
	public void CmdPower(int power)
	{
		m_power = power;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// パワーの同期
	/// </summary>
	/// <param name="power">パワー</param>
	[Client]
	public void SyncPower(int power)
	{
		m_power = power;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 戦績にプレイヤー情報を入れる
	/// </summary>
	/// <param name="battleRecords">戦績たち</param>
	public static void PutPlayerInfoInBattleRecord(ref PublishRecord[] battleRecords)
	{
		//全ての戦績
		for (var i = 0; i < battleRecords.Length; i++)
		{
			//プレイヤー情報の代入
			m_netPlayerSettings[i].Rank = 0;
			battleRecords[i].rank = m_netPlayerSettings[i].Rank;
			battleRecords[i].level = m_netPlayerSettings[i].Level;
			battleRecords[i].playerName = m_netPlayerSettings[i].m_playerName;
			battleRecords[i].height = 0;
			battleRecords[i].numOfCoins = 0;
			battleRecords[i].score = 0;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 最終高度の通知
	/// </summary>
	/// <param name="finalAltitude">最終高度</param>
	[Command]
	public void CmdFinalAltitude(float finalAltitude)
	{
		m_finalAltitude = finalAltitude;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 最終高度の同期
	/// </summary>
	/// <param name="finalAltitude">最終高度</param>
	[Client]
	public void SyncFinalAltitude(float finalAltitude)
	{
		m_finalAltitude = finalAltitude;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コイン枚数の通知
	/// </summary>
	/// <param name="numOfCoins">コイン枚数</param>
	[Command]
	public void CmdNumOfCoins(int numOfCoins)
	{
		m_numOfCoins = numOfCoins;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コイン枚数の同期
	/// </summary>
	/// <param name="numOfCoins">コイン枚数</param>
	[Client]
	public void SyncNumOfCoins(int numOfCoins)
	{
		m_numOfCoins = numOfCoins;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スコアの通知
	/// </summary>
	/// <param name="score">スコア</param>
	[Command]
	public void CmdScore(int score)
	{
		m_score = score;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スコアの同期
	/// </summary>
	/// <param name="score">スコア</param>
	[Client]
	public void SyncScore(int score)
	{
		m_score = score;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 戦績にプレイヤー成果を入れる
	/// </summary>
	/// <param name="battleRecords">戦績たち</param>
	public static void PutPlayerAchievementsInBattleRecord(ref PublishRecord[] battleRecords)
	{
		//評価されるネットプレイヤー
		for (m_numToBeChanged = 0; m_numToBeChanged < m_netPlayerSettings.Count - 1; m_numToBeChanged++)
		{
			//評価するネットプレイヤー
			for (m_targetNum = m_numToBeChanged + 1; m_targetNum < m_netPlayerSettings.Count; m_targetNum++)
			{
				//低いスコアのプレイヤーが順位が下がる
				if (m_netPlayerSettings[m_numToBeChanged].Score > m_netPlayerSettings[m_targetNum].Score)
					m_netPlayerSettings[m_targetNum].Rank++;
				else
					m_netPlayerSettings[m_numToBeChanged].Rank++;
			}
		}

		//全ての戦績
		for (m_targetNum = 0; m_targetNum < battleRecords.Length; m_targetNum++)
		{
			//ネットワーク切れのプレイヤー
			if (!m_netPlayerSettings[m_targetNum])
				continue;

			//プレイヤー成果の代入
			battleRecords[m_targetNum].height = (int)m_netPlayerSettings[m_targetNum].FinalAltitude;
			battleRecords[m_targetNum].numOfCoins = m_netPlayerSettings[m_targetNum].NumOfCoins;
			battleRecords[m_targetNum].score = m_netPlayerSettings[m_targetNum].Score;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続が切れたプレイヤーの確認
	/// </summary>
	void ConfirmationOfPlayersWhoAreDisconnected()
	{
		for (m_workInt = 0; m_workInt < m_netPlayerSettings.Count;)
		{
			//存在していない
			if (!m_netPlayerSettings[m_workInt])
				m_netPlayerSettings.RemoveAt(m_workInt);
			else
				m_workInt++;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 名札の表示名を切り替える
	/// </summary>
	public void ChangeDisplayNameOfNameplate()
	{
		foreach (var nameplate in Nameplates)
		{
			nameplate.text = (nameplate.text == m_playerName) ? m_yourOwnDisplayName : m_playerName;
		}
	}
}
