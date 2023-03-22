using UnityEngine;
using UnityEngine.UI;

public class Revive : MonoBehaviour
{
    public GameObject GameOver_UI;
    GameManager manager;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ReviveBtn()
    {
        // Pause를 누르고 "계속 진행" 버튼을 누르면 3초 후 게임 계속할 수 있도록
        GameOver_UI.SetActive(false);
        manager.isOver=false;
        Time.timeScale = 1f;
    }
}