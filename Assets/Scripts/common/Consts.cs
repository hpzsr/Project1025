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
    public static float DevScreenWidth = 960;
    public static float DevScreenHeight = 540;
    

    public static float getWidth()
    {
        // 如果设备比设计长，则使用设备宽度,否则使用设计宽度
        if((Screen.width / Screen.height) >= (DevScreenWidth / DevScreenHeight))
        {
            return Screen.width;
        }

        return DevScreenWidth;
    }

    public static float getHeight()
    {
        // 如果设备比设计长，则使用设备高度,否则使用设计高度
        if (Screen.width / Screen.height > DevScreenWidth / DevScreenHeight)
        {
            return DevScreenHeight;
        }

        return Screen.height;
    }

    public enum PlayerState
    {
        idle,
        run_left,
        run_right,
        jump,
        climb,
        crouch,
        hurt,
        shoot,
        sprint,
        drop,

        stop_run_left,
        stop_run_right,
    }

    public enum MoveDirection
    {
        left,
        right,
        up,
        down,
    }

    public enum InputType
    {
        KeyBoard,       // 键盘
        Touch,          // 触摸
        HandShank       // 手柄
    }

    public enum Layer
    {
        GameLayer,
        GameResultLayer,
    }
}
