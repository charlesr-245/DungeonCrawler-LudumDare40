using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour {

    public int[] enemiesPerRoom;
    public int extraEnemiesBeforeSuperBoss;

    public int maxSuperBossesPerRoom;
    private int[] superBossesInRoom;

    public GameObject superBoss;

    private List<int> enemiesInRoom;

    private void Start()
    {
        superBossesInRoom = new int[enemiesPerRoom.Length];
        enemiesInRoom = new List<int>();
        for (int x = 0; x<enemiesPerRoom.Length; x++)
        {
            enemiesInRoom.Add(enemiesPerRoom[x]);
        }
    }

    public bool superBossInRoom(int zone)
    {
        Debug.Log(superBossesInRoom[zone]);
        if (superBossesInRoom[zone]>0)
        {
            return true;
        }
        return false;
    }

    public void AddEnemies(int amount, int zone, Transform t)
    {
        enemiesInRoom[zone] += amount;
        CheckEnemies(zone, t);
    }

    public void AddSuperBoss(int zone)
    {
        superBossesInRoom[zone] ++;
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
        GameObject s = Instantiate(superBoss, t.position,Quaternion.identity);
        s.GetComponent<SuperBossAI>().SetZone(int.Parse(t.GetComponent<EnemyAI>().zone.name));
        Destroy(t.gameObject);
        AddEnemies(-extraEnemiesBeforeSuperBoss, zone, t);
    }

}
