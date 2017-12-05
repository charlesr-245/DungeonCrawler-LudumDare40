using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public int distance;
    public GameObject Projectile;
    public float speed;
    private BasicStats stats;
    public GameObject position;
    public GameObject Sprite;

     void Start(){
        stats = GetComponent<BasicStats>();
    }
     void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            RangedAttack();
        }
    }
    void RangedAttack() {

        //Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        //position = Camera.main.ScreenToWorldPoint(position);
        GameObject laser = Instantiate(Projectile, position.transform.position, Quaternion.identity, null);
        laser.transform.eulerAngles = Sprite.transform.eulerAngles;
        //go.transform.LookAt(position);
        //Debug.Log(position);
        laser.GetComponent<Rigidbody>().AddForce(laser.transform.up * speed, ForceMode.Impulse);
        
    }
}


