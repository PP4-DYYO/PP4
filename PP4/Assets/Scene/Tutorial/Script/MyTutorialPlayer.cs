////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/1/15～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTutorialPlayer : MonoBehaviour
{

	/// <summary>
	/// UIのスクリプト
	/// </summary>
	[SerializeField]
	MyMainUi MainUiScript;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり判定
	/// </summary>
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == SplasheInfo.TAG || other.tag == CloudInfo.RAIN_TAG)
		{
			MainUiScript.WearWater();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり続ける判定
	/// </summary>
	void OnTriggerStay(Collider other)
	{
		if (other.tag == SplasheInfo.TAG || other.tag == CloudInfo.RAIN_TAG)
		{
			MainUiScript.WearWater();
		}
	}
}
