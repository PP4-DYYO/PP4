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
	Vector3 m_surfingStartPos;

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
	bool endLeftSurfingMove;

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

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート
	/// </summary>
	void Start()
	{
		//手前のキャラクターの状態
		FrontStartPos = FrontCharacter.transform.position.x;
		FrontNomalMask.SetActive(true);
		FrontStarEyeMask.SetActive(false);

		//右のサーファーの状態
		m_surfingStartPos = RightSurfingCharacter.transform.position;
		m_isSurfing = true;
		SetAnimation(PlayerBehaviorStatus.Idle, 1);
		SetAnimation(PlayerBehaviorStatus.Idle, 2);

		//左のサーファーの状態
		endLeftSurfingMove = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タイトル画面での動き
	/// </summary>
	void Update()
	{
		MessageColorChange();
		FrontCharacterMove();
		SurfingCharacterMove();
		frontSufingCharacterMove();

		//右からのサーファーの移動終了後
		if (RightSurfingCharacter.transform.position.x < END_POINT_X)
		{
			//右のサーファーの位置を初期位置に戻す
			m_isSurfing = false;
			RightSurfingCharacter.transform.position = m_surfingStartPos;

			//手前のキャラクターを星目に変え、エフェクトを発生
			endLeftSurfingMove = true;
			FrontNomalMask.SetActive(false);
			FrontStarEyeMask.SetActive(true);
			GameObject el = Instantiate(StarEyesEffect, FrontCharacter.transform);
			el.transform.position = StarEyesPosition.transform.position;

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
	/// 手前のキャラクターの動きの制御
	/// </summary>
	private IEnumerator LeaveCharacter()
	{
		yield return new WaitForSeconds(1.0f);
		m_frontLeave = true;
		yield return new WaitForSeconds(1.5f);
		m_frontSufingStart = true;
		yield break;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 手前のキャラクターの動き
	/// </summary>
	void FrontCharacterMove()
	{
		if (FrontCharacter.transform.position.x < FrontStopPosObj.transform.position.x && !m_frontLeave && !endLeftSurfingMove)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x + m_frontMovingSpeed,
				FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);
		}

		if (m_frontLeave)
		{
			FrontCharacter.transform.position = new Vector3(FrontCharacter.transform.position.x - m_frontMovingSpeed,
							FrontCharacter.transform.position.y, FrontCharacter.transform.position.z);

			if (FrontCharacter.transform.position.x < FrontStartPos)
			{
				m_frontLeave = false;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 右からのサーファーの動き
	/// </summary>
	void SurfingCharacterMove()
	{
		if (m_isSurfing)
		{
			if (RightSurfingCharacter.transform.position.x < 0)
			{
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - m_surfingSpeed,
				RightSurfingCharacter.transform.position.y + m_surfingSpeed, RightSurfingCharacter.transform.position.z);

				SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 1);
			}
			else
			{
				RightSurfingCharacter.transform.position = new Vector3(RightSurfingCharacter.transform.position.x - m_surfingSpeed,
				RightSurfingCharacter.transform.position.y, RightSurfingCharacter.transform.position.z);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 左からのサーファーの動き
	/// </summary>
	void frontSufingCharacterMove()
	{
		if (m_frontSufingStart)
		{
			LeftSurfingCharacter.transform.position = new Vector3(LeftSurfingCharacter.transform.position.x + m_surfingSpeed,
				LeftSurfingCharacter.transform.position.y, LeftSurfingCharacter.transform.position.z);
			if (LeftSurfingCharacter.transform.position.x > m_surfingStartPos.x)
			{
				LeftSurfingCharacter.transform.position = new Vector3(-RightSurfingCharacter.transform.position.x,
				RightSurfingCharacter.transform.position.y, RightSurfingCharacter.transform.position.z);
				m_frontSufingStart = false;
				Start();
			}
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
			m_messagAlphaColor -= m_ChangeAlphaColorSpeed;
		}
		else if (m_messagAlphaColor < 1 && m_messagAlphaColorPlus)
		{
			m_messagAlphaColor += m_ChangeAlphaColorSpeed;
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
