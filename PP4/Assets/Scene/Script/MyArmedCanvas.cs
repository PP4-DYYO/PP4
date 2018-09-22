////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/9/22～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武装のキャンバス
/// </summary>
public class MyArmedCanvas : MonoBehaviour
{
	/// <summary>
	/// 武装用のカメラ
	/// </summary>
	[SerializeField]
	Camera ArmedCamera;
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		ArmedCamera.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 表示
	/// </summary>
	public void Display()
	{
		ArmedCamera.enabled = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー名を保存
	/// </summary>
	public void SavePlayerName()
	{
		Debug.Log("プレイヤー名を保存しました。");
	}
}
