////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/20～
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
	/// 水しぶきの水クラス
	/// </summary>
	[SerializeField]
	MySplasheWater SplasheWater;
	public MySplasheWater SplasheWaterScript
	{
		get { return SplasheWater; }
	}
}
