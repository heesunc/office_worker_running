using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject SecUI;
    
    public void ExitBtn()
    {
        // Pause를 누르고 "나가기" 버튼을 누르면 시작화면으로 이동
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1f;
    }

    public void ProgressBtn()
    {
        PauseUI.SetActive(false); // PauseUI 끔
        SecUI.SetActive(true);
        Sec secScript = SecUI.GetComponent<Sec>();
        secScript.StartSecond(); // 3초 세는 코루틴 함수 실행
    }
}
