﻿////////////////////////////////////////////////////////////////////////////////////////////////////
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
	/// <summary>
	/// タイプ
	/// </summary>
	[SerializeField]
	CloudType m_type;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
