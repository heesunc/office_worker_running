using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSound : MonoBehaviour
{
    public AudioSource itemAudioSource;
    public AudioClip[] itemAudioList;
    private float volume = 0.05f;
    /*
     0 = ��
     1 = �� ��Ģ
     2 = ��������
     3 = ����
     4 = ����Ʈ��
     5 = Ŀ��
      */
    private void Awake()
    {
       
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
                itemAudioSource.clip = itemAudioList[0];
                volume = 1.0f;
            }
            else if (name == "RuleMoney")
            {
                itemAudioSource.clip = itemAudioList[1];
                volume = 0.7f; ;
            }  
            else if (name == "Bomb")
            {
                itemAudioSource.clip = itemAudioList[2];
                volume = 0.7f;
            }   
            else if (name == "Mail")
            {
                itemAudioSource.clip = itemAudioList[3];
                volume = 0.7f;
            }  
            else if (name == "PostIt")
            {
                itemAudioSource.clip = itemAudioList[4];
                volume = 0.7f;
            }
            else if (name == "Coffee")
            {
                itemAudioSource.clip = itemAudioList[5];
                volume = 0.3f;
            }

            itemAudioSource.loop = false;
            itemAudioSource.volume = volume;
            itemAudioSource.Play();


        }

    }
    

}