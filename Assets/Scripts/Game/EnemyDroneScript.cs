using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDroneScript : MonoBehaviour {

	public Image self_img;
	public Image bomb_img;

	public static EnemyDroneScript Create(Transform parent)
	{
		GameObject pre = Resources.Load("Prefabs/Game/EnemyDrone", typeof(GameObject)) as GameObject;
		GameObject bullet = GameObject.Instantiate(pre, parent);
		EnemyDroneScript script = bullet.GetComponent<EnemyDroneScript>();

		return script;
	}

	void Start () {
		EnemyManager.addEnemyDrone(gameObject.GetComponent<EnemyDroneScript>());
	}
	
	void Update () {
		
	}

	public void hurt()
	{
		showBombEffect();
	}

	public void die()
	{
		showBombEffect();
	}

	public void showBombEffect()
	{
		bomb_img.transform.localScale = new Vector3(1,1,1);
		FrameAnimationUtil.getInstance().startAnimation(bomb_img, "Sprites/enemy-explosion/enemy-explosion-", Consts.FrameAnimationSpeed.low,false,()=>
		{
			bomb_img.transform.localScale = new Vector3(0,0,0);
		});
	}
}
