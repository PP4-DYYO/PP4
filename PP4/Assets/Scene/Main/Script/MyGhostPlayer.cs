////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/26～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴーストプレイヤー
/// </summary>
public class MyGhostPlayer : MonoBehaviour
{
	/// <summary>
	/// 体
	/// </summary>
	[SerializeField]
	GameObject Body;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerEnter(Collider other)
	{
		//プレイヤーと重なった
		if(other.tag.Equals(PlayerInfo.TAG))
		{
			Body.SetActive(false);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり終わり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerExit(Collider other)
	{
		//プレイヤーと重なった
		if (other.tag.Equals(PlayerInfo.TAG))
		{
			Body.SetActive(true);
		}
	}
}
