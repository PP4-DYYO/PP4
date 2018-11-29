////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/22～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 武装モード
/// </summary>
enum ArmedMode
{
	/// <summary>
	/// キャラクターの選択
	/// </summary>
	SelectCharacter,
	/// <summary>
	/// キャラクターの生成
	/// </summary>
	CreateCharacter,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 武装のキャンバス
/// </summary>
public class MyArmedCanvas : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// 武装用のカメラ
	/// </summary>
	[SerializeField]
	Camera ArmedCamera;

	/// <summary>
	/// キャラクター選択のタイトル
	/// </summary>
	[SerializeField]
	GameObject CharacterSelectionTitle;

	/// <summary>
	/// キャラクター作成のタイトル
	/// </summary>
	[SerializeField]
	GameObject TitleOfCharacterCreation;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	[SerializeField]
	InputField PlayerName;

	/// <summary>
	/// 名前生成器
	/// </summary>
	[SerializeField]
	Button NameGenerator;

	/// <summary>
	/// キャラクター収集物
	/// </summary>
	[SerializeField]
	MyArmedCharacters Characters;

	/// <summary>
	/// キャラクターの種類
	/// </summary>
	[SerializeField]
	GameObject[] CharactersKind;

	/// <summary>
	/// ゴミ箱
	/// </summary>
	[SerializeField]
	Transform GarbageCan;

	/// <summary>
	/// 前を表示するボタン
	/// </summary>
	[SerializeField]
	Button ButtonToDisplayPrev;

	/// <summary>
	/// 次を表示するボタン
	/// </summary>
	[SerializeField]
	Button ButtonToDisplayNext;

	/// <summary>
	/// キャラクター生成器
	/// </summary>
	[SerializeField]
	Button CharacterGenerator;

	/// <summary>
	/// ゲームをスタートするボタン
	/// </summary>
	[SerializeField]
	Button ButtonToStartGame;
	#endregion

	#region モードの共通
	[Header("モードの共通")]
	/// <summary>
	/// モード
	/// </summary>
	ArmedMode m_mode;

	/// <summary>
	/// 表示中のキャラクター番号
	/// </summary>
	int m_characterNumBeingDisplayed;
	#endregion

	#region 選択画面
	[Header("選択画面")]
	/// <summary>
	/// 削除するための時間
	/// </summary>
	[SerializeField]
	float m_timeToDelete;

	/// <summary>
	/// 削除するための時間を数える
	/// </summary>
	float m_countTimeToDelete;
	#endregion

	#region 生成画面
	[Header("生成画面")]
	/// <summary>
	/// サンプルのプレイヤー名たち
	/// </summary>
	[SerializeField]
	string[] m_samplePlayerNames;

	/// <summary>
	/// 禁止文字
	/// </summary>
	[SerializeField]
	char m_prohibitedChar;

	/// <summary>
	/// 置き換え文字
	/// </summary>
	[SerializeField]
	char m_replacementChar;

	/// <summary>
	/// キャラクターを選択するために必要な数
	/// </summary>
	[SerializeField]
	int m_numNeededToSelectCharacter;

	/// <summary>
	/// 名前生成器の番号
	/// </summary>
	int m_nameGeneratorNum;
	#endregion

	#region キーボード関係
	[Header("キーボード関係")]
	/// <summary>
	/// Aボタンを押した
	/// </summary>
	bool m_isAButtonDown;

	/// <summary>
	/// Bボタンを押した
	/// </summary>
	bool m_isBButtonDown;

	/// <summary>
	/// Xボタンを押した
	/// </summary>
	bool m_isXButtonDown;

	/// <summary>
	/// Xボタンを押し続ける
	/// </summary>
	bool m_isKeepressingXButton;

	/// <summary>
	/// バックボタンを押した
	/// </summary>
	bool m_isBackButtonDown;

	/// <summary>
	/// ホームボタンを押した
	/// </summary>
	bool m_isHomeButtonDown;

	/// <summary>
	/// Mキーを押した
	/// </summary>
	bool m_isMKeyDown;

	/// <summary>
	/// 左コントロールキーを押し続ける
	/// </summary>
	bool m_isKeepressingLCtrlKey;

	/// <summary>
	/// DパッドXがポジティブになった
	/// </summary>
	bool m_isDpadXBecamePositive;

	/// <summary>
	/// フレーム前にDパッドXがポジティブになった
	/// </summary>
	bool m_isDpadXBecamePositivePrev;

	/// <summary>
	/// DパッドXがネガティブになった
	/// </summary>
	bool m_isDpadXBecameNegative;

	/// <summary>
	/// フレーム前にDパッドXがネガティブになった
	/// </summary>
	bool m_isDpadXBecameNegativePrev;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		ArmedCamera.enabled = false;
	}
	
	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		InputProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力処理
	/// </summary>
	void InputProcess()
	{
		//押したタイミングの取得
		if (Input.GetButtonDown("AButton"))
			m_isAButtonDown = true;
		if (Input.GetButtonDown("BButton"))
			m_isBButtonDown = true;
		if (Input.GetButtonDown("XButton"))
			m_isXButtonDown = true;
		if (Input.GetButtonDown("BackButton"))
			m_isBackButtonDown = true;
		if (Input.GetButtonDown("HomeButton"))
			m_isHomeButtonDown = true;
		if (Input.GetKeyDown(KeyCode.M))
			m_isMKeyDown = true;
		if (Input.GetAxis("DpadX") > 0 && !m_isDpadXBecamePositivePrev)
			m_isDpadXBecamePositive = true;
		m_isDpadXBecamePositivePrev = (Input.GetAxis("DpadX") > 0);
		if (Input.GetAxis("DpadX") < 0 && !m_isDpadXBecameNegativePrev)
			m_isDpadXBecameNegative = true;
		m_isDpadXBecameNegativePrev = (Input.GetAxis("DpadX") < 0);

		//押し続けている
		m_isKeepressingXButton = (Input.GetButton("XButton"));
		m_isKeepressingLCtrlKey = (Input.GetKey(KeyCode.LeftControl));
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//モード
		switch (m_mode)
		{
			case ArmedMode.SelectCharacter:
				SelectCharacterProcess();
				break;
			case ArmedMode.CreateCharacter:
				CreateCharacterProcess();
				break;
		}

		//共通の処理
		CommonProcess();

		//入力のリセット
		ResetInput();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを選択する処理
	/// </summary>
	void SelectCharacterProcess()
	{
		//左を押すと前のキャラクター表示
		if (m_isDpadXBecameNegative)
			OnClickButtonToDisplayPrev();

		//右を押すと次のキャラクター表示
		if (m_isDpadXBecamePositive)
			OnClickButtonToDisplayNext();

		//バックボタンでキャラクター生成画面
		if (m_isBackButtonDown)
			OnClickCharacterGenerator();

		//ホームボタンでゲームスタート
		if (m_isHomeButtonDown)
			OnClickButtonToStartGame();

		//Xボタンで削除時間初期化
		if (m_isXButtonDown)
			m_countTimeToDelete = 0;

		//Bボタン長押しで削除時間を増やす
		if (m_isKeepressingXButton)
		{
			//プレイヤーが消えかける
			m_countTimeToDelete += Time.deltaTime;
			Characters.AnimCharacter(PlayerBehaviorStatus.BeFrightened, m_characterNumBeingDisplayed);
			Characters.MakeCharacterTransparent(1 - (m_countTimeToDelete / m_timeToDelete));
		}
		else
		{
			//キャラクターは常にSelect状態
			Characters.AnimCharacter(PlayerBehaviorStatus.Select, m_characterNumBeingDisplayed);
			Characters.MakeCharacterTransparent(1);
		}

		//削除時間を超えたらキャラクターの削除
		if (m_countTimeToDelete >= m_timeToDelete)
			DeleteCharacter();

		//キャラクターの数が少ない
		ButtonToDisplayPrev.interactable = (Characters.transform.childCount >= m_numNeededToSelectCharacter);
		ButtonToDisplayNext.interactable = (Characters.transform.childCount >= m_numNeededToSelectCharacter);

		//選択するキャラクターがいる
		ButtonToStartGame.interactable = (Characters.transform.childCount != 0);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの削除
	/// </summary>
	void DeleteCharacter()
	{
		//削除するキャラクターがいない
		if (Characters.transform.childCount <= 0)
			return;

		//データの削除
		MyGameInfo.Instance.DeleteCharacter(m_characterNumBeingDisplayed);

		//時間の初期化
		m_countTimeToDelete = 0;

		//InputFieldの初期化
		PlayerName.text = "";
		PlayerName.placeholder.GetComponent<Text>().enabled = false;

		//選択画面を再読み込み
		DisplayOfCharacterSelectionScreen();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクター選択画面の表示
	/// </summary>
	void DisplayOfCharacterSelectionScreen()
	{
		m_mode = ArmedMode.SelectCharacter;

		//非表示オブジェクト
		TitleOfCharacterCreation.SetActive(false);
		PlayerName.placeholder.GetComponent<Text>().enabled = false;
		NameGenerator.gameObject.SetActive(false);

		//表示オブジェクト
		CharacterSelectionTitle.SetActive(true);
		CharacterGenerator.gameObject.SetActive(true);

		//設定
		PlayerName.interactable = false;
		m_characterNumBeingDisplayed = 0;

		//保存キャラクターの読み込み
		LoadingSavedCharacters();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 保存されたキャラクターの読み込み
	/// </summary>
	void LoadingSavedCharacters()
	{
		//キャラクターズのリセット
		ResetCharacters();

		//保存されたキャラ情報の取得
		var characters = MyGameInfo.Instance.LoadCharacters();

		//読み込みキャラクターなし
		if (characters == null)
			return;

		//読み込みキャラクターの生成
		foreach (var character in characters)
		{
			var newCharacter = Instantiate(CharactersKind[character.typeNum], Characters.transform);
			newCharacter.name = character.name;
		}

		//初期表示
		Characters.DisplayCharacters(0, true, true, true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターズのリセット
	/// </summary>
	void ResetCharacters()
	{
		//全子供の削除
		for (var i = 0; i < Characters.transform.childCount;)
		{
			//削除用Transform
			Characters.transform.GetChild(i).parent = GarbageCan;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを生成する処理
	/// </summary>
	void CreateCharacterProcess()
	{
		//Aボタンで名前生成
		if (m_isAButtonDown)
			OnClickNameGenerator();

		//左を押すと前のキャラクター表示
		if (m_isDpadXBecameNegative)
			OnClickButtonToDisplayPrev();

		//右を押すと次のキャラクター表示
		if (m_isDpadXBecamePositive)
			OnClickButtonToDisplayNext();

		//名前が入力されているandホームボタンでゲームスタート
		if (!(System.String.IsNullOrEmpty(PlayerName.text)) && ((m_isHomeButtonDown && !m_isMKeyDown) || (m_isMKeyDown && m_isKeepressingLCtrlKey)))
			OnClickButtonToStartGame();

		//キャラクターは常にSelect状態
		Characters.AnimCharacter(PlayerBehaviorStatus.Select, m_characterNumBeingDisplayed);

		//キャラクターの種類が少ない
		ButtonToDisplayPrev.interactable = (CharactersKind.Length >= m_numNeededToSelectCharacter);
		ButtonToDisplayNext.interactable = (CharactersKind.Length >= m_numNeededToSelectCharacter);

		//プレイヤー名が何もないとスタートボタンが無効
		ButtonToStartGame.interactable = !(System.String.IsNullOrEmpty(PlayerName.text));
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 共通の処理
	/// </summary>
	void CommonProcess()
	{
		//Bボタンでタイトルへ
		if (m_isBButtonDown)
			MySceneManager.Instance.ChangeScene(MyScene.Title);

		//ゴミ箱のリセット
		ResetGarbageCan();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゴミ箱のリセット
	/// </summary>
	void ResetGarbageCan()
	{
		//全子供を削除
		foreach(Transform child in GarbageCan)
		{
			Destroy(child.gameObject);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 入力のリセット
	/// </summary>
	void ResetInput()
	{
		m_isAButtonDown = false;
		m_isBButtonDown = false;
		m_isXButtonDown = false;
		m_isBackButtonDown = false;
		m_isHomeButtonDown = false;
		m_isMKeyDown = false;
		m_isDpadXBecamePositive = false;
		m_isDpadXBecameNegative = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 表示
	/// </summary>
	public void Display()
	{
		ArmedCamera.enabled = true;
		DisplayOfCharacterSelectionScreen();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 名前生成器をクリック
	/// </summary>
	public void OnClickNameGenerator()
	{
		//名前生成器番号のループ
		if (m_nameGeneratorNum >= m_samplePlayerNames.Length)
			m_nameGeneratorNum = 0;

		//名前の代入
		PlayerName.text = m_samplePlayerNames[m_nameGeneratorNum++];
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤー名を編集し終えた
	/// </summary>
	public void OnEndEditPlayerName()
	{
		//禁止文字を置き換える
		PlayerName.text = PlayerName.text.Replace(m_prohibitedChar, m_replacementChar);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 前の表示するボタンをクリック
	/// </summary>
	public void OnClickButtonToDisplayPrev()
	{
		m_characterNumBeingDisplayed =
			((m_characterNumBeingDisplayed - 1) + Characters.transform.childCount) % Characters.transform.childCount;
		Characters.DisplayCharacters(m_characterNumBeingDisplayed, true, (m_mode == ArmedMode.SelectCharacter));
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 次の表示するボタンをクリック
	/// </summary>
	public void OnClickButtonToDisplayNext()
	{
		m_characterNumBeingDisplayed =
			((m_characterNumBeingDisplayed + 1) + Characters.transform.childCount) % Characters.transform.childCount;
		Characters.DisplayCharacters(m_characterNumBeingDisplayed, false, (m_mode == ArmedMode.SelectCharacter));
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクター生成器をクリック
	/// </summary>
	public void OnClickCharacterGenerator()
	{
		//キャラクター生成画面の表示
		DisplayOfCharacterGenerationScreen();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクター生成画面の表示
	/// </summary>
	void DisplayOfCharacterGenerationScreen()
	{
		m_mode = ArmedMode.CreateCharacter;

		//非表示オブジェクト
		CharacterSelectionTitle.SetActive(false);
		CharacterGenerator.gameObject.SetActive(false);

		//表示オブジェクト
		TitleOfCharacterCreation.SetActive(true);
		PlayerName.placeholder.GetComponent<Text>().enabled = true;
		NameGenerator.gameObject.SetActive(true);

		//設定
		PlayerName.interactable = true;
		PlayerName.text = "";
		m_characterNumBeingDisplayed = 0;

		//選択するキャラクターの読み込み
		LoadSelectedCharacters();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 選択するキャラクターの読み込み
	/// </summary>
	void LoadSelectedCharacters()
	{
		//キャラクターズのリセット
		ResetCharacters();

		//読み込みキャラクターの生成
		foreach (var character in CharactersKind)
		{
			Instantiate(character, Characters.transform);
		}

		//初期表示
		Characters.DisplayCharacters(0, true, false, true);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲームをスタートするボタンをクリック
	/// </summary>
	public void OnClickButtonToStartGame()
	{
		//モード
		switch (m_mode)
		{
			case ArmedMode.SelectCharacter:
				MyGameInfo.Instance.SetCharacter(m_characterNumBeingDisplayed);
				break;
			case ArmedMode.CreateCharacter:
				SaveCharacter();
				MyGameInfo.Instance.SetCharacter();
				break;
		}

		//共通処理
		MySceneManager.Instance.ChangeScene(MyScene.Matching);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを保存
	/// </summary>
	void SaveCharacter()
	{
		//キャラクター登録
		MyGameInfo.Instance.CharacterRegistration(m_characterNumBeingDisplayed, PlayerName.text);
	}
}
