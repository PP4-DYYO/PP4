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


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.position += Vector3.up * Time.deltaTime;
	}
}
