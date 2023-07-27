using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    const int normal = 10, hard = 20, chaos = 30;
    private static AudioPlay _instance; // 사운드를 한 곳에서 관리할 수 있도록
    public AudioSource audioSource;
    public AudioClip[] audioList;
    private bool isMuted = false;
    private bool effectIsMuted = false;
    private float pitch = 1.0f;
    public float playTime;
    private int count = 0;
    public int stageIndex;
    

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
        //if(SceneManager.GetActiveScene().name == "Stage")
        //{
        //    playTime += Time.deltaTime;
        //    if(playTime > 30.0f && count < 3 && stageIndex <= hard) //최대 세 번만 배속, 하드 스테이지 이하만 배속
        //    {
        //        playTime = 0.0f;
        //        pitch = pitch + (pitch * 0.08f);
        //        audioSource.pitch = pitch;
        //        count++; //배속 카운터
        //    }
        //    Debug.Log("playTime : " + playTime);
        //}
    }

    // scene이 로딩됐을때 해당 scene 이름과 같은 이름의 bgm 재생
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        stageIndex = GameObject.FindObjectOfType<LoadGame>().Index;
        if(arg0.name == "StartScene")
        {
            AudioSoundPlay(audioList[0]);
            
        }
        else if(arg0.name == "Stage")
        {
            playTime = 0.0f;
            count = 0;
            pitch = 1.0f; //Scene 변경 시 count 및 배속 초기화

           if (stageIndex <= normal)
                AudioSoundPlay(audioList[1]);
           else if (stageIndex <= hard)
                AudioSoundPlay(audioList[2]);
           else if (stageIndex <= chaos)
                AudioSoundPlay(audioList[3]);
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