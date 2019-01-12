////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/10/15～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// 武装のキャラクターズ
/// </summary>
public class MyArmedCharacters : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// プレイヤー名
	/// </summary>
	[SerializeField]
	InputField PlayerName;

	/// <summary>
	/// キャラクターのマテリアル
	/// </summary>
	[SerializeField]
	Material[] CharacterMaterials;
	#endregion

	#region キャラクターの移動関係
	[Header("キャラクターの移動関係")]
	/// <summary>
	/// キャラクターの中心位置
	/// </summary>
	[SerializeField]
	Vector3 m_playerCenterPos;

	/// <summary>
	/// プレイヤーの初期回転値
	/// </summary>
	[SerializeField]
	Vector3 m_playerInitRotation;

	/// <summary>
	/// プレイヤーの初期スケール
	/// </summary>
	[SerializeField]
	Vector3 m_playerInitScale;

	/// <summary>
	/// キャラクターの移動時間
	/// </summary>
	[SerializeField]
	float m_characterMovementTime;

	/// <summary>
	/// キャラクターの左の移動位置
	/// </summary>
	[SerializeField]
	Vector3 m_characterLeftMovementLocation;

	/// <summary>
	/// キャラクターの右の移動位置
	/// </summary>
	[SerializeField]
	Vector3 m_characterRightMovementLocation;

	/// <summary>
	/// 描画順番
	/// </summary>
	[SerializeField]
	int m_drawingOrder;

	/// <summary>
	/// キャラクター移動時間を数える
	/// </summary>
	float m_countCharacterMovementTime = -1;

	/// <summary>
	/// 選択されたキャラクター番号
	/// </summary>
	int m_selectedCharacterNum;

	/// <summary>
	/// 前に選択されたキャラクター番号
	/// </summary>
	int m_selectedCharacterNumPrev;

	/// <summary>
	/// 左表示フラグ
	/// </summary>
	bool m_isLeftDisplay;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用のFloat
	/// </summary>
	float m_workFloat;

	/// <summary>
	/// 作業用のColor
	/// </summary>
	Color m_workColor;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//キャラクターの移動
		if (m_countCharacterMovementTime != -1)
			MoveCharacter();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの移動
	/// </summary>
	void MoveCharacter()
	{
		m_countCharacterMovementTime += Time.deltaTime;

		//時間割合
		m_workFloat = m_countCharacterMovementTime / m_characterMovementTime;

		//表示するキャラクターの中心移動
		transform.GetChild(m_selectedCharacterNum).localPosition =
			Vector3.Lerp(m_isLeftDisplay ? m_characterRightMovementLocation : m_characterLeftMovementLocation, m_playerCenterPos, m_workFloat);

		//非表示キャラクターの端移動
		transform.GetChild(m_selectedCharacterNumPrev).localPosition =
			Vector3.Lerp(m_playerCenterPos, m_isLeftDisplay ? m_characterLeftMovementLocation : m_characterRightMovementLocation, m_workFloat);

		//時間が超えた
		if (m_countCharacterMovementTime >= m_characterMovementTime)
		{
			//選択中のキャラクター位置
			transform.GetChild(m_selectedCharacterNum).localPosition = m_playerCenterPos;

			//非表示のキャラクター
			transform.GetChild(m_selectedCharacterNumPrev).gameObject.SetActive(false);

			m_countCharacterMovementTime = -1;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		m_countCharacterMovementTime = -1;
		m_selectedCharacterNum = 0;
		m_selectedCharacterNumPrev = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの表示
	/// </summary>
	/// <param name="characterNum">キャラクター番号</param>
	/// <param name="isLeftDisplay">左表示フラグ</param>
	/// <param name="isDisplayName">名前の表示</param>
	/// <param name="isInit">キャラクターズの初期化</param>
	/// <returns>表示成功</returns>
	public bool DisplayCharacters(int characterNum, bool isLeftDisplay, bool isDisplayName = true, bool isInitCharacters = false)
	{
		//移動中
		if (m_countCharacterMovementTime != -1)
			return false;

		//初期化で子供なし
		if (isInitCharacters && transform.childCount == 0)
		{
			PlayerName.placeholder.GetComponent<Text>().enabled = false;
			return false;
		}

		//名前の表示
		if (isDisplayName)
			PlayerName.text = transform.GetChild(characterNum).name;

		//初期化
		if (isInitCharacters)
		{
			//キャラクターの初期化
			CharacterInitialization();

			//キャラクターの表示
			transform.GetChild(characterNum).gameObject.SetActive(true);

			return true;
		}

		//初期設定
		m_countCharacterMovementTime = 0;
		m_selectedCharacterNumPrev = m_selectedCharacterNum;
		m_selectedCharacterNum = characterNum;
		m_isLeftDisplay = isLeftDisplay;

		//キャラクターの表示
		transform.GetChild(characterNum).gameObject.SetActive(true);

		return true;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの初期化
	/// </summary>
	void CharacterInitialization()
	{
		//全ての子供
		foreach (Transform child in transform)
		{
			//Transformの初期化
			child.localPosition = m_playerCenterPos;
			child.eulerAngles = m_playerInitRotation;
			child.localScale = m_playerInitScale;

			//非表示
			child.gameObject.SetActive(false);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターをアニメーション
	/// </summary>
	/// <param name="state">状態</param>
	/// <param name="characterNum">キャラクター番号</param>
	public void AnimCharacter(PlayerBehaviorStatus state, int characterNum = int.MaxValue)
	{
		//子供なし
		if (transform.childCount == 0)
			return;

		//全キャラクターが対象
		if (characterNum == int.MaxValue)
		{
			//全子供
			foreach (Transform child in transform)
			{
				//キャラクターのアニメーション
				child.GetComponent<Animator>().SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
			}
		}
		else
		{
			//指定キャラクターのアニメーション
			transform.GetChild(characterNum).GetComponent<Animator>().SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを透明にする
	/// </summary>
	/// <param name="alphaValue">α値</param>
	public void MakeCharacterTransparent(float alphaValue)
	{
		//全マテリアル
		for (var i = 0; i < CharacterMaterials.Length; i++)
		{
			//α値の反映
			m_workColor = CharacterMaterials[i].color;
			m_workColor.a = alphaValue;
			CharacterMaterials[i].color = m_workColor;
		}
	}
}
