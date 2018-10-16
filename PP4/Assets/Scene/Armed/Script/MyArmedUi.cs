////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/22～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武装UI
/// </summary>
public class MyArmedUi : MonoBehaviour
{
	/// <summary>
	/// 武装するキャンバス
	/// </summary>
	[SerializeField]
	MyArmedCanvas ArmedCanvas;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		//武装メニューの表示
		ArmedCanvas.Display();
	}
}
