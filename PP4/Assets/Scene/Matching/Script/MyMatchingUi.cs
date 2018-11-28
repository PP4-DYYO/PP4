////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/27～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// マッチングUI状態
/// </summary>
enum MatchingUiStatus
{
	/// <summary>
	/// マッチング
	/// </summary>
	Matching,
	/// <summary>
	/// 待機
	/// </summary>
	Wait,
	/// <summary>
	/// 状態の数
	/// </summary>
	Count,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// マッチングUI
/// </summary>
public class MyMatchingUi : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 再マッチング
	/// </summary>
	[SerializeField]
	GameObject Rematch;

	/// <summary>
	/// 再マッチング開始ボタン
	/// </summary>
	[SerializeField]
	Button StartRematchButton;

	/// <summary>
	/// マッチング終了ボタン
	/// </summary>
	[SerializeField]
	Button MatchEndButton;
	#endregion

	#region 送付される処理
	[Header("送付される処理")]
	/// <summary>
	/// 再マッチングする処理
	/// </summary>
	ProcessingWhenButtonIsPressed ToRematch;
	public ProcessingWhenButtonIsPressed ToRematchProcess
	{
		set { ToRematch = value; }
	}

	/// <summary>
	/// マッチングを終了する処理
	/// </summary>
	ProcessingWhenButtonIsPressed FinishMatching;
	public ProcessingWhenButtonIsPressed FinishMatchingProcess
	{
		set { FinishMatching = value; }
	}
	#endregion
	
	#region 色
	[Header("色")]
	/// <summary>
	/// 選択中の色
	/// </summary>
	[SerializeField]
	Color m_selectColor;

	/// <summary>
	/// 選択していない色
	/// </summary>
	[SerializeField]
	Color m_nonSelectColor;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再マッチングボタンをクリック
	/// </summary>
	public void OnClickRematchButton()
	{
		ToRematch();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再マッチングを隠す
	/// </summary>
	public void HideRematch()
	{
		Rematch.SetActive(false);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マッチングを終了するボタンをクリック
	/// </summary>
	public void OnClickFinishMatchingButton()
	{
		FinishMatching();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再マッチングの確認
	/// </summary>
	public void StartConfirmRematch()
	{
		Rematch.SetActive(true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 再マッチングボタンを選択する
	/// </summary>
	public void SelectRematchButton()
	{
		StartRematchButton.image.color = m_selectColor;
		MatchEndButton.image.color = m_nonSelectColor;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// マッチング終了ボタンを選択する
	/// </summary>
	public void SelectFinishMatchingButton()
	{
		MatchEndButton.image.color = m_selectColor;
		StartRematchButton.image.color = m_nonSelectColor;
	}
}
