using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public int damage;
    public int distance= 20;
    public GameObject Projectile;
    public float projectileSpeed;
    public float FiringRate=0.2f;
     void Start(){

    }
     void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("RangedAttack",0.00000001f, FiringRate);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("RangedAttack");
        }
    }
    void RangedAttack() {

        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        position = Camera.main.ScreenToWorldPoint(position);
        GameObject go = Instantiate(Projectile, transform.position, Quaternion.identity) as GameObject;
        go.transform.LookAt(position);
        Debug.Log(position);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * 1000);


    }
   
    }


