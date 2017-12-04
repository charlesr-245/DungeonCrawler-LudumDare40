using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : MonoBehaviour {

    public float speed;

    public BoxCollider NormalCollider;
    public BoxCollider HitCollider;

    public GameObject MainSprite;

    private static GameObject player; //Reference to Player
    private Rigidbody rb; //Reference to local RigidBody

    public int framesBeforeRestore;

    private BasicStats stats;
    private AnimationManager animationManager;
    private bool stunned;
    private int frames;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" && NormalCollider.isTrigger == false)
        {
            collision.gameObject.GetComponent<BasicStats>().DecreaseHP(stats.GetAttack());
            Hit(0, true);
        }
    }

    private void Start()
    {
        stats = GetComponent<BasicStats>();
        animationManager = GetComponent<AnimationManager>();
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        rb = GetComponent<Rigidbody>();
        MoveTowardsPlayer();
        //Hit(stats);
    }

    private void Update()
    {
        if (framesBeforeRestore <= frames)
        {
            stunned = false;
            frames = 0;
        }
        if (!stunned)
        {
            MoveTowardsPlayer();
        }
    }

    void FixedUpdate()
    {
        if (stunned)
        {
            frames++;
        }

    }

    private void MoveTowardsPlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        MainSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction.z = 0;
        rb.velocity = direction * speed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        //Debug.Log("Rush Speed: " + rb.velocity);
    }

    public void Hit(int amount, bool isStunned = false)
    {
        stunned = true;
        rb.velocity = -rb.velocity;
        HitCollider.isTrigger = false;
        NormalCollider.isTrigger = true;
        StartCoroutine(ResumeCollider());
        if (!isStunned)
        {
            stats.DecreaseHP(amount);
            if (stats.GetHP() <= 0)
            {
                Die();
            }
            else
            {
                animationManager.AddToQueue("Hit");
            }
        }
        else
        {
            animationManager.AddToQueue("Stunned");
        }
    }


    private IEnumerator ResumeCollider()
    {
        stunned = true;
        for (int x = 0; x < 80; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        HitCollider.isTrigger = true;
        NormalCollider.isTrigger = false;
        stunned = false;
    }

    private void Die()
    {
        animationManager.AddToQueue("Dead");
        StartCoroutine(LifeCountdown());
    }

    private IEnumerator LifeCountdown()
    {
        for (int x = 0; x < 50; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }


}
