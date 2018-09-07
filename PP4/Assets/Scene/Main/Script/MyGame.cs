////////////////////////////////////////////////////////////////////////////////////////////////////
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
enum GameStatus
{
	/// <summary>
	/// 人を募集
	/// </summary>
	RecruitPeople,
	/// <summary>
	/// ゲーム開始
	/// </summary>
	GameStart,
	/// <summary>
	/// ゲーム中
	/// </summary>
	Game,
	/// <summary>
	/// ゲーム終了
	/// </summary>
	GameEnd,
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

	/// <summary>
	/// フレーム前の状態
	/// </summary>
	GameStatus m_statePrev;
	#endregion

	#region 時間
	[Header("時間")]
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

	/// <summary>
	/// 状態の時間を数える
	/// </summary>
	float m_countTheTimeOfTheState;
	#endregion

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	float m_scoreOfTeam1;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	float m_scoreOfTeam2;

	/// <summary>
	/// 人数募集の終了フラグ
	/// </summary>
	bool m_isEndPeopleRecruitment;
	public bool IsEndPeopleRecruitment
	{
		set { m_isEndPeopleRecruitment = value; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_statePrev = GameStatus.GameEnd;
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
			case GameStatus.GameStart:
				GameStartStateProcess();
				break;
			case GameStatus.Game:
				GameStateProcess();
				break;
			case GameStatus.GameEnd:
				GameEndStateProcess();
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
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;
		}

		//人数募集の終了
		if (m_isEndPeopleRecruitment)
			m_state = GameStatus.GameStart;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲームスタート状態の処理
	/// </summary>
	void GameStartStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//プレイヤーの停止
			OperatingPlayer.enabled = false;
		}

		//カウントダウン時間が過ぎた
		if (m_countTheTimeOfTheState >= m_initialCountdownTime)
		{
			//状態遷移とプレイヤー設定
			m_state = GameStatus.Game;
			OperatingPlayer.enabled = true;

			//カウントダウン反映
			MainUi.CountdownText.text = "";
		}
		else
		{
			//カウントダウン反映
			MainUi.CountdownText.text = ((int)(m_initialCountdownTime - m_countTheTimeOfTheState) + 1).ToString();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲーム状態の処理
	/// </summary>
	void GameStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;
		}

		//バトル時間が過ぎた
		if (m_countTheTimeOfTheState >= m_battleTime)
		{
			//状態遷移
			m_state = GameStatus.GameEnd;
			MainUi.TimerText.text = 0.ToString();
		}
		else
		{
			//タイマー反映
			MainUi.TimerText.text = ((int)(m_battleTime - m_countTheTimeOfTheState)).ToString();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲーム終了状態の処理
	/// </summary>
	void GameEndStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//プレイヤーの停止
			OperatingPlayer.enabled = false;

			//勝敗を決める
			DecideOnWinningOrLosing();
		}

		//もう一度か終了か
		if(Input.GetButtonDown("AButton"))
		{
			MainUi.ResultScreenObj.SetActive(false);
			m_state = GameStatus.GameStart;
		}
		else if(Input.GetButtonDown("BButton"))
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
		//スコアの取得
		m_scoreOfTeam1 = Players.GetTeam1HeightTotal();
		m_scoreOfTeam2 = Players.GetTeam2HeightTotal();

		//表示
		MainUi.ResultScreenObj.SetActive(true);
		MainUi.ScoreOfTeam1Text.text = m_scoreOfTeam1.ToString();
		MainUi.ScoreOfTeam2Text.text = m_scoreOfTeam2.ToString();
	}
}
