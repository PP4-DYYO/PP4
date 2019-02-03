////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/1/31～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空間把握
/// </summary>
public class MySpaceGrasp : MonoBehaviour
{
	/// <summary>
	/// 対象者
	/// </summary>
	[SerializeField]
	Transform Target;

	/// <summary>
	/// プレイヤー
	/// </summary>
	List<GameObject> m_players = new List<GameObject>();
	public List<GameObject> Players
	{
		get { return m_players; }
	}

	/// <summary>
	/// 水しぶき
	/// </summary>
	List<GameObject> m_splashes = new List<GameObject>();
	public List<GameObject> Splashes
	{
		get { return m_splashes; }
	}

	/// <summary>
	/// 隕石が近くにあるか
	/// </summary>
	bool m_isMeteoriteNear;
	public bool MeteoriteNear
	{
		get { return m_isMeteoriteNear; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 後フレーム
	/// </summary>
	void LateUpdate()
	{
		//対象者から離れる
		if (transform.parent == Target)
			transform.parent = Target.parent;

		//対象者の位置を追跡
		if (Target)
			transform.position = Target.position;
		else
			Destroy(gameObject);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerEnter(Collider other)
	{
		//対象者は除外
		if (other.transform == Target)
			return;

		//プレイヤー
		if (other.tag.Equals(PlayerInfo.TAG))
			m_players.Add(other.gameObject);

		//水しぶき
		if (other.tag.Equals(SplasheInfo.TAG))
			m_splashes.Add(other.gameObject);

		if (other.tag.Equals(StageInfo.METEORITE_TAG))
			m_isMeteoriteNear = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり終わり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerExit(Collider other)
	{
		//プレイヤー
		if (other.tag.Equals(PlayerInfo.TAG))
		{
			//削除
			if (m_players.Contains(other.gameObject))
				m_players.Remove(other.gameObject);
		}

		//水しぶき
		if (other.tag.Equals(SplasheInfo.TAG))
		{
			//削除
			if (m_splashes.Contains(other.gameObject))
				m_splashes.Remove(other.gameObject);
		}

		//隕石
		if (other.tag.Equals(StageInfo.METEORITE_TAG))
			m_isMeteoriteNear = false;
	}
}
