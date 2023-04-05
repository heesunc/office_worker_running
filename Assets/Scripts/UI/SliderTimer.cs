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
    private float volume = 0.05f;
    private float pitch = 1.0f;
    private bool isTimeOut = false;

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

        TimerSoundPlay("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        if(slTimer.value >0.0f)
        {
            slTimer.value -= Time.deltaTime;
            pitch = 1.0f + (1.0f - slTimer.value/fSliderTime); //효과음 속도 비율
            timerAudioSource.pitch = pitch;

            if (Time.timeScale != 1.0f)
            {
                timerAudioSource.volume = 0.0f;
            }
            else
            {
                timerAudioSource.volume = 0.5f;
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
                timerAudioSource.loop = true;
               
            }
            else if(name == "TimeOut")
            {
                timerAudioSource.pitch = 1.0f;
                timerAudioSource.clip = timerAudioList[1];
                timerAudioSource.loop = false;
                
            }

            timerAudioSource.Play();
        }

   }

    void Delay()
    {
        Debug.Log("delay");
    }
}
