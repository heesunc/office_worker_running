using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource playerAudioSource;
    public AudioClip[] playerAudioList;
    private float volume = 0.05f;

    /*
     0 = 걷기
     1 = 점프
     2 = 게임 클리어
     3 = 게임 오버
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
            if (name == "Walk")
            {
                WalkSoundPlay();
            }
            else if(name == "Jump")
            {
                playerAudioSource.clip = playerAudioList[1];
                volume = 1.0f;
                playerAudioSource.loop = false;
                playerAudioSource.volume = volume;
                playerAudioSource.Play();

                Invoke("WalkSoundPlay", 1f);
            }
            else 
            {
                if (name == "GameClear")
                {
                    playerAudioSource.clip = playerAudioList[2];
                    volume = 0.4f;
                }
                else if (name == "GameOver")
                {
                    playerAudioSource.clip = playerAudioList[3];
                    volume = 0.7f;

                }
                playerAudioSource.loop = false;
                playerAudioSource.volume = volume;
                playerAudioSource.Play();
            }
           

        }
    }

    public void WalkSoundPlay()
    {
        playerAudioSource.clip = playerAudioList[0];
        volume = 0.4f;
        playerAudioSource.loop = true;
        playerAudioSource.volume = volume;
        playerAudioSource.Play();
    }

    public void SoundStop()
    {
       
        playerAudioSource.Stop();
    }
}
