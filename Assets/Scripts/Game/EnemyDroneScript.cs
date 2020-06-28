using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDroneScript : MonoBehaviour {

	public Image self_img;
	public Image bomb_img;
    public Image blood_bg_img;
    public Image blood_img;

    bool isDie = false;
    float fullBlood = 10;
    float curBlood;
    float shootDurTime = 3;

    public Consts.MoveDirection direction = Consts.MoveDirection.right;

    public static EnemyDroneScript Create(Transform parent, Consts.MoveDirection _direction)
	{
		GameObject pre = Resources.Load("Prefabs/Game/EnemyDrone", typeof(GameObject)) as GameObject;
		GameObject bullet = GameObject.Instantiate(pre, parent);
		EnemyDroneScript script = bullet.GetComponent<EnemyDroneScript>();
        script.direction = _direction;
        return script;
	}

	void Start () {
        curBlood = fullBlood;
        
        EnemyManager.addEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());

        if(direction == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            blood_bg_img.transform.localScale = new Vector3(-1,1,1);
        }

        InvokeRepeating("onShoot", shootDurTime, shootDurTime);
	}
	
	void Update () {
		
	}

    void onShoot()
    {
        if (!isDie)
        {
            EnemyBulletScript.Create(transform, direction);
        }
    }

    public bool hurt(float damage)
	{
        if(isDie)
        {
            return false;
        }

        curBlood -= damage;
        if(curBlood <= 0)
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
        CancelInvoke("onShoot");
        blood_img.transform.parent.localScale = new Vector3(0,0,0);
        showBombEffect();
        self_img.DOFade(0, 0.6f);
    }

	public void showBombEffect()
	{
		bomb_img.transform.localScale = new Vector3(1,1,1);
		FrameAnimationUtil.getInstance().startAnimation(bomb_img, "Sprites/enemy-explosion/enemy-explosion-", FrameAnimationUtil.FrameAnimationSpeed.low,false,()=>
		{
            EnemyManager.destroyEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());
		});
	}
}
