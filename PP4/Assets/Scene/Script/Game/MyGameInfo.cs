////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018年7月27日～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// PlayerPrefsのキー
/// </summary>
public struct PlayerPrefsKeys
{
	/// <summary>
	/// Trueの数字
	/// </summary>
	public const int TRUE = 1;

	/// <summary>
	/// Falseの数字
	/// </summary>
	public const int FALSE = 0;

	///// <summary>
	///// ステージ番号
	///// </summary>
	//public const string STAGE_NUM = "StageNum";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ゲーム情報
/// </summary>
public class MyGameInfo : MySingleton<MyGameInfo>
{
	/// <summary>
	/// プレイヤー名
	/// </summary>
	string m_playerName = "たけし";
	public string PlayerName
	{
		get { return m_playerName; }
		set { m_playerName = value; }
	}

	/// <summary>
	/// ランク
	/// </summary>
	int m_rank = 1;
	public int Rank
	{
		get { return m_rank; }
		set { m_rank = value; }
	}

	/// <summary>
	/// 経験値
	/// </summary>
	int m_exp = 0;
	public int Exp
	{
		get { return m_exp; }
		set { m_exp = value; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// データをリセットする
	/// </summary>
	public void ResetData()
	{
		//PlayerPrefsのリセット
		//PlayerPrefs.SetInt(PlayerPrefsKeys.STAGE_NUM, 0);
	}
}
