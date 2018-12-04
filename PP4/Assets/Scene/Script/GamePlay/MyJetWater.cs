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
	/// 水しぶきの最小サイズ(3)
	/// </summary>
	[SerializeField]
	float m_splasheMinimumSize;

	/// <summary>
	/// 水しぶきのサイズ変更式内の定数(6)
	/// </summary>
	[SerializeField]
	float m_splasheSmollerAmount;

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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>	
	void Start()
	{
		//水しぶきの配列に代入
		for (m_index = 0; m_index < Splashes.Length; m_index++)
		{
			Splashes[m_index] = Instantiate(Splashe, transform);
			Splashes[m_index].SplasheWaterScript.ActiveChange(false);
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

				Splashes[m_splasheNum].SplasheWaterScript.ActiveChange(true);

				//サイズのリセット
				Splashes[m_splasheNum].SplasheWaterScript.transform.localScale = Vector3.one;

				//親の設定
				if (Splashes[m_splasheNum].transform.parent != Player.PlayersScript.SplashesTrans && Player)
				{
					Splashes[m_splasheNum].transform.parent = Player.PlayersScript.SplashesTrans;
					Splashes[m_splasheNum].transform.position = Vector3.zero;
					Splashes[m_splasheNum].transform.rotation = Quaternion.identity;
					Splashes[m_splasheNum].transform.localScale = Vector3.one;
				}

				//水しぶきの場所
				Splashes[m_splasheNum].SplasheWaterScript.transform.position = JetCenter.transform.position;

				//水しぶきの角度
				Splashes[m_splasheNum].SplasheWaterScript.transform.LookAt(JetCenter.transform.position + JetCenter.transform.forward);

				//水しぶきに力を加える
				Splashes[m_splasheNum].SplasheWaterScript.AddingForce(-transform.forward * m_waterPower);

				m_splasheNum++;

				if (m_splasheNum >= Splashes.Length)
				{
					m_splasheNum = 0;
				}
				m_countFiringIntervalTime = 0;
			}
		}
		//水しぶきの操作
		ChangeSplasheMovement();
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
	/// 水しぶきの向きとサイズの変更
	/// </summary>
	void ChangeSplasheMovement()
	{
		//水しぶきの向きの変更
		for (m_index = 0; m_index < Splashes.Length; m_index++)
		{
			if (!Splashes[m_index].SplasheWaterScript.isDisplay)
				continue;
			if (m_index == 0)
			{
				Splashes[m_index].SplasheWaterScript.transform.LookAt(Splashes[m_index].SplasheWaterScript.transform.position +
					(Splashes[m_index].SplasheWaterScript.transform.position - Splashes[Splashes.Length - 1].SplasheWaterScript.transform.position));
			}
			else
			{
				Splashes[m_index].SplasheWaterScript.transform.LookAt(Splashes[m_index].SplasheWaterScript.transform.position +
					(Splashes[m_index].SplasheWaterScript.transform.position - Splashes[m_index - 1].SplasheWaterScript.transform.position));
			}
			//空中にあればサイズ変更
			if (Splashes[m_index].SplasheWaterScript.transform.position.y > 0)
			{
				m_splasheScale = Splashes[m_index].SplasheWaterScript.transform.localScale;

				m_splasheIndex = m_index - m_splasheNum;
				if (m_splasheIndex < 0)
					m_splasheIndex += Splashes.Length;
				m_splasheScale.z = m_splasheMinimumSize + (Splashes.Length - m_splasheIndex) / m_splasheSmollerAmount;

				Splashes[m_index].SplasheWaterScript.transform.localScale = m_splasheScale;
			}
		}
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
				MySpreadSplashe ss = Instantiate(SpreadSplashe, Player.transform);
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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきを全て非表示にする
	/// </summary>
	public void SplasheHideAll()
	{
		for (m_index = 0; m_index < Splashes.Length; m_index++)
		{
			Splashes[m_index].SplasheWaterScript.ActiveChange(false);
		}
	}
}
