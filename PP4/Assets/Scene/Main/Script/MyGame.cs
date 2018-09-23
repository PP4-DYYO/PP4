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
	/// バトルが終了して停止する時間
	/// </summary>
	[SerializeField]
	float m_timeWhenTheBattleEndsAndStops;

	/// <summary>
	/// １ｍ毎のサポート率
	/// </summary>
	[SerializeField]
	float m_supportRatePerMeter;

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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_statePrev = GameStatus.Result;
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

			//UIの初期設定
			MainUi.WantedRecruitment();
		}

		//ゲームが開始できるか
		if (OperatingNetPlayerSetting.IsBattleStart())
		{
			//バトルの開始設定
			m_state = GameStatus.BattleSetting;
		}
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

			//設定
			PlayerBattleSettings();
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
			OperatingPlayer.SupportRate =
				(Players.GetMaximumAltitude() - OperatingPlayer.transform.position.y + 1) * m_supportRatePerMeter;
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
		if (Input.GetButtonDown("AButton"))
		{
			m_state = GameStatus.RecruitPeople;
			OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
		}
		else if (Input.GetButtonDown("BButton"))
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
}
