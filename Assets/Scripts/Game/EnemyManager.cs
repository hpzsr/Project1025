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
}
