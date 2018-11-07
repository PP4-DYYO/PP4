////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/17～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム
/// </summary>
public class MyGame : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// プレイヤーの収集物
	/// </summary>
	[SerializeField]
	protected MyPlayers Players;
	public MyPlayers PlayersScript
	{
		get { return Players; }
	}

	/// <summary>
	/// ステージ
	/// </summary>
	[SerializeField]
	protected MyStage Stage;
	public MyStage StageScript
	{
		get { return Stage; }
	}

	/// <summary>
	/// 操作しているカメラ
	/// </summary>
	[SerializeField]
	protected MyCamera OperatingCamera;
	public MyCamera OperatingCameraScript
	{
		get { return OperatingCamera; }
	}

	/// <summary>
	/// メインのUI
	/// </summary>
	[SerializeField]
	protected MyMainUi MainUi;
	public MyMainUi MainUiScript
	{
		get { return MainUi; }
	}

	/// <summary>
	/// 操作しているプレイヤー
	/// </summary>
	protected MyPlayer OperatingPlayer;
	public MyPlayer OperatingPlayerScript
	{
		get { return OperatingPlayer; }
		set { OperatingPlayer = value; }
	}
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを続ける
	/// </summary>
	public virtual void ContinueBattle()
	{
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトルを辞める
	/// </summary>
	public virtual void LeaveBattle()
	{
	}
}
