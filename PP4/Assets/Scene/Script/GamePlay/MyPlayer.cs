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
/// プレイヤー行動状態
/// </summary>
public enum PlayerBehaviorStatus
{
	/// <summary>
	/// 静止
	/// </summary>
	Idle,
	/// <summary>
	/// 水平移動
	/// </summary>
	HorizontalMovement,
	/// <summary>
	/// ジェット上昇
	/// </summary>
	JetRise,
	/// <summary>
	/// ジェット下降
	/// </summary>
	JetDescent,
	/// <summary>
	/// 落下
	/// </summary>
	Falling,
	/// <summary>
	/// 空中で静止
	/// </summary>
	IdleInTheAir,
	/// <summary>
	/// 立つ
	/// </summary>
	Stand,
	/// <summary>
	/// ボードを手に持つ
	/// </summary>
	HoldBoardInHand,
	/// <summary>
	/// ボードを手に持つ（パターン２）
	/// </summary>
	HoldBoardInHnad2,
	/// <summary>
	/// 変化なし
	/// </summary>
	Non,
};

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

	/// <summary>
	/// アニメーションのレイヤー
	/// </summary>
	public const string ANIM_LAYER = "Base Layer.";

	/// <summary>
	/// アニメーションパラメータ名
	/// </summary>
	public const string ANIM_PARAMETER_NAME = "PlayerAnimIdx";
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
	public MyPlayers PlayersScript
	{
		get { return Players; }
	}

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

	/// <summary>
	/// アニメーター
	/// </summary>
	[SerializeField]
	Animator Anim;

	/// <summary>
	/// 水のゲージ
	/// </summary>
	[SerializeField]
	Transform WaterGauge;

	/// <summary>
	/// ボードの方向
	/// </summary>
	[SerializeField]
	Transform BoardDirection;
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

	#region 状態
	[Header("状態")]
	/// <summary>
	/// 状態
	/// </summary>
	PlayerBehaviorStatus m_state;
	#endregion

	#region 移動速度
	[Header("移動速度")]
	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	float m_movingSpeed;
	#endregion

	#region ジェットボードの移動
	[Header("ジェットボードの移動")]
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
	/// 水圧による移動量
	/// </summary>
	[SerializeField]
	float m_transferAmountByWaterPressure;

	/// <summary>
	/// 滞在するための回転量
	/// </summary>
	[SerializeField]
	float m_rotationAmountForStay;

	/// <summary>
	/// 水平移動距離
	/// </summary>
	Vector3 m_horizontalTravelDistance;

	/// <summary>
	/// 飛んでいる
	/// </summary>
	bool m_isFly;

	/// <summary>
	/// 落下している
	/// </summary>
	bool m_isFalling;
	#endregion

	#region 浮遊関係
	[Header("浮遊関係")]
	/// <summary>
	/// 足の高さ
	/// </summary>
	[SerializeField]
	float m_footHeight;

	/// <summary>
	/// ジェット使用時間
	/// </summary>
	[SerializeField]
	float m_jetUseTime;

	/// <summary>
	/// ジェット加速時の消費率
	/// </summary>
	[SerializeField]
	float m_consumptionRateAtJetAcceleration;

	/// <summary>
	/// ジェット減速時の消費率
	/// </summary>
	[SerializeField]
	float m_consumptionRateAtJetDeceleration;

	/// <summary>
	/// ジェット回復率
	/// </summary>
	[SerializeField]
	float m_jetRecoveryRate;

	/// <summary>
	/// 足場Ray
	/// </summary>
	Ray m_scaffoldRay = new Ray();

	/// <summary>
	/// 足元の情報
	/// </summary>
	RaycastHit m_footInfo;

	/// <summary>
	/// ジェット使用時間を数える
	/// </summary>
	float m_countJetUseTime;
	#endregion

	#region サポート
	[Header("サポート")]
	/// <summary>
	/// サポート率
	/// </summary>
	[SerializeField]
	float m_supportRate;
	public float SupportRate
	{
		set { m_supportRate = value; }
	}
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
			transform.parent = GameObject.Find("Game").GetComponent<MyGame>().PlayersScript.transform;

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
		//浮遊状態を調べる
		ExamineFloatingState();

		//飛ぶ
		if (m_isFly)
			MovementInTheAir();
		else
			MovementOnTheGround();

		//アニメーション処理
		AnimProcess();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 浮遊状態を調べる
	/// </summary>
	void ExamineFloatingState()
	{
		//足元を調べる
		ExamineFeet();

		//ジェットを調べる
		ExamineJet();

		//Rボタンを押している間は飛ぶ
		if (m_isKeepPressingRButton)
			m_isFly = true;

		Rb.useGravity = !m_isFly || m_isFalling;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 足元を調べる
	/// </summary>
	void ExamineFeet()
	{
		//足元のRay
		m_scaffoldRay.origin = transform.position + (Vector3.up * m_footHeight);
		m_scaffoldRay.direction = Vector3.down;

		//地に足が付いた
		if (Physics.Raycast(m_scaffoldRay, out m_footInfo, m_footHeight) && m_footInfo.transform.tag == StageInfo.GROUND_TAG)
			m_isFly = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ジェットを調べる
	/// </summary>
	void ExamineJet()
	{
		//飛んでいるand落下していない
		if (m_isFly && !m_isFalling)
		{
			//ジェットの上昇・下降・滞在によってジェットの燃料が減る
			if (m_isKeepPressingRButton)
				m_countJetUseTime += Time.deltaTime * m_consumptionRateAtJetAcceleration;
			else if (m_isKeepPressingLButton)
				m_countJetUseTime += Time.deltaTime * m_consumptionRateAtJetDeceleration;
			else
				m_countJetUseTime += Time.deltaTime;
		}
		else if (!m_isFalling)
		{
			//地面の上
			m_countJetUseTime = 0;
		}

		//ジェットの燃料切れ
		if (m_countJetUseTime > m_jetUseTime)
		{
			m_isFalling = true;
			m_countJetUseTime = m_jetUseTime;
		}

		//水の残量
		WaterGauge.localScale = Vector3.Scale(WaterGauge.localScale, Vector3.up + Vector3.forward)
			+ Vector3.right * ((m_jetUseTime - m_countJetUseTime) / m_jetUseTime);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 空中移動
	/// </summary>
	void MovementInTheAir()
	{
		//移動
		Movement();

		//落ちていない
		if (!m_isFalling)
		{
			//Lボタンでジェット下降
			if (m_isKeepPressingLButton)
				transform.position -= Vector3.up * m_descendingForce * Time.deltaTime;

			//Rボタンでジェット上昇
			if (m_isKeepPressingRButton)
				transform.position += Vector3.up * m_risingForce * m_supportRate * Time.deltaTime;

			//ジェット上昇下降なし
			if (!m_isKeepPressingLButton && !m_isKeepPressingRButton)
			{
				//水平移動なし
				if (m_horizontalTravelDistance == Vector3.zero)
				{
					//位置滞在による回転
					transform.Rotate(Vector3.up, m_rotationAmountForStay * Time.deltaTime);
				}
			}

			//水圧による自動移動
			transform.position +=
				Vector3.Scale(BoardDirection.forward, Vector3.right + Vector3.forward) * (m_transferAmountByWaterPressure * Time.deltaTime);
		}

		//後処理
		PostProcessingOfMovement();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 地上での移動
	/// </summary>
	void MovementOnTheGround()
	{
		//移動
		Movement();

		//後処理
		PostProcessingOfMovement();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 移動
	/// </summary>
	void Movement()
	{
		m_posPrev = transform.position;

		//カメラの向きに対応した移動
		m_horizontalTravelDistance = (Vector3.Scale(m_camera.transform.forward, (Vector3.right + Vector3.forward)) * Input.GetAxis("Vertical")
			+ m_camera.transform.right * Input.GetAxis("Horizontal")).normalized * m_movingSpeed * Time.deltaTime;
		transform.position += m_horizontalTravelDistance;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 移動の後処理
	/// </summary>
	void PostProcessingOfMovement()
	{
		//ステージ移動制限
		StageMovementRestriction();

		//移動に伴う回転
		Rotation();
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ステージの移動制限
	/// </summary>
	void StageMovementRestriction()
	{
		//平面距離がステージ半径を超える
		if (Vector3.Scale(transform.position, (Vector3.right + Vector3.forward)).sqrMagnitude
			> Mathf.Pow(Players.GameScript.StageScript.CurrentFieldScript.FieldRudius, 2))
		{
			//平面のベクトルの長さをステージ半径にする
			transform.position = (Vector3.Scale(transform.position, (Vector3.right + Vector3.forward)).normalized
				* Players.GameScript.StageScript.CurrentFieldScript.FieldRudius) + (Vector3.up * transform.position.y);
		}
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
	/// アニメーション処理
	/// </summary>
	void AnimProcess()
	{
		//状態の取得
		GetState();

		//状態とアニメーション遷移が同じ
		if ((int)m_state == Anim.GetInteger(PlayerInfo.ANIM_PARAMETER_NAME))
		{
			//状態遷移を無効に
			Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)PlayerBehaviorStatus.Non);
			return;
		}

		//現在のアニメーションと状態が同じ
		if (Anim.GetCurrentAnimatorStateInfo(0).IsName(PlayerInfo.ANIM_LAYER + m_state.ToString()))
			return;

		//遷移変更
		Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)m_state);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 状態の取得
	/// </summary>
	void GetState()
	{
		//落下
		if (m_isFalling)
		{
			m_state = PlayerBehaviorStatus.Falling;
			return;
		}

		//上昇と下降と水平移動と空中状態と地上
		if (m_isKeepPressingRButton)
			m_state = PlayerBehaviorStatus.JetRise;
		else if (m_isKeepPressingLButton)
			m_state = PlayerBehaviorStatus.JetDescent;
		else if (m_horizontalTravelDistance != Vector3.zero)
			m_state = PlayerBehaviorStatus.HorizontalMovement;
		else if (m_isFly)
			m_state = PlayerBehaviorStatus.IdleInTheAir;
		else
			m_state = PlayerBehaviorStatus.Idle;
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
			//重なった位置が自分より下
			if (other.ClosestPointOnBounds(transform.position).y < transform.position.y)
				return;

			//相手のジェットウォータ―で下降する
			transform.position -= Vector3.up * m_risingForce * Time.deltaTime;

			//ジェットウォータに当たる分だけ水分回復
			m_countJetUseTime = Mathf.Max(0f, m_countJetUseTime - (Time.deltaTime * m_jetRecoveryRate));
		}
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり判定
	/// </summary>
	/// <param name="other">当たったもの</param>
	void OnCollisionEnter(Collision other)
	{
		//地面以外の衝突
		m_isFalling = !other.transform.tag.Equals(StageInfo.GROUND_TAG);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 当たり続ける判定
	/// </summary>
	/// <param name="other">当たったもの</param>
	void OnCollisionStay(Collision other)
	{
		//地面に触れている間は落下しない
		if (other.transform.tag.Equals(StageInfo.GROUND_TAG))
			m_isFalling = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 権限のないプレイヤーになる
	/// </summary>
	public void BecomeUnauthorizedPlayer()
	{
		//親設定と不要なものを取り除く
		Players = GameObject.Find("Game").GetComponent<MyGame>().PlayersScript;
		transform.parent = Players.transform;
		Rb.isKinematic = true;
		enabled = false;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// アニメーションの変更
	/// </summary>
	/// <param name="state">状態</param>
	public void SetAnimation(PlayerBehaviorStatus state)
	{
		Anim.SetInteger(PlayerInfo.ANIM_PARAMETER_NAME, (int)state);
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// 指定位置に立つ
	/// </summary>
	/// <param name="pos">位置</param>
	/// <param name="direction">方向</param>
	public void StandAtSpecifiedPos(Vector3 pos, Vector3 direction)
	{
		transform.position = pos;
		transform.LookAt(pos + direction);
		enabled = false;
		Rb.velocity = Vector3.zero;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// ゲーム状態にする
	/// </summary>
	public void MakeItBattleState()
	{
		enabled = true;
		Rb.constraints = RigidbodyConstraints.FreezeRotation;
	}

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// バトル終了状態にする
	/// </summary>
	public void MakeItBattleEndState()
	{
		enabled = false;
		Rb.constraints = RigidbodyConstraints.FreezeAll;
	}
}
