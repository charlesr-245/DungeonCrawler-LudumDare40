using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().speed+=2;
            other.gameObject.GetComponent<SoundManager>().UpgradeAudio();
                Destroy(gameObject);

        }

    }
}
