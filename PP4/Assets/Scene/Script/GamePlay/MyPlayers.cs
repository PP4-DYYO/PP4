﻿////////////////////////////////////////////////////////////////////////////////////////////////////
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
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
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
	#endregion

	#region チームカラー
	[Header("チームカラー")]
	/// <summary>
	/// チーム１のカラー
	/// </summary>
	[SerializeField]
	Color Team1Color;

	/// <summary>
	/// チーム２のカラー
	/// </summary>
	[SerializeField]
	Color Team2Color;
	#endregion

	/// <summary>
	/// ネットワークプレイヤー設定たち
	/// </summary>
	MyNetPlayerSetting[] m_netPlayerSettings;

	/// <summary>
	/// パワー達
	/// </summary>
	List<int> m_powers = new List<int>();

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
		m_powers.Clear();
		if (m_heightRanks == null)
			m_heightRanks = new int[m_netPlayerSettings.Length];

		//パワーの順位を決める
		DetermineOrderOfPower();

		var team1Order = 0;
		var team2Order = 0;

		//全プレイヤー
		for (var i = 0; i < m_netPlayerSettings.Length; i++)
		{
			//パワーランクが偶数だとチーム１
			if (m_heightRanks[i] % 2 == 0)
			{
				//親とチームとチーム番号とスキンの変更
				m_netPlayerSettings[i].transform.parent = Team1.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team1;
				m_netPlayerSettings[i].TeamOrder = team1Order++;
				m_netPlayerSettings[i].SelectSkin.SetTeamColor(Team1Color);
			}
			else
			{
				//親とチームとチーム番号とスキンの変更
				m_netPlayerSettings[i].transform.parent = Team2.transform;
				m_netPlayerSettings[i].TeamNum = Team.Team2;
				m_netPlayerSettings[i].TeamOrder = team2Order++;
				m_netPlayerSettings[i].SelectSkin.SetTeamColor(Team2Color);
			}
		}

		return m_netPlayerSettings;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// パワーの順位を決める
	/// </summary>
	void DetermineOrderOfPower()
	{
		//初期化
		for (m_targetNum = 0; m_targetNum < m_heightRanks.Length; m_targetNum++)
		{
			//順位
			m_heightRanks[m_targetNum] = 0;
		}

		//全プレイヤー
		for (var i = 0; i < m_netPlayerSettings.Length; i++)
		{
			//パワーの取得
			m_powers.Add(m_netPlayerSettings[i].Power);
		}

		//全プレイヤーにアクセス
		for (m_numToBeChanged = 0; m_numToBeChanged < m_netPlayerSettings.Length; m_numToBeChanged++)
		{
			//変更される番号以外のプレイヤーにアクセス
			for (m_targetNum = m_netPlayerSettings.Length - 1; m_targetNum > m_numToBeChanged; m_targetNum--)
			{
				//パワーが小さければ順位を下げる
				if (m_powers[m_numToBeChanged] < m_powers[m_targetNum])
					m_heightRanks[m_numToBeChanged]++;
				else
					m_heightRanks[m_targetNum]++;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 高さランキングの更新
	/// </summary>
	public void UpdateHeightRank()
	{
		//初期化
		for (m_targetNum = 0; m_targetNum < m_heightRanks.Length; m_targetNum++)
		{
			//順位
			m_heightRanks[m_targetNum] = 0;
		}
		m_maximumAltitude = 0;

		//ネットプレイヤー非対応
		if (m_netPlayerSettings[0] == null)
			return;

		//全プレイヤーにアクセス
		for (m_numToBeChanged = 0; m_numToBeChanged < m_netPlayerSettings.Length; m_numToBeChanged++)
		{
			//変更される番号以外のプレイヤーにアクセス
			for (m_targetNum = m_netPlayerSettings.Length - 1; m_targetNum > m_numToBeChanged; m_targetNum--)
			{
				//高さが小さければ順位を下げる
				if ((int)m_netPlayerSettings[m_numToBeChanged].transform.position.y < (int)m_netPlayerSettings[m_targetNum].transform.position.y)
					m_heightRanks[m_numToBeChanged]++;
				else
					m_heightRanks[m_targetNum]++;
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
