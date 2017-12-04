using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public int damage;
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
    

        
        GameObject beam = Instantiate(Projectile,transform.position,Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody>().velocity = new Vector3(0, projectileSpeed);

       
    }
   
    }


