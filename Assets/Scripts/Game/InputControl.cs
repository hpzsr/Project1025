using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public enum KeyBoard
    {
        Down_W,
        Keep_W,
        Up_W,
        Keep_A,
        Up_A,
        Keep_D,
        Up_D,
        Down_S,
        Keep_S,
        Up_S,
        Down_N,
        Down_M,
    }

    // 输入事件回调
    public delegate void InputCallBack(KeyBoard key);
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                inputCallBack(KeyBoard.Down_W);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                inputCallBack(KeyBoard.Up_W);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                inputCallBack(KeyBoard.Keep_W);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                inputCallBack(KeyBoard.Up_A);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                inputCallBack(KeyBoard.Keep_A);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                inputCallBack(KeyBoard.Up_D);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputCallBack(KeyBoard.Keep_D);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                inputCallBack(KeyBoard.Up_S);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                inputCallBack(KeyBoard.Keep_S);
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                inputCallBack(KeyBoard.Down_N);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                inputCallBack(KeyBoard.Down_M);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                inputCallBack(KeyBoard.Down_S);
            }

            //// 跳跃
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    inputCallBack(Consts.PlayerState.jump);
            //}
            //// 左移
            //else if (Input.GetKey(KeyCode.A))
            //{
            //    inputCallBack(Consts.PlayerState.run_left);
            //}
            //// 停止左移
            //else if (Input.GetKeyUp(KeyCode.A))
            //{
            //    inputCallBack(Consts.PlayerState.stop_run_left);
            //}
            //// 蹲下
            //else if (Input.GetKeyDown(KeyCode.S))
            //{
            //    inputCallBack(Consts.PlayerState.crouch);
            //}
            //// 右移
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    inputCallBack(Consts.PlayerState.run_right);
            //}
            //// 停止右移
            //else if (Input.GetKeyUp(KeyCode.D))
            //{
            //    inputCallBack(Consts.PlayerState.stop_run_right);
            //}

            //// 开枪
            //if (Input.GetKeyDown(KeyCode.M))
            //{
            //    inputCallBack(Consts.PlayerState.shoot);
            //}
            //// 闪现
            //else if (Input.GetKeyDown(KeyCode.N))
            //{
            //    inputCallBack(Consts.PlayerState.sprint);
            //}
        }
    }
}