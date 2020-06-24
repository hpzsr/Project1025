﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo
{
    public Transform m_transform;
    public float m_width;

    public RoadInfo(Transform _transform, float _width)
    {
        m_transform = _transform;
        m_width = _width;
    }
}

public class RoadScript : MonoBehaviour
{
    public static RoadScript s_instance = null;

    List<RoadInfo> roadList = new List<RoadInfo>();
    List<RoadInfo> ladderList = new List<RoadInfo>();

    void Start()
    {
        s_instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform road = transform.GetChild(i);
            float width = road.GetComponent<RectTransform>().sizeDelta.x;
            if (road.rotation.z == 0)
            {
                roadList.Add(new RoadInfo(road,width));
            }
            else
            {
                ladderList.Add(new RoadInfo(road, width));
            }
        }
    }

    void Update()
    {
        
    }

    public Transform checkStandRoad(Vector3 vec3)
    {
        for(int i = 0; i < roadList.Count; i++)
        {
            Vector3 roadPos = roadList[i].m_transform.localPosition;
            float roadWidth = roadList[i].m_width;
            //Debug.Log(vec3 + "  " + roadList[i].m_transform.localPosition + roadWidth);
            if ((vec3.x >= roadPos.x) && (vec3.x <= (roadPos.x + roadWidth)))
            {
                float jily_y = roadPos.y - vec3.y;
                if ((jily_y >= 0) && (jily_y < 25))
                {
                    return roadList[i].m_transform;
                }
            }
        }

        return null;
    }

    public Transform checkLadder(Vector3 vec3)
    {
        for (int i = 0; i < ladderList.Count; i++)
        {
            Vector3 roadPos = ladderList[i].m_transform.localPosition;
            float roadWidth = ladderList[i].m_width;
            float jili_x = Mathf.Abs(roadPos.x - vec3.x);
            // 人物和梯子横向距离<10即可认为是爬梯子
            if(jili_x < 10)
            {
                // 为什么要-5？防止梯子高于地面，导致爬不上去
                if ((vec3.y >= (roadPos.y - 5)) && (vec3.y <= (roadPos.y + roadWidth)))
                {
                    return ladderList[i].m_transform;
                }
            }
        }

        return null;
    }
}
