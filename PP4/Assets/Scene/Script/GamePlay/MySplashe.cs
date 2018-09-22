////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/20～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶき
/// </summary>
public class MySplashe : MonoBehaviour
{
	/// <summary>
	/// リジットボディ
	/// </summary>
	[SerializeField]
	Rigidbody rg;

	/// <summary>
	/// 現在の位置
	/// </summary>
	Vector3 m_pos;

	/// <summary>
	/// 前のフレームの位置
	/// </summary>
	Vector3 m_posPrev;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アップデート
	/// </summary>	
	void FixedUpdate()
	{
		m_posPrev = m_pos;
		m_pos = transform.position;

		if (m_posPrev != Vector3.zero)
		{
			transform.LookAt(transform.position + (m_pos - m_posPrev));
		}
	}
}
