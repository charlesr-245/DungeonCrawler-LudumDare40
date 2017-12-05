using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncremetor : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag==("Player") )
            {
            other.gameObject.GetComponent<BasicStats>().DecreaseHP(-4);
            other.gameObject.GetComponent<SoundManager>().UpgradeAudio();
            Destroy(gameObject);
        }
        
    }
}
