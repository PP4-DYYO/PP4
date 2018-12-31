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
	/// <summary>
	///ランキング配列
	/// </summary>
	int[] m_ranking = new int[8];

	/// <summary>
	/// 現在置（高さ）配列
	/// </summary>
	int[] m_height = new int[8];

	/// <summary>
	/// プレイヤー
	/// </summary>
	[SerializeField]
	GameObject PlayerObject;

	/// <summary>
	/// プレイヤー番号
	/// </summary>
	int m_playerNum;

	/// <summary>
	/// 現在の順位
	/// </summary>
	int nowRank;

	/// <summary>
	/// ミッション
	/// </summary>
	struct Mission
	{
		/// <summary>
		/// 説明文
		/// </summary>
		public string m_text;

		/// <summary>
		///クリア済みか 
		/// </summary>
		public bool m_clear;
	}

	/// <summary>
	/// ミッション一覧
	/// </summary>
	Mission[] TutorialMission = new Mission[3];

	/// <summary>
	/// Clearの文字のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject[] ClearObject = new GameObject[3];

	/// <summary>
	/// ミッション用のテキスト
	/// </summary>
	[SerializeField]
	Text[] MissionText = new Text[3];

	/// <summary>
	/// チュートリアル内時間用のテキスト
	/// </summary>
	[SerializeField]
	Text TimerText;

	/// <summary>
	/// チュートリアル内の時間
	/// </summary>
	[SerializeField]
	float m_tutorialTime;

	/// <summary>
	/// プレイヤーが落下
	/// </summary>
	bool m_playerFall;

	/// <summary>
	/// MyJetWaterのscript
	/// </summary>
	[SerializeField]
	MyJetWater MyJetWaterScript;

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

		//最初は８位
		nowRank = 8;

		//プレイヤーは７番
		m_playerNum = 7;
		MainUi.SetRank(m_ranking, m_playerNum);

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

		MissionText[0].text = TutorialMission[0].m_text;
		MissionText[1].text = TutorialMission[1].m_text;
		MissionText[2].text = TutorialMission[2].m_text;

		TimerText.text = "" + m_tutorialTime;

		MyJetWaterScript.JetFire(true);
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
		CheckMission();

		//ランキング
		CheckRanking();

		//時間関係の動き
		TutorialTimer();

		//水の残量による動き
		CheckRemainingWater();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水の残量の確認
	/// </summary>
	void CheckRemainingWater()
	{
		//残量０で落下開始
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0)
		{
			MainUi.StartOfFall(OperatingPlayer.ReasonForFallingEnum);
			if (!m_playerFall)
			{
				m_playerFall = true;
			}
		}

		//回復で落下終了
		if (m_playerFall && OperatingPlayer.GetPercentageOfRemainingWater() >= 0.99)
		{
			MainUi.StopOfFall();
			m_playerFall = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///時間経過での動き
	/// </summary>
	void TutorialTimer()
	{
		if (m_tutorialTime > 0)
		{
			m_tutorialTime -= Time.deltaTime;
		}
		else
		{
			m_tutorialTime = 0;
			MySceneManager.Instance.ChangeScene(MyScene.Title);
		}
	
		//残り時間のテキスト
		TimerText.text = m_tutorialTime.ToString("f2");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ミッションの判定
	/// </summary>
	void CheckMission()
	{
		//300mに到達
		if (PlayerObject.transform.position.y > 300)
		{
			TutorialMission[0].m_clear = true;
			MissionText[0].color = Color.gray;
			ClearObject[0].SetActive(true);
		}

		//水の残量0
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0)
		{
			TutorialMission[1].m_clear = true;
			MissionText[1].color = Color.gray;
			ClearObject[1].SetActive(true);
		}

		//コイン10枚取得
		if (OperatingPlayer.NumOfCoins >= 10)
		{
			TutorialMission[2].m_clear = true;
			MissionText[2].color = Color.gray;
			ClearObject[2].SetActive(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ランキングの設定
	/// </summary>
	void CheckRanking()
	{
		if (PlayerObject.transform.position.y > m_height[6] && nowRank > 7)
		{
			m_ranking[6] = 7;
			m_ranking[7] = 6;
			nowRank = 7;
			for (int i = 0; i < m_ranking.Length; i++)
			{
				Debug.Log(i + "位" + m_ranking[i]);
			}
		}

		if (PlayerObject.transform.position.y > m_height[5] && nowRank > 6)
		{
			m_ranking[5] = 7;
			m_ranking[6] = 5;
			nowRank = 6;
			for (int i = 0; i < m_ranking.Length; i++)
			{
				Debug.Log(i + "位" + m_ranking[i]);
			}
		}
		MainUi.SetRank(m_ranking, m_playerNum);
	}
}
