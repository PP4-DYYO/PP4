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
	/// オーラを投げた
	/// </summary>
	bool m_isAuraThrow;

	/// <summary>
	/// オーラを投げるための乱数生成用
	/// </summary>
	int m_randomThrow;

	/// <summary>
	/// 乱数生成用の上限の数字
	/// </summary>
	const int UPPER_LIMIT_NUM = 101;


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
	/// オーラを投げられる
	/// </summary>
	bool m_ableThrowAura;

	/// <summary>
	/// オーラの待機時間を数えるか
	/// </summary>
	bool m_isAuraTimeCount;

	/// <summary>
	/// この数値を超えていたらオーラを使える(0.5f)
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
	/// 隕石との距離
	/// </summary>
	float m_meteoriteDistance;

	/// <summary>
	/// 隕石のゲームオブジェクト
	/// </summary>
	GameObject Meteorite;

	/// <summary>
	/// 隕石が近くにあるか
	/// </summary>
	bool m_isMeteoriteNear;

	/// <summary>
	/// 隕石を破壊しようとする距離(50f)
	/// </summary>
	[SerializeField]
	float m_breakMeteoriteDistance;

	/// <summary>
	/// 加速を使える
	/// </summary>
	bool m_ableSp;

	/// <summary>
	/// 加速を使った
	/// </summary>
	bool m_isUsedSp;

	/// <summary>
	/// 加速の待機時間を数えるか
	/// </summary>
	bool m_isSpTimeCount;

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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 最初の動作
	/// </summary>
	void Start()
	{
		Meteorite = GameObject.Find("Field").GetComponent<MyField>().MeteoriteObject;
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

		//隕石破壊行動
		ThinkbreakMeteorite();

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
		if (m_isSpTimeCount)
		{
			if (m_spWaitingTime < m_spIntervalTime)
				m_spWaitingTime += Time.deltaTime;
			else
			{
				m_ableSp = true;
				m_isSpTimeCount = false;
				m_spWaitingTime = 0;
			}
		}

		if (!m_isSpTimeCount)
		{
			m_ableSp = true;
		}
		else
		{
			m_ableSp = false;
		}

		//加速が使えるとき
		if (m_ableSp)
		{
			//SPゲージが一定数ある時または隕石が近い時に加速
			if (GetPercentageOfRemainingSpGauge() >= m_spUseLimit || m_isMeteoriteNear)
			{
				m_isKeepPressingAButton = true;
			}
			//SPゲージが一定数を切ると再使用まで待機時間発生
			else
			{
				m_ableSp = false;
				m_isSpTimeCount = true;
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
			m_isPushedRecoveryButton = true;
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
		if (m_isAuraTimeCount)
		{
			if (m_auraWaitingTime < m_auraIntervalTime)
				m_auraWaitingTime += Time.deltaTime;
			else
			{
				m_isAuraTimeCount = false;
				m_auraWaitingTime = 0;
			}
		}

		//投げた後は投げたい情報リセット
		if (m_isAuraThrow)
		{
			m_wantToThrowHeatAura = false;
			m_wantToThrowElasticityAura = false;
			m_wantToThrowElectricalAura = false;
			m_isAuraThrow = false;
		}

		if (GetPercentageOfRemainingSpGauge() > m_auraUseLimit && !m_isAuraTimeCount)
		{
			m_ableThrowAura = true;
		}
		else
		{
			m_ableThrowAura = false;
		}

		//オーラが投げられるとき
		if (m_ableThrowAura)
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
				m_isAuraThrow = true;
				m_isAuraTimeCount = true;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 隕石の破壊を考える
	/// </summary>
	void ThinkbreakMeteorite()
	{
		//隕石との距離
		m_meteoriteDistance = Vector3.Distance(transform.position, Meteorite.transform.position);

		if (m_meteoriteDistance < m_breakMeteoriteDistance)
			m_isMeteoriteNear = true;
		else
			m_isMeteoriteNear = false;
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
