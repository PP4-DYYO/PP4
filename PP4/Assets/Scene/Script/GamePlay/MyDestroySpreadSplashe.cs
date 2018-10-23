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

	/// <summary>
	/// 水しぶきの泡のパーティクル
	/// </summary>
	[SerializeField]
	GameObject particle1;

	/// <summary>
	/// 水しぶきの泡のパーティクル
	/// </summary>
	[SerializeField]
	GameObject particle2;

	/// <summary>
	/// 水しぶきの泡を小さくする変数
	/// </summary>
	[SerializeField]
	float m_smallerNum;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 一定時間経過後またはサイズが０で消滅する
	/// </summary>
	void Update()
	{
		m_time += Time.deltaTime;
		if (m_time > m_limitTime)
		{
			Destroy(gameObject);
		}

		if(particle1!=null&& particle2 != null)
		{
			if (particle1.transform.localScale.x - m_smallerNum <= 0)
			{
				Destroy(gameObject);
			}
			else
			{
				//大きさの変更
				particle1.transform.localScale = new Vector3(particle1.transform.localScale.x - m_smallerNum,
					particle1.transform.localScale.y - m_smallerNum,
					particle1.transform.localScale.z - m_smallerNum);

				particle2.transform.localScale = new Vector3(particle2.transform.localScale.x - m_smallerNum,
					particle2.transform.localScale.y - m_smallerNum,
					particle2.transform.localScale.z - m_smallerNum);
			}
		}
	}
}
