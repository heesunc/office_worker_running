using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseUI;

    public void ExitBtn()
    {
        // Pause를 누르고 "나가기" 버튼을 누르면 시작화면으로 이동
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1f;
    }

    public void ProgressBtn()
    {
        // Pause를 누르고 "계속 진행" 버튼을 누르면 3초 후 게임 계속할 수 있도록
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
