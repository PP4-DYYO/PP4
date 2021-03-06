﻿////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/20～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶき情報
/// </summary>
public struct SplasheInfo
{
	/// <summary>
	/// タグ
	/// </summary>
	public const string TAG = "Splashe";

	/// <summary>
	/// プレイヤーと当たらない水しぶきのタグ
	/// </summary>
	public const string TRANS_TAG = "TransSplashe";
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 水しぶきの水
/// </summary>
public class MySplasheWater : MonoBehaviour
{
	/// <summary>
	/// 自身のRigidbody
	/// </summary>
	[SerializeField]
	Rigidbody Rb;

	/// <summary>
	/// 水しぶきの残る時間
	/// </summary>
	[SerializeField]
	float m_splasheLifeTime;

	/// <summary>
	/// 水しぶきが発生してからの経過時間
	/// </summary>
	float m_splasheLivingTime;

	/// <summary>
	/// 水しぶきを大きくする値(Z方向)
	/// </summary>
	[SerializeField]
	float m_splasheSizeChange;

	/// <summary>
	/// 水しぶきを小さくする数値(X,Y方向)
	/// </summary>
	[SerializeField]
	float m_splasheXYSizeChange;

	/// <summary>
	/// 水しぶきが地面に落ちたかどうか
	/// </summary>
	[SerializeField]
	bool m_isfallen;
	public bool Fallen
	{
		get { return m_isfallen; }
	}

	/// <summary>
	/// 自身のZの大きさ
	/// </summary>
	float m_splasheScaleZ;

	/// <summary>
	/// 着地から消えるまでの時間
	/// </summary>
	[SerializeField]
	float m_splasheSmallerTime;

	/// <summary>
	/// 水しぶきの本体
	/// </summary>
	[SerializeField]
	GameObject Body;
	public GameObject BodyObj
	{
		get { return Body; }
	}

	/// <summary>
	/// 水しぶきのbodyの変更用
	/// </summary>
	Vector3 m_changedBody;

	/// <summary>
	/// 自身の表示状態
	/// </summary>
	bool m_isDisplay;
	public bool isDisplay
	{
		get { return m_isDisplay; }
	}

	/// <summary>
	/// 生成されてからプレイヤーと接触しない時間
	/// </summary>
	[SerializeField]
	float m_nonTouchTime;

	/// <summary>
	/// 水しぶきのパーティクルシステム
	/// </summary>
	[SerializeField]
	ParticleSystem SplashParticle;

	/// <summary>
	/// 水しぶきのパーティクルシステムのモジュール
	/// </summary>
	ParticleSystem.MainModule SplashParticleMainModule;

	/// <summary>
	/// 水しぶきのパーティクルシステムのStartColor
	/// </summary>
	ParticleSystem.MinMaxGradient SplashParticleMinMaxGradient;

	/// <summary>
	/// 水しぶきのパーティクルシステムのモジュールがうごいているか
	/// </summary>
	bool m_isParticleModuleMoving;

	/// <summary>
	/// 水しぶきのパーティクルシステムのDuration
	/// </summary>
	float m_splashParticleDuration;

	/// <summary>
	/// ループが止まるまでの時間へ足す数値
	/// </summary>
	[SerializeField]
	float m_addNum;

	/// <summary>
	/// ループが止まる時間
	/// </summary>
	float m_loopStopTime;

	/// <summary>
	/// 作業用のColor
	/// </summary>
	Color m_workColor;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Start()
	{
		m_splasheScaleZ = Body.transform.localScale.z;
		tag = SplasheInfo.TRANS_TAG;
		m_isParticleModuleMoving = true;
		SplashParticleMainModule = SplashParticle.main;
		SplashParticleMinMaxGradient = SplashParticle.main.startColor;
		m_loopStopTime = SplashParticleMainModule.duration + m_addNum;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきの動き
	/// </summary>	
	void Update()
	{
		if (m_isDisplay)
		{
			//時間経過で消滅する
			if (m_splasheLivingTime >= m_splasheLifeTime)
			{
				MySplasheDestroy();
			}

			//パーティクルの設定
			ParticleProcess();

			//タグの調整
			TagControl();

			//着地後
			if (m_isfallen)
			{
				//サイズが０以下になるときには消す
				if (Body.transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime) < 0)
				{
					MySplasheDestroy();
				}
				else
				{
					m_changedBody.x = Body.transform.localScale.x;
					m_changedBody.y = Body.transform.localScale.y;
					m_changedBody.z = Body.transform.localScale.z - (m_splasheScaleZ / m_splasheSmallerTime) * Time.deltaTime;

					Body.transform.localScale = m_changedBody;
				}
			}
			//空中
			else
			{
				//x、yは小さく、zは大きく変更される
				m_changedBody.x = Body.transform.localScale.x - m_splasheXYSizeChange * Time.deltaTime;
				m_changedBody.y = Body.transform.localScale.y;
				m_changedBody.z = Body.transform.localScale.z + m_splasheSizeChange * Time.deltaTime;

				//マイナスのVector3回避
				if (m_changedBody.x < 0)
					m_changedBody.x = m_changedBody.y;
				else
					m_changedBody.y = m_changedBody.x;

				//スケールの更新
				Body.transform.localScale = m_changedBody;
				m_splasheScaleZ = Body.transform.localScale.z;
			}
			m_splasheLivingTime += Time.deltaTime;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶき当たり判定
	/// </summary>
	void OnTriggerEnter(Collider other)
	{
		//ステージに衝突時消える
		if (other.tag == StageInfo.GROUND_TAG)
		{
			m_isfallen = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身が消えるときの処理
	/// </summary>
	public void MySplasheDestroy()
	{
		MySoundManager.Instance.Play(SeCollection.SplashFallsOnSurfaceOfWater, true, true, Body.transform.position);
		Body.SetActive(false);
		m_isfallen = false;
		m_isDisplay = false;
		m_splasheLivingTime = 0;
		m_isParticleModuleMoving = true;
		tag = SplasheInfo.TRANS_TAG;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 自身の表示の切り替え
	/// </summary>
	public void ActiveChange(bool choosing)
	{
		Body.SetActive(choosing);
		m_isDisplay = choosing;
		m_splasheLivingTime = 0;
		m_isParticleModuleMoving = true;
		tag = SplasheInfo.TRANS_TAG;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきに力を加える
	/// </summary>
	public void AddingForce(Vector3 v)
	{
		Rb.velocity = Vector3.zero;
		Rb.AddForce(v);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 水しぶきパーティクルの設定
	/// </summary>
	void ParticleProcess()
	{
		//一定時間経過か着地でループが止まる
		if (m_splasheLifeTime - m_splasheLivingTime <= m_loopStopTime && m_isParticleModuleMoving || m_isfallen)
			m_isParticleModuleMoving = false;

		//ループ制御
		if (SplashParticleMainModule.loop != m_isParticleModuleMoving)
			SplashParticleMainModule.loop = m_isParticleModuleMoving;

		//色の反映
		m_workColor = SplashParticleMinMaxGradient.color;
		m_workColor.a = 1 - Mathf.Pow((m_splasheLivingTime / m_splasheLifeTime), 2);
		SplashParticleMinMaxGradient.color = m_workColor;
		SplashParticleMainModule.startColor = SplashParticleMinMaxGradient;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// タグの調整
	/// </summary>
	void TagControl()
	{
		if (tag == SplasheInfo.TRANS_TAG)
		{
			if (m_splasheLivingTime >= m_nonTouchTime)
			{
				tag = SplasheInfo.TAG;
			}
		}
	}
}
