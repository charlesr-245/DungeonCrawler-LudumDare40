using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTime : MonoBehaviour {

    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(TimeCountdown());
    }

    private IEnumerator TimeCountdown()
    {
        for (int x =0; x<450; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("BasicEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("HitEnemyBasic"))
        {
            Debug.Log("BASIC");
            collision.gameObject.transform.parent.GetComponent<EnemyAI>().Hit(player.GetComponent<BasicStats>().GetAttack());
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("RushEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("HitEnemyRush"))
        {
            Debug.Log("Rush");
            
            collision.GetComponent<EnemyParent>().parent.GetComponent<RushAI>().Hit(player.GetComponent<BasicStats>().GetAttack());
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SuperBoss"))
        {
            Debug.Log("SUPER");
            collision.gameObject.transform.parent.transform.parent.GetComponent<SuperBossAI>().Hit(player.GetComponent<BasicStats>().GetAttack());
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }

}
