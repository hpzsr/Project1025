using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 英雄配置表字段
public class HeroEntityData
{
    public int id;
    public string name;
    public string level;
    public int star;
    public int needSeat;
    public int hp;
    public int shuijing;
    public int defense_guangsu;
    public int defense_gedou;
    public int defense_shidan;
    public int huibi_rate;
    public int jidongli;
    public int en_max;
    public int en_huifu;
    public int fanji_rate;
    public int zhuiji_rate;
}

public class HeroData
{
    public float hp = 0;
    public float en = 0;
}

public class Consts
{
    public enum PlayerState
    {
        idle,
        walk,
        run_left,
        run_right,
        run_shoot,
        jump,
        back_jump,
        climb,
        crouch,
        hurt,
        shoot,
    }

    public enum MoveDirection
    {
        left,
        right,
    }

    public enum FrameAnimationSpeed
    {
        low,
        normal,
        quick,
    }

    public enum InputType
    {
        KeyBoard,       // 键盘
        Touch,          // 触摸
        HandShank       // 手柄
    }
}
