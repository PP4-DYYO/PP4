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
	/// スケールが変更される時間
	/// </summary>
	float m_scaleChangeTime;

	/// <summary>
	/// 自分のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem Effect;
	public ParticleSystem SplasheEffect
	{
		get { return Effect; }
	}

	/// <summary>
	/// 毎フレーム行うサイズ変更の大きさの変数
	/// </summary>
	float m_changeScaleSize;

	/// <summary>
	/// 0.5fの設定
	/// </summary>
	const float HALF = 0.5f;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート処理
	/// </summary>
	void Start()
	{
		//消えるまでの時間より少し小さい数
		m_scaleChangeTime = GetComponent<MyDestroySpreadSplashe>().LimitTime - HALF;
		SpreadSplasheScale = transform.localScale;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スケールの変更
	/// </summary>
	void Update()
	{
		//空中では発生しない
		if (transform.position.y > 0)
		{
			Effect.Stop();
		}
		m_changeScaleSize = Time.deltaTime * (m_magni / m_scaleChangeTime);

		SpreadSplasheScale.x = transform.localScale.x + m_changeScaleSize;
		SpreadSplasheScale.y = transform.localScale.y + m_changeScaleSize;
		SpreadSplasheScale.z = transform.localScale.z + (m_changeScaleSize * HALF);

		transform.localScale = SpreadSplasheScale;
	}
}
