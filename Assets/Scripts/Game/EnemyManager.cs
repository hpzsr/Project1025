using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public static List<EnemyDroneScript> enemyDroneList = new List<EnemyDroneScript>();

    public static void addEnemyDrone(EnemyDroneScript script)
    {
        enemyDroneList.Add(script);
    }

    public static void destroyEnemyDrone(EnemyDroneScript script)
    {
        for(int i = 0; i < enemyDroneList.Count; i++)
        {
            if(enemyDroneList[i] == script)
            {
                GameObject.Destroy(script.gameObject);
                enemyDroneList.RemoveAt(i);
                break;
            }
        }
    }
}
