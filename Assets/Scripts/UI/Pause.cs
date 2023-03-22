using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI;
    private Text PauseSec;
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
            PauseSec = PauseUI.GetComponentInChildren<Text>();
            PauseSec.enabled = false;
        }
    }
}
