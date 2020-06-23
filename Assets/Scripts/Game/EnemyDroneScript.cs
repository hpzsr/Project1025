using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDroneScript : MonoBehaviour {

	public Image self_img;
	public Image bomb_img;
    public Image blood_img;

    bool isDie = false;
    // float bloodWidth;
    float fullBlood = 10;
    float curBlood;

    public static EnemyDroneScript Create(Transform parent)
	{
		GameObject pre = Resources.Load("Prefabs/Game/EnemyDrone", typeof(GameObject)) as GameObject;
		GameObject bullet = GameObject.Instantiate(pre, parent);
		EnemyDroneScript script = bullet.GetComponent<EnemyDroneScript>();

		return script;
	}

	void Start () {
        curBlood = fullBlood;
        // bloodWidth = blood_img.GetComponent<RectTransform>().sizeDelta.x;

        EnemyManager.addEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());
	}
	
	void Update () {
		
	}

	public bool hurt(float damage)
	{
        if(isDie)
        {
            return false;
        }

        curBlood -= damage;
        if(curBlood < 0)
        {
            curBlood = 0;
            die();
        }

        // 设置血条进度
        {
            blood_img.transform.localScale = new Vector3(curBlood / fullBlood, 1,1);
        }

        return true;
    }

	public void die()
	{
        isDie = true;
        blood_img.transform.parent.localScale = new Vector3(0,0,0);
        showBombEffect();
	}

	public void showBombEffect()
	{
		bomb_img.transform.localScale = new Vector3(1,1,1);
		FrameAnimationUtil.getInstance().startAnimation(bomb_img, "Sprites/enemy-explosion/enemy-explosion-", Consts.FrameAnimationSpeed.low,false,()=>
		{
            EnemyManager.destroyEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());
		});
	}
}
