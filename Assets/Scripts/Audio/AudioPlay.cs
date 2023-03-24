using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlay : MonoBehaviour
{
    private static AudioPlay _instance; // 사운드를 한 곳에서 관리할 수 있도록
    public AudioSource audioSource;
    public AudioClip[] audioList;
    private bool isMuted = false;

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

    // scene이 로딩됐을때 해당 scene 이름과 같은 이름의 bgm 재생
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < audioList.Length; i++)
        {
            if (arg0.name == audioList[i].name)
            {
                AudioSoundPlay(audioList[i]);
            }
        }
        audioSource.mute = MuteManager.IsMuted;
    }

    // 음악 플레이
    public void AudioSoundPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
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
}