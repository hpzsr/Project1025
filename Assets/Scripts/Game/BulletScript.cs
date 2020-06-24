using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{
    Image self_img;
    public Consts.MoveDirection moveDirection;

    float moveSpeed = 6.0f;

    public static BulletScript Create(Transform parent,Consts.MoveDirection _moveDirection)
    {
        GameObject pre = Resources.Load("Prefabs/Game/Bullet", typeof(GameObject)) as GameObject;
        GameObject bullet = GameObject.Instantiate(pre, parent);
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.moveDirection = _moveDirection;
        return script;
    }

    void Start()
    {
        self_img = gameObject.GetComponent<Image>();
        FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/bullet/bullet-", Consts.FrameAnimationSpeed.low);

        float parentHeight = PlayerScript.s_instance.GetComponent<RectTransform>().sizeDelta.y;
        Vector3 pos = PlayerScript.s_instance.transform.localPosition;
        float posY = pos.y + parentHeight * 0.6f;
        if (PlayerScript.s_instance.playerState == Consts.PlayerState.crouch)
        {
            posY = pos.y + parentHeight * 0.35f;
        }
        else if (PlayerScript.s_instance.playerState == Consts.PlayerState.drop)
        {
            posY = pos.y + parentHeight * 0.6f;
        }
        if (moveDirection == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(-1,1,1);
            transform.localPosition = new Vector3(pos.x - 30, posY, 0);
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(pos.x + 30, posY, 0);
        }

        transform.SetParent(GameObject.Find("Canvas/GameLayer/bg/distance1").transform);
    }

    void Update()
    {
        if(moveDirection == Consts.MoveDirection.left)
        {
            transform.localPosition -= new Vector3(moveSpeed, 0, 0);
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.localPosition += new Vector3(moveSpeed, 0, 0);
        }

        float x = transform.position.x;
        if (x < 0)
        {
            DestroySelf();
            return;
        }
        else if (x > Screen.width)
        {
            DestroySelf();
            return;
        }

        checkCollision();
    }

    void checkCollision()
    {
        for(int i = 0; i < EnemyManager.enemyDroneList.Count; i++)
        {
            EnemyDroneScript script = EnemyManager.enemyDroneList[i];
            if (CommonUtil.uiPosIsInContent(transform.position, script.transform))
            {
                if(script.hurt(PlayerScript.s_instance.damage))
                {
                    DestroySelf();
                }
            }
        }
    }

    public void DestroySelf()
    {
        FrameAnimationUtil.getInstance().stopAnimation(self_img);
        Destroy(gameObject);
    }
}
