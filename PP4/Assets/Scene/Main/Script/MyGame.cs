﻿////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/17～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ゲームの状態
/// </summary>
public enum GameStatus
{
	/// <summary>
	/// 人を募集
	/// </summary>
	RecruitPeople,
	/// <summary>
	/// バトル設定
	/// </summary>
	BattleSetting,
	/// <summary>
	/// バトル開始
	/// </summary>
	BattleStart,
	/// <summary>
	/// バトル中
	/// </summary>
	Battle,
	/// <summary>
	/// バトル終了
	/// </summary>
	BattleEnd,
	/// <summary>
	/// 結果
	/// </summary>
	Result,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ゲーム
/// </summary>
public class MyGame : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// プレイヤーの収集物
	/// </summary>
	[SerializeField]
	MyPlayers Players;
	public MyPlayers PlayersScript
	{
		get { return Players; }
	}

	/// <summary>
	/// ゴーストプレイヤー達
	/// </summary>
	[SerializeField]
	GameObject GhostPlayers;

	/// <summary>
	/// ステージ
	/// </summary>
	[SerializeField]
	MyStage Stage;
	public MyStage StageScript
	{
		get { return Stage; }
	}

	/// <summary>
	/// 操作しているカメラ
	/// </summary>
	[SerializeField]
	MyCamera OperatingCamera;
	public MyCamera OperatingCameraScript
	{
		get { return OperatingCamera; }
	}

	/// <summary>
	/// メインのUI
	/// </summary>
	[SerializeField]
	MyMainUi MainUi;
	public MyMainUi MainUiScript
	{
		get { return MainUi; }
	}

	/// <summary>
	/// 操作しているプレイヤー
	/// </summary>
	MyPlayer OperatingPlayer;
	public MyPlayer OperatingPlayerScript
	{
		get { return OperatingPlayer; }
		set { OperatingPlayer = value; }
	}

	/// <summary>
	/// 操作しているネットワークプレイヤー設定
	/// </summary>
	MyNetPlayerSetting OperatingNetPlayerSetting;
	public MyNetPlayerSetting OperationgNetPlayerSettingScript
	{
		get { return OperatingNetPlayerSetting; }
	}
	#endregion

	#region 状態
	[Header("状態")]
	/// <summary>
	/// 状態
	/// </summary>
	GameStatus m_state;
	public GameStatus State
	{
		get { return m_state; }
	}

	/// <summary>
	/// フレーム前の状態
	/// </summary>
	GameStatus m_statePrev;
	#endregion

	#region ゲームの情報
	[Header("ゲームの情報")]
	/// <summary>
	/// 人を待つ時間
	/// </summary>
	[SerializeField]
	float m_timeToWaitForPeople;

	/// <summary>
	/// 人を待つ時のプレイヤー位置
	/// </summary>
	[SerializeField]
	Vector3[] m_playerPosWhenWaitingForPeople;

	/// <summary>
	/// 人を待つ時のプレイヤー向き
	/// </summary>
	[SerializeField]
	Vector3[] m_playerDirectionWhenWaitingForPeople;

	/// <summary>
	/// 人を待つときのカメラ位置
	/// </summary>
	[SerializeField]
	Vector3 m_cameraPosWhenWaitingForPeople;

	/// <summary>
	/// 人を待つときのカメラ向き
	/// </summary>
	[SerializeField]
	Vector3 m_cameraDirectionWhenWaitingForPeople;

	/// <summary>
	/// バトル設定時間
	/// </summary>
	[SerializeField]
	float m_battleSettingTime;

	/// <summary>
	/// 初めのカウントダウン時間
	/// </summary>
	[SerializeField]
	float m_initialCountdownTime;

	/// <summary>
	/// バトル時間
	/// </summary>
	[SerializeField]
	float m_battleTime;
	public float BattleTime
	{
		get { return m_battleTime; }
	}

	/// <summary>
	/// １ｍ毎のサポート率
	/// </summary>
	[SerializeField]
	float m_supportRatePerMeter;

	/// <summary>
	/// バトルが終了して停止する時間
	/// </summary>
	[SerializeField]
	float m_timeWhenTheBattleEndsAndStops;

	/// <summary>
	/// 状態の時間を数える
	/// </summary>
	float m_countTheTimeOfTheState;

	/// <summary>
	/// プレイヤー人数
	/// </summary>
	public const int NUM_OF_PLAYERS = 2;
	#endregion

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	float m_scoreOfTeam1;

	/// <summary>
	/// チーム１のスコア配列
	/// </summary>
	float[] m_scoreOfTeam1Array = new float[(NUM_OF_PLAYERS + 1) / 2];

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	float m_scoreOfTeam2;

	/// <summary>
	/// チーム２のスコア配列
	/// </summary>
	float[] m_scoreOfTeam2Array = new float[(NUM_OF_PLAYERS + 1) / 2];

	/// <summary>
	/// 操作しているプレイヤーの番号
	/// </summary>
	int m_operatingPlayerNum;

	#region キーボード関係
	[Header("キーボード関係")]
	/// <summary>
	/// Aボタンを押した
	/// </summary>
	bool m_isAButtonDown;

	/// <summary>
	/// Bボタンを押した
	/// </summary>
	bool m_isBButtonDown;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_state = GameStatus.RecruitPeople;
		m_statePrev = GameStatus.Result;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		if(Input.GetButtonDown("AButton"))
			m_isAButtonDown = true;
		if(Input.GetButtonDown("BButton"))
			m_isBButtonDown = true;
	}
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//全体の状態初期設定
		if (m_state != m_statePrev)
		{
			m_countTheTimeOfTheState = 0;
		}

		//状態
		switch (m_state)
		{
			case GameStatus.RecruitPeople:
				RecruitPeopleStateProcess();
				break;
			case GameStatus.BattleSetting:
				BattleSettingStateProcess();
				break;
			case GameStatus.BattleStart:
				BattleStartStateProcess();
				break;
			case GameStatus.Battle:
				BattleStateProcess();
				break;
			case GameStatus.BattleEnd:
				BattleEndStateProcess();
				break;
			case GameStatus.Result:
				ResultStateProcess();
				break;
		}

		//時間経過
		m_countTheTimeOfTheState += Time.deltaTime;

		//入力のリセット
		ResetInput();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を募集する状態の処理
	/// </summary>
	void RecruitPeopleStateProcess()
	{
		//操作しているプレイヤーが登録されていない
		if (!OperatingPlayer)
			return;

		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//必要なインスタンス
			if (!OperatingNetPlayerSetting)
				OperatingNetPlayerSetting = OperatingPlayer.GetComponent<MyNetPlayerSetting>();

			//プレイヤーとカメラとUI
			MovePlayerToPosToWaitForPeople(OperatingNetPlayerSetting.GetPlayerNum());
			GhostPlayers.SetActive(true);
			OperatingCamera.BecomeFixedCamera(m_cameraPosWhenWaitingForPeople, m_cameraDirectionWhenWaitingForPeople);
			MainUi.WantedRecruitment();
		}

		//ゲームが開始できるか
		if (OperatingNetPlayerSetting.IsBattleStart())
		{
			//バトルの開始設定
			m_state = GameStatus.BattleSetting;
		}
		else if(m_countTheTimeOfTheState > m_timeToWaitForPeople)
		{
			//プレイヤーの解放（ネット接続を切る）
			FindObjectOfType<MyNetworkManager>().StopConnection();
		}
		else
		{
			//解放時間を更新
			MainUi.SetTimeToWaitWorPeople(m_timeToWaitForPeople - m_countTheTimeOfTheState);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を待つ位置にプレイヤーを移動する
	/// </summary>
	/// <param name="playerNum">プレイヤー番号</param>
	void MovePlayerToPosToWaitForPeople(int playerNum)
	{
		OperatingPlayer.StandAtSpecifiedPos(m_playerPosWhenWaitingForPeople[playerNum], m_playerDirectionWhenWaitingForPeople[playerNum]);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル設定状態の処理
	/// </summary>
	void BattleSettingStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//プレイヤーとカメラとUI
			PlayerBattleSettings();
			GhostPlayers.SetActive(false);
			OperatingCamera.BecomePursuitCamera();
			MainUi.BattleStartSetting();
		}

		//設定時間が過ぎた
		if (m_countTheTimeOfTheState >= m_battleSettingTime)
		{
			//状態遷移
			m_state = GameStatus.BattleStart;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーのバトル設定
	/// </summary>
	void PlayerBattleSettings()
	{
		//準備
		OperatingPlayer.PreparationForBattleSetting();

		//チーム分け
		m_operatingPlayerNum = OperatingNetPlayerSetting.GetPlayerNum();
		Players.DecideOnTeam(MyNetPlayerSetting.NetPlayerSettings.ToArray());

		//チームによる位置
		switch (OperatingNetPlayerSetting.TeamNum)
		{
			case Team.Team1:
				OperatingPlayer.transform.position =
					Stage.CurrentFieldScript.Team1StartPositions[OperatingPlayer.transform.GetSiblingIndex()];
				break;
			case Team.Team2:
				OperatingPlayer.transform.position =
					Stage.CurrentFieldScript.Team2StartPositions[OperatingPlayer.transform.GetSiblingIndex()];
				break;
		}
	}
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルスタート状態の処理
	/// </summary>
	void BattleStartStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			OperatingPlayer.MakeItBattleStartState();
			OperatingCamera.SetPosition(OperatingPlayer.transform.position);
			MainUi.BattleStart();
		}

		//カウントダウン時間が過ぎた
		if (m_countTheTimeOfTheState >= m_initialCountdownTime)
		{
			//状態遷移
			m_state = GameStatus.Battle;

			//カウントダウン反映
			MainUi.SetCountdown();
		}
		else
		{
			//カウントダウン反映
			MainUi.SetCountdown(m_initialCountdownTime - m_countTheTimeOfTheState);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態の処理
	/// </summary>
	void BattleStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			OperatingPlayer.MakeItBattleState();
		}

		//バトル時間が過ぎた
		if (m_countTheTimeOfTheState >= m_battleTime)
		{
			//状態遷移
			m_state = GameStatus.BattleEnd;
			MainUi.SetTimer();
		}
		else
		{
			//タイマー反映
			MainUi.SetTimer(m_battleTime - m_countTheTimeOfTheState);

			//プレイヤーサポート(サポート率０回避付き)
			OperatingPlayer.SupportRate = 1 +
				(Players.GetMaximumAltitude() - OperatingPlayer.transform.position.y) * (m_supportRatePerMeter - 1);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲーム終了の処理
	/// </summary>
	void BattleEndStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			OperatingPlayer.MakeItBattleEndState();
			MainUi.EndBattle();
		}

		//バトル終了時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeWhenTheBattleEndsAndStops)
		{
			m_state = GameStatus.Result;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 結果状態の処理
	/// </summary>
	void ResultStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//勝敗を決める
			DecideOnWinningOrLosing();

			//勝敗確認のため
			OperatingNetPlayerSetting.CmdNotifyOfIsReady(false);
		}

		//もう一度か終了か
		if (m_isAButtonDown)
		{
			m_state = GameStatus.RecruitPeople;
			OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
		}
		else if (m_isBButtonDown)
		{
			Debug.Log("終了");
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 勝敗を決める
	/// </summary>
	void DecideOnWinningOrLosing()
	{
		//高さの取得
		m_scoreOfTeam1 = Players.GetTeam1HeightTotal(ref m_scoreOfTeam1Array);
		m_scoreOfTeam2 = Players.GetTeam2HeightTotal(ref m_scoreOfTeam2Array);

		//表示
		MainUi.CalculateScoreOfTeam1(m_scoreOfTeam1Array);
		MainUi.CalculateScoreOfTeam2(m_scoreOfTeam2Array);
		MainUi.Result();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力のリセット
	/// </summary>
	void ResetInput()
	{
		m_isAButtonDown = false;
		m_isBButtonDown = false;
	}

}
