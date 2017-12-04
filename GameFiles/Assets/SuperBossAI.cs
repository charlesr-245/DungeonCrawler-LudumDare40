using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBossAI : MonoBehaviour {

    public float speed;
    private int frames;

    public int framesBeforeAction;
    public GameObject BasicEnemy;
    private List<Transform> enemySpawns;
    private static GameObject player;
    private int zone =-1;
    private bool attackRange;
    public int stunnedFrames;
    private bool stunned = false;
    public bool attack2;
    private bool added = false;
    private Rigidbody rb;
    private static EnemyManagement enemy;
    private AnimationManager anim;
    private BasicStats stats;
    private void Start()
    {
        anim = GetComponent<AnimationManager>();
        stats = GetComponent<BasicStats>();
        rb = GetComponent<Rigidbody>();
        if (enemy == null)
        {
            enemy = GameObject.Find("EnemyManager").GetComponent<EnemyManagement>();
        }
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
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

    public void SetZone(int _zone)
    {
        zone = _zone;
    }

    private void Update()
    {
        if (zone != -1 &&!added)
        {
            added = true;
            enemy.AddSuperBoss(zone);
        }
        if (frames >= framesBeforeAction)
        {
            frames = 0;
            AIChoice();
        }
    }

    private void FixedUpdate()
    {
        //DealDamage(5);
        frames++;
    }

    public void StartParticleSystem()
    {
        GetComponent<ParticleSystem>().Play();
    }

    IEnumerator Defeated()
    {
        for (int x =0; x < 60; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        enemy.DestroySuperBoss(zone);
        Destroy(gameObject);

    }

    public void Hit(int amount, bool isStunned = false)
    {
        stunned = true;
        rb.velocity = -rb.velocity;
        stats.DecreaseHP(amount);
        if (!isStunned)
        {
            if (stats.GetHP() <= 0)
            {
                anim.AddToQueue("Dead");
                StartCoroutine(Defeated());
            }
            else
            {
                anim.AddToQueue("Hit");
            }
        } else
        {
            anim.AddToQueue("Stunned");
        }
    }

    private void AIChoice()
    {
        if (zone == int.Parse(player.GetComponent<ZoneRelay>().GetZone().name))
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
        CameraShake2();
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

    private void CameraShake2()
    {
        Camera.main.GetComponent<Animator>().Play("CameraShake2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            attackRange = true;
        }
        if (zone == -1 && other.gameObject.transform.parent.name == "Level")
        {
            zone = int.Parse(other.gameObject.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (zone == -1 && other.gameObject.transform.parent.name == "Level")
        {
            zone = int.Parse(other.gameObject.name);
        }
        if (zone == -1)
        {
            Debug.Log("true2");
            Debug.Log(other.gameObject.transform.parent.name);
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
