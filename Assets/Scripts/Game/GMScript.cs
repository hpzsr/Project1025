using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMScript : MonoBehaviour
{
    public static GMScript s_instance = null;

    public Button btn_closeGM;
    public InputField input_speed;
    public InputField input_damage;

    void Start()
    {
        s_instance = this;

        if (PlayerScript.s_instance)
        {
            input_speed.text = PlayerScript.s_instance.runSpeed.ToString();
            input_damage.text = PlayerScript.s_instance.damage.ToString();
        }
    }

    public void show()
    {
        input_speed.text = PlayerScript.s_instance.runSpeed.ToString();
        input_damage.text = PlayerScript.s_instance.damage.ToString();
        transform.localScale = new Vector3(1,1,1);
    }

    public void onClickCloseGM()
    {
        if (input_speed.text != "")
        {
            PlayerScript.s_instance.runSpeed = float.Parse(input_speed.text);
        }

        if (input_damage.text != "")
        {
            PlayerScript.s_instance.damage = float.Parse(input_damage.text);
        }

        transform.localScale = new Vector3(0, 0, 0);
    }
}
