using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public static GameScript s_instance = null;

    public Image blood_img;

    public Text info_state;
    public Text info_jumpSpeed;
    public Text info_dropSpeed;
    public Transform jieshao;
    public Button btn_jieshao;
    public Button btn_closeJieShao;
    public Transform gm;
    public Button btn_gm;

    void Start()
    {
        Application.targetFrameRate = 60;

        s_instance = this;
        GameObject map = CommonUtil.createObjFromPrefab(transform.Find("bg/distance1"), "Prefabs/Game/map/map1");
        BgScript.s_instance.setMap(map.transform);
    }
    
    void Update()
    {
        if (PlayerScript.s_instance)
        {
            info_state.text = "状态：" + PlayerScript.s_instance.playerState.ToString();

            //if (PlayerScript.s_instance.playerState == Consts.PlayerState.jump)
            //{
            //    info_jumpSpeed.text = "跳速：" + PlayerScript.s_instance.curJumpPower;
            //}
            //else
            //{
            //    info_jumpSpeed.text = "跳速：0";
            //}

            //if (PlayerScript.s_instance.playerState == Consts.PlayerState.drop)
            //{
            //    info_dropSpeed.text = "降速：" + PlayerScript.s_instance.curJumpPower;
            //}
            //else
            //{
            //    info_dropSpeed.text = "降速：0";
            //}
        }
    }

    public void restart()
    {
        FrameAnimationUtil.getInstance().clearAll();
        GameObject.Destroy(BgScript.s_instance.map.gameObject);

        GameObject map = CommonUtil.createObjFromPrefab(transform.Find("bg/distance1"), "Prefabs/Game/map/map1");
        BgScript.s_instance.setMap(map.transform);
    }

    public void onClickJieShao()
    {
        jieshao.transform.localScale = new Vector3(1,1,1);
    }

    public void onClickCloseJieShao()
    {
        jieshao.transform.localScale = new Vector3(0,0,0);
    }

    public void onClickGM()
    {
        GMScript.s_instance.show();
    }
}
