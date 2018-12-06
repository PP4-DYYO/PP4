////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/27～
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
/// マッチング状態
/// </summary>
enum MatchingStatus
{
	/// <summary>
	/// マッチング
	/// </summary>
	Matching,
	/// <summary>
	/// 待機
	/// </summary>
	Wait,
	/// <summary>
	/// 状態の数
	/// </summary>
	Count,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 状態の処理
/// </summary>
delegate void StateProcessing();

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ボタンを押した時の処理
/// </summary>
public delegate void ProcessingWhenButtonIsPressed();

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// マッチング
/// </summary>
public class MyMatching : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// マッチングUI
	/// </summary>
	[SerializeField]
	MyMatchingUi MatchingUi;

	/// <summary>
	/// ネットワークマネージャ
	/// </summary>
	MyNetworkManager m_netManager;
	#endregion

	#region 状態
	[Header("状態")]
	/// <summary>
	/// 状態
	/// </summary>
	MatchingStatus m_state;

	/// <summary>
	/// フレーム前の状態
	/// </summary>
	MatchingStatus m_statePrev = MatchingStatus.Count;

	/// <summary>
	/// 状態の処理
	/// </summary>
	StateProcessing[] StateProcess = new StateProcessing[(int)MatchingStatus.Count];
	#endregion

	#region マッチング状態
	[Header("マッチング状態")]
	/// <summary>
	/// 選択中の番号
	/// </summary>
	int m_selectedNum;
	#endregion

	#region キーボード関係
	[Header("キーボード関係")]
	/// <summary>
	/// Aボタンを押した
	/// </summary>
	bool m_isAButtonDown;

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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		//状態の処理
		StateProcess[(int)MatchingStatus.Matching] = MatchingProcess;
		StateProcess[(int)MatchingStatus.Wait] = WaitProcess;

		//マッチング処理の送付
		MatchingUi.ToRematchProcess = ToRematch;
		MatchingUi.FinishMatchingProcess = FinishMatching;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_netManager = FindObjectOfType<MyNetworkManager>();
		m_netManager.IsStandbyState = false;

		//BGM
		MySoundManager.Instance.Play(BgmCollection.Matching);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		//入力
		InputProcess();

		//処理
		StateProcess[(int)m_state]();

		//入力のリセット
		ResetInput();
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
	/// マッチング処理
	/// </summary>
	void MatchingProcess()
	{
		//初期状態
		if (m_state != m_statePrev)
		{
			//ネットワークマネージャを動かす
			m_netManager.IsStandbyState = false;
			m_statePrev = m_state;
		}

		//サーバと接続が切れた
		if (m_netManager.IsConnectionWithServerIsBroken)
		{
			//待機状態に
			m_state = MatchingStatus.Wait;
			m_netManager.IsStandbyState = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 待機処理
	/// </summary>
	void WaitProcess()
	{
		//初期状態
		if (m_state != m_statePrev)
		{
			//再マッチングの確認
			MatchingUi.StartConfirmRematch();
			m_state = m_statePrev;
		}

		//横方向の入力
		if(m_isDpadXBecamePositive || m_isDpadXBecameNegative || m_isHorizontalBecamePositive || m_isHorizontalBecameNegative)
		{
			//選択番号
			switch (m_selectedNum)
			{
				case 0:
					m_selectedNum = 1;
					MatchingUi.SelectFinishMatchingButton();
					break;
				case 1:
					m_selectedNum = 0;
					MatchingUi.SelectRematchButton();
					break;
			}
		}

		//Aボタンの入力
		if (m_isAButtonDown)
		{
			//選択番号
			switch(m_selectedNum)
			{
				case 0:
					ToRematch();
					break;
				case 1:
					FinishMatching();
					break;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再マッチングする
	/// </summary>
	void ToRematch()
	{
		m_state = MatchingStatus.Matching;
		m_netManager.IsStandbyState = false;
		m_netManager.IsConnectionWithServerIsBroken = false;
		MatchingUi.HideRematch();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マッチングの終了
	/// </summary>
	void FinishMatching()
	{
		//初期状態
		m_state = MatchingStatus.Matching;
		m_netManager.IsStandbyState = true;
		m_netManager.IsConnectionWithServerIsBroken = false;
		MatchingUi.HideRematch();

		MySceneManager.Instance.ChangeScene(MyScene.Armed);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力のリセット
	/// </summary>
	void ResetInput()
	{
		m_isAButtonDown = false;
		m_isDpadXBecamePositive = false;
		m_isDpadXBecameNegative = false;
		m_isHorizontalBecamePositive = false;
		m_isHorizontalBecameNegative = false;
	}
}
