using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : MonoBehaviour {

    public float speed;

    private static GameObject player; //Reference to Player
    private Rigidbody rb; //Reference to local RigidBody

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        rb = GetComponent<Rigidbody>();
        MoveTowardsPlayer();
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction.z = 0;
        rb.velocity = direction * speed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        Debug.Log("Rush Speed: " + rb.velocity);
    }

}
