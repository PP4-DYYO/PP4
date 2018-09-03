////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/18～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインのUI
/// </summary>
public class MyMainUi : MonoBehaviour
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
	/// タイマー
	/// </summary>
	[SerializeField]
	Text Timer;
	public Text TimerText
	{
		get { return Timer; }
	}

	/// <summary>
	/// カウントダウン
	/// </summary>
	[SerializeField]
	Text Countdown;
	public Text CountdownText
	{
		get { return Countdown; }
	}

	/// <summary>
	/// 結果画面
	/// </summary>
	[SerializeField]
	GameObject ResultScreen;
	public GameObject ResultScreenObj
	{
		get { return ResultScreen; }
	}

	/// <summary>
	/// チーム１のスコア
	/// </summary>
	[SerializeField]
	Text ScoreOfTeam1;
	public Text ScoreOfTeam1Text
	{
		get { return ScoreOfTeam1; }
	}

	/// <summary>
	/// チーム２のスコア
	/// </summary>
	[SerializeField]
	Text ScoreOfTeam2;
	public Text ScoreOfTeam2Text
	{
		get { return ScoreOfTeam2; }
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
