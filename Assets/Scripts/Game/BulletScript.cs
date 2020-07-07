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
        GameObject bullet = GameObject.Instantiate(pre, BgScript.s_instance.map);
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.moveDirection = _moveDirection;
        return script;
    }

    void Start()
    {
        self_img = gameObject.GetComponent<Image>();
        FrameAnimationUtil.getInstance().startAnimation(self_img, "Sprites/bullet/bullet-", FrameAnimationUtil.FrameAnimationSpeed.low);

        PlayerScript player = PlayerScript.s_instance;
        switch(player.playerState)
        {
            case Consts.PlayerState.crouch:
                {
                    transform.position = player.crouchShootPoint.transform.position;
                    break;
                }

            default:
                {
                    transform.position = player.standShootPoint.transform.position;
                    break;
                }
        }

        if (moveDirection == Consts.MoveDirection.left)
        {
            transform.localScale = new Vector3(-1,1,1);
            
        }
        else if (moveDirection == Consts.MoveDirection.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
    }

    public void DestroySelf()
    {
        FrameAnimationUtil.getInstance().stopAnimation(self_img);
        Destroy(gameObject);
    }
}
