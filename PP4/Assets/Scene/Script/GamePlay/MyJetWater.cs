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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの発生
	/// </summary>	
	void FixedUpdate()
	{
		m_countFiringIntervalTime += Time.deltaTime;

		if (m_countFiringIntervalTime >= m_firingIntervalTime)
		{
			GameObject w1 = Instantiate(Water, Player.PlayersScript.SplashesTrans);
			w1.transform.position = Water1.transform.position;
			w1.transform.LookAt(Water1.transform.position + Water1.transform.forward);
			w1.GetComponent<Rigidbody>().AddForce(-transform.forward * m_waterPower);

			GameObject w2 = Instantiate(Water, Player.PlayersScript.SplashesTrans);
			w2.transform.position = Water2.transform.position;
			w2.transform.LookAt(Water2.transform.position + Water2.transform.forward);
			w2.GetComponent<Rigidbody>().AddForce(-transform.forward * m_waterPower);

			m_countFiringIntervalTime = 0;
		}
	}
}
