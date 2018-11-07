////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// チュートリアルステージ
/// </summary>
public class MyTutorial : MyGame
{
	int[] m_ranking = new int[8];

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///スタート
	/// </summary>
	private void Start()
	{
		MainUi.SetNumOfCoins();
		MainUi.StopOfFall();
		//ランキング仮設定	
		m_ranking[0] = 0;
		m_ranking[1] = 1;
		m_ranking[2] = 2;
		m_ranking[3] = 3;
		m_ranking[4] = 4;
		m_ranking[5] = 5;
		m_ranking[6] = 6;
		m_ranking[7] = 7;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///定期 
	/// </summary>
	void FixedUpdate()
	{
		//水の残量表示
		MainUi.SetRemainingAmountOfWater(OperatingPlayer.GetPercentageOfRemainingWater());

		//残量０で落下開始
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0)
		{
			MainUi.StartOfFall();
		}
		//残量１で落下終了
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 1)
		{
			MainUi.StopOfFall();
		}

		//コイン所持数表示
		MainUi.SetNumOfCoins(OperatingPlayer.NumOfCoins);

		MainUi.SetRank(m_ranking, 7);
	}
}
