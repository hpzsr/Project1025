using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 英雄配置表字段
public class PlayerStateChangeEntityData
{
	public string oldState;
	public string newState;
}

public class PlayerStateChangeEntity
{
	static PlayerStateChangeEntity s_instance = null;

	public List<PlayerStateChangeEntityData> heroList = new List<PlayerStateChangeEntityData>();

	static public PlayerStateChangeEntity getInstance()
	{
		if(s_instance == null)
		{
			s_instance = new PlayerStateChangeEntity();
			s_instance.init();
		}

		return s_instance;
	}

	void init()
	{
        try
        {
            heroList = JsonUtils.loadJsonToList<PlayerStateChangeEntityData>("PlayerStateChange");
        }
        catch(Exception ex)
        {
            LogUtil.s_instance.log(ex.ToString());
        }
	}

	public bool checkIsCanChange(Consts.PlayerState oldState, Consts.PlayerState newState)
	{
		for(int i = 0; i < heroList.Count; i++)
		{
			if((heroList[i].oldState == oldState.ToString()) && (heroList[i].newState == newState.ToString()))
			{
				return true;
			}
		}

		return false;
	}
}
