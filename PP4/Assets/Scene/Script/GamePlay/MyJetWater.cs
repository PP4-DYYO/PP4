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
	/// 水しぶきのクラス
	/// </summary>
	[SerializeField]
	MySplashe Splashe;

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

	/// <summary>
	/// 水しぶきのオブジェクトの配列(100個用意する)
	/// </summary>
	MySplashe[] Splashes = new MySplashe[100];

	/// <summary>
	/// 水しぶきの番号
	/// </summary>
	int m_splasheNum;

	/// <summary>
	/// 索引
	/// </summary>
	int m_index;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>	
	void Start()
	{
		//水しぶきの配列に代入
		for (m_index = 0; m_index < Splashes.Length; m_index++)
		{
			Splashes[m_index] = Instantiate(Splashe);
			Splashes[m_index].ActiveChange(false);
		}
	}

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
				//配列の水しぶきの設定
				Splashes[m_splasheNum].ActiveChange(true);

				//サイズのリセット
				Splashes[m_splasheNum].transform.localScale = Vector3.one;

				//親の設定
				if (!Splashes[m_splasheNum].transform.parent && Player)
				{
					Splashes[m_splasheNum].transform.parent = Player.PlayersScript.SplashesTrans;
				}

				//水しぶきの場所
				Splashes[m_splasheNum].transform.position = JetCenter.transform.position;

				//水しぶきの角度
				Splashes[m_splasheNum].transform.LookAt(JetCenter.transform.position + JetCenter.transform.forward);

				//水しぶきに力を加える
				Splashes[m_splasheNum].AddingForce(-transform.forward * m_waterPower);

				//場所リストの更新
				m_centerSplasheTrans.Add(Splashes[m_splasheNum].transform);

				m_splasheNum ++;
				if (m_splasheNum >= Splashes.Length)
				{
					m_splasheNum = 0;
				}
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
		//水しぶきがあるときに実行
		if (m_centerSplasheTrans.Count != 0)
		{
			//存在しない水しぶきの削除
			for (m_index = 0; m_index < m_centerSplasheTrans.Count;)
			{
				if (m_centerSplasheTrans[m_index] == null)
				{
					m_centerSplasheTrans.Remove(m_centerSplasheTrans[m_index]);
				}
				else
				{
					m_index++;
				}
			}

			//水しぶきの向きの変更
			for (m_index = 1; m_index < m_centerSplasheTrans.Count; m_index++)
			{
				m_centerSplasheTrans[m_index].LookAt(m_centerSplasheTrans[m_index].position +
					(m_centerSplasheTrans[m_index].position - m_centerSplasheTrans[m_index - 1].position));
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
		for (m_index = 1; m_index < m_centerSplasheTrans.Count; m_index++)
		{
			//空中にあればサイズ変更
			if (m_centerSplasheTrans[m_index].transform.position.y>0)
			{
				m_splasheScale = m_centerSplasheTrans[m_index].localScale;
				m_splasheScale.z = Vector3.Distance(m_centerSplasheTrans[m_index].position, m_centerSplasheTrans[m_index - 1].position);
				m_centerSplasheTrans[m_index].localScale = m_splasheScale;
			}
		}
	}
}
