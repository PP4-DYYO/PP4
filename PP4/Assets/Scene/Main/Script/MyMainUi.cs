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
	/// 準備完了メッセージ
	/// </summary>
	[SerializeField]
	MyImageAnimation ReadyMessage;

	/// <summary>
	/// Goメッセージ
	/// </summary>
	[SerializeField]
	MyImageAnimation GoMessage;

	/// <summary>
	/// バトル画面
	/// </summary>
	[SerializeField]
	GameObject BattleScreen;

	/// <summary>
	/// タイマー
	/// </summary>
	[SerializeField]
	Text Timer;

	/// <summary>
	/// 水の残量
	/// </summary>
	[SerializeField]
	Image RemainingAmountOfWater;

	/// <summary>
	/// 順位
	/// </summary>
	[SerializeField]
	GameObject Rank;

	/// <summary>
	/// 順位の親たち
	/// </summary>
	[SerializeField]
	Transform[] ParentsOfRank;

	/// <summary>
	/// マップ上のプレイヤー
	/// </summary>
	[SerializeField]
	GameObject[] PlayersOnMap;

	/// <summary>
	/// マップ上のプレイヤー名
	/// </summary>
	[SerializeField]
	Text[] PlayerNamesOnTheMap;

	/// <summary>
	/// 被水
	/// </summary>
	[SerializeField]
	MyImageAnimation[] WearWaters;

	/// <summary>
	/// 落下
	/// </summary>
	[SerializeField]
	GameObject Falling;

	/// <summary>
	/// 残り時間
	/// </summary>
	[SerializeField]
	MyImageAnimation RemainingTime;

	/// <summary>
	/// バトル終了のカウントダウン
	/// </summary>
	[SerializeField]
	MyImageAnimation CountdownOfBattleFinish;

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

	#region バトル画面
	[Header("バトル画面")]
	/// <summary>
	/// 順位入れ替え時間
	/// </summary>
	[SerializeField]
	float m_rankChangeTime;

	/// <summary>
	/// 順位が上がった時にマップ上の回避する時間
	/// </summary>
	[SerializeField]
	float m_timeToAvoidOnMapWhenRankingGoesUp;

	/// <summary>
	/// 順位が上がった時にマップ上の回避する位置
	/// </summary>
	[SerializeField]
	Vector3 m_posToAvoidOnMapWhenRankingGoesUp;

	/// <summary>
	/// 被水する時間
	/// </summary>
	[SerializeField]
	float m_timeToWearWater;

	/// <summary>
	/// 被水する時のα値
	/// </summary>
	[SerializeField]
	float m_alphaValueWhenWearingWater;

	/// <summary>
	/// 時間を通知するための残り時間
	/// </summary>
	[SerializeField]
	float m_remainingTimeToNotifyTime;

	/// <summary>
	/// バトル終了前のカウントダウン時間
	/// </summary>
	[SerializeField]
	float m_countdownTimeBeforeBattleEnd;

	/// <summary>
	/// １分当たりの秒
	/// </summary>
	const int SECONDS_PER_MINUTE = 60;

	/// <summary>
	/// 時間の区切り文字
	/// </summary>
	const string TIME_SEPARATOR = ":";

	/// <summary>
	/// 操作プレイヤー順位
	/// </summary>
	int m_operatingPlayerRank;

	/// <summary>
	/// フレーム前の操作プレイヤー順位
	/// </summary>
	int m_operatingPlayerRankPrev;

	/// <summary>
	/// 操作プレイヤーの順位が上がったフラグ
	/// </summary>
	bool m_isRankingOfOperationPlayerHasRisen;

	/// <summary>
	/// 順位入れ替え時間を数える
	/// </summary>
	float m_countRankChangeTime;

	/// <summary>
	/// 高さの順位たち
	/// </summary>
	int[] m_heightRanks;

	/// <summary>
	/// 被水する時間を数える
	/// </summary>
	float m_countTimeToWearWater;

	/// <summary>
	/// 被水するフラグ
	/// </summary>
	bool m_isWearWater;

	/// <summary>
	/// 残り時間の通知フラグ
	/// </summary>
	bool m_isRemainingTimeNotification;

	/// <summary>
	/// バトル終了前のカウントダウンフラグ
	/// </summary>
	bool m_isCountdownOfBattleFinish;
	#endregion

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	float m_team1Score;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	float m_team2Score;

	/// <summary>
	/// 対象の番号
	/// </summary>
	int m_targetNum;

	/// <summary>
	/// 作業用のInt
	/// </summary>
	int m_workInt;

	/// <summary>
	/// 作業用のColor
	/// </summary>
	Color m_workColor;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//フェードアウト
		if (m_isFadeOutRunning)
			FadeOutProcess();

		//フェードイン
		if (m_isFadeInRunning)
			FadeInProcess();

		//プレイヤーの順位を上げる
		if (m_isRankingOfOperationPlayerHasRisen)
			RaisePlayerRankProcess();

		//被水する
		if (m_isWearWater)
			WearWaterProcess();
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
	/// フェードイン処理
	/// </summary>
	void FadeInProcess()
	{
		m_countFadeInOut += Time.deltaTime;

		//時間に応じてαを減らす
		m_fadeInOutColor = FadeInOut.color;
		m_fadeInOutColor.a = 1f - (m_countFadeInOut / m_fadeInOutTime);
		FadeInOut.color = m_fadeInOutColor;

		//時間が過ぎた
		if (m_countFadeInOut >= m_fadeInOutTime)
			m_isFadeInRunning = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーの順位を上げる処理
	/// </summary>
	void RaisePlayerRankProcess()
	{
		m_countRankChangeTime += Time.deltaTime;

		//順位が下がるプレイヤー番号
		for(var i = m_operatingPlayerRank; i < m_operatingPlayerRankPrev; i++)
		{
			//時間により下に移動
			ParentsOfRank[i].GetChild(0).localPosition =
				(ParentsOfRank[i + 1].localPosition - ParentsOfRank[i].localPosition) * (m_countRankChangeTime / m_rankChangeTime);
		}

		//回避する時間
		if (m_countRankChangeTime < m_timeToAvoidOnMapWhenRankingGoesUp)
		{
			//順位が上がるプレイヤーが回避
			ParentsOfRank[m_operatingPlayerRankPrev].GetChild(0).localPosition =
				m_posToAvoidOnMapWhenRankingGoesUp * (m_countRankChangeTime / m_timeToAvoidOnMapWhenRankingGoesUp);
		}
		else
		{
			//順位が上がるプレイヤーが本来の位置に移動
			ParentsOfRank[m_operatingPlayerRankPrev].GetChild(0).localPosition =
				((ParentsOfRank[m_operatingPlayerRank].localPosition -
				ParentsOfRank[m_operatingPlayerRankPrev].localPosition - m_posToAvoidOnMapWhenRankingGoesUp)
				* ((m_countRankChangeTime - m_timeToAvoidOnMapWhenRankingGoesUp) / (m_rankChangeTime - m_timeToAvoidOnMapWhenRankingGoesUp)))
				+ m_posToAvoidOnMapWhenRankingGoesUp;
		}

		//順位を上げる処理の終了
		if(m_countRankChangeTime >= m_rankChangeTime)
		{
			m_isRankingOfOperationPlayerHasRisen = false;
			m_operatingPlayerRankPrev = m_operatingPlayerRank;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 被水する処理
	/// </summary>
	void WearWaterProcess()
	{
		m_countTimeToWearWater += Time.deltaTime;

		//全ての被水
		foreach (var wearWater in WearWaters)
		{
			//α値の設定
			m_workColor = wearWater.TargetImage.color;
			m_workColor.a = m_alphaValueWhenWearingWater - (m_alphaValueWhenWearingWater * (m_countTimeToWearWater / m_timeToWearWater));
			wearWater.TargetImage.color = m_workColor;
		}

		//終了
		if(m_countTimeToWearWater >= m_timeToWearWater)
		{
			//全ての被水
			foreach (var wearWater in WearWaters)
			{
				//アニメーションの停止
				wearWater.StopAnimation();
			}
			m_isWearWater = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人材募集をする
	/// </summary>
	public void WantedRecruitment()
	{
		//全体の表示
		RecruitPeopleScreen.SetActive(true);

		//不要オブジェクトの非表示
		MessageToStartGame.SetActive(false);
		BattleScreen.SetActive(false);
		ReadyMessage.StopAnimation();
		GoMessage.StopAnimation();
		Falling.SetActive(false);
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
	/// フェードインを開始
	/// </summary>
	void StartFadeIn()
	{
		m_isFadeInRunning = true;
		m_isFadeOutRunning = false;
		m_countFadeInOut = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル開始
	/// </summary>
	/// <param name="battleTime">バトル時間</param>
	public void BattleStart(float battleTime)
	{
		//不要オブジェクトの非表示
		RecruitPeopleScreen.SetActive(false);

		//必要オブジェクトの設定
		StartFadeIn();
		BattleScreen.SetActive(true);
		SetTimer(battleTime);
		SetRemainingAmountOfWater();
		WritePlayerNamesOnTheMap();
		DisplayRank();
		m_isRemainingTimeNotification = false;
		m_isCountdownOfBattleFinish = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイマーを設定する
	/// </summary>
	/// <param name="time">時間</param>
	public void SetTimer(float time = 0)
	{
		//小数点以下の切り捨て
		time = (int)time;

		//分の確保
		m_workInt = (int)(time / SECONDS_PER_MINUTE);

		//タイマー時間の代入
		Timer.text = m_workInt + TIME_SEPARATOR + (time - (m_workInt * SECONDS_PER_MINUTE)).ToString("00");

		//通知していないand残り時間が通知時間になった
		if (!m_isRemainingTimeNotification && (time <= m_remainingTimeToNotifyTime))
		{
			//時間通知
			m_isRemainingTimeNotification = true;
			RemainingTime.StartAnimation();
		}

		//通知していないandカウントダウンの時間になった
		if(!m_isCountdownOfBattleFinish && (time <= m_countdownTimeBeforeBattleEnd))
		{
			//カウントダウン開始
			m_isCountdownOfBattleFinish = true;
			CountdownOfBattleFinish.StartAnimation();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水の残量を設定する
	/// </summary>
	/// <param name="remainingAmount">残量</param>
	public void SetRemainingAmountOfWater(float remainingAmount = 1)
	{
		RemainingAmountOfWater.fillAmount = remainingAmount;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マップ上にプレイヤー名を書く
	/// </summary>
	void WritePlayerNamesOnTheMap()
	{
		//全プレイヤーをテキストに代入
		for (var i = 0; i < PlayerNamesOnTheMap.Length; i++)
		{
			//ネットプレイヤー設定がない
			if (i >= MyNetPlayerSetting.NetPlayerSettings.Count)
				return;

			PlayerNamesOnTheMap[i].text = MyNetPlayerSetting.NetPlayerSettings[i].PlayerName;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 準備完了アニメーションを開始
	/// </summary>
	public void StartReadyAnimation()
	{
		ReadyMessage.StartAnimation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 開始アニメーションを開始
	/// </summary>
	public void StartGoAnimation()
	{
		ReadyMessage.StopAnimation();
		GoMessage.StartAnimation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 順位を設定
	/// </summary>
	/// <param name="heightRanks">高さ順位たち</param>
	/// <param name="numOfOperatingPlayer">操作プレイヤーの番号</param>
	public void SetRank(int[] heightRanks, int numOfOperatingPlayer)
	{
		//順位が上げているところ
		if (m_isRankingOfOperationPlayerHasRisen)
			return;

		//操作プレイヤーの順位
		m_operatingPlayerRank = heightRanks[numOfOperatingPlayer];

		//操作プレイヤーの順位が上がった
		if (m_operatingPlayerRank < m_operatingPlayerRankPrev)
		{
			m_isRankingOfOperationPlayerHasRisen = true;
			m_countRankChangeTime = 0;
			return;
		}

		//入れ替え
		RearrangeRankOnMap(heightRanks);

		//操作プレイヤーの順位
		m_operatingPlayerRankPrev = heightRanks[numOfOperatingPlayer];
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マップ上の順位を入れ替える
	/// </summary>
	/// <param name="heightRanks">高さ順位たち</param>
	void RearrangeRankOnMap(int[] heightRanks)
	{
		//全プレイヤーにアクセス
		for (m_targetNum = 0; m_targetNum < heightRanks.Length; m_targetNum++)
		{
			//順位に合った親と位置
			PlayersOnMap[m_targetNum].transform.SetParent(ParentsOfRank[heightRanks[m_targetNum]]);
			PlayersOnMap[m_targetNum].transform.localPosition = Vector3.zero;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 順位を表示する
	/// </summary>
	/// <param name="isDisplay">表示するか</param>
	public void DisplayRank(bool isDisplay = true)
	{
		Rank.SetActive(isDisplay);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水を被る
	/// </summary>
	public void WearWater()
	{
		//全ての被水
		foreach(var wearWater in WearWaters)
		{
			//アニメーションの開始
			wearWater.StartAnimation(false);

			//α値の設定
			m_workColor = wearWater.TargetImage.color;
			m_workColor.a = m_alphaValueWhenWearingWater;
			wearWater.TargetImage.color = m_workColor;
		}

		m_countTimeToWearWater = 0;
		m_isWearWater = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 落下の開始
	/// </summary>
	public void StartOfFall()
	{
		Falling.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 落下の停止
	/// </summary>
	public void StopOfFall()
	{
		Falling.SetActive(false);
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
