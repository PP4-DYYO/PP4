////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018年8月4日～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//編集者
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 表示状態
/// </summary>
enum DisplayStatus
{
	/// <summary>
	/// 登場
	/// </summary>
	appearance,
	/// <summary>
	/// 表示
	/// </summary>
	display,
	/// <summary>
	/// 消える
	/// </summary>
	disappear,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 起動クラス
/// </summary>
public class MyStartUp : MonoBehaviour
{
	/// <summary>
	/// 作者ロゴ
	/// </summary>
	[SerializeField]
	Image AuthorLogo;

	/// <summary>
	/// リセットメッセージ
	/// </summary>
	[SerializeField]
	Text ResetMessage;

	/// <summary>
	/// 表示状態
	/// </summary>
	DisplayStatus m_displayState;

	/// <summary>
	/// 出現時間
	/// </summary>
	[SerializeField]
	float m_appearanceTime;

	/// <summary>
	/// 表示時間
	/// </summary>
	[SerializeField]
	float m_displayTime;

	/// <summary>
	/// 時間を数える
	/// </summary>
	float m_countTime;

	/// <summary>
	/// 作業用の色
	/// </summary>
	Color m_workColor;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		//データリセット
		if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.Alpha0))
		{
			MyGameInfo.Instance.ResetData();
			ResetMessage.enabled = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		switch (m_displayState)
		{
			case DisplayStatus.appearance:
				AppearanceLogo();
				break;
			case DisplayStatus.display:
				DisplayLogo();
				break;
			case DisplayStatus.disappear:
				DisappearLogo();
				break;
		}

		m_countTime += Time.deltaTime;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ロゴの登場
	/// </summary>
	void AppearanceLogo()
	{
		m_workColor = AuthorLogo.color;
		m_workColor.a = m_countTime / m_appearanceTime;
		AuthorLogo.color = m_workColor;

		//状態の終了
		if(m_workColor.a >= 1f)
		{
			m_displayState = DisplayStatus.display;
			m_countTime = 0;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ロゴの表示
	/// </summary>
	void DisplayLogo()
	{
		//状態の終了
		if (m_countTime >= m_displayTime)
		{
			m_displayState = DisplayStatus.disappear;
			m_countTime = 0;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ロゴを消す
	/// </summary>
	void DisappearLogo()
	{
		m_workColor = AuthorLogo.color;
		m_workColor.a = 1f - (m_countTime / m_appearanceTime);
		AuthorLogo.color = m_workColor;

		//状態の終了
		if (m_workColor.a < 0f)
		{
			SceneManager.LoadScene("Title");
		}
	}
}
