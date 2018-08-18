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

	/// <summary>
	/// カメラとプレイヤーとの距離[m]
	/// </summary>
	[SerializeField, Header("カメラとプレイヤーの距離")]
	float m_distanceToPlayer;

	/// <summary>
	/// 注視点の高さ[m]
	/// </summary>
	[SerializeField, Header("注視点の高さ")]
	float m_heightToWatch;

	/// <summary>
	/// 回転感度
	/// </summary>
	[SerializeField, Header("回転感度")]
	float m_rotationSensitivity;

	/// <summary>
	/// 回転する限界比率
	/// </summary>
	[SerializeField, Header("回転する限界比率")]
	float m_rotationalLimitingRatio;

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

		//カメラの位置リセット
		if (Input.GetKey(KeyCode.Joystick1Button9))
			SetPosition(-m_target.transform.forward + Vector3.up * 1.75f);

		//カメラの回転量
		//m_rotX = Input.GetAxis("HorizontalR") * Time.deltaTime * m_rotationSensitivity;
		//m_rotY = Input.GetAxis("VerticalR") * Time.deltaTime * m_rotationSensitivity;
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
	/// カメラの位置を設定
	/// </summary>
	/// <param name="relativeLocations">相対的位置</param>
	public void SetPosition(Vector3 relativeLocations)
	{
		transform.position = m_target.transform.position + relativeLocations;
		transform.LookAt(m_target.transform.position + Vector3.up * m_heightToWatch);
	}
}
