////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/12/10～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
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
/// オーラ属性
/// </summary>
public enum AuraAttribute
{
	/// <summary>
	/// 温熱
	/// </summary>
	Heat,
	/// <summary>
	/// 弾性
	/// </summary>
	Elasticity,
	/// <summary>
	/// 電気
	/// </summary>
	Electrical,
	/// <summary>
	/// なし
	/// </summary>
	Non,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// オーラ情報
/// </summary>
public struct AuraInfo
{
	/// <summary>
	/// 温熱のタグ
	/// </summary>
	public const string HEAT_TAG = "HeatAura";

	/// <summary>
	/// 弾性のタグ
	/// </summary>
	public const string ELASTICITY_TAG = "ElasticityAura";

	/// <summary>
	/// 電気のタグ
	/// </summary>
	public const string ELECTRICAL_TAG = "ElectricalAura";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// オーラボール
/// </summary>
public class MyAuraBall : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 投手
	/// </summary>
	[SerializeField]
	Transform Pitcher;

	/// <summary>
	/// 温熱オーラボール
	/// </summary>
	[SerializeField]
	GameObject HeatAuraBall;

	/// <summary>
	/// 弾性オーラボール
	/// </summary>
	[SerializeField]
	GameObject ElasticityAuraBall;

	/// <summary>
	/// 電気オーラボール
	/// </summary>
	[SerializeField]
	GameObject ElectricalAuraBall;

	/// <summary>
	/// 温熱のエフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem HeatEffect;

	/// <summary>
	/// 弾性のエフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem ElasticityEffect;

	/// <summary>
	/// 電気のエフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem ElectricalEffect;
	#endregion

	#region ターゲット
	[Header("ターゲット")]
	/// <summary>
	/// ターゲット
	/// </summary>
	GameObject m_target;

	/// <summary>
	/// 当たったプレイヤー名
	/// </summary>
	string m_hitPlayerName = null;
	public string HitPlayerName
	{
		get { return m_hitPlayerName; }
	}
	#endregion

	#region 能力
	[Header("能力")]
	/// <summary>
	/// 移動時間
	/// </summary>
	[SerializeField]
	float m_travelTime;

	/// <summary>
	/// 移動速度が変わる割合
	/// </summary>
	[SerializeField]
	float m_rateAtWhichMovementSpeedChanges;

	/// <summary>
	/// ヒット情報の記憶時間
	/// </summary>
	[SerializeField]
	float m_hitInfoStorageTime;

	/// <summary>
	/// 注意をし始める距離
	/// </summary>
	[SerializeField]
	float m_distanceToStartToNote;

	/// <summary>
	/// オーラ
	/// </summary>
	AuraAttribute m_aura;

	/// <summary>
	/// 移動時間を数える
	/// </summary>
	float m_countTravelTime = -1;

	/// <summary>
	/// ヒット情報記憶時間を数える
	/// </summary>
	float m_countHitInfoStorageTime = -1;
	#endregion

	/// <summary>
	/// 作業用のFloat
	/// </summary>
	float m_workFloat;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//移動
		if (m_countTravelTime != -1)
			Moving();

		//ヒット情報の記憶する時間
		if (m_countHitInfoStorageTime != -1)
			ForgetHitInfo();

		//カメラを注視
		transform.LookAt(Camera.main.transform);

		//接近音
		SoundApproachingSound();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 移動する
	/// </summary>
	void Moving()
	{
		//標的を見失う
		if (!m_target)
		{
			//オーラの消滅
			EraseAura();

			return;
		}

		//終了
		if (m_countTravelTime >= m_travelTime)
			EraseAura();

		m_countTravelTime += Time.deltaTime;

		//始まりと終わりがゆっくりな移動率
		m_workFloat = m_countTravelTime / m_travelTime;
		m_workFloat = (m_workFloat > m_rateAtWhichMovementSpeedChanges ?
			(1 - Mathf.Pow(1 - m_workFloat, 2) / (1 - m_rateAtWhichMovementSpeedChanges))
			: (m_workFloat * m_workFloat) / m_rateAtWhichMovementSpeedChanges);

		transform.position = Vector3.Lerp(Pitcher.position, m_target.transform.position, m_workFloat);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// オーラを消す
	/// </summary>
	public void EraseAura()
	{
		m_countTravelTime = -1;

		//オーラ
		switch (m_aura)
		{
			case AuraAttribute.Heat:
				HeatAuraBall.SetActive(false);
				break;
			case AuraAttribute.Elasticity:
				ElasticityAuraBall.SetActive(false);
				break;
			case AuraAttribute.Electrical:
				ElectricalAuraBall.SetActive(false);
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ヒット情報を忘れる
	/// </summary>
	void ForgetHitInfo()
	{
		m_countHitInfoStorageTime += Time.deltaTime;

		if (m_countHitInfoStorageTime >= m_hitInfoStorageTime)
		{
			m_countHitInfoStorageTime = -1;
			m_hitPlayerName = null;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 接近した音を鳴らす
	/// </summary>
	void SoundApproachingSound()
	{
		//オーラ移動中でない
		if (m_countTravelTime == -1)
			return;

		//カメラとの距離
		if ((Camera.main.transform.position - transform.position).sqrMagnitude >= m_distanceToStartToNote * m_distanceToStartToNote)
			return;

		//オーラ
		switch (m_aura)
		{
			case AuraAttribute.Heat:
				MySoundManager.Instance.Play(SeCollection.FlameAura, true, true, transform.position);
				break;
			case AuraAttribute.Elasticity:
				MySoundManager.Instance.Play(SeCollection.LightningAura, true, true, transform.position);
				break;
			case AuraAttribute.Electrical:
				MySoundManager.Instance.Play(SeCollection.EarthAura, true, true, transform.position);
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerEnter(Collider other)
	{
		//投手と重なる
		if (transform.IsChildOf(other.transform))
			return;

		//プレイヤー
		if (other.tag == PlayerInfo.TAG)
		{
			//プレイヤー名
			if (other.GetComponent<MyPlayer>())
				SetHitPlayerName(other.GetComponent<MyPlayer>().Name);

			//ヒット演出
			switch(m_aura)
			{
				case AuraAttribute.Heat:
					HeatEffect.transform.position = transform.position;
					HeatEffect.Play();
					break;
				case AuraAttribute.Elasticity:
					ElasticityEffect.transform.position = transform.position;
					ElasticityEffect.Play();
					break;
				case AuraAttribute.Electrical:
					ElectricalEffect.transform.position = transform.position;
					ElectricalEffect.Play();
					break;
			}

			EraseAura();

			return;
		}

		//オーラ
		switch (m_aura)
		{
			case AuraAttribute.Heat:
				if (other.tag == AuraInfo.ELECTRICAL_TAG)
					EraseAura();
				break;
			case AuraAttribute.Elasticity:
				if (other.tag == AuraInfo.HEAT_TAG)
					EraseAura();
				break;
			case AuraAttribute.Electrical:
				if (other.tag == AuraInfo.ELASTICITY_TAG)
					EraseAura();
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 投げる
	/// </summary>
	/// <param name="target">ターゲット</param>
	/// <param name="aura">オーラ</param>
	public void Throw(GameObject target, AuraAttribute aura)
	{
		//使用中
		if (m_countTravelTime != -1)
			return;

		//オーラ
		switch (aura)
		{
			case AuraAttribute.Heat:
				HeatAuraBall.SetActive(true);
				break;
			case AuraAttribute.Elasticity:
				ElasticityAuraBall.SetActive(true);
				break;
			case AuraAttribute.Electrical:
				ElectricalAuraBall.SetActive(true);
				break;
		}

		m_countTravelTime = 0;
		m_target = target;
		m_aura = aura;
		transform.position = Pitcher.position;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たったプレイヤー名の設定
	/// </summary>
	/// <param name="playerName">プレイヤー名</param>
	public void SetHitPlayerName(string playerName)
	{
		m_hitPlayerName = playerName;
		m_countHitInfoStorageTime = 0;
	}
}
