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
    [Range(500,100000)]
    public int maxTimeToLive; //Max number of frames for which the rush enemy can live

    private int framesSinceLastMovement; //Records the number of FixedUdpate() frames since the last choice was made by the AI
    private static GameObject player; //References the player
    private Rigidbody rb; //References the local rigidbody
    private GameObject[] spawnedRushEnemies; //Keeps track of the spawned in rush enemies by the single enemy
    private bool spawningRushEnemies; //Stops new movements from occuring while true

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        rb = GetComponent<Rigidbody>();
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
        int choice = Random.Range(0, 50);
        if (choice <= 40)
        {
            MoveTowardsPlayer();
        }
        else if (choice <= 49 && spawnedRushEnemies == null)
        {
            SyncronizedAssault();
        }
        else
        {
            //CallInEnemies();
            MoveTowardsPlayer();
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

}
