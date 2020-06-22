﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CommonUtil
{
    static Material ImageGray = null;

    // 格式2017/7/12 15:05:03
    public static string getCurTime()
    {
        return DateTime.Now.ToString();
    }

    /// <summary>
    /// 32位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string GetMD5(string password)
    {
        string cl = password;
        string pwd = "";
        MD5 md5 = MD5.Create(); //实例化一个md5对像
        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            pwd = pwd + s[i].ToString("X2");
        }
        return pwd;
    }

    // 格式2017/7
    static public string getCurYearMonth()
    {
        return DateTime.Now.Year + "/" + DateTime.Now.Month;
    }

    // 格式2017/7/10
    static public string getCurYearMonthDay()
    {
        return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
    }

    static public int getCurYear()
    {
        return DateTime.Now.Year;
    }

    static public int getCurMonth()
    {
        return DateTime.Now.Month;
    }

    static public int getCurDay()
    {
        return DateTime.Now.Day;
    }

    static public int getCurHour()
    {
        return DateTime.Now.Hour;
    }

    static public int getCurMinute()
    {
        return DateTime.Now.Minute;
    }

    static public int getCurSecond()
    {
        return DateTime.Now.Second;
    }

    static public int getCurMonthAllDays()
    {
        return DateTime.DaysInMonth(getCurYear(), getCurMonth());
    }

    static public bool splitStrIsPerfect(string str, List<string> list, char c)
    {
        bool b = false;
        {
            if (str[str.Length - 1] == c)
            {
                b = true;
            }
        }

        splitStr(str, list, c);

        return b;
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * list：裁剪后存放的地方
     * c：裁剪规则
     * 如：splitStr("1.2.3.4.5",list,'.');
     */
    static public void splitStr(string str, List<string> list, char c)
    {
        string temp = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != c)
            {
                temp += str[i];
            }
            else
            {
                list.Add(temp);
                temp = "";
            }

            if ((str[i] != c) && (i == (str.Length - 1)))
            {
                list.Add(temp);
                temp = "";
            }
        }
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * c：裁剪规则
     * 返回1
     */
    static public int splitStr_Start(string str, char c)
    {
        List<string> list = new List<string>();
        splitStr(str, list, c);

        return int.Parse(list[0]);
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * c：裁剪规则
     * 返回200
     */
    static public int splitStr_End(string str, char c)
    {
        List<string> list = new List<string>();
        splitStr(str, list, c);

        return int.Parse(list[1]);
    }

    /*
     * subStringEndByChar("1/2/3/4/5/6",'/')
     * 返回6
     */
    static public string subStringEndByChar(string str, char c)
    {
        return str.Substring(str.LastIndexOf(c) + 1);
    }

    // size：物品数量
    // jiange：物品间隔
    // index：物品序号（从0开始）
    // centerPosX：居中位置坐标
    static public int getPosX(int size, int jiange, int index, int centerPosX)
    {
        int firstX;
        if (size % 2 == 0)
        {
            firstX = (centerPosX - jiange / 2) - (size / 2 - 1) * jiange;
        }
        else
        {
            firstX = centerPosX - (size / 2) * jiange;
        }

        return firstX + jiange * index;
    }

    static public bool isStrContain(string sourceStr, string containStr)
    {
        if (containStr.CompareTo("") == 0)
        {
            return false;
        }

        if (sourceStr.Length < containStr.Length)
        {
            return false;
        }

        for (int i = 0; i <= sourceStr.Length - containStr.Length; i++)
        {
            string temp = "";
            for (int j = i; j < (i + containStr.Length); j++)
            {
                temp += sourceStr[j];
            }

            if (temp.CompareTo(containStr) == 0)
            {
                return true;
            }
        }

        return false;
    }

    static public void setImageSprite(Image image, string path)
    {
        image.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    static public void setImageColor(Image image, float r, float g, float b)
    {
        image.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    static public void setImageColor(Image image, float r, float g, float b, float a)
    {
        image.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

    static public void setFontColor(Text text, float r, float g, float b)
    {
        text.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    static public void setButtonEnable(Button btn, bool enable)
    {
        if (enable)
        {
            btn.interactable = true;
            setImageColor(btn.transform.Find("Image").GetComponent<Image>(), 255, 255, 255, 255);
        }
        else
        {
            btn.interactable = false;
            setImageColor(btn.transform.Find("Image").GetComponent<Image>(), 255, 255, 255, 125);
        }
    }

    //字符转ASCII码：
    //character长度只能为1
    static public int charToAsc(string character)
    {
        System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
        int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];

        return intAsciiCode;
    }

    // 天数差
    // data_old:xxxx-xx-xx xx:xx:xx
    // data_new:xxxx-xx-xx xx:xx:xx
    static public int tianshucha(string data_old, string data_new)
    {
        DateTime d1 = Convert.ToDateTime(data_old);
        DateTime d2 = Convert.ToDateTime(data_new);
        DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));
        DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
        int days = (d4 - d3).Days;

        return days;
    }

    // 秒数差
    // data_old:xxxx-xx-xx xx:xx:xx
    // data_new:xxxx-xx-xx xx:xx:xx
    static public int miaoshucha(string data_old, string data_new)
    {
        DateTime d1 = Convert.ToDateTime(data_old);
        DateTime d2 = Convert.ToDateTime(data_new);
        DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", d1.Year, d1.Month, d1.Day, d1.Hour, d1.Minute, d1.Second));
        DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", d2.Year, d2.Month, d2.Day, d2.Hour, d2.Minute, d2.Second));

        int days = (d4 - d3).Days;
        int hours = (d4 - d3).Hours;
        int minutes = (d4 - d3).Minutes;
        int seconds = (d4 - d3).Seconds;

        TimeSpan t1 = new TimeSpan(days, hours, minutes, seconds);
        int i = (int)t1.TotalSeconds;

        return i;
    }

    static public void setGray(Image img, bool isGray)
    {
        if (isGray)
        {
            if (ImageGray == null)
            {
                ImageGray = Resources.Load("materials/ImageGray", typeof(Material)) as Material;
            }

            img.material = ImageGray;
        }
        else
        {
            img.material = null;
        }
    }

    static public Sprite getSprite(string path)
    {
        return Resources.Load("Sprites/" + path, typeof(Sprite)) as Sprite;
    }

    static public Vector3 TouchPosToWorldPos(Vector2 vec2)
    {
        // 相机是世界的，世界到屏幕
        Vector3 camera = Camera.main.WorldToScreenPoint(GameObject.Find("Main Camera").transform.position);
        Vector3 pos = new Vector3(vec2.x, vec2.y, camera.z);
        Vector3 WorldPoint = Camera.main.ScreenToWorldPoint(pos);
        return new Vector3(WorldPoint.x, WorldPoint.y, 0);
    }

    static public bool mousePosIsInContent(Vector2 vec2,Transform content)
    {
        vec2 = TouchPosToWorldPos(vec2);

        float x = content.transform.position.x;
        float y = content.transform.position.y;

        float width = content.GetComponent<RectTransform>().sizeDelta.x / 100;
        float height = content.GetComponent<RectTransform>().sizeDelta.y / 100;
        if ((vec2.x >= (x - width / 2)) &&
            (vec2.x <= (x + width / 2)) &&
            (vec2.y >= (y - height / 2)) &&
            (vec2.y <= (y + height / 2)))
        {
            return true;
        }

        return false;
    }

    static public bool uiPosIsInContent(Vector2 vec2, Transform content)
    {
        float x = content.transform.localPosition.x;
        float y = content.transform.localPosition.y;

        float width = content.GetComponent<RectTransform>().sizeDelta.x;
        float height = content.GetComponent<RectTransform>().sizeDelta.y;
        if ((vec2.x >= (x - width / 2)) &&
            (vec2.x <= (x + width / 2)) &&
            (vec2.y >= (y - height / 2)) &&
            (vec2.y <= (y + height / 2)))
        {
            return true;
        }

        return false;
    }

    static public void setScrollViewEnable(Transform scrollView, bool enable)
    {
        scrollView.GetComponent<ScrollRect>().enabled = enable;
    }

    static public void setVisible(Transform obj , bool visible)
    {
        obj.localScale = new Vector3(visible ? 1 : 0, 1,1);
    }

    static public float twoObjDistance_3D(GameObject obj1,GameObject obj2)
    {
        return (obj1.transform.position - obj2.transform.position).magnitude;
    }

    static public float twoObjDistance_2D(GameObject obj1, GameObject obj2)
    {
        Vector2 pos1 = new Vector2(obj1.transform.position.x, obj1.transform.position.z);
        Vector2 pos2 = new Vector2(obj2.transform.position.x, obj2.transform.position.z);

        return (pos1 - pos2).magnitude;
    }

    static public float twoObjDistance_2D_2(GameObject obj1, GameObject obj2)
    {
        Vector2 pos1 = new Vector2(obj1.transform.position.y, obj1.transform.position.z);
        Vector2 pos2 = new Vector2(obj2.transform.position.y, obj2.transform.position.z);

        return (pos1 - pos2).magnitude;
    }

    static public float angleToRadian(float angle)
    {
        return angle * Mathf.Deg2Rad;
    }

}
