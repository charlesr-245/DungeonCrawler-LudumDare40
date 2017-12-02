  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Movement()
    {
        Vector3 direction;
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * speed;
        rb.velocity = direction;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }
    
	void FixedUpdate () {
        Movement();
    }
}
