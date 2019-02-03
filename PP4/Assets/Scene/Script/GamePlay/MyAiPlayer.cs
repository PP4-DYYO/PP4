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

	#region 能力
	[Header("能力")]
	/// <summary>
	/// 衝突の危険を感じる距離
	/// </summary>
	[SerializeField]
	float m_distanceAtWhichRiskOfCollisionIsFelt;

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
	/// オーラを投げるための乱数生成用
	/// </summary>
	int m_randomThrow;

	/// <summary>
	/// 乱数生成用の上限の数字
	/// </summary>
	const int UPPER_LIMIT_NUM = 101;

	/// <summary>
	/// オーラのの使用状態
	/// </summary>
	ActionState m_auraState;

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

	/// <summary>
	/// 加速の使用状態
	/// </summary>
	ActionState m_SpState;

	/// <summary>
	/// 加速の使用間隔(10f)
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

	/// <summary>
	/// 投げるオーラの種類
	/// </summary>
	public enum RandomThrowAuraNum
	{
		HeatAura,
		ElasticityAura,
		ElectricalAura
	}

	/// <summary>
	/// 使用状態
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
		ThinkAboutCollisionAvoidance();

		//オーラ
		ThinkThrowAuraBall();

		//ボタン入力 
		ThinkButtonPress();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ボタン入力を考える
	/// </summary>
	void ThinkButtonPress()
	{
		//常に上昇ボタンを入力する 
		m_isKeepPressingRButton = true;

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
		if (m_SpState == ActionState.Used)
		{
			if (m_spWaitingTime < m_spIntervalTime)
				m_spWaitingTime += Time.deltaTime;
			else
			{
				m_SpState = ActionState.Waiting;
				m_spWaitingTime = 0;
			}
		}

		//加速が使えるとき
		if (m_SpState == ActionState.Waiting || m_SpState == ActionState.Using)
		{
			//SPゲージが一定数ある時または隕石が近い時に加速
			if (GetPercentageOfRemainingSpGauge() >= m_spUseLimit || SpaceGrasp.MeteoriteNear)
			{
				m_isKeepPressingAButton = true;
				m_SpState = ActionState.Using;
			}
			//使用後は待機時間発生
			else
			{
				if (m_SpState == ActionState.Using)
					m_SpState = ActionState.Used;
				m_isKeepPressingAButton = false;
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
		if (m_auraState == ActionState.Used)
		{
			if (m_auraWaitingTime < m_auraIntervalTime)
				m_auraWaitingTime += Time.deltaTime;
			else
			{
				m_auraState = ActionState.Waiting;
				m_auraWaitingTime = 0;
			}
		}

		//投げた後は投げたい情報リセット
		if (m_auraState == ActionState.Using)
		{
			if (m_wantToThrowHeatAura)
				m_wantToThrowHeatAura = false;
			if (m_wantToThrowElasticityAura)
				m_wantToThrowElasticityAura = false;
			if (m_wantToThrowElectricalAura)
				m_wantToThrowElectricalAura = false;
			m_auraState = ActionState.Used;
		}

		//オーラが投げられるとき
		if (m_auraState == ActionState.Waiting && GetPercentageOfRemainingSpGauge() >= m_auraUseLimit)
		{
			m_randomThrow = Random.Range(0, UPPER_LIMIT_NUM);
			switch (m_randomThrow)
			{
				case (int)RandomThrowAuraNum.HeatAura:
					m_wantToThrowHeatAura = true;
					break;
				case (int)RandomThrowAuraNum.ElasticityAura:
					m_wantToThrowElasticityAura = true;
					break;
				case (int)RandomThrowAuraNum.ElectricalAura:
					m_wantToThrowElectricalAura = true;
					break;
			}
			if (m_randomThrow <= (int)RandomThrowAuraNum.ElectricalAura)
			{
				m_auraState = ActionState.Using;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 風の発生
	/// </summary>
	protected override void GenerateWind()
	{
		return;
	}
}
