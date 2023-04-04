using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource playerAudioSource;
    public AudioClip[] playerAudioList;
    private float volume = 0.05f;

    /*
     0 = 점프
     1 = 게임 클리어
     2 = 게임 오버
      */

    // Start is called before the first frame update
    void Start()
    {
        SoundPlay("Walk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SoundPlay(string name)
    {
        if (!MuteManager.EffectIsMuted)
        {
            
            if(name == "Jump")
            {
                playerAudioSource.clip = playerAudioList[0];
                volume = 1.0f;
            }
            else if (name == "GameClear")
            {
                    playerAudioSource.clip = playerAudioList[1];
                    volume = 0.4f;
            }
            else if (name == "GameOver")
            {
                    playerAudioSource.clip = playerAudioList[2];
                    volume = 0.7f;

            }

            playerAudioSource.loop = false;
            playerAudioSource.volume = volume;
            playerAudioSource.Play();
        }
    }

    public void SoundStop()
    {
       
        playerAudioSource.Stop();
    }
}
