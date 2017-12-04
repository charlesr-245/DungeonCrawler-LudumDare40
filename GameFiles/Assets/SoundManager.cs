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
        if (pastPlayerZone != playerZone && playerZone != -1)
        {
            OnZoneChange();
        }
        if (music.isPlaying == false && (music.clip == standardAudio[0] || music.clip == superBossAudio[0])) {
            LoopMusic();
        }
    }

    private void OnZoneChange()
    {
        //Debug.Log("true");
        pastPlayerZone = playerZone;
        if (enemy.superBossInRoom(playerZone) && (music.clip == standardAudio[0] || music.clip == standardAudio[1]))
        {
            music.clip = superBossAudio[0];
            music.Play();
        } else if (!enemy.superBossInRoom(playerZone) && (music.clip == superBossAudio[0] || music.clip == superBossAudio[1] || music.clip == superBossAudio[2]))
        {
            music.clip = standardAudio[0];
            music.Play();
        }
    }

    private void LoopMusic()
    {
        music.loop = true;
        if (music.clip == standardAudio[0])
        {
            music.clip = standardAudio[1];

        } else if (music.clip == superBossAudio[0])
        {
            music.clip = superBossAudio[1];
        }
        music.Play();
    }

}
