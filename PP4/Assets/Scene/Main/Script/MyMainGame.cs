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
	/// サーバ接続を確認する時間
	/// </summary>
	[SerializeField]
	float m_timeToCheckServerConnection;

	/// <summary>
	/// 人が集まり状態遷移する時間
	/// </summary>
	[SerializeField]
	float m_timeWhenPeopleGatherAndChangeState;

	/// <summary>
	/// 整列時間
	/// </summary>
	[SerializeField]
	float m_alignmentTime;

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
	/// 最低プレイヤー人数
	/// </summary>
	[SerializeField]
	int m_minNumOfPlayers;
	public int MinNumOfPlayers
	{
		get { return m_minNumOfPlayers; }
	}

	/// <summary>
	/// 人が集まった時間
	/// </summary>
	float m_timeWhenPeopleGathered;

	/// <summary>
	/// サーバ確認フラグ
	/// </summary>
	bool m_isCheckServer;

	/// <summary>
	/// 整列するプレイヤーの初期位置
	/// </summary>
	Vector3 m_initialPosOfPlayerToBeAligned;

	/// <summary>
	/// 整列を開始する時間
	/// </summary>
	float m_timeToStartAligning;

	/// <summary>
	/// 整列フラグ
	/// </summary>
	bool m_isAlignment;

	/// <summary>
	/// フレーム前のプレイヤー人数
	/// </summary>
	int m_numOfPlayersPrev;
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
	/// 順位の表示
	/// </summary>
	bool m_isDisplayRank;

	/// <summary>
	/// 操作表の表示
	/// </summary>
	bool m_isDisplayOperationTable;

	/// <summary>
	/// 操作プレイヤーが落下フラグ
	/// </summary>
	bool m_isOperatingPlayerFall;

	/// <summary>
	/// プレイヤーの帯電時間を数える
	/// </summary>
	float m_countChargingTimeOfPlayer;

	/// <summary>
	/// プレイヤーの帯電率を数える
	/// </summary>
	float[] m_countPlayerChargeRate;
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
	/// 再戦待機時間
	/// </summary>
	[SerializeField]
	float m_rematchWaitingTime;

	/// <summary>
	/// 結果状態のプレイヤーの初期高さ
	/// </summary>
	[SerializeField]
	float m_playerInitialHeightOfResultState;

	/// <summary>
	/// 着地を試みる高さ
	/// </summary>
	[SerializeField]
	float m_heightTryingToLand;

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

	/// <summary>
	/// 再戦待機時間を数える
	/// </summary>
	float m_countRematchWaitingTime;
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
	/// スタートボタンを押した
	/// </summary>
	bool m_isStartButtonDown;

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
		m_countRematchWaitingTime = 0;

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
		if (Input.GetButtonDown("HomeButton"))
			m_isStartButtonDown = true;
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
			m_countTheTimeOfTheState = 0 + (m_statePrev == GameStatus.Result ? m_countRematchWaitingTime : 0);

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
			Players.ResetPlayerColor();
			GhostPlayers.SetActive(true);
			OperatingCamera.BecomeFixedCamera(m_cameraPosWhenWaitingForPeople, m_cameraDirectionWhenWaitingForPeople);
			MainUi.WantedRecruitment(MyGameInfo.Instance.Level, MyGameInfo.Instance.Exp, MyGameInfo.Instance.Power);

			//レベルとパワーの同期
			OperatingNetPlayerSetting.CmdLevel(MyGameInfo.Instance.Level);
			OperatingNetPlayerSetting.CmdPower(MyGameInfo.Instance.Power);

			//変数
			m_timeWhenPeopleGathered = -1;
			m_isCheckServer = false;

			//BGM
			MySoundManager.Instance.Play(BgmCollection.Matching);
		}

		//ゲームが開始できるか
		if (OperatingNetPlayerSetting.IsBattleStart() || OperatingNetPlayerSetting.IsDuringBattle())
		{
			//次の状態への準備
			GetReadyForPeopleGathered();
		}
		else if (m_countTheTimeOfTheState > m_timeToWaitForPeople)
		{
			//時間切れ時のサーバ処理
			if (!OperatingNetPlayerSetting.IsCreateAi && OperatingNetPlayerSetting.GetNetPlayerNum() == 0)
			{
				//AIキャラのスポーンとゲーム開始通知
				OperatingNetPlayerSetting.SpawnAiCharacter(true);
				OperatingNetPlayerSetting.CmdIsDuringBattle(true);
			}
		}
		else
		{
			//状態の更新
			UpdatingStateToRecruitPeople();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まった状態の準備
	/// </summary>
	void GetReadyForPeopleGathered()
	{
		//人が集まった時間
		if (m_timeWhenPeopleGathered == -1)
		{
			m_timeWhenPeopleGathered = m_countTheTimeOfTheState;

			//人材募集の停止
			m_netManager.matchMaker.SetMatchAttributes(
				(UnityEngine.Networking.Types.NetworkID)m_netManager.NetworkIdInUse, false, 0, m_netManager.OnSetMatchAttributes);
		}

		//状態遷移する時間
		if (m_countTheTimeOfTheState >= m_timeWhenPeopleGathered + m_timeWhenPeopleGatherAndChangeState)
			m_state = GameStatus.PeopleGathered;

		//時間表示
		MainUi.SetTimeToWaitWorPeople(0);

		//プレイヤーの位置固定
		MovePlayerToPosToWaitForPeople();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を待つ位置にプレイヤーを移動する
	/// </summary>
	void MovePlayerToPosToWaitForPeople()
	{
		//操作プレイヤーの番号
		m_workInt = OperatingNetPlayerSetting.GetNetPlayerNum();

		//操作プレイヤー
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Stand);
		OperatingPlayer.StandAtSpecifiedPos(m_playerPosWhenWaitingForPeople[m_workInt], m_playerDirectionWhenWaitingForPeople[m_workInt]);

		//AIプレイヤー
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//AIプレイヤーの番号
			m_workInt = aiPlayer.GetNetPlayerNum();

			aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.Stand);
			aiPlayer.PlayerScript.StandAtSpecifiedPos(m_playerPosWhenWaitingForPeople[m_workInt], m_playerDirectionWhenWaitingForPeople[m_workInt]);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人を集めるための状態を更新
	/// </summary>
	void UpdatingStateToRecruitPeople()
	{
		//人が集まっていない
		m_timeWhenPeopleGathered = -1;

		//サーバのチェック
		if(!m_isCheckServer && m_countTheTimeOfTheState >= m_timeToCheckServerConnection)
		{
			m_isCheckServer = true;

			//一つ目のサーバでないandプレイヤーが1人
			if (!m_netManager.IsFirstConnection() && m_numOfPlayersPrev == 1)
				m_netManager.StopConnection();
		}

		//整列中
		if (!OperatingNetPlayerSetting.IsReady)
			AlignmentWhenPeopleGather();
		else
			MovePlayerToPosToWaitForPeople();

		//プレイヤー人数が増えた
		if (m_numOfPlayersPrev < MyNetPlayerSetting.NetPlayerSettings.Count)
			m_countTheTimeOfTheState = 0;

		//解放時間を更新
		MainUi.SetTimeToWaitWorPeople(m_timeToWaitForPeople - m_countTheTimeOfTheState);

		//プレイヤー人数の更新
		m_numOfPlayersPrev = MyNetPlayerSetting.NetPlayerSettings.Count;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 人が集まる時の整列
	/// </summary>
	void AlignmentWhenPeopleGather()
	{
		//初期
		if (OperatingNetPlayerSetting.IsAlignment && !m_isAlignment)
		{
			m_isAlignment = true;

			//設定
			m_initialPosOfPlayerToBeAligned = OperatingNetPlayerSetting.transform.position;
			m_timeToStartAligning = m_countTheTimeOfTheState;

			//整列対象
			if (OperatingNetPlayerSetting.GetNetPlayerNum() >= OperatingNetPlayerSetting.NumOfPlayerWithDisconnected)
				OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Run);
		}

		//整列対象
		if (OperatingNetPlayerSetting.GetNetPlayerNum() >= OperatingNetPlayerSetting.NumOfPlayerWithDisconnected)
		{
			//整列経過時間とプレイヤーネット番号
			m_workFloat = m_countTheTimeOfTheState - m_timeToStartAligning;
			m_workInt = OperatingNetPlayerSetting.GetNetPlayerNum();

			//時間によるプレイヤーの位置
			OperatingPlayer.transform.position =
				Vector3.Lerp(m_initialPosOfPlayerToBeAligned, m_playerPosWhenWaitingForPeople[m_workInt], m_workFloat / m_alignmentTime);

			//向きたい角度
			m_workFloat =
				Vector3.Cross(OperatingPlayer.transform.forward, m_playerPosWhenWaitingForPeople[m_workInt] - m_initialPosOfPlayerToBeAligned).y;
			m_workFloat = Vector3.Angle(OperatingPlayer.transform.forward,
				Vector3.Scale((m_playerPosWhenWaitingForPeople[m_workInt] - m_initialPosOfPlayerToBeAligned), Vector3.right + Vector3.forward))
				* (m_workFloat < 0 ? -1 : 1);

			//向ける角度が向きたい角度より大きい
			if (m_playerRotationSpeed * Time.deltaTime >= Mathf.Abs(m_workFloat))
				OperatingPlayer.transform.Rotate(Vector3.up, m_workFloat);
			else if (m_workFloat > 0)
				OperatingPlayer.transform.Rotate(Vector3.up, m_playerRotationSpeed * Time.deltaTime);
			else
				OperatingPlayer.transform.Rotate(Vector3.up, -m_playerRotationSpeed * Time.deltaTime);
		}

		//終了
		if (m_countTheTimeOfTheState - m_timeToStartAligning >= m_alignmentTime)
		{
			//プレイヤーネット番号
			m_workInt = OperatingNetPlayerSetting.GetNetPlayerNum();

			//整列後の位置
			MovePlayerToPosToWaitForPeople();

			//設定
			OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
			OperatingNetPlayerSetting.NumOfPlayerWithDisconnected = MyNetPlayerSetting.NetPlayerSettings.Count;
			OperatingNetPlayerSetting.IsAlignment = false;
			m_isAlignment = false;
		}
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

			//SE
			MySoundManager.Instance.Play(SeCollection.PeopleGathered);
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
		//プレイヤーネット番号
		m_workInt = OperatingNetPlayerSetting.GetNetPlayerNum();

		//プレイヤーが船の端に行く時間
		if (m_countTheTimeOfTheState <= m_timeWhenPlayerGoesToEdgeOfShip)
		{
			//走るアニメーション
			OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Run);

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

			//AIの処理
			AiPlayerGoesToEdgeOfShip();

			return;
		}

		//プレイヤーが飛び降りる時間
		if (m_countTheTimeOfTheState <= m_timeWhenPlayerGoesToEdgeOfShip + m_timePlayerJumpsOffShip)
		{
			//アイドルアニメーション
			OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Idle);

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

			//AIの処理
			AiPlayerJumpsOff();

			return;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// AIプレイヤーが船の端に行く
	/// </summary>
	void AiPlayerGoesToEdgeOfShip()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//プレイヤーネット番号
			m_workInt = aiPlayer.GetNetPlayerNum();

			//走るアニメーション
			aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.Run);

			//プレイヤーの初期位置
			m_workVector3 = m_playerPosWhenWaitingForPeople[m_workInt];

			//時間によるプレイヤーの位置
			aiPlayer.PlayerScript.transform.position = m_workVector3 +
				((m_posOfEndOfShip[m_workInt] - m_workVector3) * (m_countTheTimeOfTheState / m_timeWhenPlayerGoesToEdgeOfShip));

			//向きたい角度
			m_workFloat = Vector3.Cross(aiPlayer.PlayerScript.transform.forward, m_posOfEndOfShip[m_workInt] - m_workVector3).y;
			m_workFloat = Vector3.Angle(aiPlayer.PlayerScript.transform.forward,
				Vector3.Scale((m_posOfEndOfShip[m_workInt] - m_workVector3), Vector3.right + Vector3.forward)) * (m_workFloat < 0 ? -1 : 1);

			//向ける角度が向きたい角度より大きい
			if (m_playerRotationSpeed * Time.deltaTime >= Mathf.Abs(m_workFloat))
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, m_workFloat);
			else if (m_workFloat > 0)
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, m_playerRotationSpeed * Time.deltaTime);
			else
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, -m_playerRotationSpeed * Time.deltaTime);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// AIプレイヤーが飛び降りる
	/// </summary>
	void AiPlayerJumpsOff()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//プレイヤーネット番号
			m_workInt = aiPlayer.GetNetPlayerNum();

			//アイドルアニメーション
			aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.Idle);

			//プレイヤーの初期位置
			m_workVector3 = m_posOfEndOfShip[m_workInt];

			//時間によるプレイヤーの位置
			aiPlayer.PlayerScript.transform.position = m_workVector3 + ((m_posToJumpOffShip[m_workInt] - m_workVector3)
				* ((m_countTheTimeOfTheState - m_timeWhenPlayerGoesToEdgeOfShip) / m_timePlayerJumpsOffShip));

			//向きたい角度
			m_workFloat = Vector3.Cross(aiPlayer.PlayerScript.transform.forward, m_posToJumpOffShip[m_workInt] - m_workVector3).y;
			m_workFloat = Vector3.Angle(aiPlayer.PlayerScript.transform.forward,
				Vector3.Scale((m_posToJumpOffShip[m_workInt] - m_workVector3), Vector3.right + Vector3.forward)) * (m_workFloat < 0 ? -1 : 1);

			//向ける角度が向きたい角度より大きい
			if (m_playerRotationSpeed * Time.deltaTime >= Mathf.Abs(m_workFloat))
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, m_workFloat);
			else if (m_workFloat > 0)
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, m_playerRotationSpeed * Time.deltaTime);
			else
				aiPlayer.PlayerScript.transform.Rotate(Vector3.up, -m_playerRotationSpeed * Time.deltaTime);
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
			if (m_battleRecords == null || m_battleRecords.Length != MyNetPlayerSetting.NetPlayerSettings.Count)
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
		OperatingPlayer.transform.LookAt(Vector3.right + Vector3.forward);

		//アニメーション
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Select);

		//AIの処理
		AiPlayerBattleSettings();

		//プレイヤーの帯電率のリセット
		if (m_countPlayerChargeRate == null || m_countPlayerChargeRate.Length != MyNetPlayerSetting.NetPlayerSettings.Count)
		{
			m_countPlayerChargeRate = new float[MyNetPlayerSetting.NetPlayerSettings.Count];
		}
		else
		{
			for (var i = 0; i < m_countPlayerChargeRate.Length; i++)
			{
				m_countPlayerChargeRate[i] = 0;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// AIプレイヤーのバトル設定
	/// </summary>
	void AiPlayerBattleSettings()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//プレイヤー番号よる位置
			aiPlayer.PlayerScript.transform.position = Stage.CurrentFieldScript.StartPositions[aiPlayer.GetNetPlayerNum()];

			//中心を見る
			aiPlayer.PlayerScript.transform.LookAt(Vector3.right + Vector3.forward);

			//アニメーション
			aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.Select);
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

			//フラグとプレイヤーとカメラとUIの設定
			m_isItJustBeforeBattle = false;
			m_isDisplayRank = true;
			m_isDisplayOperationTable = false;
			Players.UpdateHeightRank();
			SettingOfCameraInBattleStartState();
			MainUi.SetRank(Players.HeightRanks, OperatingNetPlayerSetting.GetNetPlayerNum());
			MainUi.BattleStart(m_battleTime);
			MainUi.MarkPlayersOnMap(OperatingNetPlayerSetting.GetNetPlayerNum());

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
			foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
			{
				aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.Idle);
			}
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

			//プレイヤーとカメラとUIとフラグ
			OperatingPlayer.MakeItBattleState();
			foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
			{
				aiPlayer.PlayerScript.MakeItBattleState();
			}
			OperatingNetPlayerSetting.NameplateDisplay(false);
			OperatingCamera.BecomeOperablePursuitCamera();

			//BGM
			MySoundManager.Instance.Play(BgmCollection.Battle);
		}

		//ステージとプレイヤーとカメラとUI
		BattleStateStage();
		BattleStatePlayer();
		BattleStateCamera();
		BattleStateUi();

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

		//オーラボール
		BattleStateAuraBall();

		//AI処理
		BattleStateAiPlayer();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のオーラボール
	/// </summary>
	void BattleStateAuraBall()
	{
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
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル状態のAIプレイヤー
	/// </summary>
	void BattleStateAiPlayer()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//サポート
			aiPlayer.PlayerScript.SupportRate = 1 +
				(Players.MaximumAltitude - aiPlayer.PlayerScript.transform.position.y) * (m_supportRatePerMeter - 1);

			//SPゲージが満タンでない
			if (aiPlayer.PlayerScript.GetPercentageOfRemainingSpGauge() < 1f)
				continue;

			//上位の順位
			m_workInt = Players.HeightRanks[aiPlayer.GetNetPlayerNum()] - 1;

			if (m_workInt < 0)
				continue;

			//一つ上の上位プレイヤー
			m_workGameObj = Players.GetPlayer(m_workInt);

			//3種のオーラ
			if (((MyAiPlayer)(aiPlayer.PlayerScript)).WantToThrowHeatAura)
				aiPlayer.ThrowAuraBall(m_workGameObj, AuraAttribute.Heat);
			if (((MyAiPlayer)(aiPlayer.PlayerScript)).WantToThrowElasticityAura)
				aiPlayer.ThrowAuraBall(m_workGameObj, AuraAttribute.Elasticity);
			if (((MyAiPlayer)(aiPlayer.PlayerScript)).WantToThrowElectricalAura)
				aiPlayer.ThrowAuraBall(m_workGameObj, AuraAttribute.Electrical);
		}
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
		MainUi.SetMarkThatCanNotAccelerated(OperatingPlayer.IsUseSp);

		//順位の反映
		MainUi.SetRank(Players.HeightRanks, OperatingNetPlayerSetting.GetNetPlayerNum());

		//Yボタンを押した
		if (m_isYButtonDown)
		{
			m_isDisplayRank = !m_isDisplayRank;

			//順位の表示
			MainUi.ShowRankOnMap(m_isDisplayRank);
		}

		//Startボタンを押した
		if (m_isStartButtonDown)
		{
			m_isDisplayOperationTable = !m_isDisplayOperationTable;

			//操作表の表示
			MainUi.ShowOperationTable(m_isDisplayOperationTable);
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

		//落下状態
		if (m_isOperatingPlayerFall)
			MainUi.SetRecoveryRate(OperatingPlayer.GetRecoveryRate());

		//オーラボールのヒット情報
		MainUi.ShowHitInfoOfAuraBall(OperatingPlayer.GetKilledPlayerName());
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
			OperatingNetPlayerSetting.MakeItBattleEndState();
			MainUi.EndBattle();
			m_isFadeOutAfterBattleEnds = false;

			//Sound
			MySoundManager.Instance.StopBGM();
			MySoundManager.Instance.Play(SeCollection.BattleEnd);
		}

		//フェードアウトしていないandフェードアウト時間
		if (!m_isFadeOutAfterBattleEnds && m_countTheTimeOfTheState >= m_timeWhenTheBattleEndsAndStops)
		{
			//フェードアウト
			m_isFadeOutAfterBattleEnds = true;
			MainUi.StartFadeOut();

			//オーラボールの停止
			MyNetPlayerSetting.EraseAllAuraBall();

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

		//AIの同期
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//高さの同期
			aiPlayer.CmdFinalAltitude(aiPlayer.transform.position.y);

			//コインの同期
			aiPlayer.CmdNumOfCoins(aiPlayer.PlayerScript.NumOfCoins);

			//スコアの同期
			aiPlayer.CmdScore((int)(aiPlayer.transform.position.y * (1 + m_coinMagnificationAgainstHeight * aiPlayer.PlayerScript.NumOfCoins)));
		}

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
		if (!m_isLanding)
		{
			//バトル結果
			BattleResult();
			AiPlayerBattleResult();
		}

		//表彰台が表示される時間
		if (!m_isDisplayPodium && m_countTheTimeOfTheState >= m_timePodiumIsDisplayed)
		{
			m_isLanding = true;

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

			//表彰台の完全表示と表彰
			Stage.CurrentFieldScript.MovePodium(1);
			Stage.CurrentFieldScript.HonorPlayer(OperatingNetPlayerSetting.transform.position, m_isWin);

			//SE
			MySoundManager.Instance.Play(SeCollection.Result);
		}

		//結果表示していないand結果表示する時間
		if (!m_isResultDisplay && m_countTheTimeOfTheState >= m_timeToStartDisplayingResults)
		{
			m_isResultDisplay = true;

			//バトル結果表示
			BattleResultDisplay();

			//BGM
			MySoundManager.Instance.Play(m_isWin ? BgmCollection.Win : BgmCollection.Defeat);
		}

		//経験値を表示していないand経験値を表示する時間
		if (!m_isDisplayOfExp && m_countTheTimeOfTheState >= m_timeToStartDisplayingExp)
		{
			m_isDisplayOfExp = true;

			//経験値の表示
			MainUi.ShowExp((MyGameInfo.Instance.Exp - m_additionalExp), m_additionalExp, m_nextTargetExp.ToArray(), MyGameInfo.Instance.Level);
		}

		//再戦表示していないand再戦表示する時間
		if (!m_isRematchDisplay && m_countTheTimeOfTheState >= m_timeToStartDisplayingRematch)
		{
			m_isRematchDisplay = true;

			//再戦表示
			MainUi.ShowRematch();

			//AIキャラクターの削除とバトル中フラグと人材募集開始
			if (OperatingNetPlayerSetting.GetNetPlayerNum() == 0)
			{
				OperatingNetPlayerSetting.SpawnAiCharacter(false);
				OperatingNetPlayerSetting.CmdIsDuringBattle(false);
				m_netManager.matchMaker.SetMatchAttributes(
					(UnityEngine.Networking.Types.NetworkID)m_netManager.NetworkIdInUse, true, 0, m_netManager.OnSetMatchAttributes);
			}

			m_countRematchWaitingTime = 0;
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
		if (OperatingNetPlayerSetting.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
			m_workVector3 = Stage.CurrentFieldScript.PositionsOfRank[OperatingNetPlayerSetting.Rank];
		else
			m_workVector3 = Stage.CurrentFieldScript.PositionsOfRank[
				Stage.CurrentFieldScript.PositionsOfRank.Length - (MyNetPlayerSetting.NetPlayerSettings.Count - OperatingNetPlayerSetting.Rank)];

		//高さを除いた位置を代入
		m_workVector3.y = OperatingPlayer.transform.position.y;
		OperatingPlayer.transform.position = m_workVector3;

		//AI処理
		MakeAiPlayerIntoResultState();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// AIプレイヤーを結果状態にする
	/// </summary>
	void MakeAiPlayerIntoResultState()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			aiPlayer.PlayerScript.MakeItResultState();

			//指定高度より高い
			if (aiPlayer.PlayerScript.transform.position.y >= m_playerInitialHeightOfResultState)
			{
				//高さの変更
				m_workVector3 = aiPlayer.PlayerScript.transform.position;
				m_workVector3.y = m_playerInitialHeightOfResultState;
				aiPlayer.PlayerScript.transform.position = m_workVector3;
			}

			//プレイヤー番号による指定位置
			if (aiPlayer.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
				m_workVector3 = Stage.CurrentFieldScript.PositionsOfRank[aiPlayer.Rank];
			else
				m_workVector3 = Stage.CurrentFieldScript.PositionsOfRank[
					Stage.CurrentFieldScript.PositionsOfRank.Length - (MyNetPlayerSetting.NetPlayerSettings.Count - aiPlayer.Rank)];

			//高さを除いた位置を代入
			m_workVector3.y = aiPlayer.PlayerScript.transform.position.y;
			aiPlayer.PlayerScript.transform.position = m_workVector3;

			//X軸を中心として反対側を見る
			aiPlayer.PlayerScript.transform.LookAt(Vector3.Scale(aiPlayer.PlayerScript.transform.position, Vector3.one + (-Vector3.forward * 2)));
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果
	/// </summary>
	void BattleResult()
	{
		//着地していない
		if (OperatingPlayer.transform.position.y > m_heightTryingToLand)
			return;

		//順位が上位
		if (OperatingNetPlayerSetting.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
			WinBattle();
		else
			LoseBattle();

		//UIに順位の表示
		MainUi.DisplayRanking(OperatingNetPlayerSetting.Rank + 1, OperatingNetPlayerSetting.Score);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// AIプレイヤーのバトル結果
	/// </summary>
	void AiPlayerBattleResult()
	{
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//着地していない
			if (aiPlayer.transform.position.y > m_heightTryingToLand)
				continue;

			if (aiPlayer.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
				aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.SuccessfulLanding);
			else
				aiPlayer.PlayerScript.SetAnimation(PlayerBehaviorStatus.LandingFailed);
		}
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
		if (OperatingNetPlayerSetting.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
			OperatingPlayer.StartAnimByRank(OperatingNetPlayerSetting.Rank + 1);
		else
			OperatingPlayer.StartAnimByRank(Stage.CurrentFieldScript.PositionsOfRank.Length
				- (MyNetPlayerSetting.NetPlayerSettings.Count - OperatingNetPlayerSetting.Rank) + 1);

		//AIの処理
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//リジッドボディの速度を０
			aiPlayer.PlayerScript.SetVelocityOfRbToZero();

			//順位のアニメーション
			if (aiPlayer.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2)
				aiPlayer.PlayerScript.StartAnimByRank(aiPlayer.Rank + 1);
			else
				aiPlayer.PlayerScript.StartAnimByRank(Stage.CurrentFieldScript.PositionsOfRank.Length
					- (MyNetPlayerSetting.NetPlayerSettings.Count - aiPlayer.Rank) + 1);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル結果表示
	/// </summary>
	void BattleResultDisplay()
	{
		//プレイヤーアニメーション
		OperatingPlayer.SetAnimation(m_isWin ? PlayerBehaviorStatus.Win : PlayerBehaviorStatus.Defeat);

		//AIの処理
		foreach (var aiPlayer in OperatingNetPlayerSetting.AiNetPlayerSettings)
		{
			//アニメーション
			aiPlayer.PlayerScript.SetAnimation(aiPlayer.Rank < (MyNetPlayerSetting.NetPlayerSettings.Count + 1) / 2 ?
				PlayerBehaviorStatus.Win : PlayerBehaviorStatus.Defeat);
		}

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
				OperatingPlayer.transform.position + (Vector3.up * OperatingCamera.HeightToWatch)
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
		//再戦時間を数える
		m_countRematchWaitingTime += Time.deltaTime;

		//Dパッドか左スティックの横方向の入力あり
		if (m_isDpadXBecamePositive || m_isDpadXBecameNegative || m_isHorizontalBecamePositive || m_isHorizontalBecameNegative)
		{
			m_isContinueBattle = !m_isContinueBattle;

			//SE
			MySoundManager.Instance.Play(SeCollection.Select);
		}

		//UI
		MainUi.SelectionOfRematch(m_isContinueBattle);
		MainUi.SetLeaveBattleCount(m_rematchWaitingTime - m_countRematchWaitingTime + 1);

		//もう一度か終了か
		if (m_isAButtonDown)
		{
			//バトルを続けるか辞めるか
			if (m_isContinueBattle)
				ContinueBattle();
			else
				LeaveBattle();
		}
		else if (m_isBButtonDown || m_countRematchWaitingTime >= m_rematchWaitingTime)
		{
			LeaveBattle();
		}

		//準備完了になった
		if (OperatingNetPlayerSetting.IsReady)
			m_state = GameStatus.RecruitPeople;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを続ける
	/// </summary>
	public override void ContinueBattle()
	{
		OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
		MainUi.MarkPlayersOnMap(OperatingNetPlayerSetting.GetNetPlayerNum(), false);
		MySoundManager.Instance.Play(SeCollection.Decide);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを辞める
	/// </summary>
	public override void LeaveBattle()
	{
		m_netManager.StopConnection();
		MySceneManager.Instance.ChangeScene(MyScene.Armed, true);
		MySoundManager.Instance.Play(SeCollection.PlayerLeaves);
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
		m_isStartButtonDown = false;
		m_isDpadXBecamePositive = false;
		m_isDpadXBecameNegative = false;
		m_isDpadYBecameNegative = false;
		m_isHorizontalBecamePositive = false;
		m_isHorizontalBecameNegative = false;
	}
}
