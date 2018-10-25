////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/3～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ジェットウォーター情報
/// </summary>
public struct JetWaterInfo
{
	/// <summary>
	/// タグ
	/// </summary>
	public const string TAG = "JetWater";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ジェットウォーター
/// </summary>
public class MyJetWater : MonoBehaviour
{
	/// <summary>
	/// プレイヤーのスクリプト
	/// </summary>
	[SerializeField]
	MyPlayer Player;
	public MyPlayer PlayerScript
	{
		set { Player = value; }
	}

	/// <summary>
	/// 水発射点のオブジェクト1
	/// </summary>
	[SerializeField]
	GameObject Water1;

	/// <summary>
	/// 水発射点のオブジェクト2
	/// </summary>
	[SerializeField]
	GameObject Water2;

	/// <summary>
	/// 水発射点のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject Water;

	/// <summary>
	/// 水の威力
	/// </summary>
	[SerializeField]
	float m_waterPower;

	/// <summary>
	/// 水の発射時間間隔
	/// </summary>
	[SerializeField]
	float m_firingIntervalTime;

	/// <summary>
	/// 水の発射カウント用
	/// </summary>
	float m_countFiringIntervalTime;

	/// <summary>
	/// 水を発射するかどうか
	/// </summary>
	bool m_isSplasheFire;

	/// <summary>
	/// 右の水しぶきの場所リスト
	/// </summary>
	List<Transform> m_rightSplasheTrans = new List<Transform>();

	/// <summary>
	/// 左の水しぶきの場所リスト
	/// </summary>
	List<Transform> m_leftSplasheTrans = new List<Transform>();

	/// <summary>
	/// 水しぶきの大きさ
	/// </summary>
	Vector3 m_splasheScale;

	/// <summary>
	/// 水しぶきの間の距離
	/// </summary>
	Vector3 m_splasheDistance;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの発生
	/// </summary>	
	void FixedUpdate()
	{
		if (m_isSplasheFire)
		{
			m_countFiringIntervalTime += Time.deltaTime;

			//一定時間毎に水しぶき発射
			if (m_countFiringIntervalTime >= m_firingIntervalTime)
			{
				GameObject w1 = Instantiate(Water, Player.PlayersScript.SplashesTrans);
				w1.transform.position = Water1.transform.position;
				w1.transform.LookAt(Water1.transform.position + Water1.transform.forward);
				w1.GetComponent<Rigidbody>().AddForce(-transform.forward * m_waterPower);
				m_rightSplasheTrans.Add(w1.transform);

				GameObject w2 = Instantiate(Water, Player.PlayersScript.SplashesTrans);
				w2.transform.position = Water2.transform.position;
				w2.transform.LookAt(Water2.transform.position + Water2.transform.forward);
				w2.GetComponent<Rigidbody>().AddForce(-transform.forward * m_waterPower);
				m_leftSplasheTrans.Add(w2.transform);

				m_countFiringIntervalTime = 0;
			}
		}

		//水しぶきの向き変更
		ChangeSplasheDirection();
		//水しぶきのサイズ変更
		ChangeSplasheScale();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ジェットの起動、停止
	/// </summary>
	/// <param name="isFire">ジェットの発射</param>
	public void JetFire(bool isFire)
	{
		Water1.SetActive(isFire);
		Water2.SetActive(isFire);
		m_isSplasheFire = isFire;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの向きの変更
	/// </summary>
	void ChangeSplasheDirection()
	{
		//右の水しぶきがあるときに実行
		if (m_rightSplasheTrans.Count != 0)
		{
			//存在しない右の水しぶきの削除
			for (var i=0;i<m_rightSplasheTrans.Count;)
			{
				if (m_rightSplasheTrans[i] == null)
				{
					m_rightSplasheTrans.Remove(m_rightSplasheTrans[i]);
				}
				else
				{
					i++;
				}
			}

			//右の水しぶきの向きの変更
			for (var i = 1; i < m_rightSplasheTrans.Count; i++)
			{
					m_rightSplasheTrans[i].LookAt(m_rightSplasheTrans[i].position +
						(m_rightSplasheTrans[i].position - m_rightSplasheTrans[i - 1].position));			
			}
		}

		//左の水しぶきがあるときに実行
		if (m_leftSplasheTrans.Count != 0)
		{
			//存在しない左の水しぶきの削除
			for (var i = 0; i < m_leftSplasheTrans.Count;)
			{
				if (m_leftSplasheTrans[i] == null)
				{
					m_leftSplasheTrans.Remove(m_leftSplasheTrans[i]);
				}
				else
				{
					i++;
				}
			}

			//左の水しぶきの向きの変更
			for (var i = 1; i < m_leftSplasheTrans.Count; i++)
			{
				m_leftSplasheTrans[i].LookAt(m_leftSplasheTrans[i].position +
					(m_leftSplasheTrans[i].position - m_leftSplasheTrans[i - 1].position));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきのサイズ変更
	/// </summary>
	void ChangeSplasheScale()
	{
		//右の水しぶき
		for (var i = 1; i < m_rightSplasheTrans.Count; i++)
		{
			m_splasheScale = m_rightSplasheTrans[i].localScale;
			m_splasheScale.z = Vector3.Distance(m_rightSplasheTrans[i].position, m_rightSplasheTrans[i - 1].position);
			m_rightSplasheTrans[i].localScale = m_splasheScale;
		}

		//左の水しぶき
		for (var i = 1; i < m_leftSplasheTrans.Count; i++)
		{
			m_splasheScale = m_leftSplasheTrans[i].localScale;
			m_splasheScale.z = Vector3.Distance(m_leftSplasheTrans[i].position, m_leftSplasheTrans[i - 1].position);
			m_leftSplasheTrans[i].localScale = m_splasheScale;
		}
	}
}
