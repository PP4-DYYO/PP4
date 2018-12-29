////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// チュートリアルステージ
/// </summary>
public class MyTutorial : MyGame
{
	int[] m_ranking = new int[8];
	int[] m_height = new int[8];

	[SerializeField]
	GameObject PlayerObject;

	int myRankNum;

	int nowRank;

	struct Mission
	{
		public string m_text;
		public bool m_clear;
	}

	Mission[] TutorialMission = new Mission[3];

	[SerializeField]
	GameObject[] ClearObject = new GameObject[3];

	[SerializeField]
	Text[] MissionText = new Text[3];

	[SerializeField]
	Text TimerText;

	[SerializeField]
	float m_tutorialTime;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///スタート
	/// </summary>
	private void Start()
	{
		MainUi.SetNumOfCoins();
		MainUi.StopOfFall();
		//ランキング設定	
		m_ranking[0] = 0;
		m_ranking[1] = 1;
		m_ranking[2] = 2;
		m_ranking[3] = 3;
		m_ranking[4] = 4;
		m_ranking[5] = 5;
		m_ranking[6] = 6;
		m_ranking[7] = 7;
		myRankNum = 7;
		nowRank = 8;

		MainUi.SetRank(m_ranking, myRankNum);

		//現在の高さ配列
		m_height[0] = 700;
		m_height[1] = 600;
		m_height[2] = 500;
		m_height[3] = 400;
		m_height[4] = 300;
		m_height[5] = 200;
		m_height[6] = 100;
		m_height[7] = (int)PlayerObject.transform.position.y;

		TutorialMission[0].m_text = "３００mに到達せよ";
		TutorialMission[1].m_text = "タンクの水を空にせよ";
		TutorialMission[2].m_text = "コインを１０枚所得せよ";

		MissionText[0].text = TutorialMission[0].m_text + "\n";
		MissionText[1].text = TutorialMission[1].m_text + "\n";
		MissionText[2].text = TutorialMission[2].m_text + "\n";

		TimerText.text = "" + m_tutorialTime;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ミッションの判定
	/// </summary>
	void MissionCheck()
	{
		if (PlayerObject.transform.position.y > 100)
		{
			TutorialMission[0].m_clear = true;
			MissionText[0].color = Color.gray;
			ClearObject[0].SetActive(true);
		}

		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0)
		{
			TutorialMission[1].m_clear = true;
			MissionText[1].color = Color.gray;
			ClearObject[1].SetActive(true);
		}

		if (OperatingPlayer.NumOfCoins >= 10)
		{
			TutorialMission[2].m_clear = true;
			MissionText[2].color = Color.gray;
			ClearObject[2].SetActive(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///定期更新
	/// </summary>
	void Update()
	{
		//水の残量表示
		MainUi.SetRemainingAmountOfWater(OperatingPlayer.GetPercentageOfRemainingWater());

		//コイン所持数表示
		MainUi.SetNumOfCoins(OperatingPlayer.NumOfCoins);

		//ミッションの判定
		MissionCheck();

		if (m_tutorialTime > 0)
		{
			m_tutorialTime -= Time.deltaTime;
		}
		else
		{
			m_tutorialTime = 0;
			MySceneManager.Instance.ChangeScene(MyScene.Title);
		}
		TimerText.text = m_tutorialTime.ToString("f2");

		//残量０で落下開始
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0)
		{
			MainUi.StartOfFall(OperatingPlayer.ReasonForFallingEnum);
		}
		//残量１で落下終了
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 1)
		{
			MainUi.StopOfFall();
		}

		if (PlayerObject.transform.position.y > m_height[6] && nowRank > 7)
		{
			m_ranking[6] = 7;
			m_ranking[7] = 6;
			nowRank = 7;
		}

		if (PlayerObject.transform.position.y > m_height[5] && nowRank > 6)
		{
			m_ranking[5] = 7;
			m_ranking[6] = 5;
			nowRank = 6;
		}

		//if (PlayerObject.transform.position.y > m_height[4] && nowRank > 5)
		//{
		//	m_ranking[5] = 4;
		//	m_ranking[4] = myRankNum;
		//	nowRank = 5;
		//}

		//if (PlayerObject.transform.position.y > m_height[3] && nowRank > 4)
		//{
		//	m_ranking[4] = 3;
		//	m_ranking[3] = myRankNum;
		//	nowRank = 4;
		//}

		//if (PlayerObject.transform.position.y > m_height[2] && nowRank > 3)
		//{
		//	m_ranking[3] = 2;
		//	m_ranking[2] = myRankNum;
		//	nowRank = 3;
		//}
		//if (PlayerObject.transform.position.y > m_height[1] && nowRank > 2)
		//{
		//	m_ranking[2] = 1;
		//	m_ranking[1] = myRankNum;
		//	nowRank = 2;
		//}
		//if (PlayerObject.transform.position.y > m_height[0] && nowRank > 1)
		//{
		//	m_ranking[1] = 1;
		//	m_ranking[0] = myRankNum;
		//	nowRank = 1;
		//}
		MainUi.SetRank(m_ranking, myRankNum);
	}
}
