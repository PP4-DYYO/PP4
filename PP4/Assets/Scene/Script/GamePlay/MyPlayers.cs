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
	/// 水しぶき
	/// </summary>
	[SerializeField]
	Transform Splashes;
	public Transform SplashesTrans
	{
		get { return Splashes; }
	}

	/// <summary>
	/// 操作プレイヤー
	/// </summary>
	[SerializeField]
	MyPlayer OperatingPlayer;
	#endregion

	#region プレイヤーカラー
	[Header("プレイヤーカラー")]
	/// <summary>
	/// プレイヤーのカラー達
	/// </summary>
	[SerializeField]
	Color[] m_playerColor;
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
	/// 起動
	/// </summary>
	void Awake()
	{
		if (OperatingPlayer)
			Game.OperatingPlayerScript = OperatingPlayer;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーカラーのリセット
	/// </summary>
	public void ResetPlayerColor()
	{
		//ネットプレイヤー設定がない
		if (m_netPlayerSettings == null)
			return;

		//全プレイヤー
		foreach(var netPlayerSetting in m_netPlayerSettings)
		{
			//スキンのデフォルト化
			netPlayerSetting.SelectSkin.DefaultSkinColor();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー番号を決める
	/// </summary>
	/// <param name="netPlayerSettings">ネットワークプレイヤー設定たち</param>
	public MyNetPlayerSetting[] DecidePlayerNum(MyNetPlayerSetting[] netPlayerSettings)
	{
		//配列の初期化
		m_netPlayerSettings = netPlayerSettings;
		m_powers.Clear();
		if (m_heightRanks == null)
			m_heightRanks = new int[m_netPlayerSettings.Length];

		//パワーの順位を決める
		DetermineOrderOfPower();

		//全プレイヤー
		for (var i = 0; i < m_netPlayerSettings.Length; i++)
		{
			//プレイヤー番号とスキンの変更
			m_netPlayerSettings[i].PlayerNum = m_heightRanks[i];
			m_netPlayerSettings[i].SelectSkin.SetTeamColor(m_playerColor[m_heightRanks[i]]);
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
}
