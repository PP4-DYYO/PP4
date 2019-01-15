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
/// 水しぶき情報
/// </summary>
public struct SplasheInfo
{
	/// <summary>
	/// タグ
	/// </summary>
	public const string TAG = "Splashe";

	/// <summary>
	/// プレイヤーと当たらない水しぶきのタグ
	/// </summary>
	public const string TRANS_TAG = "TransSplashe";
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶきの水
/// </summary>
public class MySplasheWater : MonoBehaviour
{
	/// <summary>
	/// 自身のRigidbody
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
	/// 水しぶきの本体
	/// </summary>
	[SerializeField]
	GameObject Body;
	public GameObject BodyObj
	{
		get { return Body; }
	}

	/// <summary>
	/// 水しぶきのbodyの変更用
	/// </summary>
	Vector3 m_changedBody;

	/// <summary>
	/// 自身の表示状態
	/// </summary>
	bool m_isDisplay;
	public bool isDisplay
	{
		get { return m_isDisplay; }
	}

	/// <summary>
	/// 生成されてからプレイヤーと接触しないフレーム数
	/// </summary>
	[SerializeField]
	int m_nonTouchFrame;

	/// <summary>
	/// 生成されてからのフレーム数
	/// </summary>
	int m_SplasheWaterFrame;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Start()
	{
		m_splasheScaleZ = Body.transform.localScale.z;
		tag = SplasheInfo.TAG;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Update()
	{
		if (m_isDisplay)
		{
			//時間経過で消滅する
			if (m_splasheLivingTime >= m_splasheLifeTime)
			{
				MySplasheDestroy();
			}

			//タグの調整
			TagControl();

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
					m_changedBody.x = Body.transform.localScale.x;
					m_changedBody.y = Body.transform.localScale.y;
					m_changedBody.z = Body.transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime);

					Body.transform.localScale = m_changedBody;
				}
			}
			//空中
			else
			{
				//x、yがまだ小さくなれるとき
				if (Body.transform.localScale.x - m_splasheXYSizeChange > 0)
				{
					//x、yは小さく、zは大きく変更される
					m_changedBody.x = Body.transform.localScale.x - m_splasheXYSizeChange;
					m_changedBody.y = Body.transform.localScale.y - m_splasheXYSizeChange;
					m_changedBody.z = Body.transform.localScale.z + m_splasheSizeChange;
					Body.transform.localScale = m_changedBody;
				}
				else
				{
					//x、yは同じサイズのまま、zだけ大きく変更される
					m_changedBody.x = Body.transform.localScale.x;
					m_changedBody.y = Body.transform.localScale.y;
					m_changedBody.z = Body.transform.localScale.z + m_splasheSizeChange;
					Body.transform.localScale = m_changedBody;
				}
				m_splasheScaleZ = Body.transform.localScale.z;
			}
			m_splasheLivingTime += Time.deltaTime;
		}
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
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身が消えるときの処理
	/// </summary>
	public void MySplasheDestroy()
	{
		MySoundManager.Instance.Play(SeCollection.SplashFallsOnSurfaceOfWater, true, false, Body.transform.position);
		Body.SetActive(false);
		m_isfallen = false;
		m_isDisplay = false;
		m_splasheLivingTime = 0;
		m_SplasheWaterFrame = 0;
		tag = tag = SplasheInfo.TAG;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身の表示の切り替え
	/// </summary>
	public void ActiveChange(bool choosing)
	{
		Body.SetActive(choosing);
		m_isDisplay = choosing;
		m_splasheLivingTime = 0;
		m_SplasheWaterFrame = 0;
		tag = tag = SplasheInfo.TAG;
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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タグの調整
	/// </summary>
	void TagControl()
	{
		if (tag != SplasheInfo.TRANS_TAG)
		{
			m_SplasheWaterFrame++;
		}

		if (m_SplasheWaterFrame > m_nonTouchFrame)
		{
			tag = SplasheInfo.TRANS_TAG;
		}
	}
}
