////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/4/20～
//製作者　京都コンピュータ学院　ゲーム学科　３回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初期設定及びシーンを跨ぐオブジェクト設定
/// </summary>
public class MyBootloader : MonoBehaviour
{
	/// <summary>
	/// シーンを跨ぐオブジェクト
	/// </summary>
	[SerializeField]
	GameObject[] ScenesObject;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動メソッド
	/// </summary>
	void Awake()
	{
		//シーンを跨ぐオブジェクト生成
		foreach (var child in ScenesObject)
		{
			if (GameObject.Find(child.name))
				continue;
			var obj = Instantiate(child);
			obj.name = obj.name.Replace("(Clone)", "");
			obj.AddComponent<MyDontDestroyOnLoad>();
		}

		//ブートローダーの破棄
		Destroy(gameObject);
	}
}
