using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultScript : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void onClickRestart()
    {
        GameObject.Destroy(gameObject);
        GameScript.s_instance.restart();
    }
}
