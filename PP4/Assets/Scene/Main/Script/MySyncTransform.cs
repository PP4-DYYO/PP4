////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/16～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
/// Transformを同期するクラス
/// </summary>
[NetworkSettings(channel = 1, sendInterval = 0.033f)]
public class MySyncTransform : NetworkBehaviour
{
	/// <summary>
	/// 同期する位置
	/// </summary>
	[SyncVar(hook = "SyncPositionValues")]
	Vector3 m_syncPos;

	/// <summary>
	/// 同期する回転
	/// </summary>
	[SyncVar(hook = "OnPlayerRotSynced")]
	float m_syncRotation;

	/// <summary>
	/// 補間率
	/// </summary>
	float m_lerpRate;

	/// <summary>
	/// 普通の補間間隔
	/// </summary>
	const float NORMAL_LERP_RATE = 10;

	/// <summary>
	/// 早い補間間隔
	/// </summary>
	const float FASTER_LERP_RATE = 25;

	/// <summary>
	/// 前回の通知位置
	/// </summary>
	Vector3 m_lastPos;

	/// <summary>
	/// 前回の通知回転
	/// </summary>
	float m_lastRot;

	/// <summary>
	/// しきい値（境目の値）
	/// </summary>
	const float THRESHOLD = 0.125f;

	/// <summary>
	/// しきい値（境目の回転値）
	/// </summary>
	const float THRESHOLD_ROT = 1;

	/// <summary>
	/// 位置同期用リスト
	/// </summary>
	List<Vector3> m_syncPosList = new List<Vector3>();

	/// <summary>
	/// 回転同期用リスト
	/// </summary>
	List<float> m_syncPlayerRotList = new List<float>();

	/// <summary>
	/// HistoricalLerpingメソッドを使うか
	/// </summary>
	[SerializeField]
	bool m_useHistoricalLerping = false;

	/// <summary>
	/// HistoricalInterpolationメソッドを使うか
	/// </summary>
	[SerializeField]
	bool m_useHistoricalInterpolation;

	/// <summary>
	/// ２点間の距離の判定用
	/// </summary>
	const float CLOSE_ENOUGH = 0.1f;

	/// <summary>
	/// 2点間の角度の判定用
	/// </summary>
	const float CLOSE_ENOUGH_ROT = 0.4f;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 初期
	/// </summary>
	void Start()
	{
		m_syncPos = transform.position;
		m_syncRotation = transform.localEulerAngles.y;
		m_lerpRate = NORMAL_LERP_RATE;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレームメソッド
	/// </summary>
	void Update()
	{
		//２点間の補間
		LerpPosition();
		LerpRotations();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 一定フレームメソッド
	/// </summary>
	void FixedUpdate()
	{
		//情報を送る
		TransmitPosition();
		TransmitRotations();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ポジション補間
	/// </summary>
	void LerpPosition()
	{
		if (!isLocalPlayer)
		{
			if (m_useHistoricalLerping)
				HistoricalLerping();
			else
				OrdinaryLerping();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 位置情報を通知
	/// </summary>
	/// <param name="pos">位置</param>
	[Command]
	void CmdProvidePositionToServer(Vector3 pos)
	{
		m_syncPos = pos;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回転情報を通知
	/// </summary>
	/// <param name="playerRot">回転</param>
	[Command]
	void CmdProvideRotationsToServer(float playerRot)
	{
		m_syncRotation = playerRot;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 位置情報を送る
	/// </summary>
	[ClientCallback]
	void TransmitPosition()
	{
		//現在位置と前フレームの位置がthresholdより大きい時
		if (isLocalPlayer && (transform.position - m_lastPos).sqrMagnitude > THRESHOLD * THRESHOLD)
		{
			CmdProvidePositionToServer(transform.position);
			m_lastPos = transform.position;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回転情報を送る
	/// </summary>
	[Client]
	void TransmitRotations()
	{
		if (isLocalPlayer)
		{
			if (Mathf.Abs(transform.localEulerAngles.y - m_lastRot) > THRESHOLD_ROT)
			{
				m_lastRot = transform.localEulerAngles.y;
				CmdProvideRotationsToServer(m_lastRot);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 位置の同期
	/// </summary>
	/// <param name="pos">位置</param>
	[Client]
	void SyncPositionValues(Vector3 pos)
	{
		m_syncPos = pos;
		m_syncPosList.Add(m_syncPos);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// syncPlayerRotation変数が変更された時に実行
	/// </summary>
	/// <param name="latestPlayerRot">回転</param>
	[Client]
	void OnPlayerRotSynced(float latestPlayerRot)
	{
		m_syncRotation = latestPlayerRot;
		m_syncPlayerRotList.Add(m_syncRotation);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 補間メソッド
	/// </summary>
	void OrdinaryLerping()
	{
		transform.position = Vector3.Lerp(transform.position, m_syncPos, Time.deltaTime * m_lerpRate);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 補間メソッド(古い)
	/// </summary>
	void HistoricalLerping()
	{
		if (m_syncPosList.Count > 0)
		{
			//現在位置とListの0番目の位置との中間値を補間
			transform.position = Vector3.Lerp(transform.position, m_syncPosList[0], Time.deltaTime * m_lerpRate);

			//2点間がcloseEnoughより小さくなった時
			if ((transform.position - m_syncPosList[0]).sqrMagnitude < CLOSE_ENOUGH * CLOSE_ENOUGH)
				m_syncPosList.RemoveAt(0);

			if (m_syncPosList.Count > 10)
				m_lerpRate = FASTER_LERP_RATE;
			else
				m_lerpRate = NORMAL_LERP_RATE;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 角度を補間する
	/// </summary>
	void LerpRotations()
	{
		if (!isLocalPlayer)
		{
			if (m_useHistoricalInterpolation)
				HistoricalInterpolation();
			else
				OrdinaryLerpinga();
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 角度同期（古い）
	/// </summary>
	void HistoricalInterpolation()
	{
		if (m_syncPlayerRotList.Count > 0)
		{
			LerpPlayerRotation(m_syncPlayerRotList[0]);

			if (Mathf.Abs(transform.localEulerAngles.y - m_syncPlayerRotList[0]) < CLOSE_ENOUGH_ROT)
				m_syncPlayerRotList.RemoveAt(0);
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 角度同期
	/// </summary>
	void OrdinaryLerpinga()
	{
		LerpPlayerRotation(m_syncRotation);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 現在角度を補間
	/// </summary>
	/// <param name="rotAngle">回転</param>
	void LerpPlayerRotation(float rotAngle)
	{
		var playerNewRot = Vector3.up * rotAngle;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(playerNewRot), NORMAL_LERP_RATE * Time.deltaTime);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 位置を変える
	/// </summary>
	/// <param name="pos">位置</param>
	public void ChangePos(Vector3 pos)
	{
		m_syncPos = pos;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回転角度を変える
	/// </summary>
	/// <param name="rote">回転</param>
	public void ChangeRote(float rote)
	{
		m_syncRotation = rote;
	}
}
