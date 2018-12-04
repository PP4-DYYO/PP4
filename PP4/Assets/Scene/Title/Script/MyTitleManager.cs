////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/10/8～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTitleManager : MonoBehaviour
{
	/// <summary>
	/// 手前にいるキャラクター
	/// </summary>
	[SerializeField]
	GameObject FrontCharacter;

	/// <summary>
	/// 手前にいるキャラクターの通常状態の仮面
	/// </summary>
	[SerializeField]
	GameObject FrontNomalMask;

	/// <summary>
	/// 手前にいるキャラクターの星目の仮面
	/// </summary>
	[SerializeField]
	GameObject FrontStarEyeMask;

	/// <summary>
	/// 手前にいるキャラクターのスタート位置
	/// </summary>
	float FrontStartPos;

	/// <summary>
	/// 手前にいるキャラクターの止まる位置
	/// </summary>
	[SerializeField]
	GameObject FrontStopPosObj;

	/// <summary>
	/// 手前にいるキャラクターが画面外へ向けて移動する
	/// </summary>
	bool m_frontLeave;

	/// <summary>
	/// 手前にいるキャラクターの移動速度
	/// </summary>
	[SerializeField]
	float m_frontMovingSpeed;

	/// <summary>
	/// 右からサーブボードで移動するキャラクター
	/// </summary>
	[SerializeField]
	GameObject RightSurfingCharacter;

	/// <summary>
	/// 星の飛ぶエフェクトの出現場所
	/// </summary>
	[SerializeField]
	GameObject StarEyesPosition;

	/// <summary>
	/// 右からサーブボードで移動するキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator RightSufingAnim;

	/// <summary>
	/// 左からサーブボードで移動するキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	GameObject LeftSurfingCharacter;

	/// <summary>
	/// 前キャラクターの目を輝かせるフラグ
	/// </summary>
	bool m_changeStarEye;

	/// <summary>
	/// 目から飛ぶ星用のエフェクト
	/// </summary>
	[SerializeField]
	GameObject StarEyesEffect;

	/// <summary>
	/// 右のサーファーが移動しているか
	/// </summary>
	bool m_isSurfing;

	/// <summary>
	/// 右のサーファーのx座標端
	/// </summary>
	const float END_POINT_X = -15.0f;

	/// <summary>
	/// 右のサーフィン開始位置
	/// </summary>
	Vector3 m_rightSurfingStartPos;

	/// <summary>
	/// 左のサーフィン開始位置
	/// </summary>
	Vector3 m_leftSurfingStartPos;

	/// <summary>
	/// サーフィン移動速度
	/// </summary>
	[SerializeField]
	float m_surfingSpeed;

	/// <summary>
	/// 左からのサーファーの移動を始める
	/// </summary>
	bool m_frontSufingStart;

	/// <summary>
	/// 左からサーブボードで移動するキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator LeftSufingAnim;

	/// <summary>
	/// 左からのサーファーの移動が終わったか
	/// </summary>
	bool m_endLeftSurfingMove;

	/// <summary>
	/// スタートメッセージのテキスト
	/// </summary>
	[SerializeField]
	Text StartMessage;

	/// <summary>
	/// スタートメッセージのテキストのa値
	/// </summary>
	[SerializeField]
	float m_messagAlphaColor;

	/// <summary>
	/// スタートメッセージのテキストのa値のプラスマイナス切り替え
	/// </summary>
	bool m_messagAlphaColorPlus;

	/// <summary>
	/// スタートメッセージのテキストのa値のプラスマイナス切り替え
	/// </summary>
	[SerializeField]
	float m_ChangeAlphaColorSpeed;

	/// <summary>
	/// 前のキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator Anim;

	/// <summary>
	/// 前のキャラクターを回転させる
	/// </summary>
	bool m_frontRotation;

	/// <summary>
	/// 前のキャラクターの初期のrotationYの値
	/// </summary>
	float m_frontStartRotationY;

	/// <summary>
	/// 回転の上限回数
	/// </summary>
	const float ROTATION_LIMIT = 45;

	/// <summary>
	/// 回転の回数
	/// </summary>
	float m_rotationNum;

	/// <summary>
	/// 前のキャラクターの回転速度
	/// </summary>
	[SerializeField]
	float m_frontRotationSpeed;

	/// <summary>
	/// 前キャラクターのコルーチン１番目待ち時間
	/// </summary>
	const float LEAVE_CHARACTER_TIME_ONE = 1.0f;

	/// <summary>
	/// 前キャラクターのコルーチン2番目待ち時間
	/// </summary>
	const float LEAVE_CHARACTER_TIME_TWO = 0.1f;

	/// <summary>
	/// 前キャラクターのコルーチン3番目待ち時間
	/// </summary>
	const float LEAVE_CHARACTER_TIME_THREE = 1.5f;

	/// <summary>
	/// 中央のｘ座標
	/// </summary>
	const float CENTER_POS_X = 0f;

	/// <summary>
	/// 中央付近のｘ座標(右のサーファーがここを超えると前のキャラクターが目を輝かせる)
	/// </summary>
	const float NEAR_CENTER_POS_X = -3f;

	/// <summary>
	/// 右のサーファーのジェットウォータークラス
	/// </summary>
	[SerializeField]
	MyTitleJetWater RightMyJetWater;

	/// <summary>
	/// 右のサーファーのジェットウォータークラスの起動状態
	/// </summary>
	bool m_startRightJetWater;

	/// <summary>
	/// 左のサーファーのジェットウォータークラス
	/// </summary>
	[SerializeField]
	MyTitleJetWater LeftMyJetWater;

	/// <summary>
	/// 左のサーファーのジェットウォータークラスの起動状態
	/// </summary>
	bool m_startLeftJetWater;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート
	/// </summary>
	void Start()
	{
		//手前のキャラクターの状態
		m_changeStarEye = false;
		FrontStartPos = FrontCharacter.transform.position.x;
		m_frontStartRotationY = FrontCharacter.transform.eulerAngles.y;
		FrontNomalMask.SetActive(true);
		FrontStarEyeMask.SetActive(false);
		Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)PlayerBehaviorStatus.Walk);

		//右のサーファーの状態
		m_rightSurfingStartPos = RightSurfingCharacter.transform.position;
		m_isSurfing = true;
		SetAnimation(PlayerBehaviorStatus.Idle, 1);
		SetAnimation(PlayerBehaviorStatus.Idle, 2);

		//左のサーファーの状態
		m_leftSurfingStartPos = LeftSurfingCharacter.transform.position;
		m_endLeftSurfingMove = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイトル画面での動き
	/// </summary>
	void Update()
	{
		MessageColorChange();
		FrontCharacterMove();
		RightSurfingCharacterMove();
		LeftSufingCharacterMove();

		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				Application.Quit();
			}
			else
			{
				MySceneManager.Instance.ChangeScene(MyScene.Armed);
			}
		}

		//右から来るサーファーが飛び始めると目を輝かせる
		if (RightSurfingCharacter.transform.position.x < NEAR_CENTER_POS_X && !m_changeStarEye)
		{
			m_changeStarEye = true;
			FrontNomalMask.SetActive(false);
			FrontStarEyeMask.SetActive(true);
			GameObject el = Instantiate(StarEyesEffect, FrontCharacter.transform);
			el.transform.position = StarEyesPosition.transform.position;
		}

		//右からのサーファーの移動終了後
		if (RightSurfingCharacter.transform.position.x < END_POINT_X)
		{
			//右のサーファーの位置を初期位置に戻す
			m_isSurfing = false;
			RightSurfingCharacter.transform.position = m_rightSurfingStartPos;

			//左のサーファーを動かす
			m_endLeftSurfingMove = true;

			//手前のキャラクターが動きを始める
			StartCoroutine("LeaveCharacter");
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
				RightSufingAnim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 2:
				LeftSufingAnim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 手前のキャラクターの動き(戻るとき)の制御
	/// </summary>
	IEnumerator LeaveCharacter()
	{
		yield return new WaitForSeconds(LEAVE_CHARACTER_TIME_ONE);
		//回転させる
		m_frontRotation = true;
		yield return new WaitForSeconds(LEAVE_CHARACTER_TIME_TWO);
		//歩き始める
		m_frontLeave = true;
		Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)PlayerBehaviorStatus.Walk);
		yield return new WaitForSeconds(LEAVE_CHARACTER_TIME_THREE);
		//状態リセット
		FrontCharacter.transform.eulerAngles = new Vector3(0, m_frontStartRotationY, 0);
		m_rotationNum = 0;
		m_frontSufingStart = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 手前のキャラクターの動き
	/// </summary>
	void FrontCharacterMove()
	{
		//スタート直後である場合、島の端へ向かって歩く
		if (FrontCharacter.transform.position.x < FrontStopPosObj.transform.position.x && !m_frontLeave && !m_endLeftSurfingMove)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x + (m_frontMovingSpeed * Time.deltaTime),
				FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);
		}
		//島の端に着いた時
		if (FrontCharacter.transform.position.x >= FrontStopPosObj.transform.position.x && !m_frontLeave)
		{
			Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)PlayerBehaviorStatus.StandStill);
		}
		//画面外へ向かって歩く
		if (m_frontLeave)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x - (m_frontMovingSpeed * Time.deltaTime),
				FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);
			//停止位置で止まる
			if (FrontCharacter.transform.position.x < FrontStartPos)
			{
				m_frontLeave = false;
			}
		}
		//回転
		if (m_frontRotation)
		{
			if (m_rotationNum < ROTATION_LIMIT)
			{
				FrontCharacter.transform.Rotate(Vector3.up * m_frontRotationSpeed * Time.deltaTime);
				m_rotationNum += 1;
			}
			else
			{
				m_frontRotation = false;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 右からのサーファーの動き
	/// </summary>
	void RightSurfingCharacterMove()
	{
		if (m_isSurfing)
		{
			//中央からは上昇する
			if (RightSurfingCharacter.transform.position.x < CENTER_POS_X)
			{
				if (!m_startRightJetWater)
				{
					//水しぶきの発射
					StartRightSurfingJetWater();
				}
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - (m_surfingSpeed * Time.deltaTime),
				RightSurfingCharacter.transform.position.y + (1.5f * m_surfingSpeed * Time.deltaTime), RightSurfingCharacter.transform.position.z);

				SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 1);
			}
			else
			{
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - (m_surfingSpeed * Time.deltaTime),
				RightSurfingCharacter.transform.position.y, RightSurfingCharacter.transform.position.z);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左からのサーファーの動き
	/// </summary>
	void LeftSufingCharacterMove()
	{
		if (m_frontSufingStart)
		{
			//中央からは上昇する
			if (LeftSurfingCharacter.transform.position.x > CENTER_POS_X)
			{
				if (!m_startLeftJetWater)
				{
					//水しぶきの発射
					StartLeftSurfingJetWater();
				}
				LeftSurfingCharacter.transform.position = new Vector3(LeftSurfingCharacter.transform.position.x + (m_surfingSpeed * Time.deltaTime),
					LeftSurfingCharacter.transform.position.y + (1.5f * m_surfingSpeed * Time.deltaTime), LeftSurfingCharacter.transform.position.z);
				SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 2);
			}
			else
			{
				LeftSurfingCharacter.transform.position = new Vector3(LeftSurfingCharacter.transform.position.x + (m_surfingSpeed * Time.deltaTime),
					LeftSurfingCharacter.transform.position.y, LeftSurfingCharacter.transform.position.z);
			}

			if (LeftSurfingCharacter.transform.position.x > m_rightSurfingStartPos.x)
			{
				LeftSurfingCharacter.transform.position = m_leftSurfingStartPos;
				m_frontSufingStart = false;
				Start();
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 右のサーファーのジェットの起動
	/// </summary>
	void StartRightSurfingJetWater()
	{
		RightMyJetWater.JetFire(true);
		m_startRightJetWater = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左のサーファーのジェットの起動
	/// </summary>
	void StartLeftSurfingJetWater()
	{
		LeftMyJetWater.JetFire(true);
		m_startLeftJetWater = true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタートメッセージの色の変更
	/// </summary>
	private void MessageColorChange()
	{
		if (m_messagAlphaColor > 0 && !m_messagAlphaColorPlus)
		{
			m_messagAlphaColor -= m_ChangeAlphaColorSpeed * Time.deltaTime;
		}
		else if (m_messagAlphaColor < 1 && m_messagAlphaColorPlus)
		{
			m_messagAlphaColor += m_ChangeAlphaColorSpeed * Time.deltaTime;
		}

		if (m_messagAlphaColor >= 1)
		{
			m_messagAlphaColorPlus = false;
		}
		else if (m_messagAlphaColor <= 0)
		{
			m_messagAlphaColorPlus = true;
		}
		StartMessage.GetComponent<Text>().color =
			new Color(StartMessage.color.r, StartMessage.color.g, StartMessage.color.b, m_messagAlphaColor);
	}
}
