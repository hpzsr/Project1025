using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript s_instance = null;

    public Image self_img;
    public Text text_playerState;
    Consts.PlayerState playerState;
    Consts.MoveDirection moveDirection = Consts.MoveDirection.right;

    float playerScale = 2.0f;
    float runSpeed = 3.0f;

    float jumpPower = 10.0f;
    float curJumpPower = 10.0f;

    void Start()
    {
        s_instance = this;

        Application.targetFrameRate = 60;
        InputControl.registerCallBack(inputCallBack);

        setState(Consts.PlayerState.idle);
    }

    void inputCallBack(Consts.PlayerState _playerState)
    {
        if(PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, _playerState))
        {
            switch (_playerState)
            {
                // 静止
                case Consts.PlayerState.idle:
                    {
                        setState(Consts.PlayerState.idle);
                        break;
                    }

                // 跳跃
                case Consts.PlayerState.jump:
                    {
                        if (playerState == Consts.PlayerState.crouch)
                        {
                            setState(Consts.PlayerState.idle);
                        }
                        else
                        {
                            setState(Consts.PlayerState.jump);
                        }
                        break;
                    }

                // 左移、右移
                case Consts.PlayerState.run_left:
                case Consts.PlayerState.run_right:
                    {
                        Consts.MoveDirection direction = _playerState == Consts.PlayerState.run_left ? Consts.MoveDirection.left : Consts.MoveDirection.right;
                        if (playerState == Consts.PlayerState.jump)
                        {
                            setMoveDirection(direction);
                            move(false);
                        }
                        else if (playerState == Consts.PlayerState.crouch)
                        {
                            setMoveDirection(direction);
                            // 蹲下时只移动方向，不进行位移
                            // move(false);         
                        }
                        else
                        {
                            setState(_playerState);
                        }
                        break;
                    }

                // 蹲下
                case Consts.PlayerState.crouch:
                    {
                        setState(Consts.PlayerState.crouch);
                        break;
                    }

                // 开枪
                case Consts.PlayerState.shoot:
                    {
                        if (playerState == Consts.PlayerState.jump)
                        {
                            BulletScript.Create(transform.parent, moveDirection);
                        }
                        else if (playerState == Consts.PlayerState.crouch)
                        {
                            BulletScript.Create(transform.parent, moveDirection);
                        }
                        else
                        {
                            setState(Consts.PlayerState.shoot);
                        }
                        
                        break;
                    }
            }
        }
    }

    void Update()
    {
        // 检测左右移动
        move(true);
        
        // 跳跃
        if (playerState == Consts.PlayerState.jump)
        {
            transform.localPosition += new Vector3(0, curJumpPower, 0);
            curJumpPower -= 0.5f;
            if (transform.localPosition.y < -140)
            {
                transform.localPosition = new Vector3(transform.localPosition.x,-140,0);
                setState(Consts.PlayerState.idle);
                curJumpPower = jumpPower;
            }
        }
    }

    void move(bool isCheck)
    {
        if(isCheck)
        {
            if (playerState == Consts.PlayerState.run_left)
            {
                transform.localPosition -= new Vector3(runSpeed, 0, 0);
            }
            else if (playerState == Consts.PlayerState.run_right)
            {
                transform.localPosition += new Vector3(runSpeed, 0, 0);
            }
        }
        else
        {
            if(moveDirection == Consts.MoveDirection.left)
            {
                transform.localPosition -= new Vector3(runSpeed, 0, 0);
            }
            else
            {
                transform.localPosition += new Vector3(runSpeed, 0, 0);
            }
        }
    }

    public void setMoveDirection(Consts.MoveDirection _moveDirection)
    {
        moveDirection = _moveDirection;
        if(moveDirection == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
        }
        else
        {
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        }
    }

    public void setState(Consts.PlayerState _playerState)
    {
        playerState = _playerState;
        text_playerState.text = _playerState.ToString();

        switch (_playerState)
        {
            case Consts.PlayerState.idle:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/idle-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            case Consts.PlayerState.run_left:
                {
                    setMoveDirection(Consts.MoveDirection.left);
                    transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/run-", Consts.FrameAnimationSpeed.normal);
                    break;
                }

            case Consts.PlayerState.run_right:
                {
                    setMoveDirection(Consts.MoveDirection.right);
                    transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/run-", Consts.FrameAnimationSpeed.normal);
                    break;
                }

            case Consts.PlayerState.jump:
                {
                    curJumpPower = jumpPower;
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/jump-", Consts.FrameAnimationSpeed.low,false);
                    break;
                }

            case Consts.PlayerState.back_jump:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/back-jump-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            case Consts.PlayerState.climb:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/climb-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            case Consts.PlayerState.crouch:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/crouch-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            case Consts.PlayerState.hurt:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/hurt-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            case Consts.PlayerState.shoot:
                {
                    BulletScript.Create(transform.parent, moveDirection);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/shoot-", Consts.FrameAnimationSpeed.low, false,() => {
                        setState(Consts.PlayerState.idle);
                    });

                    break;
                }
        }
    }

    bool checkCanChangeState(Consts.PlayerState oldState, Consts.PlayerState newState)
    {

        return false;
    }
}
