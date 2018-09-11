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
	/// <summary>
	/// チーム１の開始位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_team1StartPositions;
	public Vector3[] Team1StartPositions
	{
		get { return m_team1StartPositions; }
	}

	/// <summary>
	/// チーム２の開始位置たち
	/// </summary>
	[SerializeField]
	Vector3[] m_team2StartPositions;
	public Vector3[] Team2StartPositions
	{
		get { return m_team2StartPositions; }
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
}
