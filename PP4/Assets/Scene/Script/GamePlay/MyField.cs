﻿////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド
/// </summary>
public class MyField : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 船
	/// </summary>
	[SerializeField]
	GameObject Ship;

	/// <summary>
	/// フィールド上のコイン
	/// </summary>
	[SerializeField]
	MyItem[] FieldCoins;

	/// <summary>
	/// 雲中のコイン
	/// </summary>
	[SerializeField]
	MyItem[] CloudCoins;

	/// <summary>
	/// 嵐
	/// </summary>
	[SerializeField]
	GameObject Storm;

	/// <summary>
	/// 雷
	/// </summary>
	[SerializeField]
	GameObject Thunder;

	/// <summary>
	/// 雷雲
	/// </summary>
	[SerializeField]
	ParticleSystem Thundercloud;

	/// <summary>
	/// 雷の通知
	/// </summary>
	[SerializeField]
	GameObject ThunderNotice;

	/// <summary>
	/// 雷の光
	/// </summary>
	[SerializeField]
	GameObject LightningOfThunder;

	/// <summary>
	/// 雲
	/// </summary>
	[SerializeField]
	MyCloud[] Clouds;

	/// <summary>
	/// 隕石
	/// </summary>
	[SerializeField]
	GameObject Meteorite;
	public GameObject MeteoriteObject
	{
		get { return Meteorite; }
	}

	/// <summary>
	/// 隕石の本体
	/// </summary>
	[SerializeField]
	GameObject MeteoriteBody;

	/// <summary>
	/// 隕石の落下エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem[] MeteoriteFallEffects;

	/// <summary>
	/// 隕石の破壊エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem MeteoriteDestructionEffect;

	/// <summary>
	/// 表彰台
	/// </summary>
	[SerializeField]
	GameObject Podium;

	/// <summary>
	/// 表彰
	/// </summary>
	[SerializeField]
	GameObject Honor;

	/// <summary>
	/// 勝ちの表彰
	/// </summary>
	[SerializeField]
	GameObject HonorOfWin;

	/// <summary>
	/// 負けの表彰
	/// </summary>
	[SerializeField]
	GameObject HonorOfLose;
	#endregion

	#region 情報
	[Header("情報")]
	/// <summary>
	/// フィールドの半径
	/// </summary>
	[SerializeField]
	float m_fieldRudius;
	public float FieldRudius
	{
		get { return m_fieldRudius; }
	}
	#endregion

	#region 開始位置
	[Header("開始位置")]
	/// <summary>
	/// 開始位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_startPositions;
	public Vector3[] StartPositions
	{
		get { return m_startPositions; }
	}
	#endregion

	#region 雷
	[Header("雷")]
	/// <summary>
	/// 落雷の時間
	/// </summary>
	[SerializeField]
	float m_timeOfLightning;

	/// <summary>
	/// 雷表示時間
	/// </summary>
	[SerializeField]
	float m_lightningDisplayTime;

	/// <summary>
	/// 雷の時間を数える
	/// </summary>
	float m_countThunderDuration = -1;
	#endregion

	#region 隕石
	[Header("隕石")]
	/// <summary>
	/// 隕石落下の時間
	/// </summary>
	[SerializeField]
	float m_timeOfMeteoriteFall;

	/// <summary>
	/// 隕石出現の時間
	/// </summary>
	[SerializeField]
	float m_timeOfMeteoriteAppearance;

	/// <summary>
	/// 隕石消滅の時間
	/// </summary>
	[SerializeField]
	float m_timeForMeteoriteExtinction;

	/// <summary>
	/// 隕石の速度
	/// </summary>
	[SerializeField]
	float m_meteoriteSpeed;

	/// <summary>
	/// 隕石の回転速度
	/// </summary>
	[SerializeField]
	float m_meteoriteRotateSpeed;

	/// <summary>
	/// 爆風範囲
	/// </summary>
	[SerializeField]
	float m_blastRange;

	/// <summary>
	/// 隕石の時間を数える
	/// </summary>
	float m_countMeteoriteTime = -1;
	#endregion

	#region コイン
	[Header("コイン")]
	/// <summary>
	/// コインの高さ
	/// </summary>
	[SerializeField]
	float[] m_coinHeight;
	#endregion

	#region 雲
	[Header("雲")]
	/// <summary>
	/// 雲の高さ
	/// </summary>
	[SerializeField]
	float[] m_cloudHeight;
	#endregion

	#region 表彰
	[Header("表彰")]
	/// <summary>
	/// 表彰台を隠す位置
	/// </summary>
	[SerializeField]
	Vector3 m_posToHidePodium;

	/// <summary>
	/// 表彰台の初期位置
	/// </summary>
	[SerializeField]
	Vector3 m_initPosOfPodium;

	/// <summary>
	/// 表彰台の目標位置
	/// </summary>
	[SerializeField]
	Vector3 m_targetPosOfPodium;

	/// <summary>
	/// 順位の位置
	/// </summary>
	[SerializeField]
	Vector3[] m_positionsOfRanks;
	public Vector3[] PositionsOfRank
	{
		get { return m_positionsOfRanks; }
	}
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用Vector3
	/// </summary>
	Vector3 m_workVector3;

	/// <summary>
	/// 作業用Float
	/// </summary>
	float m_workFloat;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定フレーム
	/// </summary>
	void FixedUpdate()
	{
		//雷時間を使用している
		if (m_countThunderDuration != -1)
			ThunderFalls();

		//隕石時間を使用している
		if (m_countMeteoriteTime != -1)
			MeteoriteFalls();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雷が落ちる
	/// </summary>
	void ThunderFalls()
	{
		m_countThunderDuration += Time.deltaTime;

		//雷の光の向き
		m_workVector3 = Camera.main.transform.position;
		m_workVector3.y = LightningOfThunder.transform.position.y;
		LightningOfThunder.transform.LookAt(m_workVector3);

		//雷が非表示になる時間
		if (m_countThunderDuration >= m_timeOfLightning + m_lightningDisplayTime)
		{
			LightningOfThunder.SetActive(false);
			m_countThunderDuration = -1;
			return;
		}

		//落雷の時間
		if (m_countThunderDuration >= m_timeOfLightning)
		{
			//通知がアクティブ
			if (ThunderNotice.activeInHierarchy)
			{
				ThunderNotice.SetActive(false);
				LightningOfThunder.SetActive(true);
				MySoundManager.Instance.Play(SeCollection.Lightning);
			}
			return;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 隕石が落ちる
	/// </summary>
	void MeteoriteFalls()
	{
		m_countMeteoriteTime += Time.deltaTime;

		//落下
		Meteorite.transform.position += Vector3.down * m_meteoriteSpeed * Time.deltaTime;
		Meteorite.transform.Rotate(Vector3.up, m_meteoriteRotateSpeed * Time.deltaTime);

		//隕石が出現する時間
		if (m_countMeteoriteTime <= m_timeOfMeteoriteAppearance)
		{
			//出現中
			Meteorite.transform.localScale = Vector3.one * (m_countMeteoriteTime / m_timeOfMeteoriteAppearance);
		}
		else if (m_countMeteoriteTime >= m_timeOfMeteoriteFall - m_timeForMeteoriteExtinction)
		{
			//消滅中
			m_workVector3 = Vector3.one * ((m_timeOfMeteoriteFall - m_countMeteoriteTime) / m_timeForMeteoriteExtinction);
			if (m_workVector3.x < 0)
				Meteorite.transform.localScale = Vector3.zero;
			else
				Meteorite.transform.localScale = m_workVector3;
		}

		//隕石が消滅する時間
		if (m_countMeteoriteTime >= m_timeOfMeteoriteFall)
			StopMeteorit();

		//SE
		MySoundManager.Instance.Play(SeCollection.Meteorite, true, true, Meteorite.transform.position);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 隕石の停止
	/// </summary>
	void StopMeteorit()
	{
		m_countMeteoriteTime = -1;
		Meteorite.SetActive(false);
		MeteoriteDestructionEffect.Stop();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 船の表示
	/// </summary>
	/// <param name="isDisplay">表示</param>
	public void DisplayShip(bool isDisplay = true)
	{
		Ship.SetActive(isDisplay);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フィールドのリセット
	/// </summary>
	public void ResetField()
	{
		//船を隠す
		DisplayShip(false);

		//表彰台を隠す
		Podium.transform.position = m_posToHidePodium;

		//表彰の非表示
		HonorOfWin.SetActive(false);
		HonorOfLose.SetActive(false);

		//嵐を表示
		ShowStorm();

		//コインのリセット
		ResetCoin();

		//雲のリセット
		ResetCloud();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 嵐を表示
	/// </summary>
	/// <param name="isShow">表示するか</param>
	public void ShowStorm(bool isShow = true)
	{
		Storm.SetActive(isShow);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 嵐の位置を設定
	/// </summary>
	/// <param name="pos">位置</param>
	public void SetStormPos(Vector3 pos)
	{
		//位置
		m_workVector3 = Vector3.Scale(pos, Vector3.up);
		Storm.transform.position = m_workVector3;

		//向き
		Storm.transform.LookAt(pos);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コインのリセット
	/// </summary>
	void ResetCoin()
	{
		//フィールド上
		foreach (var coin in FieldCoins)
		{
			//高さの変更
			m_workVector3 = coin.transform.position;
			m_workVector3.y = m_coinHeight[Random.Range(0, m_coinHeight.Length)];
			coin.transform.position = m_workVector3;
			coin.EffectParticle.transform.position = coin.transform.position;

			//リセット
			coin.ResetItem();
		}

		//雲中
		foreach (var coin in CloudCoins)
		{
			//リセット
			coin.ResetItem();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲のリセット
	/// </summary>
	void ResetCloud()
	{
		//全ての雲
		foreach (var cloud in Clouds)
		{
			//高さの変更
			m_workVector3 = cloud.transform.position;
			m_workVector3.y = m_cloudHeight[Random.Range(0, m_cloudHeight.Length)];
			cloud.transform.position = m_workVector3;

			//リセット
			cloud.ResetCloud();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 落雷の開始
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="playerHeight">プレイヤー高度</param>
	public void StartThunderbolt(Vector3 pos, float playerHeight)
	{
		//雷の時間がカウントされている
		if (m_countThunderDuration != -1)
			return;

		//プレイヤーに適した位置
		pos.y = playerHeight;

		Thunder.transform.position = pos;
		Thundercloud.Play();
		ThunderNotice.SetActive(true);
		m_countThunderDuration = 0;

		//SE
		MySoundManager.Instance.Play(SeCollection.Thundercloud);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 落下隕石の開始
	/// </summary>
	/// <param name="startPos">開始高さ</param>
	public void StartOfFallingMeteorite(float startHeight)
	{
		//隕石時間が使用中
		if (m_countMeteoriteTime != -1)
			return;

		Meteorite.transform.localScale = Vector3.zero;
		Meteorite.transform.position = Vector3.up * startHeight;
		Meteorite.SetActive(true);
		if (!MeteoriteBody.activeInHierarchy)
		{
			MeteoriteBody.SetActive(true);
			foreach (var effect in MeteoriteFallEffects)
			{
				effect.Play();
			}
		}
		m_countMeteoriteTime = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 隕石の破壊
	/// </summary>
	/// <param name="targetPos">爆風のターゲット位置</param>
	/// <returns>爆風の影響率</returns>
	public float DestructionOfMeteorite(Vector3 targetPos)
	{
		//落下演出の停止
		MeteoriteBody.SetActive(false);
		foreach (var effect in MeteoriteFallEffects)
		{
			effect.Stop();
		}

		//爆発演出
		MeteoriteDestructionEffect.Play();
		MySoundManager.Instance.Play(SeCollection.MeteoriteDestroyed, true, true, Meteorite.transform.position);

		//爆風影響率
		m_workFloat = 1 - ((MeteoriteBody.transform.position - targetPos).sqrMagnitude / (m_blastRange * m_blastRange));
		return (m_workFloat < 0) ? 0 : m_workFloat;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 隕石の一時停止
	/// </summary>
	public void PauseMeteorit()
	{
		m_countMeteoriteTime = -1;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フィールドの停止
	/// </summary>
	public void StopField()
	{
		ShowStorm(false);
		StopMeteorit();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 表彰台の移動
	/// </summary>
	/// <param name="movementRatio">移動率</param>
	public void MovePodium(float movementRatio)
	{
		Podium.transform.position = m_initPosOfPodium + ((m_targetPosOfPodium - m_initPosOfPodium) * movementRatio);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーを表彰する
	/// </summary>
	/// <param name="pos">表彰位置</param>
	/// <param name="isWin">勝ち</param>
	public void HonorPlayer(Vector3 pos, bool isWin = true)
	{
		Honor.transform.position = pos;
		(isWin ? HonorOfWin : HonorOfLose).SetActive(true);
	}
}
