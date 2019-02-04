////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2019/2/4～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PV
/// </summary>
public class MyPv : MonoBehaviour
{
	/// <summary>
	/// PVのスクリーン
	/// </summary>
	[SerializeField]
	RawImage PvScreen;

	/// <summary>
	/// PV
	/// </summary>
	[SerializeField]
	MovieTexture Pv;

	/// <summary>
	/// 文字
	/// </summary>
	[SerializeField]
	Image PressAnyButton;

	/// <summary>
	/// 点滅速度
	/// </summary>
	[SerializeField]
	float m_blinkRate;

	/// <summary>
	/// 動画終了後の時間
	/// </summary>
	[SerializeField]
	float m_timeAfterVideoEnd;

	/// <summary>
	/// 経過時間
	/// </summary>
	float m_elapsedTime;

	/// <summary>
	/// 動画終了後時間を数える
	/// </summary>
	float m_countTimeAfterVideoEnd;

	/// <summary>
	/// 作業用のColor
	/// </summary>
	Color m_workColor;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		PvScreen.texture = Pv;
		Pv.Play();
		m_elapsedTime = 0;
		m_countTimeAfterVideoEnd = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		m_elapsedTime += Time.deltaTime;

		//文字の点滅
		m_workColor = PressAnyButton.color;
		m_workColor.a = (Mathf.Cos(m_elapsedTime * m_blinkRate) + 1) / 2;
		PressAnyButton.color = m_workColor;

		//入力
		if (Input.anyKey)
			MySceneManager.Instance.ChangeScene(MyScene.Title);

		//PVの終了
		if (!Pv.isPlaying)
		{
			m_countTimeAfterVideoEnd += Time.deltaTime;

			//シーン遷移
			if (m_countTimeAfterVideoEnd >= m_timeAfterVideoEnd)
				MySceneManager.Instance.ChangeScene(MyScene.Title);
		}
	}
}
