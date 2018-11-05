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
	/// 消失する部位
	/// </summary>
	[SerializeField]
	GameObject[] DisappearingParts;

	/// <summary>
	/// 姿を消すフラグ
	/// </summary>
	bool m_isDisappear;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//姿を消す
		foreach(var part in DisappearingParts)
		{
			part.SetActive(!m_isDisappear);
		}

		m_isDisappear = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerStay(Collider other)
	{
		//プレイヤーと重なった
		if(other.tag.Equals(PlayerInfo.TAG))
		{
			m_isDisappear = true;
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
			//消失する部位の復元
			foreach (var part in DisappearingParts)
			{
				part.SetActive(true);
			}
		}
	}
}
