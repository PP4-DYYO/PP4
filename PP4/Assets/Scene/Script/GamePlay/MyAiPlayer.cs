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
		//回復
		if (IsFalling)
			ThinkingRecovery();

		//衝突回避
		ThinkAboutCollisionAvoidance();

		Debug.Log("下の変数を使ってAIを作ってください");
		{
			//ボタンを押している 
			//m_isKeepPressingAButton = false;
			//m_isKeepPressingBButton = false;
			//m_isKeepPressingXButton = false;
			//m_isKeepPressingLButton = false;
			m_isKeepPressingRButton = true;
			//m_horizontalValue = 0;
			//m_verticalValue = 0;

			//m_wantToThrowHeatAura = false;
			//m_wantToThrowElasticityAura = false;
			//m_wantToThrowElectricalAura = false;
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
	/// 風の発生
	/// </summary>
	protected override void GenerateWind()
	{
		return;
	}
}
