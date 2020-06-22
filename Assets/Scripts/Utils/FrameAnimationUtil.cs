using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void AnimationEnd();

public class FrameData
{
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    public Consts.FrameAnimationSpeed speed = Consts.FrameAnimationSpeed.normal;
    public int curIndex = 0;
    public bool isLoop = true;
    public AnimationEnd animationEnd = null;

    public FrameData(Image _image, string path, Consts.FrameAnimationSpeed _speed, bool _isLoop = true, AnimationEnd _animationEnd = null)
    {
        image = _image;
        speed = _speed;
        isLoop = _isLoop;
        animationEnd = _animationEnd;

        for (int i = 1; i < 100; i++)
        {
            Sprite sprite = Resources.Load(path + i, typeof(Sprite)) as Sprite;
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
            else
            {
                break;
            }
        }
        image.sprite = sprites[curIndex];
    }
}

public class FrameAnimationUtil : MonoBehaviour
{
    public static FrameAnimationUtil instance = null;
    public List<FrameData> frameDataList = new List<FrameData>();

    static public FrameAnimationUtil getInstance()
    {
        if (FrameAnimationUtil.instance == null)
        {
            GameObject obj = new GameObject();
            FrameAnimationUtil.instance = obj.AddComponent<FrameAnimationUtil>();
            obj.name = "FrameAnimationUtil";
            DontDestroyOnLoad(obj);
        }

        return FrameAnimationUtil.instance;
    }

    void Start()
    {
        InvokeRepeating("Timer_low", 0.1f, 1.0f / 6.0f);
        InvokeRepeating("Timer_normal", 0.1f, 1.0f / 12.0f);
    }

    public void startAnimation(Image _image, string path ,Consts.FrameAnimationSpeed _speed, bool _isLoop = true, AnimationEnd _animationEnd = null)
    {
        if(getFrameData(_image) != null)
        {
            stopAnimation(_image);
        }

        FrameData data = new FrameData(_image, path, _speed, _isLoop, _animationEnd);
        frameDataList.Add(data);
    }

    public void stopAnimation(Image _image)
    {
        FrameData data = getFrameData(_image);
        if(data != null)
        {
            frameDataList.Remove(data);
        }
    }

    FrameData getFrameData(Image _image)
    {
        for (int i = 0; i < frameDataList.Count; i++)
        {
            if (frameDataList[i].image == _image)
            {
                return frameDataList[i];
            }
        }

        return null;
    }

    void Timer_low()
    {
        for (int i = 0; i < frameDataList.Count; i++)
        {
            if (frameDataList[i].speed == Consts.FrameAnimationSpeed.low)
            {
                if (frameDataList[i].curIndex >= (frameDataList[i].sprites.Count - 1))
                {
                    if (frameDataList[i].isLoop)
                    {
                        frameDataList[i].curIndex = 0;
                    }
                    else
                    {
                        if (frameDataList[i].animationEnd != null)
                        {
                            frameDataList[i].animationEnd();
                        }
                        return;
                    }
                }
                else
                {
                    ++frameDataList[i].curIndex;
                }
                frameDataList[i].image.sprite = frameDataList[i].sprites[frameDataList[i].curIndex];
            }
        }
    }

    void Timer_normal()
    {
        for(int i = 0; i < frameDataList.Count; i++)
        {
            if(frameDataList[i].speed == Consts.FrameAnimationSpeed.normal)
            {
                if(frameDataList[i].curIndex >= (frameDataList[i].sprites.Count - 1))
                {
                    if (frameDataList[i].isLoop)
                    {
                        frameDataList[i].curIndex = 0;
                    }
                    else
                    {
                        if (frameDataList[i].animationEnd != null)
                        {
                            frameDataList[i].animationEnd();
                        }
                    }
                }
                else
                {
                    ++frameDataList[i].curIndex;
                }
                frameDataList[i].image.sprite = frameDataList[i].sprites[frameDataList[i].curIndex];
            }
        }
    }
}