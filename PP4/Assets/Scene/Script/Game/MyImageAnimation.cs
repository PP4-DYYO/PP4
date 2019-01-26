////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/10/1～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画像アニメーション
/// </summary>
public class MyImageAnimation : MonoBehaviour
{
	/// <summary>
	/// アニメーションする画像
	/// </summary>
	[SerializeField]
	Image Target;
	public Image TargetImage
	{
		get { return Target; }
	}

	/// <summary>
	/// 画像たち
	/// </summary>
	[SerializeField]
	Sprite[] Images;

	/// <summary>
	/// 画像表示時間
	/// </summary>
	[SerializeField]
	float m_imageDisplayTime;

	/// <summary>
	/// 起動時に再生
	/// </summary>
	[SerializeField]
	bool m_isPlayOnAwake;

	/// <summary>
	/// ループ
	/// </summary>
	[SerializeField]
	bool m_isLoop;

	/// <summary>
	/// アニメーション終了で消える
	/// </summary>
	[SerializeField]
	bool m_isDisappearAnimEnd;

	/// <summary>
	/// 画像番号
	/// </summary>
	int m_imageNum;

	/// <summary>
	/// 画像表示時間を数える
	/// </summary>
	float m_countImageDisplayTime;

	/// <summary>
	/// 再生している
	/// </summary>
	bool m_isPlay;

	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		//再生
		if (m_isPlayOnAwake)
			StartAnimation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//再生
		if (m_isPlay)
		{
			Target.sprite = Images[m_imageNum];

			m_countImageDisplayTime += Time.deltaTime;

			//表示時間を超える
			if (m_countImageDisplayTime >= m_imageDisplayTime)
			{
				//ループ
				if (m_isLoop)
				{
					//次の画像か初めの画像か
					m_imageNum = (m_imageNum + 1 < Images.Length) ? m_imageNum + 1 : 0;
				}
				else if (++m_imageNum >= Images.Length)
				{
					//画像番号が最後を超えたら停止
					m_isPlay = false;
					Target.enabled = !m_isDisappearAnimEnd;
				}

				m_countImageDisplayTime = 0;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーションを開始
	/// </summary>
	/// <param name="isInitialization">初期化するか</param>
	public void StartAnimation(bool isInitialization = true)
	{
		Target.enabled = true;

		m_isPlay = true;

		//初期化
		if (isInitialization)
		{
			m_imageNum = 0;
			m_countImageDisplayTime = 0;
			Target.sprite = Images[m_imageNum];
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーションを停止
	/// </summary>
	public void StopAnimation()
	{
		Target.enabled = false;

		m_isPlay = false;
		m_imageNum = 0;
		m_countImageDisplayTime = 0;
	}
}
