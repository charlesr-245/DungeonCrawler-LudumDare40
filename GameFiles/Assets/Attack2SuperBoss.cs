using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2SuperBoss : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            transform.parent.GetComponent<SuperBossAI>().attack2 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            transform.parent.GetComponent<SuperBossAI>().attack2 = false;
        }
    }
}
