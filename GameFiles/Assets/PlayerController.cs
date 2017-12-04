 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float speed;

    private Rigidbody rb;
    private BasicStats stats;
    private AnimationManager anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<AnimationManager>();
        stats = GetComponent<BasicStats>();
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

    public void Hit(float amount)
    {
        stats.DecreaseHP(amount);
        if (stats.GetHP() <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.AddToQueue("Dead");
        StartCoroutine(CountdownToDeath());
    }
     
    IEnumerator CountdownToDeath()
    {
        for (int x =0; x < 50; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }



}
