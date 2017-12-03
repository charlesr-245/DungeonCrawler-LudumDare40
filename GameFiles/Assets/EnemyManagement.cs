using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour {

    public int[] enemiesPerRoom;
    public int extraEnemiesBeforeSuperBoss;

    public int maxSuperBossesPerRoom;

    public GameObject superBoss;

    private List<int> enemiesInRoom;

    private void Start()
    {
        enemiesInRoom = new List<int>();
        for (int x = 0; x<enemiesPerRoom.Length; x++)
        {
            enemiesInRoom.Add(enemiesPerRoom[x]);
        }
    }

    public void AddEnemies(int amount, int zone, Transform t)
    {
        enemiesInRoom[zone] += amount;
        CheckEnemies(zone, t);
    }

    public void CheckEnemies(int zone, Transform t)
    {
        if (enemiesInRoom[zone] >= enemiesPerRoom[zone]+extraEnemiesBeforeSuperBoss)
        {
            SpawnSuperBoss(zone,t);
        }
    }

    private void SpawnSuperBoss(int zone, Transform t)
    {
        Instantiate(superBoss, t.position,Quaternion.identity);
        Destroy(t.gameObject);
        AddEnemies(-extraEnemiesBeforeSuperBoss, zone, t);
    }

}
