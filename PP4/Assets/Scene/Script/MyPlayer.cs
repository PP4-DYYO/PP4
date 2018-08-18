////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/16～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー
/// </summary>
public class MyPlayer : MonoBehaviour
{
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		Debug.Log("操作するでゴワス");
		if (Input.GetKey(KeyCode.W))
			transform.Translate(Vector3.forward * Time.deltaTime * 5);
		if (Input.GetKey(KeyCode.S))
			transform.Translate(Vector3.back * Time.deltaTime * 5);
		if (Input.GetKey(KeyCode.A))
			transform.Translate(Vector3.left * Time.deltaTime * 5);
		if (Input.GetKey(KeyCode.D))
			transform.Translate(Vector3.right * Time.deltaTime * 5);
	}
}
