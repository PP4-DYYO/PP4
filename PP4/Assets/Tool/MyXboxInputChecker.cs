////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/6/12～
//作成者　京都コンピュータ学院京都駅前校　ゲーム学科　4回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

///<summary>
/// Xboxコントローラーのボタン入力確認用
///</summary>
public class MyXboxInputChecker : MonoBehaviour
{
	/// <summary>
	/// 入力を受け付ける値
	/// </summary>
	const float VALUE_TO_ACCEPT_INPUT = 0.7f;

	//----------------------------------------------------------------------------------------------------
	/// <summary>
	/// フレーム
	/// </summary>
	void Update()
	{
		if (Input.GetKey(KeyCode.Joystick1Button0))
			Debug.Log("Button A Push");

		if (Input.GetKey(KeyCode.Joystick1Button1))
			Debug.Log("Button B Push");

		if (Input.GetKey(KeyCode.Joystick1Button2))
			Debug.Log("Button X Push");

		if (Input.GetKey(KeyCode.Joystick1Button3))
			Debug.Log("Button Y Push");

		if (Input.GetKey(KeyCode.Joystick1Button4))
			Debug.Log("Button LB Push");

		if (Input.GetKey(KeyCode.Joystick1Button5))
			Debug.Log("Button RB Push");

		if (Input.GetKey(KeyCode.Joystick1Button6))
			Debug.Log("Button Back Push");

		if (Input.GetKey(KeyCode.Joystick1Button7))
			Debug.Log("Button START Push");

		if (Input.GetKey(KeyCode.Joystick1Button8))
			Debug.Log("L Stick Push");

		if (Input.GetKey(KeyCode.Joystick1Button9))
			Debug.Log("R Stick Push");

		var leftStickX = Input.GetAxis("Horizontal");
		if (leftStickX >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("L Stick Lean To The Right");
		else if(leftStickX <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("L Stick Lean To The Left");

		var leftStickY = Input.GetAxis("Vertical");
		if (leftStickY >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("L Stick Lean To The Up");
		else if (leftStickY <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("L Stick Lean To The Down");

		//Name       :Trigger
		//Gravity    :0
		//Dead       :0.19
		//Sensitivity:1
		//Type       :Joystick Axis
		//Axis       :3rd axis
		var triger = Input.GetAxis("Trigger");
		if (triger >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("L Triger");
		else if (triger <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("R Triger");

		//Name       :RightStickX
		//Gravity    :0
		//Dead       :0.19
		//Sensitivity:1
		//Type       :Joystick Axis
		//Axis       :4th axis
		var rightStickX = Input.GetAxis("RightStickX");
		if (rightStickX >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("R Stick Lean To The Right");
		else if (rightStickX <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("R Stick Lean To The Left");

		//Name       :RightStickY
		//Gravity    :0
		//Dead       :0.19
		//Sensitivity:1
		//Type       :Joystick Axis
		//Axis       :5th axis
		var rightStickY = Input.GetAxis("RightStickY");
		if (rightStickY >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("R Stick Lean To The Up");
		else if (rightStickY <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("R Stick Lean To The Down");

		//Name       :DpadX
		//Gravity    :0
		//Dead       :0.19
		//Sensitivity:1
		//Type       :Joystick Axis
		//Axis       :6th axis
		var dpadX = Input.GetAxis("DpadX");
		if (dpadX >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("Dpad Lean To The Right");
		else if (dpadX <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("Dpad Lean To The Left");

		//Name       :DpadY
		//Gravity    :0
		//Dead       :0.19
		//Sensitivity:1
		//Type       :Joystick Axis
		//Axis       :7th axis
		var dpadY = Input.GetAxis("DpadY");
		if (dpadY >= VALUE_TO_ACCEPT_INPUT)
			Debug.Log("Dpad Lean To The Up");
		else if (dpadY <= -VALUE_TO_ACCEPT_INPUT)
			Debug.Log("Dpad Lean To The Down");
	}
}
