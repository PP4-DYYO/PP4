////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/12/28～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドの泡
/// </summary>
public class MyFieldFoam : MonoBehaviour
{
	/// <summary>
	/// 波マテリアル
	/// </summary>
	[SerializeField]
	Material m_waveMaterial;

	/// <summary>
	/// ターゲットのRenderer
	/// </summary>
	[SerializeField]
	Renderer m_targetRenderer;

	/// <summary>
	/// 波テクスチャ
	/// </summary>
	RenderTexture[] m_waveTex = new RenderTexture[3];

	/// <summary>
	/// 現在のテクスチャ番号
	/// </summary>
	int m_currentTexIdx = 0;

	/// <summary>
	/// １フレーム前のテクスチャ番号
	/// </summary>
	int m_prev1TexIdx;

	/// <summary>
	/// ２フレーム前のテクスチャ番号
	/// </summary>
	int m_prev2TexIdx;

	/// <summary>
	/// 泡発生位置たち
	/// </summary>
	Vector3[] m_bubbleGenerationPos = new Vector3[10];

	/// <summary>
	/// 作業用のInt
	/// </summary>
	int m_workInt;

	/// <summary>
	/// 作業用のVector3
	/// </summary>
	Vector3 m_workVector3;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		//テクスチャの作成
		for (m_workInt = 0; m_workInt < m_waveTex.Length; m_workInt++)
		{
			m_waveTex[m_workInt] = new RenderTexture(2048, 2048, 24);
			m_waveTex[m_workInt].wrapMode = TextureWrapMode.Clamp;
			m_waveTex[m_workInt].Create();
		}

		//泡発生位置の初期化
		for (m_workInt = 0; m_workInt < m_bubbleGenerationPos.Length; m_workInt++)
		{
			m_bubbleGenerationPos[m_workInt] = -Vector3.one;
			m_waveMaterial.SetVector("_bubbleGenerationPos" + m_workInt, -Vector4.one);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//全ての泡
		for (m_workInt = 0; m_workInt < m_bubbleGenerationPos.Length; m_workInt++)
		{
			m_workVector3 = m_waveMaterial.GetVector("_bubbleGenerationPos" + m_workInt);

			//未使用
			if (m_bubbleGenerationPos[m_workInt] == -Vector3.one)
				continue;

			//泡が反映されている
			if (m_bubbleGenerationPos[m_workInt] == m_workVector3)
			{
				//泡の位置の通知
				m_bubbleGenerationPos[m_workInt] = -Vector3.one;
				m_waveMaterial.SetVector("_bubbleGenerationPos" + m_workInt, -Vector4.one);
			}
			else
			{
				GenerateBubbles(m_bubbleGenerationPos[m_workInt], m_workInt);
			}
		}

		//テクスチャの更新
		UpdateTexture();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 泡の発生
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="bubbleNum">泡番号</param>
	void GenerateBubbles(Vector3 pos, int bubbleNum = 0)
	{
		m_waveMaterial.SetVector("_bubbleGenerationPos" + bubbleNum, pos);

		//SE
		MySoundManager.Instance.Play(SeCollection.SplashedFoam, true, false, pos);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// テクスチャの更新
	/// </summary>
	void UpdateTexture()
	{
		//番号を更新
		m_prev1TexIdx = (m_currentTexIdx - 1 + 3) % 3;
		m_prev2TexIdx = (m_currentTexIdx - 2 + 3) % 3;

		//テクスチャの通知
		m_waveMaterial.SetTexture("_prev1", m_waveTex[m_prev1TexIdx]);
		m_waveMaterial.SetTexture("_prev2", m_waveTex[m_prev2TexIdx]);

		//テクスチャのコピー
		Graphics.Blit(m_waveTex[m_prev1TexIdx], m_waveTex[m_currentTexIdx], m_waveMaterial);

		//テクスチャの反映
		m_targetRenderer.material.mainTexture = m_waveTex[m_currentTexIdx];
		m_targetRenderer.material.SetTexture("_mainTex", m_waveTex[m_currentTexIdx]);

		m_currentTexIdx = (m_currentTexIdx + 1) % 3;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり始める判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerEnter(Collider other)
	{
		//ジェットウォータ以外を破棄
		if (!other.tag.Equals(JetWaterInfo.TAG))
			return;

		//泡発生位置の保存
		for (m_workInt = 0; m_workInt < m_bubbleGenerationPos.Length; m_workInt++)
		{
			if (m_bubbleGenerationPos[m_workInt] == -Vector3.one)
			{
				m_bubbleGenerationPos[m_workInt] = Vector3.Scale(other.transform.position, Vector3.right + Vector3.forward);
				break;
			}
		}
	}
}
