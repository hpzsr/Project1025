using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDroneScript : MonoBehaviour {

	public Image self_img;
    public Image blood_bg_img;
    public Image blood_img;
	public Image bomb_img;

    public bool isCanDamage = true;
    public bool isDie = false;
    float fullBlood = 10;
    float curBlood;
    float shootDurTime = 5;

    Tweener tweener = null;

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

        setDirection(direction);

        //InvokeRepeating("onShoot", shootDurTime, shootDurTime);
        InvokeRepeating("changeDirection", 7, 7);
    }
	
	void Update () {
		
	}

    public void setDirection(Consts.MoveDirection _direction)
    {
        float moveToX = 0;
        direction = _direction;
        if(_direction == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(1, 1, 1);
            blood_bg_img.transform.localScale = new Vector3(1, 1, 1);
            moveToX = transform.localPosition.x - 200;
        }
        else if(_direction == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            blood_bg_img.transform.localScale = new Vector3(-1, 1, 1);
            moveToX = transform.localPosition.x + 200;
        }

        tweener = self_img.rectTransform.DOAnchorPosX(moveToX, 3.0f, false).SetEase(Ease.Linear).SetDelay(1.0f);
    }

    void changeDirection()
    {
        FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/drone/drone-", FrameAnimationUtil.FrameAnimationSpeed.low, false, () =>
        {
            if (direction == Consts.MoveDirection.left)
            {
                self_img.sprite = CommonUtil.getSprite("Sprites/drone/drone-1");
                setDirection(Consts.MoveDirection.right);
            }
            else if (direction == Consts.MoveDirection.right)
            {
                self_img.sprite = CommonUtil.getSprite("Sprites/drone/drone-1");
                setDirection(Consts.MoveDirection.left);
            }
        });
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
        if (tweener != null)
        {
            tweener.Kill();
        }
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
            EnemyManager.destroyEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());
		});
	}

    public void setCanDamage(bool b)
    {
        isCanDamage = b;

        if(!isCanDamage)
        {
            // 1秒内不能造成伤害
            TimerUtil.getInstance().delayTime(1.0f,()=> {
                setCanDamage(true);
            });
        }
    }

    public bool getIsCanDamage()
    {
        return !isDie && isCanDamage;
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
