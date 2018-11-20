////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶきの泡のサイズ調整
/// </summary>
public class MySpreadSplashe : MonoBehaviour
{
	/// <summary>
	/// 時間計測用
	/// </summary>
	float m_spreadSplasheTime;

	/// <summary>
	/// 水しぶきの泡オブジェクトのサイズ
	/// </summary>
	Vector3 SpreadSplasheScale;

	/// <summary>
	/// サイズの変更の倍率
	/// </summary>
	[SerializeField]
	float m_magni;

	/// <summary>
	/// 泡が消えるまでの時間
	/// </summary>
	float m_limitTime;

	/// <summary>
	/// 自分のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem Effect;
	public ParticleSystem SplasheEffect
	{
		get { return Effect; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート処理
	/// </summary>
	void Start()
	{
		m_limitTime = GetComponent<MyDestroySpreadSplashe>().LimitTime - 0.5f;
		SpreadSplasheScale = transform.localScale;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// サイズの変更
	/// </summary>
	void FixedUpdate()
	{
		SpreadSplasheScale = new Vector3(transform.localScale.x + (Time.fixedDeltaTime * (m_magni / m_limitTime)),
			transform.localScale.y + (Time.fixedDeltaTime * (m_magni / m_limitTime)), transform.localScale.z + ((Time.fixedDeltaTime * (m_magni / m_limitTime)) / 2));
		transform.localScale = SpreadSplasheScale;
	}
}
