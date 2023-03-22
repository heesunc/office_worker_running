using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject[] musics;
    void Awake() // 오브젝트의 유일성 보장, 배경음악 한 번만 틀어줌, 씬이동시 파괴X
    {
        musics = GameObject.FindGameObjectsWithTag("Music");

        if(musics.Length >=2)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if(audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
