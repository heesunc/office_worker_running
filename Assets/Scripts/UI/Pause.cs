using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject SecUI;
    public bool isPause;

    GameManager manager;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnTogglePauseButton();
        }
    }
    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 1 && !manager.isOver && !manager.isClear)
        {
            isPause = true;
            Time.timeScale = 0.001f; // 시간이 느리게 흘러가도록
            PauseUI.SetActive(true);
            Sec secScript = SecUI.GetComponent<Sec>();

            GameObject.Find("PlayerSoundSource").GetComponent<PlayerSound>().SoundStop();
        }
    }
}
