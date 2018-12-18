////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/12/11～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCreditManager : MonoBehaviour
{
	/// <summary>
	/// 背景
	/// </summary>
	[SerializeField]
	GameObject BackGround;

	/// <summary>
	/// 背景の位置
	/// </summary>
	Vector3 BackGroundPosition;

	/// <summary>
	/// 中央のキャラクター
	/// </summary>
	[SerializeField]
	GameObject CreditCharacter;

	/// <summary>
	/// 中央のキャラクター移動速度
	/// </summary>
	[SerializeField]
	float m_creditCharacterSpeed;

	/// <summary>
	/// 中央のキャラクターの位置
	/// </summary>
	Vector3 CreditCharacterPosition;

	/// <summary>
	/// 中央のキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator CreditCharacterAnim;

	/// <summary>
	/// 中央のキャラクターの回転軸のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject AxisObject;

	/// <summary>
	/// オブジェクトの移動速度
	/// </summary>
	const float SPEED = 20f;

	/// <summary>
	/// 左側のキャラクター
	/// </summary>
	[SerializeField]
	GameObject LeftCharacter;

	/// <summary>
	/// 左側のキャラクターの位置
	/// </summary>
	Vector3 LeftCharacterPosition;

	/// <summary>
	/// 左側のキャラクターのアニメーション
	/// </summary>
	[SerializeField]
	Animator LeftCharacterAnim;

	/// <summary>
	/// 左側のキャラクターの初期位置
	/// </summary>
	[SerializeField]
	GameObject LeftCharacterStartPositionObject;

	/// <summary>
	/// メインのカメラ
	/// </summary>
	[SerializeField]
	Camera CreditCamera;

	/// <summary>
	/// メインのカメラの位置
	/// </summary>
	Vector3 CameraPosition;

	/// <summary>
	/// メインのカメラの場所用オブジェクト
	/// </summary>
	[SerializeField]
	GameObject CameraPositionObject;

	/// <summary>
	/// 雲の右端
	/// </summary>
	[SerializeField]
	GameObject CloudRightEndObject;

	/// <summary>
	/// 画面外の位置
	/// </summary>
	[SerializeField]
	GameObject DisplayOutsideObject;

	/// <summary>
	/// 雲のα値
	/// </summary>
	float CloudAlpha;

	/// <summary>
	/// テキストの移動するスピード
	/// </summary>
	[SerializeField]
	float m_textSpeed;

	/// <summary>
	/// テキストの移動スタート位置
	/// </summary>
	[SerializeField]
	GameObject TextStartPositionObject;

	/// <summary>
	/// テキストを配列で保持
	/// </summary>
	[SerializeField]
	GameObject[] CreditText = new GameObject[14];

	/// <summary>
	/// テキストを配列で保持
	/// </summary>
	const int MAX_TEXT_NUM = 13;

	/// <summary>
	/// テキストの下側停止位置
	/// </summary>
	[SerializeField]
	GameObject TextStopPositionObject;

	/// <summary>
	/// 表示するテキストの番号
	/// </summary>
	int m_textNum;

	/// <summary>
	/// 次へ進み始めた
	/// </summary>
	bool m_textChanged;

	/// <summary>
	/// テキストが動く
	/// </summary>
	bool m_textMoving;

	/// <summary>
	/// 左のキャラクターが動きおわった
	/// </summary>
	bool m_leftCharacterMoved;

	/// <summary>
	/// イベントが動いているか
	/// </summary>
	bool m_eventPlaying;

	/// <summary>
	/// 隕石のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject Meteorite;

	/// <summary>
	/// 隕石の位置
	/// </summary>
	Vector3 MeteoritePosition;

	/// <summary>
	/// 隕石の初期位置
	/// </summary>
	[SerializeField]
	GameObject MeteoriteStartPositionObject;

	/// <summary>
	/// 隕石が落ちた
	/// </summary>
	bool m_fallDownMeteorite;

	/// <summary>
	/// 雷のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject Thunder;

	/// <summary>
	/// 雷のレンダラー
	/// </summary>
	[SerializeField]
	SpriteRenderer ThunderRenderer;

	// <summary>
	/// 雷のレンダラーのα値
	/// </summary>
	float ThunderRendererAlpha;

	// <summary>
	/// 最大のα値
	/// </summary>
	const float ALPHA_MAX = 1;

	// <summary>
	/// 雷のレンダラーのα値の変更速度
	/// </summary>
	[SerializeField]
	float m_ChangeAlphaSpeed;

	/// <summary>
	/// 雷のフラッシュ
	/// </summary>
	[SerializeField]
	GameObject ThunderFlash;

	/// <summary>
	/// 雷のフラッシュのレンダラー
	/// </summary>
	[SerializeField]
	SpriteRenderer ThunderFlashRenderer;

	/// <summary>
	/// 雷の色
	/// </summary>
	Color ThunderColor;

	/// <summary>
	/// 雷用の時間
	/// </summary>
	float m_thunderTime;

	/// <summary>
	/// 雷のフラッシュの時間間隔
	/// </summary>
	const float THUNDER_FLASH_INTERVAL = 20;

	/// <summary>
	/// 雷のフラッシュのカウント
	/// </summary>
	int m_thunderFlashCount;

	/// <summary>
	/// 雷のフラッシュの制限回数
	/// </summary>
	const int THUNDER_FLASH_COUNT_LIMIT = 4;

	/// <summary>
	/// 雷が落ちた
	/// </summary>
	bool m_lightningStruck;

	/// <summary>
	/// 雷用の2
	/// </summary>
	const int THUNDER_TWICE = 2;

	/// <summary>
	/// 雲のパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem ParticleCloud;

	/// <summary>
	/// 雲のオブジェクト
	/// </summary>
	[SerializeField]
	GameObject CloudObject;

	/// <summary>
	/// 雲の時間
	/// </summary>
	float m_cloudTime;

	/// <summary>
	/// 雲が消えるまでの時間
	/// </summary>
	const float CLOUD_LIMIT_TIME = 3f;

	/// <summary>
	/// 雲の状態列挙
	/// </summary>
	enum CloudState
	{
		/// <summary>
		/// 動いていない
		/// </summary>
		Non,
		/// <summary>
		/// 生成
		/// </summary>
		Generate,
		/// <summary>
		/// 動きはじめる
		/// </summary>
		Play,
		/// <summary>
		/// 進行中
		/// </summary>
		Progress,
		/// <summary>
		/// 消えた
		/// </summary>
		Destroy
	}

	/// <summary>
	/// 雲の状態
	/// </summary>
	CloudState cloudState;

	/// <summary>
	/// 雲が消える原因列挙
	/// </summary>
	enum CloudDestroyCause
	{
		/// <summary>
		/// 消えていない
		/// </summary>
		Non,
		/// <summary>
		/// ジェット
		/// </summary>
		Jet,
		/// <summary>
		/// 隕石
		/// </summary>
		Meteorite,
		/// <summary>
		/// 雷
		/// </summary>
		Thunder
	}

	/// <summary>
	/// 雲が消える原因
	/// </summary>
	CloudDestroyCause cloudDestroyCause;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期設定
	/// </summary>
	void Start()
	{
		//アニメーションの初期設定
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 1);
		SetAnimation(PlayerBehaviorStatus.HorizontalMovement, 2);

		//開始前を設定
		m_textNum = -1;

		ThunderColor = ThunderRenderer.GetComponent<SpriteRenderer>().color;
		ThunderRendererAlpha = ThunderRenderer.color.a;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期更新
	/// </summary>
	void Update()
	{
		//カメラの移動
		CameraMoving();

		//中央のキャラクターの移動
		CreditCharacterMoving();

		//状況の確認
		CheckEventPlay();

		//背景の移動
		BackGroundMoving();

		//雲のイベント
		CloudEvent();

		//テキストの移動
		PlayTextEvent();

		//テキスト表示イベントを開始する

		if (Input.GetKeyDown(KeyCode.Space) && !m_eventPlaying)
		{
			cloudDestroyCause = CloudDestroyCause.Jet;
			cloudState = CloudState.Generate;
		}
		if (Input.GetKeyDown(KeyCode.Return) && !m_eventPlaying)
		{
			cloudDestroyCause = CloudDestroyCause.Meteorite;
			cloudState = CloudState.Generate;
		}
		if (Input.GetKeyDown(KeyCode.T) && !m_eventPlaying)
		{
			cloudDestroyCause = CloudDestroyCause.Thunder;
			cloudState = CloudState.Generate;
		}

		if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("BackButton"))
		{
			MySceneManager.Instance.ChangeScene(MyScene.Title);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 進行状況の確認
	/// </summary>
	void CheckEventPlay()
	{
		if (cloudDestroyCause != CloudDestroyCause.Non)
		{
			m_eventPlaying = true;
		}
		else
		{
			m_eventPlaying = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲の動き
	/// </summary>
	void CloudEvent()
	{
		switch (cloudState)
		{
			//雲が生成される
			case CloudState.Generate:
				if (m_cloudTime == 0)
				{
					ParticleCloud.Play();
					m_cloudTime += Time.deltaTime;
				}
				else
				{
					ParticleCloud.Pause();
				}
				break;

			//雲が動く
			case CloudState.Play:
				ParticleCloud.Play();
				cloudState = CloudState.Progress;
				break;

			//雲が動いている
			case CloudState.Progress:
				m_cloudTime += Time.deltaTime;
				if (m_cloudTime >= CLOUD_LIMIT_TIME)
				{
					cloudState = CloudState.Destroy;
					m_cloudTime = 0;
				}
				break;

			//雲が消えた
			case CloudState.Destroy:
				cloudState = CloudState.Non;
				m_textMoving = true;
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// テキストを表示するイベントの動き
	/// </summary>
	void PlayTextEvent()
	{
		switch (cloudDestroyCause)
		{
			case CloudDestroyCause.Jet:
				CloudJetDestroy();
				break;

			case CloudDestroyCause.Meteorite:
				CloudMeteoriteDestroy();
				break;

			case CloudDestroyCause.Thunder:
				CloudThunderDestroy();
				break;
		}

		if (m_textMoving)
		{
			TextMoving();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// テキストの動き
	/// </summary>
	void TextMoving()
	{
		//テキストが下に下がっていく
		CreditText[m_textNum].transform.position = Vector3.MoveTowards(CreditText[m_textNum].transform.position,
	TextStopPositionObject.transform.position, Time.deltaTime * m_textSpeed);

		//停止位置に着くとリセットする
		if (CreditText[m_textNum].transform.position.y <= TextStopPositionObject.transform.position.y)
		{
			switch (cloudDestroyCause)
			{
				case CloudDestroyCause.Jet:
					m_leftCharacterMoved = false;
					break;

				case CloudDestroyCause.Meteorite:
					m_fallDownMeteorite = false;
					break;

				case CloudDestroyCause.Thunder:
					m_thunderTime = 0;
					m_thunderFlashCount = 0;
					m_lightningStruck = false;
					break;
			}
			cloudState = CloudState.Non;
			cloudDestroyCause = CloudDestroyCause.Non;
			m_textChanged = false;
			m_textMoving = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// テキストを変更
	/// </summary>
	void TextChange()
	{
		if (m_textNum < MAX_TEXT_NUM)
		{
			m_textNum++;
			CreditText[m_textNum].transform.position = TextStartPositionObject.transform.position;
			m_textChanged = true;
		}
		else
		{
			Debug.Log("クレジット表示終了");
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲がジェットで消える一連の動き
	/// </summary>
	void CloudJetDestroy()
	{
		if (!m_textChanged)
		{
			TextChange();
		}

		if (!m_leftCharacterMoved)
		{
			//左側のキャラクターの動き
			LeftCharacterPosition = LeftCharacter.transform.position;
			LeftCharacterPosition.z += Time.deltaTime * SPEED;
			LeftCharacter.transform.position = LeftCharacterPosition;

			//雲の右端で雲が動き始める
			if (LeftCharacterPosition.z >= CloudRightEndObject.transform.position.z)
			{
				cloudState = CloudState.Play;
			}

			//画面外へ着くと初期位置へ戻す
			if (LeftCharacterPosition.z >= DisplayOutsideObject.transform.position.z)
			{
				m_leftCharacterMoved = true;
				LeftCharacter.transform.position = LeftCharacterStartPositionObject.transform.position;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲が隕石で消える一連の動き
	/// </summary>
	void CloudMeteoriteDestroy()
	{
		//表示するテキストを変更
		if (!m_textChanged)
		{
			TextChange();
		}

		//テキスト移動開始までは隕石が移動
		if (!m_fallDownMeteorite)
		{
			MeteoritePosition = Meteorite.transform.position;
			MeteoritePosition.y -= SPEED * Time.deltaTime;
			Meteorite.transform.position = MeteoritePosition;

			//隕石が雲の位置に着くと雲が分裂（現在は消える）
			if (Meteorite.transform.position.y <= CloudObject.transform.position.y)
			{
				cloudState = CloudState.Play;
			}

			//隕石が停止位置に着くと初期位置に戻りテキストの移動を始める
			if (Meteorite.transform.position.y <= TextStopPositionObject.transform.position.y)
			{
				Meteorite.transform.position = MeteoriteStartPositionObject.transform.position;
				m_fallDownMeteorite = true;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 雲が雷で消える一連の動き
	/// </summary>
	void CloudThunderDestroy()
	{
		if (!m_textMoving)
		{
			m_thunderTime++;

			//表示するテキストを変更
			if (!m_textChanged)
			{
				TextChange();
			}

			if (!m_lightningStruck)
			{
				//雷が落ちるまで点滅する
				if (m_thunderFlashCount < THUNDER_FLASH_COUNT_LIMIT)
				{
					if (m_thunderTime % THUNDER_FLASH_INTERVAL == 0 && m_thunderFlashCount % THUNDER_TWICE == 0)
					{
						ThunderFlash.SetActive(true);
						m_thunderFlashCount++;
					}
					if (m_thunderTime % (THUNDER_FLASH_INTERVAL * THUNDER_TWICE) == 0 && m_thunderFlashCount % THUNDER_TWICE != 0)
					{
						ThunderFlash.SetActive(false);
						m_thunderFlashCount++;
					}
				}
				else
				{
					//雷が落ちる（稲妻が出現する）
					Thunder.SetActive(true);
					m_lightningStruck = true;
					//落ちた時に雲は消え始める
					cloudState = CloudState.Play;
				}
			}
			else
			{
				//雷が落ちた後、稲妻の透明度を下げていく
				ThunderRendererAlpha -= m_ChangeAlphaSpeed;
				ThunderColor.a = ThunderRendererAlpha;
				ThunderRenderer.color = ThunderColor;

				if (ThunderRendererAlpha <= 0)
				{
					ThunderRendererAlpha = ALPHA_MAX;
					ThunderColor.a = ThunderRendererAlpha;

					ThunderRenderer.color = ThunderColor;

					Thunder.SetActive(false);
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 中央のキャラクターの移動
	/// </summary>
	void CreditCharacterMoving()
	{
		CreditCharacter.transform.RotateAround(AxisObject.transform.position, -Vector3.up, m_creditCharacterSpeed);
		CreditCharacter.transform.LookAt(AxisObject.transform.position);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 背景の移動
	/// </summary>
	void BackGroundMoving()
	{
		BackGroundPosition = BackGround.transform.position;
		BackGroundPosition.y -= Time.deltaTime * SPEED;

		BackGround.transform.position = BackGroundPosition;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// カメラの移動
	/// </summary>
	void CameraMoving()
	{
		CreditCamera.transform.position = CameraPositionObject.transform.position;
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
				CreditCharacterAnim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
			case 2:
				LeftCharacterAnim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
				break;
		}
	}
}
