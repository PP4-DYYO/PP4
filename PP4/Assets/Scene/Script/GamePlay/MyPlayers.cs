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
	/// 水しぶき
	/// </summary>
	[SerializeField]
	Transform Splashes;
	public Transform SplashesTrans
	{
		get { return Splashes; }
	}

	/// <summary>
	/// ネットワークプレイヤー設定たち
	/// </summary>
	MyNetPlayerSetting[] m_netPlayerSettings;

	/// <summary>
	/// 最高高度
	/// </summary>
	float m_maximumAltitude;
	public float MaximumAltitude
	{
		get { return m_maximumAltitude; }
	}

	/// <summary>
	/// 高さの順位
	/// </summary>
	int[] m_heightRanks;
	public int[] HeightRanks
	{
		get { return m_heightRanks; }
	}

	/// <summary>
	/// 変更される番号
	/// </summary>
	int m_numToBeChanged;

	/// <summary>
	/// 対象の番号
	/// </summary>
	int m_targetNum;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// チームを決める
	/// </summary>
	/// <param name="netPlayerSettings">ネットワークプレイヤー設定たち</param>
	public MyNetPlayerSetting[] DecideOnTeam(MyNetPlayerSetting[] netPlayerSettings)
	{
		m_netPlayerSettings = netPlayerSettings;
		m_heightRanks = new int[m_netPlayerSettings.Length];

		var team1Order = 0;
		var team2Order = 0;

		for (var i = 0; i < m_netPlayerSettings.Length; i++)
		{

			//交互にチーム分け
			if (i % 2 == 0)
			{
				m_netPlayerSettings[i].transform.parent = Team1.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team1;
				m_netPlayerSettings[i].TeamOrder = team1Order++;
			}
			else
			{
				m_netPlayerSettings[i].transform.parent = Team2.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team2;
				m_netPlayerSettings[i].TeamOrder = team2Order++;
			}
		}

		return m_netPlayerSettings;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 高さランキングの更新
	/// </summary>
	public void UpdateHeightRank()
	{
		//初期化
		m_heightRanks.Initialize();
		m_maximumAltitude = 0;

		//ネットプレイヤー非対応
		if (m_netPlayerSettings[0] == null)
			return;

		//全プレイヤーにアクセス
		for (m_numToBeChanged = 0; m_numToBeChanged < m_netPlayerSettings.Length; m_numToBeChanged++)
		{
			//変更される番号以外のプレイヤーにアクセス
			for (m_targetNum = m_numToBeChanged + 1; m_targetNum < m_netPlayerSettings.Length; m_targetNum++)
			{
				//対象の高さが小さければ順位を下げる
				if (m_netPlayerSettings[m_numToBeChanged].transform.position.y < m_netPlayerSettings[m_targetNum].transform.position.y)
					m_heightRanks[m_numToBeChanged]++;
			}

			//最大高度の取得
			m_maximumAltitude = (m_maximumAltitude < m_netPlayerSettings[m_numToBeChanged].transform.position.y) ?
				m_netPlayerSettings[m_numToBeChanged].transform.position.y : m_maximumAltitude;
		}
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
		for (var i = 0; i < Team1.transform.childCount; i++)
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
