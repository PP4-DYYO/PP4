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
public class MyDestroySpreadSplashe : MonoBehaviour {

	float m_time;
	
	void Update () {
		m_time += Time.deltaTime;
		if (m_time > 0.5f)
		{
			Destroy(gameObject);
		}
	}
}
