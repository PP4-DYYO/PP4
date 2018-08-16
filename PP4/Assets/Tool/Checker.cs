////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2018/6/12～
//製作者 京都コンピュータ学院京都駅前校ゲーム学科四回生　吉田純基
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

///<summary>
///コントローラーのボタン入力確認用
///</summary>
public class Checker : MonoBehaviour {
    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button0))
        {
            Debug.Log("Button A Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            Debug.Log("Button B Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button2))
        {
            Debug.Log("Button X Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            Debug.Log("Button Y Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            Debug.Log("Button LB Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            Debug.Log("Button RB Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button6))
        {
            Debug.Log("Button Back Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button7))
        {
            Debug.Log("Button START Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button8))
        {
            Debug.Log("L Stick Push Push");
        }

        if (Input.GetKey(KeyCode.Joystick1Button9))
        {
            Debug.Log("R Stick Push");
        }

        float TrigerInput = Input.GetAxis("Trigger");
        if (TrigerInput == 1.0f)
        {
            Debug.Log("L Triger");
        }
        else if (TrigerInput == -1.0f)
        {
            Debug.Log("R Triger");
        }

        float HorizontalKeyInput = Input.GetAxis("HorizontalKey");
        if (HorizontalKeyInput ==-1.0f)
        {
            Debug.Log("Left Key");
        }
        else if (HorizontalKeyInput == 1.0f)
        {
            Debug.Log("Right Key");
        }

        float VerticalKeyInput = Input.GetAxis("VerticalKey");
        if (VerticalKeyInput == -1.0f)
        {
            Debug.Log("Up Key");
        }
        else if (VerticalKeyInput ==1.0f)
        {
            Debug.Log("Down Key");
        }
    }
}
