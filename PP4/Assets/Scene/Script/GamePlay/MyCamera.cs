////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/5/8～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　中村智哉
//協力者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　奥田裕也
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
/// カメラモード
/// </summary>
enum CameraMode
{
	/// <summary>
	/// 追跡
	/// </summary>
	Pursuit,
	/// <summary>
	/// 操作可能な追跡
	/// </summary>
	OperablePursuit,
	/// <summary>
	/// 特別仕様の操作可能な追跡
	/// </summary>
	CustomOperablePursuit,
	/// <summary>
	/// 固定
	/// </summary>
	Fixed,
	/// <summary>
	/// 指定位置をたどる
	/// </summary>
	FollowSpecifiedPos,
}

//----------------------------------------------------------------------------------------------------
/// <summary>
/// 特別仕様のカメラ
/// </summary>
[System.Serializable]
public struct CustomCamera
{
	/// <summary>
	/// カメラとプレイヤーとの距離[m]
	/// </summary>
	public float distanceToPlayer;

	/// <summary>
	/// 注視点の高さ[m]
	/// </summary>
	public float heightToWatch;

	/// <summary>
	/// 回転感度
	/// </summary>
	public float rotationSensitivity;

	/// <summary>
	/// 回転する限界比率
	/// </summary>
	public float rotationalLimitingRatio;

	/// <summary>
	/// プレイヤーの高さ
	/// </summary>
	public float playerHeight;

	/// <summary>
	/// 水平移動フラグ
	/// </summary>
	public bool isHorizontalMovement;

	/// <summary>
	/// 垂直移動フラグ
	/// </summary>
	public bool isVerticalMovement;

	/// <summary>
	/// 初期の相対的位置
	/// </summary>
	public Vector3 initialRelativePos;

	/// <summary>
	/// Zの回転値
	/// </summary>
	public float rotationValueOfZ;

	/// <summary>
	/// 壁のチェックフラグ
	/// </summary>
	public bool isWallCheck;
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// カメラを制御する
/// </summary>
[RequireComponent(typeof(Camera))]
public class MyCamera : MonoBehaviour
{
	#region 外部のインスタンス
	/// <summary>
	/// ゲーム
	/// </summary>
	[SerializeField]
	MyGame Game;
	public MyGame GameScript
	{
		get { return Game; }
	}

	/// <summary>
	/// カメラ対象
	/// </summary>
	MonoBehaviour m_target;
	#endregion

	#region 能力
	[Header("能力")]
	/// <summary>
	/// カメラモード
	/// </summary>
	[SerializeField]
	CameraMode m_mode;

	/// <summary>
	/// モード切替時間
	/// </summary>
	[SerializeField]
	float m_modeSwitchingTime;

	/// <summary>
	/// モード切替の移動フラグ
	/// </summary>
	bool m_isMoveModeSwitch;

	/// <summary>
	/// 現在の相対的位置
	/// </summary>
	Vector3 m_currentRelativePos;

	/// <summary>
	/// 目標の相対的位置
	/// </summary>
	Vector3 m_targetRelativePos;

	/// <summary>
	/// 現在の回転
	/// </summary>
	Vector3 m_currentRotation;

	/// <summary>
	/// 目標のの回転
	/// </summary>
	Vector3 m_targetRotation;

	/// <summary>
	/// モード切替時間を数える
	/// </summary>
	float m_countModeSwithingTime;
	#endregion

	#region 追跡カメラ
	[Header("追跡カメラ")]
	/// <summary>
	/// カメラとプレイヤーとの距離[m]
	/// </summary>
	[SerializeField, Tooltip("カメラとプレイヤーの距離")]
	float m_distanceToPlayer;
	public float DistanceToPlayer
	{
		get { return m_distanceToPlayer; }
	}

	/// <summary>
	/// 注視点の高さ[m]
	/// </summary>
	[SerializeField, Tooltip("注視点の高さ")]
	float m_heightToWatch;
	public float HeightToWatch
	{
		get { return m_heightToWatch; }
	}

	/// <summary>
	/// 回転感度
	/// </summary>
	[SerializeField, Tooltip("回転感度")]
	float m_rotationSensitivity;

	/// <summary>
	/// 回転する限界比率
	/// </summary>
	[SerializeField, Tooltip("回転する限界比率")]
	float m_rotationalLimitingRatio;

	/// <summary>
	/// プレイヤーの高さ
	/// </summary>
	[SerializeField, Tooltip("プレイヤーの高さ")]
	float m_playerHeight;

	/// <summary>
	/// Xの回転量
	/// </summary>
	float m_rotX;

	/// <summary>
	/// Yの回転量
	/// </summary>
	float m_rotY;

	/// <summary>
	/// プレイヤーの中心位置
	/// </summary>
	Vector3 m_playerCenterPos;

	/// <summary>
	/// Rayの方向
	/// </summary>
	Vector3 m_rayDirection;

	/// <summary>
	/// Rayの作成
	/// </summary>
	Ray m_ray;

	/// <summary>
	/// Rayが衝突したコライダーの情報を得る
	/// </summary>
	RaycastHit m_hit;
	#endregion

	#region カスタム追跡カメラ
	[Header("カスタム追跡カメラ")]
	/// <summary>
	/// 特別仕様のカメラ
	/// </summary>
	[SerializeField, Tooltip("特別仕様のカメラ")]
	CustomCamera[] m_customCamera;

	/// <summary>
	/// カスタムカメラの番号
	/// </summary>
	int m_numOfCustomCamera;
	#endregion

	#region 指定位置をたどるカメラ
	[Header("指定位置をたどるカメラ")]
	/// <summary>
	/// 指定位置
	/// </summary>
	Vector3[] m_specifiedPos;

	/// <summary>
	/// 指定方向
	/// </summary>
	Vector3[] m_specifiedDirection;

	/// <summary>
	/// 指定移動時間
	/// </summary>
	float[] m_specifiedMovingTime;

	/// <summary>
	/// 移動時間を数える
	/// </summary>
	float m_countMovingTime;

	/// <summary>
	/// 指定番号
	/// </summary>
	int m_specifiedNum;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用Float
	/// </summary>
	float m_workFloat;
	#endregion

#if DEBUG
	#region Debug
	[Header("Debug")]
	/// <summary>
	/// ターゲットを自分自身で取得する
	/// </summary>
	[SerializeField]
	bool m_isSearchForTheTargetOnYourOwn_debug;
	#endregion
#endif

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		//対象の設定
		SearchTarget();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ターゲットを探す
	/// </summary>
	void SearchTarget()
	{
#if DEBUG
		//自分自身でターゲットを探す
		if (m_isSearchForTheTargetOnYourOwn_debug)
		{
			m_target = FindObjectOfType<MyPlayer>();

			//モード毎に初期化
			switch (m_mode)
			{
				case CameraMode.Pursuit:
					BecomePursuitCamera();
					break;
				case CameraMode.OperablePursuit:
					BecomeOperablePursuitCamera();
					break;
				case CameraMode.CustomOperablePursuit:
					BecomeCustomOperablePursuitCamera();
					break;
				case CameraMode.Fixed:
					BecomeFixedCamera(transform.position, m_target.transform.position - transform.position);
					break;
				case CameraMode.FollowSpecifiedPos:
					break;
			}
			return;
		}
#endif
		m_target = Game.OperatingPlayerScript;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 後フレーム
	/// </summary>
	void LateUpdate()
	{
		//ターゲットなし
		if (!m_target)
		{
			//ターゲットを探す
			SearchTarget();
			return;
		}

		//モード
		switch (m_mode)
		{
			case CameraMode.Pursuit:
				PursuitProcess();
				break;
			case CameraMode.OperablePursuit:
				OperablePursuitProcess();
				break;
			case CameraMode.CustomOperablePursuit:
				CustomOperablePursuitProcess();
				break;
			case CameraMode.Fixed:
				FixedProcess();
				break;
			case CameraMode.FollowSpecifiedPos:
				FollowSpecifiedPosProcess();
				break;
		}

		//モード切替の移動
		if (m_isMoveModeSwitch)
			MoveModeSwitch();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 追跡カメラ処理
	/// </summary>
	void PursuitProcess()
	{
		//プレイヤーの中心位置
		m_playerCenterPos = m_target.transform.position + Vector3.up * m_heightToWatch;

		// カメラとプレイヤーとの間の距離を調整
		transform.position = m_playerCenterPos - transform.forward * m_distanceToPlayer;

		// 視点の設定
		transform.LookAt(m_playerCenterPos);

		//壁のチェック
		CheckWall();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 敵とプレイヤーとの間に壁があるかを確認する
	/// </summary>
	void CheckWall()
	{
		// 自分の位置とプレイヤーの位置から向きベクトルを作成しRayに渡す
		m_rayDirection = transform.position - m_playerCenterPos;
		m_ray = new Ray(m_playerCenterPos, m_rayDirection);

		// Rayが衝突したかどうか
		if (Physics.Raycast(m_ray, out m_hit, m_distanceToPlayer))
		{
			//触れることが可
			if (!m_hit.collider.isTrigger)
				transform.position = m_hit.point;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 操作可能な追跡処理
	/// </summary>
	void OperablePursuitProcess()
	{
		//カメラの位置リセット
		if (Input.GetKey(KeyCode.Joystick1Button9))
			SetPosition(-m_target.transform.forward + Vector3.up * m_playerHeight);

		//カメラの回転量
		m_rotX = Input.GetAxis("Mouse X") * Time.deltaTime * m_rotationSensitivity;
		m_rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * m_rotationSensitivity;

		//プレイヤーの中心位置
		m_playerCenterPos = m_target.transform.position + Vector3.up * m_heightToWatch;

		//Y回転
		transform.RotateAround(m_playerCenterPos, Vector3.up, m_rotX);

		//X回転
		//カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
		if (transform.forward.y > m_rotationalLimitingRatio && m_rotY > 0)
		{
			m_rotY = 0;
		}
		if (transform.forward.y < -m_rotationalLimitingRatio && m_rotY < 0)
		{
			m_rotY = 0;
		}
		transform.RotateAround(m_playerCenterPos, transform.right, -m_rotY);

		//追跡処理
		PursuitProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 特別仕様の操作可能な追跡カメラ処理
	/// </summary>
	void CustomOperablePursuitProcess()
	{
		//カメラの位置リセット
		if (Input.GetKey(KeyCode.Joystick1Button9))
			SetPosition(-m_target.transform.forward + Vector3.up * m_customCamera[m_numOfCustomCamera].playerHeight);

		//カメラの回転量
		m_rotX = (m_customCamera[m_numOfCustomCamera].isHorizontalMovement ?
			Input.GetAxis("Mouse X") * Time.deltaTime * m_rotationSensitivity : 0);
		m_rotY = (m_customCamera[m_numOfCustomCamera].isVerticalMovement ?
			Input.GetAxis("Mouse Y") * Time.deltaTime * m_rotationSensitivity : 0);

		//プレイヤーの中心位置
		m_playerCenterPos = m_target.transform.position + Vector3.up * m_customCamera[m_numOfCustomCamera].heightToWatch;

		//Y回転
		transform.RotateAround(m_playerCenterPos, Vector3.up, m_rotX);

		//X回転
		//カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
		if (transform.forward.y > m_customCamera[m_numOfCustomCamera].rotationalLimitingRatio && m_rotY > 0)
		{
			m_rotY = 0;
		}
		if (transform.forward.y < -m_customCamera[m_numOfCustomCamera].rotationalLimitingRatio && m_rotY < 0)
		{
			m_rotY = 0;
		}
		transform.RotateAround(m_playerCenterPos, transform.right, -m_rotY);

		// カメラとプレイヤーとの間の距離を調整
		transform.position = m_playerCenterPos - transform.forward * m_customCamera[m_numOfCustomCamera].distanceToPlayer;

		// 視点の設定
		transform.LookAt(m_playerCenterPos);

		transform.Rotate(Vector3.forward, m_customCamera[m_numOfCustomCamera].rotationValueOfZ);

		//壁のチェック
		if (m_customCamera[m_numOfCustomCamera].isWallCheck)
			CheckWall();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定カメラ処理
	/// </summary>
	void FixedProcess()
	{
		return;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 指定位置をたどる処理
	/// </summary>
	void FollowSpecifiedPosProcess()
	{
		m_countMovingTime += Time.deltaTime;

		//移動時間の割合
		m_workFloat = m_countMovingTime / m_specifiedMovingTime[m_specifiedNum - 1];

		//時間の割合による位置と方向
		transform.position = m_specifiedPos[m_specifiedNum - 1] +
			(m_specifiedPos[m_specifiedNum] - m_specifiedPos[m_specifiedNum - 1]) * m_workFloat;
		transform.LookAt(transform.position +
			((m_specifiedDirection[m_specifiedNum] * m_workFloat) + (m_specifiedDirection[m_specifiedNum - 1] * (1f - m_workFloat))));

		//指定時間を超えた
		if (m_countMovingTime >= m_specifiedMovingTime[m_specifiedNum - 1])
		{
			//指定位置がなければ固定カメラに
			if (++m_specifiedNum >= m_specifiedPos.Length)
				m_mode = CameraMode.Fixed;

			m_countMovingTime = 0;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// モード切替の移動
	/// </summary>
	void MoveModeSwitch()
	{
		//目標位置が未設定
		if (m_targetRelativePos == m_currentRelativePos)
			m_targetRelativePos = transform.position - m_target.transform.position;

		//目標回転が未設定
		if (m_targetRotation == m_currentRotation)
			m_targetRotation = transform.rotation.eulerAngles;

		m_countModeSwithingTime += Time.deltaTime;

		//位置
		transform.position = m_currentRelativePos +
			(m_targetRelativePos - m_currentRelativePos) * (m_countModeSwithingTime / m_modeSwitchingTime) + m_target.transform.position;

		//回転
		transform.eulerAngles = m_currentRotation +
			(m_targetRotation - m_currentRotation) * (m_countModeSwithingTime / m_modeSwitchingTime);

		//終了
		if (m_countModeSwithingTime >= m_modeSwitchingTime)
		{
			transform.position = m_targetRelativePos + m_target.transform.position;
			transform.eulerAngles = m_targetRotation;
			m_isMoveModeSwitch = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 固定カメラになる
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="direction">方向</param>
	/// <param name="isMoveModeSwitch">モード切替の移動フラグ</param>
	public void BecomeFixedCamera(Vector3 pos, Vector3 direction, bool isMoveModeSwitch = false)
	{
		//モード切替の移動フラグ
		if (isMoveModeSwitch)
		{
			m_isMoveModeSwitch = true;
			m_currentRelativePos = transform.position - m_target.transform.position;
			m_targetRelativePos = pos - m_target.transform.position;
			m_currentRotation = transform.eulerAngles;
			m_targetRotation = m_currentRotation;
			m_countModeSwithingTime = 0;
		}

		m_mode = CameraMode.Fixed;
		transform.position = pos;
		transform.LookAt(transform.position + direction);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 操作可能な追跡カメラになる
	/// </summary>
	/// <param name="isMoveModeSwitch">モード切替の移動フラグ</param>
	/// <param name="target">ターゲット</param>
	public void BecomeOperablePursuitCamera(bool isMoveModeSwitch = false, MonoBehaviour target = null)
	{
		//モード切替の移動フラグ
		if (isMoveModeSwitch)
		{
			m_isMoveModeSwitch = true;
			m_currentRelativePos = transform.position - (target == null ? m_target.transform.position : target.transform.position);
			m_targetRelativePos = m_currentRelativePos;
			m_currentRotation = transform.eulerAngles;
			m_targetRotation = m_currentRotation;
			m_countModeSwithingTime = 0;
		}

		m_mode = CameraMode.OperablePursuit;

		if (target)
			m_target = target;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 特別仕様の操作可能な追跡カメラになる
	/// </summary>
	/// <param name="isMoveModeSwitch">モード切替の移動フラグ</param>
	/// <param name="numOfCustomCamera">カスタムカメラ番号</param>
	/// <param name="target">ターゲット</param>
	public void BecomeCustomOperablePursuitCamera(bool isMoveModeSwitch = false, int numOfCustomCamera = 0, MonoBehaviour target = null)
	{
		//モード切替の移動フラグ
		if (isMoveModeSwitch)
		{
			m_isMoveModeSwitch = true;
			m_currentRelativePos = transform.position - (target == null ? m_target.transform.position : target.transform.position);
			m_targetRelativePos = m_currentRelativePos;
			m_currentRotation = transform.eulerAngles;
			m_targetRotation = m_currentRotation;
			m_countModeSwithingTime = 0;
		}

		m_mode = CameraMode.CustomOperablePursuit;
		m_numOfCustomCamera = numOfCustomCamera;

		if (target)
			m_target = target;

		//初期位置
		SetPosition(m_customCamera[numOfCustomCamera].initialRelativePos);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 追跡カメラになる
	/// </summary>
	/// <param name="isMoveModeSwitch">モード切替の移動フラグ</param>
	/// <param name="relativePos">相対的位置</param>
	/// <param name="target">ターゲット</param>
	public void BecomePursuitCamera(bool isMoveModeSwitch = false, Vector3? relativePos = null, MonoBehaviour target = null)
	{
		//モード切替の移動フラグ
		if (isMoveModeSwitch)
		{
			m_isMoveModeSwitch = true;
			m_currentRelativePos = transform.position - (target == null ? m_target.transform.position : target.transform.position);
			m_targetRelativePos = m_currentRelativePos;
			m_currentRotation = transform.eulerAngles;
			m_targetRotation = m_currentRotation;
			m_countModeSwithingTime = 0;
		}

		m_mode = CameraMode.Pursuit;

		if (target)
			m_target = target;

		//位置の指定
		if (relativePos != null)
			SetPosition((Vector3)relativePos);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 指定位置をたどるカメラになる
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="direction">方向</param>
	/// <param name="movingTime">移動時間</param>
	/// <param name="isMoveModeSwitch">モード切替の移動フラグ</param>
	public void BecomeFollowSpecifiedPosCamera(Vector3[] pos, Vector3[] direction, float[] movingTime, bool isMoveModeSwitch = false)
	{
		//モード切替の移動フラグ
		if (isMoveModeSwitch)
		{
			m_isMoveModeSwitch = true;
			m_currentRelativePos = transform.position - m_target.transform.position;
			m_targetRelativePos = m_currentRelativePos;
			m_currentRotation = transform.eulerAngles;
			m_targetRotation = m_currentRotation;
			m_countModeSwithingTime = 0;
		}

		//引数の要素数が違っている
		if (pos.Length != direction.Length || direction.Length != movingTime.Length)
			Debug.LogError("引数の要素数を合わせてください");

		//代入
		m_specifiedPos = pos;
		m_specifiedDirection = direction;
		m_specifiedMovingTime = movingTime;

		//初期設定
		m_mode = CameraMode.FollowSpecifiedPos;
		m_specifiedNum = 1;
		m_countMovingTime = 0;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// カメラの位置を設定
	/// </summary>
	/// <param name="relativeLocations">相対的位置</param>
	public void SetPosition(Vector3 relativeLocations)
	{
		transform.position = m_target.transform.position + relativeLocations;
		transform.LookAt(m_target.transform.position + Vector3.up * m_heightToWatch);
	}
}
