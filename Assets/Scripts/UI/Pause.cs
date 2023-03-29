using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject SecUI;

    GameManager manager;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        transform.Translate(Time.deltaTime, 0, 0);
    }
    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.001f; // 시간이 느리게 흘러가도록
            PauseUI.SetActive(true);
            Sec secScript = SecUI.GetComponent<Sec>();

            GameObject.Find("PlayerSoundSource").GetComponent<PlayerSound>().SoundStop();
        }
    }
}
