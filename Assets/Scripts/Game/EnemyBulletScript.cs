﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBulletScript : MonoBehaviour
{
    Image self_img;
    public Consts.MoveDirection moveDirection;
    public Transform parent;

    float moveSpeed = 5.0f;
    float damage = 1.0f;

    public static EnemyBulletScript Create(Transform _parent,Consts.MoveDirection _moveDirection)
    {
        GameObject pre = Resources.Load("Prefabs/Game/EnemyBullet", typeof(GameObject)) as GameObject;
        GameObject bullet = GameObject.Instantiate(pre, BgScript.s_instance.map);
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
        transform.position = parent.Find("shootPoint").position;
    }

    void Update()
    {
        if (moveDirection == Consts.MoveDirection.left)
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
    }

    public void DestroySelf()
    {
        FrameAnimationUtil.getInstance().stopAnimation(self_img);
        Destroy(gameObject);
    }
}
