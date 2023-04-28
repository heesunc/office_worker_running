using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    private static AudioPlay _instance; // 사운드를 한 곳에서 관리할 수 있도록
    public AudioSource audioSource;
    public AudioClip[] audioList;
    private bool isMuted = false;
    private bool effectIsMuted = false;
    private float pitch = 1.0f;
    public float playTime;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // scene이 전환되어도 object가 없어지지 않도록
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        if(SceneManager.GetActiveScene().name == "Stage")
        {
            playTime += Time.deltaTime;
            if(playTime > 30.0f)
            {
                playTime = 0.0f;
                pitch = pitch + (pitch * 0.05f);
                audioSource.pitch = pitch;
            }
            Debug.Log("playTime : " + playTime);
        }
    }

    // scene이 로딩됐을때 해당 scene 이름과 같은 이름의 bgm 재생
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < audioList.Length; i++)
        {
            if (arg0.name == audioList[i].name)
            {
                AudioSoundPlay(audioList[i]);
                pitch = 1.0f;
            }
        }
        audioSource.mute = MuteManager.IsMuted;
    }

    // 음악 플레이
    public void AudioSoundPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.15f;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        if (isMuted)
        {
            Debug.Log("Mute is turned on");
        }
        else
        {
            Debug.Log("Mute turned off");
        }
        MuteManager.IsMuted = isMuted;
    }
    public void EffectToggleMute()
    {
        effectIsMuted = !effectIsMuted;
        audioSource.mute = isMuted;
        if (effectIsMuted)
        {
            Debug.Log("EMute is turned on");
        }
        else
        {
            Debug.Log("EMute turned off");
        }
        MuteManager.EffectIsMuted = effectIsMuted;
    }
}