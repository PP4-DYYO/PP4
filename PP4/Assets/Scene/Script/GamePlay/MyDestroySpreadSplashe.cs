////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/30～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶきの広がりの自動消滅
/// </summary>
public class MyDestroySpreadSplashe : MonoBehaviour
{
	/// <summary>
	/// 時間計測用
	/// </summary>
	float m_time;

	/// <summary>
	/// 自動消滅までの時間(0.5f)
	/// </summary>
	[SerializeField]
	float m_limitTime;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 一定時間経過後、消滅する
	/// </summary>
	void Update()
	{
		m_time += Time.deltaTime;
		if (m_time > m_limitTime)
		{
			Destroy(gameObject);
		}
	}
}
