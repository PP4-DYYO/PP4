////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/4/17～
//製作者　京都コンピュータ学院　ゲーム学科　３回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SingletonのMonoBehaviourクラス
/// </summary>
public abstract class MySingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>
	/// 唯一のインスタンス
	/// </summary>
	private static T instance;

	/// <summary>
	/// インスタンス
	/// </summary>
	public static T Instance
	{
		get
		{
			if (instance == null)  //インスタンスが生成されていない
			{
				var t = typeof(T);

				instance = (T)FindObjectOfType(t);

				if (instance == null)  //インスタンスが生成されなかった
					Debug.LogError(t + " をアタッチしているGameObjectはありません");
			}
			return instance;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動時のメソッド
	/// </summary>
	virtual protected void Awake()
	{
		//他のGameObjectにアタッチされていれば破棄
		if (this != Instance)
		{
			Destroy(this);
			Debug.LogError(typeof(T) + " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました。" + " アタッチされているGameObjectは " + Instance.gameObject.name + " です。");
			return;
		}
	}
}
