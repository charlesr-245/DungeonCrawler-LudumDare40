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

    private BasicStats stats;
    private AnimationManager animationManager;
    private bool stunned;

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
        if (!stunned)
        {
            MoveTowardsPlayer();
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
        if (stats.GetHP() <= 0)
        {
            Die();
        }
        else
        {
            animationManager.AddToQueue("Hit");
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
