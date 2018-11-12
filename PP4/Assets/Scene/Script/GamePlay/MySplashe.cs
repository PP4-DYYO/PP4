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
	/// 自身のRigitbode
	/// </summary>
	[SerializeField]
	Rigidbody rigidbody;


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
	/// 水しぶきの広がりオブジェクト
	/// </summary>
	[SerializeField]
	GameObject spreadSplashe;

	/// <summary>
	/// 水しぶきが地面に落ちたかどうか
	/// </summary>
	[SerializeField]
	bool isfallen;
	public bool Fallen
	{
		get { return isfallen; }
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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Start()
	{
		m_splasheScaleZ = this.transform.localScale.z;	
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
			gameObject.SetActive(false);
			isfallen = false;
			m_splasheLivingTime = 0;
		}

		//サイズが０以下になるときには消す
		if (transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime) < 0)
		{
			gameObject.SetActive(false);
			isfallen = false;
			m_splasheLivingTime = 0;
		}
		else
		{
			//オブジェクトの大きさ変化
			if (isfallen)
			{
				transform.localScale = new Vector3(transform.localScale.x,
					 transform.localScale.y, transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime));
			}
			else
			{
				if (transform.localScale.x - m_splasheXYSizeChange > 0)
				{
					transform.localScale = new Vector3(transform.localScale.x -m_splasheXYSizeChange,
						 transform.localScale.y - m_splasheXYSizeChange, transform.localScale.z + m_splasheSizeChange);
				}
				else
				{
					transform.localScale = new Vector3(transform.localScale.x ,
						 transform.localScale.y, transform.localScale.z + m_splasheSizeChange);
				}
				m_splasheScaleZ = transform.localScale.z;

			}
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
			isfallen = true;
			MakeSpreadSplashe();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの広がりのオブジェクト生成
	/// </summary>
	void MakeSpreadSplashe()
	{
		GameObject ss = Instantiate(spreadSplashe, transform.parent);
		ss.transform.position = transform.position;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身の表示の切り替え
	/// </summary>
	public void ActiveChange(bool choosing)
	{
		gameObject.SetActive(choosing);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきに力を加える
	/// </summary>
	public void AddingForce(Vector3 v)
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(v);
	}
}
