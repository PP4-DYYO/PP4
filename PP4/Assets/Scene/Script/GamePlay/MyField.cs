////////////////////////////////////////////////////////////////////////////////////////////////////
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
	/// コイン
	/// </summary>
	[SerializeField]
	MyItem Coin;

	/// <summary>
	/// フィールド上のコイン
	/// </summary>
	[SerializeField]
	MyItem[] FieldCoins;

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
	/// 表彰台
	/// </summary>
	[SerializeField]
	GameObject Podium;
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
			}
			return;
		}
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

		//全コイン
		foreach (var coin in FieldCoins)
		{
			//リセット
			coin.ResetItem();
		}

		//嵐を表示と位置のリセット
		ShowStorm();
		SetStormPos(Vector3.zero);
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
		Storm.transform.position = pos;
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
}
