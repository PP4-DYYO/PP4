////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/18～
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
/// ステージ情報
/// </summary>
public struct StageInfo
{
	/// <summary>
	/// 地のタグ
	/// </summary>
	public const string GROUND_TAG = "Ground";

	/// <summary>
	/// 雷のタグ
	/// </summary>
	public const string THUNDER_TAG = "Thunder";

	/// <summary>
	/// 雷の通知タグ
	/// </summary>
	public const string THUNDER_NOTICE_TAG = "ThunderNotice";

	/// <summary>
	/// 隕石のタグ
	/// </summary>
	public const string METEORITE_TAG = "Meteorite";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ステージ
/// </summary>
public class MyStage : MonoBehaviour
{
	/// <summary>
	/// ゲーム
	/// </summary>
	[SerializeField]
	MyGame Game;
	public MyGame GameScript
	{
		get { return Game; }
	}

	/// <summary>
	/// フィールド達
	/// </summary>
	[SerializeField]
	MyField[] Fields;

	/// <summary>
	/// 稼働中のフィールド
	/// </summary>
	public MyField CurrentFieldScript
	{
		get { return Fields[0]; }
	}
}
