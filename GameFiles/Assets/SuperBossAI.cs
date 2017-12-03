using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBossAI : MonoBehaviour {

    public float speed;

    public GameObject BasicEnemy;
    private List<Transform> enemySpawns;
    private GameObject player;
    private GameObject zone;
    private bool attackRange;
    public bool attack2;
    private Rigidbody rb;
    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        enemySpawns = new List<Transform>();
        foreach (Transform t in transform)
        {
            if (t.tag == "CallInSpawnPoint")
            {
                enemySpawns.Add(t);
            }
        }
    }

    private void AIChoice()
    {
        if (zone == player.GetComponent<ZoneRelay>().GetZone())
        {
            rb.velocity = Vector3.zero;
            if (attackRange)
            {
                Attack();
            } else if (attack2)
            {
                Attack2();
            } else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Attack()
    {
        CameraShake();
    }

    private void Attack2()
    {
        CameraShake();
    }

    private void MoveTowardsPlayer()
    {
        CameraShake();
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction.z = 0;
        rb.velocity = direction * speed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }

    private void CameraShake()
    {
        Camera.main.GetComponent<Animator>().Play("CameraShake");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            attackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            attackRange = false;
        }
    }

    private void SpawnEnemies()
    {
        for (int x =0; x<enemySpawns.Count; x++)
        {
            GameObject enemy = Instantiate(BasicEnemy,enemySpawns[x].position,Quaternion.identity);
            //enemy.GetComponent<EnemyAI>().canMerge = false;
            enemy.GetComponent<EnemyAI>().spawnedEnemies = true;
        }
    }

}
