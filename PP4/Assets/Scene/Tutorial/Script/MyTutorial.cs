////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/11/6～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// チュートリアルステージ
/// </summary>
public class MyTutorial : MyGame
{
	/// <summary>
	///参加者番号配列
	/// </summary>
	int[] m_players = new int[8];

	/// <summary>
	/// 現在置（高さ）配列
	/// </summary>
	int[] m_height = new int[8];

	/// <summary>
	/// ランキング用オブジェクト
	/// </summary>
	[SerializeField]
	GameObject Ranking;

	/// <summary>
	/// ランキングの表示状態
	/// </summary>
	bool m_displayRanking;

	/// <summary>
	/// プレイヤー(スキン1)
	/// </summary>
	[SerializeField]
	GameObject PlayerSkinOne;

	/// <summary>
	/// プレイヤーのオーラ
	/// </summary>
	[SerializeField]
	GameObject AuraObject;

	/// <summary>
	/// 敵のゲームオブジェクト
	/// </summary>
	[SerializeField]
	GameObject[] EnemyObject;

	/// <summary>
	/// プレイヤーのスクリプト
	/// </summary>
	public MyPlayer MyPlayerScript;

	/// <summary>
	/// プレイヤー番号
	/// </summary>
	int m_playerNum;

	/// <summary>
	/// 今の順位
	/// </summary>
	int m_nowRank;

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
	/// チュートリアル内の時間(分)
	/// </summary>
	int m_tutorialMinute;

	/// <summary>
	/// チュートリアル内の時間(秒)
	/// </summary>
	int m_tutorialTimeSecond;

	/// <summary>
	/// 時間の単位変換用
	/// </summary>
	const int TIME_CONVERSION = 60;

	/// <summary>
	/// 10秒
	/// </summary>
	const int TEN_SECOND = 10;

	/// <summary>
	/// プレイヤーが落下
	/// </summary>
	bool m_playerFall;

	/// <summary>
	/// MyJetWaterのscript(0がプレイヤー)
	/// </summary>
	[SerializeField]
	MyJetWater[] MyJetWaterScript;

	/// <summary>
	/// 紙吹雪の親オブジェクト
	/// </summary>
	[SerializeField]
	GameObject Papers;

	/// <summary>
	/// 赤の紙吹雪
	/// </summary>
	[SerializeField]
	GameObject RedPaper;

	/// <summary>
	/// 赤の紙吹雪のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem ParticleRedPaper;

	/// <summary>
	/// 青の紙吹雪
	/// </summary>
	[SerializeField]
	GameObject BluePaper;

	/// <summary>
	/// 青の紙吹雪のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem ParticleBluePaper;

	/// <summary>
	/// 黄の紙吹雪
	/// </summary>
	[SerializeField]
	GameObject YellowPaper;

	/// <summary>
	/// 黄の紙吹雪のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem ParticleYellowPaper;

	/// <summary>
	/// 緑の紙吹雪
	/// </summary>
	[SerializeField]
	GameObject GreenPaper;

	/// <summary>
	/// 緑の紙吹雪のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem ParticleGreenPaper;

	/// <summary>
	/// 紙吹雪の出現場所
	/// </summary>
	[SerializeField]
	GameObject PaperPositionObject;

	/// <summary>
	/// 紙吹雪を出現させる
	/// </summary>
	bool m_appearingPaper;

	/// <summary>
	/// 紙吹雪の生存時間(Duration + StartLifeTime)
	/// </summary>
	[SerializeField]
	float m_paperLifeTime;

	/// <summary>
	/// 紙吹雪の出現後の経過時間
	/// </summary>
	float m_paperProgressTime;

	/// <summary>
	/// 敵キャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator[] EnemyAnim;

	/// <summary>
	/// 敵キャラクター移動速度
	/// </summary>
	[SerializeField]
	float m_enemyMoveSpeed;

	/// <summary>
	/// 中央のキャラクターの回転軸のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject[] AxisObject;

	/// <summary>
	/// 楕円運動用度数
	/// </summary>
	float m_degree;

	/// <summary>
	/// m_degreeの値をラジアンに変換するための定数
	/// </summary>
	const float DEGREE_TO_RADIAN = Mathf.PI / 180.0f;

	/// <summary>
	/// m_degreeの値をラジアンに変換した値
	/// </summary>
	float m_radian;

	/// <summary>
	/// 楕円運動のX方向の半径
	/// </summary>
	[SerializeField]
	float m_radiusX;

	/// <summary>
	/// 楕円運動のZ方向の半径
	/// </summary>
	[SerializeField]
	float m_radiusZ;

	/// <summary>
	/// 角度の最大
	/// </summary>
	const float ANGLE_MAX = 360;

	/// <summary>
	/// オブジェクトの移動速度
	/// </summary>
	const float SPEED = 20f;

	/// <summary>
	/// 順位
	/// </summary>
	public enum Ranks
	{
		/// <summary>
		/// 1位
		/// </summary>
		First,
		/// <summary>
		/// 2位
		/// </summary>
		Second,
		/// <summary>
		/// 3位
		/// </summary>
		Third,
		/// <summary>
		/// 4位
		/// </summary>
		Fourth,
		/// <summary>
		/// 5位
		/// </summary>
		Fifth,
		/// <summary>
		/// 6位
		/// </summary>
		Sixth,
		/// <summary>
		/// 7位
		/// </summary>
		Seventh,
		/// <summary>
		/// 8位
		/// </summary>
		Eighth
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///スタート
	/// </summary>
	private void Start()
	{
		MainUi.SetNumOfCoins();
		MainUi.StopOfFall();

		//音の再生
		MySoundManager.Instance.Play(BgmCollection.Battle);

		//敵の設定
		for (int i = 1; i < MyJetWaterScript.Length; i++)
		{
			SetAnimation(PlayerBehaviorStatus.JetRise, i);
			MyJetWaterScript[i].JetFire(true);
		}

		//ランキング設定	
		m_players[0] = (int)Ranks.First;
		m_players[1] = (int)Ranks.Second;
		m_players[2] = (int)Ranks.Third;
		m_players[3] = (int)Ranks.Fourth;
		m_players[4] = (int)Ranks.Fifth;
		m_players[5] = (int)Ranks.Sixth;
		m_players[6] = (int)Ranks.Seventh;
		m_players[7] = (int)Ranks.Eighth;

		//プレイヤーは７番
		m_playerNum = 7;
		MainUi.SetRank(m_players, m_playerNum);

		//現在の高さ配列
		m_height[0] = 700;
		m_height[1] = 600;
		m_height[2] = 500;
		m_height[3] = 400;
		m_height[4] = 300;
		m_height[5] = 200;
		m_height[6] = 100;
		m_height[7] = (int)MyPlayerScript.transform.position.y;

		m_displayRanking = true;

		TutorialMission[0].m_text = "３００mに到達せよ";
		TutorialMission[1].m_text = "タンクの水を空にせよ";
		TutorialMission[2].m_text = "コインを１０枚所得せよ";

		MissionText[0].text = TutorialMission[0].m_text;
		MissionText[1].text = TutorialMission[1].m_text;
		MissionText[2].text = TutorialMission[2].m_text;

		TimerText.text = "" + m_tutorialTime;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	///定期更新
	/// </summary>
	void Update()
	{
		//水の残量表示
		MainUi.SetRemainingAmountOfWater(OperatingPlayer.GetPercentageOfRemainingWater());

		//スペシャルゲージの割合表示
		MainUi.SetRemainingAmountOfAcceleration(OperatingPlayer.GetPercentageOfRemainingSpGauge());

		//コイン所持数表示
		MainUi.SetNumOfCoins(OperatingPlayer.NumOfCoins);

		//ミッションの判定
		CheckMission();

		//ランキング
		CheckRanking();

		//時間関係の動き
		TutorialTimer();

		//敵を動かす
		EnemyMoving();

		//水の残量による動き
		CheckRemainingWater();

		//ボタン入力確認
		MyInputButtonCheck();

		//ジェットの制御
		JetControl();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ジェットのON/OFF切り替え
	/// </summary>
	void JetControl()
	{
		if (MyPlayerScript.State == PlayerBehaviorStatus.Idle || MyPlayerScript.State == PlayerBehaviorStatus.Falling)
		{
			MyJetWaterScript[0].JetFire(false);
		}
		if (MyPlayerScript.State == PlayerBehaviorStatus.IdleInTheAir || MyPlayerScript.State == PlayerBehaviorStatus.IdleInTheAir
			|| MyPlayerScript.State == PlayerBehaviorStatus.JetRise || MyPlayerScript.State == PlayerBehaviorStatus.JetDescent)
		{
			MyJetWaterScript[0].JetFire(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ボタン入力
	/// </summary>
	void MyInputButtonCheck()
	{
		//シーン移動
		if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("HomeButton"))
		{
			MySceneManager.Instance.ChangeScene(MyScene.Matching);
		}

		//ランキング表示切り替え
		if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("YButton"))
		{
			if (m_displayRanking)
			{
				m_displayRanking = false;
			}
			else
			{
				m_displayRanking = true;
			}
			Ranking.SetActive(m_displayRanking);
		}

		//オーラ発動
		if (Mathf.Abs(Input.GetAxis("DpadX")) > 0.1 || Mathf.Abs(Input.GetAxis("DpadY")) > 0.1)
		{
			AuraObject.SetActive(true);
			if (m_nowRank != 0)
			{
				OperatingPlayer.ThrowAuraBall(EnemyObject[m_nowRank - 1], AuraAttribute.Elasticity);
			}
			else
			{
				OperatingPlayer.ThrowAuraBall(EnemyObject[0], AuraAttribute.Elasticity);
			}
		}
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
				MyJetWaterScript[0].JetFire(false);
			}
		}

		//回復で落下終了
		if (m_playerFall && OperatingPlayer.GetPercentageOfRemainingWater() >= 0.99)
		{
			MainUi.StopOfFall();
			MyJetWaterScript[0].JetFire(true);
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

		if (m_appearingPaper)
		{
			m_paperProgressTime += Time.deltaTime;
			if (m_paperProgressTime > m_paperLifeTime)
			{
				m_paperProgressTime = 0;
				Paper(false);
			}
		}

		//残り時間を分と秒に分ける
		m_tutorialMinute = (int)m_tutorialTime / TIME_CONVERSION;
		m_tutorialTimeSecond = (int)m_tutorialTime - m_tutorialMinute * TIME_CONVERSION;

		//残り時間のテキスト
		if (m_tutorialTimeSecond < TEN_SECOND)
		{
			TimerText.text = m_tutorialMinute + ":0" + m_tutorialTimeSecond;
		}
		else
		{
			TimerText.text = m_tutorialMinute + ":" + m_tutorialTimeSecond;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ミッションの判定
	/// </summary>
	void CheckMission()
	{
		//300mに到達
		if (MyPlayerScript.transform.position.y > 300 && TutorialMission[0].m_clear == false)
		{
			TutorialMission[0].m_clear = true;
			MissionText[0].color = Color.gray;
			ClearObject[0].SetActive(true);
			Paper(true);
		}

		//水の残量0
		if (OperatingPlayer.GetPercentageOfRemainingWater() == 0 && TutorialMission[1].m_clear == false)
		{
			TutorialMission[1].m_clear = true;
			MissionText[1].color = Color.gray;
			ClearObject[1].SetActive(true);
			Paper(true);
		}

		//コイン10枚取得
		if (OperatingPlayer.NumOfCoins >= 10 && TutorialMission[2].m_clear == false)
		{
			TutorialMission[2].m_clear = true;
			MissionText[2].color = Color.gray;
			ClearObject[2].SetActive(true);
			Paper(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 紙吹雪の切り替え
	/// </summary>
	/// <param name="ActiveState">true or false</param>
	void Paper(bool ActiveState)
	{
		//すでに発生していたら一度消す
		if (m_appearingPaper)
		{
			RedPaper.SetActive(false);
			BluePaper.SetActive(false);
			YellowPaper.SetActive(false);
			GreenPaper.SetActive(false);
			m_paperProgressTime = 0;
		}

		m_appearingPaper = ActiveState;

		if (ActiveState)
		{
			Papers.transform.position = PaperPositionObject.transform.position;
		}
		RedPaper.SetActive(ActiveState);
		BluePaper.SetActive(ActiveState);
		YellowPaper.SetActive(ActiveState);
		GreenPaper.SetActive(ActiveState);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ランキングの設定
	/// </summary>
	void CheckRanking()
	{
		if (m_displayRanking)
		{
			if (MyPlayerScript.transform.position.y < m_height[6])
			{
				m_players[6] = (int)Ranks.Seventh;
				m_players[7] = (int)Ranks.Eighth;
			}
			else if (MyPlayerScript.transform.position.y < m_height[5])
			{
				m_players[5] = (int)Ranks.Sixth;
				m_players[6] = (int)Ranks.Eighth;
				m_players[7] = (int)Ranks.Seventh;
			}
			else if (MyPlayerScript.transform.position.y < m_height[4])
			{
				m_players[4] = (int)Ranks.Fifth;
				m_players[5] = (int)Ranks.Seventh;
				m_players[7] = (int)Ranks.Sixth;
			}
			else if (MyPlayerScript.transform.position.y < m_height[3])
			{
				m_players[3] = (int)Ranks.Fourth;
				m_players[4] = (int)Ranks.Sixth;
				m_players[7] = (int)Ranks.Fifth;
			}
			else if (MyPlayerScript.transform.position.y < m_height[2])
			{
				m_players[2] = (int)Ranks.Third;
				m_players[3] = (int)Ranks.Fifth;
				m_players[7] = (int)Ranks.Fourth;
			}
			else if (MyPlayerScript.transform.position.y < m_height[1])
			{
				m_players[1] = (int)Ranks.Second;
				m_players[2] = (int)Ranks.Fourth;
				m_players[7] = (int)Ranks.Third;
			}
			else if (MyPlayerScript.transform.position.y < m_height[0])
			{
				m_players[0] = (int)Ranks.First;
				m_players[1] = (int)Ranks.Third;
				m_players[7] = (int)Ranks.Second;
			}
			else
			{
				m_players[0] = (int)Ranks.Second;
				m_players[7] = (int)Ranks.First;
			}
			m_nowRank = m_players[7];
			MainUi.SetRank(m_players, m_playerNum);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーションの変更
	/// </summary>
	/// <param name="state">状態</param>
	/// <param name="num">キャラクター番号(1:右 2:左)</param>
	public void SetAnimation(PlayerBehaviorStatus state, int num)
	{
		switch (num)
		{
			case 1:
				EnemyAnim[0].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 2:
				EnemyAnim[1].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 3:
				EnemyAnim[2].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 4:
				EnemyAnim[3].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 5:
				EnemyAnim[4].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 6:
				EnemyAnim[5].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 7:
				EnemyAnim[6].SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 敵キャラクターの移動(楕円運動)
	/// </summary>
	void EnemyMoving()
	{
		// 移動する分の値をm_degreeに入れる
		m_degree += m_enemyMoveSpeed * Time.deltaTime;

		// m_degreeの値を0から360までの間の数値に変換する
		m_degree = (m_degree % ANGLE_MAX + ANGLE_MAX) % ANGLE_MAX;

		// m_degreeの値をラジアンに変換した値をm_radianに入れる
		m_radian = m_degree * DEGREE_TO_RADIAN;

		for (int i = 0; i < EnemyObject.Length; i++)
		{
			if (i % 2 == 0)
			{
				EnemyObject[i].transform.position = new Vector3(AxisObject[i].transform.position.x + Mathf.Cos(m_radian) * m_radiusX, AxisObject[i].transform.position.y, AxisObject[i].transform.position.z + Mathf.Sin(m_radian) * m_radiusZ);
			}
			else
			{
				EnemyObject[i].transform.position = new Vector3(AxisObject[i].transform.position.x - Mathf.Cos(m_radian) * m_radiusX, AxisObject[i].transform.position.y, AxisObject[i].transform.position.z + Mathf.Sin(m_radian) * m_radiusZ);
			}
			EnemyObject[i].transform.LookAt(AxisObject[i].transform.position);
		}
	}
}
