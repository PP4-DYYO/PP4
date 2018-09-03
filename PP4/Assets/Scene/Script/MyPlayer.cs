////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/8/16～
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
/// 行動状態
/// </summary>
enum BehaviorStatus
{ };

//----------------------------------------------------------------------------------------------------
/// <summary>
/// プレイヤー情報
/// </summary>
public struct PlayerInfo
{
	/// <summary>
	/// タグ
	/// </summary>
	public const string TAG = "Player";
}

//----------------------------------------------------------------------------------------------------
//クラス
//----------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------
/// <summary>
/// プレイヤー
/// </summary>
public class MyPlayer : MonoBehaviour
{
	#region 外部のインスタンス
	[Header("外部のインスタンス")]
	/// <summary>
	/// プレイヤーの収集物
	/// </summary>
	MyPlayers Players;

	/// <summary>
	/// カメラ
	/// </summary>
	MyCamera m_camera;
	#endregion

	#region コンポーネント
	[Header("コンポーネント")]
	/// <summary>
	/// リジッドボディ
	/// </summary>
	[SerializeField]
	Rigidbody Rb;
	#endregion

	#region トランスフォーム
	[Header("トランスフォーム")]
	/// <summary>
	/// 回転スピード
	/// </summary>
	[SerializeField]
	float m_rotationSpeed;

	/// <summary>
	/// 補正角度
	/// </summary>
	[SerializeField]
	int m_correctionAngle;

	/// <summary>
	/// フレーム前の位置
	/// </summary>
	Vector3 m_posPrev;

	/// <summary>
	/// 移動量
	/// </summary>
	Vector3 m_direction;

	/// <summary>
	/// 角度
	/// </summary>
	float m_angle;

	/// <summary>
	/// １周の角度
	/// </summary>
	const int ONE_TURNING_ANGLE = 360;

	/// <summary>
	/// 半周の角度
	/// </summary>
	const int HALF_CIRCUMFERENCE_ANGLE = 180;
	#endregion

	#region 移動速度
	[Header("移動速度")]
	/// <summary>
	/// 徒歩速度
	/// </summary>
	[SerializeField]
	float m_walkSpeed;

	/// <summary>
	/// 空中移動速度
	/// </summary>
	[SerializeField]
	float m_airMovingSpeed;
	#endregion

	#region 移動
	[Header("移動")]
	/// <summary>
	/// 上昇する力
	/// </summary>
	[SerializeField]
	float m_risingForce;

	/// <summary>
	/// 下降する力
	/// </summary>
	[SerializeField]
	float m_descendingForce;

	/// <summary>
	/// 加速率
	/// </summary>
	[SerializeField]
	float m_accelerationRate;

	/// <summary>
	/// 飛んでいる
	/// </summary>
	bool m_isFly;
	#endregion

	#region 足関係
	[Header("足関係")]
	/// <summary>
	/// 足の高さ
	/// </summary>
	[SerializeField]
	float m_footHeight;

	/// <summary>
	/// 足場Ray
	/// </summary>
	Ray m_scaffoldRay = new Ray();

	/// <summary>
	/// 足元の情報
	/// </summary>
	RaycastHit m_footInfo;
	#endregion

	#region キーボード関係
	[Header("キーボード関係")]
	/// <summary>
	/// Lボタンを押しっぱなし
	/// </summary>
	bool m_isKeepPressingLButton;

	/// <summary>
	/// Rボタンを押しっぱなし
	/// </summary>
	bool m_isKeepPressingRButton;
	#endregion

	#region 作業用
	/// <summary>
	/// 作業用のVector３
	/// </summary>
	Vector3 m_workVector3;
	#endregion

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 起動
	/// </summary>
	void Awake()
	{
		SearchInstance();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// インスタンスを探す
	/// </summary>
	void SearchInstance()
	{
		//親なし
		if (!transform.parent)
			return;

		//親クラスなし
		Players = transform.parent.GetComponent<MyPlayers>();
		if (!Players)
			return;

		//不足インスタンスの設定
		m_camera = Players.GameScript.OperatingCameraScript;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		m_isKeepPressingLButton = Input.GetButton("LButton");
		m_isKeepPressingRButton = Input.GetButton("RButton");
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 定期フレーム
	/// </summary>
	void FixedUpdate()
	{
		//インスタンスが未取得
		if (!Players)
		{
			SearchInstance();
			return;
		}

		//足元を調べる
		ExamineFeet();

		//飛ぶ
		if (m_isFly)
			MovementInTheAir();
		else
			Movement();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 足元を調べる
	/// </summary>
	void ExamineFeet()
	{
		m_scaffoldRay.origin = transform.position + (Vector3.up * m_footHeight);
		m_scaffoldRay.direction = Vector3.down;

		//地に足が付いた
		if (Physics.Raycast(m_scaffoldRay, out m_footInfo, m_footHeight) && m_footInfo.transform.tag == StageInfo.GROUND_TAG)
			m_isFly = false;

		//Rボタンを押している間は飛ぶ
		if (m_isKeepPressingRButton)
			m_isFly = true;

		Rb.useGravity = !m_isFly;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 空中移動
	/// </summary>
	void MovementInTheAir()
	{
		m_posPrev = transform.position;

		//Lボタンでジェット下降
		if (m_isKeepPressingLButton)
			transform.position -= Vector3.up * m_descendingForce * Time.deltaTime;

		//Rボタンでジェット上昇
		if (m_isKeepPressingRButton)
			transform.position += Vector3.up * m_risingForce * Time.deltaTime;

		//ジェット上昇下降なし
		if (!m_isKeepPressingLButton && !m_isKeepPressingRButton)
		{
		}

		//カメラの向きに対応した移動
		transform.position +=
			(Vector3.Scale(m_camera.transform.forward, (Vector3.right + Vector3.forward)) * Input.GetAxis("Vertical")
			+ m_camera.transform.right * Input.GetAxis("Horizontal")).normalized * m_airMovingSpeed * Time.deltaTime;

		//移動に伴う回転
		Rotation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 移動
	/// </summary>
	void Movement()
	{
		m_posPrev = transform.position;

		//カメラの向きに対応した移動
		transform.position +=
			(Vector3.Scale(m_camera.transform.forward, (Vector3.right + Vector3.forward)) * Input.GetAxis("Vertical")
			+ m_camera.transform.right * Input.GetAxis("Horizontal")).normalized * m_walkSpeed * Time.deltaTime;

		//移動に伴う回転
		Rotation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 回転
	/// </summary>
	void Rotation()
	{
		//移動量
		m_direction = Vector3.Scale((transform.position - m_posPrev), (Vector3.right + Vector3.forward)).normalized;

		//移動なし
		if (m_direction == Vector3.zero)
			return;

		//移動量から向くべき角度を算出
		m_angle = Mathf.Asin(m_direction.x) / Mathf.PI * HALF_CIRCUMFERENCE_ANGLE;
		if (m_direction.z < 0)
			m_angle = HALF_CIRCUMFERENCE_ANGLE - m_angle;

		m_workVector3 = transform.eulerAngles;

		//マイナス角度の修正
		m_angle += (m_angle < 0) ? ONE_TURNING_ANGLE : 0;
		m_workVector3.y += (m_workVector3.y < 0) ? ONE_TURNING_ANGLE : 0;

		//回転したい角度
		var m_workFloat = Mathf.Abs(m_workVector3.y - m_angle);

		//回転角度が小さい
		if (m_workFloat <= m_correctionAngle)
		{
			//向きたい方向に向く
			m_workVector3.y = m_angle;
		}
		else if (m_workFloat < HALF_CIRCUMFERENCE_ANGLE) //向きたい角度が想定範囲
		{
			//徐々に向きたい方向に向く
			m_angle -= m_workVector3.y;
			m_angle *= Time.deltaTime * m_rotationSpeed;
			m_workVector3.y += m_angle;
		}
		else
		{
			//大きい角度をマイナスにする
			if (m_angle < m_workVector3.y)
				m_workVector3.y -= ONE_TURNING_ANGLE;
			else
				m_angle -= ONE_TURNING_ANGLE;

			//徐々に向きたい方向に向く
			m_angle -= m_workVector3.y;
			m_angle *= Time.deltaTime * m_rotationSpeed;
			m_workVector3.y += m_angle;
		}

		transform.eulerAngles = m_workVector3;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 重なり続ける判定
	/// </summary>
	/// <param name="other">重なったもの</param>
	void OnTriggerStay(Collider other)
	{
		//ジェットウォーターに当たる
		if (other.tag.Equals(JetWaterInfo.TAG))
		{
			//相手のジェットウォータ―で下降する
			transform.position -= Vector3.up * m_risingForce * Time.deltaTime;
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーパラメータを設定
	/// </summary>
	/// <param name="player">設定したいプレイヤー</param>
	public void SetPlayerParameters(MyPlayer player)
	{
		//必要なコンポーネント
		Rb = GetComponent<Rigidbody>();

		//トランスフォーム
		m_rotationSpeed = player.m_rotationSpeed;
		m_correctionAngle = player.m_correctionAngle;

		//移動速度
		m_walkSpeed = player.m_walkSpeed;
		m_airMovingSpeed = player.m_airMovingSpeed;

		//移動
		m_risingForce = player.m_risingForce;
		m_descendingForce = player.m_descendingForce;
		m_accelerationRate = player.m_accelerationRate;

		//足関係
		m_footHeight = player.m_footHeight;
	}
}
