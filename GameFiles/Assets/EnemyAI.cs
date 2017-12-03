using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [Header("Enemy Movement")]
    public int framesUntilMovement; //Specifies the number of frames needed before allowing a new movement
    public float speed; //This is constant, velocity is set directly, no incremental force
    [Range(0, 100)] //After a set number of frames, the enemy will have a chance to perform a new action
    public int moveChance;

    [Header("Rush Enemeies")]
    public GameObject RushEnemy; //Spawns during the syncronized attack
    public int maxRushEnemies; //Max rush enemies a single enemy can spawn
    [Range(500,10000)]
    public int maxTimeToLive; //Max number of frames for which the rush enemy can live

    [Header("Call In Enemies")]
    public GameObject BasicEnemy;
    public bool spawnedEnemies; //Prevents the enemy from spawning in more than one set of enemies.

    private int framesSinceLastMovement; //Records the number of FixedUdpate() frames since the last choice was made by the AI
    private static GameObject player; //References the player
    private Rigidbody rb; //References the local rigidbody
    private GameObject[] spawnedRushEnemies; //Keeps track of the spawned in rush enemies by the single enemy
    private bool spawningRushEnemies; //Stops new movements from occuring while true
    private List<Transform> callInSpawns; //Spawn points for when the enemy calls in more enemies.
    public BoxCollider zone; //Zone of allowed movement;
    private AnimationManager animationManager;
    private BasicStats stats;
    private EnemyManagement eManager;
    public BoxCollider HitCollider;
    public BoxCollider NormalCollider;
    public bool canMerge = true;

    private void Start()
    {
        eManager = GameObject.Find("EnemyManager").GetComponent<EnemyManagement>();
        animationManager = GetComponent<AnimationManager>();
        stats = GetComponent<BasicStats>();
        framesSinceLastMovement = 10000; //Makes sure the enemy will be able to make a movement choice on the first frame.
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        rb = GetComponent<Rigidbody>();
        //Gets Spawn Points
        callInSpawns = new List<Transform>();
        foreach (Transform t in transform)
        {
            if (t.tag == "CallInSpawnPoint")
            {
                callInSpawns.Add(t);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zone == null && other.gameObject.transform.parent.name == "Level")
        {
            zone = other.gameObject.GetComponent<BoxCollider>();
        }
    }

    private void Update()
    {
        AICheck();
    }

    private void FixedUpdate()
    {
        if (!spawningRushEnemies) {
            framesSinceLastMovement++;
        }
    }

    private void AICheck()
    {
        if (framesSinceLastMovement >= framesUntilMovement)
        {
            int choice = Random.Range(0, 100);
            if (choice <= moveChance) //Checks to see if the randomly generated number is within the specified range.
            {
                framesSinceLastMovement = 0; //Resets the framesSinceLastMovement counter
                AIChoice(); //The AI will now make a decision and perform it
            }
        }
    }

    private void AIChoice()
    {
        if (zone == player.GetComponent<ZoneRelay>().GetZone())
        {
            int choice = Random.Range(0, 50);
            if (choice <= 43)
            {
                MoveTowardsPlayer();
            }
            else if (choice <= 47 && spawnedRushEnemies == null)
            {
                SyncronizedAssault();
            }
            else
            {
                if (!spawnedEnemies)
                {
                    CallInEnemies();
                }

                MoveTowardsPlayer();
            }
        } else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void MoveTowardsPlayer() //Moves Enemy towards the player 
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction.z = 0;
        rb.velocity = direction * speed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }

    private void SyncronizedAssault() //Spawns "Rush Enemies" that die upon impact or a certain number of frames. Rush the player at an increased speed
    {
        rb.velocity = Vector3.zero;
        int timeToLive = Random.Range(500, maxTimeToLive);
        StartCoroutine(SpawnRushEnemies(timeToLive));
        StartCoroutine(CheckRushLife(timeToLive));
    }

    private void CallInEnemies()
    {
        eManager.AddEnemies(callInSpawns.Count,int.Parse(zone.name),transform);
        spawnedEnemies = true;
        for (int x=0; x<callInSpawns.Count; x++)
        {
            GameObject enemy = Instantiate(gameObject, callInSpawns[x].position, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().spawnedEnemies = true;
        }
    }

    IEnumerator SpawnRushEnemies(int timeToLive)
    {
        spawningRushEnemies = true;
        int choice = Random.Range(1, maxRushEnemies);
        spawnedRushEnemies = new GameObject[choice];
        for (int x = 0; x < choice; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                yield return new WaitForFixedUpdate();
            }

            spawnedRushEnemies[x] = Instantiate(RushEnemy, new Vector3(transform.position.x+2*x,transform.position.y,0), Quaternion.identity, null);
        }
        spawningRushEnemies = false;
    }

    IEnumerator CheckRushLife(int timeToLive)
    {
        for (int x = 0; x < timeToLive;x++)
        {
            yield return new WaitForFixedUpdate();
        }
        foreach (GameObject r in spawnedRushEnemies)
        {
            Destroy(r); //Kills the rush enemies once the time limit is exceeded
        }
        spawnedRushEnemies = null; //Resets variable space
    }

    public void Hit(BasicStats playerStats)
    {
        float damage = playerStats.GetAttack() - stats.GetDefense() / 4;
        Debug.Log(damage);
        stats.DecreaseHP(damage);
        Debug.Log(stats.GetHP());
        rb.velocity = -rb.velocity;
        HitCollider.isTrigger = false;
        NormalCollider.isTrigger = true;
        StartCoroutine(ResumeCollider());
        framesSinceLastMovement = -100;
        if (stats.GetHP() <= 0)
        {
            Die();
        } else
        {
            animationManager.AddToQueue("Hit");
        }
    }

    private IEnumerator ResumeCollider()
    {
        for (int x = 0; x < 80; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        HitCollider.isTrigger = true;
        NormalCollider.isTrigger = false;
    }

    private void Die()
    {
        animationManager.AddToQueue("Dead");
        StartCoroutine(LifeCountdown());
    }

    private IEnumerator LifeCountdown()
    {
        for (int x =0; x < 50; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

}
