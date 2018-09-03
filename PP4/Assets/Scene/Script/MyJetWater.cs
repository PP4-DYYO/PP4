////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/3～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// テスト用
/// </summary>
public class MyJetWater : MonoBehaviour
{
	/// <summary>
	/// プレイヤーのオブジェクト
	/// </summary>
	[SerializeField]
	MyPlayer myPlayer;

	/// <summary>
	/// 水発射点のオブジェクト1
	/// </summary>
	[SerializeField]
	GameObject Water1;

	/// <summary>
	/// 水発射点のオブジェクト2
	/// </summary>
	[SerializeField]
	GameObject Water2;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アップデート
	/// </summary>	
	void Update()
	{
		Water1.transform.localScale = new Vector3(0.1f, (myPlayer.transform.position.y / 20) + 0.1f, 0.1f);
		Water2.transform.localScale = new Vector3(0.1f, (myPlayer.transform.position.y / 20) + 0.1f, 0.1f);
	}
}
