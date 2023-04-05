using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderTimer : MonoBehaviour
{
    GameManager manager;

    Slider slTimer;
    public float fSliderTime= 1000.0f;

    public AudioSource timerAudioSource;
    public AudioClip[] timerAudioList;
    private float volume = 0.5f;
    private bool isTimeOut = false;
    private bool activeSound = false;
    /* 
        0 = timer
        1= timeOver
    */

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slTimer = GetComponent<Slider>();
        slTimer.maxValue = fSliderTime;
        slTimer.value = fSliderTime;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(slTimer.value >0.0f)
        {
            slTimer.value -= Time.deltaTime;

            if(slTimer.value < 8.0f)
            {
                if(activeSound == false)
                {
                    TimerSoundPlay("Timer");
                    activeSound = true;
                }
            }

            if (Time.timeScale != 1.0f)
            {
                
                    timerAudioSource.Pause();
            }
            else
            {
                if (!timerAudioSource.isPlaying)
                    timerAudioSource.Play();
            }
            

        }
        else
        {
            if(isTimeOut == false)
            {
                isTimeOut = true;
                TimerSoundPlay("TimeOut");
            } 
            manager.GameOver();
        }


    }

   public void TimerSoundPlay(string name)
   {
        if (!MuteManager.EffectIsMuted)
        {
            if(name == "Timer")
            {
                timerAudioSource.clip = timerAudioList[0];
  
            }
            else if(name == "TimeOut")
            {
                timerAudioSource.clip = timerAudioList[1];
             
            }

            timerAudioSource.loop = false;
            timerAudioSource.volume = volume;
            timerAudioSource.Play();
        }

   }

    void Delay()
    {
        Debug.Log("delay");
    }
}
