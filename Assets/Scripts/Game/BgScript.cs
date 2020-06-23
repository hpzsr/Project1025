using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScript : MonoBehaviour
{
    public static BgScript s_instance = null;

    public GameObject distance1;
    public GameObject distance2;
    public GameObject distance3;

    void Start()
    {
        s_instance = this;
    }
    
    void Update()
    {
    }

    public void move(float x,float y)
    {
        // distance1
        {
            if (x != 0)
            {
                distance1.transform.localPosition += new Vector3(x,0,0);
            }

            if (y != 0)
            {
                distance1.transform.localPosition += new Vector3(0, y, 0);
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
                if (maxLeftObj.position.x < -maxLeftObj.GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    maxLeftObj.localPosition = new Vector3(maxRightObj.localPosition.x + maxLeftObj.GetComponent<RectTransform>().sizeDelta.x, maxLeftObj.localPosition.y, 0);
                }
            }
            else if (x > 0)
            {
                if (maxRightObj.position.x > (Screen.width + maxRightObj.GetComponent<RectTransform>().sizeDelta.x / 2))
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
                    if (distance3.transform.GetChild(i).localPosition.x < maxLeftObj.localPosition.x)
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
                    if (distance3.transform.GetChild(i).localPosition.x > maxRightObj.localPosition.x)
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
                if (maxLeftObj.position.x < -maxLeftObj.GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    maxLeftObj.localPosition = new Vector3(maxRightObj.localPosition.x + maxLeftObj.GetComponent<RectTransform>().sizeDelta.x, maxLeftObj.localPosition.y, 0);
                }
            }
            else if (x > 0)
            {
                if (maxRightObj.position.x > (Screen.width + maxRightObj.GetComponent<RectTransform>().sizeDelta.x / 2))
                {
                    maxRightObj.localPosition = new Vector3(maxLeftObj.localPosition.x - maxRightObj.GetComponent<RectTransform>().sizeDelta.x, maxRightObj.localPosition.y, 0);
                }
            }
        }
    }
}
