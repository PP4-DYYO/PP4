using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTutorialPlayer : MonoBehaviour
{

	[SerializeField]
	MyMainUi MainUiScript;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり判定
	/// </summary>
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == SplasheInfo.TAG || other.tag == SplasheInfo.TRANS_TAG)
		{
			MainUiScript.WearWater();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり続ける判定
	/// </summary>
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == SplasheInfo.TAG || other.tag == SplasheInfo.TRANS_TAG)
		{
			MainUiScript.WearWater();
		}
	}
}
