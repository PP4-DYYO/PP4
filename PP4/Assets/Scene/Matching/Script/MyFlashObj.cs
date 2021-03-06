﻿////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/1/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点滅するオブジェクト
/// </summary>
public class MyFlashObj : MonoBehaviour
{
	/// <summary>
	/// 点滅するオブジェクト
	/// </summary>
	[SerializeField]
	GameObject[] FlashObj;

	/// <summary>
	/// 点灯時間
	/// </summary>
	[SerializeField]
	float m_lightningTime;

	/// <summary>
	/// 点灯時間
	/// </summary>
	float m_countLightningTime;

	/// <summary>
	///	アクティブフラグ
	/// </summary>
	bool m_isActive;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		m_countLightningTime += Time.deltaTime;

		//点滅の時間
		if(m_countLightningTime >= m_lightningTime)
		{
			foreach(var obj in FlashObj)
			{
				m_isActive = obj.activeInHierarchy;
				obj.SetActive(!m_isActive);

				//SE
				if (!m_isActive)
					MySoundManager.Instance.Play(SeCollection.SplashedFoam, true, false, obj.transform.position);
			}
			m_countLightningTime = 0;
		}
	}
}
