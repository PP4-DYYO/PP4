////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/12～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

/// <summary>
/// 雲の種類
/// </summary>
enum CloudType
{
	/// <summary>
	/// 雨雲
	/// </summary>
	RainCloud,
	/// <summary>
	/// 雷雲
	/// </summary>
	Thundercloud,
	/// <summary>
	/// 風雨
	/// </summary>
	WindCloud,
	/// <summary>
	/// 金雲
	/// </summary>
	GoldCloud,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 雲情報
/// </summary>
public struct CloudInfo
{
	/// <summary>
	/// 雨雲のタグ
	/// </summary>
	public const string RAIN_TAG = "RainCloud";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 雲
/// </summary>
public class MyCloud : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 雲
	/// </summary>
	[SerializeField]
	ParticleSystem Cloud;

	/// <summary>
	/// 落雷
	/// </summary>
	[SerializeField]
	GameObject Lightning;

	/// <summary>
	/// 雷
	/// </summary>
	[SerializeField]
	GameObject Thunder;
	#endregion

	#region 共通
	[Header("共通")]
	/// <summary>
	/// タイプ
	/// </summary>
	[SerializeField]
	CloudType m_type;
	#endregion

	#region 雷
	[Header("雷")]
	/// <summary>
	/// 落雷のタイミング
	/// </summary>
	[SerializeField]
	float m_timingOfLightning;

	/// <summary>
	/// 落雷の時間
	/// </summary>
	[SerializeField]
	float m_timeOfLightning;

	/// <summary>
	/// 雷の回転値
	/// </summary>
	[SerializeField]
	float m_thunderRotationValue;

	/// <summary>
	/// 雷時間を数える
	/// </summary>
	float m_countThunderDuration;

	/// <summary>
	/// 回転値のランダム性の為の雷番号
	/// </summary>
	int m_thunderNum;
	#endregion

	#region 風
	[Header("風")]
	/// <summary>
	/// 風の力
	/// </summary>
	[SerializeField]
	Vector3 m_windForce;
	#endregion

	/// <summary>
	/// 作業用Vector3
	/// </summary>
	Vector3 m_workVector3;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定フレーム
	/// </summary>
	void FixedUpdate()
	{
		//タイプ
		switch (m_type)
		{
			case CloudType.RainCloud:
				break;
			case CloudType.Thundercloud:
				ThundercloudProcess();
				break;
			case CloudType.WindCloud:
				break;
			case CloudType.GoldCloud:
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雷雲処理
	/// </summary>
	void ThundercloudProcess()
	{
		m_countThunderDuration += Time.deltaTime;

		//落雷のタイミング
		if (m_countThunderDuration >= m_timingOfLightning && !Lightning.activeInHierarchy)
		{
			//Y軸の回転
			m_workVector3 = Lightning.transform.eulerAngles;
			m_workVector3.y += m_thunderRotationValue * (m_thunderNum++);
			Lightning.transform.eulerAngles = m_workVector3;

			//落雷の表示
			Lightning.SetActive(true);

			//数字のループ
			if (m_thunderNum >= int.MaxValue)
				m_thunderNum = 0;
		}

		//落雷が終わるタイミング
		if (m_countThunderDuration >= m_timingOfLightning + m_timeOfLightning)
		{
			m_countThunderDuration = 0;

			//落雷の非表示
			Lightning.SetActive(false);
		}

		//雷の向き(カメラ向き＋ローカルXZ軸回転の無効)
		Thunder.transform.LookAt(Camera.main.transform);
		Thunder.transform.localEulerAngles = Vector3.Scale(Thunder.transform.localEulerAngles, Vector3.up);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり続ける判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerStay(Collider other)
	{
		//タイプ
		switch (m_type)
		{
			case CloudType.RainCloud:
				break;
			case CloudType.Thundercloud:
				break;
			case CloudType.WindCloud:
				if (other.GetComponent<Rigidbody>())
					WindCloudProcess(other.gameObject);
				break;
			case CloudType.GoldCloud:
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 風雲処理
	/// </summary>
	/// <param name="target">影響を受ける対象</param>
	void WindCloudProcess(GameObject target)
	{
		target.transform.position += m_windForce * Time.deltaTime;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり終わり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerExit(Collider other)
	{
		//タイプ
		switch(m_type)
		{
			case CloudType.RainCloud:
				break;
			case CloudType.Thundercloud:
				break;
			case CloudType.WindCloud:
				break;
			case CloudType.GoldCloud:
				if (other.tag.Equals(PlayerInfo.TAG))
					GoldCloudProcess();
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 金雲処理
	/// </summary>
	void GoldCloudProcess()
	{
		//雲の停止
		if (Cloud.isPlaying)
			Cloud.Stop();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲のリセット
	/// </summary>
	public void ResetCloud()
	{
		//雲の再生
		if (!Cloud.isPlaying)
			Cloud.Play();
	}
}
