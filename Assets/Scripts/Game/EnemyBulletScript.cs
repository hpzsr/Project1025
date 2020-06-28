using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBulletScript : MonoBehaviour
{
    Image self_img;
    public Consts.MoveDirection moveDirection;
    public Transform parent;

    float moveSpeed = 3.0f;

    public static EnemyBulletScript Create(Transform _parent,Consts.MoveDirection _moveDirection)
    {
        GameObject pre = Resources.Load("Prefabs/Game/EnemyBullet", typeof(GameObject)) as GameObject;
        GameObject bullet = GameObject.Instantiate(pre, GameObject.Find("Canvas/GameLayer/bg/distance1/map").transform);
        EnemyBulletScript script = bullet.GetComponent<EnemyBulletScript>();
        script.setData(_parent, _moveDirection);
        return script;
    }

    public void setData(Transform _parent, Consts.MoveDirection _moveDirection)
    {
        moveDirection = _moveDirection;
        parent = _parent;
    }

    void Start()
    {
        self_img = gameObject.GetComponent<Image>();
        FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/bullet/bullet-", FrameAnimationUtil.FrameAnimationSpeed.low);
        
        float parentHeight = parent.GetComponent<RectTransform>().sizeDelta.y;
        Vector3 pos = parent.position;
        
        if (moveDirection == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(-1,1,1);
            transform.position = new Vector3(pos.x - 30, pos.y, 0);
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(pos.x + 30, pos.y, 0);
        }
    }

    void Update()
    {
        if(moveDirection == Consts.MoveDirection.left)
        {
            transform.position -= new Vector3(moveSpeed, 0, 0);
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.position += new Vector3(moveSpeed, 0, 0);
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
        if (CommonUtil.uiPosIsInContent(transform.position, PlayerScript.s_instance.transform, PlayerScript.s_instance.getCollsionSize()))
        {
            //Debug.Log("打中人");
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        FrameAnimationUtil.getInstance().stopAnimation(self_img);
        Destroy(gameObject);
    }
}
