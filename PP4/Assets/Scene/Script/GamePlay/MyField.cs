////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド
/// </summary>
public class MyField : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 船
	/// </summary>
	[SerializeField]
	GameObject Ship;

	/// <summary>
	/// コイン
	/// </summary>
	[SerializeField]
	MyItem Coin;

	/// <summary>
	/// フィールド上のコイン
	/// </summary>
	[SerializeField]
	MyItem[] FieldCoins;

	/// <summary>
	/// 嵐
	/// </summary>
	[SerializeField]
	GameObject Storm;

	/// <summary>
	/// 表彰台
	/// </summary>
	[SerializeField]
	GameObject Podium;
	#endregion

	#region 情報
	[Header("情報")]
	/// <summary>
	/// フィールドの半径
	/// </summary>
	[SerializeField]
	float m_fieldRudius;
	public float FieldRudius
	{
		get { return m_fieldRudius; }
	}
	#endregion

	#region 開始位置
	[Header("開始位置")]
	/// <summary>
	/// 開始位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_startPositions;
	public Vector3[] StartPositions
	{
		get { return m_startPositions; }
	}
	#endregion

	#region 表彰
	[Header("表彰")]
	/// <summary>
	/// 表彰台を隠す位置
	/// </summary>
	[SerializeField]
	Vector3 m_posToHidePodium;

	/// <summary>
	/// 表彰台の初期位置
	/// </summary>
	[SerializeField]
	Vector3 m_initPosOfPodium;

	/// <summary>
	/// 表彰台の目標位置
	/// </summary>
	[SerializeField]
	Vector3 m_targetPosOfPodium;

	/// <summary>
	/// 順位の位置
	/// </summary>
	[SerializeField]
	Vector3[] m_positionsOfRanks;
	public Vector3[] PositionsOfRank
	{
		get { return m_positionsOfRanks; }
	}
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 船の表示
	/// </summary>
	/// <param name="isDisplay">表示</param>
	public void DisplayShip(bool isDisplay = true)
	{
		Ship.SetActive(isDisplay);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フィールドのリセット
	/// </summary>
	public void ResetField()
	{
		//船を隠す
		DisplayShip(false);

		//表彰台を隠す
		Podium.transform.position = m_posToHidePodium;

		//全コイン
		foreach (var coin in FieldCoins)
		{
			//リセット
			coin.ResetItem();
		}

		//嵐を表示と位置のリセット
		ShowStorm();
		SetStormPos(Vector3.zero);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 嵐を表示
	/// </summary>
	/// <param name="isShow">表示するか</param>
	public void ShowStorm(bool isShow = true)
	{
		Storm.SetActive(isShow);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 嵐の位置を設定
	/// </summary>
	/// <param name="pos">位置</param>
	public void SetStormPos(Vector3 pos)
	{
		Storm.transform.position = pos;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 表彰台の移動
	/// </summary>
	/// <param name="movementRatio">移動率</param>
	public void MovePodium(float movementRatio)
	{
		Podium.transform.position = m_initPosOfPodium + ((m_targetPosOfPodium - m_initPosOfPodium) * movementRatio);
	}
}
