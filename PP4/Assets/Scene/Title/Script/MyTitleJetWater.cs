////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/12/4～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTitleJetWater : MonoBehaviour
{
	/// <summary>
	/// プレイヤーのスクリプト
	/// </summary>
	[SerializeField]
	GameObject Player;

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
	/// 水を発射するかどうか
	/// </summary>
	bool m_isSplasheFire;

	/// <summary>
	/// 索引
	/// </summary>
	int m_index;

	/// <summary>
	/// 索引
	/// </summary>
	int m_splasheIndex;

	/// <summary>
	/// 水しぶきの泡オブジェクトクラス
	/// </summary>
	[SerializeField]
	MySpreadSplashe SpreadSplashe;

	/// <summary>
	/// ジェットが水面に当たっている時間
	/// </summary>
	float m_jetStayTime;

	/// <summary>
	/// ジェットが水しぶきを出す周期
	/// </summary>
	[SerializeField]
	float m_jetMakeSpreadTime;

	/// <summary>
	/// 水しぶきの出現場所
	/// </summary>
	Vector3 m_splashePosition;

	/// <summary>
	/// ジェットの出す水しぶきの泡の高さ
	/// </summary>
	const float JET_SPREAD_SPLASHE_HEIGHT = 0;

	/// <summary>
	/// ジェットの出す水しぶきの位置用ゲームオブジェクト(1段階目)
	/// </summary>
	[SerializeField]
	GameObject Effectpos;

	/// <summary>
	/// ジェットの出す水しぶきの位置用ゲームオブジェクト(2段階目)
	/// </summary>
	[SerializeField]
	GameObject Effectpos2;

	/// <summary>
	/// 水しぶきの位置が変わる高さ(1段階目)
	/// </summary>
	const float PLAYER_HEIGHT_ONE = 1.5f;

	/// <summary>
	/// 水しぶきの位置が変わる高さ(2段階目)
	/// </summary>
	const float PLAYER_HEIGHT_TWO = 3.5f;

	/// <summary>
	/// 水しぶきの位置が変わる高さは(3段階目)
	/// </summary>
	const float PLAYER_HEIGHT_THREE = 5.5f;

	/// <summary>
	/// 水しぶきの親用ゲームオブジェクト
	/// </summary>
	[SerializeField]
	GameObject SplashesObject;

	/// <summary>
	/// 水しぶきのSEの再生状態
	/// </summary>
	bool m_playJetSE;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの音の再生
	/// </summary>	
	void FixedUpdate()
	{

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
	/// ジェットの当たり判定
	/// </summary>
	void OnTriggerStay(Collider other)
	{
		if (other.tag == StageInfo.GROUND_TAG && m_isSplasheFire)
		{
			m_jetStayTime += Time.deltaTime;
			if (m_jetStayTime > m_jetMakeSpreadTime)
			{
				MySpreadSplashe ss = Instantiate(SpreadSplashe, transform.parent);
				m_splashePosition.y = JET_SPREAD_SPLASHE_HEIGHT;
				//プレイヤーの高さで水しぶきの泡の出現場所を変える
				if (Player.transform.position.y > PLAYER_HEIGHT_THREE)
				{
					m_splashePosition.x = JetCenter.transform.position.x;
					m_splashePosition.z = JetCenter.transform.position.z;
				}
				else if (Player.transform.position.y > PLAYER_HEIGHT_TWO)
				{
					m_splashePosition.x = Effectpos2.transform.position.x;
					m_splashePosition.z = Effectpos2.transform.position.z;
				}
				else if (Player.transform.position.y > PLAYER_HEIGHT_ONE)
				{
					m_splashePosition.x = Effectpos.transform.position.x;
					m_splashePosition.z = Effectpos.transform.position.z;
				}
				else
				{
					m_splashePosition.x = transform.position.x;
					m_splashePosition.z = transform.position.z;
				}
				ss.transform.position = m_splashePosition;
				m_jetStayTime = 0;
			}
		}
	}
}
