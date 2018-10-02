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
	/// 現在の位置
	/// </summary>
	Vector3 m_pos;

	/// <summary>
	/// 前のフレームの位置
	/// </summary>
	Vector3 m_posPrev;

	/// <summary>
	/// 水しぶきの残る時間
	/// </summary>
	[SerializeField]
	float m_splasheLifeTime;

	/// <summary>
	/// 水しぶきが発生してからの経過時間
	/// </summary>
	float m_splasheLivingTime;

	/// <summary>
	/// 水しぶきを小さくする数値
	/// </summary>
	[SerializeField]
	float m_splasheSizeChange;

	/// <summary>
	/// 水しぶきの広がりオブジェクト
	/// </summary>
	[SerializeField]
	GameObject spreadSplashe;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void FixedUpdate()
	{
		//時間経過で消滅する
		if (m_splasheLivingTime >= m_splasheLifeTime)
		{
			Destroy(gameObject);
		}

		m_posPrev = m_pos;
		m_pos = transform.position;

		if (transform.localScale.z > 0)
		{
			//サイズが０以下になるときには消す
			if (transform.localScale.z - m_splasheSizeChange < 0)
			{
				GameObject ss = Instantiate(spreadSplashe);
				ss.transform.position = transform.position;
				Destroy(gameObject);
			}
			else
			{
				//オブジェクトが小さくなる
				transform.localScale = new Vector3(transform.localScale.x,
					 transform.localScale.y, transform.localScale.z - m_splasheSizeChange);
			}
		}

		//水しぶきオブジェクトの向きを調整
		if (m_posPrev != Vector3.zero)
			transform.LookAt(transform.position + (m_pos - m_posPrev));

		m_splasheLivingTime += Time.deltaTime;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶき当たり判定
	/// </summary>
	void OnTriggerEnter(Collider other)
	{
		//ステージに衝突時消える
		if (other.tag == StageInfo.GROUND_TAG)
		{
			Destroy(gameObject);
			MakeSpreadSplashe();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの広がりのオブジェクト生成
	/// </summary>
	void MakeSpreadSplashe()
	{
		GameObject ss = Instantiate(spreadSplashe);
		ss.transform.position = transform.position;
	}
}
