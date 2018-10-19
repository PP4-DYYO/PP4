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
public enum GameStatus
{
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
	/// チームの人数
	/// </summary>
	public const int NUM_OF_TEAM_MEMBERS = 4;
	#endregion

	#region 人が集まった状態
	[Header("人が集まった状態")]
	/// <summary>
	/// 人が集まった状態の時間
	/// </summary>
	[SerializeField]
	float m_timeWhenPeopleGather;

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
	/// マップ上にチームカラーを表示する時間
	/// </summary>
	[SerializeField]
	float m_timeToDisplayTeamColorOnMap;

	/// <summary>
	/// １ｍ毎のサポート率
	/// </summary>
	[SerializeField]
	float m_supportRatePerMeter;

	/// <summary>
	/// 順位の表示
	/// </summary>
	bool m_isDisplayRank;

	/// <summary>
	/// 操作プレイヤーが落下フラグ
	/// </summary>
	bool m_isOperatingPlayerFall;

	/// <summary>
	/// マップ上にチームカラーを表示する時間が過ぎたフラグ
	/// </summary>
	bool m_isTimeToDisplayTeamColorOnMapHasPassed;
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
	/// チーム１のスコア
	/// </summary>
	int m_scoreOfTeam1;

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	int m_scoreOfTeam2;
	
	/// <summary>
	/// 勝ちフラグ
	/// </summary>
	bool m_isWin;

	/// <summary>
	/// 着水フラグ
	/// </summary>
	bool m_isLanding;

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
			OperatingNetPlayerSetting.NameplateDisplay();
			MovePlayerToPosToWaitForPeople(OperatingNetPlayerSetting.GetPlayerNum());
			GhostPlayers.SetActive(true);
			OperatingCamera.BecomeFixedCamera(m_cameraPosWhenWaitingForPeople, m_cameraDirectionWhenWaitingForPeople);
			MainUi.WantedRecruitment(MyGameInfo.Instance.Rank, MyGameInfo.Instance.Exp, MyGameInfo.Instance.Power);

			//ランクの同期
			OperatingNetPlayerSetting.CmdRank(MyGameInfo.Instance.Rank);
		}

		//ゲームが開始できるか
		if (OperatingNetPlayerSetting.IsBattleStart())
		{
			//バトルの開始設定
			m_state = GameStatus.PeopleGathered;
		}
		else if (m_countTheTimeOfTheState > m_timeToWaitForPeople)
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
		OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Stand);
		OperatingPlayer.StandAtSpecifiedPos(m_playerPosWhenWaitingForPeople[playerNum], m_playerDirectionWhenWaitingForPeople[playerNum]);
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

			//プレイヤーとカメラとUI
			OperatingPlayer.SetAnimation(
				(OperatingPlayer.transform.position.x < 0) ? PlayerBehaviorStatus.HoldBoardInHand : PlayerBehaviorStatus.HoldBoardInHand2);
			GhostPlayers.SetActive(false);
			OperatingCamera.BecomeFollowSpecifiedPosCamera(
				m_cameraPosWhenPeopleGather, m_cameraDirectionWhenPeopleGather, m_cameraMovingTimeWhenPeopleGather);
			MainUi.PeopleGathered();
		}

		//設定時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeWhenPeopleGather)
		{
			//状態遷移
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

			//ステージの初期化
			Stage.CurrentFieldScript.ResetField();

			//プレイヤーとUI
			PlayerBattleSettings();
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
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーのバトル設定
	/// </summary>
	void PlayerBattleSettings()
	{
		//チーム分け
		MyNetPlayerSetting.NetPlayerSettings.CopyTo(Players.DecideOnTeam(MyNetPlayerSetting.NetPlayerSettings.ToArray()));

		//チームによる位置
		switch (OperatingNetPlayerSetting.TeamNum)
		{
			case Team.Team1:
				OperatingPlayer.transform.position =
					Stage.CurrentFieldScript.Team1StartPositions[OperatingNetPlayerSetting.TeamOrder];
				break;
			case Team.Team2:
				OperatingPlayer.transform.position =
					Stage.CurrentFieldScript.Team2StartPositions[OperatingNetPlayerSetting.TeamOrder];
				break;
		}

		//Z軸を中心とした反対側を見る
		OperatingPlayer.transform.LookAt(Vector3.Scale(OperatingPlayer.transform.position, Vector3.one - (Vector3.right * 2)));
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
		}

		//カウントダウン時間が過ぎた
		if (m_countTheTimeOfTheState >= m_timeBeforeBattle)
		{
			//状態遷移
			m_state = GameStatus.Battle;

			//Goメッセージ
			MainUi.StartGoAnimation();
		}
		else if (!m_isItJustBeforeBattle && m_countTheTimeOfTheState >= m_timeBeforeBattle - m_timeJustBeforeBattle)
		{
			//バトル直前フラグ（一度だけTrue）
			m_isItJustBeforeBattle = true;

			//プレイヤーとReadyメッセージ設定
			OperatingPlayer.SetAnimation(PlayerBehaviorStatus.Idle);
			MainUi.StartReadyAnimation();
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
			m_isTimeToDisplayTeamColorOnMapHasPassed = false;
		}

		//時間が過ぎていないフラグandマップ上にチームカラーを表示する時間が過ぎた
		if (!m_isTimeToDisplayTeamColorOnMapHasPassed && m_countTheTimeOfTheState >= m_timeToDisplayTeamColorOnMap)
		{
			m_isTimeToDisplayTeamColorOnMapHasPassed = true;

			MainUi.HideTeamColorOnMap();
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
			//プレイヤーとカメラとUI
			BattleStatePlayer();
			BattleStateCamera();
			BattleStateUi();
		}
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

		//順位の反映
		MainUi.SetRank(Players.HeightRanks, OperatingNetPlayerSetting.GetPlayerNum());

		//Yボタンを押した
		if (m_isYButtonDown)
		{
			m_isDisplayRank = !m_isDisplayRank;

			//順位の表示
			MainUi.DisplayRank(m_isDisplayRank);
		}

		//被水している
		if (OperatingPlayer.GetIsWearWater())
			MainUi.WearWater();

		//落下状態が変わった
		if (m_isOperatingPlayerFall != OperatingPlayer.IsFalling)
		{
			m_isOperatingPlayerFall = OperatingPlayer.IsFalling;

			//落下フラグ
			if (m_isOperatingPlayerFall)
				MainUi.StartOfFall();
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

			//プレイヤーとUIとフラグ
			OperatingPlayer.MakeItBattleEndState();
			OperatingNetPlayerSetting.ChangeDisplayNameOfNameplate();
			OperatingNetPlayerSetting.NameplateDisplay();
			MainUi.EndBattle();
			m_isFadeOutAfterBattleEnds = false;
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
				MyGameInfo.Instance.Rank = i + 1;
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
		if(MyGameInfo.Instance.Power != 0)
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

			//勝敗を決める
			DecideOnWinningOrLosing();

			//プレイヤーとカメラとUI
			MakePlayerIntoResultState();
			OperatingCamera.BecomePursuitCamera(false, Vector3.Scale(OperatingPlayer.transform.position, Vector3.right));
			MainUi.MakeItResultState();

			//フラグの初期化
			m_isLanding = false;
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
	/// 勝敗を決める
	/// </summary>
	void DecideOnWinningOrLosing()
	{
		//戦績の取得
		MyNetPlayerSetting.PutPlayerAchievementsInBattleRecord(ref m_battleRecords);

		//チームスコアの初期化
		m_scoreOfTeam1 = 0;
		m_scoreOfTeam2 = 0;

		//全ての戦績
		foreach(var battleRecord in m_battleRecords)
		{
			//チーム毎のスコア
			switch(battleRecord.team)
			{
				case Team.Team1:
					m_scoreOfTeam1 += battleRecord.score;
					break;
				case Team.Team2:
					m_scoreOfTeam2 += battleRecord.score;
					break;
			}
		}

		//チームスコアの代入
		MainUi.ScoreAssignment(m_scoreOfTeam1, m_scoreOfTeam2);
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

		//チームによる指定位置
		switch (OperatingNetPlayerSetting.TeamNum)
		{
			case Team.Team1:
				m_workVector3 = Stage.CurrentFieldScript.Team1StartPositions[OperatingNetPlayerSetting.TeamOrder];
				break;
			case Team.Team2:
				m_workVector3 = Stage.CurrentFieldScript.Team2StartPositions[OperatingNetPlayerSetting.TeamOrder];
				break;
		}

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
		//チーム
		switch (OperatingNetPlayerSetting.TeamNum)
		{
			case Team.Team1:
				//得点により勝ち負け
				if (m_scoreOfTeam1 < m_scoreOfTeam2)
					LoseBattle();
				else
					WinBattle();
				break;
			case Team.Team2:
				//得点により勝ち負け
				if (m_scoreOfTeam1 < m_scoreOfTeam2)
					WinBattle();
				else
					LoseBattle();
				break;
		}

		//UIに勝敗の表示
		MainUi.IndicationOfVictoryOrDefeat(m_isWin);
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
	/// バトル結果表示
	/// </summary>
	void BattleResultDisplay()
	{
		//プレイヤーアニメーション
		OperatingPlayer.SetAnimation(m_isWin ? PlayerBehaviorStatus.Win : PlayerBehaviorStatus.Defeat);

		//カメラ設定
		CameraSettingOfBattleResultDisplay();

		//UI表示
		MainUi.DisplayResults(m_battleRecords, OperatingNetPlayerSetting.GetPlayerNum());
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
	public void ContinueBattle()
	{
		m_state = GameStatus.RecruitPeople;
		OperatingNetPlayerSetting.CmdNotifyOfIsReady(true);
		OperatingNetPlayerSetting.ChangeDisplayNameOfNameplate();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを辞める
	/// </summary>
	public void LeaveBattle()
	{
		Debug.Log("終了");
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
		m_isHorizontalBecamePositive = false;
		m_isHorizontalBecameNegative = false;
	}
}
