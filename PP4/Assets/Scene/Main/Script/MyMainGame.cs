////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/7～
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
	/// スキンの生成
	/// </summary>
	CreateSkin,
	/// <summary>
	/// 人を募集
	/// </summary>
	RecruitPeople,
	/// <summary>
	/// 人が集まった
	/// </summary>
	PeopleGathered,
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

//----------------------------------------------------------------------------------------------------/// <summary>
/// メインゲーム
/// </summary>
public class MyMainGame : MyGame
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// ゴーストプレイヤー達
	/// </summary>
	[SerializeField]
	GameObject GhostPlayers;

	/// <summary>
	/// ネットワークマネージャ
	/// </summary>
	MyNetworkManager m_netManager;

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

	/// <summary>
	/// 状態の時間を数える
	/// </summary>
	float m_countTheTimeOfTheState;
	#endregion

	#region 人を待つ状態
	[Header("人を待つ状態")]
	/// <summary>
	/// 人を待つ時間
	/// </summary>
	[SerializeField]
	float m_timeToWaitForPeople;

	/// <summary>
	/// 人が集まり状態遷移する時間
	/// </summary>
	[SerializeField]
	float m_timeWhenPeopleGatherAndChangeState;

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
	/// プレイヤー人数
	/// </summary>
	[SerializeField]
	int m_numOfPlayers;
	public int NumOfPlayers
	{
		get { return m_numOfPlayers; }
	}

	/// <summary>
	/// 人が集まった時間
	/// </summary>
	float m_timeWhenPeopleGathered;
	#endregion

	#region 人が集まった状態
	[Header("人が集まった状態")]
	/// <summary>
	/// 人が集まった状態の時間
	/// </summary>
	[SerializeField]
	float m_timeWhenPeopleGather;

	/// <summary>
	/// プレイヤーが船の端に行く時間
	/// </summary>
	[SerializeField]
	float m_timeWhenPlayerGoesToEdgeOfShip;

	/// <summary>
	/// プレイヤーが船から飛び降りる時間
	/// </summary>
	[SerializeField]
	float m_timePlayerJumpsOffShip;

	/// <summary>
	/// 船の端の位置
	/// </summary>
	[SerializeField]
	Vector3[] m_posOfEndOfShip;

	/// <summary>
	/// 船から飛び降りる位置
	/// </summary>
	[SerializeField]
	Vector3[] m_posToJumpOffShip;

	/// <summary>
	/// プレイヤー回転スピード
	/// </summary>
	[SerializeField]
	float m_playerRotationSpeed;

	/// <summary>
	/// 人が集まった時のカメラ位置
	/// </summary>
	[SerializeField]
	Vector3[] m_cameraPosWhenPeopleGather;

	/// <summary>
	/// 人が集まった時のカメラ方向
	/// </summary>
	[SerializeField]
	Vector3[] m_cameraDirectionWhenPeopleGather;

	/// <summary>
	/// 人が集まった時のカメラ移動時間
	/// </summary>
	[SerializeField]
	float[] m_cameraMovingTimeWhenPeopleGather;
	#endregion

	#region バトル設定状態
	[Header("バトル設定状態")]
	/// <summary>
	/// バトル設定時間
	/// </summary>
	[SerializeField]
	float m_battleSettingTime;
	#endregion

	#region バトル前状態
	[Header("バトル前状態")]
	/// <summary>
	/// バトル前の時間
	/// </summary>
	[SerializeField]
	float m_timeBeforeBattle;

	/// <summary>
	/// バトル直前の時間
	/// </summary>
	[SerializeField]
	float m_timeJustBeforeBattle;

	/// <summary>
	/// バトル開始前の最初のカメラ位置
	/// </summary>
	[SerializeField]
	Vector3 m_firstCameraPosBeforeStartingBattle;

	/// <summary>
	/// バトル開始前のカメラ絶対的位置の数
	/// </summary>
	[SerializeField]
	int m_numOfAbsolutePosOfCameraBeforeStartingBattle;

	/// <summary>
	/// バトル開始前のプレイヤーへの相対的カメラ位置
	/// </summary>
	[SerializeField]
	Vector3[] m_cameraPosRelativeToPlayerBeforeStartingBattle;

	/// <summary>
	/// バトル開始前のカメラ移動時間
	/// </summary>
	[SerializeField]
	float[] m_cameraMovingTimeBeforeStaringBattle;

	/// <summary>
	/// バトル直前フラグ
	/// </summary>
	bool m_isItJustBeforeBattle;

	/// <summary>
	/// 指定するためのカメラ位置
	/// </summary>
	Vector3[] m_cameraPosForSpecifying;

	/// <summary>
	/// 指定するためのカメラ方向
	/// </summary>
	Vector3[] m_cameraDirectionForSpecifying;
	#endregion

	#region バトル状態
	[Header("バトル状態")]
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
	/// マップ上にプレイヤー名を表示する時間
	/// </summary>
	[SerializeField]
	float m_timeToDisplayPlayerNameOnMap;

	/// <summary>
	/// １ｍ毎のサポート率
	/// </summary>
	[SerializeField]
	float m_supportRatePerMeter;

	/// <summary>
	/// プレイヤーの帯電時間
	/// </summary>
	[SerializeField]
	float m_chargingTimeOfPlayer;

	/// <summary>
	/// 順位による帯電率
	/// </summary>
	[SerializeField]
	float[] m_chargeRateByRank;

	/// <summary>
	/// 隕石が落下するプレイヤー高低差
	/// </summary>
	[SerializeField]
	float m_differenceInElevationOfPlayersFallingMeteorite;

	/// <summary>
	/// 隕石発生する相対的高さ
	/// </summary>
	[SerializeField]
	float m_meteoriteGenerationRelativeHeight;

	/// <summary>
	/// オーラボールをリセットするスペシャルゲージ割合
	/// </summary>
	[SerializeField]
	float m_spRatioToResetAuraBall;

	/// <summary>
	/// 順位の表示
	/// </summary>
	bool m_isDisplayRank;

	/// <summary>
	/// 操作プレイヤーが落下フラグ
	/// </summary>
	bool m_isOperatingPlayerFall;

	/// <summary>
	/// マップ上にプレイヤー名を表示する時間が過ぎたフラグ
	/// </summary>
	bool m_isTimeToDisplayPlayerNameOnMapHasPassed;

	/// <summary>
	/// プレイヤーの帯電時間を数える
	/// </summary>
	float m_countChargingTimeOfPlayer;

	/// <summary>
	/// プレイヤーの帯電率を数える
	/// </summary>
	float[] m_countPlayerChargeRate;

	/// <summary>
	/// オーラボールのリセットフラグ
	/// </summary>
	bool m_isResetAuraBall;
	#endregion

	#region バトル後状態
	[Header("バトル後状態")]
	/// <summary>
	/// バトル後の時間
	/// </summary>
	[SerializeField]
	float m_timeAfterCombatEnd;

	/// <summary>
	/// バトルが終了して停止する時間
	/// </summary>
	[SerializeField]
	float m_timeWhenTheBattleEndsAndStops;

	/// <summary>
	/// バトル終了後のフェードアウトフラグ
	/// </summary>
	bool m_isFadeOutAfterBattleEnds;
	#endregion

	#region 結果状態
	[Header("結果状態")]
	/// <summary>
	/// 表彰台が表示される時間
	/// </summary>
	[SerializeField]
	float m_timePodiumIsDisplayed;

	/// <summary>
	/// 表彰台が表示された時間
	/// </summary>
	[SerializeField]
	float m_timePodiumWasDisplayed;

	/// <summary>
	/// 結果表示し始める時間
	/// </summary>
	[SerializeField]
	float m_timeToStartDisplayingResults;

	/// <summary>
	/// 経験値表示し始める時間
	/// </summary>
	[SerializeField]
	float m_timeToStartDisplayingExp;

	/// <summary>
	/// 再戦表示し始める時間
	/// </summary>
	[SerializeField]
	float m_timeToStartDisplayingRematch;

	/// <summary>
	/// 結果状態のプレイヤーの初期高さ
	/// </summary>
	[SerializeField]
	float m_playerInitialHeightOfResultState;
	
	/// <summary>
	/// 結果表示中のプレイヤーから見たカメラ相対的位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_relativeCameraPosSeenFromDisplayingResult;

	/// <summary>
	/// 結果表示中のカメラ移動時間たち
	/// </summary>
	[SerializeField]
	float[] m_cameraMovementTimeDisplayingResults;

	/// <summary>
	/// 高度に対するコイン倍率
	/// </summary>
	[SerializeField]
	float m_coinMagnificationAgainstHeight;

	/// <summary>
	/// 表彰される数
	/// </summary>
	[SerializeField]
	int m_numToBeAwarded;

	/// <summary>
	/// 目標経験値
	/// </summary>
	[SerializeField]
	int[] m_targetExp;

	/// <summary>
	/// 参加した経験値
	/// </summary>
	[SerializeField]
	int m_expParticipated;

	/// <summary>
	/// 経験値当たりの高さ
	/// </summary>
	[SerializeField]
	int m_heightPerExp;

	/// <summary>
	/// パワーの収集回数
	/// </summary>
	[SerializeField]
	int m_powerCollectionCount;

	/// <summary>
	/// 勝ちフラグ
	/// </summary>
	bool m_isWin;

	/// <summary>
	/// 着水フラグ
	/// </summary>
	bool m_isLanding;

	/// <summary>
	/// 表彰台の表示フラグ
	/// </summary>
	bool m_isDisplayPodium;

	/// <summary>
	/// 結果表示フラグ
	/// </summary>
	bool m_isResultDisplay;

	/// <summary>
	/// 経験値の表示フラグ
	/// </summary>
	bool m_isDisplayOfExp;

	/// <summary>
	/// 追加する経験値
	/// </summary>
	int m_additionalExp;

	/// <summary>
	/// 次の目標経験値たち
	/// </summary>
	List<int> m_nextTargetExp = new List<int>();

	/// <summary>
	/// 戦績
	/// </summary>
	PublishRecord[] m_battleRecords;

	/// <summary>
	/// 再戦表示フラグ
	/// </summary>
	bool m_isRematchDisplay;

	/// <summary>
	/// バトルを続けるフラグ
	/// </summary>
	bool m_isContinueBattle;
	#endregion

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

	/// <summary>
	/// Yボタンを押した
	/// </summary>
	bool m_isYButtonDown;

	/// <summary>
	/// DパッドXがポジティブになった
	/// </summary>
	bool m_isDpadXBecamePositive;

	/// <summary>
	/// フレーム前にDパッドXがポジティブになった
	/// </summary>
	bool m_isDpadXBecamePositivePrev;

	/// <summary>
	/// DパッドXがネガティブになった
	/// </summary>
	bool m_isDpadXBecameNegative;

	/// <summary>
	/// フレーム前にDパッドXがネガティブになった
	/// </summary>
	bool m_isDpadXBecameNegativePrev;
	
	/// <summary>
	/// DパッドのY軸がネガティブになった
	/// </summary>
	bool m_isDpadYBecameNegative;

	/// <summary>
	/// フレーム前にDパッドのY軸がネガティブになった
	/// </summary>
	bool m_isDpadYBecameNegativePrev;

	/// <summary>
	/// 水平移動量がポジティブになった
	/// </summary>
	bool m_isHorizontalBecamePositive;

	/// <summary>
	/// フレーム前に水平移動量がポジティブになった
	/// </summary>
	bool m_isHorizontalBecamePositivePrev;

	/// <summary>
	/// 水平移動量がネガティブになった
	/// </summary>
	bool m_isHorizontalBecameNegative;

	/// <summary>
	/// フレーム前に水平移動量がネガティブになった
	/// </summary>
	bool m_isHorizontalBecameNegativePrev;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用のVector3
	/// </summary>
	Vector3 m_workVector3;

	/// <summary>
	/// 作業用のInt
	/// </summary>
	int m_workInt;

	/// <summary>
	/// 作業用のFloat
	/// </summary>
	float m_workFloat;

	/// <summary>
	/// 作業用のGameObjec
	/// </summary>
	GameObject m_workGameObj;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_state = GameStatus.CreateSkin;
		m_statePrev = GameStatus.Result;

		m_netManager = FindObjectOfType<MyNetworkManager>();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		InputProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力処理
	/// </summary>
	void InputProcess()
	{
		//押したタイミングの取得
		if (Input.GetButtonDown("AButton"))
			m_isAButtonDown = true;
		if (Input.GetButtonDown("BButton"))
			m_isBButtonDown = true;
		if (Input.GetButtonDown("YButton"))
			m_isYButtonDown = true;
		if (Input.GetAxis("DpadX") > 0 && !m_isDpadXBecamePositivePrev)
			m_isDpadXBecamePositive = true;
		m_isDpadXBecamePositivePrev = (Input.GetAxis("DpadX") > 0);
		if (Input.GetAxis("DpadX") < 0 && !m_isDpadXBecameNegativePrev)
			m_isDpadXBecameNegative = true;
		m_isDpadXBecameNegativePrev = (Input.GetAxis("DpadX") < 0);
		if (Input.GetAxis("DpadY") < 0 && !m_isDpadYBecameNegativePrev)
			m_isDpadYBecameNegative = true;
		m_isDpadYBecameNegativePrev = (Input.GetAxis("DpadY") < 0);
		if (Input.GetAxis("Horizontal") > 0 && !m_isHorizontalBecamePositivePrev)
			m_isHorizontalBecamePositive = true;
		m_isHorizontalBecamePositivePrev = (Input.GetAxis("Horizontal") > 0);
		if (Input.GetAxis("Horizontal") < 0 && !m_isHorizontalBecameNegativePrev)
			m_isHorizontalBecameNegative = true;
		m_isHorizontalBecameNegativePrev = (Input.GetAxis("Horizontal") < 0);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//サーバと接続が切れている
		if (m_netManager != null && m_netManager.IsConnectionWithServerIsBroken)
			return;

		//全体の状態初期設定
		if (m_state != m_statePrev)
			m_countTheTimeOfTheState = 0;

		//状態
		switch (m_state)
		{
			case GameStatus.CreateSkin:
				CreateSkinStateProcess();
				break;
			case GameStatus.RecruitPeople:
				RecruitPeopleStateProcess();
				break;
			case GameStatus.PeopleGathered:
				PeopleGatheredStateProcess();
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
	/// スキンの生成状態の処理
	/// </summary>
	void CreateSkinStateProcess()
	{
		//操作しているプレイヤーが登録されていない
		if (!OperatingPlayer)
			return;

		//必要なインスタンス
		if (!OperatingNetPlayerSetting)
			OperatingNetPlayerSetting = OperatingPlayer.GetComponent<MyNetPlayerSetting>();

		//スキンの生成が終了
		if (OperatingNetPlayerSetting.SelectSkin != null)
			m_state = GameStatus.RecruitPeople;
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

			//船の出現
			Stage.CurrentFieldScript.DisplayShip();

			//プレイヤーとカメラとUI
			OperatingNetPlayerSetting.BattleInitSetting();
			MovePlayerToPosToWaitForPeople(OperatingNetPlayerSetting.GetNetPlayerNum());
			Players.ResetPlayerColor();
			GhostPlayers.SetActive(true);
			OperatingCamera.BecomeFixedCamera(m_cameraPosWhenWaitingForPeople, m_cameraDirectionWhenWaitingForPeople);
			MainUi.WantedRecruitment(MyGameInfo.Instance.Level, MyGameInfo.Instance.Exp, MyGameInfo.Instance.Power);

			//レベルとパワーの同期
			OperatingNetPlayerSetting.CmdLevel(MyGameInfo.Instance.Level);
			OperatingNetPlayerSetting.CmdPower(MyGameInfo.Instance.Power);

			//変数
			m_timeWhenPeopleGathered = -1;

			//BGM
			MySoundManager.Instance.Play(BgmCollection.Matching);
		}

		//ゲームが開始できるか
		if (OperatingNetPlayerSetting.IsBattleStart())
		{
			//次の状態への準備
			GetReadyForPeopleGathered();
		}
		else if (m_countTheTimeOfTheState > m_timeToWaitForPeople)
		{
			//プレイヤーの解放（ネット接続を切る）
			m_netManager.StopConnection();
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
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Stand);
		OperatingPlayer.StandAtSpecifiedPos(m_playerPosWhenWaitingForPeople[playerNum], m_playerDirectionWhenWaitingForPeople[playerNum]);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった状態の準備
	/// </summary>
	void GetReadyForPeopleGathered()
	{
		//人が集まった時間
		if (m_timeWhenPeopleGathered == -1)
			m_timeWhenPeopleGathered = m_countTheTimeOfTheState;

		//状態遷移する時間
		if (m_countTheTimeOfTheState >= m_timeWhenPeopleGathered + m_timeWhenPeopleGatherAndChangeState)
			m_state = GameStatus.PeopleGathered;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった状態の処理
	/// </summary>
	void PeopleGatheredStateProcess()
	{
		//状態初期設定
		if (m_state != m_statePrev)
		{
			m_statePrev = m_state;

			//カメラとUI
			GhostPlayers.SetActive(false);
			OperatingCamera.BecomeFollowSpecifiedPosCamera(
				m_cameraPosWhenPeopleGather, m_cameraDirectionWhenPeopleGather, m_cameraMovingTimeWhenPeopleGather);
			MainUi.PeopleGathered();
		}

		//人が集まった状態のプレイヤー
		PlayersWithPeopleGathered();

		//設定時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeWhenPeopleGather)
		{
			//状態遷移
			m_state = GameStatus.BattleSetting;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった状態のプレイヤー
	/// </summary>
	void PlayersWithPeopleGathered()
	{
		//走るアニメーション
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Run);

		//プレイヤーネット番号
		m_workInt = OperatingNetPlayerSetting.GetNetPlayerNum();

		//プレイヤーが船の端に行く時間
		if (m_countTheTimeOfTheState <= m_timeWhenPlayerGoesToEdgeOfShip)
		{
			//プレイヤーの初期位置
			m_workVector3 = m_playerPosWhenWaitingForPeople[m_workInt];

			//時間によるプレイヤーの位置
			OperatingPlayer.transform.position = m_workVector3 +
				((m_posOfEndOfShip[m_workInt] - m_workVector3) * (m_countTheTimeOfTheState / m_timeWhenPlayerGoesToEdgeOfShip));

			//向きたい角度
			m_workFloat = Vector3.Cross(OperatingPlayer.transform.forward, m_posOfEndOfShip[m_workInt] - m_workVector3).y;
			m_workFloat = Vector3.Angle(OperatingPlayer.transform.forward,
				Vector3.Scale((m_posOfEndOfShip[m_workInt] - m_workVector3), Vector3.right + Vector3.forward)) * (m_workFloat < 0 ? -1 : 1);

			//向ける角度が向きたい角度より大きい
			if (m_playerRotationSpeed * Time.deltaTime >= Mathf.Abs(m_workFloat))
				OperatingPlayer.transform.Rotate(Vector3.up, m_workFloat);
			else if (m_workFloat > 0)
				OperatingPlayer.transform.Rotate(Vector3.up, m_playerRotationSpeed * Time.deltaTime);
			else
				OperatingPlayer.transform.Rotate(Vector3.up, -m_playerRotationSpeed * Time.deltaTime);

			return;
		}

		//プレイヤーが飛び降りる時間
		if (m_countTheTimeOfTheState <= m_timeWhenPlayerGoesToEdgeOfShip + m_timePlayerJumpsOffShip)
		{
			//プレイヤーの初期位置
			m_workVector3 = m_posOfEndOfShip[m_workInt];

			//時間によるプレイヤーの位置
			OperatingPlayer.transform.position = m_workVector3 + ((m_posToJumpOffShip[m_workInt] - m_workVector3)
				* ((m_countTheTimeOfTheState - m_timeWhenPlayerGoesToEdgeOfShip) / m_timePlayerJumpsOffShip));

			//向きたい角度
			m_workFloat = Vector3.Cross(OperatingPlayer.transform.forward, m_posToJumpOffShip[m_workInt] - m_workVector3).y;
			m_workFloat = Vector3.Angle(OperatingPlayer.transform.forward,
				Vector3.Scale((m_posToJumpOffShip[m_workInt] - m_workVector3), Vector3.right + Vector3.forward)) * (m_workFloat < 0 ? -1 : 1);

			//向ける角度が向きたい角度より大きい
			if (m_playerRotationSpeed * Time.deltaTime >= Mathf.Abs(m_workFloat))
				OperatingPlayer.transform.Rotate(Vector3.up, m_workFloat);
			else if (m_workFloat > 0)
				OperatingPlayer.transform.Rotate(Vector3.up, m_playerRotationSpeed * Time.deltaTime);
			else
				OperatingPlayer.transform.Rotate(Vector3.up, -m_playerRotationSpeed * Time.deltaTime);

			return;
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

			//プレイヤーとプレイヤーズとUI
			PlayerBattleSettings();
			Players.DisplaySplashes();
			MainUi.BattleStartSetting();

			//プレイヤー情報を戦績に代入
			if (m_battleRecords == null)
				m_battleRecords = new PublishRecord[MyNetPlayerSetting.NetPlayerSettings.Count];
			MyNetPlayerSetting.PutPlayerInfoInBattleRecord(ref m_battleRecords);
		}

		//設定時間が過ぎた
		if (m_countTheTimeOfTheState >= m_battleSettingTime)
		{
			//状態遷移
			m_state = GameStatus.BattleStart;

			//ステージの初期化
			Stage.CurrentFieldScript.ResetField();
			Stage.CurrentFieldScript.SetStormPos(OperatingPlayer.transform.position);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーのバトル設定
	/// </summary>
	void PlayerBattleSettings()
	{
		//プレイヤー番号を決める
		MyNetPlayerSetting.NetPlayerSettings.CopyTo(Players.DecidePlayerNum(MyNetPlayerSetting.NetPlayerSettings.ToArray()));

		//プレイヤー番号よる位置
		OperatingPlayer.transform.position = Stage.CurrentFieldScript.StartPositions[OperatingNetPlayerSetting.GetNetPlayerNum()];

		//中心を見る
		OperatingPlayer.transform.LookAt(Vector3.zero);

		//アニメーション
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Select);

		//プレイヤーの帯電率のリセット
		if (m_countPlayerChargeRate == null)
		{
			m_countPlayerChargeRate = new float[MyNetPlayerSetting.NetPlayerSettings.Count];
		}
		else
		{
			for(var i = 0; i < m_countPlayerChargeRate.Length; i++)
			{
				m_countPlayerChargeRate[i] = 0;
			}
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

			//フラグとカメラとUIの設定
			m_isItJustBeforeBattle = false;
			m_isDisplayRank = true;
			SettingOfCameraInBattleStartState();
			MainUi.BattleStart(m_battleTime);

			//BGM
			MySoundManager.Instance.StopBGM();
		}

		//バトル直前のタイミング
		if (!m_isItJustBeforeBattle && m_countTheTimeOfTheState >= m_timeBeforeBattle - m_timeJustBeforeBattle)
		{
			//バトル直前フラグ（一度だけTrue）
			m_isItJustBeforeBattle = true;

			//プレイヤーとReadyメッセージ設定
			OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Idle);
			MainUi.StartReadyAnimation();
		}

		//カウントダウン時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeBeforeBattle)
		{
			//状態遷移
			m_state = GameStatus.Battle;

			//Goメッセージ
			MainUi.StartGoAnimation();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル開始状態のカメラ設定
	/// </summary>
	void SettingOfCameraInBattleStartState()
	{
		//カメラ位置とカメラ方向の要素数
		m_cameraPosForSpecifying =
			new Vector3[m_cameraPosRelativeToPlayerBeforeStartingBattle.Length + m_numOfAbsolutePosOfCameraBeforeStartingBattle];
		m_cameraDirectionForSpecifying =
			new Vector3[m_cameraPosRelativeToPlayerBeforeStartingBattle.Length + m_numOfAbsolutePosOfCameraBeforeStartingBattle];

		int index;

		//カメラを絶対的位置で登録
		for (index = 0; index < m_numOfAbsolutePosOfCameraBeforeStartingBattle; index++)
		{
			//カメラ初期位置とカメラ初期方向の設定
			m_cameraPosForSpecifying[index] = m_firstCameraPosBeforeStartingBattle;
			m_cameraDirectionForSpecifying[index] = -m_firstCameraPosBeforeStartingBattle;
		}

		//操作プレイヤーに対応するカメラ位置とカメラ方向の設定
		for (index = m_numOfAbsolutePosOfCameraBeforeStartingBattle; index < m_cameraPosForSpecifying.Length; index++)
		{
			//プレイヤーの背後の位置からカメラ位置を生成
			m_cameraPosForSpecifying[index] =
				(OperatingPlayer.transform.position + (Vector3.up * OperatingCamera.HeightToWatch)
				- (OperatingPlayer.transform.forward * OperatingCamera.DistanceToPlayer))
				+ m_cameraPosRelativeToPlayerBeforeStartingBattle[index - m_numOfAbsolutePosOfCameraBeforeStartingBattle];

			//プレイヤーを見る方向を生成
			m_cameraDirectionForSpecifying[index] =
				OperatingPlayer.transform.position + (Vector3.up * OperatingCamera.HeightToWatch) - m_cameraPosForSpecifying[index];
		}

		//指定位置をだどるカメラにする
		OperatingCamera.BecomeFollowSpecifiedPosCamera(
			m_cameraPosForSpecifying, m_cameraDirectionForSpecifying, m_cameraMovingTimeBeforeStaringBattle);
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

			//プレイヤーとカメラとフラグ
			OperatingPlayer.MakeItBattleState();
			OperatingNetPlayerSetting.NameplateDisplay(false);
			OperatingCamera.BecomeOperablePursuitCamera();
			m_isTimeToDisplayPlayerNameOnMapHasPassed = false;

			//BGM
			MySoundManager.Instance.Play(BgmCollection.Battle);
		}

		//ステージとプレイヤーとカメラとUI
		BattleStateStage();
		BattleStatePlayer();
		BattleStateCamera();
		BattleStateUi();

		//時間が過ぎていないフラグandマップ上にプレイヤー名を表示する時間が過ぎた
		if (!m_isTimeToDisplayPlayerNameOnMapHasPassed && m_countTheTimeOfTheState >= m_timeToDisplayPlayerNameOnMap)
		{
			m_isTimeToDisplayPlayerNameOnMapHasPassed = true;

			MainUi.HidePlayerOnMap(OperatingNetPlayerSetting.GetNetPlayerNum());
		}

		//バトル時間が過ぎた
		if (m_countTheTimeOfTheState >= m_battleTime)
		{
			//状態遷移
			m_state = GameStatus.BattleEnd;
			MainUi.SetTimer();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のステージ
	/// </summary>
	void BattleStateStage()
	{
		//嵐位置
		Stage.CurrentFieldScript.SetStormPos(OperatingPlayer.transform.position);

		//雷
		m_countChargingTimeOfPlayer += Time.deltaTime;

		//プレイヤーに帯電するタイミング
		if (m_countChargingTimeOfPlayer >= m_chargingTimeOfPlayer)
		{
			m_countChargingTimeOfPlayer -= m_chargingTimeOfPlayer;

			//帯電するプレイヤー
			m_workInt = Random.Range(0, MyNetPlayerSetting.NetPlayerSettings.Count);
			m_countPlayerChargeRate[m_workInt] += m_chargeRateByRank[Players.HeightRanks[m_workInt]];

			//帯電率が上限を超えた
			if (m_countPlayerChargeRate[m_workInt] >= 1f)
			{
				//落雷
				m_countPlayerChargeRate[m_workInt] -= 1f;

				//プレイヤーが存在
				if (MyNetPlayerSetting.NetPlayerSettings[m_workInt] != null)
					Stage.CurrentFieldScript.StartThunderbolt(
						MyNetPlayerSetting.NetPlayerSettings[m_workInt].transform.position, OperatingPlayer.transform.position.y);
			}
		}

		//隕石の発生
		if (Players.MaximumAltitude - Players.MinimumAltitude >= m_differenceInElevationOfPlayersFallingMeteorite)
			Stage.CurrentFieldScript.StartOfFallingMeteorite(Players.MaximumAltitude + m_meteoriteGenerationRelativeHeight);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のプレイヤー
	/// </summary>
	void BattleStatePlayer()
	{
		//プレイヤー順位の更新
		Players.UpdateHeightRank();

		//プレイヤーサポート(サポート率０回避付き)
		OperatingPlayer.SupportRate = 1 +
			(Players.MaximumAltitude - OperatingPlayer.transform.position.y) * (m_supportRatePerMeter - 1);

		//オーラボールのリセット
		if (!m_isResetAuraBall && OperatingPlayer.GetPercentageOfRemainingSpGauge() > m_spRatioToResetAuraBall)
		{
			m_isResetAuraBall = true;
			OperatingNetPlayerSetting.ResetAuraBall();
		}

		//SPゲージが満タンでない
		if (OperatingPlayer.GetPercentageOfRemainingSpGauge() < 1f)
			return;

		//操作プレイヤーの上位の順位
		m_workInt = Players.HeightRanks[OperatingNetPlayerSetting.GetNetPlayerNum()] - 1;

		if (m_workInt < 0)
			return;

		//一つ上の上位プレイヤー
		m_workGameObj = Players.GetPlayer(m_workInt);

		//3種のオーラ
		if (m_isDpadYBecameNegative)
			OperatingNetPlayerSetting.ThrowAuraBall(m_workGameObj, AuraAttribute.Heat);
		if (m_isDpadXBecameNegative)
			OperatingNetPlayerSetting.ThrowAuraBall(m_workGameObj, AuraAttribute.Elasticity);
		if (m_isDpadXBecamePositive)
			OperatingNetPlayerSetting.ThrowAuraBall(m_workGameObj, AuraAttribute.Electrical);

		if (m_isDpadYBecameNegative || m_isDpadXBecameNegative || m_isDpadXBecamePositive)
			m_isResetAuraBall = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のカメラ
	/// </summary>
	void BattleStateCamera()
	{
		//落下状態が変わった
		if (m_isOperatingPlayerFall != OperatingPlayer.IsFalling)
		{
			//落下
			if (OperatingPlayer.IsFalling)
				OperatingCamera.BecomeCustomOperablePursuitCamera(true);
			else
				OperatingCamera.BecomeOperablePursuitCamera(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のUI
	/// </summary>
	void BattleStateUi()
	{
		//タイマー反映
		MainUi.SetTimer(m_battleTime - m_countTheTimeOfTheState);

		//タンクの残量
		MainUi.SetRemainingAmountOfWater(OperatingPlayer.GetPercentageOfRemainingWater());
		MainUi.SetRemainingAmountOfAcceleration(OperatingPlayer.GetPercentageOfRemainingSpGauge());
		MainUi.SetMarkThatCanBeAccelerated(OperatingPlayer.IsUseSp);

		//順位の反映
		MainUi.SetRank(Players.HeightRanks, OperatingNetPlayerSetting.GetNetPlayerNum());

		//Yボタンを押した
		if (m_isYButtonDown)
		{
			m_isDisplayRank = !m_isDisplayRank;

			//順位の表示
			MainUi.ShowRankOnMap(m_isDisplayRank);
		}

		//コイン枚数の反映
		MainUi.SetNumOfCoins(OperatingPlayer.NumOfCoins);

		//被水している
		if (OperatingPlayer.GetIsWearWater())
			MainUi.WearWater();

		//落下状態が変わった
		if (m_isOperatingPlayerFall != OperatingPlayer.IsFalling)
		{
			m_isOperatingPlayerFall = OperatingPlayer.IsFalling;

			//落下フラグ
			if (m_isOperatingPlayerFall)
				MainUi.StartOfFall(OperatingPlayer.ReasonForFallingEnum);
			else
				MainUi.StopOfFall();
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

			//ステージとプレイヤーとUIとフラグ
			Stage.CurrentFieldScript.PauseMeteorit();
			OperatingPlayer.MakeItBattleEndState();
			OperatingNetPlayerSetting.MakeItBattleEndState();
			MainUi.EndBattle();
			m_isFadeOutAfterBattleEnds = false;

			//BGM
			MySoundManager.Instance.StopBGM();
		}

		//フェードアウトしていないandフェードアウト時間
		if (!m_isFadeOutAfterBattleEnds && m_countTheTimeOfTheState >= m_timeWhenTheBattleEndsAndStops)
		{
			//フェードアウト
			m_isFadeOutAfterBattleEnds = true;
			MainUi.StartFadeOut();

			//バトル結果を記録
			RecordBattleResults();
		}

		//バトル終了時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeAfterCombatEnd)
		{
			m_state = GameStatus.Result;

			//ステージの停止
			Stage.CurrentFieldScript.StopField();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果を記録
	/// </summary>
	void RecordBattleResults()
	{
		//高さの同期
		OperatingNetPlayerSetting.CmdFinalAltitude(OperatingPlayer.transform.position.y);

		//コインの同期
		OperatingNetPlayerSetting.CmdNumOfCoins(OperatingPlayer.NumOfCoins);

		//スコアの同期
		OperatingNetPlayerSetting.CmdScore(
			(int)(OperatingPlayer.transform.position.y * (1 + m_coinMagnificationAgainstHeight * OperatingPlayer.NumOfCoins)));

		//経験値の計算
		CalculationOfExp();

		//パワーの計算
		CalculationOfPower();

		//キャラクター戦歴の保存
		MyGameInfo.Instance.SaveVariable();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 経験値の計算
	/// </summary>
	void CalculationOfExp()
	{
		//経験値追加前の経験値
		m_workInt = MyGameInfo.Instance.Exp;

		//経験値の加算
		m_additionalExp = m_expParticipated + ((int)(OperatingPlayer.transform.position.y) / m_heightPerExp);
		MyGameInfo.Instance.Exp += m_additionalExp;

		//目標経験値を求める
		m_nextTargetExp.Clear();
		for (var i = 0; i < m_targetExp.Length; i++)
		{
			//目標経験値を追加
			if (m_workInt < m_targetExp[i])
				m_nextTargetExp.Add(m_targetExp[i]);

			//現在の目標経験値
			if (MyGameInfo.Instance.Exp < m_targetExp[i])
			{
				//ランクの代入
				MyGameInfo.Instance.Level = i + 1;
				break;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// パワーの計算
	/// </summary>
	void CalculationOfPower()
	{
		//今回のパワー
		m_workInt = (int)(OperatingPlayer.transform.position.y * (1 + m_coinMagnificationAgainstHeight * OperatingPlayer.NumOfCoins));

		//パワーが記録されている
		if (MyGameInfo.Instance.Power != 0)
		{
			//疑似的にパワーの平均値を求める
			m_workInt = (MyGameInfo.Instance.Power * (m_powerCollectionCount - 1) + m_workInt) / m_powerCollectionCount;
		}

		//パワーの代入
		MyGameInfo.Instance.Power = m_workInt;
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

			//戦績の代入
			MyNetPlayerSetting.PutPlayerAchievementsInBattleRecord(ref m_battleRecords);

			//プレイヤーとプレイヤーズとカメラとUI
			MakePlayerIntoResultState();
			Players.DisplaySplashes(false);
			OperatingCamera.BecomeCustomOperablePursuitCamera(false, (int)TypesOfCustomCamera.DistantTrackingCamera);
			MainUi.MakeItResultState();

			//フラグの初期化
			m_isLanding = false;
			m_isDisplayPodium = false;
			m_isResultDisplay = false;
			m_isDisplayOfExp = false;
			m_isRematchDisplay = false;
			m_isContinueBattle = true;
			OperatingNetPlayerSetting.CmdNotifyOfIsReady(false);
		}

		//着水していないandプレイヤーが着水
		if (!m_isLanding && OperatingPlayer.transform.position.y <= 0f)
		{
			m_isLanding = true;

			//バトル結果
			BattleResult();
		}

		//表彰台が表示される時間
		if (!m_isDisplayPodium && m_countTheTimeOfTheState >= m_timePodiumIsDisplayed)
		{
			//時間経過による表彰台の位置
			Stage.CurrentFieldScript.MovePodium(
				(m_countTheTimeOfTheState - m_timePodiumIsDisplayed) / (m_timePodiumWasDisplayed - m_timePodiumIsDisplayed));

			//プレイヤー
			ControlPlayerWhenPodiumIsDisplayed();
		}

		//表彰台が表示された時間
		if (!m_isDisplayPodium && m_countTheTimeOfTheState >= m_timePodiumWasDisplayed)
		{
			m_isDisplayPodium = true;

			//表彰台の完全表示
			Stage.CurrentFieldScript.MovePodium(1);

			//SE
			MySoundManager.Instance.Play(SeCollection.Result);
		}

		//結果表示していないand結果表示する時間
		if (!m_isResultDisplay && m_countTheTimeOfTheState >= m_timeToStartDisplayingResults)
		{
			m_isResultDisplay = true;

			//バトル結果表示
			BattleResultDisplay();
		}

		//経験値を表示していないand経験値を表示する時間
		if (!m_isDisplayOfExp && m_countTheTimeOfTheState >= m_timeToStartDisplayingExp)
		{
			m_isDisplayOfExp = true;

			//経験値の表示
			MainUi.ShowExp((MyGameInfo.Instance.Exp - m_additionalExp), m_additionalExp, m_nextTargetExp.ToArray());
		}

		//再戦表示していないand再戦表示する時間
		if (!m_isRematchDisplay && m_countTheTimeOfTheState >= m_timeToStartDisplayingRematch)
		{
			m_isRematchDisplay = true;

			//再戦表示
			MainUi.ShowRematch();
		}

		//再戦表示済み
		if (m_isRematchDisplay)
		{
			//再戦の選択
			SelectRematch();
		}

		//プレイヤーの向き
		m_workVector3 = OperatingCamera.transform.position;
		m_workVector3.y = OperatingPlayer.transform.position.y;
		OperatingPlayer.transform.LookAt(m_workVector3);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーを結果状態にする
	/// </summary>
	void MakePlayerIntoResultState()
	{
		OperatingPlayer.MakeItResultState();

		//プレイヤーが指定高度より高い
		if (OperatingPlayer.transform.position.y >= m_playerInitialHeightOfResultState)
		{
			//高さの変更
			m_workVector3 = OperatingPlayer.transform.position;
			m_workVector3.y = m_playerInitialHeightOfResultState;
			OperatingPlayer.transform.position = m_workVector3;
		}

		//プレイヤー番号による指定位置
		m_workVector3 = Stage.CurrentFieldScript.PositionsOfRank[OperatingNetPlayerSetting.Rank];

		//高さを除いた位置を代入
		m_workVector3.y = OperatingPlayer.transform.position.y;
		OperatingPlayer.transform.position = m_workVector3;

		//Z軸を中心として外側を見る
		OperatingPlayer.transform.LookAt(Vector3.Scale(OperatingPlayer.transform.position, Vector3.one + (Vector3.right * 2)));
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果
	/// </summary>
	void BattleResult()
	{
		//順位が上位or順位が表彰対象
		if (OperatingNetPlayerSetting.Rank < MyNetPlayerSetting.NetPlayerSettings.Count / 2 || OperatingNetPlayerSetting.Rank < m_numToBeAwarded)
			WinBattle();
		else
			LoseBattle();

		//UIに順位の表示
		MainUi.DisplayRanking(OperatingNetPlayerSetting.Rank + 1, OperatingNetPlayerSetting.Score);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルに勝つ
	/// </summary>
	void WinBattle()
	{
		m_isWin = true;
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.SuccessfulLanding);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルに負ける
	/// </summary>
	void LoseBattle()
	{
		m_isWin = false;
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.LandingFailed);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 表彰台が表示される時にプレイヤーを制御
	/// </summary>
	void ControlPlayerWhenPodiumIsDisplayed()
	{
		//プレイヤーのリジッドボディの速度を０
		OperatingPlayer.SetVelocityOfRbToZero();

		//順位のアニメーション
		OperatingPlayer.StartAnimByRank(OperatingNetPlayerSetting.Rank + 1);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果表示
	/// </summary>
	void BattleResultDisplay()
	{
		//プレイヤーアニメーション
		OperatingPlayer.SetAnimation(m_isWin ? PlayerBehaviorStatus.Win : PlayerBehaviorStatus.Defeat);

		//カメラ設定
		CameraSettingOfBattleResultDisplay();

		//UI表示
		MainUi.DisplayResults(m_battleRecords, OperatingNetPlayerSetting.GetNetPlayerNum());
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果表示のカメラ設定
	/// </summary>
	void CameraSettingOfBattleResultDisplay()
	{
		//指定カメラの位置と方向
		m_cameraPosForSpecifying = new Vector3[m_relativeCameraPosSeenFromDisplayingResult.Length];
		m_cameraDirectionForSpecifying = new Vector3[m_relativeCameraPosSeenFromDisplayingResult.Length];

		//指定カメラに必要な位置と方向の作成
		for (var i = 0; i < m_relativeCameraPosSeenFromDisplayingResult.Length; i++)
		{
			//プレイヤーの前方位置からカメラ位置を生成(プレイヤーの向きに応じた位置)
			m_cameraPosForSpecifying[i] =
				(OperatingPlayer.transform.position + (Vector3.up * OperatingCamera.HeightToWatch)
				+ (OperatingPlayer.transform.forward * OperatingCamera.DistanceToPlayer))
				+ OperatingPlayer.transform.right * m_relativeCameraPosSeenFromDisplayingResult[i].x
				+ OperatingPlayer.transform.up * m_relativeCameraPosSeenFromDisplayingResult[i].y
				+ OperatingPlayer.transform.forward * m_relativeCameraPosSeenFromDisplayingResult[i].z;

			//プレイヤーを見る方向を生成
			m_cameraDirectionForSpecifying[i] =
				OperatingPlayer.transform.position + (Vector3.up * OperatingCamera.HeightToWatch) - m_cameraPosForSpecifying[0];
		}

		//指定位置をたどるカメラ
		OperatingCamera.BecomeFollowSpecifiedPosCamera(
			m_cameraPosForSpecifying, m_cameraDirectionForSpecifying, m_cameraMovementTimeDisplayingResults);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再戦の選択
	/// </summary>
	void SelectRematch()
	{
		//Dパッドか左スティックの横方向の入力あり
		if (m_isDpadXBecamePositive || m_isDpadXBecameNegative || m_isHorizontalBecamePositive || m_isHorizontalBecameNegative)
			m_isContinueBattle = !m_isContinueBattle;

		//ボタンの色変更
		MainUi.SelectionOfRematch(m_isContinueBattle);

		//もう一度か終了か
		if (m_isAButtonDown)
		{
			//バトルを続けるか辞めるか
			if (m_isContinueBattle)
				ContinueBattle();
			else
				LeaveBattle();
		}
		else if (m_isBButtonDown)
		{
			LeaveBattle();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを続ける
	/// </summary>
	public override void ContinueBattle()
	{
		m_state = GameStatus.RecruitPeople;
		OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを辞める
	/// </summary>
	public override void LeaveBattle()
	{
		m_netManager.StopConnection();
		MySceneManager.Instance.ChangeScene(MyScene.Armed);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力のリセット
	/// </summary>
	void ResetInput()
	{
		m_isAButtonDown = false;
		m_isBButtonDown = false;
		m_isYButtonDown = false;
		m_isDpadXBecamePositive = false;
		m_isDpadXBecameNegative = false;
		m_isDpadYBecameNegative = false;
		m_isHorizontalBecamePositive = false;
		m_isHorizontalBecameNegative = false;
	}
}
