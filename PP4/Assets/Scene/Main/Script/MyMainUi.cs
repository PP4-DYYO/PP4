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
	/// フェードインアウト
	/// </summary>
	[SerializeField]
	Image FadeInOut;

	/// <summary>
	/// 人材募集の画面
	/// </summary>
	[SerializeField]
	GameObject RecruitPeopleScreen;

	/// <summary>
	/// 人を待つ時間
	/// </summary>
	[SerializeField]
	Text TimeToWaitWorPeople;

	/// <summary>
	/// ゲームを開始するメッセージ
	/// </summary>
	[SerializeField]
	GameObject MessageToStartGame;

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

	#region フェードインアウト
	[Header("フェードインアウト")]
	/// <summary>
	/// フェードインアウトの時間
	/// </summary>
	[SerializeField]
	float m_fadeInOutTime;

	/// <summary>
	/// フェードインアウトの時間を数える
	/// </summary>
	float m_countFadeInOut;

	/// <summary>
	/// フェードインが動作している
	/// </summary>
	bool m_isFadeInRunning;

	/// <summary>
	/// フェードアウトが動作している
	/// </summary>
	bool m_isFadeOutRunning;

	/// <summary>
	/// フェードインアウト用の色
	/// </summary>
	Color m_fadeInOutColor;
	#endregion

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	float m_team1Score;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	float m_team2Score;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		if (m_isFadeOutRunning)
			FadeOutProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フェードアウト処理
	/// </summary>
	void FadeOutProcess()
	{
		m_countFadeInOut += Time.deltaTime;

		//時間に応じてαを増やす
		m_fadeInOutColor = FadeInOut.color;
		m_fadeInOutColor.a = m_countFadeInOut / m_fadeInOutTime;
		FadeInOut.color = m_fadeInOutColor;

		//時間が過ぎた
		if (m_countFadeInOut >= m_fadeInOutTime)
			m_isFadeOutRunning = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人材募集をする
	/// </summary>
	public void WantedRecruitment()
	{
		RecruitPeopleScreen.SetActive(true);

		MessageToStartGame.SetActive(false);
		BattleEnd.SetActive(false);
		ResultScreen.SetActive(false);
		Win.SetActive(false);
		Defeat.SetActive(false);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を待つ時間の設定
	/// </summary>
	/// <param name="time">時間</param>
	public void SetTimeToWaitWorPeople(float time)
	{
		TimeToWaitWorPeople.text = ((int)time).ToString();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった
	/// </summary>
	public void PeopleGathered()
	{
		//ゲーム開始のメッセージ
		MessageToStartGame.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル開始設定
	/// </summary>
	public void BattleStartSetting()
	{
		StartFadeOut();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フェードアウトを開始
	/// </summary>
	void StartFadeOut()
	{
		m_isFadeOutRunning = true;
		m_isFadeInRunning = false;
		m_countFadeInOut = 0;
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
