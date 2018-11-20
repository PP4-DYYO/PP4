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
/// 水しぶきの水
/// </summary>
public class MySplasheWater : MonoBehaviour
{
	/// <summary>
	/// 自身のRigitbode
	/// </summary>
	[SerializeField]
	Rigidbody Rb;

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
	/// 水しぶきを大きくする値(Z方向)
	/// </summary>
	[SerializeField]
	float m_splasheSizeChange;

	/// <summary>
	/// 水しぶきを小さくする数値(X,Y方向)
	/// </summary>
	[SerializeField]
	float m_splasheXYSizeChange;

	/// <summary>
	/// 水しぶきが地面に落ちたかどうか
	/// </summary>
	[SerializeField]
	bool m_isfallen;
	public bool Fallen
	{
		get { return m_isfallen; }
	}

	/// <summary>
	/// 自身のZの大きさ
	/// </summary>
	float m_splasheScaleZ;

	/// <summary>
	/// 着地から消えるまでの時間
	/// </summary>
	[SerializeField]
	float m_splasheSmallerTime;

	/// <summary>
	/// 水しぶきの泡オブジェクト
	/// </summary>
	[SerializeField]
	MySpreadSplashe SpreadSplashe;

	/// <summary>
	/// 水しぶきの本体
	/// </summary>
	[SerializeField]
	GameObject Body;
	public GameObject BodyObj
	{
		get { return Body; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Start()
	{
		m_splasheScaleZ = Body.transform.localScale.z;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void FixedUpdate()
	{
		//時間経過で消滅する
		if (m_splasheLivingTime >= m_splasheLifeTime)
		{
			MySplasheDestroy();
		}

		//着地後
		if (m_isfallen)
		{
			//サイズが０以下になるときには消す
			if (Body.transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime) < 0)
			{
				MySplasheDestroy();
			}
			else
			{
				Body.transform.localScale = new Vector3(Body.transform.localScale.x,
						 Body.transform.localScale.y, Body.transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime));
			}
		}
		//空中
		else
		{
			if (Body.transform.localScale.x - m_splasheXYSizeChange > 0)
			{
				Body.transform.localScale = new Vector3(Body.transform.localScale.x - m_splasheXYSizeChange,
					 Body.transform.localScale.y - m_splasheXYSizeChange, Body.transform.localScale.z + m_splasheSizeChange);
			}
			else
			{
				Body.transform.localScale = new Vector3(Body.transform.localScale.x,
					 Body.transform.localScale.y, Body.transform.localScale.z + m_splasheSizeChange);
			}
			m_splasheScaleZ = Body.transform.localScale.z;
		}
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
			m_isfallen = true;
			MakeSpreadSplashe();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの広がりのオブジェクト生成
	/// </summary>
	void MakeSpreadSplashe()
	{
		SpreadSplashe.SplasheEffect.Play();
		SpreadSplashe.SplasheEffect.transform.position = transform.position;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身が消えるときの処理
	/// </summary>
	public void MySplasheDestroy()
	{
		Body.SetActive(false);
		m_isfallen = false;
		m_splasheLivingTime = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身の表示の切り替え
	/// </summary>
	public void ActiveChange(bool choosing)
	{
		Body.SetActive(choosing);
		m_splasheLivingTime = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきに力を加える
	/// </summary>
	public void AddingForce(Vector3 v)
	{
		Rb.velocity = Vector3.zero;
		Rb.AddForce(v);
	}
}
