using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSound : MonoBehaviour
{
    public AudioSource btnAudioSource;
    public AudioClip btnAudioClip;
    private float volume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SoundPlay()
    {
        if (!MuteManager.EffectIsMuted)
        {
            btnAudioSource.clip = btnAudioClip;
            volume = 0.1f;

            btnAudioSource.loop = false;
            btnAudioSource.volume = volume;
            btnAudioSource.Play();
        }
    }
}