////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/1/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動
/// </summary>
public class MyMovement : MonoBehaviour
{
	/// <summary>
	/// 方向
	/// </summary>
	[SerializeField]
	Vector3 m_direction;

	/// <summary>
	/// 速度
	/// </summary>
	[SerializeField]
	float m_speed;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		transform.position += m_direction * (m_speed * Time.deltaTime);
	}
}
