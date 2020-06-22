using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    // 输入事件回调
    public delegate void InputCallBack(Consts.PlayerState playerState);
    static InputCallBack inputCallBack = null;

    Consts.InputType inputType = Consts.InputType.KeyBoard;

    public static void registerCallBack(InputCallBack _inputCallBack)
    {
        inputCallBack = _inputCallBack;
    }
    
    void Update()
    {
        if(inputCallBack == null)
        {
            return;
        }

        // 键盘控制
        if (inputType == Consts.InputType.KeyBoard)
        {
            // 跳跃
            if (Input.GetKeyDown(KeyCode.W))
            {
                inputCallBack(Consts.PlayerState.jump);
            }
            // 左移
            else if (Input.GetKey(KeyCode.A))
            {
                inputCallBack(Consts.PlayerState.run_left);
            }
            // 停止左移
            else if (Input.GetKeyUp(KeyCode.A))
            {
                inputCallBack(Consts.PlayerState.idle);
            }
            // 蹲下
            else if (Input.GetKeyDown(KeyCode.S))
            {
                inputCallBack(Consts.PlayerState.crouch);
            }
            // 右移
            else if (Input.GetKey(KeyCode.D))
            {
                inputCallBack(Consts.PlayerState.run_right);
            }
            // 停止右移
            else if (Input.GetKeyUp(KeyCode.D))
            {
                inputCallBack(Consts.PlayerState.idle);
            }

            // 开枪
            if (Input.GetKeyDown(KeyCode.M))
            {
                inputCallBack(Consts.PlayerState.shoot);
            }
        }
    }
}