////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/11～
//制作者　京都コンピュータ学院京都駅前校　ゲーム学科　四回生　奥田裕也
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
/// BGM集
/// </summary>
public enum BgmCollection
{
	/// <summary>
	/// タイトル
	/// </summary>
	Title,
	/// <summary>
	/// 武装
	/// </summary>
	Armed,
	/// <summary>
	/// マッチング
	/// </summary>
	Matching,
	/// <summary>
	/// バトル
	/// </summary>
	Battle,
	/// <summary>
	/// 勝ち
	/// </summary>
	Win,
	/// <summary>
	/// 負け
	/// </summary>
	Defeat,
	/// <summary>
	/// クレジット
	/// </summary>
	Credit,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// SE集
/// </summary>
public enum SeCollection
{
	/// <summary>
	/// 選択
	/// </summary>
	Select,
	/// <summary>
	/// 決定
	/// </summary>
	Decide,
	/// <summary>
	/// キャンセル
	/// </summary>
	Cancel,
	/// <summary>
	/// 注意
	/// </summary>
	Note,
	/// <summary>
	/// 目が輝く
	/// </summary>
	EyesShine,
	/// <summary>
	/// 水噴射
	/// </summary>
	WaterInjection,
	/// <summary>
	/// 水しぶきの泡
	/// </summary>
	SplashedFoam,
	/// <summary>
	/// 足音
	/// </summary>
	Footsteps,
	/// <summary>
	/// 水しぶきが水面に落ちる
	/// </summary>
	SplashFallsOnSurfaceOfWater,
	/// <summary>
	/// キャラクターが消える
	/// </summary>
	CharacterDisappears,
	/// <summary>
	/// プレイヤーが入室
	/// </summary>
	PlayerEnters,
	/// <summary>
	/// プレイヤーが退出
	/// </summary>
	PlayerLeaves,
	/// <summary>
	/// 人が集まった
	/// </summary>
	PeopleGathered,
	/// <summary>
	/// レディー
	/// </summary>
	Ready,
	/// <summary>
	/// ゴー
	/// </summary>
	Go,
	/// <summary>
	/// 水を被る
	/// </summary>
	WearWater,
	/// <summary>
	/// 帯電
	/// </summary>
	Charging,
	/// <summary>
	/// 炎オーラ
	/// </summary>
	FlameAura,
	/// <summary>
	/// 雷オーラ
	/// </summary>
	LightningAura,
	/// <summary>
	/// 土オーラ
	/// </summary>
	EarthAura,
	/// <summary>
	/// 落下の始まり
	/// </summary>
	BeginningOfFall,
	/// <summary>
	/// 落下中
	/// </summary>
	Falling,
	/// <summary>
	/// コイン
	/// </summary>
	Coin,
	/// <summary>
	/// 嵐
	/// </summary>
	Storm,
	/// <summary>
	/// 雨雲
	/// </summary>
	RainCloud,
	/// <summary>
	/// 風雲
	/// </summary>
	WindCloud,
	/// <summary>
	/// 雷雲
	/// </summary>
	Thundercloud,
	/// <summary>
	/// 落雷
	/// </summary>
	Lightning,
	/// <summary>
	/// 隕石
	/// </summary>
	Meteorite,
	/// <summary>
	/// 隕石が破壊
	/// </summary>
	MeteoriteDestroyed,
	/// <summary>
	/// 空中ミサイル
	/// </summary>
	AirMissile,
	/// <summary>
	/// バトル終了
	/// </summary>
	BattleEnd,
	/// <summary>
	/// 着地成功
	/// </summary>
	SuccessfulLanding,
	/// <summary>
	/// 着地失敗
	/// </summary>
	LandingFailed,
	/// <summary>
	/// 結果
	/// </summary>
	Result,
	/// <summary>
	/// 経験値のアップ
	/// </summary>
	UpExp,
	/// <summary>
	/// レベルのアップ
	/// </summary>
	UpLv,
	/// <summary>
	/// プレイヤーが横切る
	/// </summary>
	PlayerCrosses,
	/// <summary>
	/// 船が横切る
	/// </summary>
	CrossShip,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 特別なSE集
/// </summary>
public enum SpecialSeCollection
{
	/// <summary>
	/// 泡
	/// </summary>
	Bubble,
	/// <summary>
	/// 水落下
	/// </summary>
	WaterFall,
	/// <summary>
	/// 船の横切り
	/// </summary>
	CrossShip,
	/// <summary>
	/// 嵐
	/// </summary>
	Storm,
	/// <summary>
	/// 雨雲
	/// </summary>
	RainCloud,
	/// <summary>
	/// 風雲
	/// </summary>
	WindCloud,
	/// <summary>
	/// 雷雲
	/// </summary>
	Thundercloud,
	/// <summary>
	/// 隕石
	/// </summary>
	Meteorite,
	/// <summary>
	/// 隕石爆発
	/// </summary>
	MeteoriteExplosion,

	/// <summary>
	/// 数
	/// </summary>
	Count,
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// サウンドマネージャークラス
/// </summary>
public class MySoundManager : MySingletonMonoBehaviour<MySoundManager>
{
	#region オーディオソース
	[Header("オーディオソース")]
	/// <summary>
	/// BGM用のオーディオソース
	/// </summary>
	[SerializeField]
	AudioSource BgmAudioSource;

	/// <summary>
	/// SE用の3Dオーディオソース
	/// </summary>
	[SerializeField]
	AudioSource[] Se3DAudioSources;

	/// <summary>
	/// 特別なSE用の3Dオーディオソース
	/// </summary>
	[SerializeField]
	AudioSource[] SpecialSe3DAudioSources;

	/// <summary>
	/// SE用の2Dオーディオソース
	/// </summary>
	[SerializeField]
	AudioSource Se2DAudioSource;
	#endregion

	#region BGM
	[Header("BGM")]
	/// <summary>
	/// BGM達
	/// </summary>
	[SerializeField]
	AudioClip[] Bgms;

	/// <summary>
	/// 基本的なBGMの音量
	/// </summary>
	[SerializeField]
	float m_bgmBaseVolume;

	/// <summary>
	/// BGMの小音量
	/// </summary>
	[SerializeField]
	float m_bgmSmallVolume;

	/// <summary>
	/// BGM鳴らすか
	/// </summary>
	bool m_isBgm = true;
	public bool IsBgm
	{
		set
		{
			m_isBgm = value;
			BgmAudioSource.mute = m_isBgm;
		}
	}

	/// <summary>
	/// BGMの音量
	/// </summary>
	float m_bgmVolume;
	public float BgmVolume
	{
		set
		{
			m_bgmVolume = value;
			BgmAudioSource.volume = m_bgmVolume;
		}
	}

	/// <summary>
	/// BGMのピッチ
	/// </summary>
	float m_bgmPitch;
	public float BgmPitch
	{
		set
		{
			m_bgmPitch = value;
			BgmAudioSource.pitch = m_bgmPitch;
		}
	}
	#endregion

	#region SE
	[Header("SE")]
	/// <summary>
	/// SE達
	/// </summary>
	[SerializeField]
	AudioClip[] Ses;

	/// <summary>
	/// SE鳴らすか
	/// </summary>
	bool m_isSe = true;
	public bool IsSe
	{
		set
		{
			m_isSe = value;
			foreach(var audioSource in Se3DAudioSources)
			{
				audioSource.mute = m_isSe;
			}
			Se2DAudioSource.mute = m_isSe;
		}
	}

	/// <summary>
	/// 3DのSEの番号
	/// </summary>
	int m_se3DAudioSourceNum;

	/// <summary>
	/// 特別なSE番号の配列
	/// </summary>
	int[] m_specialSeNum = new int[(int)SpecialSeCollection.Count];
	#endregion

	#region 作業用
	[Header("作業用")]
	/// <summary>
	/// 作業用のAudioSource
	/// </summary>
	AudioSource m_workAudioSource;
	
	/// <summary>
	/// 作業用のInt
	/// </summary>
	int m_workInt;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	protected override void Awake()
	{
		base.Awake();

		//特別なSE番号の初期化
		for(m_workInt = 0; m_workInt < m_specialSeNum.Length; m_workInt++)
		{
			m_specialSeNum[m_workInt] = -1;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// BGMを再生
	/// </summary>
	/// <param name="Bgm">BGM</param>
	public void Play(BgmCollection Bgm)
	{
		if (m_isBgm)
		{
			//BGMを止める
			StopBGM();

			//BGMをセット
			BgmAudioSource.clip = Bgms[(int)Bgm];

			//再生
			BgmAudioSource.Play();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// BGMを止める
	/// </summary>
	public void StopBGM()
	{
		if (BgmAudioSource.isPlaying)
		{
			BgmAudioSource.Stop();
			BgmAudioSource.pitch = 1f;
			BgmAudioSource.volume = m_bgmBaseVolume;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// BGMを小音量にする
	/// </summary>
	public void MakeBgmSmallVolume()
	{
		m_bgmVolume = m_bgmSmallVolume;
		BgmAudioSource.volume = m_bgmVolume;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// SEを再生
	/// </summary>
	/// <param name="Se">サウンドエフェクト</param>
	/// <param name="isSe3D">3DのSEか</param>
	/// <param name="isSpecial">特別か</param>
	/// <param name="pos">位置</param>
	public void Play(SeCollection Se, bool isSe3D = false, bool isSpecial = false, Vector3? pos = null)
	{
		if (m_isSe)
		{
			//2DのSEか3DのSEか
			if(!isSe3D)
			{
				m_workAudioSource = Se2DAudioSource;
			}
			else
			{
				if (isSpecial)
				{
					//特別なSE番号の検索
					for(m_workInt = 0; m_workInt < m_specialSeNum.Length; m_workInt++)
					{
						//未使用オーディオソース
						if (m_specialSeNum[m_workInt] == -1)
						{
							m_specialSeNum[m_workInt] = (int)Se;
						}

						//特別なSEのオーディオソース発見
						if (m_specialSeNum[m_workInt] == (int)Se)
						{
							m_workAudioSource = SpecialSe3DAudioSources[m_workInt];
							break;
						}
					}
				}
				else
				{
					//音切れ防止
					m_workAudioSource = Se3DAudioSources[m_se3DAudioSourceNum];
					m_se3DAudioSourceNum = (m_se3DAudioSourceNum + 1) % Se3DAudioSources.Length;
				}
			}

			//位置
			pos = (pos == null) ? Vector3.zero : pos;
			m_workAudioSource.transform.position = (Vector3)pos;

			//SEの設定
			m_workAudioSource.clip = Ses[(int)Se];

			//再生
			if (!isSpecial || !m_workAudioSource.isPlaying)
				m_workAudioSource.Play();
		}
	}
}
