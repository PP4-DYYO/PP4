﻿////////////////////////////////////////////////////////////////////////////////////////////////////
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
	/// 手前にいるキャラクターのスタート位置用のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject FrontStartPositionObject;

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
	/// 手前にいるキャラクターのコルーチンの開始状況
	/// </summary>
	bool m_startFrontCoroutine;

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
	/// 左のサーファーの目から飛ぶ星の位置のオブジェクト
	/// </summary>
	[SerializeField]
	Transform StarEyesEffectNomalPosition;

	/// <summary>
	/// 左のサーファーの目から飛ぶ星の位置のオブジェクト
	/// </summary>
	[SerializeField]
	Transform StarEyesEffectFlyingPosition;

	/// <summary>
	/// 左のサーファーの目から飛ぶ星のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject StarEyesEffectObject;

	/// <summary>
	/// 右のサーファーが移動しているか
	/// </summary>
	bool m_isRightSurfing;

	/// <summary>
	/// 右のサーファーのx座標端
	/// </summary>
	const float LEFT_START_MOVING_POINT_X = -25.0f;

	/// <summary>
	/// 右のサーファーのx座標端
	/// </summary>
	const float END_RIGHT_SURFING_POINT_X = -45.0f;

	/// <summary>
	/// 手前のキャラクターが動き出す右のサーファーのx座標
	/// </summary>
	const float START_FRONT_POINT_X = -7.0f;

	/// <summary>
	/// 右のサーフィン開始位置
	/// </summary>
	Vector3 m_rightSurfingStartPos;

	/// <summary>
	/// 左のサーファー開始位置
	/// </summary>
	Vector3 m_leftSurfingStartPos;

	/// <summary>
	/// 左のサーファーの終了位置
	/// </summary>
	const float END_LEFT_SURFING_POINT_X = 45.0f;

	/// <summary>
	/// サーフィンの横移動速度
	/// </summary>
	[SerializeField]
	float m_surfingHorizontalSpeed;

	/// <summary>
	/// サーフィンの縦移動速度
	/// </summary>
	[SerializeField]
	float m_surfingVerticalSpeed;

	/// <summary>
	/// 左からのサーファーの移動状態
	/// </summary>
	bool m_isLeftSurfing;

	/// <summary>
	/// 左からサーブボードで移動するキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator LeftSufingAnim;

	/// <summary>
	/// 前のキャラクターの移動が終わったか
	/// </summary>
	bool m_endFrontMove;

	/// <summary>
	/// スタートメッセージのテキスト
	/// </summary>
	[SerializeField]
	Image StartMessage;

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
	/// 回転の回数
	/// </summary>
	float m_rotationNum;

	/// <summary>
	/// 前のキャラクターの回転速度
	/// </summary>
	[SerializeField]
	float m_frontRotationSpeed;

	/// <summary>
	/// 前のキャラクターの最大の回転角度
	/// </summary>
	const float MAX_ANGLE = 180;

	/// <summary>
	/// 回転の上限回数
	/// </summary>
	float m_rotationLimit;

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
	/// 右のサーファーのジェットの起動状態
	/// </summary>
	bool m_rightJetWaterState;

	/// <summary>
	/// 左のサーファーのジェットウォータークラス
	/// </summary>
	[SerializeField]
	MyTitleJetWater LeftMyJetWater;

	/// <summary>
	/// 左のサーファーのジェットの起動状態
	/// </summary>
	bool m_leftJetWaterState;

	/// <summary>
	/// タイトルの動きが全て終わった
	/// </summary>
	bool m_endTitle;

	/// <summary>
	/// 再スタートの待ち時間
	/// </summary>
	[SerializeField]
	float m_waitingRestartTime;

	/// <summary>
	/// 動きが全て終わってからの経過時間
	/// </summary>
	float m_elapsedTime;

	/// <summary>
	/// ループ回数
	/// </summary>
	[SerializeField]
	int m_numOfLoops;

	/// <summary>
	/// ループ回数を数える
	/// </summary>
	int m_countNumOfLoops;

	/// <summary>
	/// メッセージ
	/// </summary>
	[SerializeField]
	Text Message;

	/// <summary>
	/// メッセージの最小サイズ
	/// </summary>
	[SerializeField]
	Vector3 m_messageMinSize;

	/// <summary>
	/// メッセージの拡縮
	/// </summary>
	[SerializeField]
	float m_messageScale;

	/// <summary>
	/// メッセージの速度
	/// </summary>
	[SerializeField]
	float m_messageSpeed;

	/// <summary>
	/// メッセージ用の経過時間
	/// </summary>
	float m_elapsedTimeForMessage;

	/// <summary>
	/// コルーチン
	/// </summary>
	IEnumerator Coroutine;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート
	/// </summary>
	void Start()
	{
		//手前のキャラクターの状態
		m_rotationLimit = Mathf.Abs(MAX_ANGLE / m_frontRotationSpeed);
		m_changeStarEye = false;
		m_frontStartRotationY = FrontCharacter.transform.eulerAngles.y;
		FrontNomalMask.SetActive(true);
		FrontStarEyeMask.SetActive(false);
		SetAnimation(PlayerBehaviorStatus.Walk, 0);

		//右のサーファーの状態
		m_rightSurfingStartPos = RightSurfingCharacter.transform.position;
		m_isRightSurfing = true;
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 1);
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 2);

		//左のサーファーの状態
		m_leftSurfingStartPos = LeftSurfingCharacter.transform.position;
		m_endFrontMove = false;

		m_countNumOfLoops = 0;

		//BGM
		MySoundManager.Instance.Play(BgmCollection.Title);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイトル画面での動き
	/// </summary>
	void Update()
	{
		//スタートメッセージの点滅
		MessageColorChange();

		//手前のキャラクターの動作
		FrontCharacterMove();

		//右側のキャラクターの動作
		RightSurfingCharacterMove();

		//左側のキャラクターの動作
		LeftSufingCharacterMove();

		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("BackButton"))
			{
				Application.Quit();
			}
			else if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("HomeButton"))
			{
				MySceneManager.Instance.ChangeScene(MyScene.Credit);
			}
			else
			{
				MySceneManager.Instance.ChangeScene(MyScene.Armed);
			}
		}

		if (m_endTitle)
		{
			WaitingRestart();
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
			case 0:
				Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
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
		SetAnimation(PlayerBehaviorStatus.Walk, 0);
		yield return new WaitForSeconds(LEAVE_CHARACTER_TIME_THREE);
		//状態リセット
		SetAnimation(PlayerBehaviorStatus.StandStill, 0);
		m_endFrontMove = true;
		yield break;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 手前のキャラクターの動き
	/// </summary>
	void FrontCharacterMove()
	{
		//スタート直後である場合、島の端へ向かって歩く
		if (FrontCharacter.transform.position.x < FrontStopPosObj.transform.position.x && !m_frontLeave && !m_endFrontMove)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x + (m_frontMovingSpeed * Time.deltaTime),
				FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);
		}
		//島の端に着いた時
		if (FrontCharacter.transform.position.x >= FrontStopPosObj.transform.position.x && !m_frontLeave)
		{
			SetAnimation(PlayerBehaviorStatus.StandStill, 0);
		}
		//画面外へ向かって歩く
		if (m_frontLeave)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x - (m_frontMovingSpeed * Time.deltaTime),
				FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);
			//停止位置で止まる
			if (FrontCharacter.transform.position.x < FrontStartPositionObject.transform.position.x)
			{
				m_frontLeave = false;
				SetAnimation(PlayerBehaviorStatus.Stand, 0);
			}
		}
		//回転
		if (m_frontRotation)
		{
			if (m_rotationNum < m_rotationLimit)
			{
				FrontCharacter.transform.Rotate(Vector3.up * m_frontRotationSpeed);
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
		if (m_isRightSurfing)
		{
			//前のキャラクターが目を輝かせる
			if (RightSurfingCharacter.transform.position.x < NEAR_CENTER_POS_X && !m_changeStarEye)
			{
				m_changeStarEye = true;
				FrontNomalMask.SetActive(false);
				FrontStarEyeMask.SetActive(true);
				GameObject el = Instantiate(StarEyesEffect, FrontCharacter.transform);
				el.transform.position = StarEyesPosition.transform.position;
			}

			//前のキャラクターが移動し始める
			if (RightSurfingCharacter.transform.position.x < START_FRONT_POINT_X && !m_startFrontCoroutine)
			{
				//手前のキャラクターが動きを始める
				Coroutine = LeaveCharacter();
				StartCoroutine(Coroutine);
				m_startFrontCoroutine = true;
			}

			//中央からは上昇する
			if (RightSurfingCharacter.transform.position.x < CENTER_POS_X)
			{
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - (m_surfingHorizontalSpeed * Time.deltaTime),
				RightSurfingCharacter.transform.position.y + (m_surfingVerticalSpeed * m_surfingHorizontalSpeed * Time.deltaTime), RightSurfingCharacter.transform.position.z);

				SetAnimation(PlayerBehaviorStatus.JetRise, 1);
			}
			//中央までは横移動のみ
			else
			{
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - (m_surfingHorizontalSpeed * Time.deltaTime),
				RightSurfingCharacter.transform.position.y, RightSurfingCharacter.transform.position.z);
			}

			//移動終了
			if (RightSurfingCharacter.transform.position.x < END_RIGHT_SURFING_POINT_X)
			{
				//左のサーファー移動開始
				m_isLeftSurfing = true;

				//位置を初期位置に戻す
				m_isRightSurfing = false;
				RightSurfingCharacter.transform.position = m_rightSurfingStartPos;
				RightSurfingJetWater(false);
			}
		}
		RightSurfingJetWater(m_isRightSurfing);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左からのサーファーの動き
	/// </summary>
	void LeftSufingCharacterMove()
	{
		if (m_isLeftSurfing)
		{
			//中央からは上昇する
			if (LeftSurfingCharacter.transform.position.x > CENTER_POS_X)
			{
				//目の位置の補正
				ChangeStarEyesPosition(true);

				LeftSurfingCharacter.transform.position = new Vector3(LeftSurfingCharacter.transform.position.x + (m_surfingHorizontalSpeed * Time.deltaTime),
					LeftSurfingCharacter.transform.position.y + (m_surfingVerticalSpeed * m_surfingHorizontalSpeed * Time.deltaTime), LeftSurfingCharacter.transform.position.z);
				SetAnimation(PlayerBehaviorStatus.JetRise, 2);
			}
			else
			{
				LeftSurfingCharacter.transform.position = new Vector3(LeftSurfingCharacter.transform.position.x + (m_surfingHorizontalSpeed * Time.deltaTime),
					LeftSurfingCharacter.transform.position.y, LeftSurfingCharacter.transform.position.z);
			}

			if (LeftSurfingCharacter.transform.position.x > END_LEFT_SURFING_POINT_X)
			{
				LeftSurfingCharacter.transform.position = m_leftSurfingStartPos;
				m_isLeftSurfing = false;
				m_endTitle = true;
			}
		}
		LeftSurfingJetWater(m_isLeftSurfing);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 右のサーファーのジェットの起動
	/// </summary>
	/// <param name="jetState">起動か停止か</param>
	void RightSurfingJetWater(bool jetState)
	{
		if (jetState != m_rightJetWaterState)
		{
			RightMyJetWater.JetFire(jetState);
			m_rightJetWaterState = jetState;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左のサーファーのジェットの起動
	/// </summary>
	/// <param name="jetState">起動か停止か</param>
	void LeftSurfingJetWater(bool jetState)
	{
		if (jetState != m_leftJetWaterState)
		{
			LeftMyJetWater.JetFire(jetState);
			m_leftJetWaterState = jetState;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左のサーファーの星目の位置調整
	/// </summary>
	/// <param name="b">左のサーファーが飛んでいるか</param>
	void ChangeStarEyesPosition(bool b)
	{
		if (b == true)
		{
			StarEyesEffectObject.transform.parent = StarEyesEffectFlyingPosition.transform;
			StarEyesEffectObject.transform.position = StarEyesEffectFlyingPosition.position;
		}
		else
		{
			StarEyesEffectObject.transform.parent = StarEyesEffectNomalPosition.transform;
			StarEyesEffectObject.transform.position = StarEyesEffectNomalPosition.position;
		}
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
		StartMessage.color =
			new Color(StartMessage.color.r, StartMessage.color.g, StartMessage.color.b, m_messagAlphaColor);

		//メッセージ
		m_elapsedTimeForMessage += Time.deltaTime;
		Message.rectTransform.localScale =
			Vector3.one * m_messageScale * (1 + Mathf.Cos(m_elapsedTimeForMessage * m_messageSpeed)) / 2 + m_messageMinSize;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイトルでの一連の動きが終了した後の動作
	/// </summary>
	void WaitingRestart()
	{
		m_elapsedTime += Time.deltaTime;
		if (m_elapsedTime > m_waitingRestartTime)
		{
			m_elapsedTime = 0;
			m_endTitle = false;
			m_countNumOfLoops++;

			if (m_countNumOfLoops >= m_numOfLoops)
				MySceneManager.Instance.ChangeScene(MyScene.Pv);
			else
				Restart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期状態へ再設定する
	/// </summary>
	void Restart()
	{
		//手前のキャラクターの状態
		m_changeStarEye = false;
		m_rotationNum = 0;
		FrontNomalMask.SetActive(true);
		FrontStarEyeMask.SetActive(false);
		FrontCharacter.transform.eulerAngles = new Vector3(0, m_frontStartRotationY, 0);
		SetAnimation(PlayerBehaviorStatus.Walk, 0);
		m_startFrontCoroutine = false;

		//右のサーファーの状態
		m_isRightSurfing = true;
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 1);

		//左のサーファーの状態
		m_endFrontMove = false;
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 2);
		ChangeStarEyesPosition(false);
	}
}
