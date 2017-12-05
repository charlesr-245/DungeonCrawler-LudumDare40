using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float speed;

    private Rigidbody rb;
    private BasicStats stats;
    private AnimationManager anim;
    public GameObject Sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<AnimationManager>();
        stats = GetComponent<BasicStats>();
    }

	void Movement()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 eulerAngles = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;
        eulerAngles.z -= 90;
        Sprite.transform.eulerAngles = eulerAngles;
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
        for (int x =0; x < 20; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }



}
