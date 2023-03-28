using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSoundManager : MonoBehaviour
{
    public AudioSource effectAudioSource;
    public AudioClip[] effectAudioList;
    private float volume = 0.05f;
    /*
     0 = 돈
     1 = 돈 규칙
     2 = 서류더미
     3 = 메일
     4 = 포스트잇
     5 = 커피
     6 = 스테이지 클리어
     7 = 스테이지 실패
      */
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundPlay(string name)
    {
        if (!MuteManager.EffectIsMuted)
        {
            if(name == "Money")
            {
                effectAudioSource.clip = effectAudioList[0];
                volume = 1.0f;
            }
            else if (name == "RuleMoney")
            {
                effectAudioSource.clip = effectAudioList[1];
                volume = 0.7f; ;
            }  
            else if (name == "Bomb")
            {
                effectAudioSource.clip = effectAudioList[2];
            }   
            else if (name == "Mail")
            {
                effectAudioSource.clip = effectAudioList[3];
            }  
            else if (name == "PostIt")
            {
                effectAudioSource.clip = effectAudioList[4];
                volume = 0.7f;
            }
            else if (name == "Coffee")
            {
                effectAudioSource.clip = effectAudioList[5];
            }
            else if (name == "GameClear")
            {
                effectAudioSource.clip = effectAudioList[6];
                volume = 0.2f;
            }
            else if (name == "GameOver")
            {
                effectAudioSource.clip = effectAudioList[7];
                
            }


            effectAudioSource.loop = false;
            effectAudioSource.volume = volume;
            effectAudioSource.Play();


        }

    }
    

}
