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
    public Consts.PlayerState playerState;
    public Consts.MoveDirection moveDirection = Consts.MoveDirection.right;

    float playerScale = 1.0f;
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

                // 闪现
                case Consts.PlayerState.sprint:
                    {
                        setState(Consts.PlayerState.sprint);
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
            changePlayerPos(0, curJumpPower);
            curJumpPower -= 0.5f;
            curJumpPower = curJumpPower < -20 ? -20 : curJumpPower;
            if (curJumpPower <= 0)
            {
                Transform road = RoadScript.s_instance.checkStandRoad(transform.position);
                if (road != null)
                {
                    transform.position = new Vector3(transform.position.x, road.position.y, 0);
                    setState(Consts.PlayerState.idle);
                    curJumpPower = jumpPower;
                }
            }
        }

        // 降落时检测是否落地
        switch (playerState)
        {
            case Consts.PlayerState.idle:
            case Consts.PlayerState.run_left:
            case Consts.PlayerState.run_right:
                {
                    // 脚下没路则设为降落状态
                    Transform road = RoadScript.s_instance.checkStandRoad(transform.position);
                    if (road == null)
                    {
                        curJumpPower = 0;
                        playerState = Consts.PlayerState.jump;
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, road.position.y, 0);
                    }
                    break;
                }
        }
    }

    void move(bool isCheck)
    {
        if(isCheck)
        {
            if (playerState == Consts.PlayerState.run_left)
            {
                changePlayerPos(-runSpeed,0);
            }
            else if (playerState == Consts.PlayerState.run_right)
            {
                changePlayerPos(runSpeed, 0);
            }
        }
        else
        {
            if(moveDirection == Consts.MoveDirection.left)
            {
                changePlayerPos(-runSpeed, 0);
            }
            else if (moveDirection == Consts.MoveDirection.right)
            {
                changePlayerPos(runSpeed, 0);
            }
        }
    }

    void changePlayerPos(float x,float y)
    {
        // 人物移动范围：离左右两边的距离为屏幕宽度的X倍
        float playerNotChangePosRatio_x = 0.25f;
        
        // 人物移动范围：离上下两边的距离为屏幕高度的X倍
        float playerNotChangePosRatio_y = 0.25f;

        // 左移
        if (x < 0)
        {
            if(transform.position.x < Screen.width * playerNotChangePosRatio_x)
            {
                BgScript.s_instance.move(-x,y);
            }
            else
            {
                transform.localPosition += new Vector3(x, 0, 0);
            }
        }
        // 右移
        else if (x > 0)
        {
            if (transform.position.x > Screen.width * (1 - playerNotChangePosRatio_x))
            {
                BgScript.s_instance.move(-x, y);
            }
            else
            {
                transform.localPosition += new Vector3(x, 0, 0);
            }
        }

        // 下移
        if(y < 0)
        {
            if (transform.position.y < 20)
            {
                BgScript.s_instance.move(0, y);
            }
            else
            {
                transform.localPosition += new Vector3(0, y, 0);
            }
        }
        // 上移
        else if (y > 0)
        {
            if (transform.position.y > Screen.height * (1 - playerNotChangePosRatio_x))
            {
                BgScript.s_instance.move(0, -y);
            }
            else
            {
                transform.localPosition += new Vector3(0, y, 0);
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
        else if (moveDirection == Consts.MoveDirection.right)
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

            case Consts.PlayerState.sprint:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/sprint-", Consts.FrameAnimationSpeed.low);

                    // 闪现
                    {
                        self_img.color = new Color(1, 1, 1, 0.5f);

                        float targetX = transform.localPosition.x;
                        if(moveDirection == Consts.MoveDirection.left)
                        {
                            targetX = transform.localPosition.x - 200;
                        }
                        else if (moveDirection == Consts.MoveDirection.right)
                        {
                            targetX = transform.localPosition.x + 200;
                        }
                        transform.GetComponent<RectTransform>().DOAnchorPosX(targetX, 0.3f, false).OnComplete(() =>
                        {
                            self_img.color = new Color(1, 1, 1, 1);
                            setState(Consts.PlayerState.idle);
                        });
                    }

                    break;
                }
        }
    }

    bool checkCanChangeState(Consts.PlayerState oldState, Consts.PlayerState newState)
    {

        return false;
    }
}
