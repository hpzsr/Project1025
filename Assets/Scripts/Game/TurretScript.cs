using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretScript : MonoBehaviour {

	public Image self_img;
	public Image bomb_img;
    public Image blood_bg_img;
    public Image blood_img;

    bool isDie = false;
    float fullBlood = 10;
    float curBlood;
    float shootDurTime = 3;

    public Consts.MoveDirection direction = Consts.MoveDirection.right;

    public static TurretScript Create(Transform parent, Consts.MoveDirection _direction)
	{
		GameObject pre = Resources.Load("Prefabs/Game/EnemyDrone", typeof(GameObject)) as GameObject;
		GameObject bullet = GameObject.Instantiate(pre, parent);
        TurretScript script = bullet.GetComponent<TurretScript>();
        script.direction = _direction;
        return script;
	}

	void Start () {
        curBlood = fullBlood;
        
        EnemyManager.addTurret(gameObject.GetComponent<TurretScript>());

        shootDurTime = RandomUtil.getRandom(1,5);
        InvokeRepeating("onShoot", shootDurTime, shootDurTime);
        FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/turret/turret-", FrameAnimationUtil.FrameAnimationSpeed.low);

        if (direction == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (direction == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
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
        else
        {
            self_img.color = new Color(255,0,0);
            TimerUtil.getInstance().delayTime(0.1f,()=>
            {
                self_img.color = new Color(255, 255, 255);
            });
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
		FrameAnimationUtil.getInstance().startAnimation(bomb_img, "Sprites/enemy-explosion/enemy-explosion-", FrameAnimationUtil.FrameAnimationSpeed.normal, false,()=>
		{
            EnemyManager.destroyTurret(gameObject.GetComponent<TurretScript>());
		});
	}

    void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.tag == "PlayerBullet")
        {
            if (hurt(PlayerScript.s_instance.damage))
            {
                collidedObject.GetComponent<BulletScript>().DestroySelf();
            }
        }
    }
}
