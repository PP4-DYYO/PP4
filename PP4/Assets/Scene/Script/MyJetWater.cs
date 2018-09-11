////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/9/3～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
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
/// ジェットウォーター情報
/// </summary>
public struct JetWaterInfo
{
	/// <summary>
	/// タグ
	/// </summary>
	public const string TAG = "JetWater";
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 方向キー入力状況
/// </summary>
public enum JetDirection
{
	NON,
	UP,
	DOWN,
	RIGHT,
	LEFT,
	UP_RIGHT,
	UP_LEFT,
	DOWN_RIGHT,
	DOWN_LEFT
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// テスト用
/// </summary>
public class MyJetWater : MonoBehaviour
{
	/// <summary>
	/// プレイヤーのオブジェクト
	/// </summary>
	[SerializeField]
	MonoBehaviour Player;

	/// <summary>
	/// 水発射点のオブジェクト1
	/// </summary>
	[SerializeField]
	GameObject Water1;

	/// <summary>
	/// 水発射点のオブジェクト2
	/// </summary>
	[SerializeField]
	GameObject Water2;

	float m_xrotate;
	float m_zrotate;
	const float MIN_ANGLE = 0.0F;
	const float MAX_ANGLE = 90.0F;

	JetDirection m_jetDirection;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// スタート
	/// </summary>	
	void Start()
	{
		m_xrotate = 0.05f;
		m_zrotate = 0.05f;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アップデート
	/// </summary>	
	void Update()
	{		
		if(m_jetDirection == JetDirection.RIGHT|| m_jetDirection == JetDirection.LEFT)
		{
			Water1.transform.localScale = new Vector3(0.1f, Mathf.Sqrt(2)* (Player.transform.position.y / 20) + 0.1f, 0.1f);
			Water2.transform.localScale = new Vector3(0.1f, Mathf.Sqrt(2)*(Player.transform.position.y / 20) + 0.1f, 0.1f);
		}
		else
		{
			Water1.transform.localScale = new Vector3(0.1f, (Player.transform.position.y / 20) + 0.1f, 0.1f);
			Water2.transform.localScale = new Vector3(0.1f, (Player.transform.position.y / 20) + 0.1f, 0.1f);
		}

		CheckDirection();
		//RotationJetWater();	
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 移動方向判定
	/// </summary>	
	void CheckDirection()
	{
		//キー入力時
		if (Input.GetKey("up"))
		{
			if (m_jetDirection == JetDirection.RIGHT)
			{
				m_jetDirection = JetDirection.UP_RIGHT;
			}
			else if (m_jetDirection == JetDirection.LEFT)
			{
				m_jetDirection = JetDirection.UP_LEFT;
			}
			else
			{
				m_jetDirection = JetDirection.UP;
			}
		}
		else if (Input.GetKey("down"))
		{
			if (m_jetDirection == JetDirection.RIGHT)
			{
				m_jetDirection = JetDirection.DOWN_RIGHT;
			}
			else if (m_jetDirection == JetDirection.LEFT)
			{
				m_jetDirection = JetDirection.DOWN_LEFT;
			}
			else
			{
				m_jetDirection = JetDirection.DOWN;
			}
		}
		else if (Input.GetKey("right"))
		{
			if (m_jetDirection == JetDirection.UP)
			{
				m_jetDirection = JetDirection.UP_RIGHT;
			}
			else if (m_jetDirection == JetDirection.DOWN)
			{
				m_jetDirection = JetDirection.DOWN_RIGHT;
			}
			else
			{
				m_jetDirection = JetDirection.RIGHT;
			}
		}
		else if (Input.GetKey("left"))
		{
			if (m_jetDirection == JetDirection.UP)
			{
				m_jetDirection = JetDirection.UP_LEFT;
			}
			else if (m_jetDirection == JetDirection.DOWN)
			{
				m_jetDirection = JetDirection.DOWN_LEFT;
			}
			else
			{
				m_jetDirection = JetDirection.LEFT;
			}
		}

		//キーを離したとき
		if (Input.GetKeyUp("up") || Input.GetKeyUp("down") || Input.GetKeyUp("right") || Input.GetKeyUp("left"))
		{
			m_jetDirection = JetDirection.NON;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ジェットウォーターの回転
	/// </summary>	
	//void RotationJetWater()
	//{
	//	switch (m_jetDirection)
	//	{
	//		//↑移動時
	//		case JetDirection.UP:
	//			float upAngle = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_xrotate);
	//			if (m_xrotate <= 0.5)
	//			{
	//				m_xrotate += 0.05f;
	//			}
	//			Water1.transform.eulerAngles = new Vector3(upAngle, 0, 0);
	//			Water2.transform.eulerAngles = new Vector3(upAngle, 0, 0);
	//			break;
	//		//↓移動時
	//		case JetDirection.DOWN:
	//			float downAngle = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_xrotate);
	//			if (m_xrotate <= 0.5)
	//			{
	//				m_xrotate += 0.05f;
	//			}
	//			Water1.transform.eulerAngles = new Vector3(-downAngle, 0, 0);
	//			Water2.transform.eulerAngles = new Vector3(-downAngle, 0, 0);
	//			break;
	//		//→移動時
	//		case JetDirection.RIGHT:
	//			float rightAngle = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_zrotate);
	//			if (m_zrotate <= 0.5)
	//			{
	//				m_zrotate += 0.05f;
	//			}
	//			Water1.transform.eulerAngles = new Vector3(0, 0, -rightAngle);
	//			Water2.transform.eulerAngles = new Vector3(0, 0, -rightAngle);
	//			break;
	//		//←移動時
	//		case JetDirection.LEFT:
	//			float leftAngle = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_zrotate);
	//			if (m_zrotate <= 0.5)
	//			{
	//				m_zrotate += 0.05f;
	//			}
	//			Water1.transform.eulerAngles = new Vector3(0, 0, leftAngle);
	//			Water2.transform.eulerAngles = new Vector3(0, 0, leftAngle);
	//			break;
	//		//↑→移動時
	//		case JetDirection.UP_RIGHT:			
	//			break;
	//		//停止時
	//		case JetDirection.NON:
	//			float angle = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_xrotate);
	//			float angle_ = Mathf.LerpAngle(MIN_ANGLE, MAX_ANGLE, m_zrotate);
	//			if (m_xrotate > 0)
	//			{
	//				m_xrotate -= 0.05f;
	//			}
	//			if (m_zrotate > 0)
	//			{
	//				m_zrotate -= 0.05f;
	//			}
	//			Water1.transform.eulerAngles = new Vector3(angle, 0, angle_);
	//			Water2.transform.eulerAngles = new Vector3(angle, 0, angle_);
	//			break;
	//	}
	//}
}
