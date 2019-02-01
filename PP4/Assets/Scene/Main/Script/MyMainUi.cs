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

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 公開する戦績
/// </summary>
public struct PublishRecord
{
	/// <summary>
	/// 順位
	/// </summary>
	public int rank;

	/// <summary>
	/// レベル
	/// </summary>
	public int level;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	public string playerName;

	/// <summary>
	/// 高度
	/// </summary>
	public int height;

	/// <summary>
	/// コインの数
	/// </summary>
	public int numOfCoins;

	/// <summary>
	/// スコア
	/// </summary>
	public int score;
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
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
	/// 数字
	/// </summary>
	[SerializeField]
	Sprite[] Number;

	/// <summary>
	/// 人を待つ時間たち
	/// </summary>
	[SerializeField]
	Image[] TimeToWaitForPeople;

	/// <summary>
	/// 自分情報のランク
	/// </summary>
	[SerializeField]
	Text LevelOfYourOwnInfo;

	/// <summary>
	/// 自分情報の経験値
	/// </summary>
	[SerializeField]
	Text ExpOfYourOwnInfo;

	/// <summary>
	/// 自分情報のパワー
	/// </summary>
	[SerializeField]
	Text PowerOfYourOwnInfo;

	/// <summary>
	/// 人を待つメッセージ
	/// </summary>
	[SerializeField]
	GameObject MessageToRecruitPeople;

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
	/// 加速の残量
	/// </summary>
	[SerializeField]
	Image RemainingAmountOfAcceleration;

	/// <summary>
	/// 加速できない印
	/// </summary>
	[SerializeField]
	Image MarkThatCanNotAccelerated;

	/// <summary>
	/// マップ上の順位
	/// </summary>
	[SerializeField]
	GameObject RankOnMap;

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
	/// ランキングのSPゲージ達
	/// </summary>
	[SerializeField]
	Image[] RankingSpGauges;

	/// <summary>
	/// マップ上のプレイヤーの前景たち
	/// </summary>
	[SerializeField]
	Image[] ForegroundOfPlayerOnMap;

	/// <summary>
	/// コイン枚数
	/// </summary>
	[SerializeField]
	Text NumOfCoins;

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
	/// 落下理由たち
	/// </summary>
	[SerializeField]
	Sprite[] ReasonForFallings;

	/// <summary>
	/// 落下理由の画像
	/// </summary>
	[SerializeField]
	Image ReasonForFallingImage;

	/// <summary>
	/// 被雷
	/// </summary>
	[SerializeField]
	Image Thunder;

	/// <summary>
	/// 回復率
	/// </summary>
	[SerializeField]
	Image RecoveryRate;

	/// <summary>
	///	ヒット情報
	/// </summary>
	[SerializeField]
	GameObject HitInfo;

	/// <summary>
	/// 当たったものの名
	/// </summary>
	[SerializeField]
	Text HitName;

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
	/// 勝敗
	/// </summary>
	[SerializeField]
	GameObject WinningOrLosing;

	/// <summary>
	/// 結果の順位
	/// </summary>
	[SerializeField]
	Image[] ResultRanks;
	
	/// <summary>
	/// 自分自身のパワー
	/// </summary>
	[SerializeField]
	Text YourOnwPower;

	/// <summary>
	/// 結果画面
	/// </summary>
	[SerializeField]
	GameObject ResultScreen;
	
	/// <summary>
	/// 戦績
	/// </summary>
	[SerializeField]
	GameObject BattleRecords;

	/// <summary>
	/// プレイヤーの戦績
	/// </summary>
	[SerializeField]
	GameObject[] PlayerRecords;

	/// <summary>
	/// 戦績のレベル達
	/// </summary>
	[SerializeField]
	Text[] LevelsOfBattleRecord;

	/// <summary>
	/// 戦績のプレイヤー名たち
	/// </summary>
	[SerializeField]
	Text[] PlayerNamesOfBattleRecord;

	/// <summary>
	/// 戦績の高度たち
	/// </summary>
	[SerializeField]
	Text[] HeightsOfBattleRecord;

	/// <summary>
	/// 戦績のコイン枚数たち
	/// </summary>
	[SerializeField]
	Text[] NumOfCoinsOfBattleRecord;

	/// <summary>
	/// 戦績のスコア達
	/// </summary>
	[SerializeField]
	Text[] ScoresOfBattleRecord;

	/// <summary>
	/// 自分の戦績の矢印
	/// </summary>
	[SerializeField]
	Image[] ArrowOfYourOwnAchievement;

	/// <summary>
	/// 経験値
	/// </summary>
	[SerializeField]
	GameObject Exp;

	/// <summary>
	/// 経験値の割合
	/// </summary>
	[SerializeField]
	Image PercentageOfExp;

	/// <summary>
	/// 経験値数
	/// </summary>
	[SerializeField]
	Text ExpNum;

	/// <summary>
	/// 再戦
	/// </summary>
	[SerializeField]
	GameObject Rematch;

	/// <summary>
	/// バトルを続ける
	/// </summary>
	[SerializeField]
	Button ContinueBattle;

	/// <summary>
	/// 選ばれたバトル継続
	/// </summary>
	[SerializeField]
	Sprite SelectedContinueBattle;

	/// <summary>
	/// 選ばれなかったバトル継続
	/// </summary>
	[SerializeField]
	Sprite NonSelectedContinueBattle;

	/// <summary>
	/// バトルをやめる
	/// </summary>
	[SerializeField]
	Button LeaveBattle;

	/// <summary>
	/// 選ばれたバトル中止
	/// </summary>
	[SerializeField]
	Sprite SelectedLeaveBattle;

	/// <summary>
	/// 選ばれなかったバトル中止
	/// </summary>
	[SerializeField]
	Sprite NonSelectedLeaveBattle;

	/// <summary>
	/// バトル中止のカウント
	/// </summary>
	[SerializeField]
	Text LeaveBattleCount;
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
	/// 雷の点滅開始時間
	/// </summary>
	[SerializeField]
	float[] m_blinkingStartTimeOfLightning;

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
	/// 雷の点滅時間を数える
	/// </summary>
	float m_countBlinkingTimeOfLightning = -1;

	/// <summary>
	/// 残り時間の通知フラグ
	/// </summary>
	bool m_isRemainingTimeNotification;

	/// <summary>
	/// バトル終了前のカウントダウンフラグ
	/// </summary>
	bool m_isCountdownOfBattleFinish;
	#endregion

	#region リザルト画面
	[Header("リザルト画面")]
	/// <summary>
	/// 勝敗の移動時間
	/// </summary>
	[SerializeField]
	float m_travelTimeOfWinOrLose;

	/// <summary>
	/// 勝敗の初期位置
	/// </summary>
	[SerializeField]
	Vector3 m_initPosOfWinOrLose;

	/// <summary>
	/// 勝敗の移動量
	/// </summary>
	[SerializeField]
	Vector3 m_movementAmountOfWinOrLose;

	/// <summary>
	/// 経験値の区切り
	/// </summary>
	[SerializeField]
	string m_expBreak;

	/// <summary>
	/// 経験値加算時間
	/// </summary>
	[SerializeField]
	float m_expAddedTime;

	/// <summary>
	/// 経験値音の時間
	/// </summary>
	[SerializeField]
	float m_expSoundTime;

	/// <summary>
	/// 勝敗が移動しているフラグ
	/// </summary>
	bool m_isWinningOrLosingMoves;

	/// <summary>
	/// 勝敗の移動時間を数える
	/// </summary>
	float m_countTravelTimeOfWinOrLose;

	/// <summary>
	/// 経験値加算時間を数える
	/// </summary>
	float m_countExpAddedTime = -1;

	/// <summary>
	/// 経験値音の時間を数える
	/// </summary>
	float m_countExpSoundTime;

	/// <summary>
	/// 元の経験値
	/// </summary>
	int m_originalExp;

	/// <summary>
	/// 増える経験値
	/// </summary>
	int m_increasingExp;

	/// <summary>
	/// 目標経験値たち
	/// </summary>
	int[] m_targetExp;

	/// <summary>
	/// 前の目標経験値
	/// </summary>
	int m_targetExpPrev;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用のInt
	/// </summary>
	int m_workInt;

	/// <summary>
	/// 作業用のFloat
	/// </summary>
	float m_workFloat;

	/// <summary>
	/// 作業用のColor
	/// </summary>
	Color m_workColor;
	#endregion

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

		//被雷する
		if (m_countBlinkingTimeOfLightning != -1)
			ThunderProcess();

		//勝敗の移動
		if (m_isWinningOrLosingMoves)
			MovementOfWinOrLosingProcess();

		//経験値の加算
		if (m_countExpAddedTime != -1)
			AddExp();
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
		for (var i = m_operatingPlayerRank; i < m_operatingPlayerRankPrev; i++)
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
		if (m_countRankChangeTime >= m_rankChangeTime)
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
		if (m_countTimeToWearWater >= m_timeToWearWater)
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
	/// 被雷処理
	/// </summary>
	void ThunderProcess()
	{
		m_countBlinkingTimeOfLightning += Time.deltaTime;

		//点滅開始時間を逆順
		for(m_workInt = m_blinkingStartTimeOfLightning.Length - 1; m_workInt >= 0; m_workInt--)
		{
			//時間に対応した配列番号の検索
			if(m_countBlinkingTimeOfLightning >= m_blinkingStartTimeOfLightning[m_workInt])
			{
				//配列番号が偶数だと表示
				Thunder.enabled = (m_workInt % 2 == 0);

				//終了
				if(m_workInt == m_blinkingStartTimeOfLightning.Length - 1)
				{
					Thunder.enabled = false;
					m_countBlinkingTimeOfLightning = -1;
				}

				return;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 勝敗の移動処理
	/// </summary>
	void MovementOfWinOrLosingProcess()
	{
		m_countTravelTimeOfWinOrLose += Time.deltaTime;

		//時間によって移動する
		WinningOrLosing.transform.localPosition =
			m_initPosOfWinOrLose + (m_movementAmountOfWinOrLose * (m_countTravelTimeOfWinOrLose / m_travelTimeOfWinOrLose));

		//終了
		if(m_countTravelTimeOfWinOrLose >= m_travelTimeOfWinOrLose)
		{
			WinningOrLosing.transform.localPosition = m_initPosOfWinOrLose + m_movementAmountOfWinOrLose;
			m_isWinningOrLosingMoves = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 経験値の加算
	/// </summary>
	void AddExp()
	{
		m_countExpAddedTime += Time.deltaTime;
		m_countExpSoundTime += Time.deltaTime;

		//経験値
		m_workFloat = m_originalExp + (m_increasingExp * m_countExpAddedTime / m_expAddedTime);

		//目標経験値と増加中経験値の探索
		foreach (var targetExp in m_targetExp)
		{
			if(m_workFloat <= targetExp)
			{
				//経験値の反映
				PercentageOfExp.fillAmount = m_workFloat / targetExp;
				ExpNum.text = (int)m_workFloat + m_expBreak + targetExp;

				//レベルアップ
				if(m_targetExpPrev != targetExp)
				{
					m_targetExpPrev = targetExp;
					MySoundManager.Instance.Play(SeCollection.UpLv);
				}

				break;
			}
		}

		//経験値音
		if(m_countExpSoundTime >= m_expSoundTime)
		{
			m_countExpSoundTime = 0;
			MySoundManager.Instance.Play(SeCollection.UpExp, true, false, Camera.main.transform.position);
		}

		//終了
		if (m_countExpAddedTime >= m_expAddedTime)
		{
			m_countExpAddedTime = -1;

			//全経験値
			m_workInt = m_originalExp + m_increasingExp;

			//経験値の反映
			PercentageOfExp.fillAmount = (float)m_workInt / m_targetExp[m_targetExp.Length - 1];
			ExpNum.text = m_workInt + m_expBreak + m_targetExp[m_targetExp.Length - 1];
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人材募集をする
	/// </summary>
	/// <param name="rank">ランク</param>
	/// <param name="exp">経験値</param>
	/// <param name="power">パワー</param>
	public void WantedRecruitment(int rank, int exp, int power)
	{
		//全体の表示
		RecruitPeopleScreen.SetActive(true);
		MessageToRecruitPeople.SetActive(true);

		//不要オブジェクトの非表示
		MessageToStartGame.SetActive(false);
		BattleScreen.SetActive(false);
		ReadyMessage.StopAnimation();
		GoMessage.StopAnimation();
		Falling.SetActive(false);
		BattleEnd.SetActive(false);
		ResultScreen.SetActive(false);
		foreach(var r in ResultRanks)
		{
			r.enabled = false;
		}

		//設定
		LevelOfYourOwnInfo.text = rank.ToString();
		ExpOfYourOwnInfo.text = exp.ToString();
		PowerOfYourOwnInfo.text = power.ToString();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を待つ時間の設定
	/// </summary>
	/// <param name="time">時間</param>
	public void SetTimeToWaitWorPeople(float time)
	{
		//桁ごとに時間の代入
		for(m_workInt = 0; m_workInt < TimeToWaitForPeople.Length; m_workInt++)
		{
			TimeToWaitForPeople[m_workInt].sprite = (time >= 0 ? Number[(int)time % Number.Length] : Number[0]);
			time /= Number.Length;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった
	/// </summary>
	public void PeopleGathered()
	{
		//不要オブジェクトの非表示
		MessageToRecruitPeople.SetActive(false);

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
	public void StartFadeOut()
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
		SetRemainingAmountOfAcceleration();
		SetMarkThatCanNotAccelerated();
		WritePlayerNamesOnTheMap();
		ShowRankOnMap(true, false);
		SetNumOfCoins();
		ShowHitInfoOfAuraBall(null);
		m_operatingPlayerRankPrev = m_operatingPlayerRank;
		m_isRankingOfOperationPlayerHasRisen = false;
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
		if (!m_isCountdownOfBattleFinish && (time <= m_countdownTimeBeforeBattleEnd))
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
	/// 加速の残量を設定する
	/// </summary>
	/// <param name="remainingAmount">残量</param>
	public void SetRemainingAmountOfAcceleration(float remainingAmount = 1)
	{
		RemainingAmountOfAcceleration.fillAmount = remainingAmount;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 加速できない印を設定
	/// </summary>
	/// <param name="isCanAccelerate">加速できる</param>
	public void SetMarkThatCanNotAccelerated(bool isCanAccelerate = true)
	{
		MarkThatCanNotAccelerated.enabled = !isCanAccelerate;
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
			//順位の整理
			PlayersOnMap[i].transform.SetParent(ParentsOfRank[i]);
			PlayersOnMap[i].transform.localPosition = Vector3.zero;

			//ネットプレイヤー設定がない
			if (i >= MyNetPlayerSetting.NetPlayerSettings.Count)
			{
				PlayersOnMap[i].SetActive(false);
				continue;
			}

			//マップのプレイヤーを表示してプレイヤー名の入力
			PlayersOnMap[i].SetActive(true);
			PlayerNamesOnTheMap[i].text = MyNetPlayerSetting.NetPlayerSettings[i].PlayerName;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マップ上のプレイヤーに印をつける
	/// </summary>
	/// <param name="playerNum">プレイヤー番号</param>
	/// <param name="isMarking">印をつける</param>
	public void MarkPlayersOnMap(int playerNum, bool isMarking = true)
	{
		ForegroundOfPlayerOnMap[playerNum].enabled = isMarking;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 準備完了アニメーションを開始
	/// </summary>
	public void StartReadyAnimation()
	{
		ReadyMessage.StartAnimation();

		//SE
		MySoundManager.Instance.Play(SeCollection.Ready);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 開始アニメーションを開始
	/// </summary>
	public void StartGoAnimation()
	{
		ReadyMessage.StopAnimation();
		GoMessage.StartAnimation();

		//SE
		MySoundManager.Instance.Play(SeCollection.Go);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// SPゲージの設定
	/// </summary>
	/// <param name="playerNum">プレイヤー番号</param>
	/// <param name="gauge">ゲージ</param>
	public void SetRankingSpGauge(int playerNum, float gauge)
	{
		RankingSpGauges[playerNum].fillAmount = gauge;
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
		for (m_workInt = 0; m_workInt < heightRanks.Length; m_workInt++)
		{
			//順位に合った親と位置
			PlayersOnMap[m_workInt].transform.SetParent(ParentsOfRank[heightRanks[m_workInt]]);
			PlayersOnMap[m_workInt].transform.localPosition = Vector3.zero;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マップ上の順位を表示する
	/// </summary>
	/// <param name="isDisplay">表示するか</param>
	/// <param name="isSe">SEを鳴らすか</param>
	public void ShowRankOnMap(bool isDisplay = true, bool isSe = true)
	{
		RankOnMap.SetActive(isDisplay);

		//SE
		if (isSe)
			MySoundManager.Instance.Play(isDisplay ? SeCollection.Select : SeCollection.Cancel);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コイン枚数
	/// </summary>
	/// <param name="numOfCoins">コイン枚数</param>
	public void SetNumOfCoins(int numOfCoins = 0)
	{
		NumOfCoins.text = numOfCoins.ToString();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水を被る
	/// </summary>
	public void WearWater()
	{
		//全ての被水
		foreach (var wearWater in WearWaters)
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
	/// <param name="reason">理由</param>
	public void StartOfFall(ReasonForFalling reason)
	{
		//理由
		switch(reason)
		{
			case ReasonForFalling.WaterRunsOut:
				ReasonForFallingImage.sprite = ReasonForFallings[0];
				break;
			case ReasonForFalling.CollisionWithPlayers:
				ReasonForFallingImage.sprite = ReasonForFallings[1];
				break;
			case ReasonForFalling.Thunderbolt:
				ReasonForFallingImage.sprite = ReasonForFallings[2];
				StartThunder();
				break;
			case ReasonForFalling.Meteorite:
				ReasonForFallingImage.sprite = ReasonForFallings[3];
				break;
			case ReasonForFalling.AuraBall:
				ReasonForFallingImage.sprite = ReasonForFallings[4];
				break;
		}

		Falling.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 被雷の開始
	/// </summary>
	void StartThunder()
	{
		//被雷していない時
		if (m_countBlinkingTimeOfLightning == -1)
			m_countBlinkingTimeOfLightning = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回復率の設定
	/// </summary>
	/// <param name="rate">率</param>
	public void SetRecoveryRate(float rate)
	{
		RecoveryRate.fillAmount = rate;
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
	/// オーラボールのヒット情報を示す
	/// </summary>
	/// <param name="hitName">当たったものの名</param>
	public void ShowHitInfoOfAuraBall(string hitName)
	{
		HitInfo.SetActive(!(hitName == null || hitName == ""));

		HitName.text = hitName;
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
	/// 結果状態にする
	/// </summary>
	public void MakeItResultState()
	{
		//不要オブジェクトの非表示
		BattleScreen.SetActive(false);
		BattleEnd.SetActive(false);

		StartFadeIn();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 順位の表示
	/// </summary>
	/// <param name="rank">順位</param>
	/// <param name="yourOwnPower">自分自身のパワー</param>
	public void DisplayRanking(int rank, int yourOwnPower)
	{
		//表示
		WinningOrLosing.SetActive(true);

		//順位とパワー
		ResultRanks[rank - 1].enabled = true;
		YourOnwPower.text = yourOwnPower.ToString();

		//初期位置
		WinningOrLosing.transform.localPosition = m_initPosOfWinOrLose;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 結果の表示
	/// </summary>
	/// <param name="publishRecords">公開する戦績たち</param>
	/// <param name="myRecordNum">自分の戦績番号</param>
	public void DisplayResults(PublishRecord[] publishRecords, int myRecordNum)
	{
		ResultScreen.SetActive(true);
		BattleRecords.SetActive(true);
		for(m_workInt = 0; m_workInt < PlayerRecords.Length; m_workInt++)
		{
			PlayerRecords[m_workInt].SetActive(m_workInt < publishRecords.Length);
		}
		Exp.SetActive(false);
		Rematch.SetActive(false);

		var recordNum = 0;

		//全ての戦績
		for (var i = 0; i < publishRecords.Length; i++)
		{
			//戦績順位の取り出し
			recordNum = publishRecords[i].rank;

			//戦績の代入
			LevelsOfBattleRecord[recordNum].text = publishRecords[i].level.ToString();
			PlayerNamesOfBattleRecord[recordNum].text = publishRecords[i].playerName;
			HeightsOfBattleRecord[recordNum].text = publishRecords[i].height.ToString("N0");
			NumOfCoinsOfBattleRecord[recordNum].text = publishRecords[i].numOfCoins.ToString();
			ScoresOfBattleRecord[recordNum].text = publishRecords[i].score.ToString("N0");
		}

		//自分のレコード矢印
		ArrowOfYourOwnAchievement[publishRecords[myRecordNum].rank].enabled = true;

		m_isWinningOrLosingMoves = true;
		m_countTravelTimeOfWinOrLose = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 経験値の表示
	/// </summary>
	/// <param name="originalExp">元の経験値数</param>
	/// <param name="increasingExp">増える経験値数</param>
	/// <param name="targetExp">目標経験値たち</param>
	public void ShowExp(int originalExp, int increasingExp, int[] targetExp)
	{
		Exp.SetActive(true);

		//初期設定
		m_countExpAddedTime = 0;
		m_countExpSoundTime = 0;
		m_targetExp = targetExp;
		m_originalExp = originalExp;
		m_increasingExp = increasingExp;
		m_targetExpPrev = targetExp[0];

		//元の経験値の反映
		PercentageOfExp.fillAmount = (float)originalExp / targetExp[0];
		ExpNum.text = originalExp + m_expBreak + targetExp[0];
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再戦を表示
	/// </summary>
	public void ShowRematch()
	{
		//不要オブジェクトの非表示
		WinningOrLosing.SetActive(false);
		BattleRecords.SetActive(false);
		foreach (var record in PlayerRecords)
		{
			record.SetActive(true);
		}
		foreach (var arrow in ArrowOfYourOwnAchievement)
		{
			arrow.enabled = false;
		}
		Exp.SetActive(false);
		
		//表示オブジェクト
		Rematch.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再戦の選択
	/// </summary>
	/// <param name="isContinue">続けるか</param>
	public void SelectionOfRematch(bool isContinue)
	{
		ContinueBattle.image.sprite = isContinue ? SelectedContinueBattle : NonSelectedContinueBattle;
		LeaveBattle.image.sprite = !isContinue ? SelectedLeaveBattle : NonSelectedLeaveBattle;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを続けるをクリック
	/// </summary>
	public void OnClickContinueBattle()
	{
		Game.ContinueBattle();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを辞めるをクリック
	/// </summary>
	public void OnClickLeaveBattle()
	{
		Game.LeaveBattle();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル中止のカウントを設定
	/// </summary>
	/// <param name="time">時間</param>
	public void SetLeaveBattleCount(float time)
	{
		LeaveBattleCount.text = ((int)time).ToString();
	}
}
