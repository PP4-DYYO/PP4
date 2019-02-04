////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019年2月4日～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 展示クラス
/// </summary>
public class MyExhibition : MySingletonMonoBehaviour<MyExhibition>
{
	/// <summary>
	/// ゲームの初期化をする時間
	/// </summary>
	[SerializeField]
	float m_timeToInitGame;

	/// <summary>
	/// ゲーム初期化時間を数える
	/// </summary>
	float m_countTimeToInitGame;

	/// <summary>
	/// 初期化フラグ
	/// </summary>
	bool m_isInit;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		m_countTimeToInitGame += Time.deltaTime;

		//入力あり
		if (Input.anyKey)
		{
			m_countTimeToInitGame = 0;
			m_isInit = false;
		}

		//初期化
		if (!m_isInit && m_countTimeToInitGame >= m_timeToInitGame)
		{
			m_isInit = true;
			m_countTimeToInitGame = 0;

			MyGameInfo.Instance.SaveIsPlayTutorial(false);
			MySceneManager.Instance.ChangeScene(MyScene.Title);
		}
	}
}
