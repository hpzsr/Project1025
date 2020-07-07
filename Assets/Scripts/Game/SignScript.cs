using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{
    public string path;
    public Image self_img;

    void Start()
    {
        FrameAnimationUtil.getInstance().startAnimation(self_img,path,FrameAnimationUtil.FrameAnimationSpeed.low);    
    }
}
