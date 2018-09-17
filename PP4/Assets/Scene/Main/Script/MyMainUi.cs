////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインのUI
/// </summary>
public class MyMainUi : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// ゲーム
	/// </summary>
	[SerializeField]
	MyGame Game;
	public MyGame GameScript
	{
		get { return Game; }
	}

	/// <summary>
	/// 人材募集の画面
	/// </summary>
	[SerializeField]
	GameObject RecruitPeopleScreen;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	[SerializeField]
	Text[] PlayerNames;

	/// <summary>
	/// ゲームを開始するメッセージ
	/// </summary>
	[SerializeField]
	Text MessageToStartGame;

	/// <summary>
	/// タイマー
	/// </summary>
	[SerializeField]
	Text Timer;

	/// <summary>
	/// カウントダウン
	/// </summary>
	[SerializeField]
	Text Countdown;

	/// <summary>
	/// バトル終了
	/// </summary>
	[SerializeField]
	GameObject BattleEnd;

	/// <summary>
	/// 結果画面
	/// </summary>
	[SerializeField]
	GameObject ResultScreen;

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	[SerializeField]
	Text ScoreOfTeam1;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	[SerializeField]
	Text ScoreOfTeam2;

	/// <summary>
	/// 勝ち
	/// </summary>
	[SerializeField]
	GameObject Win;

	/// <summary>
	/// 負け
	/// </summary>
	[SerializeField]
	GameObject Defeat;
	#endregion

	/// <summary>
	/// 募集中のメッセージ
	/// </summary>
	const string WANTED_MESSAGE = "募集中...";

	/// <summary>
	/// ネットワークプレイヤー設定たち
	/// </summary>
	MyNetPlayerSetting[] NetPlayerSettings;

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	float m_team1Score;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	float m_team2Score;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人材募集をする
	/// </summary>
	public void WantedRecruitment()
	{
		RecruitPeopleScreen.SetActive(true);

		MessageToStartGame.enabled = false;
		BattleEnd.SetActive(false);
		ResultScreen.SetActive(false);
		Win.SetActive(false);
		Defeat.SetActive(false);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接続しているプレイヤーを一覧表示
	/// </summary>
	/// <param name="netPlayerSettings">ネットワークプレイヤー設定たち</param>
	public void ListConnectedPlayers(MyNetPlayerSetting[] netPlayerSettings)
	{
		NetPlayerSettings = netPlayerSettings;

		//表示のリセット
		ResetRecruitPeopleScreen();

		//プレイヤー名を初めから登録
		for (var i = 0; i < NetPlayerSettings.Length; i++)
		{
			RegisterPlayerName(i, NetPlayerSettings[i].PlayerName);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人材募集画面のリセット
	/// </summary>
	void ResetRecruitPeopleScreen()
	{
		RecruitPeopleScreen.SetActive(true);

		MessageToStartGame.enabled = false;
		foreach (var playerName in PlayerNames)
		{
			playerName.text = WANTED_MESSAGE;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー名を登録
	/// </summary>
	/// <param name="num">プレイヤー番号</param>
	/// <param name="name">名前</param>
	public void RegisterPlayerName(int num, string name)
	{
		if (num >= PlayerNames.Length)
			return;

		PlayerNames[num].text = name;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル開始設定
	/// </summary>
	public void BattleStartSetting()
	{
		//人材募集画面
		MessageToStartGame.enabled = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル開始
	/// </summary>
	public void BattleStart()
	{
		RecruitPeopleScreen.SetActive(false);

		Timer.text = Game.BattleTime.ToString("F0");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// カウントダウンを設定する
	/// </summary>
	/// <param name="time">時間</param>
	public void SetCountdown(float time = 0)
	{
		//表示・非表示
		if (time == 0)
			Countdown.enabled = false;
		else
			Countdown.enabled = true;

		Countdown.text = time.ToString("F0");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイマーを設定する
	/// </summary>
	/// <param name="time">時間</param>
	public void SetTimer(float time = 0)
	{
		Timer.text = time.ToString("F0");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを終了
	/// </summary>
	public void EndBattle()
	{
		BattleEnd.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム１スコアの計算
	/// </summary>
	/// <param name="scores">得点たち</param>
	public void CalculateScoreOfTeam1(float[] scores)
	{
		m_team1Score = 0;

		foreach (var score in scores)
		{
			m_team1Score += score;
		}

		ScoreOfTeam1.text = m_team1Score.ToString("F2") + StageInfo.UNIT_SYMBOL;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム２スコアの計算
	/// </summary>
	/// <param name="scores">得点たち</param>
	public void CalculateScoreOfTeam2(float[] scores)
	{
		m_team2Score = 0;

		foreach (var score in scores)
		{
			m_team2Score += score;
		}

		ScoreOfTeam2.text = m_team2Score.ToString("F2") + StageInfo.UNIT_SYMBOL;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 結果
	/// </summary>
	public void Result()
	{
		ResultScreen.SetActive(true);

		//操作キャラのチーム
		switch (Game.OperationgNetPlayerSettingScript.TeamNum)
		{
			case Team.Team1:
				//勝敗
				if (m_team1Score >= m_team2Score)
					Win.SetActive(true);
				else
					Defeat.SetActive(true);
				break;
			case Team.Team2:
				//勝敗
				if (m_team1Score <= m_team2Score)
					Win.SetActive(true);
				else
					Defeat.SetActive(true);
				break;
		}
	}
}
