using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    void MeleeAttack()
    {
        //float distance = Vector3.Distance(TargetJoint2D.transform.position, transform.position);

        //if (distance < 1f)
        //{
            //EnemyHealth EHP=
        //}
    }
	void FixedUpdate () {
        if (Input.GetButton("Fire1"))
        {
            MeleeAttack();
        }
	}
}
