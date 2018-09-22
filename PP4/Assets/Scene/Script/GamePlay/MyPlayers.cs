////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// チーム
/// </summary>
public enum Team
{
	Team1,
	Team2,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
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

	/// <summary>
	/// ネットワークプレイヤー設定たち
	/// </summary>
	MyNetPlayerSetting[] m_netPlayerSettings;

	/// <summary>
	/// 最高高度
	/// </summary>
	float m_maximumAltitude;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チームを決める
	/// </summary>
	/// <param name="netPlayerSettings">ネットワークプレイヤー設定たち</param>
	public void DecideOnTeam(MyNetPlayerSetting[] netPlayerSettings)
	{
		m_netPlayerSettings = netPlayerSettings;

		for (var i = 0; i < m_netPlayerSettings.Length; i++)
		{
			//交互にチーム分け
			if (i % 2 == 0)
			{
				m_netPlayerSettings[i].transform.parent = Team1.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team1;
			}
			else
			{
				m_netPlayerSettings[i].transform.parent = Team2.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team2;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 最高高度を取得
	/// </summary>
	public float GetMaximumAltitude()
	{
		m_maximumAltitude = 0;

		if (m_netPlayerSettings[0] == null)
		{
			//全子供
			foreach(Transform child in transform)
			{
				//最高高度を超える
				if (child.position.y > m_maximumAltitude)
					m_maximumAltitude = child.position.y;
			}
		}
		else
		{
			//全プレイヤー
			foreach(var player in m_netPlayerSettings)
			{
				//最高高度を超える
				if (player.transform.position.y > m_maximumAltitude)
					m_maximumAltitude = player.transform.position.y;
			}
		}

		return m_maximumAltitude;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム１の高さ合計を取得
	/// </summary>
	/// <param name="scoreArray">得点配列</param>
	/// <returns>高さ合計</returns>
	public float GetTeam1HeightTotal(ref float[] scoreArray)
	{
		//配列要素数が違う
		if (!scoreArray.Length.Equals(Team1.transform.childCount))
			scoreArray = new float[Team1.transform.childCount];

		var totalHeight = 0f;

		//全子供
		for(var i = 0; i < Team1.transform.childCount; i++)
		{
			totalHeight += Team1.transform.GetChild(i).position.y;
			scoreArray[i] = Team1.transform.GetChild(i).position.y;
		}

		return totalHeight;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チーム２の高さ合計を取得
	/// </summary>
	/// <param name="scoreArray">得点配列</param>
	/// <returns>高さ合計</returns>
	public float GetTeam2HeightTotal(ref float[] scoreArray)
	{
		//配列要素数が違う
		if (!scoreArray.Length.Equals(Team2.transform.childCount))
			scoreArray = new float[Team2.transform.childCount];

		var totalHeight = 0f;

		//全子供
		for (var i = 0; i < Team2.transform.childCount; i++)
		{
			totalHeight += Team2.transform.GetChild(i).position.y;
			scoreArray[i] = Team2.transform.GetChild(i).position.y;
		}

		return totalHeight;
	}
}
