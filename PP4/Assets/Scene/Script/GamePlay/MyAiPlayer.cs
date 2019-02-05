////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/1/27～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIプレイヤー
/// </summary>
public class MyAiPlayer : MyPlayer
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 空間把握
	/// </summary>
	[SerializeField]
	MySpaceGrasp SpaceGrasp;

	/// <summary>
	/// MyField
	/// </summary>
	MyField MyFieldScript;
	#endregion

	#region AIの設定
	/// <summary>
	/// AIの数
	/// </summary>
	static int s_aiCount;

	/// <summary>
	/// AIプレイヤーの最大人数
	/// </summary>
	const int MAX_AI_PLAYER_NUM = 3;

	/// <summary>
	/// AIの強さの段階(0,1,2)
	/// </summary>
	int AILevel;

	/// <summary>
	/// AIの強さによる倍率
	/// </summary>
	[SerializeField]
	float[] LevelMagni;
	#endregion

	#region 能力
	[Header("能力")]
	/// <summary>
	/// 衝突の危険を感じる距離
	/// </summary>
	[SerializeField]
	float m_distanceAtWhichRiskOfCollisionIsFelt;

	/// <summary>
	/// レベルを決めるパワーの段階
	/// </summary>
	struct Power
	{
		public const int LEVEL_ONE = 3000;
		public const int LEVEL_TWO = 5000;
		public const int LEVEL_THREE = 7000;
	}

	/// <summary>
	/// 行動可能半径
	/// </summary>
	[SerializeField]
	float m_behaviorableRadius;

	/// <summary>
	/// ボタンを連射する間隔時間
	/// </summary>
	[SerializeField]
	float m_timeIntervalOfButtonFire;

	/// <summary>
	/// ボタン連打間隔時間を数える
	/// </summary>
	float m_countTimeInterValOfButtonFire;
	#endregion

	#region オーラ
	[Header("オーラ関連")]
	/// <summary>
	/// 温熱オーラが投げたい
	/// </summary>
	bool m_wantToThrowHeatAura;
	public bool WantToThrowHeatAura
	{
		get { return m_wantToThrowHeatAura; }
	}

	/// <summary>
	/// 弾性オーラが投げたい
	/// </summary>
	bool m_wantToThrowElasticityAura;
	public bool WantToThrowElasticityAura
	{
		get { return m_wantToThrowElasticityAura; }
	}

	/// <summary>
	/// 電気オーラが投げたい
	/// </summary>
	bool m_wantToThrowElectricalAura;
	public bool WantToThrowElectricalAura
	{
		get { return m_wantToThrowElectricalAura; }
	}

	/// <summary>
	/// 投げるオーラを決める数値
	/// </summary>
	float m_chooseThrowAuraNum;

	/// <summary>
	/// オーラの使用状態(初期値はUsed)
	/// </summary>
	[SerializeField]
	ActionState AuraState;

	/// <summary>
	/// この数値以上ならオーラを使える(1f)
	/// </summary>
	[SerializeField]
	float m_auraUseLimit;

	/// <summary>
	/// オーラの使用間隔(13f)
	/// </summary>
	[SerializeField]
	float m_auraIntervalTime;

	/// <summary>
	/// オーラの待機時間
	/// </summary>
	float m_auraWaitingTime;
	#endregion

	[Header("加速関連")]
	#region
	/// <summary>
	/// 加速の使用状態
	/// </summary>
	[SerializeField]
	ActionState SpState;

	/// <summary>
	/// 加速の使用間隔(5f)
	/// </summary>
	[SerializeField]
	float m_spIntervalTime;

	/// <summary>
	/// 加速使用後の経過時間
	/// </summary>
	float m_spWaitingTime;

	/// <summary>
	/// この数値までゲージを加速に使える(0.3f)
	/// </summary>
	[SerializeField]
	float m_spUseLimit;
	#endregion

	[Header("コイン関連")]
	#region　コイン
	/// <summary>
	/// 狙うコインの場所
	/// </summary>
	[SerializeField]
	Vector3 m_targetCoin;

	/// <summary>
	/// 狙うコインのゲームオブジェクト
	/// </summary>
	[SerializeField]
	GameObject m_targetCoinObject;

	/// <summary>
	/// 前のコインの所持数
	/// </summary>
	[SerializeField]
	int m_beforeCoinNum;

	/// <summary>
	/// コインの取得手順
	/// </summary>
	public enum GetCoinAction
	{
		/// <summary>
		/// 探す
		/// </summary>
		Search,
		/// <summary>
		/// 近づく
		/// </summary>
		Approach
	}

	/// <summary>
	/// コインを取る動作の状態
	/// </summary>
	[SerializeField]
	GetCoinAction CoinAction;

	/// <summary>
	/// プレイヤーとコインとの距離
	/// </summary>
	float playerToCoinDistanceY;

	/// <summary>
	/// コインを見失う距離
	/// </summary>
	const float SEARCH_LIMIT_DISTANCE = 3;

	/// <summary>
	/// コインを取りに行くときのプレイヤーの速度
	/// </summary>
	const float PLAYER_STEP = 10f;

	/// <summary>
	/// プレイヤーの当たり判定の高さ
	/// </summary>
	const float PLAYER_HEIGHT = 1.5f;
	#endregion

	/// <summary>
	/// 投げるオーラを決める判断の数値
	/// </summary>
	struct RandomThrowAuraNum
	{
		public const float HEAT_AURA = 0.33f;
		public const float ELASTICITY_AURA = 0.66f;
		public const float ELECTRICAL_AURA = 1.0f;
	}

	/// <summary>
	/// 行動の状態
	/// </summary>
	public enum ActionState
	{
		/// <summary>
		/// 使用可能状態
		/// </summary>
		Waiting,
		/// <summary>
		/// 使用中
		/// </summary>
		Using,
		/// <summary>
		/// 使用後
		/// </summary>
		Used
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期設定
	/// </summary>
	void Start()
	{
		s_aiCount++;
		if (s_aiCount > MAX_AI_PLAYER_NUM)
		{
			s_aiCount = MAX_AI_PLAYER_NUM;
		}

		switch (s_aiCount)
		{
			case 1:
				AILevel = 1;
				break;
			case 2:
				AILevel = 2;
				break;
			case 3:
				AILevel = 0;
				break;
		}

		//レベルによるステータス変更
		m_timeIntervalOfButtonFire *= LevelMagni[AILevel];
		m_spIntervalTime *= LevelMagni[AILevel];
		m_auraIntervalTime *= LevelMagni[AILevel];
		m_spUseLimit *= LevelMagni[AILevel];
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		m_horizontalValue = 0;
		m_verticalValue = 0;

		//行動を考える
		ThinkAction();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 行動を考える
	/// </summary>
	void ThinkAction()
	{
		//Debug.Log("下の変数を使ってAIを作ってください");
		//m_isKeepPressingAButton = false;
		//m_isKeepPressingBButton = false;
		//m_isKeepPressingXButton = false;
		//m_isKeepPressingLButton = false;
		//m_isKeepPressingRButton = true;
		//m_horizontalValue = 0;
		//m_verticalValue = 0;
		//m_wantToThrowHeatAura = false;
		//m_wantToThrowElasticityAura = false;
		//m_wantToThrowElectricalAura = false;

		//回復
		if (IsFalling)
			ThinkingRecovery();

		//衝突回避
		if (CoinAction == GetCoinAction.Search)
			ThinkAboutCollisionAvoidance();

		//オーラ
		ThinkThrowAuraBall();

		//コイン取得
		ThinkGetCoin();

		//ボタン入力 
		ThinkButtonPress();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ボタン入力を考える
	/// </summary>
	void ThinkButtonPress()
	{
		//コインを狙っていないときは常に上昇
		if (CoinAction == GetCoinAction.Search)
		{
			m_isKeepPressingRButton = true;
			m_isKeepPressingLButton = false;
		}

		else if (CoinAction != GetCoinAction.Search)
		{
			m_isKeepPressingRButton = false;
			m_isKeepPressingLButton = false;
			transform.position = Vector3.MoveTowards(transform.position, m_targetCoin, PLAYER_STEP * Time.deltaTime);
		}

		//加速は条件あり
		ThinkAcceleration();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 加速使用タイミングを考える
	/// </summary>
	void ThinkAcceleration()
	{
		//使用後は
		if (SpState == ActionState.Used)
		{
			if (m_spWaitingTime < m_spIntervalTime)
				m_spWaitingTime += Time.deltaTime;
			else
			{
				SpState = ActionState.Waiting;
				m_spWaitingTime = 0;
			}
		}

		//加速
		if (SpState != ActionState.Used)
		{
			//隕石回避行動を最優先
			if (SpaceGrasp.MeteoriteNear && GetPercentageOfRemainingSpGauge() > m_spUseLimit)
			{
				if (!m_isKeepPressingAButton)
					m_isKeepPressingAButton = true;
				if (SpState != ActionState.Using)
					SpState = ActionState.Using;
				m_spWaitingTime = 0;
				ResetCoinAction();
			}

			//オーラを使っていなければ加速が使える
			else if (AuraState != ActionState.Using)
			{
				//SPゲージが一定数ある時または隕石が近い時に加速
				if (GetPercentageOfRemainingSpGauge() > m_spUseLimit)
				{
					//コインを見つけていない場合
					if (CoinAction == GetCoinAction.Search)
					{
						m_isKeepPressingAButton = true;
						SpState = ActionState.Using;
					}
				}

				//使用後は待機時間発生
				if (SpState == ActionState.Using && GetPercentageOfRemainingSpGauge() <= m_spUseLimit)
				{
					SpState = ActionState.Used;
					m_isKeepPressingAButton = false;
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回復を考える
	/// </summary>
	void ThinkingRecovery()
	{
		m_countTimeInterValOfButtonFire += Time.deltaTime;

		//回復ボタンを押す
		if (m_countTimeInterValOfButtonFire >= m_timeIntervalOfButtonFire)
		{
			m_isPushedRecoveryButton = true;
			m_countTimeInterValOfButtonFire = 0;
		}
		else
			m_isPushedRecoveryButton = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 衝突回避を考える
	/// </summary>
	void ThinkAboutCollisionAvoidance()
	{
		//行動不可半径
		if (Vector3.Scale(transform.position, (Vector3.right + Vector3.forward)).sqrMagnitude > m_behaviorableRadius * m_behaviorableRadius)
		{
			//中央への方向
			m_workVector3 = -transform.position;

			m_horizontalValue = Vector3.Dot(Camera.main.transform.right, m_workVector3);
			m_verticalValue = Vector3.Dot(Camera.main.transform.forward, m_workVector3);
		}

		//プレイヤー
		foreach (var player in SpaceGrasp.Players)
		{
			//プレイヤーへの逆方向
			m_workVector3 = transform.position - player.transform.position;

			//衝突回避距離
			if (m_workVector3.sqrMagnitude < m_distanceAtWhichRiskOfCollisionIsFelt * m_distanceAtWhichRiskOfCollisionIsFelt)
			{
				m_horizontalValue = Vector3.Dot(Camera.main.transform.right, m_workVector3);
				m_verticalValue = Vector3.Dot(Camera.main.transform.forward, m_workVector3);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// オーラを投げることを考える
	/// </summary>
	void ThinkThrowAuraBall()
	{
		//使用後
		if (AuraState == ActionState.Used)
		{
			if (m_auraWaitingTime < m_auraIntervalTime)
				m_auraWaitingTime += Time.deltaTime;
			else
			{
				AuraState = ActionState.Waiting;
				m_auraWaitingTime = 0;
			}
		}

		//投げた後は投げたい情報リセット(自分が一位の場合は抜かさせるまでゲージを使わない)
		if (AuraState == ActionState.Using && GetPercentageOfRemainingSpGauge() < m_spUseLimit)
		{
			if (m_wantToThrowHeatAura)
				m_wantToThrowHeatAura = false;
			if (m_wantToThrowElasticityAura)
				m_wantToThrowElasticityAura = false;
			if (m_wantToThrowElectricalAura)
				m_wantToThrowElectricalAura = false;
			AuraState = ActionState.Used;
		}

		//オーラが投げられるとき
		if (AuraState == ActionState.Waiting && GetPercentageOfRemainingSpGauge() >= m_auraUseLimit)
		{
			m_chooseThrowAuraNum = transform.position.y % 1;
			if (m_chooseThrowAuraNum <= RandomThrowAuraNum.HEAT_AURA)
			{
				m_wantToThrowHeatAura = true;
			}
			else if (m_chooseThrowAuraNum <= RandomThrowAuraNum.ELASTICITY_AURA)
			{
				m_wantToThrowElasticityAura = true;
			}
			else
			{
				m_wantToThrowElectricalAura = true;
			}
			AuraState = ActionState.Using;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コインの取得を考える
	/// </summary>
	void ThinkGetCoin()
	{
		foreach (var coins in SpaceGrasp.Coins)
		{
			//加速を使っていないかつコイン探索中
			if (CoinAction == GetCoinAction.Search && SpState != ActionState.Using)
			{
				//コインを見つける
				if (coins.activeSelf == true)
				{
					CoinAction = GetCoinAction.Approach;
					m_targetCoinObject = coins;
					m_targetCoin = coins.transform.position;
					m_beforeCoinNum = NumOfCoins;
				}
			}
		}

		if (CoinAction != GetCoinAction.Search)
		{
			playerToCoinDistanceY = transform.position.y - m_targetCoin.y;
		}

		if (CoinAction == GetCoinAction.Approach)
		{
			if (m_targetCoinObject.activeSelf == true)
			{
				//距離が3より大きい
				if (m_beforeCoinNum != NumOfCoins || Mathf.Abs(playerToCoinDistanceY) > SEARCH_LIMIT_DISTANCE)
				{
					ResetCoinAction();
				}
			}
			else
			{
				//ターゲットが消えた
				ResetCoinAction();
			}
		}
	}

	void ResetCoinAction()
	{
		m_targetCoin = Vector3.zero;
		CoinAction = GetCoinAction.Search;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 風の発生
	/// </summary>
	protected override void GenerateWind()
	{
		return;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 消えるときに実行される
	/// </summary>
	private void OnDestroy()
	{
		if (s_aiCount != 0)
			s_aiCount = 0;
	}
}
