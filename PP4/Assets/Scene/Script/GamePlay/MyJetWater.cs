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
	/// 水発射点のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject JetCenter;

	/// <summary>
	/// 水しぶきのオブジェクト
	/// </summary>
	[SerializeField]
	GameObject Water;

	/// <summary>
	/// 右のジェットのオブジェクト
	/// </summary>
	[SerializeField]
	GameObject RightJet;

	/// <summary>
	/// 左のジェットのオブジェクト
	/// </summary>
	[SerializeField]
	GameObject LeftJet;

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
	/// 水しぶきの場所リスト
	/// </summary>
	List<Transform> m_centerSplasheTrans = new List<Transform>();

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
				GameObject water = Instantiate(Water, Player.PlayersScript.SplashesTrans);
				water.transform.position = JetCenter.transform.position;
				water.transform.LookAt(JetCenter.transform.position + JetCenter.transform.forward);
				water.GetComponent<Rigidbody>().AddForce(-transform.forward * m_waterPower);
				m_centerSplasheTrans.Add(water.transform);

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
		RightJet.SetActive(isFire);
		LeftJet.SetActive(isFire);
		m_isSplasheFire = isFire;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの向きの変更
	/// </summary>
	void ChangeSplasheDirection()
	{
		//右の水しぶきがあるときに実行
		if (m_centerSplasheTrans.Count != 0)
		{
			//存在しない右の水しぶきの削除
			for (var i = 0; i < m_centerSplasheTrans.Count;)
			{
				if (m_centerSplasheTrans[i] == null)
				{
					m_centerSplasheTrans.Remove(m_centerSplasheTrans[i]);
				}
				else
				{
					i++;
				}
			}

			//右の水しぶきの向きの変更
			for (var i = 1; i < m_centerSplasheTrans.Count; i++)
			{
				m_centerSplasheTrans[i].LookAt(m_centerSplasheTrans[i].position +
					(m_centerSplasheTrans[i].position - m_centerSplasheTrans[i - 1].position));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきのサイズ変更
	/// </summary>
	void ChangeSplasheScale()
	{
		//水しぶき
		for (var i = 1; i < m_centerSplasheTrans.Count; i++)
		{
			if (!m_centerSplasheTrans[i].GetComponent<MySplashe>().Fallen)
			{
				m_splasheScale = m_centerSplasheTrans[i].localScale;
				m_splasheScale.z = Vector3.Distance(m_centerSplasheTrans[i].position, m_centerSplasheTrans[i - 1].position);
				m_centerSplasheTrans[i].localScale = m_splasheScale;
			}
		}
	}
}
