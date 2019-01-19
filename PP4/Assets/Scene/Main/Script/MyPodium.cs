////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/1/19～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表彰台
/// </summary>
public class MyPodium : MonoBehaviour
{
	/// <summary>
	/// 相対的な乗る位置
	/// </summary>
	[SerializeField]
	Vector3 m_relativeRidingPos;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり続ける判定
	/// </summary>
	/// <param name="other">当たったもの</param>
	void OnCollisionStay(Collision other)
	{
		//プレイヤー以外
		if (!other.transform.tag.Equals(PlayerInfo.TAG))
			return;

		//乗る位置
		other.transform.position = transform.position + m_relativeRidingPos;
	}
}
