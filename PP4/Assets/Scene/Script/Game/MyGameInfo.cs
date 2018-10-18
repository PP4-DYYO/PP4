////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018年7月27日～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
//Enum・Struct
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// PlayerPrefsのキー
/// </summary>
public struct PlayerPrefsKeys
{
	/// <summary>
	/// Trueの数字
	/// </summary>
	public const int TRUE = 1;

	/// <summary>
	/// Falseの数字
	/// </summary>
	public const int FALSE = 0;

	/// <summary>
	/// キャラクターの人数
	/// </summary>
	public const string NUM_OF_CHARACTERS = "NumOfCharacters";

	/// <summary>
	/// キャラクター種類番号
	/// </summary>
	public const string CHARACTER_TYPE_NUM = "CharacterTypeNum";

	/// <summary>
	/// キャラクター名
	/// </summary>
	public const string CHARACTER_NAME = "CharacterName";

	/// <summary>
	/// キャラクターランク
	/// </summary>
	public const string CHARACTER_RANK = "CharacterRank";

	/// <summary>
	/// キャラクター経験値
	/// </summary>
	public const string CHARACTER_EXP = "CharacterExp";

	/// <summary>
	/// キャラクターのパワー
	/// </summary>
	public const string CHARACTER_POWER = "CharacterPower";
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 開示するキャラクター情報
/// </summary>
public struct DisclosedCharacterInfo
{
	/// <summary>
	/// 種類番号
	/// </summary>
	public int typeNum;

	/// <summary>
	/// 名前
	/// </summary>
	public string name;
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// ゲーム情報
/// </summary>
public class MyGameInfo : MySingleton<MyGameInfo>
{
	/// <summary>
	/// キャラクター番号
	/// </summary>
	int m_characterNum;

	/// <summary>
	/// 種類番号
	/// </summary>
	int m_typeNum = 0;

	/// <summary>
	/// プレイヤー名
	/// </summary>
	string m_playerName = "たけし";
	public string PlayerName
	{
		get { return m_playerName; }
		set { m_playerName = value; }
	}

	/// <summary>
	/// ランク
	/// </summary>
	int m_rank = 1;
	public int Rank
	{
		get { return m_rank; }
		set { m_rank = value; }
	}

	/// <summary>
	/// 経験値
	/// </summary>
	int m_exp = 0;
	public int Exp
	{
		get { return m_exp; }
		set { m_exp = value; }
	}

	/// <summary>
	/// パワー
	/// </summary>
	int m_power = 0;
	public int Power
	{
		get { return m_power; }
		set { m_power = value; }
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// データをリセットする
	/// </summary>
	public void ResetData()
	{
		//PlayerPrefsに保存されているプレイヤーのリセット
		for (var i = 0; i < PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS); i++)
		{
			//種類番号と名前とランクと経験値とパワーの初期化
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + i, 0);
			PlayerPrefs.SetString(PlayerPrefsKeys.CHARACTER_NAME + i, "");
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_RANK + i, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_EXP + i, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_POWER + i, 0);
		}

		//保存人数のリセット
		PlayerPrefs.SetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS, 0);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの登録
	/// </summary>
	/// <param name="typeNum">種類番号</param>
	/// <param name="name">名前</param>
	/// <param name="rank">ランク</param>
	/// <param name="exp">経験値</param>
	/// <param name="power">パワー</param>
	public void CharacterRegistration(int typeNum, string name, int rank = 1, int exp = 0, int power = 0)
	{
		//登録されているキャラクター数
		var numOfCharacters = PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS);

		//種類番号と名前とランクと経験値とパワーの登録
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + numOfCharacters, typeNum);
		PlayerPrefs.SetString(PlayerPrefsKeys.CHARACTER_NAME + numOfCharacters, name);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_RANK + numOfCharacters, rank);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_EXP + numOfCharacters, exp);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_POWER + numOfCharacters, power);

		//登録したキャラクター数
		PlayerPrefs.SetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS, numOfCharacters + 1);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを設定
	/// </summary>
	/// <param name="characterNum">キャラクター番号</param>
	public void SetCharacter(int characterNum = int.MaxValue)
	{
		//最後のプレイヤー
		if (characterNum == int.MaxValue)
			characterNum = PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS) - 1;

		//変数に設定
		m_typeNum = PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + characterNum);
		m_playerName = PlayerPrefs.GetString(PlayerPrefsKeys.CHARACTER_NAME + characterNum);
		m_rank = PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_RANK + characterNum);
		m_exp = PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_EXP + characterNum);
		m_power = PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_POWER + characterNum);

		m_characterNum = characterNum;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターを読み込む
	/// </summary>
	/// <returns>開示するオブジェクト情報たち</returns>
	public DisclosedCharacterInfo[] LoadCharacters()
	{
		//保存キャラクターなし
		if (PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS) <= 0)
			return null;

		//取得するキャラクター
		var characters = new DisclosedCharacterInfo[PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS)];

		//保存されている全キャラクター
		for(var i = 0; i < characters.Length; i++)
		{
			//開示情報の取得
			characters[i].typeNum = PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + i);
			characters[i].name = PlayerPrefs.GetString(PlayerPrefsKeys.CHARACTER_NAME + i);
		}

		return characters;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// キャラクターの削除
	/// </summary>
	/// <param name="characterNum">キャラクター番号</param>
	public void DeleteCharacter(int characterNum)
	{
		int index;

		//PlayerPrefsに保存されているプレイヤー
		for (index = characterNum; index < PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS) - 1; index++)
		{
			//データ（種類番号と名前とランクと経験値とパワー）を詰める
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + index, PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + (index + 1)));
			PlayerPrefs.SetString(PlayerPrefsKeys.CHARACTER_NAME + index, PlayerPrefs.GetString(PlayerPrefsKeys.CHARACTER_NAME + (index + 1)));
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_RANK + index, PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_RANK + (index + 1)));
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_EXP + index, PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_EXP + (index + 1)));
			PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_POWER + index, PlayerPrefs.GetInt(PlayerPrefsKeys.CHARACTER_POWER + (index + 1)));
		}

		//最後のデータをリセット
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_TYPE_NUM + index, 0);
		PlayerPrefs.SetString(PlayerPrefsKeys.CHARACTER_NAME + index, "");
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_RANK + index, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_EXP + index, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_POWER + index, 0);

		//保存人数を減らす
		PlayerPrefs.SetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS, PlayerPrefs.GetInt(PlayerPrefsKeys.NUM_OF_CHARACTERS) - 1);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 変数の保存
	/// </summary>
	public void SaveVariable()
	{
		//ランクと経験値とパワーの保存
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_RANK + m_characterNum, m_rank);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_EXP + m_characterNum, m_exp);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CHARACTER_POWER + m_characterNum, m_power);
	}
}
