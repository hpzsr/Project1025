using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript s_instance = null;

    public Image self_img;
    public Consts.PlayerState playerState;
    public Consts.MoveDirection moveDirection = Consts.MoveDirection.right;

    float width;
    float height;
    float playerScale = 1.0f;
    float jumpPower = 10.0f;
    float curJumpPower = 10.0f;
    public float runSpeed = 3.0f;
    public float climbSpeed = 1.5f;
    public float damage = 4;

    void Start()
    {
        s_instance = this;
        
        InputControl.registerCallBack(inputCallBack);

        setState(Consts.PlayerState.idle);

        width = transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        height = transform.GetComponent<RectTransform>().sizeDelta.y;
    }

    void inputCallBack(InputControl.KeyBoard key)
    {
        switch(key)
        {
            // 跳跃、进入爬梯子状态
            case InputControl.KeyBoard.Down_W:
                {
                    Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                    if(ladder)
                    {
                        // transform.position = new Vector3(ladder.position.x, transform.position.y, 0);
                        setState(Consts.PlayerState.climb);
                    }
                    else
                    {
                        if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, Consts.PlayerState.jump))
                        {
                            if (playerState == Consts.PlayerState.crouch)
                            {
                                setState(Consts.PlayerState.idle);
                            }
                            else
                            {
                                setState(Consts.PlayerState.jump);
                            }
                        }
                    }

                    break;
                }

            // 向上爬梯子
            case InputControl.KeyBoard.Keep_W:
                {
                    if(playerState == Consts.PlayerState.climb)
                    {
                        changePlayerPos(0, climbSpeed);
                        Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                        if (!ladder)
                        {
                            setState(Consts.PlayerState.idle);
                        }
                    }
                        
                    break;
                }

            // 暂停向上爬梯子
            case InputControl.KeyBoard.Up_W:
                {
                    Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                    if (ladder)
                    {
                        FrameAnimationUtil.getInstance().stopAnimation(self_img);
                    }
                    break;
                }

            // 左右移动
            case InputControl.KeyBoard.Keep_A:
            case InputControl.KeyBoard.Keep_D:
                {
                    Consts.MoveDirection direction = key == InputControl.KeyBoard.Keep_A ? Consts.MoveDirection.left : Consts.MoveDirection.right;
                    Consts.PlayerState _playerState = key == InputControl.KeyBoard.Keep_A ? Consts.PlayerState.run_left : Consts.PlayerState.run_right;

                    if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, _playerState))
                    {
                        if (playerState == Consts.PlayerState.jump)
                        {
                            setMoveDirection(direction);
                            move(false);
                        }
                        else if (playerState == Consts.PlayerState.drop)
                        {
                            setMoveDirection(direction);
                            move(false);
                        }
                        else if (playerState == Consts.PlayerState.crouch)
                        {
                            setState(_playerState);
                        }
                        else
                        {
                            setState(_playerState);
                        }
                    }

                    break;
                }

            // 停止左右移动
            case InputControl.KeyBoard.Up_A:
            case InputControl.KeyBoard.Up_D:
                {
                    Consts.PlayerState _playerState = key == InputControl.KeyBoard.Up_A ? Consts.PlayerState.run_left : Consts.PlayerState.run_right;

                    if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, Consts.PlayerState.idle))
                    {
                        if (playerState == _playerState)
                        {
                            setState(Consts.PlayerState.idle);
                        }
                    }
                        
                    break;
                }

            // 蹲下、进入爬梯子状态
            case InputControl.KeyBoard.Down_S:
                {
                    Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                    if (ladder)
                    {
                        // transform.position = new Vector3(ladder.position.x, transform.position.y, 0);
                        setState(Consts.PlayerState.climb);
                    }
                    else
                    {
                        if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, Consts.PlayerState.crouch))
                        {
                            setState(Consts.PlayerState.crouch);
                        }
                    }

                    break;
                }

            // 往下爬梯子
            case InputControl.KeyBoard.Keep_S:
                {
                    if (playerState == Consts.PlayerState.climb)
                    {
                        changePlayerPos(0, -climbSpeed);
                        Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                        if (!ladder)
                        {
                            setState(Consts.PlayerState.idle);
                        }
                    }
                    break;
                }

            // 暂停向下爬梯子
            case InputControl.KeyBoard.Up_S:
                {
                    Transform ladder = RoadScript.s_instance.checkLadder(transform.localPosition);
                    if (ladder)
                    {
                        FrameAnimationUtil.getInstance().stopAnimation(self_img);
                    }
                    break;
                }

            // 闪现
            case InputControl.KeyBoard.Down_N:
                {
                    if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState, Consts.PlayerState.sprint))
                    {
                        setState(Consts.PlayerState.sprint);
                    }
                        
                    break;
                }

            // 射击
            case InputControl.KeyBoard.Down_M:
                {
                    if (PlayerStateChangeEntity.getInstance().checkIsCanChange(playerState,Consts.PlayerState.shoot))
                    {
                        switch (playerState)
                        {
                            case Consts.PlayerState.jump:
                            case Consts.PlayerState.drop:
                            case Consts.PlayerState.crouch:
                                {
                                    BulletScript.Create(transform.parent, moveDirection);
                                    break;
                                }

                            default:
                                {
                                    setState(Consts.PlayerState.shoot);
                                    break;
                                }
                        }
                    }

                    break;
                }
        }
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
                        else if (playerState == Consts.PlayerState.drop)
                        {
                            setMoveDirection(direction);
                            move(false);
                        }
                        else if (playerState == Consts.PlayerState.crouch)
                        {
                            // setMoveDirection(direction);

                            // 蹲下时只移动方向，不进行位移
                            // move(false);   

                            setState(_playerState);
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
                        switch(playerState)
                        {
                            case Consts.PlayerState.jump:
                            case Consts.PlayerState.drop:
                            case Consts.PlayerState.crouch:
                                {
                                    BulletScript.Create(transform.parent, moveDirection);
                                    break;
                                }

                            default:
                                {
                                    setState(Consts.PlayerState.shoot);
                                    break;
                                }
                        }
                        
                        break;
                    }

                // 闪现
                case Consts.PlayerState.sprint:
                    {
                        setState(Consts.PlayerState.sprint);
                        break;
                    }

                // 降落
                case Consts.PlayerState.drop:
                    {
                        setState(Consts.PlayerState.drop);
                        break;
                    }

                // 停止左移
                case Consts.PlayerState.stop_run_left:
                    {
                        if(playerState == Consts.PlayerState.run_left)
                        {
                            setState(Consts.PlayerState.idle);
                        }
                        break;
                    }

                // 停止右移
                case Consts.PlayerState.stop_run_right:
                    {
                        if (playerState == Consts.PlayerState.run_right)
                        {
                            setState(Consts.PlayerState.idle);
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
            changePlayerPos(0, curJumpPower);
            curJumpPower -= 0.5f;
            if (curJumpPower <= 0)
            {
                inputCallBack(Consts.PlayerState.drop);
            }
        }
        else if (playerState == Consts.PlayerState.drop)
        {
            changePlayerPos(0, curJumpPower);
            curJumpPower -= 0.5f;
            curJumpPower = curJumpPower < -20 ? -20 : curJumpPower;     // 控制最大下落速度
            Transform road = RoadScript.s_instance.checkStandRoad(transform.localPosition);
            if (road != null)
            {
                transform.position = new Vector3(transform.position.x, road.position.y, 0);
                setState(Consts.PlayerState.idle);
                curJumpPower = jumpPower;
            }
        }

        // 检测是否落地
        switch (playerState)
        {
            case Consts.PlayerState.idle:
            case Consts.PlayerState.run_left:
            case Consts.PlayerState.run_right:
                {
                    // 脚下没路则设为降落状态
                    Transform road = RoadScript.s_instance.checkStandRoad(transform.localPosition);
                    if (road == null)
                    {
                        inputCallBack(Consts.PlayerState.drop);
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
        
        // 人物移动范围：离上边的距离为屏幕高度的X倍
        float playerNotChangePosRatio_y_up = 0.35f;

        // 人物移动范围：离下边的距离为屏幕高度的X倍
        float playerNotChangePosRatio_y_down = 0.15f;

        // 左移
        if (x < 0)
        {
            if(transform.position.x < Screen.width * playerNotChangePosRatio_x)
            {
                if(!BgScript.s_instance.moveX(-x))
                {
                    if (transform.position.x > width / 2)
                    {
                        transform.localPosition += new Vector3(x, 0, 0);
                    }
                    else
                    {
                        transform.position = new Vector3(width / 2, transform.position.y, 0);
                    }
                }
                else
                {
                    transform.localPosition += new Vector3(x, 0, 0);
                }
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
                if (!BgScript.s_instance.moveX(-x))
                {
                    if (transform.position.x < (Screen.width - width / 2))
                    {
                        transform.localPosition += new Vector3(x, 0, 0);
                    }
                    else
                    {
                        transform.position = new Vector3((Screen.width - width / 2), transform.position.y,0);
                    }
                }
                else
                {
                    transform.localPosition += new Vector3(x, 0, 0);
                }
            }
            else
            {
                transform.localPosition += new Vector3(x, 0, 0);
            }
        }

        // 下移
        if(y < 0)
        {
            if (transform.position.y < Screen.height * playerNotChangePosRatio_y_down)
            {
                if(!BgScript.s_instance.moveY(-y))
                {
                    transform.localPosition += new Vector3(0, y, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(0, y, 0);
                }
            }
            else
            {
                transform.localPosition += new Vector3(0, y, 0);
            }
        }
        // 上移
        else if (y > 0)
        {
            if (transform.position.y > Screen.height * (1 - playerNotChangePosRatio_y_up))
            {
                if(BgScript.s_instance.moveY(-y))
                {
                    transform.localPosition += new Vector3(0, y, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(0, y, 0);
                }
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

        switch (_playerState)
        {
            // 站立
            case Consts.PlayerState.idle:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/idle-", Consts.FrameAnimationSpeed.low);
                    break;
                }
                
            // 左跑
            case Consts.PlayerState.run_left:
                {
                    setMoveDirection(Consts.MoveDirection.left);
                    transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/run-", Consts.FrameAnimationSpeed.normal);
                    break;
                }

            // 右跑
            case Consts.PlayerState.run_right:
                {
                    setMoveDirection(Consts.MoveDirection.right);
                    transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/run-", Consts.FrameAnimationSpeed.normal);
                    break;
                }

            // 跳跃
            case Consts.PlayerState.jump:
                {
                    curJumpPower = jumpPower;
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/jump-", Consts.FrameAnimationSpeed.low,false);
                    break;
                }

            // 爬楼梯
            case Consts.PlayerState.climb:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/climb-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            // 蹲下
            case Consts.PlayerState.crouch:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/crouch-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            // 被攻击
            case Consts.PlayerState.hurt:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/hurt-", Consts.FrameAnimationSpeed.low);
                    break;
                }

            // 射击
            case Consts.PlayerState.shoot:
                {
                    BulletScript.Create(transform.parent, moveDirection);
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/shoot-", Consts.FrameAnimationSpeed.low, false,() => {
                        setState(Consts.PlayerState.idle);
                    });

                    break;
                }

            // 闪现
            case Consts.PlayerState.sprint:
                {
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/sprint-", Consts.FrameAnimationSpeed.low);

                    // 位移
                    {
                        self_img.color = new Color(1, 1, 1, 0.3f);

                        float targetX = transform.position.x;
                        if(moveDirection == Consts.MoveDirection.left)
                        {
                            targetX = transform.position.x - 200;
                        }
                        else if (moveDirection == Consts.MoveDirection.right)
                        {
                            targetX = transform.position.x + 200;
                        }
                        transform.GetComponent<RectTransform>().DOMoveX(targetX, 0.3f, false).OnComplete(() =>
                        {
                            self_img.color = new Color(1, 1, 1, 1);
                            setState(Consts.PlayerState.idle);

                            // 如果闪现后不在屏幕内，则把坐标强制改到屏幕内
                            if (!checkIsInScreen())
                            {
                                if (moveDirection == Consts.MoveDirection.left)
                                {
                                    transform.position = new Vector3(width / 2, transform.position.y, 0);
                                }
                                else if (moveDirection == Consts.MoveDirection.right)
                                {
                                    transform.position = new Vector3((Screen.width - width / 2), transform.position.y, 0);
                                }
                            }
                        });
                    }

                    break;
                }

            // 降落
            case Consts.PlayerState.drop:
                {
                    curJumpPower = 0;
                    playerState = Consts.PlayerState.drop;
                    FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/player/drop-", Consts.FrameAnimationSpeed.low);

                    break;
                }
        }
    }

    bool checkCanChangeState(Consts.PlayerState oldState, Consts.PlayerState newState)
    {

        return false;
    }

    bool checkIsInScreen()
    {
        Vector3 pos = transform.position;
        if((pos.x >= 0) && (pos.x <= Screen.width) && (pos.y >= 0) && (pos.y <= Screen.height))
        {
            return true;
        }

        return false;
    }
}
