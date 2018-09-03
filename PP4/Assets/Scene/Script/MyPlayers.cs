////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの収集物
/// </summary>
public class MyPlayers : MonoBehaviour
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
	/// チーム１
	/// </summary>
	[SerializeField]
	GameObject Team1;

	/// <summary>
	/// チーム２
	/// </summary>
	[SerializeField]
	GameObject Team2;
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム１の高さ合計を取得
	/// </summary>
	/// <returns>高さ合計</returns>
	public float GetTeam1HeightTotal()
	{
		var totalHeight = 0f;

		//全子供
		foreach(Transform child in Team1.transform)
		{
			totalHeight += child.position.y;
		}

		return totalHeight;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム２の高さ合計を取得
	/// </summary>
	/// <returns>高さ合計</returns>
	public float GetTeam2HeightTotal()
	{
		var totalHeight = 0f;

		//全子供
		foreach (Transform child in Team2.transform)
		{
			totalHeight += child.position.y;
		}

		return totalHeight;
	}
}
