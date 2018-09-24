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
	/// ジェットの名前
	/// </summary>
	const string JET_NAME = "Jet";

	/// <summary>
	/// ステージのタグ
	/// </summary>
	const string GROUND_TAG = "Ground";

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート
	/// </summary>	
	void Start()
	{
		//this.transform.parent =GameObject.Find(JET_NAME).transform;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void FixedUpdate()
	{
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
				Destroy(gameObject);
			}
			else
			{
				transform.localScale = new Vector3(transform.localScale.x,
					 transform.localScale.y, transform.localScale.z - m_splasheSizeChange);
			}
		}

		if (m_posPrev != Vector3.zero)
		{
			transform.LookAt(transform.position + (m_pos - m_posPrev));

			if (GetComponentInChildren<MeshRenderer>().enabled == false)
			{
				GetComponentInChildren<MeshRenderer>().enabled = true;
			}
		}
		m_splasheLivingTime += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == GROUND_TAG)
		{
			Destroy(gameObject);
		}
	}
}
