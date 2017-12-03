using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public int pastPlayerZone = -1;
    public int playerZone;

    public AudioClip[] superBossAudio;
    public AudioClip[] standardAudio;
    public AudioClip upgradeAudio;

    private bool playingStandard = true;

    public AudioSource music;
    public AudioSource effects;

    private EnemyManagement enemy;

    void Start()
    {
        enemy = GameObject.Find("EnemyManager").GetComponent<EnemyManagement>();
    }


    private void Update()
    {
        if (pastPlayerZone != playerZone)
        {
            OnZoneChange();
        }
    }

    private void OnZoneChange()
    {
        pastPlayerZone = playerZone;
        
    }

}
