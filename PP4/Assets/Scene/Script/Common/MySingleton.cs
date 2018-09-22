////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/4/19～
//製作者　京都コンピュータ学院　ゲーム学科　３回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singletonクラス
/// </summary>
public abstract class MySingleton<T> where T : class, new()
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
				instance = new T();

			return instance;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// コンストラクタ
	/// </summary>
	protected MySingleton()
	{
		Initialize();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期化
	/// </summary>
	protected virtual void Initialize()
	{
	}
}
