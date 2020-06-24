using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScript : MonoBehaviour
{
    public static BgScript s_instance = null;

    public GameObject distance1;
    public GameObject distance2;
    public GameObject distance3;

    public Transform map;
    public float mapWidth;

    void Start()
    {
        s_instance = this;

        map = transform.Find("distance1/map");
        mapWidth = map.GetComponent<RectTransform>().sizeDelta.x;
    }
    
    void Update()
    {
    }

    public bool moveX(float x)
    {
        // distance1
        {
            if (x != 0)
            {
                float maxLeft_x = Consts.getWidth() - mapWidth;

                map.localPosition += new Vector3(x, 0, 0);
                if (map.position.x > 0)
                {
                    map.position = new Vector3(0, map.position.y, 0);
                    return false;
                }
                else if (map.localPosition.x < maxLeft_x)
                {
                    map.localPosition = new Vector3(maxLeft_x, map.localPosition.y, 0);
                    return false;
                }
            }
        }

        // distance2
        {
            Transform maxLeftObj = null;
            Transform maxRightObj = null;

            for (int i = 0; i < distance2.transform.childCount; i++)
            {
                Transform trans = distance2.transform.GetChild(i);

                if (maxLeftObj == null)
                {
                    maxLeftObj = trans;
                }
                else
                {
                    if (distance2.transform.GetChild(i).localPosition.x < maxLeftObj.localPosition.x)
                    {
                        maxLeftObj = trans;
                    }
                }
                if (maxRightObj == null)
                {
                    maxRightObj = trans;
                }
                else
                {
                    if (distance2.transform.GetChild(i).localPosition.x > maxRightObj.localPosition.x)
                    {
                        maxRightObj = trans;
                    }
                }

                if(x != 0)
                {
                    trans.localPosition += new Vector3(x / 2, 0, 0);
                }
            }

            if (x < 0)
            {
                if (maxLeftObj.localPosition.x < -maxLeftObj.GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    maxLeftObj.localPosition = new Vector3(maxRightObj.localPosition.x + maxLeftObj.GetComponent<RectTransform>().sizeDelta.x, maxLeftObj.localPosition.y, 0);
                }
            }
            else if (x > 0)
            {
                if (maxRightObj.localPosition.x > maxRightObj.GetComponent<RectTransform>().sizeDelta.x * (distance2.transform.childCount - 1))
                {
                    maxRightObj.localPosition = new Vector3(maxLeftObj.localPosition.x - maxRightObj.GetComponent<RectTransform>().sizeDelta.x, maxRightObj.localPosition.y, 0);
                }
            }
        }

        // distance3
        {
            Transform maxLeftObj = null;
            Transform maxRightObj = null;

            for (int i = 0; i < distance3.transform.childCount; i++)
            {
                Transform trans = distance3.transform.GetChild(i);

                if (maxLeftObj == null)
                {
                    maxLeftObj = trans;
                }
                else
                {
                    if (distance3.transform.GetChild(i).position.x < maxLeftObj.position.x)
                    {
                        maxLeftObj = trans;
                    }
                }
                if (maxRightObj == null)
                {
                    maxRightObj = trans;
                }
                else
                {
                    if (distance3.transform.GetChild(i).position.x > maxRightObj.position.x)
                    {
                        maxRightObj = trans;
                    }
                }

                if (x != 0)
                {
                    trans.localPosition += new Vector3(x / 3, 0, 0);
                }
            }

            if (x < 0)
            {
                if (maxLeftObj.localPosition.x < -maxLeftObj.GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    maxLeftObj.localPosition = new Vector3(maxRightObj.localPosition.x + maxLeftObj.GetComponent<RectTransform>().sizeDelta.x, maxLeftObj.localPosition.y, 0);
                }
            }
            else if (x > 0)
            {
                if (maxRightObj.localPosition.x > maxRightObj.GetComponent<RectTransform>().sizeDelta.x * (distance3.transform.childCount - 1))
                {
                    maxRightObj.localPosition = new Vector3(maxLeftObj.localPosition.x - maxRightObj.GetComponent<RectTransform>().sizeDelta.x, maxRightObj.localPosition.y, 0);
                }
            }
        }

        return true;
    }

    public bool moveY(float y)
    {
        // distance1
        {
            if (y != 0)
            {
                map.localPosition += new Vector3(0, y, 0);
                if (map.position.y > 0)
                {
                    map.position = new Vector3(map.position.x, 0, 0);
                    return false;
                }
            }
        }

        return true;
    }
}
