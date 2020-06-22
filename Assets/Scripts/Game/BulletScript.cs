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

        Vector3 pos = PlayerScript.s_instance.transform.localPosition;
        if (moveDirection == Consts.MoveDirection.left)
        {
            transform.localPosition = new Vector3(pos.x - 30, pos.y + 10,0);
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.localPosition = new Vector3(pos.x + 30, pos.y + 10, 0);
        }
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

        float x = transform.localPosition.x;
        if (x < -(Screen.width / 2 + 100))
        {
            DestroySelf();
            return;
        }
        else if (x > Screen.width / 2 + 100)
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
            if (CommonUtil.uiPosIsInContent(transform.localPosition, script.transform))
            {
                script.hurt();
                DestroySelf();
            }
        }
    }

    public void DestroySelf()
    {
        FrameAnimationUtil.getInstance().stopAnimation(self_img);
        Destroy(gameObject);
    }
}
