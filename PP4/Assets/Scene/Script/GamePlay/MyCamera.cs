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
	/// 固定
	/// </summary>
	Fixed,
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
	#endregion

	#region 追跡カメラ
	[Header("追跡カメラ")]
	/// <summary>
	/// カメラとプレイヤーとの距離[m]
	/// </summary>
	[SerializeField, Tooltip("カメラとプレイヤーの距離")]
	float m_distanceToPlayer;

	/// <summary>
	/// 注視点の高さ[m]
	/// </summary>
	[SerializeField, Tooltip("注視点の高さ")]
	float m_heightToWatch;

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
			case CameraMode.Fixed:
				FixedProcess();
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 追跡処理
	/// </summary>
	void PursuitProcess()
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
		transform.position -= m_ray.GetPoint(Camera.main.nearClipPlane) - m_playerCenterPos;
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
	/// 固定カメラになる
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="direction">方向</param>
	public void BecomeFixedCamera(Vector3 pos, Vector3 direction)
	{
		m_mode = CameraMode.Fixed;
		transform.position = pos;
		transform.LookAt(transform.position + direction);
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
