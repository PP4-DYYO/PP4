﻿////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018年7月27日～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//編集者
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
/// アイテム情報
/// </summary>
public struct ItemInfo
{
	public const string TAG = "Coin";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// アイテムクラス
/// </summary>
public class MyItem : MonoBehaviour
{
	/// <summary>
	/// 回転角度
	/// </summary>
	[SerializeField]
	int m_rotateAngle;

	/// <summary>
	/// エフェクト
	/// </summary>
	[SerializeField]
	ParticleSystem Effect;
	public ParticleSystem EffectParticle
	{
		get { return Effect; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		transform.Rotate(Vector3.up, m_rotateAngle * Time.deltaTime);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerEnter(Collider other)
	{
		//プレイヤー
		if(other.tag.Equals(PlayerInfo.TAG))
		{
			//コイン取得
			gameObject.SetActive(false);
			Effect.Play();
			MySoundManager.Instance.Play(SeCollection.Coin, true, false, transform.position);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アイテムのリセット
	/// </summary>
	public void ResetItem()
	{
		if (!gameObject.activeInHierarchy)
			gameObject.SetActive(true);
	}
}
