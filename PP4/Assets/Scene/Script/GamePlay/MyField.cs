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
	/// 表彰台
	/// </summary>
	[SerializeField]
	GameObject Podium;

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
	#endregion

	#region 表彰台
	[Header("表彰台")]
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
	#endregion

	/// <summary>
	/// 開始位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_startPositions;
	public Vector3[] StartPositions
	{
		get { return m_startPositions; }
	}

	/// <summary>
	/// 順位の位置
	/// </summary>
	[SerializeField]
	Vector3[] m_positionsOfRanks;
	public Vector3[] PositionsOfRank
	{
		get { return m_positionsOfRanks; }
	}

	/// <summary>
	/// ステージの半径
	/// </summary>
	[SerializeField]
	float m_fieldRudius;
	public float FieldRudius
	{
		get { return m_fieldRudius; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フィールドのリセット
	/// </summary>
	public void ResetField()
	{
		//表彰台を隠す
		Podium.transform.position = m_posToHidePodium;

		//全コイン
		foreach (var coin in FieldCoins)
		{
			//リセット
			coin.ResetItem();
		}
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
